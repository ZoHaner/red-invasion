using System;
using UnityEngine;

namespace Code.Player
{
    public class GunController 
    {
        public Action<Vector3, Vector3> Shoot;

        public void Tick(bool isAttackButtonPressed, Vector3 shootPosition, Vector3 shootDirection)
        {
            if (isAttackButtonPressed)
                Shoot?.Invoke(shootPosition, shootDirection);
        }
    }
}