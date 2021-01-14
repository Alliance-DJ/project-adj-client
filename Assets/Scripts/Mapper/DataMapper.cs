using UnityEngine;

public class DataMapper : MonoBehaviour
{
    public string InspectorDataType;

    private BaseData data;

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
