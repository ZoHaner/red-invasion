using System.Threading.Tasks;
using Code.Bullets;
using Code.Enemies;
using Code.Input;
using Code.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services
{
    public class GameSession : IGameSession
    {
        private BulletFactory _bulletFactory;

        private const string EnemyAddress = "Enemy";
        private const string PlayerAddress = "Player";
        private const string EnemiesPointsHolderAddress = "EnemiesPointsHolder";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly IInputService _inputService;
        private readonly BulletsCollisionHandler _bulletsCollisionHandler;

        private GameObject _playerPrefab;
        private GameObject _enemyPrefab;

        public GameSession(IAssetProvider assetProvider, IUpdateProvider updateProvider, IInputService inputService, BulletsCollisionHandler bulletsCollisionHandler)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            _inputService = inputService;
            _bulletsCollisionHandler = bulletsCollisionHandler;
        }

        public async void Initialize()
        {
            _bulletFactory = new BulletFactory(_assetProvider, _updateProvider);
            await _bulletFactory.WarmUp();
        }

        public async Task WarmUp()
        {
            _playerPrefab = await _assetProvider.Load<GameObject>(PlayerAddress);
            _enemyPrefab = await _assetProvider.Load<GameObject>(EnemyAddress);
        }

        public void SpawnPlayer()
        {
            var player = Object.Instantiate(_playerPrefab, Vector3.up, Quaternion.identity);

            var playerCamera = player.GetComponent<CameraRotationView>();
            playerCamera.Construct(_inputService);
            playerCamera.Initialize();

            var playerBody = player.GetComponent<BodyRotationView>();
            playerBody.Construct(_inputService);
            playerBody.Initialize();

            var playerMovement = player.GetComponent<PlayerMovementView>();
            playerMovement.Construct(_inputService);
            playerMovement.Initialize();

            ConfigureGun(player);

            RegisterUpdatableObject(playerCamera);
            RegisterUpdatableObject(playerBody);
            RegisterUpdatableObject(playerMovement);
        }

        private void ConfigureGun(GameObject player)
        {
            var gunController = new GunController();
            gunController.Shoot += SpawnBullet;

            var gunView = player.GetComponentInChildren<GunView>();
            gunView.Construct(_inputService, gunController);

            RegisterUpdatableObject(gunView);
        }


        private void SpawnBullet(Vector3 position, Vector3 direction)
        {
            var bulletView = _bulletFactory.CreateBullet(position, direction);
            bulletView.Collided += OnBulletCollided;
        }

        private void OnBulletCollided(BulletView bulletView, GameObject collidedObject)
        {
            bulletView.Collided -= OnBulletCollided;
            _bulletFactory.DestroyBullet(bulletView);
        }


        public async void SpawnEnemies()
        {
            var spawnPoints = await GetSpawnPoints();

            foreach (var spawnPoint in spawnPoints)
            {
                SpawnEnemy(spawnPoint, _enemyPrefab);
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
            _updateProvider.EnqueueRegister(updatable);
    }
}