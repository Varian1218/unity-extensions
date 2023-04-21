using UnityEditor;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomPropertyDrawer(typeof(ObjectPair<>), true)]
    public class ObjectPairPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded
                ? EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2
                : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            var pos = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, label);
            if (property.isExpanded)
            {
                pos.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var hashProp = property.FindPropertyRelative("hash");
                var objProp = property.FindPropertyRelative("obj");
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(pos, hashProp);
                pos.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(pos, objProp);
                if (GUI.changed)
                {
                    hashProp.stringValue = objProp.objectReferenceValue == null
                        ? string.Empty
                        : objProp.objectReferenceValue.name;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}