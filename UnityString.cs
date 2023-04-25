using System;
using UnityEngine;

namespace UnityBoosts
{
    [Serializable]
    public class UnityString
    {
        [SerializeField] private string value;

        public static implicit operator string(UnityString s)
        {
            return s.value;
        }
    }
}