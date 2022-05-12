namespace Code.States
{
    public class WinState : IState
    {
        private readonly StateMachine _stateMachine;

        public WinState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}