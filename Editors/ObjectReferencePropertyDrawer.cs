using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Editors
{
    [CustomPropertyDrawer(typeof(ObjectReference<>), true)]
    public class ObjectReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            // EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var targetProperty = property.FindPropertyRelative("target");
            // EditorGUI.PropertyField(position, targetProperty, GUIContent.none);
            var args = fieldInfo.FieldType.GetGenericArguments();
            var obj = EditorGUI.ObjectField(position, targetProperty.objectReferenceValue, args[0], true);
            if (targetProperty.objectReferenceValue == obj) return;
            targetProperty.objectReferenceValue = obj;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}