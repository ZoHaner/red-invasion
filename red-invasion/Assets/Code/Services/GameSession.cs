using System.Threading.Tasks;
using Code.Bullets;
using Code.Bullets.VFX;
using Code.Damage;
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

        private const string PlayerAddress = "Player";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly IInputService _inputService;

        private GameObject _playerPrefab;

        private BulletsCollisionHandler _bulletsCollisionHandler;
        private BulletVFXPool _bulletVFXPool;
        private EnemyFactory _enemyFactory;
        private DamageProvider _damageProvider;
        
        private EnemyMovementView[] _enemies;

        public GameSession(IAssetProvider assetProvider, IUpdateProvider updateProvider, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            _inputService = inputService;
        }

        public async Task WarmUp()
        {
            _playerPrefab = await _assetProvider.Load<GameObject>(PlayerAddress);
            
            _enemyFactory = new EnemyFactory(_assetProvider, _updateProvider);
            _enemyFactory.Initialize();
            await _enemyFactory.WarmUp();

            _bulletFactory = new BulletFactory(_assetProvider, _updateProvider);
            await _bulletFactory.WarmUp();

            _bulletVFXPool = new BulletVFXPool(_assetProvider);
            _bulletVFXPool.Initialize();
            await _bulletVFXPool.WarmUp();
        }

        public void Initialize()
        {
            _damageProvider = new DamageProvider();
            _bulletsCollisionHandler = new BulletsCollisionHandler(_bulletVFXPool);
            _bulletsCollisionHandler.SetBulletCollisionCallback(SpawnBulletVFX);
            // _bulletsCollisionHandler.AddCollisionHandler("Enemies", OnEnemyHit);
        }

        public void Cleanup()
        {
            _enemies = null;
        }

        private void OnEnemyHit(EnemyMovementView enemyMovementView)
        {
            enemyMovementView.Hitted -= OnEnemyHit;
            _enemyFactory.ReleaseEnemy(enemyMovementView);
        }

        public void Start()
        {
            _enemies = _enemyFactory.SpawnEnemiesAtSpawnPoints();
            foreach (var enemy in _enemies)
            {
                enemy.Hitted += OnEnemyHit;
            }
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
            var bulletController = _bulletFactory.GetBullet(position, direction);

            bulletController.Collided += OnBulletCollided;
            bulletController.Collided += _damageProvider.TryApplyHit;
            bulletController.Collided += _bulletsCollisionHandler.OnBulletCollided;
        }

        private void SpawnBulletVFX(Vector3 position)
        {
            var bulletVFXView = _bulletVFXPool.GetBulletVFX(position);
            bulletVFXView.ParticlesStopped += ReleaseBulletVFX;
        }

        private void ReleaseBulletVFX(BulletVFXView bulletVFXView)
        {
            bulletVFXView.ParticlesStopped -= ReleaseBulletVFX;
            _bulletVFXPool.ReleaseBulletVFX(bulletVFXView);
        }

        private void OnBulletCollided(BulletController bulletController, Vector3 bulletPosition, Collider[] colliders)
        {
            bulletController.Collided -= OnBulletCollided;
            bulletController.Collided -= _damageProvider.TryApplyHit;
            bulletController.Collided -= _bulletsCollisionHandler.OnBulletCollided;

            _bulletFactory.ReleaseBullet(bulletController);
        }

        private void RegisterUpdatableObject(IUpdatable updatable) =>
            _updateProvider.EnqueueRegister(updatable);
    }
}