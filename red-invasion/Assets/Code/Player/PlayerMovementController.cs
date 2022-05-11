using System;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovementController
    {
        public Action<Vector3> DeltaMoved;

        private const float HitDecreaseCoefficient = 0.9f;

        private Vector3 _moveVector;
        private Vector3 _hitVector;

        private float _groundedTimer;
        private float _verticalVelocity;

        public void AddForce(Vector3 hitDirection, float forceValue) =>
            _hitVector += hitDirection * forceValue;

        public void Tick(PlayerMovementParams moveParams, Vector3 right, Vector3 forward, Vector2 inputVector, bool jump, bool groundedPlayer, float deltaTime)
        {
            CalculateMoveVector(right, forward, moveParams.Speed, inputVector, deltaTime);
            DecreaseHitForce();
            SetGroundedCooldown(groundedPlayer, deltaTime);
            ResetVerticalVelocityIfOnGround(groundedPlayer);
            ApplyGravity(moveParams.Gravity, deltaTime);
            if (jump)
            {
                CalculateVerticalVelocity(moveParams.JumpHeight, moveParams.Gravity);
            }

            ApplyVerticalVelocity();
            DeltaMoved?.Invoke(_moveVector);
        }

        private void CalculateMoveVector(Vector3 right, Vector3 forward, float speed, Vector2 inputVector, float deltaTime)
        {
            Vector3 move = right * inputVector.x + forward * inputVector.y;
            _moveVector = (move * speed + _hitVector) * deltaTime;
        }

        private void DecreaseHitForce()
        {
            if (_hitVector == Vector3.zero)
                return;

            _hitVector *= HitDecreaseCoefficient;

            if (_hitVector.sqrMagnitude <= 0.5f)
                _hitVector = Vector3.zero;
        }

        private void SetGroundedCooldown(bool groundedPlayer, float deltaTime)
        {
            if (groundedPlayer)
                _groundedTimer = 1f;

            if (_groundedTimer > 0)
                _groundedTimer -= deltaTime;
        }

        private void ResetVerticalVelocityIfOnGround(bool groundedPlayer)
        {
            if (groundedPlayer && _verticalVelocity < 0)
                _verticalVelocity = 0f;
        }

        private void ApplyGravity(float gravityValue, float deltaTime) =>
            _verticalVelocity -= gravityValue * deltaTime;

        private void CalculateVerticalVelocity(float jumpHeight, float gravityValue)
        {
            if (_groundedTimer > 0)
            {
                _groundedTimer = 0;
                _verticalVelocity += Mathf.Sqrt(jumpHeight * 2 * gravityValue);
            }
        }

        private void ApplyVerticalVelocity() => 
            _moveVector.y = _verticalVelocity;
    }
}