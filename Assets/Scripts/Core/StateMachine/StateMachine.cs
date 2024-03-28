using System;
using System.Collections.Generic;
using Interfaces.Core.StateMachine;
using Interfaces.Services;

namespace Core.StateMachine
{
    public class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private readonly ILoggingService _loggingService;

        private IState _currentState;

        public StateMachine(ILoggingService loggingService, List<IState> states)
        {
            _loggingService = loggingService;
            _states = new Dictionary<Type, IState>();
            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        public void ChangeState<TState>() where TState : IState
        {
            ChangeState(typeof(TState));
        }

        public void ChangeState(Type stateType)
        {
            if (_states.TryGetValue(stateType, out var state) == false)
            {
                _loggingService.LogError($"State {stateType} not found");
                return;
            }

            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState.StateChangeRequested -= ChangeState;
            }

            _currentState = state;
            _currentState.StateChangeRequested += ChangeState;
            _currentState.Enter();
        }
    }
}