/**
 * Copyright 2015 Marc Rufer, d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace biz.dfch.CS.StateMachine.Tests
{
    [TestClass]
    public class StateMachineTest
    {
        private const String STATE_CREATED = "Created";
        private const String STATE_ERROR = "InternalErrorState";
        private const String STATE_RUNNING = "Running";
        private const String STATE_COMPLETED = "Completed";
        private const String STATE_CANCELLED = "Cancelled";
        private const String STATE_FINAL = "Disposed";

        private const String STATE_PENDING = "Pending";
        private const String STATE_STOPPED = "Stopped";

        private const String CONDITION_CONTINUE = "Continue";
        private const String CONDITION_CANCEL = "Cancel";

        private const String CONDITION_DEPLOY = "Deploy";
        private const String CONDITION_RUN = "Run";

        private const String DEFAULT_STATE_MACHINE_STRING_REPRESENTATION = "{\"Created-Continue\":\"Running\",\"Created-Cancel\":\"InternalErrorState\",\"Running-Continue\":\"Completed\",\"Running-Cancel\":\"Cancelled\",\"Completed-Continue\":\"Disposed\",\"Completed-Cancel\":\"InternalErrorState\",\"Cancelled-Continue\":\"Disposed\",\"Cancelled-Cancel\":\"InternalErrorState\",\"InternalErrorState-Continue\":\"Disposed\"}";
        private const String CUSTOM_STATE_MACHINE_CONFIGURATION = "{\"Created-Continue\":\"Stopped\",\"Created-Cancel\":\"InternalErrorState\",\"Stopped-Run\":\"Running\",\"Running-Cancel\":\"Cancelled\"}";

        private StateMachine _stateMachine;

        [TestInitialize]
        public void TestInitialize()
        {
            _stateMachine = new StateMachine();
        }

        [TestMethod]
        public void CurrentStateOfNewlyCreatedInstanceReturnsCreated()
        {
            Assert.AreEqual(STATE_CREATED, _stateMachine.CurrentState);
        }

        [TestMethod]
        public void PreviousStateOfNewlyCreatedInstanceReturnsCreated()
        {
            Assert.AreEqual(STATE_CREATED, _stateMachine.CurrentState);
        }

        [TestMethod]
        public void InitialStateReturnsCreated()
        {
            Assert.AreEqual(STATE_CREATED, _stateMachine.InitialState);
        }

        [TestMethod]
        public void IsInitialStateOfNewlyCreatedInstanceReturnsTrue()
        {
            Assert.IsTrue(_stateMachine.IsInitialState);
        }

        [TestMethod]
        public void RunningStateReturnsRunning()
        {
            Assert.AreEqual(STATE_RUNNING, _stateMachine.RunningState);
        }

        [TestMethod]
        public void ErrorStateReturnsInternalErrorState()
        {
            Assert.AreEqual(STATE_ERROR, _stateMachine.ErrorState);
        }

        [TestMethod]
        public void CompletedStateReturnsCompleted()
        {
            Assert.AreEqual(STATE_COMPLETED, _stateMachine.CompletedState);
        }

        [TestMethod]
        public void CancelledStateReturnsCancelled()
        {
            Assert.AreEqual(STATE_CANCELLED, _stateMachine.CancelledState);
        }

        [TestMethod]
        public void FinalStateReturnsDisposed()
        {
            Assert.AreEqual(STATE_FINAL, _stateMachine.FinalState);
        }

        [TestMethod]
        public void IsFinalStateOfNewlyCreatedInstanceReturnsFalse()
        {
            Assert.IsFalse(_stateMachine.IsFinalState);
        }

        [TestMethod]
        public void ContinueConditionReturnsContinue()
        {
            Assert.AreEqual(CONDITION_CONTINUE, _stateMachine.ContinueCondition);
        }

        [TestMethod]
        public void CancelConditionReturnsCancel()
        {
            Assert.AreEqual(CONDITION_CANCEL, _stateMachine.CancelCondition);
        }
        
        [TestMethod]
        public void StateMachinesConstructorAddsDefaultStates()
        {
            Assert.IsTrue(_stateMachine.States.Contains(STATE_CREATED));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_RUNNING));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_COMPLETED));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_CANCELLED));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_ERROR));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_FINAL));
            Assert.AreEqual(6, _stateMachine.States.Count);
        }

        [TestMethod]
        public void StateMachinesConstructorAddsDefaultConditions()
        {
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_CONTINUE));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_CANCEL));
            Assert.AreEqual(2, _stateMachine.Conditions.Count);
        }

        [TestMethod]
        public void StateMachinesConstructorAddsDefaultTransitions()
        {
            Assert.AreEqual(9, _stateMachine.Transitions.Count);
            var transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_RUNNING, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_RUNNING, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_COMPLETED, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_COMPLETED, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_CANCELLED, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_CANCELLED, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_ERROR, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationReturnsTrue()
        {
            Assert.IsTrue(_stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION));
        }

        [TestMethod]
        public void SetupStateMachineWithEmptyConfigurationReturnsTrue()
        {
            Assert.IsTrue(_stateMachine.SetupStateMachine("{}"));
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationAddsConditionsOfCondiguration()
        {
            _stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION);
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_CONTINUE));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_CANCEL));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_RUN));
            Assert.AreEqual(3, _stateMachine.Conditions.Count);
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationAddsStatesOfCondiguration()
        {
            _stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION);
            Assert.IsTrue(_stateMachine.States.Contains(STATE_CREATED));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_STOPPED));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_ERROR));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_RUNNING));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_CANCELLED));
            Assert.AreEqual(5, _stateMachine.States.Count);
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationSetsStateTransitionsAccordingCondiguration()
        {
            _stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION);
            var transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CONTINUE);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_STOPPED, CONDITION_RUN);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            transition = new StateMachine.StateTransition(STATE_RUNNING, CONDITION_CANCEL);
            Assert.IsTrue(_stateMachine.Transitions.ContainsKey(transition));
            Assert.AreEqual(4, _stateMachine.Transitions.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupStateMachineWithNonExistingCurrentStateThrowsException()
        {
            _stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION, STATE_PENDING);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupStateMachineWithNonExistingPreviousStateThrowsException()
        {
            _stateMachine.SetupStateMachine(CUSTOM_STATE_MACHINE_CONFIGURATION, STATE_CREATED, STATE_PENDING);
        }

        [TestMethod]
        public void AddConditionWithNonExistingConditionAddsConditionToStateMachine()
        {
            _stateMachine.AddCondition(CONDITION_DEPLOY);
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_DEPLOY));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddConditionWithAlreadyExistingConditionThrowsException()
        {
            _stateMachine.AddCondition(CONDITION_CANCEL);
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingNotExistingConditionsIgnoringExistingAddsAllConditionsToStateMachine()
        {
            _stateMachine.AddConditions(new List<String>{ CONDITION_DEPLOY, CONDITION_RUN }, true);
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_DEPLOY));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_RUN));
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingNotExistingConditionsNotIgnoringExistingAddsAllConditionsToStateMachine()
        {
            _stateMachine.AddConditions(new List<String> { CONDITION_DEPLOY, CONDITION_RUN });
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_DEPLOY));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_RUN));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddConditionsWithConditionCollectionContainingExistingConditionsNotIgnoringExistingThrowsException()
        {
            _stateMachine.AddConditions(new List<String> { CONDITION_DEPLOY, CONDITION_RUN, CONDITION_CANCEL });
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingExistingConditionsIgnoringExistingAddsAllConditionsToStateMachine()
        {
            _stateMachine.AddConditions(new List<String> { CONDITION_DEPLOY, CONDITION_RUN, CONDITION_CANCEL }, true);
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_DEPLOY));
            Assert.IsTrue(_stateMachine.Conditions.Contains(CONDITION_RUN));
        }

        [TestMethod]
        public void AddStateWithNonExistingStateAddsStateToStateMachine()
        {
            _stateMachine.AddState(STATE_PENDING);
            Assert.IsTrue(_stateMachine.States.Contains(STATE_PENDING));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddStateWithAlreadyExistingStateThrowsException()
        {
            _stateMachine.AddState(STATE_RUNNING);
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingNotExistingStatesIgnoringExistingAddsAllStatesToStateMachine()
        {
            _stateMachine.AddStates(new List<String>{ STATE_PENDING, STATE_STOPPED }, true);
            Assert.IsTrue(_stateMachine.States.Contains(STATE_PENDING));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_STOPPED));
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingNotExistingStatesNotIgnoringExistingAddsAllStatesToStateMachine()
        {
            _stateMachine.AddStates(new List<String> { STATE_PENDING, STATE_STOPPED }, false);
            Assert.IsTrue(_stateMachine.States.Contains(STATE_PENDING));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_STOPPED));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddStatesWithStateCollectionContainingExistingStatesNotIgnoringExistingThrowsException()
        {
            _stateMachine.AddStates(new List<String> { STATE_PENDING, STATE_STOPPED, STATE_COMPLETED });
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingExistingStatesIgnoringExistingAddsAllStatesToStateMachine()
        {
            _stateMachine.AddStates(new List<String> { STATE_PENDING, STATE_STOPPED, STATE_COMPLETED }, true);
            Assert.IsTrue(_stateMachine.States.Contains(STATE_PENDING));
            Assert.IsTrue(_stateMachine.States.Contains(STATE_STOPPED));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingSourceStateThrowsException()
        {
            _stateMachine.SetStateTransition(STATE_STOPPED, CONDITION_CANCEL, STATE_CANCELLED);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingTargetStateAndCreateTargetStateFlagFalseThrowsException()
        {
            _stateMachine.SetStateTransition(STATE_CREATED, CONDITION_CONTINUE, STATE_STOPPED);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingConditionThrowsException()
        {
            _stateMachine.SetStateTransition(STATE_CREATED, CONDITION_RUN, STATE_COMPLETED);
        }

        [TestMethod]
        public void SetStateTransitionWithExistingStateTransitionAndReplaceTrueReplacesTransition()
        {
            _stateMachine.SetStateTransition(STATE_CREATED, CONDITION_CONTINUE, STATE_ERROR, true);
            var transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CONTINUE);
            Assert.AreEqual(STATE_ERROR, _stateMachine.Transitions[transition]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetStateTransitionWithExistingStateTransitionAndReplaceFalseThrowsException()
        {
            _stateMachine.SetStateTransition(STATE_CREATED, CONDITION_CONTINUE, STATE_ERROR);
        }

        [TestMethod]
        public void SetStateTransitionWithNonExistingStateTransitionAddsTransition()
        {
            _stateMachine.SetStateTransition(STATE_ERROR, CONDITION_CANCEL, STATE_FINAL);
            var transition = new StateMachine.StateTransition(STATE_ERROR, CONDITION_CANCEL);
            Assert.AreEqual(STATE_FINAL, _stateMachine.Transitions[transition]);
        } 

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ValidateInputThrowsExceptionForNonExistingSourceState()
        {
            var wrapper = new PrivateObject(_stateMachine);
            wrapper.Invoke("ValidateInput", new Object[4] { STATE_STOPPED, CONDITION_CONTINUE, STATE_RUNNING, false });
        }

        [TestMethod]
        public void GetNextStateWithValidConditionReturnsNextState()
        {
            Assert.AreEqual(STATE_ERROR, _stateMachine.GetNextState(CONDITION_CANCEL));
            Assert.AreEqual(STATE_RUNNING, _stateMachine.GetNextState(CONDITION_CONTINUE));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNextStateWithNonExistingConditionThrowsException()
        {
            _stateMachine.GetNextState(CONDITION_DEPLOY);
        }

        [TestMethod]
        public void NextChangesStateWithContinueCondition()
        {
            Assert.AreEqual(STATE_RUNNING, _stateMachine.Next());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NextForStateWithNoContinueTransitionThrowsException()
        {
           _stateMachine.Next();
           _stateMachine.Next();
           _stateMachine.Next();
           _stateMachine.Next();
        }

        [TestMethod]
        public void CancelChangesStateWithCancelCondition()
        {
            Assert.AreEqual(STATE_ERROR, _stateMachine.Cancel());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CancelForStateWithNoCancelTransitionThrowsException()
        {
            _stateMachine.Next();
            _stateMachine.Next();
            _stateMachine.Next();
            _stateMachine.Cancel();
        }

        [TestMethod]
        public void ChangeStateGetsNextStateBasedOnConditionAndReturnsNextState()
        {
            Assert.AreEqual(STATE_RUNNING, _stateMachine.GetNextState(CONDITION_CONTINUE));
            Assert.AreEqual(STATE_ERROR, _stateMachine.GetNextState(CONDITION_CANCEL));
        }

        [TestMethod]
        public void ClearResetsStatesConditionsAndTransitions()
        {
            _stateMachine.Clear();
            Assert.AreEqual(0, _stateMachine.States.Count);
            Assert.AreEqual(0, _stateMachine.Conditions.Count);
            Assert.AreEqual(0, _stateMachine.Transitions.Count);
        }

        [TestMethod]
        public void GetStringRepresentationReturnsJsonRepresentationOfStateMachine()
        {
            Assert.AreEqual(DEFAULT_STATE_MACHINE_STRING_REPRESENTATION, _stateMachine.GetStringRepresentation());
        }


        [TestMethod]
        public void StateTransitionToStringReturnsStringRepresentationOfStateTransition()
        {
            var transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_RUN);
            Assert.AreEqual(STATE_CREATED + "-" + CONDITION_RUN, transition.ToString());
        }

        [TestMethod]
        public void StateTransitionEqualsStateTransitionsWithSameCurrentStateAndConditionReturnsTrue()
        {
            var transition1 = new StateMachine.StateTransition(STATE_CREATED, CONDITION_RUN);
            var transition2 = new StateMachine.StateTransition(STATE_CREATED, CONDITION_RUN);
            Assert.IsTrue(transition1.Equals(transition2));
        }

        [TestMethod]
        public void StateTransitionEqualsStateTransitionsWithDifferentCurrentStateAndConditionReturnsFalse()
        {
            var transition1 = new StateMachine.StateTransition(STATE_CREATED, CONDITION_RUN);
            var transition2 = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CONTINUE);
            Assert.IsFalse(transition1.Equals(transition2));
            transition1 = new StateMachine.StateTransition(STATE_ERROR, CONDITION_CONTINUE);
            transition2 = new StateMachine.StateTransition(STATE_CREATED, CONDITION_CONTINUE);
            Assert.IsFalse(transition1.Equals(transition2));
        }

        [TestMethod]
        public void StateTransitionEqualsNullReturnsFalse()
        {
            var transition = new StateMachine.StateTransition(STATE_CREATED, CONDITION_RUN);
            Assert.IsFalse(transition.Equals(null));
        }
    }
}
