using System;
using Code.Input;
using Code.Services;
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

            var assetProvider = new AssetProvider();
            var updateProvider = CreateUpdateProvider();
            var inputService = new PlayerInputService();

            var session = new GameSessionFacade(assetProvider, updateProvider, inputService);
            
            _stateMachine.AddState(typeof(LoadLevelState), new LoadLevelState(_stateMachine, session));
            _stateMachine.AddState(typeof(GameState), new GameState(_stateMachine, session));
        }

        private IUpdateProvider CreateUpdateProvider() => 
            new GameObject(nameof(UpdateProvider)).AddComponent<UpdateProvider>();

        private void RunGame() => 
            _stateMachine.SetState(_entryState);
    }
}