using UnityEditor;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomPropertyDrawer(typeof(FullTypeNameReference), true)]
    public class FullTypeNameReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            position.y += 4;
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var targetRect = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
            var targetProp = property.FindPropertyRelative("guid");
            var valueProp = property.FindPropertyRelative("value");
            var script = EditorGUI.ObjectField(
                targetRect,
                AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(targetProp.stringValue)),
                typeof(MonoScript),
                true
            ) as MonoScript;
            if (script == null)
            {
                targetProp.stringValue = string.Empty;
                valueProp.stringValue = string.Empty;
            }
            else
            {
                targetProp.stringValue = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(script)).ToString();
                valueProp.stringValue = script.GetClass().FullName;
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}