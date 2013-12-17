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
namespace Burrows.Testing
{
    using Scenarios;
    using Subjects;

    public interface ConsumerTest<TConsumer> :
		ITestInstance
		where TConsumer : class
	{
		IConsumerTestSubject<TConsumer> Consumer { get; }
	}

	public interface ConsumerTest<TScenario, TConsumer> :
		ConsumerTest<TConsumer>
		where TConsumer : class
		where TScenario : ITestScenario
	{
		TScenario Scenario { get; }
	}
}