using UnityEngine;
using UnityEngine.UI;

namespace UnityExtensions
{
    public class LoadGraphicRaycasterAction : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private ScriptableObjectGraphicRaycaster sRaycaster;

        public void Invoke()
        {
            sRaycaster.Impl = raycaster;
        }
    }
}