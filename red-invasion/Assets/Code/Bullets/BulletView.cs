using System;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public Action<BulletView, GameObject> Collided;
        
        public void Move(Vector3 position)
        {
            transform.position = position;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision Happened with " + other.transform.name);
            Collided?.Invoke(this, other.gameObject);
        }
    }
}