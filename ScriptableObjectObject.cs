using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Object", menuName = "Unity Extensions/Object")]
    public class ScriptableObjectObject : ScriptableObject
    {
        [Serializable]
        private struct UnityField
        {
            public string fieldName;
            public UnityType fieldType;
        }

        [SerializeField] private UnityField[] fields;
        private Dictionary<string, object> _fields;

        public T GetValue<T>(string fieldName)
        {
            _fields ??= fields.ToDictionary(it => it.fieldName, it => Activator.CreateInstance(it.fieldType));
            return (T)_fields[fieldName];
        }
    }
}