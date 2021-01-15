#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropertyMapper), true)]
public class PropertyMapperEditor : Editor
{
    private const string NONE = "None";

    private string searchKey;
    private string subSearchKey;
    private readonly List<string> names = new List<string>();
    private Dictionary<string, Type> returnTypeDic;

    private void OnEnable()
    {
        PropertyMapper mapper = target as PropertyMapper;

        var type = mapper.GetDataMapperDataType();
        if (type != null)
        {
            var fields = TypeCache.GetFields(type);
            if (fields != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    if (field == null) continue;

                    var name = field.Name;
                    names.Add(name);
                }
            }

            var properties = TypeCache.GetProperties(type);
            if (properties != null)
            {
                returnTypeDic = new Dictionary<string, Type>(properties.Length);
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    if (property == null) continue;

                    var name = property.Name;
                    names.Add(name);
                    returnTypeDic[name] = property.GetGetMethod().ReturnType;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        var mapper = target as PropertyMapper;

        string defaultValue = mapper.defaultValue;
        defaultValue = EditorGUILayout.TextField("Default Value : ", defaultValue);
        mapper.defaultValue = defaultValue;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Format");

        string format = mapper.format;
        format = EditorGUILayout.TextArea(format, GUILayout.MinHeight(80f));
        mapper.format = format;

        EditorGUILayout.Space();

        var tempNames = new List<string>(names);
        tempNames.Sort();

        searchKey = EditorGUILayout.TextField("Search Property: ", searchKey);
        var searching = !string.IsNullOrEmpty(searchKey);
        if (searching)
            tempNames.RemoveAll(name => !name.ToLower().Contains(searchKey.ToLower()));

        tempNames.Insert(0, NONE);

        var currentTypeName = !string.IsNullOrEmpty(mapper.propertyName) ? mapper.propertyName : NONE;
        var index = tempNames.FindIndex(type => type == currentTypeName);
        index = EditorGUILayout.Popup(index, tempNames.ToArray());

        if (!searching || index >= 0)
            mapper.propertyName = tempNames[index] != NONE ? tempNames[index] : null;

        EditorGUILayout.LabelField("Property Name : ", !string.IsNullOrEmpty(mapper.propertyName) ? mapper.propertyName : NONE);

        string propertyName = mapper.propertyName;
        if (!string.IsNullOrEmpty(propertyName) &&
            returnTypeDic != null && returnTypeDic.TryGetValue(propertyName, out var type) &&
            type.BaseType == typeof(BaseData))
        {
            SetSubProperties(mapper, type);
        }
    }

    private void SetSubProperties(PropertyMapper mapper, Type type)
    {
        if (type == null) return;

        EditorGUILayout.Space();

        List<string> tempNames = new List<string>();
        var fields = TypeCache.GetFields(type);
        if (fields != null)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field == null) continue;

                var name = field.Name;
                tempNames.Add(name);
            }
        }

        var properties = TypeCache.GetProperties(type);
        if (properties != null)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property == null) continue;

                var name = property.Name;
                tempNames.Add(name);
            }
        }
        tempNames.Sort();

        subSearchKey = EditorGUILayout.TextField("Search Sub : ", subSearchKey);
        var searching = !string.IsNullOrEmpty(subSearchKey);
        if (searching)
            tempNames.RemoveAll(name => !name.ToLower().Contains(subSearchKey.ToLower()));

        tempNames.Insert(0, NONE);

        var currentTypeName = !string.IsNullOrEmpty(mapper.subPropertyName) ? mapper.subPropertyName : NONE;
        var index = tempNames.FindIndex(type => type == currentTypeName);
        index = EditorGUILayout.Popup(index, tempNames.ToArray());

        if (!searching || index >= 0)
            mapper.subPropertyName = tempNames[index] != NONE ? tempNames[index] : null;

        EditorGUILayout.LabelField("Sub Property Name : ", !string.IsNullOrEmpty(mapper.subPropertyName) ? mapper.subPropertyName : NONE);
    }
}

#endif