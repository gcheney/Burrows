// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace Burrows.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;

    /// <summary>
	/// The envelope in use for storing meta-data/out-of-band data and message object data.
	/// </summary>
    public class Envelope
    {
        Envelope(object message, IEnumerable<Type> messageTypes)
        {
            Headers = new Dictionary<string, string>();
            MessageType = new List<string>(messageTypes.Select(type => new MessageUrn(type).ToString()));
            Message = message;
        }

        protected Envelope()
        {
            Headers = new Dictionary<string, string>();
            MessageType = new List<string>();
        }

        public string RequestId { get; set; }
        public string ConversationId { get; set; }
        public string CorrelationId { get; set; }
        public string DestinationAddress { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public string FaultAddress { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public object Message { get; set; }
        public string MessageId { get; set; }
        public IList<string> MessageType { get; set; }
        public string Network { get; set; }
        public string ResponseAddress { get; set; }
        public int RetryCount { get; set; }
        public string SourceAddress { get; set; }

		/// <summary>
		/// Creates a new envelope using the passed send context.
		/// </summary>
		/// <typeparam name="T">The type of message</typeparam>
		/// <param name="context">Context to write to the envelope</param>
		/// <returns>The constructed envelope</returns>
        public static Envelope Create<T>(ISendContext<T> context)
            where T : class
        {
            var envelope = new Envelope(context.Message, context.Message.GetType().GetMessageTypes());
            envelope.SetUsingContext(context);

            return envelope;
        }
    }
}