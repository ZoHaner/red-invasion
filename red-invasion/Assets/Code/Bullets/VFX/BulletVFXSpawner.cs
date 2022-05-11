using UnityEngine;

namespace Code.Bullets.VFX
{
    public class BulletVFXSpawner
    {
        private readonly BulletVFXPool _bulletVFXPool;

        public BulletVFXSpawner(BulletVFXPool bulletVFXPool)
        {
            _bulletVFXPool = bulletVFXPool;
        }

        public void SpawnBulletVFX(Vector3 position)
        {
            var bulletVFXView = _bulletVFXPool.GetBulletVFX(position);
            bulletVFXView.ParticlesStopped += ReleaseBulletVFX;
        }

        private void ReleaseBulletVFX(BulletVFXView bulletVFXView)
        {
            bulletVFXView.ParticlesStopped -= ReleaseBulletVFX;
            _bulletVFXPool.ReleaseBulletVFX(bulletVFXView);
        }
    }
}