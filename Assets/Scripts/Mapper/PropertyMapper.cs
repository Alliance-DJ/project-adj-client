using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class PropertyMapper : MonoBehaviour
{
    public string defaultValue;
    public string format;
    public string propertyName, subPropertyName;

    private static readonly Dictionary<Type, Dictionary<string, IReflectionGet>> cached =
        new Dictionary<Type, Dictionary<string, IReflectionGet>>();

    private interface IReflectionGet
    {
        public object GetValue(object o);
    }

    private class FieldGet : IReflectionGet
    {
        private readonly FieldInfo fieldInfo;

        public FieldGet(FieldInfo field) => fieldInfo = field;

        public object GetValue(object o) => fieldInfo.GetValue(o);
    }

    private class PropertyGet : IReflectionGet
    {
        private readonly PropertyInfo propertyInfo;

        public PropertyGet(PropertyInfo property) => propertyInfo = property;

        public object GetValue(object o) => propertyInfo.GetValue(o);
    }

    private static IReflectionGet GetReflectionGet(Type type, string pName)
    {
        if (cached.TryGetValueDicDic(type, pName, out var r))
        {
            return r;
        }

        var field = type.GetField(pName);
        if (field != null)
        {
            r = new FieldGet(field);
            cached.AddDicDic(type, pName, r);
            return r;
        }

        var property = type.GetProperty(pName);
        if (property != null)
        {
            r = new PropertyGet(property);
            cached.AddDicDic(type, pName, r);
            return r;
        }

        cached.AddDicDic(type, pName, null);
        return null;
    }

    public void ExtractValue(BaseData data)
    {
        if (data == null || string.IsNullOrEmpty(propertyName)) return;

        object value = null;
        var type = data.GetType();
        var get = GetReflectionGet(type, propertyName);
        switch (get)
        {
            case FieldGet field:
                value = field.GetValue(data);
                break;
            case PropertyGet property:
            {
                value = property.GetValue(data);
                if (value == null) break;

                var subType = value.GetType();
                if (string.IsNullOrEmpty(subPropertyName) || subType != typeof(BaseData)) break;

                get = GetReflectionGet(subType, subPropertyName);
                if (get == null) break;
                
                value = get.GetValue(value);
                break;
            }
        }

        SetPropertyValue(value);
    }

    public string GetFormattedString(object o)
    {
        var ret = o.ToString();

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

    // Editor 전용
    public Type GetDataMapperDataType()
    {
        DataMapper dm = null;
        var t = transform;
        while (t != null && dm == null)
        {
            t = t.parent;
            if (t != null) dm = t.GetComponent<DataMapper>();
        }

        if (t != null && dm != null)
            return string.IsNullOrEmpty(dm.InspectorDataType) ? null : DataTypes.maps[dm.InspectorDataType];

        Debug.LogError("No DataMapper (this): " + gameObject.name, this);
        return null;
    }

    public void ResetPropertyMapper()
    {
        defaultValue = string.Empty;
        format = string.Empty;
        propertyName = string.Empty;
        subPropertyName = string.Empty;
    }

    public T GetDataType<T>() where T : BaseData
    {
        DataMapper dm = null;
        var t = transform;
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

        if (dm == null)
        {
            return null;
        }

        var ret = dm.GetData<T>();
        return ret;
    }

    protected virtual void SetPropertyValue(object v)
    {
        if (v == null) return;

        var components = GetComponents<MonoBehaviour>();
        foreach (var c in components)
        {
            if (c == null) continue;

            switch (c)
            {
                case DataMapper mapper:
                    mapper.SetData(v as BaseData);
                    return;
                case Text txt:
                    SetText(txt, v);
                    continue;
            }
        }
    }

    private void SetText(Text label, object o)
    {
        if (label == null) return;

        var v = o.ToString();
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