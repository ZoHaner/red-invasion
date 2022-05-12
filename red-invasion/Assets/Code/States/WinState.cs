using Code.Services;
using Task = System.Threading.Tasks.Task;

namespace Code.States
{
    public class WinState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly HUDService _hudService;
        private int _windowShowingTime = 1000;

        public WinState(StateMachine stateMachine, HUDService hudService)
        {
            _stateMachine = stateMachine;
            _hudService = hudService;
        }

        public async void Enter()
        {
            _hudService.ShowWinWindow();
            
            await Task.Delay(_windowShowingTime);
            
            _stateMachine.SetState(typeof(GameSessionState));
        }

        public void Exit()
        {
            _hudService.HideWinWindow();
        }
    }
}