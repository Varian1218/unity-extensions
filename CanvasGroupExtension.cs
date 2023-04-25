using UnityEngine;

namespace UnityBoosts
{
    public static class CanvasGroupExtension
    {
        public static void SetAlpha(this CanvasGroup canvasGroup, double value)
        {
            canvasGroup.alpha = (float)value;
        }
    }
}