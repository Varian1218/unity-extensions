using System;

namespace UnityExtensions
{
    [Serializable]
    public class UnityPair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}