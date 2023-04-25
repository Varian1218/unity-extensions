using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityBoosts
{
    public class PopupPropertyAttribute : PropertyAttribute
    {
        public string[] Options { get; }

        public PopupPropertyAttribute(Type type)
        {
            Options = type.GetFields(BindingFlags.Public | BindingFlags.Static).Select(it => (string)it.GetValue(null))
                .ToArray();
        }

        public PopupPropertyAttribute(params string[] options)
        {
            Options = options;
        }
    }
}