using System;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovementController
    {
        public Action<Vector3> DeltaMoved;

        private const float HitDecreaseCoefficient = 0.9f;

        private Vector3 _moveVector;
        private Vector3 _jumpVector;
        private Vector3 _hitVector;
        private bool _isGrounded;

        public void Tick(PlayerMovementParams moveParams, Vector3 groundCheckerPosition, Vector3 right, Vector3 forward, Vector2 inputVector, bool jump, float deltaTime)
        {
            CalculateMoveVector(right, forward, moveParams.Speed, inputVector, deltaTime);
            CheckIfPlayerOnGround(moveParams, groundCheckerPosition);
            ResetVerticalVelocityIfOnGround();
            DecreaseHitForce();

            if (CanJump(jump))
                ApplyJump(moveParams, deltaTime);

            ApplyGravity(moveParams, deltaTime);

            DeltaMoved?.Invoke(_moveVector);
            DeltaMoved?.Invoke(_jumpVector);
        }

        private void DecreaseHitForce()
        {
            if (_hitVector == Vector3.zero)
                return;

            _hitVector *= HitDecreaseCoefficient;

            if (_hitVector.sqrMagnitude <= 0.5f)
                _hitVector = Vector3.zero;
        }

        private void CalculateMoveVector(Vector3 right, Vector3 forward, float speed, Vector2 inputVector, float deltaTime)
        {
            Vector3 move = right * inputVector.x + forward * inputVector.y;
            _moveVector = (move * speed + _hitVector) * deltaTime;
        }

        private void CheckIfPlayerOnGround(PlayerMovementParams moveParams, Vector3 groundCheckerPosition)
        {
            _isGrounded = Physics.CheckSphere(
                groundCheckerPosition, moveParams.GroundDistance, moveParams.GroundLayer, QueryTriggerInteraction.Ignore);
        }

        private void ResetVerticalVelocityIfOnGround()
        {
            if (_isGrounded && _jumpVector.y < 0)
                _jumpVector.y = 0f;
        }

        private bool CanJump(bool jumpInput) =>
            jumpInput && _isGrounded;

        private void ApplyJump(PlayerMovementParams moveParams, float deltaTime) =>
            _jumpVector.y += Mathf.Sqrt(moveParams.JumpHeight * -2f * moveParams.Gravity) * deltaTime;

        private void ApplyGravity(PlayerMovementParams moveParams, float deltaTime) =>
            _jumpVector.y += moveParams.Gravity * deltaTime * deltaTime;

        public void AddForce(Vector3 hitDirection, float forceValue)
        {
            _hitVector += hitDirection * forceValue;
        }
    }
}