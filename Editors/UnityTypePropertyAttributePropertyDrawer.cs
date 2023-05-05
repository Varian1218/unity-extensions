using CSharpBoosts;
using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Editors
{
    [CustomPropertyDrawer(typeof(UnityTypePropertyAttribute))]
    public class UnityTypePropertyAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Draw(position, property, label);
        }

        public void Draw(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            var typeAttribute = (UnityTypePropertyAttribute)attribute;
            var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent($"{label.text} ({typeAttribute.Type.Name})"));
            var targetRect = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
            var guidProp = property.FindPropertyRelative("guid");
            var typeProp = property.FindPropertyRelative("typeName");
            var script = EditorGUI.ObjectField(
                targetRect,
                AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guidProp.stringValue)),
                typeof(MonoScript),
                false
            ) as MonoScript;
            if (GUI.changed)
            {
                if (script == null)
                {
                    guidProp.stringValue = string.Empty;
                    typeProp.stringValue = string.Empty;
                }
                else
                {
                    var type = script.GetClass() ?? CSharpScript.GetType(script.text);
                    if (typeAttribute.Type.IsAssignableFrom(type))
                    {
                        guidProp.stringValue =
                            AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(script)).ToString();
                        typeProp.stringValue = type.AssemblyQualifiedName;
                    }
                    else
                    {
                        guidProp.stringValue = string.Empty;
                        typeProp.stringValue = string.Empty;
                    }
                }
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}