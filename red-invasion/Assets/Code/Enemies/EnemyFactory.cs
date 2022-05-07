using System.Threading.Tasks;
using Code.Services;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyFactory
    {
        private const string EnemiesPointsHolderAddress = "EnemiesPointsHolder";
        private const string EnemyAddress = "Enemy";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;

        private EnemySpawnParams[] _spawnPoints;
        private GameObject _enemyPrefab;

        public EnemyFactory(IAssetProvider assetProvider, IUpdateProvider updateProvider)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
        }

        public async Task WarmUp()
        {
            _spawnPoints = await GetSpawnPoints();
            _enemyPrefab = await _assetProvider.Load<GameObject>(EnemyAddress);
        }

        public void SpawnEnemiesAtSpawnPoints()
        {
            foreach (var spawnPoint in _spawnPoints) 
                SpawnEnemy(spawnPoint, _enemyPrefab);
        }

        private async Task<EnemySpawnParams[]> GetSpawnPoints()
        {
            var holder = await _assetProvider.Load<EnemiesPointsHolder>(EnemiesPointsHolderAddress);
            return holder.SpawnPoints;
        }

        private void SpawnEnemy(EnemySpawnParams enemyParams, GameObject prefab)
        {
            var enemy = Object.Instantiate(prefab, enemyParams.SpawnPosition, enemyParams.SpawnRotation);
            var enemyMovement = enemy.GetComponent<EnemyMovementView>();
            enemyMovement.Construct(enemyParams.LeftMoveBorder, enemyParams.RightMoveBorder, enemyParams.Speed);
            enemyMovement.Initialize();
            RegisterUpdatableObject(enemyMovement);
        }

        private void RegisterUpdatableObject(IUpdatable updatable) =>
            _updateProvider.EnqueueRegister(updatable);
    }
}