using CSharpBoosts;
using UnityEngine;

namespace UnityBoosts.DynamicObjects
{
    [CreateAssetMenu(fileName = "Dynamic Object", menuName = "Unity Boosts/Dynamic Objects/Dynamic Object", order = 1)]
    public class ScriptableObjectDynamicObject : ScriptableObject, IDynamicObject
    {
        private IDynamicObject _impl;

        public IDynamicObject Impl
        {
            set => _impl = value;
        }

        public T GetValue<T>(string fieldName)
        {
            return _impl.GetValue<T>(fieldName);
        }
    }
}