using Code.Input;
using UnityEngine;

namespace Code.Shooting
{
    public class EnemyGunView : BaseGunView
    {
        private Transform _playerTransform;
        private Vector3 _playerCenterOffset;

        public void Construct(IAttackInput attackInput, GunController gunController, Transform playerTransform)
        {
            GunController = gunController;
            AttackInput = attackInput;
            _playerTransform = playerTransform;
            _playerCenterOffset = playerTransform.GetComponent<CharacterController>().center;
        }
        
        protected override Vector3 GetShootDirection()
        {
            return _playerTransform.position - ShootingPoint.position + _playerCenterOffset;
        }
    }
}