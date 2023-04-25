using CSharpBoosts;
using UnityEditor;

namespace UnityBoosts.Editors
{
    public static class UnityEditorGUILayout
    {
        public static UnityType TypeField(UnityType unityType)
        {
            var script = EditorGUILayout.ObjectField(
                AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(unityType.Guid)),
                typeof(MonoScript),
                true
            ) as MonoScript;
            if (script == null)
            {
                unityType.Guid = string.Empty;
                unityType.TypeName = string.Empty;
            }
            else
            {
                var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(script)).ToString();
                if (guid != unityType.Guid)
                {
                    unityType.Guid = guid;
                }

                var typeName = (script.GetClass() ?? CSharpScript.GetType(script.text)).AssemblyQualifiedName;
                if (typeName != unityType.TypeName)
                {
                    unityType.TypeName = typeName;
                }
            }

            return unityType;
        }
    }
}