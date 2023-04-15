using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    [CreateAssetMenu(
        fileName = "Mono Behaviour Database",
        menuName = "Unity Extensions/Mono Behaviour Database",
        order = 1
    )]
    public class ScriptableObjectArray : ScriptableObject, IEnumerable<Object>
    {
        [SerializeField] private UnityType type;
        [SerializeField] private Object[] values;

        public Type Type => type.Type;

        public IEnumerator<Object> GetEnumerator()
        {
            return values.Select(it => it).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}