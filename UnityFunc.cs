using System;

namespace UnityExtensions
{
    [Serializable]
    public class UnityFunc<T> : UnityFuncBase
    {
        private Func<T> _delegate;
        public T Invoke()
        {
            _delegate ??= Create<Func<T>>();
            return _delegate();
        }
    }
}