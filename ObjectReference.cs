using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    [Serializable]
    public class ObjectReference<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField] private bool dirty;
        [SerializeField] private Object target;
        [SerializeField] private string typeName;
        public T Value => target as T;

        public void OnAfterDeserialize()
        {
            typeName = GetType().AssemblyQualifiedName;
        }

        public void OnBeforeSerialize()
        {
            if (!dirty) return;
            dirty = false;
        }
    }
}