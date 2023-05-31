using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBoosts
{
    public static class UnityInputUtils
    {
        public static bool GetKeyDown(IEnumerable<KeyCode> keys)
        {
            return keys.Any(Input.GetKeyDown);
        }
        
        public static bool GetKeyUp(IEnumerable<KeyCode> keys)
        {
            return keys.Any(Input.GetKeyDown);
        }
    }
}