using Code.Services;
using UnityEngine;

namespace Code.States
{
    public class GameSessionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameSession _session;
        private int i;

        public GameSessionState(StateMachine stateMachine, IGameSession session)
        {
            _stateMachine = stateMachine;
            _session = session;
        }

        public async void Enter()
        {
            SubscribeOnEvents();

            _session.Initialize();
            _session.SpawnPlayer();
            _session.SpawnEnemies();
        }

        public void Exit()
        {
            // UnsubscribeFromEvents();

            _session.Cleanup();
        }

        private void MoveToWinScreen()
        {
            UnsubscribeFromEvents();

            Debug.Log("Win");
            _stateMachine.SetState(typeof(GameSessionState));
        }

        private void MoveToLooseScreen()
        {
            UnsubscribeFromEvents();

            i++;
            Debug.Log("Loose " + i);
            _stateMachine.SetState(typeof(GameSessionState));
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