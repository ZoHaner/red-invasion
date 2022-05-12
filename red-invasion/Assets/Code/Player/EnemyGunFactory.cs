using System;
using System.Collections.Generic;
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

        private Dictionary<EnemyGunView, EnemyAttackInput> _viewInputs= new Dictionary<EnemyGunView, EnemyAttackInput>();

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

            _viewInputs[enemyGunView] = attackInput;

            GunCreated?.Invoke(gunController);
        }

        public void DisableEnemyGun(EnemyGunView enemyGunView)
        {
            _updateProvider.EnqueueUnregister(_viewInputs[enemyGunView]);
            _updateProvider.EnqueueUnregister(enemyGunView);
        }

        private float GetRandomValue()
            => Random.value;
    }
}