using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataMapper : MonoBehaviour
{
    public Type DataType;

    private HashSet<PropertyMapper> propertyMappers;

    public BaseData Data { get; private set; }

    public HashSet<PropertyMapper> GetPropertyMappers()
    {
        var mappers = new HashSet<PropertyMapper>();

        var pMappers = GetComponentsInChildren<PropertyMapper>(true);
        foreach (var mapper in pMappers)
        {
            if (mapper == null) continue;

            mappers.Add(mapper);
        }

        var dMappers = GetComponentsInChildren<DataMapper>(true);
        foreach (var mapper in dMappers)
        {
            if (mapper == null) continue;

            var dgo = mapper.gameObject;
            if (dgo == gameObject) continue;

            var subPropertyMappers = mapper.GetComponentsInChildren<PropertyMapper>(true);
            foreach (var subMapper in subPropertyMappers)
            {
                if (subMapper == null || dgo == subMapper.gameObject) continue;

                mappers.Remove(subMapper);
            }
        }

        return mappers;
    }

    public void Reload()
    {
        if (Data == null) return;

        SetData(Data);
    }

    public void SetData<T>(T data) where T : BaseData
    {
        if (data == null || DataType == null) return;

        var type = data.GetType();
        if (type != DataType)
        {
            Debug.LogError($"NOT MATCH TYPE (SetType : {type} |  InspectorType : {DataType})", gameObject);
            return;
        }

        Data = data;

        if (propertyMappers == null)
            propertyMappers = GetPropertyMappers();

        foreach (var mapper in propertyMappers.Where(mapper => mapper != null))
        {
            mapper.ExtractValue(data);
        }
    }

    public T GetData<T>() where T : BaseData
    {
        switch (Data)
        {
            case null:
                return null;
            case T d:
                return d;
            default:
                Debug.LogError("NOT MATCH TYPE");
                return null;
        }
    }

#if UNITY_EDITOR

    [ContextMenu("Data Refresh")]
    public void DataRefresh()
    {
        Reload();
    }

#endif
}