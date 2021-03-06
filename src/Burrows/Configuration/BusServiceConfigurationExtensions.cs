﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
using Burrows.Configuration.BusConfigurators;
using Burrows.Configuration.BusServiceConfigurators;

namespace Burrows.Configuration
{
    public static class BusServiceConfigurationExtensions
	{
        public static IServiceBusConfigurator AddService<TService>(this IServiceBusConfigurator configurator, IBusServiceLayer layer, Func<TService> serviceFactory)
			where TService : IBusService
		{
			var serviceConfigurator = new DefaultBusServiceConfigurator<TService>(layer, bus => serviceFactory());

			configurator.AddBusConfigurator(serviceConfigurator);
            return configurator;
		}

        public static IServiceBusConfigurator AddService<TService>(this IServiceBusConfigurator configurator, IBusServiceLayer layer,
		                                        Func<IServiceBus, TService> serviceFactory)
			where TService : IBusService
		{
			var serviceConfigurator = new DefaultBusServiceConfigurator<TService>(layer, serviceFactory);

			configurator.AddBusConfigurator(serviceConfigurator);
            return configurator;
		}
	}
}