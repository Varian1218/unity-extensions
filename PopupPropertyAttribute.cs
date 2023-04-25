using UnityEngine;

namespace UnityBoosts
{
    public class PopupPropertyAttribute : PropertyAttribute
    {
        public string[] Options { get; }

        public PopupPropertyAttribute(params string[] options)
        {
            Options = options;
        }
    }
}