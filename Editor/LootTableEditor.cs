using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : UnityEditor.Editor
    {
        private LootTable _lootTable => (LootTable)target;
        private LootDataset _current;

        private Stack<LootDataset> _navigationStack;

        private ReorderableList _list;

        private void OnEnable()
        {
            _navigationStack = new Stack<LootDataset>();
        }

        public override void OnInspectorGUI()
        {
            if (_lootTable.Empty)
            {
                LootGroup newGroup = new LootGroup("New Loot Group");
                _lootTable.Root = newGroup;
                _current = newGroup;
                UpdateList();
            }
            else if (_current == null)
            {
                _current = _lootTable.Root;
            }

            if (_list == null)
                UpdateList();

            DrawCurrent();

            if (_current is LootGroup group)
            {
                AddGroupButton(group);
                AddLinkButton(group);
                AddItemButton(group);
            }
        }

        private void UpdateList()
        {
            _list = new ReorderableList((_current as LootGroup).SubItems, typeof(LootDataset), true, true, false, false);
            _list.drawElementCallback += CustomDrawHandler;
            _list.elementHeightCallback += CustomElementHeight;
        }

        private float CustomElementHeight(int index)
        {
            switch ((_current as LootGroup).SubItems[index])
            {
                case LootGroup:
                    return 200;

                case LootItem lootItem:
                    return 100;

                case LootLink lootLink:
                    return 100;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CustomDrawHandler(Rect rect, int index, bool isactive, bool isfocused)
        {
            switch ((_current as LootGroup).SubItems[index])
            {
                case LootGroup lootGroup:
                    EditorGUILayout.LabelField("Group");
                    EditorGUILayout.LabelField("Group");
                    EditorGUILayout.LabelField("Group");

                    break;

                case LootItem lootItem:
                    EditorGUI.LabelField(rect, "Item");
                    break;

                case LootLink lootLink:
                    EditorGUI.LabelField(rect, "Link");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddGroupButton(LootGroup lootGroup)
        {
            if (GUILayout.Button("Add Group"))
            {
                lootGroup.SubItems.Add(new LootGroup("New Loot Group"));
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void AddLinkButton(LootGroup lootGroup)
        {
            if (GUILayout.Button("Add Link"))
            {
                lootGroup.SubItems.Add(new LootLink());
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void AddItemButton(LootGroup lootGroup)
        {
            if (GUILayout.Button("Add Item"))
            {
                lootGroup.SubItems.Add(new LootItem());
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawCurrent()
        {
            _list.DoLayoutList();
        }
    }
}