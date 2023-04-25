using System.Collections.Generic;

namespace UnityExtensions
{
    public interface IObjectDatabase
    {
        IEnumerable<T> Query<T>() where T : class;
        IEnumerable<T> Query<T>(string hash) where T : class;
    }
}