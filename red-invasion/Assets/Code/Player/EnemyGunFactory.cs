using System;
using Code.Input;
using Code.Services;
using Code.Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Player
{
    public class EnemyGunFactory
    {
        public Action<GunController> GunCreated { get; set; }

        private readonly IUpdateProvider _updateProvider;

        public EnemyGunFactory(IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;
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