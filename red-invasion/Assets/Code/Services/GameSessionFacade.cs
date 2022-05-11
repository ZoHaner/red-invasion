using System;
using System.Threading.Tasks;
using Code.Areas;
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
        public Action WinGame { get; set; }
        public Action LooseGame { get; set; }

        private const string PlayerBulletParamsAddress = "Player Bullet Parameters";
        private const string EnemyBulletParamsAddress = "Enemy Bullet Parameters";

        private const string WinAreaBoundsAddress = "Win Area Bounds";
        private const string LooseAreaBoundsAddress = "Loose Area Bounds";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly IInputService _inputService;

        private BulletsCollisionHandler _bulletsCollisionHandler;
        private EnemyFactory _enemyFactory;
        private BulletFactory _playerBulletFactory;
        private BulletFactory _enemyBulletFactory;
        private PlayerFactory _playerFactory;
        private PlayerGunFactory _playerGunFactory;
        private EnemyGunFactory _enemyGunFactory;
        private BulletSpawner _bulletSpawner;
        private EnemySpawner _enemySpawner;
        private BulletVFXSpawner _bulletVFXSpawner;
        private BulletVFXPool _bulletVFXPool;
        private DamageProvider _damageProvider;
        private BulletSpawner _playerBulletSpawner;
        private BulletSpawner _enemyBulletSpawner;

        private AreaEnteringTracker _winAreaEnteringTracker;
        private AreaEnteringTracker _looseAreaEnteringTracker;

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

            _playerBulletFactory = new BulletFactory(PlayerBulletParamsAddress, _assetProvider, _updateProvider);
            await _playerBulletFactory.WarmUp();

            _enemyBulletFactory = new BulletFactory(EnemyBulletParamsAddress, _assetProvider, _updateProvider);
            await _enemyBulletFactory.WarmUp();

            _bulletVFXPool = new BulletVFXPool(_assetProvider);
            _bulletVFXPool.Initialize();
            await _bulletVFXPool.WarmUp();

            _winAreaEnteringTracker = new AreaEnteringTracker(WinAreaBoundsAddress, _assetProvider);
            await _winAreaEnteringTracker.Warmup();

            _looseAreaEnteringTracker = new AreaEnteringTracker(LooseAreaBoundsAddress, _assetProvider);
            await _looseAreaEnteringTracker.Warmup();
        }

        public void Initialize()
        {
            _bulletVFXSpawner = new BulletVFXSpawner(_bulletVFXPool);

            _damageProvider = new DamageProvider();
            _bulletsCollisionHandler = new BulletsCollisionHandler(_bulletVFXPool);
            _bulletsCollisionHandler.SetBulletCollisionCallback(_bulletVFXSpawner.SpawnBulletVFX);

            _enemySpawner = new EnemySpawner(_enemyFactory);

            _playerBulletSpawner = new BulletSpawner(_playerBulletFactory);
            _enemyBulletSpawner = new BulletSpawner(_enemyBulletFactory);

            _playerGunFactory.GunCreated += _playerBulletSpawner.SubscribeOnGunShootEvent;
            _enemyGunFactory.GunCreated += _enemyBulletSpawner.SubscribeOnGunShootEvent;

            _enemyBulletFactory.BulletCreated += _damageProvider.SubscribeOnBulletCollidedEvent;
            _enemyBulletFactory.BulletReleased += _damageProvider.UnsubscribeFromBulletCollidedEvent;
            _playerBulletFactory.BulletCreated += _damageProvider.SubscribeOnBulletCollidedEvent;
            _playerBulletFactory.BulletReleased += _damageProvider.UnsubscribeFromBulletCollidedEvent;

            _enemyBulletFactory.BulletCreated += _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _enemyBulletFactory.BulletReleased += _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;
            _playerBulletFactory.BulletCreated += _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _playerBulletFactory.BulletReleased += _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;

            _winAreaEnteringTracker.OnAreaEntered += () => WinGame?.Invoke();
            _looseAreaEnteringTracker.OnAreaEntered += () => LooseGame?.Invoke();
            
            _updateProvider.EnqueueRegister(_winAreaEnteringTracker);
            _updateProvider.EnqueueRegister(_looseAreaEnteringTracker);
        }

        public void SpawnPlayer()
        {
            var player = _playerFactory.SpawnPlayer();
            _enemyFactory.SetPlayerTransform(player.transform);
            _winAreaEnteringTracker.SetTargetTransform(player.transform);
            _looseAreaEnteringTracker.SetTargetTransform(player.transform);
        }

        public void SpawnEnemies()
        {
            _enemySpawner.SpawnEnemiesAtSpawnPoints();
        }

        public void Cleanup()
        {
            _playerGunFactory.GunCreated -= _playerBulletSpawner.SubscribeOnGunShootEvent;
            _enemyGunFactory.GunCreated -= _enemyBulletSpawner.SubscribeOnGunShootEvent;

            _enemyBulletFactory.BulletCreated -= _damageProvider.SubscribeOnBulletCollidedEvent;
            _enemyBulletFactory.BulletReleased -= _damageProvider.UnsubscribeFromBulletCollidedEvent;
            _playerBulletFactory.BulletCreated -= _damageProvider.SubscribeOnBulletCollidedEvent;
            _playerBulletFactory.BulletReleased -= _damageProvider.UnsubscribeFromBulletCollidedEvent;

            _enemyBulletFactory.BulletCreated -= _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _enemyBulletFactory.BulletReleased -= _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;
            _playerBulletFactory.BulletCreated -= _bulletsCollisionHandler.SubscribeOnBulletCollidedEvent;
            _playerBulletFactory.BulletReleased -= _bulletsCollisionHandler.UnsubscribeFromBulletCollidedEvent;
            
            _winAreaEnteringTracker.OnAreaEntered -= () => WinGame?.Invoke();
            _looseAreaEnteringTracker.OnAreaEntered -= () => LooseGame?.Invoke();
            
            _updateProvider.EnqueueUnregister(_winAreaEnteringTracker);
            _updateProvider.EnqueueUnregister(_looseAreaEnteringTracker);
        }
    }
}