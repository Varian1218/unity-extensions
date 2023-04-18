using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.Editors
{
    [CustomEditor(typeof(ScriptableObjectScriptText))]
    public class ScriptableObjectScriptTextEditor : Editor
    {
        private SerializedProperty _dataProperty;
        private ReorderableList _reorderableList;
        private ScriptableObjectScriptText _target;

        private void OnEnable()
        {
            _target = target as ScriptableObjectScriptText ?? throw new NullReferenceException(nameof(target));
            // _data = _target.data?.Select(it => new Data
            // {
            //     ScriptTextData = it
            // }).ToArray() ?? Array.Empty<Data>();
            _dataProperty = serializedObject.FindProperty("data");
            _reorderableList = new ReorderableList(serializedObject, _dataProperty);
            _reorderableList.drawElementCallback = DrawElement;
        }

        private static Array CreateArray(Array array, int length, Type type)
        {
            var result = Activator.CreateInstance(type, array.Length + 1) as Array ??
                         throw new NullReferenceException();
            Array.Copy(array, result, Math.Min(array.Length, length));
            return result;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var property = _dataProperty.GetArrayElementAtIndex(index);
            EditorGUILayout.PropertyField(_dataProperty.GetArrayElementAtIndex(index));
            if (!property.isExpanded) return;
            var data = _target.data[index];
            if (data.textAsset == null || data.type?.Type == null) return;
            try
            {
                var type = data.array ? data.type.Type.MakeArrayType() : data.type.Type;
                var value = JsonConvert.DeserializeObject(data.textAsset.text, type);
                var text = JsonConvert.SerializeObject(
                    DrawType(data.textAsset.name, type, value),
                    data.indented ? Formatting.Indented : Formatting.None
                );
                if (text == data.textAsset.text) return;
                File.WriteAllText(AssetDatabase.GetAssetPath(data.textAsset), text);
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                EditorGUILayout.HelpBox(e.Message, MessageType.Error);
            }
        }

        private static object DrawType(string label, Type type, object value)
        {
            if (type == typeof(bool)) return EditorGUILayout.Toggle(label, (bool)value);
            if (type == typeof(float)) return EditorGUILayout.FloatField(label, (float)value);
            if (type == typeof(int)) return EditorGUILayout.IntField(label, (int)value);
            if (type == typeof(string)) return EditorGUILayout.TextField(label, value as string);
            EditorGUILayout.Foldout(true, label);
            EditorGUI.indentLevel++;
            if (type.IsArray)
            {
                value ??= Activator.CreateInstance(type, 0);
                var array = value as Array ?? throw new NullReferenceException(label);
                EditorGUILayout.IntField("Size", array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    array.SetValue(DrawType($"{i}", type.GetElementType(), array.GetValue(i)), i);
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add"))
                {
                    array = CreateArray(array, array.Length + 1, type);
                }

                if (GUILayout.Button("Remove") && array.Length > 0)
                {
                    array = CreateArray(array, array.Length - 1, type);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                return array;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();
                if (genericArguments[0] == typeof(string))
                {
                    var valueType = genericArguments[1];
                    value ??= Activator.CreateInstance(type);
                    var dictionary = value as IDictionary ?? throw new NullReferenceException();
                    var keys = new List<object>((IEnumerable<object>)dictionary.Keys);
                    foreach (var it in keys)
                    {
                        EditorGUILayout.BeginHorizontal();
                        dictionary[it] = DrawType(it as string, valueType, dictionary[it]);
                        if (GUILayout.Button("Remove"))
                        {
                            dictionary.Remove(it);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.BeginHorizontal();
                    // if (GUILayout.Button("Add"))
                    // {
                    //     dictionary.Add(string.Empty, default);
                    // }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                    return dictionary;
                }
            }

            foreach (var fieldInfo in type.GetFields())
            {
                var fieldType = fieldInfo.FieldType;
                value ??= Activator.CreateInstance(type);
                fieldInfo.SetValue(value, DrawType(fieldInfo.Name, fieldType, fieldInfo.GetValue(value)));
            }

            EditorGUI.indentLevel--;
            return value;
        }

        // private void DrawType(string label, ref object obj)
        // {
        //     switch (obj)
        //     {
        //         case bool b:
        //             obj = EditorGUILayout.Toggle(b, label);
        //             break;
        //         case float f:
        //             obj = EditorGUILayout.FloatField(f, label);
        //             break;
        //         case int i:
        //             obj = EditorGUILayout.IntField(i, label);
        //             break;
        //         case string s:
        //             obj = EditorGUILayout.TextField(s, label);
        //             break;
        //         default:
        //             foreach (var fieldInfo in obj.GetType().GetFields())
        //             {
        //                 var fo = fieldInfo.GetValue(obj);
        //                 DrawType(fieldInfo.Name, ref fo);
        //                 fieldInfo.SetValue(obj, fo);
        //             }
        //             break;
        //     }
        // }

        public override void OnInspectorGUI()
        {
            _reorderableList.DoLayoutList();
            // for (var i = 0; i < _data.Length; i++)
            // {
            //     EditorGUILayout.PropertyField(_dataProperty.GetArrayElementAtIndex(i));
            //     var data = _data[i].ScriptTextData;
            //     if (data.textAsset == null || data.type?.Type == null) continue;
            //     var value = JsonConvert.DeserializeObject(data.textAsset.text, data.type.Type);
            //     var text = JsonConvert.SerializeObject(DrawType(data.textAsset.name, data.type.Type, value), Formatting.Indented);
            //     if (text == data.textAsset.text) continue;
            //     File.WriteAllText(AssetDatabase.GetAssetPath(data.textAsset), text);
            //     AssetDatabase.Refresh();
            // }
        }
    }
}