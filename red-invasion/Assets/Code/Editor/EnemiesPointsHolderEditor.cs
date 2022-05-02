using Code.Enemies;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(EnemiesPointsHolder))]
    public class EnemiesPointsHolderEditor: UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EnemiesPointsHolder targetMenuButton = (EnemiesPointsHolder)target;

            if (GUILayout.Button("Rewrite spawn positions"))
            {
                targetMenuButton.CollectSpawnPointPositionsOnScene();
            }

            EditorUtility.SetDirty(target);

            // DrawDefaultInspector();
        }
    }
}