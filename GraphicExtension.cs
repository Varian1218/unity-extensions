using UnityEngine.UI;

namespace UnityExtensions
{
    public static class GraphicExtension
    {
        public static void SetAlpha(this Graphic graphic, double alpha)
        {
            var color = graphic.color;
            color.a = (float)alpha;
            graphic.color = color;
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}