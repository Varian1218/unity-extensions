using System;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    [Serializable]
    public struct ObjectPair<T> where T : Object
    {
        public string hash;
        public T obj;
    }
}