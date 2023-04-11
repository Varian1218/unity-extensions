using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Load Map Action", menuName = "Unity Extensions/Load Map Action", order = 1)]
    public class ScriptableObjectArrayToMapAction : ScriptableObject
    {
        [Serializable]
        private struct MyStruct
        {
            public UnityEvent<Sprite, string> add;
            public UnityFunc<IEnumerable<(Sprite, string)>> query;
        }

        [SerializeField] private MyStruct[] actions;
        
        public void Invoke()
        {
            foreach (var action in actions)
            {
                foreach (var (sprite, spriteHash) in action.query.Invoke())
                {
                    action.add.Invoke(sprite, spriteHash);
                }
            }
        }
    }
}