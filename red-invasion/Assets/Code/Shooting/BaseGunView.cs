using Code.Input;
using Code.Services;
using UnityEngine;

namespace Code.Player
{
    public abstract class BaseGunView : MonoBehaviour, IUpdatable
    {
        [SerializeField] protected Transform ShootingPoint;

        protected IAttackInput AttackInput;
        protected GunController GunController;

        public void Tick(float deltaTime)
        {
            GunController.Tick(AttackInput.IsAttackButtonPressed(), ShootingPoint.position, GetShootDirection());
        }

        protected abstract Vector3 GetShootDirection();
    }
    
    public class PlayerGunView : BaseGunView
    {
        public void Construct(IAttackInput attackInput, GunController gunController)
        {
            GunController = gunController;
            AttackInput = attackInput;
        }
        
        protected override Vector3 GetShootDirection() => 
            ShootingPoint.forward;
    }
    
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