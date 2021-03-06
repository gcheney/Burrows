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

using System;
using System.Collections.Generic;
using Burrows.Configuration.Builders;
using Burrows.Configuration.BusConfigurators;
using Burrows.Configuration.Configurators;
using Magnum.Extensions;

namespace Burrows.Configuration.BusServiceConfigurators
{
    public class DefaultBusServiceConfigurator<TService> :
		IBusServiceConfigurator,
		IBusBuilderConfigurator
		where TService : IBusService
	{
		readonly Func<IServiceBus, TService> _serviceFactory;
	    readonly IBusServiceLayer _layer;

		public DefaultBusServiceConfigurator(IBusServiceLayer layer, Func<IServiceBus, TService> serviceFactory)
		{
			_serviceFactory = serviceFactory;
			_layer = layer;
		}

		public IEnumerable<IValidationResult> Validate()
		{
			if (_serviceFactory == null)
				yield return this.Failure("BusServiceFactory", "The bus service factory for '{0}' was null."
					.FormatWith(typeof (TService).Name));
		}

		public IBusBuilder Configure(IBusBuilder builder)
		{
			builder.AddBusServiceConfigurator(this);

			return builder;
		}

		public Type ServiceType
		{
			get { return typeof (TService); }
		}

		public IBusServiceLayer Layer
		{
			get { return _layer; }
		}

		public IBusService Create(IServiceBus bus)
		{
			return _serviceFactory(bus);
		}
	}
}