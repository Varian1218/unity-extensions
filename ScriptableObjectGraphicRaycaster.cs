using UnityEngine;
using UnityEngine.UI;

namespace UnityBoosts
{
    [CreateAssetMenu(menuName = "Unity Extensions/Graphic Raycaster", fileName = "Graphic Raycaster", order = 1)]
    public class ScriptableObjectGraphicRaycaster : ScriptableObject, IGraphicRaycaster
    {
        private GraphicRaycaster _impl;

        public bool Enabled
        {
            set => _impl.enabled = value;
        }

        public GraphicRaycaster Impl
        {
            set => _impl = value;
        }
    }
}