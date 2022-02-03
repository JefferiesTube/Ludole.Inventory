using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(TooltipGraph))]
    public class LudoleGraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Graph Editor"))
            {
                LudoleGraphWindow.OpenGraph(target as TooltipGraph);
            }
        }

        [OnOpenAsset]
        public static bool OpenGraphOnDoubleClick(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is TooltipGraph tg)
            {
                LudoleGraphWindow.OpenGraph(tg);
                return true;
            }

            return false;
        }
    }
}