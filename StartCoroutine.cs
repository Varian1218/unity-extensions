using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBoosts
{
    public class StartCoroutine : MonoBehaviour
    {
        [SerializeField] private List<UnityFunc<IEnumerator>> routines;

        public void Invoke()
        {
            StartCoroutine(InvokeAsync());
        }

        public IEnumerator InvokeAsync()
        {
            return routines.Select(routine => routine.Invoke()).GetEnumerator();
        }
    }
}