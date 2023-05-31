using System;
using UnityEngine;

namespace UnityBoosts
{
    public class UnityHoldInput
    {
        public KeyCode[] KeyCodes { private get; set; }

        public KeyCode KeyCode
        {
            set => KeyCodes = new[] { value };
        }

        public event Action<string> Ended;
        private bool _hold;
        private string _input;

        public void Step()
        {
            if (UnityInputUtils.GetKeyDown(KeyCodes))
            {
                _hold = true;
            }

            if (!_hold) return;
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                _input += Input.inputString;
            }

            if (!UnityInputUtils.GetKeyUp(KeyCodes)) return;
            _hold = false;
            Ended?.Invoke(_input);
            _input = string.Empty;
        }
    }
}