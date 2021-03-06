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
namespace Burrows.Testing.Factories
{
    using System;
    using ScenarioBuilders;
    using Scenarios;
    using TestInstanceConfigurators;

    public interface IHandlerTestFactory<TScenario, TMessage>
        where TMessage : class
        where TScenario : ITestScenario
    {
        HandlerTest<TScenario, TMessage> New(Action<IHandlerTestInstanceConfigurator<TScenario, TMessage>> configureTest);
    }

	public class HandlerTestFactory<TScenario, TMessage> :
		IHandlerTestFactory<TScenario, TMessage>
		where TMessage : class
		where TScenario : ITestScenario
	{
		readonly Func<IScenarioBuilder<TScenario>> _scenarioBuilderFactory;

		public HandlerTestFactory(Func<IScenarioBuilder<TScenario>> scenarioBuilderFactory)
		{
			_scenarioBuilderFactory = scenarioBuilderFactory;
		}


		public HandlerTest<TScenario, TMessage> New(Action<IHandlerTestInstanceConfigurator<TScenario, TMessage>> configureTest)
		{
			var configurator = new HandlerTestInstanceConfigurator<TScenario, TMessage>(_scenarioBuilderFactory);

			configureTest(configurator);

			return configurator.Build();
		}
	}
}