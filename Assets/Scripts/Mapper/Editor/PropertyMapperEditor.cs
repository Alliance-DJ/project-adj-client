using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PropertyMapper), true)]
    public class PropertyMapperEditor : UnityEditor.Editor
    {
        private const string NONE = "(None)";

        private string defaultValue;
        private string format;

        private string searchKey;
        private int currentIndex = -1;

        private string subSearchKey;
        private int currentSubIndex = -1;

        private PropertyMapper propertyMapper;

        private List<string> names;
        private Dictionary<string, Type> returnTypeDic;

        private void OnEnable()
        {
            propertyMapper = target as PropertyMapper;
            if (propertyMapper == null) return;

            names = new List<string>();
            returnTypeDic = new Dictionary<string, Type>();

            var type = GetDataMapperDataType();
            if (type == null)
            {
                propertyMapper.propertyName = string.Empty;
                propertyMapper.subPropertyName = string.Empty;
                Debug.LogError($"No DataMapper (this): {propertyMapper.gameObject.name}", this);
                return;
            }

            var fields = TypeCache.GetFields(type);
            if (fields != null)
            {
                Parallel.ForEach(fields, (field) =>
                {
                    var fName = field.Name;
                    names.Add(fName);
                });
            }

            var properties = TypeCache.GetProperties(type);
            if (properties != null)
            {
                Parallel.ForEach(properties, (property) =>
                {
                    var pName = property.Name;
                    names.Add(pName);
                    returnTypeDic[pName] = property.GetGetMethod().ReturnType;
                });
            }
        }

        public override void OnInspectorGUI()
        {
            if (propertyMapper == null) return;

            EditorGUI.BeginChangeCheck();
            defaultValue = EditorGUILayout.TextField("Default Value : ", propertyMapper.defaultValue);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(propertyMapper, "Change PropertyMapper Default Value");
                propertyMapper.defaultValue = defaultValue;
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Format");

            EditorGUI.BeginChangeCheck();
            format = EditorGUILayout.TextArea(propertyMapper.format, GUILayout.MinHeight(80f));

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(propertyMapper, "Change PropertyMapper Format");
                propertyMapper.format = format;
            }

            EditorGUILayout.Space();

            var tempNames = new List<string>(names);
            tempNames.Sort();

            searchKey = EditorGUILayout.TextField("Search Property: ", searchKey);
            var searching = !string.IsNullOrEmpty(searchKey);
            if (searching)
                tempNames.RemoveAll(n => !n.ToLower().Contains(searchKey.ToLower()));

            tempNames.Insert(0, NONE);

            var currentType = propertyMapper.propertyName;
            var currentTypeName = !string.IsNullOrEmpty(currentType) ? currentType : NONE;
            EditorGUI.BeginChangeCheck();
            currentIndex = EditorGUILayout.Popup(tempNames.FindIndex(t => t == currentTypeName), tempNames.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(propertyMapper, "Change PropertyMapper PropertyData");
                propertyMapper.subPropertyName = string.Empty;
                if (currentIndex >= 0)
                {
                    currentType = tempNames[currentIndex] != NONE ? tempNames[currentIndex] : string.Empty;
                    propertyMapper.propertyName = currentType;
                }
            }

            EditorGUILayout.LabelField("Property Name : ",
                !string.IsNullOrEmpty(currentType) ? currentType : NONE);

            if (!string.IsNullOrEmpty(currentType) &&
                returnTypeDic != null && returnTypeDic.TryGetValue(currentType, out var type) &&
                type.InheritsFrom(typeof(BaseData)))
            {
                SetSubProperties(type);
            }
        }

        private void SetSubProperties(Type type)
        {
            if (propertyMapper == null || type == null) return;

            EditorGUILayout.Space();

            var tempNames = new List<string>();
            var fields = TypeCache.GetFields(type);
            if (fields != null)
            {
                Parallel.ForEach(fields, (field) =>
                {
                    var fName = field.Name;
                    tempNames.Add(fName);
                });
            }

            var properties = TypeCache.GetProperties(type);
            if (properties != null)
            {
                Parallel.ForEach(properties, (property) =>
                {
                    var pName = property.Name;
                    tempNames.Add(pName);
                });
            }

            tempNames.Sort();

            subSearchKey = EditorGUILayout.TextField("Search Sub : ", subSearchKey);
            var searching = !string.IsNullOrEmpty(subSearchKey);
            if (searching)
                tempNames.RemoveAll(n => !n.ToLower().Contains(subSearchKey.ToLower()));

            tempNames.Insert(0, NONE);

            var currentType = propertyMapper.subPropertyName;
            var currentTypeName = !string.IsNullOrEmpty(currentType) ? currentType : NONE;
            var selectIndex = tempNames.FindIndex(t => t == currentTypeName);
            EditorGUI.BeginChangeCheck();
            currentSubIndex =
                EditorGUILayout.Popup(selectIndex == -1 ? 0 : selectIndex, tempNames.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(propertyMapper, "Change PropertyMapper SubPropertyData");
                if (currentSubIndex >= 0)
                {
                    currentType = tempNames[currentSubIndex] != NONE ? tempNames[currentSubIndex] : string.Empty;
                    propertyMapper.subPropertyName = currentType;
                }
            }

            EditorGUILayout.LabelField("Sub Property Name : ",
                !string.IsNullOrEmpty(currentType) ? propertyMapper.subPropertyName : NONE);
        }

        public Type GetDataMapperDataType()
        {
            if (propertyMapper == null) return null;

            DataMapper dm = null;
            var t = propertyMapper.transform;
            while (t.IsValid() && !dm.IsValid())
            {
                t = t.parent;
                if (t.IsValid())
                    dm = t.GetComponent<DataMapper>();
            }

            if (!dm.IsValid()) return null;

            return dm.DataType;
        }
    }
}