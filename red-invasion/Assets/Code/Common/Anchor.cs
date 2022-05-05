using UnityEngine;

namespace Code.Common
{
    public class Anchor<T> where T : MonoBehaviour
    {
        public static Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = new GameObject(nameof(T)).transform;

                return _transform;
            }
        }

        private static Transform _transform;
    }
}