using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    [Serializable]
    public class ObjectReference<T> where T : class
    {
        [SerializeField] private Object target;
        public T Value => target as T;
    }
}