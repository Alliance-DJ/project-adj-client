using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

public class DataMapper : MonoBehaviour
{
    [SerializeField, Inherits(typeof(BaseData), SearchbarMinItemsCount = 0)]
    private TypeReference dataType;
    public TypeReference DataType => dataType;

    private HashSet<PropertyMapper> propertyMappers;

    public BaseData Data { get; private set; }

    private void Awake()
    {
        propertyMappers = GetPropertyMappers();
    }

    public HashSet<PropertyMapper> GetPropertyMappers()
    {
        var mappers = new HashSet<PropertyMapper>();

        var pMappers = GetComponentsInChildren<PropertyMapper>(true);
        for (int i = 0; i < pMappers.Length; i++)
        {
            var mapper = pMappers[i];
            if (!mapper.IsValid()) continue;

            var obj = mapper.gameObject;
            if (obj == gameObject) continue;

            mappers.Add(mapper);
        }

        var dMappers = GetComponentsInChildren<DataMapper>(true);
        for (int i = 0; i < dMappers.Length; i++)
        {
            var mapper = dMappers[i];
            if (!mapper.IsValid()) continue;

            var obj = mapper.gameObject;
            if (obj == gameObject) continue;

            var subPropertyMappers = mapper.GetComponentsInChildren<PropertyMapper>(true);
            for (int j = 0; j < subPropertyMappers.Length; j++)
            {
                var subMapper = subPropertyMappers[j];
                if (subMapper == null) continue;

                var subObj = subMapper.gameObject;
                if (subObj == obj) continue;

                mappers.Remove(subMapper);
            }
        }

        return mappers;
    }

    [ContextMenu("Data Refresh")]
    public void RefreshData()
    {
        SetData(Data);
    }

    public void SetData<T>(T data) where T : BaseData
    {
        if (data != null)
        {
            if (dataType == null)
            {
                Debug.LogError($"Not Set Type", gameObject);
                return;
            }

            var type = data.GetType();
            if (type != dataType.Type)
            {
                Debug.LogError($"Not Match Type (SetType : {type} |  InspectorType : {dataType})", gameObject);
                return;
            }
        }

        Data = data;

        if (propertyMappers != null)
        {
            foreach (var mapper in propertyMappers)
            {
                if (!mapper.IsValid()) continue;

                mapper.Init(Data);
            }
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
                Debug.LogError($"Not Match Type (DataType : {Data})", gameObject);
                return null;
        }
    }
}