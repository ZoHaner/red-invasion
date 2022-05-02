using System;
using Code.Helpers;
using UnityEngine;

namespace Code.Enemies
{
    [Serializable]
    public class WalkingRange
    {
        public Vector3 LeftBorder { get; private set; }
        public Vector3 RightBorder { get; private set; }

        private readonly Vector3 _position;
        private readonly Vector3 _right;

        public WalkingRange(Vector3 position, Vector3 right)
        {
            _position = position;
            _right = right;
        }

        public void TrySetRightBorder(Vector3 newValue)
        {
            RightBorder = MathHelpers.GetClosestPointOnInfiniteLine(newValue, _position, _position + _right);
        }

        public void TrySetLeftBorder(Vector3 newValue)
        {
            LeftBorder = MathHelpers.GetClosestPointOnInfiniteLine(newValue, _position, _position - _right);
        }
    }
}