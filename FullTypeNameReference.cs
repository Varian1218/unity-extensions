using System;
using UnityEngine;

namespace UnityExtensions
{
    [Serializable]
    public class FullTypeNameReference
    {
        [SerializeField] private string guid;
        [SerializeField] private string value;
#pragma warning restore 0414
        [SerializeField] private bool dirty;
        public string Value => value;
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (!dirty) return;
            dirty = false;
        }
#endif
    }
}