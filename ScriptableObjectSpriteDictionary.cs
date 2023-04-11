using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Sprite Dictionary", menuName = "Unity Extensions/Sprite Dictionary", order = 1)]
    public class ScriptableObjectSpriteDictionary : ScriptableObject, IDictionary<string, Sprite>
    {
        private IDictionary<string, Sprite> _impl;

        private IDictionary<string, Sprite> Impl
        {
            set => _impl = value;
        }

        public IEnumerator<KeyValuePair<string, Sprite>> GetEnumerator()
        {
            return _impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_impl).GetEnumerator();
        }

        public void Add(KeyValuePair<string, Sprite> item)
        {
            _impl.Add(item);
        }

        public void Clear()
        {
            _impl.Clear();
        }

        public bool Contains(KeyValuePair<string, Sprite> item)
        {
            return _impl.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Sprite>[] array, int arrayIndex)
        {
            _impl.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Sprite> item)
        {
            return _impl.Remove(item);
        }

        public int Count => _impl.Count;

        public bool IsReadOnly => _impl.IsReadOnly;

        public void Add(string key, Sprite value)
        {
            _impl.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _impl.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _impl.Remove(key);
        }

        public bool TryGetValue(string key, out Sprite value)
        {
            return _impl.TryGetValue(key, out value);
        }

        public Sprite this[string key]
        {
            get => _impl[key];
            set => _impl[key] = value;
        }

        public ICollection<string> Keys => _impl.Keys;

        public ICollection<Sprite> Values => _impl.Values;
    }
}