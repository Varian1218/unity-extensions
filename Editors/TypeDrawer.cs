using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityBoosts.Editors
{
    public class TypeDrawer
    {
        private static Dictionary<Type, Func<DrawObject, DrawObject>> _draws = new()
        {
            { typeof(bool), DrawBoolType },
            { typeof(float), DrawFloatType },
            { typeof(int), DrawIntType },
            { typeof(string), DrawStringType },
        };

        public static DrawObject CreateObject(string label, object obj, Type type)
        {
            if (type == typeof(bool) || type == typeof(float) || type == typeof(int) || type == typeof(string))
            {
                return new DrawObject
                {
                    Impl = obj,
                    Label = label,
                    Type = type
                };
            }

            if (type.IsArray)
            {
                return new ArrayDrawObject
                {
                    Label = label,
                    Type = type,
                    Impl = obj
                };
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return new DictionaryDrawObject
                    {
                        Label = label,
                        Type = type,
                        Impl = obj
                    };
                }
            }

            return new CustomDrawObject
            {
                Impl = obj,
                Label = label,
                Type = type
            };
        }

        private static bool DrawAddButton()
        {
            return GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"));
        }

        private static ArrayDrawObject DrawArrayType(ArrayDrawObject obj)
        {
            obj.Impl ??= Activator.CreateInstance(obj.Type, 0);
            EditorGUILayout.IntField("Size", obj.Length);
            for (var i = 0; i < obj.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                obj[i] = DrawType(obj[i]);
                if (DrawRemoveButton())
                {
                    obj.RemoveIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (DrawAddButton())
            {
                obj.Add();
            }

            EditorGUILayout.EndHorizontal();
            return obj;
        }

        private static DrawObject DrawBoolType(DrawObject obj)
        {
            obj.Impl = EditorGUILayout.Toggle(obj.Label, (bool)obj.Impl);
            return obj;
        }

        private static DrawObject DrawComplexType(DrawObject obj)
        {
            obj.Foldout = EditorGUILayout.Foldout(obj.Foldout, obj.Label);
            if (!obj.Foldout) return obj;
            EditorGUI.indentLevel++;
            if (obj.Type.IsArray)
            {
                var array = DrawArrayType(obj as ArrayDrawObject);
                EditorGUI.indentLevel--;
                return array;
            }

            if (obj.Type.IsGenericType && obj.Type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dictionary = DrawDictionaryType(obj as DictionaryDrawObject);
                EditorGUI.indentLevel--;
                return dictionary;
            }

            obj = DrawCustomType(obj as CustomDrawObject);
            EditorGUI.indentLevel--;
            return obj;
        }

        private static CustomDrawObject DrawCustomType(CustomDrawObject obj)
        {
            for (var i = 0; i < obj.Length; i++)
            {
                obj[i] = DrawType(obj[i]);
            }
            return obj;
        }

        private static DictionaryDrawObject DrawDictionaryType(DictionaryDrawObject obj)
        {
            var keys = new List<object>((IEnumerable<object>)obj.Keys);
            foreach (DrawObject it in keys)
            {
                EditorGUILayout.BeginHorizontal();
                var keyObj = DrawType(it);
                var valObj = obj[it.Impl];
                if (it.Impl != keyObj.Impl)
                {
                    obj.Add(it.Impl, valObj.Impl);
                    obj.Remove(it.Impl);
                }

                obj[it.Impl] = DrawType(valObj);
                if (DrawRemoveButton())
                {
                    obj.Remove(keyObj.Impl);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (DrawAddButton())
            {
                obj.Add(default, default);
            }

            return obj;
        }

        private static DrawObject DrawFloatType(DrawObject obj)
        {
            obj.Impl = EditorGUILayout.FloatField(obj.Label, (float)obj.Impl);
            return obj;
        }

        private static DrawObject DrawIntType(DrawObject obj)
        {
            obj.Impl = EditorGUILayout.IntField(obj.Label, (int)obj.Impl);
            return obj;
        }

        private static bool DrawRemoveButton()
        {
            return GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"));
        }

        private static DrawObject DrawStringType(DrawObject obj)
        {
            obj.Impl = string.IsNullOrEmpty(obj.Label)
                ? EditorGUILayout.TextField(obj.Impl as string)
                : EditorGUILayout.TextField(obj.Label, obj.Impl as string);
            return obj;
        }

        public static DrawObject DrawType(DrawObject obj)
        {
            if (_draws.TryGetValue(obj.Type, out var draw))
            {
                return draw(obj);
            }
            return DrawComplexType(obj);
        }
    }
}