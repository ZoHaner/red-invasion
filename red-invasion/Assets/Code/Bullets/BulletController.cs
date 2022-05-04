using System;
using Code.Services;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletController : IUpdatable
    {
        public Action<Vector3> PositionChanged;
        
        private readonly BulletModel _model;

        public BulletController(BulletModel model)
        {
            _model = model;
        }

        public void Tick(float deltaTime) => 
            CalculateBulletModel(deltaTime);

        void CalculateBulletModel(float deltaTime)
        {
            _model.Position += _model.Direction * _model.Speed * deltaTime;
            PositionChanged?.Invoke(_model.Position);
        }
    }
}