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
using System.Collections.Generic;

namespace Burrows.Endpoints
{
    public interface IEndpointManagement :
        IDisposable
    {
        void BindQueue(string queueName, string exchangeName, string exchangeType, string routingKey = "",
            IDictionary<string, object> queueArguments = null);

        void UnbindQueue(string queueName, string exchangeName, string routingKey = "");
        void BindExchange(string destination, string source, string exchangeType, string routingKey = "");
        void UnbindExchange(string destination, string source, string routingKey);

        void Purge(string queueName);
    }
}