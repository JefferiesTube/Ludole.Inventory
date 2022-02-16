using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : UnityEditor.Editor
    {
        private static GUIStyle _linkStyle;

        private LootTable _lootTable => (LootTable)target;
        private LootDataset _current;

        private List<LootDataset> _navigationStack;

        private void OnEnable()
        {
            _navigationStack = new List<LootDataset>();
        }

        public override void OnInspectorGUI()
        {
            _linkStyle = GUI.skin.label;
            _linkStyle.richText = true;
            _linkStyle.hover.textColor = Color.blue;

            if (_lootTable.Empty)
            {
                LootGroup newGroup = new LootGroup("New Loot Group");
                _lootTable.Root = newGroup;
                _current = newGroup;
            }
            else if (_current == null)
            {
                _current = _lootTable.Root;
            }

            DrawNavigation();
            DrawCurrent();
        }

        private void DrawNavigation()
        {
            int indentLevel = EditorGUI.indentLevel;
            for (int i = 0; i < _navigationStack.Count; i++)
            {
                EditorGUI.indentLevel++;
                if (GUILayout.Button(
                        $"{(i > 0 ? "..." : "")}{_navigationStack[i].Name} [{GetShortTypeName(_navigationStack[i])} -> {_navigationStack[i].Name}]",
                        _linkStyle))
                {
                    _current = _navigationStack[i];
                    _navigationStack.RemoveRange(i, _navigationStack.Count - i);
                    return;
                }
            }

            EditorGUI.indentLevel = indentLevel;
        }

        private string GetShortTypeName(LootDataset dataset)
        {
            switch (dataset)
            {
                case LootGroup lootGroup:
                    return "Group";

                case LootItem lootItem:
                    return "Item";

                case LootLink lootLink:
                    return "Link";

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataset));
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
            switch (_current)
            {
                case LootGroup lootGroup:
                    DrawGroup(lootGroup);
                    break;

                case LootItem lootItem:
                    DrawItem(lootItem);
                    break;

                case LootLink lootLink:
                    DrawLink(lootLink);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(_current));
            }
        }

        private void DrawGroup(LootGroup lootGroup)
        {
            lootGroup.GroupName = EditorGUILayout.TextField("Name", lootGroup.GroupName);
            lootGroup.Chance = EditorGUILayout.Slider("Chance", lootGroup.Chance, 0, 1);
            lootGroup.Mode = (GroupMode) EditorGUILayout.EnumPopup("Mode", lootGroup.Mode);
            lootGroup.Limit = Mathf.Max(1,  EditorGUILayout.IntField("Limit", lootGroup.Limit));

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Contents");
                for (int i = 0; i < lootGroup.SubItems.Count; i++)
                {
                    LootDataset item = lootGroup.SubItems[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(item.Name);
                        if(GUILayout.Button("X"))
                        {
                            lootGroup.SubItems.RemoveAt(i);
                            i--;
                            continue;
                        }

                        if (GUILayout.Button("Edit"))
                        {
                            _navigationStack.Add(_current);
                            _current = item;
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                AddGroupButton(lootGroup);
                AddLinkButton(lootGroup);
                AddItemButton(lootGroup);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawItem(LootItem lootItem)
        {
            lootItem.Chance = EditorGUILayout.Slider("Chance", lootItem.Chance, 0, 1);
            lootItem.ItemTemplate = (ItemBase) EditorGUILayout.ObjectField(lootItem.ItemTemplate, typeof(ItemBase), false);
            lootItem.Amount = Mathf.Max(1, EditorGUILayout.IntField("Amount", lootItem.Amount));
        }

        private void DrawLink(LootLink lootLink)
        {
            lootLink.Chance = EditorGUILayout.Slider("Chance", lootLink.Chance, 0, 1);
            lootLink.LootTable = (LootTable) EditorGUILayout.ObjectField(lootLink.LootTable, typeof(LootTable), false);
        }
    }
}