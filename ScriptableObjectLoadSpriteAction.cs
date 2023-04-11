using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Load Sprite Action", menuName = "Unity Extensions/Load Sprite Action", order = 1)]
    public class ScriptableObjectLoadSpriteAction : ScriptableObject,
        IEnumerable<(ScriptableObjectSpriteArray Array, ScriptableObjectSpriteDictionary Dictionary)>
    {
        [Serializable]
        private struct Pair
        {
            public ScriptableObjectSpriteArray array;
            public ScriptableObjectSpriteDictionary dictionary;
        }

        [SerializeField] private Pair[] pairs;

        public void Invoke()
        {
            foreach (var pair in pairs)
            {
                foreach (var (sprite, spriteHash) in pair.array)
                {
                    pair.dictionary.Add(spriteHash, sprite);
                }
            }
        }

        IEnumerator<(ScriptableObjectSpriteArray, ScriptableObjectSpriteDictionary)>
            IEnumerable<(ScriptableObjectSpriteArray Array, ScriptableObjectSpriteDictionary Dictionary)>.
            GetEnumerator()
        {
            return pairs.Select(it => (it.array, it.dictionary)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        public void Set(
            IEnumerable<(ScriptableObjectSpriteArray Array, ScriptableObjectSpriteDictionary Dictionary)> value)
        {
            pairs = value.Select(it => new Pair
            {
                array = it.Array,
                dictionary = it.Dictionary
            }).ToArray();
        }
    }
}