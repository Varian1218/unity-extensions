using Doubles;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtensions
{
    public static class UnityEventExtension
    {
        public static void Invoke(this UnityEvent<float> unityEvent, double value)
        {
            unityEvent.Invoke((float)value);
        }

        public static void Invoke(this UnityEvent<Vector3> unityEvent, Double3 value)
        {
            unityEvent.Invoke(value.ToVector3());
        }
    }
}