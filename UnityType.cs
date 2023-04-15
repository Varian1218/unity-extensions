using System;
using UnityEngine;

namespace UnityExtensions
{
    [Serializable]
    public class UnityType : ISerializationCallbackReceiver
    {
        [SerializeField] private string guid;
        [SerializeField] private string typeName;
#pragma warning restore 0414
        [SerializeField] private bool dirty;
        private Type _type;
        public Type Type => _type ??= string.IsNullOrEmpty(typeName) ? null : Type.GetType(typeName);
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (!dirty) return;
            dirty = false;
        }

        public void OnAfterDeserialize()
        {
            _type = Type.GetType(typeName);
        }
#endif
    }
}