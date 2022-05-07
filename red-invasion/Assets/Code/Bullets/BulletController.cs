using System;
using Code.Services;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletController : IUpdatable
    {
        public Action<Vector3> PositionChanged;
        public Action<BulletController, Collider[]> Collided;
        
        private readonly BulletModel _model;
        private readonly int _bulletCollisionMask;

        public BulletController(BulletModel model, int bulletCollisionMask)
        {
            _model = model;
            _bulletCollisionMask = bulletCollisionMask;
        }

        public void Tick(float deltaTime)
        {
            CalculateBulletModel(deltaTime);
            CheckCollisions();
        }

        void CalculateBulletModel(float deltaTime)
        {
            _model.Position += _model.Direction * _model.Speed * deltaTime;
            PositionChanged?.Invoke(_model.Position);
        }

        private void CheckCollisions()
        {
            var collisions = Physics.OverlapSphere(_model.Position, _model.Radius, _bulletCollisionMask, QueryTriggerInteraction.Ignore);
            
            if (collisions.Length != 0) 
                Collided?.Invoke(this, collisions);
        }
    }
}