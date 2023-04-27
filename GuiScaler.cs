using UnityEngine;

namespace UnityBoosts
{
    public class GuiScaler
    {
        // private readonly float _height;
        // private readonly float _width;
        //
        // public GuiScaler() : this(480f, 640f)
        // {
        // }
        //
        // public GuiScaler(float height, float width)
        // {
        //     _height = height;
        //     _width = width;
        // }
        //
        // public void Step()
        // {
        //     Step(Screen.height, Screen.width);
        // }
        //
        // public void Step(int height, int width)
        // {
        //     GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(
        //         width / _width,
        //         height / _height,
        //         1f
        //     ));
        // }
        public static void Step()
        {
            Step(480f, Screen.height, 640f, Screen.width);
        }

        public static void Step(float h, int height, float w, int width)
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(
                width / w,
                height / h,
                1f
            ));
        }
    }
}