using System;
using UnityEngine;

namespace UnityBoosts
{
    [Serializable]
    public class UnityType : ISerializationCallbackReceiver
    {
        [SerializeField] private string guid;
        [SerializeField] private string typeName;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public Type Type { get; private set; }

        public string TypeName
        {
            get => typeName;
            set
            {
                typeName = value;
                Type = Type.GetType(value);
            }
        }

        public static implicit operator Type(UnityType type)
        {
            return type?.Type;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Type = Type.GetType(typeName);
        }
    }
}