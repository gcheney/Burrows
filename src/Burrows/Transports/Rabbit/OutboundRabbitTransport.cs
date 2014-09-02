﻿// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Burrows.Context;
using Burrows.Endpoints;
using Burrows.Transports.Bindings;
using Burrows.Transports.PublisherConfirm;
using Magnum;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Burrows.Transports.Rabbit
{
    public class OutboundRabbitTransport : IOutboundTransport
    {
        private readonly IRabbitEndpointAddress _address;
        readonly bool _bindToQueue;
        private readonly PublisherConfirmSettings _publisherConfirmSettings;
        private readonly IConnectionHandler<TransportConnection> _connectionHandler;
        ProducerBinding _producer;

        public OutboundRabbitTransport(IRabbitEndpointAddress address,
            PublisherConfirmSettings publisherConfirmSettings,
            IConnectionHandler<TransportConnection> connectionHandler, bool bindToQueue)
        {
            _address = address;
            _publisherConfirmSettings = publisherConfirmSettings;
            _connectionHandler = connectionHandler;
            _bindToQueue = bindToQueue;
        }

        public IEndpointAddress Address
        {
            get { return _address; }
        }

        public void Send(ISendContext context)
        {
            AddProducerBinding();

            _connectionHandler.Use(connection =>
            {
                try
                {
                    IBasicProperties properties = _producer.CreateProperties();

                    properties.SetPersistent(context.DeliveryMode == DeliveryMode.Persistent);
                    properties.MessageId = context.MessageId ?? properties.MessageId ?? NewIds.NewId.Next().ToString();
                    if (context.ExpirationTime.HasValue)
                    {
                        DateTime value = context.ExpirationTime.Value;
                        properties.Expiration =
                            (value.Kind == DateTimeKind.Utc
                                ? value - SystemUtil.UtcNow
                                : value - SystemUtil.Now).
                                TotalMilliseconds.ToString("F0", CultureInfo.InvariantCulture);
                    }

                    using (var body = new MemoryStream())
                    {
                        context.SerializeTo(body);
                        properties.Headers = context.Headers.ToDictionary(entry => entry.Key,
                            entry => (object) entry.Value);
                        properties.Headers["Content-Type"] = context.ContentType;

                        _producer.Publish(_address.Name, properties, body.ToArray());

                        _address.LogSent(context.MessageId ?? properties.MessageId ?? "", context.MessageType);
                    }
                }
                catch (AlreadyClosedException ex)
                {
                    throw new InvalidConnectionException(_address.Uri, "Connection was already closed", ex);
                }
                catch (EndOfStreamException ex)
                {
                    throw new InvalidConnectionException(_address.Uri, "Connection was closed", ex);
                }
                catch (OperationInterruptedException ex)
                {
                    throw new InvalidConnectionException(_address.Uri, "Operation was interrupted", ex);
                }
                catch (IOException ex)
                {
                    if (ex.InnerException is SocketException)
                    {
                        throw new InvalidConnectionException(_address.Uri, "Connection to RabbitMQ host failed", ex);
                    }
                }
            });
        }

        public void Dispose()
        {
            RemoveProducer();
        }

        void AddProducerBinding()
        {
            if (_producer != null)
                return;

            _producer = new ProducerBinding(_address, _bindToQueue, _publisherConfirmSettings);

            _connectionHandler.AddBinding(_producer);
        }

        void RemoveProducer()
        {
            if (_producer != null)
            {
                _connectionHandler.RemoveBinding(_producer);
            }
        }
    }
}