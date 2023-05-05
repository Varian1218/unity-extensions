using System;
using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Editors
{
    [CustomPropertyDrawer(typeof(PopupPropertyAttribute))]
    public class PopupPropertyAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var popupAttribute = (PopupPropertyAttribute)attribute;
            var options = popupAttribute.Options;
            var currentValue = property.stringValue;
            var currentIndex = Array.IndexOf(options, currentValue);
            currentIndex = EditorGUI.Popup(position, currentIndex, options);
            if (currentIndex >= 0 && currentIndex < options.Length)
            {
                property.stringValue = options[currentIndex];
            }

            EditorGUI.EndProperty();
        }
    }
}