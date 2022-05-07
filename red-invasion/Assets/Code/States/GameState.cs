using Code.Services;

namespace Code.States
{
    public class GameState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameSession _session;

        public GameState(StateMachine stateMachine, GameSession session)
        {
            _stateMachine = stateMachine;
            _session = session;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}