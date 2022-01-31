using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(TooltipFactoryGraph))]
    public class TooltipFactoryGraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Graph Editor"))
            {
                TooltipGraphWindow.OpenTooltipGraph(target as TooltipFactoryGraph);
            }
        }
    }
}