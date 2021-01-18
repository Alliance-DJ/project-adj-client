#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(DataMapper))]
public class DataMapperEditor : Editor
{
    private const string NONE = "None";

    private string searchKey;

    private DataMapper mapper;

    private List<string> dataTypeNames;

    private void OnEnable()
    {
        mapper = target as DataMapper;
        if (mapper == null) return;

        dataTypeNames = new List<string>();

        var dataTypes = TypeCache.GetSubClasses<BaseData>();
        if (dataTypes != null)
        {
            for (int i = 0; i < dataTypes.Count; i++)
            {
                var type = dataTypes[i];
                if (type == null) continue;

                dataTypeNames.Add(type.Name);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        if (mapper == null) return;

        var types = new List<string>(dataTypeNames);
        types.Sort();

        searchKey = EditorGUILayout.TextField("Search Model : ", searchKey);
        var searching = !string.IsNullOrEmpty(searchKey);
        if (searching)
            types.RemoveAll(name => !name.ToLower().Contains(searchKey.ToLower()));

        types.Insert(0, NONE);

        var currentTypeName = !string.IsNullOrEmpty(mapper.InspectorDataType) ? mapper.InspectorDataType : NONE;
        var index = types.FindIndex(type => type == currentTypeName);
        index = EditorGUILayout.Popup(index, types.ToArray());

        if (!searching || index >= 0)
            mapper.InspectorDataType = types[index] != NONE ? types[index] : null;

        EditorGUILayout.LabelField("Selected Data Class : ", !string.IsNullOrEmpty(mapper.InspectorDataType) ? mapper.InspectorDataType : NONE);
    }
}

#endif