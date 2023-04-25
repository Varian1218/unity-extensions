using Numerics;
using UnityEngine;

namespace UnityBoosts
{
    public static class TransformExtension
    {
        public static void SetLocalScale(this Transform transform, Double3 value)
        {
            transform.localScale = value.ToVector3();
        }

        public static void SetLocalScale(this Transform transform, Vector3 value)
        {
            transform.localScale = value;
        }
    }
}