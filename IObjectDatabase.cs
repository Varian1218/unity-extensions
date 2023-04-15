using System.Collections.Generic;

namespace UnityExtensions
{
    public interface IObjectDatabase
    {
        IEnumerable<T> Query<T>();
    }
}