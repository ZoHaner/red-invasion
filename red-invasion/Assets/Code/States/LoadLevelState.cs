using Code.Services;

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
            await _session.WarmUp();
            _session.Initialize();
            _session.SpawnPlayer();
            _session.SpawnEnemies();

            _stateMachine.SetState(typeof(GameState));
        }

        public void Exit()
        {
        }
    }
}