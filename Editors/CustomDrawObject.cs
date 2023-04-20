using System;
using System.Linq;
using System.Reflection;

namespace UnityExtensions.Editors
{
    public class CustomDrawObject : DrawObject
    {
        private FieldInfo[] _fieldInfos;
        private DrawObject[] _objects;
        private Type _type;
        public int Length => _objects.Length;

        public override Type Type
        {
            get => _type;
            set
            {
                _type = value;
                _fieldInfos = value.GetFields();
                // _objects = _fieldInfos.Select(it => new DrawObject
                // {
                //     Impl = GetValueCreateIfNull(it, Impl),
                //     Label = it.Name,
                //     Type = it.FieldType
                // }).ToArray();
                _objects = _fieldInfos.Select(it => TypeDrawer.CreateObject(
                    it.Name,
                    GetValueCreateIfNull(it, Impl),
                    it.FieldType
                )).ToArray();
            }
        }
        
        public DrawObject this[int index]
        {
            get => _objects[index];
            set
            {
                _fieldInfos[index].SetValue(Impl, value.Impl);
                _objects[index] = value;
            }
        }

        private static object GetValueCreateIfNull(FieldInfo fieldInfo, object obj)
        {
            var value = fieldInfo.GetValue(obj);
            if (value != null) return value;
            value = fieldInfo.FieldType == typeof(string) ? "" : Activator.CreateInstance(fieldInfo.FieldType);
            fieldInfo.SetValue(obj, value);
            return value;
        }
    }
}