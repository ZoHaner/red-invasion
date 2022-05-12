using UnityEngine;

namespace Code.Areas
{
    [CreateAssetMenu(menuName = "Settings/Create Area Bounds", fileName = "Area Bounds", order = 0)]
    public class AreaBounds : ScriptableObject
    {
        public Bounds Bounds;
    }
}