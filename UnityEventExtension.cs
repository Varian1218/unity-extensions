using Numerics;
using UnityEngine;
using UnityEngine.Events;

namespace UnityBoosts
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