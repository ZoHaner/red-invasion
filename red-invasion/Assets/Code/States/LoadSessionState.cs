using Code.Services;

namespace Code.States
{
    public class LoadSessionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameSession _session;
        private readonly HUDService _hudService;

        public LoadSessionState(StateMachine stateMachine, IGameSession session, HUDService hudService)
        {
            _stateMachine = stateMachine;
            _session = session;
            _hudService = hudService;
        }

        public async void Enter()
        {
            await _session.WarmUp();
            await _hudService.Warmup();
            _stateMachine.SetState(typeof(GameSessionState));
        }

        public void Exit()
        {
        }
    }
}