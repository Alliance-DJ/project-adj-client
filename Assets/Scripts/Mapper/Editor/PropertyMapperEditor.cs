#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropertyMapper), true)]
public class PropertyMapperEditor : Editor
{
    private const string NONE = "None";

    private string defaultValue;
    private string format;
    private string searchKey;
    private string subSearchKey;
    private int currentIndex = -1;
    private int currentSubIndex = -1;

    private PropertyMapper mapper;

    private List<string> names;
    private Dictionary<string, Type> returnTypeDic;

    private void OnEnable()
    {
        mapper = target as PropertyMapper;
        if (mapper == null) return;

        names = new List<string>();
        returnTypeDic = new Dictionary<string, Type>();

        var type = mapper.GetDataMapperDataType();
        if (type != null)
        {
            var fields = TypeCache.GetFields(type);
            if (fields != null)
            {
                Parallel.ForEach(fields, (field) =>
                {
                    var name = field.Name;
                    names.Add(name);
                });
            }

            var properties = TypeCache.GetProperties(type);
            if (properties != null)
            {
                Parallel.ForEach(properties, (property) =>
                {
                    var name = property.Name;
                    names.Add(name);
                    returnTypeDic[name] = property.GetGetMethod().ReturnType;
                });
            }
        }
    }

    public override void OnInspectorGUI()
    {
        if (mapper == null) return;

        EditorGUI.BeginChangeCheck();
        defaultValue = EditorGUILayout.TextField("Default Value : ", mapper.defaultValue);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mapper, "Change PropertyMapper Default Value");
            mapper.defaultValue = defaultValue;
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Format");

        EditorGUI.BeginChangeCheck();
        format = EditorGUILayout.TextArea(mapper.format, GUILayout.MinHeight(80f));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mapper, "Change PropertyMapper Format");
            mapper.format = format;
        }

        EditorGUILayout.Space();

        var tempNames = new List<string>(names);
        tempNames.Sort();

        searchKey = EditorGUILayout.TextField("Search Property: ", searchKey);
        var searching = !string.IsNullOrEmpty(searchKey);
        if (searching)
            tempNames.RemoveAll(name => !name.ToLower().Contains(searchKey.ToLower()));

        tempNames.Insert(0, NONE);

        var currentTypeName = !string.IsNullOrEmpty(mapper.propertyName) ? mapper.propertyName : NONE;
        EditorGUI.BeginChangeCheck();
        currentIndex = EditorGUILayout.Popup(tempNames.FindIndex(type => type == currentTypeName), tempNames.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mapper, "Change PropertyMapper PropertyData");
            if (currentIndex >= 0)
                mapper.propertyName = tempNames[currentIndex] != NONE ? tempNames[currentIndex] : null;
        }

        EditorGUILayout.LabelField("Property Name : ", !string.IsNullOrEmpty(mapper.propertyName) ? mapper.propertyName : NONE);

        string propertyName = mapper.propertyName;
        if (!string.IsNullOrEmpty(propertyName) &&
            returnTypeDic != null && returnTypeDic.TryGetValue(propertyName, out var type) &&
            type.BaseType == typeof(BaseData))
        {
            SetSubProperties(type);
        }
    }

    private void SetSubProperties(Type type)
    {
        if (mapper == null || type == null) return;

        EditorGUILayout.Space();

        List<string> tempNames = new List<string>();
        var fields = TypeCache.GetFields(type);
        if (fields != null)
        {
            Parallel.ForEach(fields, (field) =>
            {
                var name = field.Name;
                tempNames.Add(name);
            });
        }

        var properties = TypeCache.GetProperties(type);
        if (properties != null)
        {
            Parallel.ForEach(properties, (property) =>
            {
                var name = property.Name;
                tempNames.Add(name);
            });
        }
        tempNames.Sort();

        subSearchKey = EditorGUILayout.TextField("Search Sub : ", subSearchKey);
        var searching = !string.IsNullOrEmpty(subSearchKey);
        if (searching)
            tempNames.RemoveAll(name => !name.ToLower().Contains(subSearchKey.ToLower()));

        tempNames.Insert(0, NONE);

        var currentTypeName = !string.IsNullOrEmpty(mapper.subPropertyName) ? mapper.subPropertyName : NONE;
        EditorGUI.BeginChangeCheck();
        currentSubIndex = EditorGUILayout.Popup(tempNames.FindIndex(type => type == currentTypeName), tempNames.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mapper, "Change PropertyMapper SubPropertyData");
            if (currentSubIndex >= 0)
                mapper.subPropertyName = tempNames[currentSubIndex] != NONE ? tempNames[currentSubIndex] : null;
        }

        EditorGUILayout.LabelField("Sub Property Name : ", !string.IsNullOrEmpty(mapper.subPropertyName) ? mapper.subPropertyName : NONE);
    }
}

#endif