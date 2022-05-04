using UnityEngine;

namespace Code.Bullets
{
    public class BulletModel
    {
        public Vector3 Position;
        public Vector3 Direction;
        public readonly float Speed;

        public BulletModel(Vector3 position, Vector3 direction, float speed)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
        }
    }
}