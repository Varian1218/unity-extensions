using CSharpExtensions;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomPropertyDrawer(typeof(UnityType), true)]
    public class UnityTypePropertyDrawer : PropertyDrawer
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
            var typeProp = property.FindPropertyRelative("typeName");
            var script = EditorGUI.ObjectField(
                targetRect,
                AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(targetProp.stringValue)),
                typeof(MonoScript),
                true
            ) as MonoScript;
            if (script == null)
            {
                targetProp.stringValue = string.Empty;
                typeProp.stringValue = string.Empty;
            }
            else
            {
                var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(script)).ToString();
                if (guid != targetProp.stringValue)
                {
                    targetProp.stringValue = guid;
                }

                var type = (script.GetClass() ?? CSharpScript.GetType(script.text)).AssemblyQualifiedName;
                if (type != typeProp.stringValue)
                {
                    typeProp.stringValue = type;
                }
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}