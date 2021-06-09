using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(DataMapper))]
    public class DataMapperEditor : UnityEditor.Editor
    {
        private const string NONE = "None";

        private static readonly Dictionary<string, Type> cacheTypes = new Dictionary<string, Type>() { { NONE, null } };

        private string searchKey;
        private int currentIndex = -1;

        private DataMapper mapper;

        private void OnEnable()
        {
            mapper = target as DataMapper;
            if (mapper == null) return;

            var types = TypeCache.GetSubClasses<BaseData>();
            if (types != null)
            {
                for (int i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    if (type == null || cacheTypes.ContainsValue(type)) continue;

                    cacheTypes.Add(type.Name, type);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (mapper == null) return;

            var types = new List<string>(cacheTypes.Values.Select(t => t?.Name).Where(t => t != null));
            types.Sort();

            searchKey = EditorGUILayout.TextField("Search Data : ", searchKey);

            EditorGUILayout.Space();

            var searching = !string.IsNullOrEmpty(searchKey);
            if (searching)
                types.RemoveAll(n => n == NONE || !n.ToLower().Contains(searchKey.ToLower()));

            types.Insert(0, NONE);

            var currentType = mapper.DataType;
            var currentTypeName = currentType != null ? currentType.Name : NONE;

            EditorGUI.BeginChangeCheck();
            var selectedIndex = types.FindIndex(type => type == currentTypeName);
            currentIndex = EditorGUILayout.Popup(selectedIndex == -1 ? 0 : selectedIndex, types.ToArray());
            Debug.Log(currentIndex);

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("INSIDE");
                Undo.RecordObject(mapper, $"Change DataMapper Data {mapper.gameObject.GetInstanceID()}");

                //var pMappers = mapper.GetPropertyMappers();
                //if (pMappers != null && pMappers.Count > 0)
                //{
                //    Undo.RecordObjects(pMappers.ToArray<Object>(), "Reset PropertyMapper Data");
                //    Parallel.ForEach(pMappers, (pMapper) => { pMapper.ResetPropertyMapper(); });
                //}

                if (currentIndex >= 0)
                {
                    string select = types[currentIndex];
                    if (cacheTypes.ContainsKey(select))
                        mapper.DataType = cacheTypes[select];
                }
            }

            EditorGUILayout.LabelField("Selected Data Class : ",
                mapper.DataType != null ? mapper.DataType.Name : NONE);

            EditorGUILayout.Space();

            if (GUILayout.Button("Data Refresh"))
            {
                mapper.DataRefresh();
            }
        }
    }
}