// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Utility class to store subclasses of particular base class keys
/// Reloads between play mode/edit mode and after re-compile of scripts
/// </summary>
public static class TypeCache
{
    private static Dictionary<Type, List<Type>> _cache;
    private static Dictionary<Type, PropertyInfo[]> _propertiesCache;
    private static Dictionary<Type, FieldInfo[]> _fieldsCache;

    /// <summary>
    /// Get all subclass types of base class type T
    /// Does not work with .NET scripting backend
    /// </summary>
    /// <typeparam name="T">base class of type T</typeparam>
    /// <returns>list of subclass types for base class T</returns>
    public static List<Type> GetSubClasses<T>()
    {
        return GetSubClasses(typeof(T));
    }

    public static PropertyInfo[] GetProperties<T>()
    {
        return GetProperties(typeof(T));
    }

    public static FieldInfo[] GetFields<T>()
    {
        return GetFields(typeof(T));
    }

    /// <summary>
    /// Get all subclass types of base class type parameter
    /// Does not work with .NET scripting backend
    /// </summary>
    /// <param name="baseClassType">base class type</param>
    /// <returns>list of subclass types for base class type parameter</returns>
    public static List<Type> GetSubClasses(Type baseClassType)
    {
#if !NETFX_CORE
        if (baseClassType == null)
        {
            return null;
        }

        _cache ??= new Dictionary<Type, List<Type>>();
        if (!_cache.ContainsKey(baseClassType))
        {
            _cache[baseClassType] = baseClassType.GetAllSubClassesOf();
        }

        return _cache[baseClassType];
#else
        return null;
#endif
    }

    public static PropertyInfo[] GetProperties(Type baseClassType)
    {
#if !NETFX_CORE
        if (baseClassType == null)
        {
            return null;
        }

        _propertiesCache ??= new Dictionary<Type, PropertyInfo[]>();
        if (!_propertiesCache.ContainsKey(baseClassType))
        {
            _propertiesCache[baseClassType] = baseClassType.GetProperties();
        }

        return _propertiesCache[baseClassType];
#else
        return null;
#endif
    }

    public static FieldInfo[] GetFields(Type baseClassType)
    {
#if !NETFX_CORE
        if (baseClassType == null)
        {
            return null;
        }

        _fieldsCache ??= new Dictionary<Type, FieldInfo[]>();
        if (!_fieldsCache.ContainsKey(baseClassType))
        {
            _fieldsCache[baseClassType] = baseClassType.GetFields();
        }

        return _fieldsCache[baseClassType];
#else
        return null;
#endif
    }
}