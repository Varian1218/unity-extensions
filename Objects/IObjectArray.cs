using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace UnityBoosts
{
    public interface IObjectArray : IEnumerable<Object>
    {
        Type Type { get; }
    }
}