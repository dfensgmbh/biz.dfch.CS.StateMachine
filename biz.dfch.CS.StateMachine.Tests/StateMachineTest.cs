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
        private const String CREATED_STATE = "Created";
        private const String ERROR_STATE = "InternalErrorState";
        private const String RUNNING_STATE = "Running";
        private const String COMPLETED_STATE = "Completed";
        private const String CANCELLED_STATE = "Cancelled";
        private const String FINAL_STATE = "Disposed";
        private const String CONTINUE_CONDITION = "Continue";
        private const String CANCEL_CONDITION = "Cancel";
        private StateMachine _stateMachine;

        [TestInitialize]
        public void TestInitialize()
        {
            _stateMachine = new StateMachine();
        }

        [TestMethod]
        public void CurrentStateOfNewlyCreatedInstanceReturnsCreated()
        {
            Assert.AreEqual(CREATED_STATE, _stateMachine.CurrentState);
        }

        [TestMethod]
        public void PreviousStateOfNewlyCreatedInstanceReturnsCreated()
        {
            Assert.AreEqual(CREATED_STATE, _stateMachine.CurrentState);
        }

        [TestMethod]
        public void InitialStateReturnsCreated()
        {
            Assert.AreEqual(CREATED_STATE, _stateMachine.InitialState);
        }

        [TestMethod]
        public void IsInitialStateOfNewlyCreatedInstanceReturnsTrue()
        {
            Assert.IsTrue(_stateMachine.IsInitialState);
        }

        [TestMethod]
        public void RunningStateReturnsRunning()
        {
            Assert.AreEqual(RUNNING_STATE, _stateMachine.RunningState);
        }

        [TestMethod]
        public void ErrorStateReturnsInternalErrorState()
        {
            Assert.AreEqual(ERROR_STATE, _stateMachine.ErrorState);
        }

        [TestMethod]
        public void CompletedStateReturnsCompleted()
        {
            Assert.AreEqual(COMPLETED_STATE, _stateMachine.CompletedState);
        }

        [TestMethod]
        public void CancelledStateReturnsCancelled()
        {
            Assert.AreEqual(CANCELLED_STATE, _stateMachine.CancelledState);
        }

        [TestMethod]
        public void FinalStateReturnsDisposed()
        {
            Assert.AreEqual(FINAL_STATE, _stateMachine.FinalState);
        }

        [TestMethod]
        public void IsFinalStateOfNewlyCreatedInstanceReturnsFalse()
        {
            Assert.IsFalse(_stateMachine.IsFinalState);
        }

        [TestMethod]
        public void ContinueConditionReturnsContinue()
        {
            Assert.AreEqual(CONTINUE_CONDITION, _stateMachine.ContinueCondition);
        }

        [TestMethod]
        public void CancelConditionReturnsCancel()
        {
            Assert.AreEqual(CANCEL_CONDITION, _stateMachine.CancelCondition);
        }

        [TestMethod]
        public void StateMachinesDefaultConstructorAddsDefaultStates()
        {
            // DFTODO impl   
        }

        [TestMethod]
        public void StateMachinesDefaultConstructorAddsDefaultConditions()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void StateMachinesDefaultConstructorAddsDefaultTransitions()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationReturnsTrue()
        {
            
        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationAddsConditionsOfCondiguration()
        {

        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationAddsStatesOfCondiguration()
        {

        }

        [TestMethod]
        public void SetupStateMachineWithValidConfigurationSetsStateTransitionsAccordingCondiguration()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupStateMachineWithNonExistingCurrentStateThrowsException()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupStateMachineWithNonExistingPreviousStateThrowsException()
        {

        }

        [TestMethod]
        public void AddConditionWithNonExistingConditionAddsConditionToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddConditionWithAlreadyExistingConditionThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingNotExistingConditionsIgnoringExistingAddsAllConditionsToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingNotExistingConditionsNotIgnoringExistingAddsAllConditionsToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddConditionsWithConditionCollectionContainingExistingConditionsNotIgnoringExistingThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddConditionsWithConditionCollectionContainingExistingConditionsIgnoringExistingAddsAllConditionsToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddStateWithNonExistingStateAddsStateToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddStateWithAlreadyExistingStateThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingNotExistingStatesIgnoringExistingAddsAllStatesToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingNotExistingStatesNotIgnoringExistingAddsAllStatesToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddStatesWithStateCollectionContainingExistingStatesNotIgnoringExistingThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void AddStatesWithStateCollectionContainingExistingStatesIgnoringExistingAddsAllStatesToStateMachine()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingSourceStateThrowsException()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingTargetStateAndCreateTargetStateFlagFalseThrowsException()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SetStateTransitionWithNonExistingConditionThrowsException()
        {
            
        }

        [TestMethod]
        public void SetStateTransitionWithExistingStateTransitionAndReplaceTrueReplacesTransition()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetStateTransitionWithExistingStateTransitionAndReplaceFalseThrowsException()
        {

        }

        [TestMethod]
        public void SetStateTransitionWithNonExistingStateTransitionAddsTransition()
        {

        } 

        [TestMethod]
        public void GetNextStateWithValidConditionReturnsNextState()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNextStateWithNonExistingConditionThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void NextChangesStateWithContinueCondition()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NextForStateWithNoContinueTransitionThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void CancelChangesStateWithCancelCondition()
        {
            // DFTODO impl
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CancelForStateWithNoCancelTransitionThrowsException()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void ChangeStateGetsNextStateBasedOnConditionAndReturnsNextState()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void ClearResetsStatesConditionsAndTransitions()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void GetStringRepresentationReturnsJsonRepresentationOfStateMachine()
        {
            // DFTODO impl
        }


        [TestMethod]
        public void StateTransitionToStringReturnsStringRepresentationOfStateTransition()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void StateTransitionEqualsStateTransitionsWithSameCurrentStateAndConditionReturnsTrue()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void StateTransitionEqualsStateTransitionsWithDifferentCurrentStateAndConditionReturnsFalse()
        {
            // DFTODO impl
        }

        [TestMethod]
        public void StateTransitionEqualsNullReturnsFalse()
        {
            // DFTODO impl
        }
    }
}
