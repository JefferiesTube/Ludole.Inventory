using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(JigsawGridDisplay), true), CanEditMultipleObjects]
    public class JigsawGridDisplayEditor : MarkedUpEditor
    {
    }
}