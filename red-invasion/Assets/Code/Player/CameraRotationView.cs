using Code.Input;
using UnityEngine;

namespace Code.Player
{
    public class CameraRotationView : MonoBehaviour
    {
        [SerializeField] private float MouseSensitivity = 100f;
        [SerializeField] private Transform Camera;

        private IInputService _inputService;

        private CameraRotationController _cameraRotationController;

        public void Construct(IInputService inputService) =>
            _inputService = inputService;

        public void Initialize()
        {
            _cameraRotationController = new CameraRotationController();
            SubscribeOnEvents();
        }

        private void OnDestroy() =>
            UnsubscribeFromEvents();

        public void Tick(float deltaTime)
        {
            var rotateVector = _inputService.GetMoveAxis();
            _cameraRotationController.Tick(rotateVector, MouseSensitivity, deltaTime);
        }

        private void ApplyRotation(Quaternion rotation) => 
            Camera.localRotation = rotation;

        private void SubscribeOnEvents() =>
            _cameraRotationController.CameraRotated += ApplyRotation;

        private void UnsubscribeFromEvents() =>
            _cameraRotationController.CameraRotated -= ApplyRotation;
    }
}