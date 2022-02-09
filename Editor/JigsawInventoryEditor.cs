using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(JigsawInventory), true), CanEditMultipleObjects]
    public class JigsawInventoryEditor : MarkedUpEditor
    {
    }
}