﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, Eric Swann, et. al.
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

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Burrows.Publishing.BackingStores
{
    public interface IUnconfirmedMessageRepository
    {
        IList<ConfirmableMessage> GetAndDeleteMessages(string publisherId, int pageSize);

        void StoreMessages(ConcurrentQueue<ConfirmableMessage> messages, string publisherId);
    }
}