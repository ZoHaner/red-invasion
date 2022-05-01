using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.States
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _currentState;

        public void AddState(Type type, IState state)
        {
            _states[type] = state;
        }

        public void SetState(Type stateType)
        {
            if (_states.TryGetValue(stateType, out var newState))
            {
                _currentState?.Exit();
                newState.Enter();
                _currentState = newState;
            }
            else
            {
                Debug.LogError($"State {stateType} wasn't registered!");
            }
        }
    }
}