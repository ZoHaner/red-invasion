using Code.Input;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovementView : MonoBehaviour
    {
        [SerializeField] protected float Speed = 12f;

        private IInputService _inputService;
        private PlayerMovementController _playerMovementController;
            
        public void Construct(IInputService inputService) => 
            _inputService = inputService;

        public void Initialize()
        {
            _playerMovementController = new PlayerMovementController();
            SubscribeOnEvents();
        }

        private void OnDestroy() => 
            UnsubscribeFromEvents();

        public void Tick(float deltaTime)
        {
            var moveVector = _inputService.GetMoveAxis();
            _playerMovementController.Tick(transform.right, transform.forward, Speed, moveVector, deltaTime);
        }

        private void ApplyMovement(Vector3 deltaMove)
        {
            
        }

        private void SubscribeOnEvents() => 
            _playerMovementController.DeltaMoved += ApplyMovement;

        private void UnsubscribeFromEvents() => 
            _playerMovementController.DeltaMoved -= ApplyMovement;
    }
}