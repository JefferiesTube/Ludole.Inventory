using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(ItemSlot), true), CanEditMultipleObjects]
    public class ItemSlotEditor : MarkedUpEditor
    {
    }
}