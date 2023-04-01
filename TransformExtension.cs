using Doubles;
using UnityEngine;

namespace UnityExtensions
{
    public static class TransformExtension
    {
        public static void SetLocalScale(this Transform transform, Double3 value)
        {
            transform.localScale = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
        }

        public static void SetLocalScale(this Transform transform, Vector3 value)
        {
            transform.localScale = value;
        }
    }
}