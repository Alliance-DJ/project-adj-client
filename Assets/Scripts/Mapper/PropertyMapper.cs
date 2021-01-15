using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PropertyMapper : MonoBehaviour
{
    public string defaultValue;
    public string format;
    public string propertyName, subPropertyName;

    private static readonly Dictionary<Type, Dictionary<string, IReflectionGet>> cached = new Dictionary<Type, Dictionary<string, IReflectionGet>>();

    private interface IReflectionGet
    {
        public object GetValue(object o);
    }

    private class FieldGet : IReflectionGet
    {
        public FieldInfo Field;

        public FieldGet(FieldInfo field) => Field = field;

        public object GetValue(object o) => Field.GetValue(o);
    }

    private class PropertyGet : IReflectionGet
    {
        public PropertyInfo Property;

        public PropertyGet(PropertyInfo property) => Property = property;

        public object GetValue(object o) => Property.GetValue(o);
    }

    private IReflectionGet GetReflectionGet(Type type, string name)
    {
        if (cached.TryGetValueDicDic(type, name, out IReflectionGet r))
        {
            return r;
        }

        var field = type.GetField(name);
        if (field != null)
        {
            r = new FieldGet(field);
            cached.AddDicDic(type, name, r);
            return r;
        }

        var property = type.GetProperty(name);
        if (property != null)
        {
            r = new PropertyGet(property);
            cached.AddDicDic(type, name, r);
            return r;
        }

        cached.AddDicDic(type, name, null);
        return null;
    }

    public void ExtractValue(BaseData data)
    {
        if (data == null || string.IsNullOrEmpty(propertyName)) return;

        object value = null;
        var type = data.GetType();
        var get = GetReflectionGet(type, propertyName);
        if (get is FieldGet field)
        {
            value = field.GetValue(data);
        }
        else if (get is PropertyGet property)
        {
            value = property.GetValue(data);
            if (value != null)
            {
                var subType = value.GetType();
                if (!string.IsNullOrEmpty(subPropertyName) && subType == typeof(BaseData))
                {
                    get = GetReflectionGet(subType, subPropertyName);
                    if (get != null)
                        value = get.GetValue(value);
                }
            }
        }

        SetPropertyValue(value);
    }

    public string GetFormattedString(object o)
    {
        string ret = o.ToString();

        if (string.IsNullOrEmpty(ret))
        {
            ret = defaultValue;
        }

        if (string.IsNullOrEmpty(ret))
        {
            return string.Empty;
        }

        if (!string.IsNullOrEmpty(format))
        {
            ret = string.Format(format, ret);
        }

        return ret;
    }

    // Editor Àü¿ë
    public Type GetDataMapperDataType()
    {
        DataMapper dm = null;
        Transform t = transform;
        while (t != null && dm == null)
        {
            t = t.parent;
            if (t != null) dm = t.GetComponent<DataMapper>();
        }

        if (t == null || dm == null)
        {
            Debug.LogError("No DataMapper (this): " + gameObject.name, this);
            return null;
        }

        if (string.IsNullOrEmpty(dm.InspectorDataType)) return null;

        return DataTypes.maps[dm.InspectorDataType];
    }

    public T GetDataType<T>() where T : BaseData
    {
        DataMapper dm = null;
        Transform t = transform;
        while (t != null && dm == null)
        {
            t = t.parent;
            if (t != null) dm = t.GetComponent<DataMapper>();
        }

        if (t == null)
        {
            Debug.LogError("No DataMapper (this): " + gameObject.name, this);
            return null;
        }

        var ret = dm.GetData<T>();
        return ret;
    }

    public virtual void SetPropertyValue(object v)
    {
        if (v == null) return;

        var components = GetComponents<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++)
        {
            var c = components[i];
            if (c == null) continue;

            if (c is DataMapper mapper)
            {
                mapper.SetData(v as BaseData);
                return;
            }
            else if (c is Text txt)
            {
                SetText(txt, v);
                continue;
            }
        }
    }

    private void SetText(Text label, object o)
    {
        if (label == null) return;

        string v = o.ToString();
        if (string.IsNullOrEmpty(v))
        {
            v = defaultValue;
        }
        else if (!string.IsNullOrEmpty(format))
        {
            v = string.Format(format, o);
        }

        label.text = v;
    }
}