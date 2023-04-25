using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityBoosts
{
    [CreateAssetMenu(fileName = "Object Array", menuName = "Unity Extensions/Object Array", order = 1)]
    public class ScriptableObjectObjectArray : ScriptableObject, IEnumerable<Object>
    {
        [SerializeField] private UnityType type;
        [SerializeField] private Object[] values;
        
        public Type Type => type?.Type;

        public IEnumerator<Object> GetEnumerator()
        {
            return (values ?? Array.Empty<Object>()).Select(it => it).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public static string GetHash(Type type)
        {
            return type?.FullName;
        }

        public void Set(IEnumerable<Object> value)
        {
            values = value.ToArray();
        }
    }
}