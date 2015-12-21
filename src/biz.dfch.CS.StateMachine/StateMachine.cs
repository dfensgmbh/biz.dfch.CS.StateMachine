/**
 * Copyright 2015 d-fens GmbH
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
using System.Diagnostics.Contracts;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("biz.dfch.CS.StateMachine.Tests")]
namespace biz.dfch.CS.FiniteStateMachine
{
    public class StateMachine
    {
        private HashSet<string> _conditions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> Conditions
        {
            get
            {
                return _conditions;
            }
            protected set
            {
                _conditions = value;
            }
        }

        private HashSet<string> _states = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> States
        {
            get
            {
                return _states;
            }
            protected set
            {
                _states = value;
            }
        }

        private Dictionary<StateTransition, string> _transitions = new Dictionary<StateTransition, string>();
        protected internal Dictionary<StateTransition, string> Transitions
        {
            get
            {
                return _transitions;
            }
            private set
            {
                _transitions = value;
            }
        }

        private object _lock = new Object();

        public bool HasState(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            return _states.Contains(name);
        }

        public bool HasCondition(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            return _conditions.Contains(name);
        }

        public HashSet<string> ConditionsFromState()
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(CurrentState));

            return ConditionsFromState(CurrentState);
        }

        public HashSet<string> ConditionsFromState(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var conditions = _transitions
                .Select(t => t.Key)
                .Where(t => t.CurrentState.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.Condition)
                .ToList();
            Contract.Assert(null != conditions);

            return new HashSet<string>(conditions);
        }

        public string CurrentState { get; protected set; }
        public string PreviousState { get; protected set; }
        public string InitialState
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

        public string RunningState
        {
            get
            {
                return "Running";
            }
            protected set { }
        }

        public string ErrorState
        {
            get
            {
                return "InternalErrorState";
            }
            protected set { }
        }

        public string CompletedState
        {
            get
            {
                return "Completed";
            }
            protected set { }
        }

        public string CancelledState
        {
            get
            {
                return "Cancelled";
            }
            protected set { }
        }

        public string FinalState
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

        public string ContinueCondition
        {
            get
            {
                return "Continue";
            }
            protected set { }
        }

        public string CancelCondition
        {
            get
            {
                return "Cancel";
            }
            protected set { }
        }

        public StateMachine()
        {
            CurrentState = InitialState;
            PreviousState = InitialState;

            AddStates(new List<string> { InitialState, RunningState, CompletedState, CancelledState, ErrorState, FinalState });
            AddConditions(new List<string> { ContinueCondition, CancelCondition });

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

        public virtual bool SetupStateMachine(string configuration, string currentState = null, string previousState = null)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(configuration));

            var fReturn = false;
            lock (_lock)
            {
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(configuration);
                Clear();
                foreach (KeyValuePair<string, string> item in dic)
                {
                    var sourceStateCondition = item.Key.Split('-');
                    var sourceState = sourceStateCondition.First();
                    var condition = sourceStateCondition.Last();
                    var targetState = item.Value.ToString();
                    _conditions.Add(condition);
                    _states.Add(sourceState);
                    _states.Add(targetState);
                    SetStateTransition(sourceState, condition, targetState, true);
                }
                if (null != currentState)
                {
                    if (!_states.Contains(currentState))
                    {
                        throw new ArgumentOutOfRangeException("currentState", string.Format("currentState: Parameter validation failed. '{0}' is not a valid state.", currentState));
                    }
                    CurrentState = currentState;
                }
                if (null != previousState)
                {
                    if (!_states.Contains(previousState))
                    {
                        throw new ArgumentOutOfRangeException("previousState", string.Format("previousState: Parameter validation failed. '{0}' is not a valid state.", previousState));
                    }
                    PreviousState = previousState;
                }
                fReturn = true;
            }
            return fReturn;
        }

        protected internal StateMachine AddCondition(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            lock (_lock)
            {
                var fReturn = _conditions.Contains(name, StringComparer.OrdinalIgnoreCase);
                if (fReturn)
                {
                    throw new ArgumentException(string.Format("Machine condition already exists: '{0}'", name), "name");
                }
                _conditions.Add(name);
            }
            return this;
        }

        protected internal StateMachine AddConditions(IEnumerable<string> names, bool ignoreExisting = false)
        {
            Contract.Requires(null != names);

            lock (_lock)
            {
                var fReturn = _conditions.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(string.Format("Machine states already exist: '{0}'", string.Join(", ", _states.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    _conditions.Add(name);
                }
            }
            return this;
        }

        protected internal StateMachine AddState(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            lock (_lock)
            {
                var fReturn = _states.Contains(name);
                if (fReturn)
                {
                    throw new ArgumentException(string.Format("Machine state already exists: '{0}'", name), "name");
                }
                _states.Add(name);
            }
            return this;
        }

        protected internal StateMachine AddStates(IEnumerable<string> names, bool ignoreExisting = false)
        {
            Contract.Requires(null != names);

            lock (_lock)
            {
                var fReturn = _states.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(string.Format("Machine states already exist: '{0}'", string.Join(", ", _states.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    _states.Add(name);
                }
            }
            return this;
        }

        protected internal StateMachine SetStateTransition(string sourceState, string condition, string targetState, bool fReplace = false)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(sourceState));
            Contract.Requires(!string.IsNullOrWhiteSpace(condition));
            Contract.Requires(!string.IsNullOrWhiteSpace(targetState));

            lock (_lock)
            {
                ValidateInput(sourceState, condition, targetState);
                string _processState;
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
                        throw new ArgumentException(string.Format("stateTransition already exists: '{0}' -- > '{1}'", sourceState, condition));
                    }
                }
                Transitions.Add(stateTransition, targetState);
            }
            return this;
        }

        private void ValidateInput(string sourceState, string condition, string targetState, bool fCreateTargetState = false)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(sourceState));
            Contract.Requires(!string.IsNullOrWhiteSpace(condition));
            Contract.Requires(!string.IsNullOrWhiteSpace(targetState));

            if (!_states.Contains(sourceState))
            {
                throw new KeyNotFoundException(string.Format("sourceState not found: '{0}'", sourceState));
            }
            if (!_states.Contains(targetState))
            {
                if (!fCreateTargetState)
                {
                    throw new KeyNotFoundException(string.Format("targetStateNew not found: '{0}'", targetState));
                }
                AddState(targetState);
            }
            if (!_conditions.Contains(condition))
            {
                throw new KeyNotFoundException(string.Format("condition not found: '{0}'", condition));
            }
        }

        public string GetNextState(string condition)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(condition));

            StateTransition transition = new StateTransition(CurrentState, condition);
            string _nextState;
            lock (_lock)
            {
                if (!Transitions.TryGetValue(transition, out _nextState))
                {
                    throw new ArgumentOutOfRangeException(string.Format("stateTransition is invalid: '{0}' @ '{1}'", CurrentState, condition));
                }
            }
            return _nextState;
        }

        public string Next() { return Continue(); }
        public string Continue() { return ChangeState(ContinueCondition); }
        public string Cancel() { return ChangeState(CancelCondition); }

        public string ChangeState(string condition)
        {
            lock (_lock)
            {
                string _nextState = GetNextState(condition);
                PreviousState = CurrentState;
                CurrentState = _nextState;
            }
            return CurrentState;
        }

        protected internal virtual void Clear()
        {
            lock (_lock)
            {
                Transitions.Clear();
                _states.Clear();
                _conditions.Clear();
            }
        }

        public virtual string GetStringRepresentation()
        {
            lock (_lock)
            {
                var dic = Transitions.ToDictionary(k => k.Key.ToString(), v => v.Value);
                var stateMachineSerialised = JsonConvert.SerializeObject(dic);
                return stateMachineSerialised;
            }
        }

        public class StateTransition
        {
            internal readonly string CurrentState;
            internal readonly string Condition;

            public StateTransition(string sourceState, string condition)
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
                return other != null
                    && this.CurrentState.Equals(other.CurrentState, StringComparison.OrdinalIgnoreCase)
                    && this.Condition.Equals(other.Condition, StringComparison.OrdinalIgnoreCase);
            }

            public override string ToString()
            {
                return string.Format("{0}-{1}", CurrentState, Condition);
            }
        }
    }
}
