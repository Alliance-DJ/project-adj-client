using UnityEngine;

[RequireComponent(typeof(DataMapper))]
public class DataSetter : PropertyMapper
{
    protected override void OnValueChanged()
    {
        if (TryGetComponent<DataMapper>(out var mapper))
        {
            mapper.SetData(Value as BaseData);
        }
    }
}
