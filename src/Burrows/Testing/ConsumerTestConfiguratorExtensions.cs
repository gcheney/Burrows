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

using Burrows.Configuration.Configuration;

namespace Burrows.Testing
{
    using System;
    using Configuration;
    using Scenarios;
    using TestInstanceConfigurators;

    public static class ConsumerTestConfiguratorExtensions
	{
		public static void ConstructUsing<TScenario, TConsumer>(
			this IConsumerTestInstanceConfigurator<TScenario, TConsumer> configurator,
			Func<TConsumer> consumer)
			where TConsumer : class
			where TScenario : ITestScenario
		{
			var consumerFactory = new DelegateConsumerFactory<TConsumer>(consumer);

			configurator.UseConsumerFactory(consumerFactory);
		}
	}
}