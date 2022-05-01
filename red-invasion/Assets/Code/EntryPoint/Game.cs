using System;
using Code.States;
using UnityEngine;

namespace Code.EntryPoint
{
    public class Game : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private readonly Type _entryState = typeof(LoadLevelState);

        private void Start()
        {
            ConfigureStateMachine();
            RunGame();
        }

        private void ConfigureStateMachine()
        {
            _stateMachine = new StateMachine();
            _stateMachine.AddState(typeof(LoadLevelState), new LoadLevelState());
        }

        private void RunGame()
        {
            _stateMachine.SetState(_entryState);
        }
    }
}