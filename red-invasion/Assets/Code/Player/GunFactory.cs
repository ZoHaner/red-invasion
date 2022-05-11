using System;
using Code.Input;
using Code.Services;
using Code.Shooting;
using UnityEngine;

namespace Code.Player
{
    public class PlayerGunFactory
    {
        public Action<GunController> GunCreated { get; set; }

        private readonly IUpdateProvider _updateProvider;
        private readonly IAttackInput _attackInput;

        public PlayerGunFactory(IAttackInput attackInput, IUpdateProvider updateProvider)
        {
            _attackInput = attackInput;
            _updateProvider = updateProvider;
        }

        public PlayerGunView ConfigurePlayerGun(GameObject player)
        {
            var gunController = new GunController();

            var gunView = player.GetComponentInChildren<PlayerGunView>();
            gunView.Construct(_attackInput, gunController);

            _updateProvider.EnqueueRegister(gunView);
            GunCreated?.Invoke(gunController);
            return gunView;
        }
    }
}