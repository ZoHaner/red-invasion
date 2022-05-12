namespace Code.States
{
    public class LooseState : IState
    {
        private readonly StateMachine _stateMachine;

        public LooseState(StateMachine stateMachine)
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