using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Sprite Map", menuName = "Unity Extensions/Sprite Map", order = 1)]
    public class ScriptableObjectSpriteMap : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private ObjectPair<Sprite>[] sprites;
        private Dictionary<string, Sprite> _sprites;
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _sprites = sprites.ToDictionary(it => it.hash, it => it.obj);
        }
    }
}