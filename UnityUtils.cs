using UnityEngine;

namespace UnityExtensions
{
    public static class UnityUtils
    {
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}