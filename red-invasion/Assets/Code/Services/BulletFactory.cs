using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Bullets;
using Code.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Services
{
    public class BulletFactory
    {
        private const string BulletParamsAddress = "Bullet Parameters";
        
        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;

        private ObjectPool<BulletView> _bullets;
        private GameObject _bulletPrefab;

        private Dictionary<BulletView, BulletController> _bulletComponents = new Dictionary<BulletView, BulletController>();
        private BulletParams _bulletParams;

        public BulletFactory(IAssetProvider assetProvider, IUpdateProvider updateProvider)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            Initialize();
        }

        private void Initialize()
        {
            _bullets = new ObjectPool<BulletView>(InstantiateBullet);
        }

        public async Task WarmUp()
        {
            _bulletParams = await _assetProvider.Load<BulletParams>(BulletParamsAddress);
            _bulletPrefab = await _assetProvider.Load<GameObject>(_bulletParams.PrefabReference);
        }


        public BulletView CreateBullet(Vector3 position, Vector3 direction)
        {
            var bullet = _bullets.Get();
            ConfigureBullet(bullet, position, direction);
            return bullet;
        }

        public void DestroyBullet(BulletView bulletToRelease)
        {
            if (_bulletComponents.TryGetValue(bulletToRelease, out var bulletController))
            {
                bulletController.PositionChanged -= bulletToRelease.Move;
                _bulletComponents.Remove(bulletToRelease);
                _bullets.Release(bulletToRelease);
                
                _updateProvider.EnqueueUnregister(bulletController);
            }
            else
            {
                Debug.LogError("Such bullet view wasn't registered");
            }
        }

        private BulletView InstantiateBullet() => 
            Object.Instantiate(_bulletPrefab, Anchor<BulletView>.Transform).GetComponent<BulletView>();

        private void ConfigureBullet(BulletView bulletView, Vector3 position, Vector3 direction)
        {
            var bulletController = new BulletController(new BulletModel(position, direction, _bulletParams.Speed, _bulletParams.Radius), _bulletParams.CollisionLayerMask);
            bulletController.PositionChanged += bulletView.Move;
            _bulletComponents[bulletView] = bulletController;
            
            _updateProvider.EnqueueRegister(bulletController);
        }
    }
}