using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(JigsawSlotDisplay), true), CanEditMultipleObjects]
    public class JigsawSlotDisplayEditor : MarkedUpEditor
    {
    }
}