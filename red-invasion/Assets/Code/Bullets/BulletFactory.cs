using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Common;
using Code.Services;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Code.Bullets
{
    public class BulletFactory
    {
        public Action<BulletController> BulletCreated;
        public Action<BulletController> BulletReleased;
        
        private const string BulletParamsAddress = "Bullet Parameters";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;

        private ObjectPool<BulletView> _bullets;
        private GameObject _bulletPrefab;

        private Dictionary<BulletController, BulletView> _bulletComponents = new Dictionary<BulletController, BulletView>();
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

        public BulletController GetBullet(Vector3 position, Vector3 direction)
        {
            var bullet = _bullets.Get();
            var bulletController = ConfigureBullet(bullet, position, direction);
            BulletCreated?.Invoke(bulletController);
            return bulletController;
        }

        public void ReleaseBullet(BulletController bulletController)
        {
            if (_bulletComponents.TryGetValue(bulletController, out var bulletView))
            {
                bulletController.PositionChanged += bulletView.Move;
                _bulletComponents.Remove(bulletController);
                _bullets.Release(bulletView);
                bulletView.gameObject.SetActive(false);

                BulletReleased?.Invoke(bulletController);
                
                _updateProvider.EnqueueUnregister(bulletController);
            }
            else
            {
                Debug.LogError("Such bullet view wasn't registered");
            }
        }

        private BulletView InstantiateBullet() =>
            Object.Instantiate(_bulletPrefab, Anchor<BulletView>.Transform).GetComponent<BulletView>();

        private BulletController ConfigureBullet(BulletView bulletView, Vector3 position, Vector3 direction)
        {
            var bulletController = new BulletController(new BulletModel(position, direction, _bulletParams.Speed, _bulletParams.Radius), _bulletParams.CollisionLayerMask);
            bulletController.PositionChanged += bulletView.Move;
            _bulletComponents[bulletController] = bulletView;
            bulletView.gameObject.SetActive(true);

            _updateProvider.EnqueueRegister(bulletController);
            return bulletController;
        }
    }
}