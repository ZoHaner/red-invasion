using Code.Services;

namespace Code.States
{
    public class LoadSessionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameSession _session;

        public LoadSessionState(StateMachine stateMachine, IGameSession session)
        {
            _stateMachine = stateMachine;
            _session = session;
        }

        public async void Enter()
        {
            await _session.WarmUp();
            _stateMachine.SetState(typeof(GameSessionState));
        }

        public void Exit()
        {
        }
    }
}