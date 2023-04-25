using System;

namespace UnityBoosts
{
    [Serializable]
    public class UnityPair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}