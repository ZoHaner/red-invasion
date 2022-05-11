using UnityEngine;

namespace Code.Damage
{
    public interface IHittable
    {
        void Hit(Vector3 hitDirection);
    }
}