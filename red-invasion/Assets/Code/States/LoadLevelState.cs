using Code.Services;
using UnityEngine;

namespace Code.States
{
    public class LoadLevelState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameSession _session;

        public LoadLevelState(StateMachine stateMachine, IGameSession session)
        {
            _stateMachine = stateMachine;
            _session = session;
        }

        public async void Enter()
        {
            SubscribeOnEvents();
            
            await _session.WarmUp();
            _session.Initialize();
            _session.SpawnPlayer();
            _session.SpawnEnemies();

            // _stateMachine.SetState(typeof(GameState));
        }

        public void Exit() => 
            UnsubscribeFromEvents();

        private void MoveToWinScreen()
        {
            Debug.Log("Win");
        }

        private void MoveToLooseScreen()
        {
            Debug.Log("Loose");
        }

        private void SubscribeOnEvents()
        {
            _session.WinGame += MoveToWinScreen;
            _session.LooseGame += MoveToLooseScreen;
        }

        private void UnsubscribeFromEvents()
        {
            _session.WinGame -= MoveToWinScreen;
            _session.LooseGame -= MoveToLooseScreen;
        }
    }
}