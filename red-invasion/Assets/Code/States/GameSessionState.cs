using Code.Services;
using UnityEngine;

namespace Code.States
{
    public class GameSessionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameSession _session;

        public GameSessionState(StateMachine stateMachine, IGameSession session)
        {
            _stateMachine = stateMachine;
            _session = session;
        }

        public void Enter()
        {
            SubscribeOnEvents();

            _session.Initialize();
            _session.SpawnPlayer();
            _session.SpawnEnemies();
        }

        public void Exit()
        {
            _session.Cleanup();
        }

        private void MoveToWinScreen()
        {
            UnsubscribeFromEvents();
            _stateMachine.SetState(typeof(WinState));
        }

        private void MoveToLooseScreen()
        {
            UnsubscribeFromEvents();
            _stateMachine.SetState(typeof(LooseState));
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