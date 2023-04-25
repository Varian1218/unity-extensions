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

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public Type Type => _type;// ??= string.IsNullOrEmpty(typeName) ? null : Type.GetType(typeName);

        public string TypeName
        {
            get => typeName;
            set
            {
                typeName = value;
                _type = Type.GetType(value);
            }
        }

        public static implicit operator Type(UnityType type)
        {
            return type?.Type;
        }

        public void OnBeforeSerialize()
        {
            if (!dirty) return;
            dirty = false;
        }

        public void OnAfterDeserialize()
        {
            _type = Type.GetType(typeName);
        }
    }
}