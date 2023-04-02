using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityExtensions
{
    public class UnityGraphicRaycaster : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private ScriptableObjectGraphicRaycaster sRaycaster;

        private void Awake()
        {
            sRaycaster.Impl = raycaster;
        }
    }
}