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

        public void CalculateNextPosition(WalkingRange range, Vector3 position, float speed, float deltaTime)
        {
            if (_direction == Vector3.zero)
                GenerateDirection(range);

            if(AlmostMotionless(speed))
                return;

            var newPos = position + speed * _direction * deltaTime;
            if (MathHelpers.OutOfBorders(position, range.LeftBorder, range.RightBorder))
            {
                newPos = MathHelpers.GetClosestPointOnFiniteLine(position, range.LeftBorder, range.RightBorder);
                FlipDirection();
            }
            
            Moved?.Invoke(newPos);
        }

        private static bool AlmostMotionless(float speed)
        {
            return Mathf.Abs(speed) <= SpeedThreshold;
        }

        private void GenerateDirection(WalkingRange rng) =>
            _direction = Random.value > 0.5f ? rng.LeftBorder - rng.RightBorder : rng.RightBorder - rng.LeftBorder;

        private void FlipDirection() => 
            _direction *= -1;
    }
}