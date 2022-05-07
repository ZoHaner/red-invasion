using System.Threading.Tasks;
using Code.Services;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Code.Bullets.VFX
{
    public class BulletVFXPool
    {
        private const string BulletVFXParamsAddress = "Bullet VFX Params";

        private readonly IAssetProvider _assetProvider;

        private ObjectPool<BulletVFXView> _bulletVFXs;
        private GameObject _bulletVFXPrefab;
        private BulletVFXParams _bulletVFXParams;

        public BulletVFXPool(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void Initialize()
        {
            _bulletVFXs = new ObjectPool<BulletVFXView>(InstantiateVFX);
        }

        public async Task WarmUp()
        {
            _bulletVFXParams = await _assetProvider.Load<BulletVFXParams>(BulletVFXParamsAddress);
            _bulletVFXPrefab = await _assetProvider.Load<GameObject>(_bulletVFXParams.PrefabReference);
        }

        public BulletVFXView GetBulletVFX(Vector3 position)
        {
            var bulletView = _bulletVFXs.Get();
            ConfigureBulletVFX(bulletView, position);
            bulletView.gameObject.SetActive(true);
            return bulletView;
        }

        private void ConfigureBulletVFX(BulletVFXView vfx, Vector3 position)
        {
            vfx.transform.position = position;
        }

        public void ReleaseBulletVFX(BulletVFXView bulletVFXView)
        {
            _bulletVFXs.Release(bulletVFXView);
            bulletVFXView.gameObject.SetActive(false);
        }

        private BulletVFXView InstantiateVFX()
        {
            var bulletVFXView = Object.Instantiate(_bulletVFXPrefab).GetComponent<BulletVFXView>();
            bulletVFXView.Initialize();
            return bulletVFXView;
        }
    }
}