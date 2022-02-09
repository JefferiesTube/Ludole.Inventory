using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(SlotInventory), true), CanEditMultipleObjects]
    public class SlotInventoryEditor : MarkedUpEditor
    {
    }
}