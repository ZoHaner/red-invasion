using System;
using Code.Input;
using Code.Services;
using Code.Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Player
{
    public class GunFactory
    {
        public Action<GunController> GunCreated;

        private readonly IUpdateProvider _updateProvider;
        private readonly IAttackInput _attackInput;

        public GunFactory(IAttackInput attackInput, IUpdateProvider updateProvider)
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

        public void ConfigureEnemyGun(GameObject enemy, float shootingRate, Transform playerTransform)
        {
            var enemyGunView = enemy.GetComponent<EnemyGunView>();
            var attackInput = new EnemyAttackInput(GetRandomValue() * shootingRate, shootingRate);
            var gunController = new GunController();

            enemyGunView.Construct(attackInput, gunController, playerTransform);
            _updateProvider.EnqueueRegister(attackInput);
            _updateProvider.EnqueueRegister(enemyGunView);

            GunCreated?.Invoke(gunController);
        }

        private float GetRandomValue()
            => Random.value;
    }
}