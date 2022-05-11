using Code.Bullets;
using UnityEngine;

namespace Code.Damage
{
    public class DamageProvider
    {
        public void SubscribeOnBulletCollidedEvent(BulletController bulletController)
        {
            bulletController.Collided += TryApplyHit;
        }
        
        public void UnsubscribeFromBulletCollidedEvent(BulletController bulletController)
        {
            bulletController.Collided -= TryApplyHit;
        }

        private void TryApplyHit(BulletController controller, Vector3 bulletPosition, Collider[] colliders)
        {
            foreach (var collider in colliders)
                TryApplyHit(collider.gameObject);
        }

        private void TryApplyHit(GameObject collidedObject) =>
            collidedObject.GetComponent<IHittable>()?.Hit();
    }
}