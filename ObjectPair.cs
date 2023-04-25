using System;
using Object = UnityEngine.Object;

namespace UnityBoosts
{
    [Serializable]
    public struct ObjectPair<T> where T : Object
    {
        public string hash;
        public T obj;
    }
}