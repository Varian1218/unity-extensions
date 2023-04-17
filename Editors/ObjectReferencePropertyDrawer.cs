using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomPropertyDrawer(typeof(ObjectReference<>), true)]
    public class ObjectReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var targetProperty = property.FindPropertyRelative("target");
            EditorGUI.PropertyField(position, targetProperty, GUIContent.none);
            var type = Type.GetType(property.FindPropertyRelative("typeName").stringValue) ??
                       throw new NullReferenceException(property.type);
            var args = type.GetGenericArguments();
            var obj = EditorGUI.ObjectField(position, targetProperty.objectReferenceValue, args[0], false);
            if (targetProperty.objectReferenceValue == obj) return;
            targetProperty.objectReferenceValue = obj;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}