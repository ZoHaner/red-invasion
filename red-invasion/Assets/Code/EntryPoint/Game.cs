using System;
using Code.Bullets;
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
            var inputService = new StandaloneInputService();


            var bulletsCollisionHandler = new BulletsCollisionHandler();
            var factory = new GameFactory(assetProvider, updateProvider, inputService, bulletsCollisionHandler);
            
            _stateMachine.AddState(typeof(LoadLevelState), new LoadLevelState(factory));
        }

        private IUpdateProvider CreateUpdateProvider()
        {
            var provider = new GameObject("Update Provider");
            return provider.AddComponent<UpdateProvider>();
        }

        private void RunGame()
        {
            _stateMachine.SetState(_entryState);
        }
    }
}