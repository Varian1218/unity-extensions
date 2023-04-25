using System;
using System.Collections.Generic;
using CSharpBoosts;

namespace UnityBoosts.Editors
{
    public class ArrayDrawObject : DrawObject
    {
        private List<DrawObject> _elements;
        private Type _elementType;
        private Array _impl;
        private Type _type;

        public override object Impl
        {
            get => _impl;
            set
            {
                _impl = value as Array ?? throw new NullReferenceException(Label);
                _elements = new List<DrawObject>();
                // foreach (var it in _impl)
                for (var i = 0; i < _impl.Length; i++)
                {
                    _elements.Add(TypeDrawer.CreateObject($"{i}", _impl.GetValue(i), _elementType)); //new DrawObject
                    // {
                    //     Impl = it
                    // });
                }
            }
        }

        public int Length => _elements.Count;

        public override Type Type
        {
            get => _type;
            set
            {
                _elementType = value.GetElementType();
                _type = value;
            }
        }

        public DrawObject this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        public void Add()
        {
            _impl = ArrayUtils.Clone(_impl, _impl.Length + 1);
            _elements.Add(new DrawObject
            {
                Impl = _impl.GetValue(_impl.Length - 1)
            });
        }

        public void RemoveIndex(int index)
        {
            _impl = ArrayUtils.CloneIgnore(_impl, index);
            _elements.RemoveAt(index);
        }
    }
}