using Code.Enemies;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(EnemySpawnPoint))]
    public class EnemySpawnPointEditor : UnityEditor.Editor
    {
        private Vector3 _pointOldPosition;
        private Quaternion _pointOldRotation;

        private EnemySpawnPoint _spawnPoint;
        private Transform _pointTransform;

        protected virtual void OnSceneGUI()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = (EnemySpawnPoint)target;
                _pointTransform = _spawnPoint.transform;

                RememberPointParams();
            }

            if (_spawnPoint.EnemyWalkingRange == null)
                _spawnPoint.EnemyWalkingRange = new WalkingRange(_pointTransform.position, _pointTransform.right);

            if (!_spawnPoint.EnemyWalkingRange.Initialized)
                _spawnPoint.EnemyWalkingRange.Initialize(_pointTransform.position, _pointTransform.right);

            if (PointPositionChanged())
                ShiftRange(_pointTransform.position);

            if (PointRotationChanged())
                RotateRange(_pointTransform.right, _pointTransform.rotation * Quaternion.Inverse(_pointOldRotation));

            DrawAndCheckLeftBorderHandle();
            DrawAndCheckRightBorderHandle();

            RememberPointParams();
        }

        private void OnDisable() =>
            ClearVariables();

        private void OnDestroy() =>
            ClearVariables();


        private void DrawAndCheckRightBorderHandle()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newRightBorderPosition = Handles.PositionHandle(_spawnPoint.EnemyWalkingRange.RightBorder, _pointTransform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                _spawnPoint.EnemyWalkingRange.TrySetRightBorder(newRightBorderPosition);
                MarkTargetDirty();
            }
        }

        private void DrawAndCheckLeftBorderHandle()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newLeftBorderPosition = Handles.PositionHandle(_spawnPoint.EnemyWalkingRange.LeftBorder, _pointTransform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                _spawnPoint.EnemyWalkingRange.TrySetLeftBorder(newLeftBorderPosition);
                MarkTargetDirty();
            }
        }

        private void RotateRange(Vector3 newRight, Quaternion deltaRotation)
        {
            _spawnPoint.EnemyWalkingRange.RotateRange(newRight, deltaRotation);
            MarkTargetDirty();
        }

        private void ShiftRange(Vector3 deltaPosition)
        {
            _spawnPoint.EnemyWalkingRange.ShiftRange(deltaPosition);
            MarkTargetDirty();
        }

        private void RememberPointParams()
        {
            _pointOldPosition = _pointTransform.position;
            _pointOldRotation = _pointTransform.rotation;
        }

        private bool PointRotationChanged() =>
            _pointTransform.rotation != _pointOldRotation;

        private bool PointPositionChanged() =>
            _pointTransform.position != _pointOldPosition;

        private void ClearVariables()
        {
            _spawnPoint = null;
            _pointTransform = null;
        }

        private void MarkTargetDirty() =>
            EditorUtility.SetDirty(target);
    }
}