using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : UnityEditor.Editor
    {
        private LootTable _lootTable;
        private LootDataset _current;

        protected LootTable table => _lootTable != null ? _lootTable : _lootTable = (LootTable) target;

        public override void OnInspectorGUI()
        {
            if (table.Root == null)
            {
                var newGroup = CreateInstance<LootGroup>();
                AssetDatabase.AddObjectToAsset(newGroup, table);
                table.Root = newGroup;
                _current = table.Root;
            }

            DrawCurrent();

            if (GUILayout.Button("Add Group"))
            {

            }
        }

        private void DrawCurrent()
        {
            
        }
    }
}
