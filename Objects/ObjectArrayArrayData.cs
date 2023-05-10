using System;
using Object = UnityEngine.Object;

namespace UnityBoosts.Objects
{
    [Serializable]
    public class ObjectArrayArrayData
    {
        public Object[] objects;

        [PopupProperty(ObjectArrayHash.ObjectArray, ObjectArrayHash.TextAsset)]
        public string type;
    }
}