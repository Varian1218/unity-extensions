using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBoosts
{
    [Serializable]
    public class UnityMap<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct Pair
        {
            public TKey key;
            public TValue Value;
        }

        [SerializeField] private Pair[] elements;
        private IDictionary<TKey, TValue> _impl;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_impl).GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _impl.Add(item);
        }

        public void Clear()
        {
            _impl.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _impl.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _impl.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _impl.Remove(item);
        }

        public int Count => _impl.Count;

        public bool IsReadOnly => _impl.IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            _impl.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _impl.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return _impl.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _impl.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => _impl[key];
            set => _impl[key] = value;
        }

        public ICollection<TKey> Keys => _impl.Keys;

        public ICollection<TValue> Values => _impl.Values;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _impl = elements.ToDictionary(it => it.key, it => it.Value);
        }
    }
}