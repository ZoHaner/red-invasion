using System;
using UnityEngine;

namespace Code.Player
{
    public class CameraRotationController
    {
        public Action<Quaternion> CameraRotated;
        private float _xRotation;

        public void Tick(Vector2 rotateVector, float mouseSensitivity, float deltaTime)
        {
            rotateVector *= mouseSensitivity * deltaTime;

            _xRotation -= rotateVector.y;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            var rotation = Quaternion.Euler(_xRotation, 0f, 0f);
            CameraRotated?.Invoke(rotation);
        }
    }
}