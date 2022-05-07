using Code.Input;
using Code.Services;
using UnityEngine;

namespace Code.Player
{
    public class GunView : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Transform _gunTip;

        private IInputService _inputService;
        private GunController _gunController;

        public void Construct(IInputService inputService, GunController gunController)
        {
            _gunController = gunController;
            _inputService = inputService;
        }

        public void Tick(float deltaTime)
        {
            _gunController.Tick(_inputService.IsAttackButtonPressed(), _gunTip.position, _gunTip.forward);
        }
    }
}