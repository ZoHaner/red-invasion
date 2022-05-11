using System.Threading.Tasks;
using Code.Bullets;
using Code.Bullets.VFX;
using Code.Damage;
using Code.Enemies;
using Code.Input;
using Code.Player;

namespace Code.Services
{
    public class GameSessionFacade : IGameSession
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly IInputService _inputService;

        private BulletsCollisionHandler _bulletsCollisionHandler;
        private EnemyFactory _enemyFactory;
        private BulletFactory _bulletFactory;
        private PlayerFactory _playerFactory;
        private PlayerGunFactory _playerGunFactory;
        private EnemyGunFactory _enemyGunFactory;
        private BulletSpawner _bulletSpawner;
        private EnemySpawner _enemySpawner;
        private BulletVFXSpawner _bulletVFXSpawner;
        private BulletVFXPool _bulletVFXPool;
        private DamageProvider _damageProvider;

        public GameSessionFacade(IAssetProvider assetProvider, IUpdateProvider updateProvider, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            _inputService = inputService;
        }

        public async Task WarmUp()
        {
            _playerGunFactory = new PlayerGunFactory(_inputService, _updateProvider);
            _enemyGunFactory = new EnemyGunFactory(_updateProvider);

            _playerFactory = new PlayerFactory(_inputService, _updateProvider, _assetProvider, _playerGunFactory);
            await _playerFactory.WarmUp();

            _enemyFactory = new EnemyFactory(_assetProvider, _updateProvider, _enemyGunFactory);
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
            _bulletVFXSpawner = new BulletVFXSpawner(_bulletVFXPool);

            _damageProvider = new DamageProvider();
            _bulletsCollisionHandler = new BulletsCollisionHandler(_bulletVFXPool);
            _bulletsCollisionHandler.SetBulletCollisionCallback(_bulletVFXSpawner.SpawnBulletVFX);

            _enemySpawner = new EnemySpawner(_enemyFactory);

            _bulletSpawner = new BulletSpawner(_bulletFactory);
            _playerGunFactory.GunCreated += _bulletSpawner.SubscribeOnGunShootEvent;
            _enemyGunFactory.GunCreated += _bulletSpawner.SubscribeOnGunShootEvent;

            _bulletFactory.BulletCreated += _damageProvider.SubscribeOnBulletCollidedEvent;
            _bulletFactory.BulletReleased += _damageProvider.UnsubscribeFromBulletCollidedEvent;

            _bulletFactory.BulletCreated += _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _bulletFactory.BulletReleased += _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;
        }

        public void SpawnPlayer()
        {
            var player = _playerFactory.SpawnPlayer();
            _enemyFactory.SetPlayerTransform(player.transform);
        }

        public void SpawnEnemies()
        {
            _enemySpawner.SpawnEnemiesAtSpawnPoints();
        }

        public void Cleanup()
        {
            _playerGunFactory.GunCreated -= _bulletSpawner.SubscribeOnGunShootEvent;
            _enemyGunFactory.GunCreated -= _bulletSpawner.SubscribeOnGunShootEvent;

            _bulletFactory.BulletCreated -= _damageProvider.SubscribeOnBulletCollidedEvent;
            _bulletFactory.BulletReleased -= _damageProvider.UnsubscribeFromBulletCollidedEvent;

            _bulletFactory.BulletCreated -= _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _bulletFactory.BulletReleased -= _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;
        }
    }
}