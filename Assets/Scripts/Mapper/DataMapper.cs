using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataMapper : MonoBehaviour
{
    public string inspectorDataType;

    private BaseData data;

    private HashSet<PropertyMapper> propertyMappers;

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
        if (data == null) return;

        SetData(data);
    }

    public void SetData<T>(T any) where T : BaseData
    {
        if (any == null) return;

        if (inspectorDataType == null || any.GetType().Name != inspectorDataType)
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

        foreach (var mapper in propertyMappers.Where(mapper => mapper != null))
        {
            mapper.ExtractValue(data);
        }
    }

    public T GetData<T>() where T : BaseData
    {
        switch (data)
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