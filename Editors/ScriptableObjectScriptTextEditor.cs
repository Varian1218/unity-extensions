using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpExtensions;
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
        private Dictionary<int, DrawObject> _objects;
        private ReorderableList _reorderableList;
        private ScriptableObjectScriptText _target;

        private void OnEnable()
        {
            _target = target as ScriptableObjectScriptText ?? throw new NullReferenceException(nameof(target));
            _objects = _target.data?.Select(CreateObject).Select((it, i) => new KeyValuePair<int, DrawObject>(i, it))
                .ToDictionary();
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

        private static DrawObject CreateObject(ScriptableObjectScriptText.Data data)
        {
            if (data.textAsset == null || data.unityType?.Type == null)
            {
                return null;
            }

            var type = data.array ? data.unityType.Type.MakeArrayType() : data.unityType;
            return TypeDrawer.CreateObject(
                data.textAsset.name,
                JsonConvert.DeserializeObject(
                    data.textAsset.text,
                    type
                ),
                type
            );
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            // var dataProps = serializedObject.FindProperty("data");
            var dataProp = _dataProperty.GetArrayElementAtIndex(index);
            EditorGUILayout.PropertyField(dataProp);
            if (GUI.changed)
            {
                _objects[index] = CreateObject(_target.data[index]);
            }

            // if (index >= _target.data.Length)
            // {
            //     var array = new ScriptableObjectScriptText.Data[index + 1];
            //     Array.Copy(_target.data, array, _target.data.Length);
            //     _target.data = array;
            // }
            // var data = _target.data[index];
            // EditorGUILayout.BeginHorizontal();
            // var textAsset = EditorGUILayout.ObjectField(data.textAsset, typeof(TextAsset), false) as TextAsset;
            // if (data.textAsset != textAsset)
            // {
            //     _target.data[index].textAsset = textAsset;
            //     _objects[index] = CreateObject(_target.data[index]);
            // }
            //
            // var unityTypeGuid = data.unityType?.Guid;
            // if (UnityEditorGUILayout.TypeField(data.unityType).Guid != unityTypeGuid)
            // {
            //     _objects[index] = CreateObject(_target.data[index]);
            // }
            //
            // EditorGUILayout.EndHorizontal();
            try
            {

                if (_objects[index] == null) return;
                var indented = dataProp.FindPropertyRelative("indented").boolValue;
                var textAsset = dataProp.FindPropertyRelative("textAsset").objectReferenceValue as TextAsset ??
                                throw new NullReferenceException();
                try
                {
                    _objects[index] = TypeDrawer.DrawType(_objects[index]);
                    var text = JsonConvert.SerializeObject(
                        _objects[index].Impl,
                        indented ? Formatting.Indented : Formatting.None
                    );
                    if (text == textAsset.text) return;
                    File.WriteAllText(AssetDatabase.GetAssetPath(textAsset), text);
                }
                catch (Exception e)
                {
                    EditorGUILayout.HelpBox(e.Message, MessageType.Error);
                }
            }
            catch (Exception)
            {
            }
        }

        public override void OnInspectorGUI()
        {
            _reorderableList.DoLayoutList();
        }
    }
}