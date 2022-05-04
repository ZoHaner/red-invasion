using System;
using UnityEngine;

namespace Code.Player
{
    public class GunController 
    {
        public Action<Vector3, Vector3> Shoot;

        public void Tick(bool isAttackButtonPressed, Vector3 gunTipPosition, Vector3 gunTipForward)
        {
            if (isAttackButtonPressed)
                Shoot?.Invoke(gunTipPosition, gunTipForward);
        }
    }
}