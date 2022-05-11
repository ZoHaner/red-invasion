using Code.Input;
using Code.Services;
using UnityEngine;

namespace Code.Shooting
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
}