﻿/**
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("biz.dfch.CS.StateMachine.Tests")]
namespace biz.dfch.CS.StateMachine
{
    public class StateMachine<T>
    {
        protected internal HashSet<String> Conditions = new HashSet<String>(StringComparer.OrdinalIgnoreCase);
        protected internal HashSet<String> States = new HashSet<String>(StringComparer.OrdinalIgnoreCase);
        protected internal Dictionary<StateTransition, String> Transitions = new Dictionary<StateTransition, String>();
        protected internal Object Lock = new Object();

        public String TenantId { get; private set; }
        public String UserId { get; private set; }
        public String CurrentState { get; protected set; }
        public String PreviousState { get; protected set; }
        public String InitialState
        {
            get
            {
                return "Created";
            }
            protected set { }
        }
        public virtual bool IsInitialState
        {
            get
            {
                return CurrentState.Equals(InitialState, StringComparison.OrdinalIgnoreCase);
            }
        }
        public String RunningState
        {
            get
            {
                return "Running";
            }
            protected set { }
        }
        public String ErrorState
        {
            get
            {
                return "InternalErrorState";
            }
            protected set { }
        }
        public String CompletedState
        {
            get
            {
                return "Completed";
            }
            protected set { }
        }
        public String CancelledState
        {
            get
            {
                return "Cancelled";
            }
            protected set { }
        }
        public String FinalState
        {
            get
            {
                return "Disposed";
            }
            protected set { }
        }
        public virtual bool IsFinalState
        {
            get
            {
                return CurrentState.Equals(FinalState, StringComparison.OrdinalIgnoreCase);
            }
        }
        public String ContinueCondition
        {
            get
            {
                return "Continue";
            }
            protected set { }
        }
        public String CancelCondition
        {
            get
            {
                return "Cancel";
            }
            protected set { }
        }

        public StateMachine(String tenantId, String userId)
        {
            TenantId = tenantId;
            UserId = userId;
            CurrentState = InitialState;
            PreviousState = InitialState;

            AddStates(new List<String> { InitialState, RunningState, CompletedState, CancelledState, ErrorState, FinalState });
            AddConditions(new List<String> { ContinueCondition, CancelCondition });

            SetDefaultStateTransitions();
        }

        private void SetDefaultStateTransitions()
        {
            SetStateTransition(InitialState, ContinueCondition, RunningState);
            SetStateTransition(InitialState, CancelCondition, ErrorState);
            SetStateTransition(RunningState, ContinueCondition, CompletedState);
            SetStateTransition(RunningState, CancelCondition, CancelledState);
            SetStateTransition(CompletedState, ContinueCondition, FinalState);
            SetStateTransition(CompletedState, CancelCondition, ErrorState);
            SetStateTransition(CancelledState, ContinueCondition, FinalState);
            SetStateTransition(CancelledState, CancelCondition, ErrorState);
            SetStateTransition(ErrorState, ContinueCondition, FinalState);
        }

        public virtual bool SetupStateMachine(String configuration, String currentState = null, String previousState = null)
        {
            var fReturn = false;
            lock (Lock)
            {
                Dictionary<String, String> dic = JsonConvert.DeserializeObject<Dictionary<String, String>>(configuration);
                Clear();
                foreach (KeyValuePair<String, String> item in dic)
                {
                    var sourceStateCondition = item.Key.Split('-');
                    var sourceState = sourceStateCondition.First();
                    var condition = sourceStateCondition.Last();
                    var targetState = item.Value.ToString();
                    Conditions.Add(condition);
                    States.Add(sourceState);
                    States.Add(targetState);
                    SetStateTransition(sourceState, condition, targetState, true);
                }
                if (null != currentState)
                {
                    if (!States.Contains(currentState))
                    {
                        throw new ArgumentOutOfRangeException("currentState", String.Format("currentState: Parameter validation failed. '{0}' is not a valid state.", currentState));
                    }
                    CurrentState = currentState;
                }
                if (null != previousState)
                {
                    if (!States.Contains(previousState))
                    {
                        throw new ArgumentOutOfRangeException("previousState", String.Format("previousState: Parameter validation failed. '{0}' is not a valid state.", previousState));
                    }
                    PreviousState = previousState;
                }
                fReturn = true;
            }
            return fReturn;
        }

        protected internal StateMachine<T> AddCondition(String name)
        {
            lock (Lock)
            {
                var fReturn = Conditions.Contains(name, StringComparer.OrdinalIgnoreCase);
                if (fReturn)
                {
                    throw new ArgumentException(String.Format("Machine condition already exists: '{0}'", name), "name");
                }
                Conditions.Add(name);
            }
            return this;
        }

        protected internal StateMachine<T> AddConditions(IEnumerable<String> names, bool ignoreExisting = false)
        {
            lock (Lock)
            {
                var fReturn = Conditions.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(String.Format("Machine states already exist: '{0}'", String.Join(", ", States.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    Conditions.Add(name);
                }
            }
            return this;
        }

        protected internal StateMachine<T> AddState(String name)
        {
            lock (Lock)
            {
                var fReturn = States.Contains(name);
                if (fReturn)
                {
                    throw new ArgumentException(String.Format("Machine state already exists: '{0}'", name), "name");
                }
                States.Add(name);
            }
            return this;
        }

        protected internal StateMachine<T> AddStates(IEnumerable<String> names, bool ignoreExisting = false)
        {
            lock (Lock)
            {
                var fReturn = States.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(String.Format("Machine states already exist: '{0}'", String.Join(", ", States.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    States.Add(name);
                }
            }
            return this;
        }

        protected internal StateMachine<T> SetStateTransition(String sourceState, String condition, String targetState, bool fReplace = false)
        {
            lock (Lock)
            {
                ValidateInput(sourceState, condition, targetState);
                String _processState;
                var stateTransition = new StateTransition(sourceState, condition);
                var fReturn = Transitions.TryGetValue(stateTransition, out _processState);
                if (fReturn)
                {
                    if (fReplace)
                    {
                        Transitions.Remove(stateTransition);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("stateTransition already exists: '{0}' -- > '{1}'", sourceState, condition));
                    }
                }
                Transitions.Add(stateTransition, targetState);
            }
            return this;
        }

        private void ValidateInput(string sourceState, string condition, string targetState, bool fCreateTargetState = false)
        {
            if (!States.Contains(sourceState))
            {
                throw new KeyNotFoundException(String.Format("sourceState not found: '{0}'", sourceState));
            }
            if (!States.Contains(targetState))
            {
                if (!fCreateTargetState)
                {
                    throw new KeyNotFoundException(String.Format("targetStateNew not found: '{0}'", targetState));
                }
                AddState(targetState);
            }
            if (!Conditions.Contains(condition))
            {
                throw new KeyNotFoundException(String.Format("condition not found: '{0}'", condition));
            }
        }

        public String GetNextState(String condition)
        {
            StateTransition transition = new StateTransition(CurrentState, condition);
            String _nextState;
            lock (Lock)
            {
                if (!Transitions.TryGetValue(transition, out _nextState))
                {
                    throw new ArgumentOutOfRangeException(String.Format("stateTransition is invalid: '{0}' @ '{1}'", CurrentState, condition));
                }
            }
            return _nextState;
        }

        public String Next() { return Continue(); }
        public String Continue() { return ChangeState(ContinueCondition); }
        public String Cancel() { return ChangeState(CancelCondition); }

        public String ChangeState(String condition)
        {
            lock (Lock)
            {
                String _nextState = GetNextState(condition);
                PreviousState = CurrentState;
                CurrentState = _nextState;
            }
            return CurrentState;
        }

        protected internal virtual void Clear()
        {
            lock (Lock)
            {
                Transitions.Clear();
                States.Clear();
                Conditions.Clear();
            }
        }

        public virtual String GetStringRepresentation()
        {
            lock (Lock)
            {
                var dic = Transitions.ToDictionary(k => k.Key.ToString(), v => v.Value);
                var stateMachineSerialised = JsonConvert.SerializeObject(dic);
                return stateMachineSerialised;
            }
        }

        protected internal class StateTransition
        {
            readonly String CurrentState;
            readonly String Condition;

            public StateTransition(String sourceState, String condition)
            {
                CurrentState = sourceState;
                Condition = condition;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Condition.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState.Equals(other.CurrentState, StringComparison.OrdinalIgnoreCase) && this.Condition.Equals(other.Condition, StringComparison.OrdinalIgnoreCase);
            }

            public override String ToString()
            {
                return String.Format("{0}-{1}", CurrentState, Condition);
            }
        }
    }
}
