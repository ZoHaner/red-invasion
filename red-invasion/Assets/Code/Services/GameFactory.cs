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

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async void SpawnEnemies()
        {
            var spawnPositions = await GetSpawnPoints();
            var prefab = await _assetProvider.Load<GameObject>(EnemyAddress);

            foreach (var position in spawnPositions)
            {
                SpawnEnemy(position, prefab);
            }
        }

        private void SpawnEnemy(Vector3 point, GameObject prefab)
        {
            var enemy = Object.Instantiate(prefab);
            enemy.transform.position = point;
            var enemyMovement = enemy.GetComponent<EnemyMovementView>();
            enemyMovement.Initialize();
        }

        private async Task<Vector3[]> GetSpawnPoints()
        {
            var holder = await _assetProvider.Load<EnemiesPointsHolder>(EnemiesPointsHolderAddress);
            return holder.SpawnPoints;
        }
    }
}