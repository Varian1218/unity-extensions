using UnityEngine;

namespace UnityExtensions
{
    public static class CanvasGroupExtension
    {
        public static void SetAlpha(this CanvasGroup canvasGroup, double value)
        {
            canvasGroup.alpha = (float)value;
        }
    }
}