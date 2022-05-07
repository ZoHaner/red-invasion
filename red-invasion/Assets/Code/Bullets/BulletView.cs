using UnityEngine;

namespace Code.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}