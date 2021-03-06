#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataMapper))]
public class DataMapperEditor : Editor
{
    private const string None = "None";

    private string searchKey;
    private int currentIndex = -1;

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
            Parallel.ForEach(dataTypes, (type) => { dataTypeNames.Add(type.Name); });
        }
    }

    public override void OnInspectorGUI()
    {
        if (mapper == null) return;

        var types = new List<string>(dataTypeNames);
        types.Sort();

        searchKey = EditorGUILayout.TextField("Search Data : ", searchKey);
        var searching = !string.IsNullOrEmpty(searchKey);
        if (searching)
            types.RemoveAll(n => !n.ToLower().Contains(searchKey.ToLower()));

        types.Insert(0, None);

        var currentTypeName = !string.IsNullOrEmpty(mapper.inspectorDataType) ? mapper.inspectorDataType : None;
        EditorGUI.BeginChangeCheck();
        currentIndex = EditorGUILayout.Popup(types.FindIndex(type => type == currentTypeName), types.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mapper, "Change DataMapper Data");

            var pMappers = mapper.GetPropertyMappers();
            if (pMappers != null && pMappers.Count > 0)
            {
                Undo.RecordObjects(pMappers.ToArray<Object>(), "Reset PropertyMapper Data");
                Parallel.ForEach(pMappers, (pMapper) => { pMapper.ResetPropertyMapper(); });
            }

            if (currentIndex >= 0)
                mapper.inspectorDataType = types[currentIndex] != None ? types[currentIndex] : null;
        }

        EditorGUILayout.LabelField("Selected Data Class : ",
            !string.IsNullOrEmpty(mapper.inspectorDataType) ? mapper.inspectorDataType : None);
    }
}

#endif