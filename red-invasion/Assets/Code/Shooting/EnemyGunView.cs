using Code.Input;
using UnityEngine;

namespace Code.Shooting
{
    public class EnemyGunView : BaseGunView
    {
        private Transform _playerTransform;

        public void Construct(IAttackInput attackInput, GunController gunController, Transform playerTransform)
        {
            GunController = gunController;
            AttackInput = attackInput;
            _playerTransform = playerTransform;
        }
        
        protected override Vector3 GetShootDirection()
        {
            return _playerTransform.position - ShootingPoint.position;
        }
    }
}