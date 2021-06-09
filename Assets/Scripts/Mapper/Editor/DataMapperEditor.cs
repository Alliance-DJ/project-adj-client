using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DataMapper))]
    public class DataMapperEditor : UnityEditor.Editor
    {
        private const string NONE = "None";

        private string searchKey;
        private int currentIndex = -1;

        private DataMapper mapper;

        private List<string> dataTypeNames;

        private void OnEnable()
        {
            mapper = target as DataMapper;
            if (mapper == null) return;

            var dataTypes = TypeCache.GetSubClasses<BaseData>();
            if (dataTypes != null)
            {
                dataTypeNames = dataTypes.Select(type => type.Name).ToList();
                dataTypeNames.Sort();
            }
        }

        public override void OnInspectorGUI()
        {
            if (mapper == null) return;

            var types = new List<string>(dataTypeNames);

            searchKey = EditorGUILayout.TextField("Search Data : ", searchKey);

            EditorGUILayout.Space();

            var searching = !string.IsNullOrEmpty(searchKey);
            if (searching)
                types.RemoveAll(n => !n.ToLower().Contains(searchKey.ToLower()));

            types.Insert(0, NONE);

            var currentTypeName = !string.IsNullOrEmpty(mapper.inspectorDataType) ? mapper.inspectorDataType : NONE;
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
                    mapper.inspectorDataType = types[currentIndex] != NONE ? types[currentIndex] : null;
            }

            EditorGUILayout.LabelField("Selected Data Class : ",
                !string.IsNullOrEmpty(mapper.inspectorDataType) ? mapper.inspectorDataType : NONE);

            EditorGUILayout.Space();

            if (GUILayout.Button("Data Refresh"))
            {
                mapper.DataRefresh();
            }
        }
    }
}