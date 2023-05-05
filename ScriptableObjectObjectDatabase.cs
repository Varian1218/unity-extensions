using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityBoosts
{
    [CreateAssetMenu(fileName = "Object Database", menuName = "Unity Extensions/Object Database", order = 1)]
    public class ScriptableObjectObjectDatabase : ScriptableObject,
        IObjectDatabase,
        IEnumerable<ScriptableObjectObjectDatabase.Pair>,
        ISerializationCallbackReceiver
    {
        [Serializable]
        public struct Pair
        {
            public string hash;
            public ScriptableObjectObjectArray objects;
        }
        [SerializeField] private Pair[] values;
        private Dictionary<string, ScriptableObjectObjectArray> _values;

        public IEnumerable<T> Query<T>() where T : class
        {
            return _values[ScriptableObjectObjectArray.GetHash(typeof(T)) ?? throw new NullReferenceException()]
                .Select(it => it is T t ? t : throw new ArgumentException());
        }

        public IEnumerable<Object> Query(string hash)
        {
            return _values[hash];
        }

        public IEnumerable<T> Query<T>(string hash) where T : class
        {
            return _values[hash].Select(it => it as T ?? throw new NullReferenceException());
        }

        public IEnumerable<Object> Query(Type type)
        {
            return _values[ScriptableObjectObjectArray.GetHash(type)];
        }

        public void SetValue(IEnumerable<Pair> value)
        {
            values = value.ToArray();
        }

        public IEnumerator<Pair> GetEnumerator()
        {
            return (values ?? Array.Empty<Pair>()).Select(it => it).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _values = values.ToDictionary(it => it.hash, it => it.objects);
        }
    }
}