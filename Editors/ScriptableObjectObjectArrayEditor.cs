using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomEditor(typeof(ScriptableObjectObjectArray))]
    public class ScriptableObjectObjectArrayEditor : Editor
    {
        private ScriptableObjectObjectDatabase[] _database;
        private int _length;
        private bool _show;
        private ScriptableObjectObjectArray _target;
        private SerializedProperty _valuesProperties;
        private ReorderableList _values;

        private void OnEnable()
        {
            _database = AssetDatabase.FindAssets($"t:{typeof(ScriptableObjectObjectDatabase)}")
                .Select(it =>
                    AssetDatabase.LoadAssetAtPath<ScriptableObjectObjectDatabase>(AssetDatabase.GUIDToAssetPath(it)))
                .ToArray();
            _target = target as ScriptableObjectObjectArray ?? throw new NullReferenceException(nameof(target));
            _valuesProperties = serializedObject.FindProperty("values");
            _values = new ReorderableList(serializedObject, _valuesProperties, true, true, true, true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                elementHeightCallback = ElementHeight
            };
        }

        public override void OnInspectorGUI()
        {
            if (_database == null || _database.Length == 0)
            {
                EditorGUILayout.HelpBox("You must be create object database", MessageType.Warning);
            }
            else
            {
                _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "Object Database");
                if (_show)
                {
                    var array = target as ScriptableObjectObjectArray ?? throw new NullReferenceException();
                    var arrayName = array.name;
                    foreach (var database in _database)
                    {
                        var map = database.ToDictionary(it => it.name);
                        var contain = map.ContainsKey(arrayName);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(database.name);
                        var toggle = EditorGUILayout.Toggle(database.name, contain);
                        EditorGUILayout.EndHorizontal();
                        if (toggle)
                        {
                            map[arrayName] = array;
                        }
                        else
                        {
                            map.Remove(arrayName);
                        }

                        if (toggle == contain) continue;
                        database.Set(map.OrderBy(it => it.Key).Select(it => it.Value));
                        EditorUtility.SetDirty(database);
                        AssetDatabase.SaveAssetIfDirty(database);
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            // base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
            if (_target.Type == null) return;
            _valuesProperties.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(
                _valuesProperties.isExpanded,
                "Values"
            );
            if (_valuesProperties.isExpanded)
            {
                _values.DoLayoutList();
            }

            AssetDatabase.SaveAssetIfDirty(_target);
        }

        private void DrawHeader(Rect rect)
        {
            // _valuesProperties.isExpanded = EditorGUI.Foldout(rect, _valuesProperties.isExpanded, "Values");
            EditorGUI.PropertyField(rect, _valuesProperties.FindPropertyRelative("Array.size"));
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!_valuesProperties.isExpanded) return;
            var element = _values.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.ObjectField(rect, element, _target.Type);
        }

        private float ElementHeight(int index)
        {
            return _valuesProperties.isExpanded
                ? EditorGUI.GetPropertyHeight(_values.serializedProperty.GetArrayElementAtIndex(index))
                : 0;
        }
    }
}