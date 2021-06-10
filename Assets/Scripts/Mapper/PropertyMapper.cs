using UnityEngine;
using UnityEngine.UI;

public class PropertyMapper : MonoBehaviour
{
    public string defaultValue;
    public string format;
    public string propertyName, subPropertyName;

    private object ReflectionValue(object obj, string pName)
    {
        if (string.IsNullOrEmpty(pName) || obj == null) return null;

        var type = obj.GetType();
        var field = type.GetField(pName);
        if (field != null)
        {
            return field.GetValue(obj);
        }

        var property = type.GetProperty(pName);
        if (property != null)
        {
            return property.GetValue(obj);
        }

        Debug.LogError($"Not Valid Property or Sub Property (Type : {type} | Name : {pName})", gameObject);
        return null;
    }

    public void ExtractValue<T>(T data) where T : BaseData
    {
        object value = null;

        if (!string.IsNullOrEmpty(propertyName) && data.IsValid())
        {
            value = ReflectionValue(data, propertyName);
        }

        if (!string.IsNullOrEmpty(subPropertyName) && value.IsValid())
        {
            var subType = value.GetType();
            if (subType.InheritsFrom(typeof(BaseData)))
                value = ReflectionValue(value, subPropertyName);
        }

        SetPropertyValue(value);
    }

    public string FormattingString(object o)
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

    protected virtual void SetPropertyValue(object v)
    {
        if (v == null)
        {
            v = defaultValue;
        }

        var components = GetComponents<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++)
        {
            var component = components[i];
            if (component == null) continue;

            switch (component)
            {
                case DataMapper mapper:
                    mapper.SetData(v as BaseData);
                    continue;
                case Text txt:
                    txt.text = FormattingString(v);
                    continue;
            }
        }
    }
}