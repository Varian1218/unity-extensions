using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpBoosts;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityBoosts.Objects
{
    [CreateAssetMenu(fileName = "Object Array Array", menuName = "Unity Extensions/Object Array Array", order = 1)]
    public class ScriptableObjectObjectArrayArray : ScriptableObject,
        IEnumerable<(IEnumerable<object>, Type)>
    {
        [SerializeField] private ObjectArrayArrayData[] data;

        private static (IEnumerable<object>, Type) Convert(IObjectArray array)
        {
            return (array, array.Type);
        }

        private static (IEnumerable<object>, Type) Convert(TextAsset objects, TextAsset type)
        {
            var elementType = CSharpScript.GetType(type.text).MakeArrayType();
            return (
                (IEnumerable<object>)JsonConvert.DeserializeObject(objects.text, elementType.MakeArrayType()),
                elementType
            );
        }

        public IEnumerator<(IEnumerable<object>, Type)> GetEnumerator()
        {
            return data.Select(it => it.type switch
            {
                ObjectArrayHash.ObjectArray => Convert((IObjectArray)it.objects[0]),
                ObjectArrayHash.TextAsset => Convert((TextAsset)it.objects[0], (TextAsset)it.objects[1]),
                _ => throw new ArgumentOutOfRangeException(it.type)
            }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}