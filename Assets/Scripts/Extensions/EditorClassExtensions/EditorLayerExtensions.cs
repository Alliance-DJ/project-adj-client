﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if UNITY_EDITOR

using UnityEditor;

public static class EditorLayerExtensions
{
    private static SerializedProperty _tagManagerLayers;

    /// <summary>
    /// The current layers defined in the Tag Manager.
    /// </summary>
    public static SerializedProperty TagManagerLayers
    {
        get
        {
            if (_tagManagerLayers == null)
            {
                InitializeTagManager();
            }

            return _tagManagerLayers;
        }
    }

    private static void InitializeTagManager()
    {
        var tagAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");

        if ((tagAssets == null) || (tagAssets.Length == 0))
        {
            Debug.LogError("Failed to load TagManager!");
            return;
        }

        var tagsManager = new SerializedObject(tagAssets);
        _tagManagerLayers = tagsManager.FindProperty("layers");

        Debug.Assert(_tagManagerLayers != null);
    }

    /// <summary>
    /// Attempts to set the layer in Project Settings Tag Manager.
    /// </summary>
    /// <param name="layerId">The layer Id to attempt to set the layer on.</param>
    /// <param name="layerName">The layer name to attempt to set the layer on.</param>
    /// <returns>
    /// True if the specified layerId was newly configured, false otherwise.
    /// </returns>
    public static bool SetupLayer(int layerId, string layerName)
    {
        var layer = TagManagerLayers.GetArrayElementAtIndex(layerId);

        if (!string.IsNullOrEmpty(layer.stringValue))
        {
            // layer already set.
            return false;
        }

        layer.stringValue = layerName;
        layer.serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
        return true;
    }

    /// <summary>
    /// Attempts to remove the layer from the Project Settings Tag Manager.
    /// </summary>
    public static void RemoveLayer(string layerName)
    {
        for (var i = 0; i < TagManagerLayers.arraySize; i++)
        {
            var layer = TagManagerLayers.GetArrayElementAtIndex(i);

            if (layer.stringValue != layerName) continue;

            layer.stringValue = string.Empty;
            layer.serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            break;
        }
    }
}

#endif