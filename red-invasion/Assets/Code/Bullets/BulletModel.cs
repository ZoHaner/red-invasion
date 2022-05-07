using UnityEngine;

namespace Code.Bullets
{
    public class BulletModel
    {
        public Vector3 Position;
        public Vector3 Direction;
        public readonly float Speed;
        public readonly float Radius;

        public BulletModel(Vector3 position, Vector3 direction, float speed, float radius)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
        }
    }
}