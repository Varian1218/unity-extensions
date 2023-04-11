using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Sprite Array", menuName = "Unity Extensions/Sprite Array", order = 1)]
    public class ScriptableObjectSpriteArray : ScriptableObject, IEnumerable<(Sprite, string)>
    {
        [SerializeField] private Sprite[] sprites;

        public IEnumerator<(Sprite, string)> GetEnumerator()
        {
            return sprites.Select(it => (it, it.name)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sprites.GetEnumerator();
        }
    }
}