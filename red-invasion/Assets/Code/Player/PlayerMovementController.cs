using System;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovementController
    {
        public Action<Vector3> DeltaMoved;
        
        public void Tick(Vector3 right, Vector3 forward, float speed, Vector2 moveVector, float deltaTime)
        {
            Vector3 move = right * moveVector.x + forward * moveVector.y;
            var deltaMove = move * speed * deltaTime;
            DeltaMoved?.Invoke(deltaMove);
        }
    }
}