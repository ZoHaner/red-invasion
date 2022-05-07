using System.Threading.Tasks;
using Code.Services;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Bullets
{
    public class BulletVFXPool
    {
        private const string BulletVFXParamsAddress = "Bullet VFX Params";

        private readonly IAssetProvider _assetProvider;

        private ObjectPool<GameObject> _bulletVFXs;
        private GameObject _bulletVFXPrefab;
        private BulletVFXParams _bulletVFXParams;

        public BulletVFXPool(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void Initialize()
        {
            _bulletVFXs = new ObjectPool<GameObject>(InstantiateVFX);
        }

        public async Task WarmUp()
        {
            _bulletVFXParams = await _assetProvider.Load<BulletVFXParams>(BulletVFXParamsAddress);
            _bulletVFXPrefab = await _assetProvider.Load<GameObject>(_bulletVFXParams.PrefabReference);
        }

        public GameObject GetBulletVFX(Vector3 position)
        {
            var bullet = _bulletVFXs.Get();
            ConfigureBulletVFX(bullet, position);
            return bullet;
        }

        private void ConfigureBulletVFX(GameObject vfx, Vector3 position)
        {
            vfx.transform.position = position;
        }

        public void ReleaseBulletVFX(GameObject bulletVFX)
        {
            _bulletVFXs.Release(bulletVFX);
            bulletVFX.gameObject.SetActive(false);
        }

        private GameObject InstantiateVFX()
        {
            return Object.Instantiate(_bulletVFXPrefab);
        }
    }
}