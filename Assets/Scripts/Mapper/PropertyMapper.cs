using UnityEngine;

public class PropertyMapper : MonoBehaviour
{
    [HideInInspector]
    public string propertyName, subPropertyName;

    private object value;
    public object Value
    {
        get => value;
        protected set
        {
            if (Equals(value, this.value)) return;

            this.value = value;
            OnValueChanged();
        }
    }

    private object GetValue(string propertyName, object data)
    {
        if (string.IsNullOrEmpty(propertyName) || !data.IsValid()) return null;

        var type = data.GetType();
        if (!type.InheritsFrom(typeof(BaseData))) return null;

        var info = type.GetNodeTypeInfo(propertyName);
        if (!info.IsValid()) return null;

        return info.GetValue(data);
    }

    public void Init(object data)
    {
        object value = GetValue(propertyName, data);

        if (!string.IsNullOrEmpty(subPropertyName))
        {
            value = GetValue(subPropertyName, value);
        }

        Value = value;
    }

    protected virtual void OnValueChanged() { }
}