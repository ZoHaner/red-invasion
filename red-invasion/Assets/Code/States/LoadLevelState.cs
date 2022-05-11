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
        }

        public void Exit()
        {
            UnsubscribeFromEvents();

            _session.Cleanup();
        }

        private void MoveToWinScreen()
        {
            Debug.Log("Win");
            _stateMachine.SetState(typeof(LoadLevelState));
        }

        private void MoveToLooseScreen()
        {
            Debug.Log("Loose");
            _stateMachine.SetState(typeof(LoadLevelState));
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