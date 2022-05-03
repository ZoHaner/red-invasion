using System;
using Code.Helpers;
using UnityEngine;

namespace Code.Enemies
{
    [Serializable]
    public class WalkingRange
    {
        private const float DefaultBorderDistance = 2f;
        public bool Initialized => _initialized;
        public Vector3 LeftBorder => _leftBorder;
        public Vector3 RightBorder => _rightBorder;

        [SerializeField, HideInInspector] private Vector3 _leftBorder;
        [SerializeField, HideInInspector] private Vector3 _rightBorder;
        [SerializeField, HideInInspector] private bool _initialized;

        [SerializeField, HideInInspector] private Vector3 _position;
        [SerializeField, HideInInspector] private Vector3 _right;
        [SerializeField, HideInInspector] private float _minRange;
        [SerializeField, HideInInspector] private float _maxRange;

        public WalkingRange(Vector3 position, Vector3 right, float minRange = 0.5f, float maxRange = 10f)
        {
            Initialize(position, right, minRange, maxRange);
        }

        public void Initialize(Vector3 position, Vector3 right, float minRange = 0.5f, float maxRange = 10f)
        {
            _position = position;
            _right = right;
            _minRange = minRange;
            _maxRange = maxRange;

            TrySetLeftBorder(_position - right * DefaultBorderDistance);
            TrySetRightBorder(_position + right * DefaultBorderDistance);

            _initialized = true;
        }

        public void ShiftRange(Vector3 newPosition)
        {
            var deltaPosition = newPosition - _position;
            _position = newPosition;
            _leftBorder += deltaPosition;
            _rightBorder += deltaPosition;
        }

        public void RotateRange(Vector3 newRight, Quaternion deltaRotation)
        {
            _right = newRight;
            _leftBorder = MathHelpers.RotatePointAroundPivot(_leftBorder, _position, deltaRotation);
            _rightBorder = MathHelpers.RotatePointAroundPivot(_rightBorder, _position, deltaRotation);
        }

        public void TrySetRightBorder(Vector3 newValue)
        {
            _rightBorder = MathHelpers.GetClosestPointOnFiniteLine(newValue, _position + _right * _minRange, _position + _right * _maxRange);
        }

        public void TrySetLeftBorder(Vector3 newValue)
        {
            _leftBorder = MathHelpers.GetClosestPointOnFiniteLine(newValue, _position - _right * _minRange, _position - _right * _maxRange);
        }
    }
}