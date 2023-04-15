using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomEditor(typeof(ScriptableObjectArray))]
    public class ScriptableObjectArrayEditor : Editor
    {
        private int _length;
        private bool _show;
        private ScriptableObjectArray _target;
        private SerializedProperty _valuesProperties;
        private ReorderableList _values;

        private void OnEnable()
        {
            _target = target as ScriptableObjectArray ?? throw new NullReferenceException(nameof(target));
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