using System;
using System.Collections.Generic;
using CSharpBoosts;
using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Editors
{
    [CustomPropertyDrawer(typeof(UnityComplexType), true)]
    public class UnityComplexTypePropertyDrawer : PropertyDrawer
    {
        private const string Bool = "Bool";
        private const string Dictionary = "Dictionary";
        private const string Float = "Float";
        private const string Int = "Int";
        private const string Custom = "Custom";

        private readonly string[] _options =
        {
            "Bool",
            "Dictionary",
            "Float",
            "Int",
            "String",
            Custom
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            position.y += 4;
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var targetRect = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
            var argsProp = property.FindPropertyRelative("args");
            var typeProp = property.FindPropertyRelative("type");
            var typeNameProp = property.FindPropertyRelative("typeName");
            var dropdownPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            typeProp.stringValue = _options[EditorGUI.Popup(
                dropdownPosition,
                Array.FindIndex(_options, it => it == typeProp.stringValue),
                _options
            )];
            switch (typeProp.stringValue)
            {
                case Bool:
                    typeNameProp.stringValue = typeof(bool).AssemblyQualifiedName;
                    break;
                case Dictionary:
                    argsProp.arraySize = 2;
                    EditorGUI.PropertyField(position, argsProp.GetArrayElementAtIndex(0));
                    EditorGUI.PropertyField(position, argsProp.GetArrayElementAtIndex(1));
                    try
                    {
                        typeNameProp.stringValue = typeof(Dictionary<,>).MakeGenericType(
                            Type.GetType(argsProp.GetArrayElementAtIndex(0).stringValue),
                            Type.GetType(argsProp.GetArrayElementAtIndex(1).stringValue)
                        ).AssemblyQualifiedName;
                    }
                    catch (Exception e)
                    {
                        EditorGUI.HelpBox(position, e.Message, MessageType.Error);
                    }
                    break;
                case Custom:
                    break;
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        // private void DrawCustomType()
        // {
        //     var guidProp = argsProp.GetArrayElementAtIndex(0);
        //     var script = EditorGUI.ObjectField(
        //         targetRect,
        //         AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guidProp.stringValue)),
        //         typeof(MonoScript),
        //         true
        //     ) as MonoScript;
        //     if (script == null)
        //     {
        //         guidProp.stringValue = string.Empty;
        //         typeNameProp.stringValue = string.Empty;
        //     }
        //     else
        //     {
        //         var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(script)).ToString();
        //         if (guid != guidProp.stringValue)
        //         {
        //             guidProp.stringValue = guid;
        //         }
        //
        //         var type = (script.GetClass() ?? CSharpScript.GetType(script.text)).AssemblyQualifiedName;
        //         if (type != typeNameProp.stringValue)
        //         {
        //             typeNameProp.stringValue = type;
        //         }
        //     }
        // }
    }
}