using UnityEngine;

namespace UnityBoosts
{
    public static class UnityRendererUtility
    {
        public static void SetAlpha(this Renderer renderer, float value)
        {
            var material = renderer.material;
            var color = material.color;
            color.a = value;
            material.color = color;
        }
    }
}