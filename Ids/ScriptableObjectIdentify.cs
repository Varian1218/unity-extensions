using CSharpBoosts;

namespace UnityBoosts.Ids
{
    public class ScriptableObjectIdentify : IId
    {
        private IId _impl;

        public IId Impl
        {
            set => _impl = value;
        }

        public int Value => _impl.Value;
    }
}