using UnityEngine;

namespace UnityExtensions
{
    public static class UnityVectorExtension
    {
        public static Vector2 ToVector2(this System.Numerics.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static Vector3 ToVector3(this System.Numerics.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static System.Numerics.Vector3 ToVector3(this Vector3 value)
        {
            return new System.Numerics.Vector3(value.x, value.y, value.z);
        }
    }
}