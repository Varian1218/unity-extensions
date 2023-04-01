using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    [Serializable]
    public class UnityFuncBase : ISerializationCallbackReceiver
    {
        [SerializeField] private Object target;

        [SerializeField] private string methodName;

//         // [SerializeField] protected Arg[] _args;
        [SerializeField] private bool dynamic;
#pragma warning disable 0414
        [SerializeField] private string typeName;
#pragma warning restore 0414
         [SerializeField] private bool dirty;
#if UNITY_EDITOR
        protected UnityFuncBase()
        {
            typeName = GetType().AssemblyQualifiedName;
        }

        public void OnBeforeSerialize()
        {
            if (!dirty) return;
            dirty = false;
        }

        public void OnAfterDeserialize()
        {
            typeName = GetType().AssemblyQualifiedName;
        }
#endif
        internal Func<T> Create<T>()
        {
            var method = target.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
                null,
                CallingConventions.Any,
                Array.Empty<Type>(), //types,
                null
            ) ?? throw new NullReferenceException();
            return Delegate.CreateDelegate(typeof(Func<T>), target, method) is Func<T> t ? t : throw new Exception();
        }
    }
}