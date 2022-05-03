using System;
using Code.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Enemies
{
    public class EnemyMovementController
    {
        public Action<Vector3> Moved;
        private Vector3 _direction;

        private const float SpeedThreshold = 0.1f;

        public void CalculateNextPosition(Vector3 leftBorder, Vector3 rightBorder, Vector3 position, float speed, float deltaTime)
        {
            if (_direction == Vector3.zero)
                GenerateDirection(leftBorder, rightBorder);

            if(AlmostMotionless(speed))
                return;

            var newPos = position + speed * _direction * deltaTime;
            if (MathHelpers.OutOfBorders(position, leftBorder, rightBorder))
            {
                newPos = MathHelpers.GetClosestPointOnFiniteLine(position, leftBorder, rightBorder);
                FlipDirection();
            }
            
            Moved?.Invoke(newPos);
        }

        private static bool AlmostMotionless(float speed)
        {
            return Mathf.Abs(speed) <= SpeedThreshold;
        }

        private void GenerateDirection(Vector3 leftBorder, Vector3 rightBorder) =>
            _direction = Random.value > 0.5f ? leftBorder - rightBorder : rightBorder - leftBorder;

        private void FlipDirection() => 
            _direction *= -1;
    }
}