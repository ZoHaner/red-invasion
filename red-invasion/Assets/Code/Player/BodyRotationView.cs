using Code.Input;
using Code.Services;
using UnityEngine;

namespace Code.Player
{
    public class BodyRotationView : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float MouseSensitivity = 100f;
        [SerializeField] private Transform Body;

        private IInputService _inputService;

        private BodyRotationController _bodyRotationController;

        public void Construct(IInputService inputService) =>
            _inputService = inputService;

        public void Initialize()
        {
            _bodyRotationController = new BodyRotationController();
            SubscribeOnEvents();
        }

        private void OnDestroy() =>
            UnsubscribeFromEvents();

        public void Tick(float deltaTime)
        {
            var rotateVector = _inputService.GetLookAxis();
            _bodyRotationController.Tick(rotateVector, MouseSensitivity, deltaTime);
        }

        private void ApplyRotation(Quaternion rotation) => 
            Body.localRotation = rotation;

        private void SubscribeOnEvents() =>
            _bodyRotationController.BodyRotated += ApplyRotation;

        private void UnsubscribeFromEvents() =>
            _bodyRotationController.BodyRotated -= ApplyRotation;
    }
}