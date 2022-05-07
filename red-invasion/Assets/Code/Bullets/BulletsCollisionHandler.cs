using System;
using System.Collections.Generic;
using Code.Bullets.VFX;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletsCollisionHandler
    {
        private readonly BulletVFXPool _bulletVFXPool;

        public BulletsCollisionHandler(BulletVFXPool bulletVFXPool)
        {
            _bulletVFXPool = bulletVFXPool;
        }

        private Dictionary<string, Action> _objectTagsHandlers = new Dictionary<string, Action>();
        private Action<Vector3> _bulletCollisionCallback;

        public void AddCollisionHandler(string tag, Action callback)
        {
            if (_objectTagsHandlers.ContainsKey(tag))
            {
                Debug.LogWarning($"Handler for object tag '{tag}' has already exist and will be overwritten!");
            }
            
            _objectTagsHandlers[tag] = callback;
        }

        public void SetBulletCollisionCallback(Action<Vector3> callback)
        {
            _bulletCollisionCallback = callback;
        }
        
        public void OnBulletCollided(BulletController bulletController, Vector3 bulletPosition, Collider[] colliders)
        {
            foreach (var collider in colliders)
            {
                PerformActionForCollidedObject(LayerMask.LayerToName(collider.gameObject.layer));
            }

            PerformActionForBullet(bulletPosition);
        }

        private void PerformActionForCollidedObject(string layerName)
        {
            if (_objectTagsHandlers.TryGetValue(layerName, out var action))
            {
                action.Invoke();
            }
        }

        private void PerformActionForBullet(Vector3 bulletPosition)
        {
            _bulletCollisionCallback.Invoke(bulletPosition);
        }
    }
}