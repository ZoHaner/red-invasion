using System.Threading.Tasks;
using Code.Enemies;
using UnityEngine;

namespace Code.Services
{
    public class GameFactory : IGameFactory
    {
        private const string EnemyAddress = "Enemy";
        private const string EnemiesPointsHolderAddress = "EnemiesPointsHolder";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;

        public GameFactory(IAssetProvider assetProvider, IUpdateProvider updateProvider)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
        }

        public async void SpawnEnemies()
        {
            var spawnPoints = await GetSpawnPoints();
            var prefab = await _assetProvider.Load<GameObject>(EnemyAddress);

            foreach (var spawnPoint in spawnPoints)
            {
                SpawnEnemy(spawnPoint, prefab);
            }
        }

        private void SpawnEnemy(EnemySpawnParams enemyParams, GameObject prefab)
        {
            var enemy = Object.Instantiate(prefab, enemyParams.SpawnPosition, enemyParams.SpawnRotation);
            var enemyMovement = enemy.GetComponent<EnemyMovementView>();
            enemyMovement.Construct(enemyParams.LeftMoveBorder, enemyParams.RightMoveBorder, enemyParams.Speed);
            enemyMovement.Initialize();
            RegisterUpdatableObject(enemyMovement);
        }

        private async Task<EnemySpawnParams[]> GetSpawnPoints()
        {
            var holder = await _assetProvider.Load<EnemiesPointsHolder>(EnemiesPointsHolderAddress);
            return holder.SpawnPoints;
        }

        private void RegisterUpdatableObject(IUpdatable updatable) =>
            _updateProvider.Register(updatable);
    }
}