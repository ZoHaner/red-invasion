using Code.Areas;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(ConfigurableArea))]
    public class ConfigurableAreaEditor : UnityEditor.Editor
    {
        protected virtual void OnSceneGUI()
        {
            var winningArea = (ConfigurableArea)target;

            if(winningArea.AreaData == null)
                return;
            
            winningArea.AreaData.Bounds.center = winningArea.transform.position;

            EditorGUI.BeginChangeCheck();
            
            Vector3 scale = Handles.ScaleHandle(winningArea.AreaData.Bounds.size, winningArea.AreaData.Bounds.center, Quaternion.identity, 1);
            
            if (EditorGUI.EndChangeCheck())
            {
                winningArea.AreaData.Bounds.size = scale;
                MarkDirty(winningArea.AreaData);
            }
        }
        
        private void MarkDirty(Object obj) =>
            EditorUtility.SetDirty(obj);
    }
}