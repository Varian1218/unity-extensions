using System;
using UnityEngine;

namespace UnityExtensions
{
    [Serializable]
    public class UnityComplexType : ISerializationCallbackReceiver
    {
        [SerializeField] private string[] args;
        [SerializeField] private string type;
        [SerializeField] private string typeName;
        private Type _value;

        public Type Value => _value;

        public static implicit operator Type(UnityComplexType type)
        {
            return type._value;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _value = Type.GetType(typeName);
        }
    }
}