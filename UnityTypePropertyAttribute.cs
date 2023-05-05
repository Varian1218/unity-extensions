using System;
using UnityEngine;

namespace UnityBoosts
{
    public class UnityTypePropertyAttribute : PropertyAttribute
    {
        public UnityTypePropertyAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}