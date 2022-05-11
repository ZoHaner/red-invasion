using Code.Shooting;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletSpawner
    {
        private readonly BulletFactory _bulletFactory;

        public BulletSpawner(BulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public void SubscribeOnGunShootEvent(GunController gunController)
        {
            gunController.Shoot += SpawnBullet;
        }

        private void SpawnBullet(Vector3 position, Vector3 direction)
        {
            var bulletController = _bulletFactory.GetBullet(position, direction);

            bulletController.Collided += OnBulletCollided;
        }

        private void OnBulletCollided(BulletController bulletController, Vector3 bulletPosition, Collider[] colliders)
        {
            bulletController.Collided -= OnBulletCollided;

            _bulletFactory.ReleaseBullet(bulletController);
        }
    }
}