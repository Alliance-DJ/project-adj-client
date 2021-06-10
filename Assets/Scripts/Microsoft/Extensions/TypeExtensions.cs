// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

public static class TypeExtensions
{
#if !NETFX_CORE

    /// <summary>
    /// Returns a list of types for all classes that extend from the current type and are not abstract
    /// </summary>
    /// <param name="rootType">The class type from which to search for inherited classes</param>
    /// <param name="searchAssemblies">List of assemblies to search through for types. If null, default is to grab all assemblies in current app domain</param>
    /// <returns>Null if rootType is not a class, otherwise returns list of types for sub-classes of rootType</returns>
    public static List<Type> GetAllSubClassesOf(this Type rootType, Assembly[] searchAssemblies = null)
    {
        if (!rootType.IsClass) return null;

        searchAssemblies ??= AppDomain.CurrentDomain.GetAssemblies();

        var results = new List<Type>();

        Parallel.ForEach(searchAssemblies, (assembly) =>
        {
            Parallel.ForEach(assembly.GetLoadableTypes(), (type) =>
            {
                if (type != null && type.IsClass && !type.IsAbstract && type.IsSubclassOf(rootType))
                {
                    results.Add(type);
                }
            });
        });

        return results;
    }

    /// <summary>
    /// Extension method to check the entire inheritance hierarchy of a
    /// type to see whether the given base type is inherited.
    /// </summary>
    /// <param name="t">The Type object this method was called on</param>
    /// <param name="baseType">The base type to look for in the 
    /// inheritance hierarchy</param>
    /// <returns>True if baseType is found somewhere in the inheritance 
    /// hierarchy, false if not</returns>
    public static bool InheritsFrom(this Type t, Type baseType)
    {
        Type cur = t.BaseType;

        while (cur != null)
        {
            if (cur.Equals(baseType))
            {
                return true;
            }

            cur = cur.BaseType;
        }

        return false;
    }

#endif
}