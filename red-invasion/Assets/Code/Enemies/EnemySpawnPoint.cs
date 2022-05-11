using UnityEngine;

namespace Code.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [HideInInspector]
        public WalkingRange EnemyWalkingRange;
        public float EnemySpeed = 1f;
        public float ShootingRate = 1.5f;

        private Vector3 BorderGizmoSize = new Vector3(0.1f, 0.5f, 0.1f);

        private void OnDrawGizmos()
        {
            DrawBorders();
            DrawRangeLine();
            DrawPointPosition();
        }

        private void DrawBorders()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(EnemyWalkingRange.LeftBorder, BorderGizmoSize);
            Gizmos.DrawCube(EnemyWalkingRange.RightBorder, BorderGizmoSize);
        }

        private void DrawRangeLine()
        {
            Gizmos.color = Color.red;
            Vector3 direction = EnemyWalkingRange.RightBorder - EnemyWalkingRange.LeftBorder;
            Gizmos.DrawRay(EnemyWalkingRange.LeftBorder, direction);
        }

        private void DrawPointPosition()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}