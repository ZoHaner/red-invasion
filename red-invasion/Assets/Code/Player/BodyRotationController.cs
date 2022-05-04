using System;
using UnityEngine;

namespace Code.Player
{
    public class BodyRotationController
    {
        public Action<Quaternion> BodyRotated;
        private float _yRotation;
        
        public void Tick(Vector2 rotateVector, float mouseSensitivity, float deltaTime)
        {
            rotateVector *= mouseSensitivity * deltaTime;
            _yRotation += rotateVector.x;

            var rotation = Quaternion.Euler(0f, _yRotation, 0f);
            BodyRotated?.Invoke(rotation);
        }
    }
}