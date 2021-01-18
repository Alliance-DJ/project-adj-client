using System.Collections.Generic;
using UnityEngine;

public class DataMapper : MonoBehaviour
{
    public string InspectorDataType;

    private BaseData data;

    private HashSet<PropertyMapper> propertyMappers;

    public HashSet<PropertyMapper> GetPropertyMappers()
    {
        var mappers = new HashSet<PropertyMapper>();

        var pMappers = GetComponentsInChildren<PropertyMapper>(true);
        for (int i = 0; i < pMappers.Length; i++)
        {
            var mapper = pMappers[i];
            if (mapper == null) continue;

            mappers.Add(mapper);
        }

        var dMappers = GetComponentsInChildren<DataMapper>(true);
        for (int i = 0; i < dMappers.Length; i++)
        {
            var mapper = dMappers[i];
            if (mapper == null) continue;

            var dgo = mapper.gameObject;
            if (dgo == gameObject) continue;

            var subPropertyMappers = mapper.GetComponentsInChildren<PropertyMapper>(true);
            for (int j = 0; j < subPropertyMappers.Length; j++)
            {
                var subMapper = subPropertyMappers[j];
                if (subMapper == null || dgo == subMapper.gameObject) continue;

                mappers.Remove(subMapper);
            }
        }

        return mappers;
    }

    public void Reload()
    {
        if (data == null) return;

        SetData(data);
    }

    public void SetData<T>(T any) where T : BaseData
    {
        if (any == null) return;

        if (InspectorDataType == null || any.GetType().Name != InspectorDataType)
        {
            Debug.LogError("NOT MATCH TYPE");
            return;
        }

        data = any;

        if (propertyMappers == null)
        {
            var pMappers = GetPropertyMappers();
            propertyMappers = new HashSet<PropertyMapper>(pMappers);
        }

        foreach (var mapper in propertyMappers)
        {
            if (mapper == null) continue;

            mapper.ExtractValue(data);
        }
    }

    public T GetData<T>() where T : BaseData
    {
        if (data == null)
        {
            return null;
        }

        var d = data as T;
        if (d == null)
        {
            Debug.LogError("NOT MATCH TYPE");
            return null;
        }

        return d;
    }

#if UNITY_EDITOR

    [ContextMenu("Data Refresh")]
    public void DataRefresh()
    {
        Reload();
    }

#endif
}