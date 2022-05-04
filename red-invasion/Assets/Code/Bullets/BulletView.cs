using System;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public Action Collided;
        
        public void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            Collided?.Invoke();
        }
    }
}