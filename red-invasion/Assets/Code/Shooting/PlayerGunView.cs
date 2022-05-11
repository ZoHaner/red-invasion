using Code.Input;
using UnityEngine;

namespace Code.Shooting
{
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
}