using System.Collections.Generic;
using Code.Shooting;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletSpawner
    {
        private readonly BulletFactory _bulletFactory;
        private readonly HashSet<BulletController> _createdBullets = new HashSet<BulletController>();

        public BulletSpawner(BulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public void SubscribeOnGunShootEvent(GunController gunController)
        {
            gunController.Shoot += SpawnBullet;
        }

        public void ReleaseAllBullets()
        {
            foreach (var bullet in _createdBullets)
            {
                _bulletFactory.ReleaseBullet(bullet);
            }
            
            _createdBullets.Clear();
        }
        
        private void SpawnBullet(Vector3 position, Vector3 direction)
        {
            var bulletController = _bulletFactory.GetBullet(position, direction);
            _createdBullets.Add(bulletController);
            bulletController.Collided += OnBulletCollided;
        }

        private void OnBulletCollided(BulletController bulletController, Vector3 bulletPosition, Collider[] colliders)
        {
            bulletController.Collided -= OnBulletCollided;
            _createdBullets.Remove(bulletController);

            _bulletFactory.ReleaseBullet(bulletController);
        }
        
        
    }
}