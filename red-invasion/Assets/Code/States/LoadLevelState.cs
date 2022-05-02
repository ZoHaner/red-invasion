using Code.Services;

namespace Code.States
{
    public class LoadLevelState : IState
    {
        private readonly IGameFactory _factory;

        public LoadLevelState(IGameFactory factory)
        {
            _factory = factory;
        }

        public void Enter()
        {
            _factory.SpawnEnemies();
        }

        public void Exit()
        {
        }
    }
}