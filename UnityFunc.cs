using System;

namespace UnityBoosts
{
    [Serializable]
    public class UnityFunc<T> : UnityFuncBase
    {
        private Func<T> _delegate;
        public T Invoke()
        {
            _delegate ??= Create<T>();
            return _delegate();
        }
    }
}