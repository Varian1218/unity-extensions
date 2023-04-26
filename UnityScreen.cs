using System;
using UnityEngine;

namespace UnityBoosts
{
    public class UnityScreen
    {
        private int _height;
        private int _width;

        public void Step()
        {
            if (_height == Screen.height && _width == Screen.width) return;
            _height = Screen.height;
            _width = Screen.width;
            SizeChanged?.Invoke(_height, _width);
        }

        public event Action<int, int> SizeChanged;
    }
}