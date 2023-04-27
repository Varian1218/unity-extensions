using UnityEngine;

namespace UnityBoosts
{
    public static class MaterialUtility
    {
        public static void SetAlpha(this Material material, float value)
        {
            var color = material.color;
            color.a = value;
            material.color = color;
        }
    }
}