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
        private readonly Type _entryState = typeof(LoadSessionState);

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
            var hudService = new HUDService(assetProvider);

            var session = new GameSessionFacade(assetProvider, updateProvider, inputService);
            
            _stateMachine.AddState(typeof(LoadSessionState), new LoadSessionState(_stateMachine, session, hudService));
            _stateMachine.AddState(typeof(GameSessionState), new GameSessionState(_stateMachine, session));
            _stateMachine.AddState(typeof(WinState), new WinState(_stateMachine, hudService));
            _stateMachine.AddState(typeof(LooseState), new LooseState(_stateMachine, hudService));
        }

        private IUpdateProvider CreateUpdateProvider() => 
            new GameObject(nameof(UpdateProvider)).AddComponent<UpdateProvider>();

        private void RunGame() => 
            _stateMachine.SetState(_entryState);
    }
}