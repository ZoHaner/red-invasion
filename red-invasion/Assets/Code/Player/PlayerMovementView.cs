using Code.Enemies;
using Code.Input;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovementView : MonoBehaviour, IUpdatable
    {
        [SerializeField] private PlayerMovementParams playerMovementParams;
        [SerializeField] private Transform GroundChecker;

        private IInputService _inputService;
        private PlayerMovementController _playerMovementController;
        private CharacterController _characterController;

        public void Construct(IInputService inputService) =>
            _inputService = inputService;

        public void Initialize()
        {
            _playerMovementController = new PlayerMovementController();
            _characterController = GetComponent<CharacterController>();
            SubscribeOnEvents();
        }

        private void OnDestroy() =>
            UnsubscribeFromEvents();

        public void Tick(float deltaTime)
        {
            var moveVector = _inputService.GetMoveAxis();
            var jump = _inputService.GetJump();

            _playerMovementController.Tick(
                playerMovementParams,
                GroundChecker.position,
                transform.right,
                transform.forward,
                moveVector,
                jump,
                deltaTime);
        }

        private void ApplyMovement(Vector3 deltaMove)
        {
            _characterController.Move(deltaMove);
        }

        private void SubscribeOnEvents() =>
            _playerMovementController.DeltaMoved += ApplyMovement;

        private void UnsubscribeFromEvents() =>
            _playerMovementController.DeltaMoved -= ApplyMovement;
    }
}