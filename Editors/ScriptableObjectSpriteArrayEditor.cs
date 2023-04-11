using System;
using System.Linq;
using UnityEditor;

namespace UnityExtensions.Editors
{
    [CustomEditor(typeof(ScriptableObjectSpriteArray))]
    public class ScriptableObjectSpriteArrayEditor : Editor
    {
        private ScriptableObjectLoadSpriteAction[] _loadSpriteActions;
        private bool _show;

        private void OnEnable()
        {
            _loadSpriteActions = AssetDatabase.FindAssets($"t:{typeof(ScriptableObjectLoadSpriteAction)}")
                .Select(it =>
                    AssetDatabase.LoadAssetAtPath<ScriptableObjectLoadSpriteAction>(AssetDatabase.GUIDToAssetPath(it)))
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            if (_loadSpriteActions == null || _loadSpriteActions.Length == 0)
            {
                EditorGUILayout.HelpBox("You must be create widget list", MessageType.Warning);
            }
            else
            {
                _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "Load Sprite Action");
                if (_show)
                {
                    var array = target as ScriptableObjectSpriteArray ?? throw new NullReferenceException();
                    var arrayName = array.name;
                    foreach (var action in _loadSpriteActions)
                    {
                        var map = action.ToDictionary(it => it.Array.name);
                        map.TryGetValue(arrayName, out var pair);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(action.name);
                        var newDictionary = EditorGUILayout.ObjectField(
                            pair.Dictionary,
                            typeof(ScriptableObjectSpriteDictionary),
                            false
                        ) as ScriptableObjectSpriteDictionary;
                        EditorGUILayout.EndHorizontal();
                        if (newDictionary)
                        {
                            if (!pair.Dictionary)
                            {
                                map.Add(arrayName, (array, newDictionary));
                            }
                        }
                        else
                        {
                            if (pair.Dictionary)
                            {
                                map.Remove(arrayName);
                            }
                        }

                        if (newDictionary == pair.Dictionary) continue;
                        action.Set(map.OrderBy(it => it.Key).Select(it => it.Value));
                        EditorUtility.SetDirty(action);
                        AssetDatabase.SaveAssetIfDirty(action);
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            base.OnInspectorGUI();
        }
    }
}