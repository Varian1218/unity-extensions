using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Text Asset Array", menuName = "Unity Extensions/Text Asset Array", order = 1)]
    public class ScriptableObjectTextAssetArray : ScriptableObject, IEnumerable<TextAsset>
    {
        [SerializeField] private TextAsset[] textAssets;

        public IEnumerator<TextAsset> GetEnumerator()
        {
            return textAssets.Select(it => it).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return textAssets.GetEnumerator();
        }
    }
}