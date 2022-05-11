using Code.Services;

namespace Code.States
{
    public class GameState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameSessionFacade _sessionFacade;

        public GameState(StateMachine stateMachine, GameSessionFacade sessionFacade)
        {
            _stateMachine = stateMachine;
            _sessionFacade = sessionFacade;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}