using System;
using UnityEngine;

namespace Code.Player
{
    [Serializable]
    public class PlayerMovementParams
    {
        public float Speed = 4f;
        public float JumpHeight = 1.5f;
        public float Gravity = -9.81f;
        public float GroundDistance = 0.2f;
        public LayerMask GroundLayer;
    }
}