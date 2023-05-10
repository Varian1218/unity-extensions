using System;
using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Objects.Editors
{
    [CustomPropertyDrawer(typeof(ObjectArrayArrayData), true)]
    public class ObjectArrayArrayDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeProp = property.FindPropertyRelative(nameof(ObjectArrayArrayData.type));
            return property.isExpanded switch
            {
                false => EditorGUIUtility.singleLineHeight,
                true => typeProp.stringValue switch
                {
                    "" => 2 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    ObjectArrayHash.ObjectArray => 3 * EditorGUIUtility.singleLineHeight +
                                                   2 * EditorGUIUtility.standardVerticalSpacing,
                    ObjectArrayHash.TextAsset => 4 * EditorGUIUtility.singleLineHeight +
                                                 3 * EditorGUIUtility.standardVerticalSpacing,
                    _ => throw new ArgumentOutOfRangeException(typeProp.stringValue)
                }
            };
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objectsProp = property.FindPropertyRelative(nameof(ObjectArrayArrayData.objects));
            position.height = EditorGUIUtility.singleLineHeight;
            var typeProp = property.FindPropertyRelative(nameof(ObjectArrayArrayData.type));
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            if (!property.isExpanded) return;
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, typeProp);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            switch (typeProp.stringValue)
            {
                case ObjectArrayHash.ObjectArray:
                    objectsProp.arraySize = 1;
                    OnGUI(position, objectsProp.GetArrayElementAtIndex(0));
                    break;
                case ObjectArrayHash.TextAsset:
                    objectsProp.arraySize = 2;
                    OnGUI(position, objectsProp.GetArrayElementAtIndex(0), objectsProp.GetArrayElementAtIndex(1));
                    break;
            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        private static void OnGUI(Rect position, SerializedProperty property)
        {
            property.objectReferenceValue = EditorGUI.ObjectField(
                position,
                "Objects",
                property.objectReferenceValue,
                typeof(IObjectArray),
                true
            );
        }

        private static void OnGUI(Rect position, SerializedProperty objectsProperty, SerializedProperty typeProperty)
        {
            objectsProperty.objectReferenceValue = EditorGUI.ObjectField(
                position,
                "Objects",
                objectsProperty.objectReferenceValue,
                typeof(TextAsset),
                true
            );
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            typeProperty.objectReferenceValue = EditorGUI.ObjectField(
                position,
                "Element Type",
                typeProperty.objectReferenceValue,
                typeof(MonoScript),
                true
            );
        }
    }
}