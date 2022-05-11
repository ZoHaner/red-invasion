using System.Threading.Tasks;
using Code.Player;
using Code.Services;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Enemies
{
    public class EnemyFactory
    {
        private const string EnemiesPointsHolderAddress = "EnemiesPointsHolder";
        private const string EnemyAddress = "Enemy";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly EnemyGunFactory _gunFactory;

        private EnemySpawnParams[] _spawnPoints;
        private GameObject _enemyPrefab;

        private ObjectPool<EnemyMovementView> _enemyPool;
        private Transform _playerTransform;

        public EnemyFactory(IAssetProvider assetProvider, IUpdateProvider updateProvider, EnemyGunFactory gunFactory)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            _gunFactory = gunFactory;
        }

        public async Task WarmUp()
        {
            _spawnPoints = await GetSpawnPoints();
            _enemyPrefab = await _assetProvider.Load<GameObject>(EnemyAddress);
        }

        public void Initialize()
        {
            _enemyPool = new ObjectPool<EnemyMovementView>(InstantiateEnemy);
        }

        public void SetPlayerTransform(Transform playerTransform) =>
            _playerTransform = playerTransform;

        public EnemyMovementView[] SpawnEnemiesAtSpawnPoints()
        {
            var enemies = new EnemyMovementView[_spawnPoints.Length];
            for (var i = 0; i < _spawnPoints.Length; i++)
            {
                enemies[i] = ConfigureEnemy(_spawnPoints[i]);
            }

            return enemies;
        }

        public void ReleaseEnemy(EnemyMovementView enemyView)
        {
            _updateProvider.EnqueueUnregister(enemyView);
            enemyView.gameObject.SetActive(false);
        }

        private EnemyMovementView InstantiateEnemy() =>
            Object.Instantiate(_enemyPrefab).GetComponent<EnemyMovementView>();

        private EnemyMovementView ConfigureEnemy(EnemySpawnParams enemyParams)
        {
            var enemyView = _enemyPool.Get();
            enemyView.transform.SetPositionAndRotation(enemyParams.SpawnPosition, enemyParams.SpawnRotation);
            enemyView.Construct(enemyParams.LeftMoveBorder, enemyParams.RightMoveBorder, enemyParams.Speed);
            enemyView.Initialize();

            _gunFactory.ConfigureEnemyGun(enemyView.gameObject, enemyParams.ShootingRate, _playerTransform);

            enemyView.gameObject.SetActive(true);
            _updateProvider.EnqueueRegister(enemyView);
            return enemyView;
        }

        private async Task<EnemySpawnParams[]> GetSpawnPoints()
        {
            var holder = await _assetProvider.Load<EnemiesPointsHolder>(EnemiesPointsHolderAddress);
            return holder.SpawnPoints;
        }
    }
}