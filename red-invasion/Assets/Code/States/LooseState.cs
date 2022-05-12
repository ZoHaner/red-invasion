using System.Threading.Tasks;
using Code.Services;

namespace Code.States
{
    public class LooseState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly HUDService _hudService;
        private int _windowShowingTime = 1000;

        public LooseState(StateMachine stateMachine, HUDService hudService)
        {
            _stateMachine = stateMachine;
            _hudService = hudService;
        }

        public async void Enter()
        {
            _hudService.ShowLooseWindow();
            
            await Task.Delay(_windowShowingTime);
            
            _stateMachine.SetState(typeof(GameSessionState));
        }

        public void Exit()
        {
            _hudService.HideLooseWindow();
        }
    }
}