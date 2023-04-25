using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityBoosts.Editors
{
    public class DictionaryDrawObject : DrawObject
    {
        private IDictionary _impl;
        private Dictionary<object, DrawObject> _keys;
        private Type _type;
        private Dictionary<object, DrawObject> _values;

        public override object Impl
        {
            get => _impl;
            set
            {
                _impl = value as IDictionary ?? throw new NullReferenceException(Label);
                _keys = new Dictionary<object, DrawObject>();
                _values = new Dictionary<object, DrawObject>();
                foreach (var key in _impl.Keys)
                {
                    var val = _impl[key];
                    _keys.Add(key, TypeDrawer.CreateObject(null, key, KeyType));// new DrawObject
                    // {
                    //     Impl = key,
                    //     Type = KeyType
                    // });
                    _values.Add(key, TypeDrawer.CreateObject(null, val, ValueType)); // new DrawObject
                    // {
                    //     Impl = val,
                    //     Type = ValueType
                    // });
                }
            }
        }

        public IEnumerable Keys => _keys.Values;
        public Type KeyType { get; private set; }
        public Type ValueType { get; private set; }

        public override Type Type
        {
            get => _type;
            set
            {
                var argTypes = value.GetGenericArguments();
                KeyType = argTypes[0];
                ValueType = argTypes[1];
                _type = value;
            }
        }

        // public override Type Type
        // {
        //     get => _type;
        //     set
        //     {
        //         _dictionary = Activator.CreateInstance(value) as IDictionary;
        //         _type = value;
        //     }
        // }

        public DrawObject this[object key]
        {
            get => _values[key];
            set
            {
                _impl[key] = value.Impl;
                _values[key] = value;
            }
        }

        public void Add(object key, object value)
        {
            _impl.Add(key, value);
            _keys.Add(key, TypeDrawer.CreateObject(null, key, KeyType));
            // {
            //     Impl = key,
            //     Type = KeyType
            // });
            _values.Add(key, TypeDrawer.CreateObject(null, value, ValueType)); //new DrawObject
            // {
            //     Impl = value,
            //     Type = ValueType
            // });
            // _keys.Add(key, new DrawObject
            // {
            //     Impl = key,
            //     Type = KeyType
            // });
            // _values.Add(key, new DrawObject
            // {
            //     Impl = value,
            //     Type = ValueType
            // });
        }

        public void Remove(object key)
        {
            _impl.Remove(key);
            _keys.Remove(key);
            _values.Remove(key);
        }
    }
}