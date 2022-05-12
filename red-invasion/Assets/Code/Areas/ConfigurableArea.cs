using UnityEngine;

namespace Code.Areas
{
    public class ConfigurableArea : MonoBehaviour
    {
        public AreaBounds AreaData;
        [SerializeField] private Color _gizmoColor;

        private void OnDrawGizmos()
        {
            if (AreaData == null)
                return;

            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(AreaData.Bounds.center, AreaData.Bounds.size);
        }
    }
}