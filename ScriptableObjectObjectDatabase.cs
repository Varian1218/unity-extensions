using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Object Database", menuName = "Unity Extensions/Object Database", order = 1)]
    public class ScriptableObjectObjectDatabase : ScriptableObject, IObjectDatabase,
        IEnumerable<ScriptableObjectObjectArray>
    {
        [SerializeField] private ScriptableObjectObjectArray[] values;
        private Dictionary<string, ScriptableObjectObjectArray> _values;

        public IEnumerable<T> Query<T>()
        {
            _values ??= values.ToDictionary(it => it.Type.FullName);
            return _values[typeof(T).FullName ?? throw new NullReferenceException()]
                .Select(it => it is T t ? t : throw new ArgumentException());
        }

        public void Set(IEnumerable<ScriptableObjectObjectArray> value)
        {
            values = value.ToArray();
        }

        public IEnumerator<ScriptableObjectObjectArray> GetEnumerator()
        {
            return (values ?? Array.Empty<ScriptableObjectObjectArray>()).Select(it => it).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}