// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

/// <summary>
/// Extension methods for .Net Collection objects, e.g. Lists, Dictionaries, Arrays
/// </summary>
public static class CollectionsExtensions
{
    /// <summary>
    /// Creates a read-only wrapper around an existing collection.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection to be wrapped.</param>
    /// <returns>The new, read-only wrapper around <paramref name="elements"/>.</returns>
    public static ReadOnlyCollection<TElement> AsReadOnly<TElement>(this IList<TElement> elements)
    {
        return new ReadOnlyCollection<TElement>(elements);
    }

    /// <summary>
    /// Creates a read-only copy of an existing collection.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection to be copied.</param>
    /// <returns>The new, read-only copy of <paramref name="elements"/>.</returns>
    public static ReadOnlyCollection<TElement> ToReadOnlyCollection<TElement>(this IEnumerable<TElement> elements)
    {
        return elements.ToArray().AsReadOnly();
    }

    /// <summary>
    /// Inserts an item in its sorted position into an already sorted collection. This is useful if you need to consume the
    /// collection in between insertions and need it to stay correctly sorted the whole time. If you just need to insert a
    /// bunch of items and then consume the sorted collection at the end, it's faster to add all the elements and then use
    /// <see cref="List{T}.Sort()"/> at the end.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection of sorted elements to be inserted into.</param>
    /// <param name="toInsert">The element to insert.</param>
    /// <param name="comparer">The comparer to use when sorting or null to use <see cref="Comparer{T}.Default"/>.</param>
    public static int SortedInsert<TElement>(this List<TElement> elements, TElement toInsert,
        IComparer<TElement> comparer = null)
    {
        var effectiveComparer = comparer ?? Comparer<TElement>.Default;

        if (Application.isEditor)
        {
            for (var iElement = 0; iElement < elements.Count - 1; iElement++)
            {
                var element = elements[iElement];
                var nextElement = elements[iElement + 1];

                if (effectiveComparer.Compare(element, nextElement) <= 0) continue;

                Debug.LogWarning("Elements must already be sorted to call this method.");
                break;
            }
        }

        var searchResult = elements.BinarySearch(toInsert, effectiveComparer);

        var insertionIndex = searchResult >= 0
            ? searchResult
            : ~searchResult;

        elements.Insert(insertionIndex, toInsert);

        return insertionIndex;
    }

    /// <summary>
    /// Disposes of all non-null elements in a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection of elements to be disposed.</param>
    public static void DisposeElements<TElement>(this IEnumerable<TElement> elements)
        where TElement : IDisposable
    {
        foreach (var element in elements)
        {
            if (element == null) continue;

            element.Dispose();
        }
    }

    /// <summary>
    /// Disposes of all non-null elements in a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection of elements to be disposed.</param>
    public static void DisposeElements<TElement>(this IList<TElement> elements)
        where TElement : IDisposable
    {
        foreach (var element in elements)
        {
            if (element == null) continue;

            element.Dispose();
        }
    }

    /// <summary>
    /// Exports the values of a uint indexed Dictionary as an Array
    /// </summary>
    /// <typeparam name="T">Type of data stored in the values of the Dictionary</typeparam>
    /// <param name="input">Dictionary to be exported</param>
    /// <returns>array in the type of data stored in the Dictionary</returns>
    public static T[] ExportDictionaryValuesAsArray<T>(this Dictionary<uint, T> input)
    {
        var output = new T[input.Count];
        input.Values.CopyTo(output, 0);
        return output;
    }

    public static void AddDicList<TK, TV>(this Dictionary<TK, List<TV>> dic, TK key, TV value)
    {
        if (!dic.TryGetValue(key, out var r))
        {
            r = new List<TV>();
            dic[key] = r;
        }

        r.Add(value);
    }

    public static void AddDicHashSet<TK, TV>(this Dictionary<TK, HashSet<TV>> dic, TK key, TV value)
    {
        if (!dic.TryGetValue(key, out var r))
        {
            r = new HashSet<TV>();
            dic[key] = r;
        }

        r.Add(value);
    }

    public static void AddDicDic<TK1, TK2, TV>(this Dictionary<TK1, Dictionary<TK2, TV>> dic, TK1 key1, TK2 key2, TV value)
    {
        if (!dic.TryGetValue(key1, out var dic2))
        {
            dic2 = new Dictionary<TK2, TV>();
            dic[key1] = dic2;
        }

        dic2[key2] = value;
    }

    public static void AddDicDicList<TK1, TK2, TV>(this Dictionary<TK1, Dictionary<TK2, List<TV>>> dic, TK1 key1, TK2 key2,
        TV value)
    {
        if (!dic.TryGetValue(key1, out var dic2))
        {
            dic2 = new Dictionary<TK2, List<TV>>();
            dic[key1] = dic2;
        }

        dic2.AddDicList(key2, value);
    }

    public static bool TryGetValueDicDic<TK1, TK2, TV>(this Dictionary<TK1, Dictionary<TK2, TV>> dic, TK1 key1, TK2 key2,
        out TV value)
    {
        if (dic.TryGetValue(key1, out var dic2)) return dic2.TryGetValue(key2, out value);

        value = default;
        return false;
    }

    public static void AddDic3<TK1, TK2, TK3, TV>(this Dictionary<TK1, Dictionary<TK2, Dictionary<TK3, TV>>> dic, TK1 key1,
        TK2 key2, TK3 key3, TV value)
    {
        if (!dic.TryGetValue(key1, out var dic2))
        {
            dic2 = new Dictionary<TK2, Dictionary<TK3, TV>>();
            dic[key1] = dic2;
        }

        dic2.AddDicDic(key2, key3, value);
    }

    public static bool TryGetValueDic3<TK1, TK2, TK3, TV>(this Dictionary<TK1, Dictionary<TK2, Dictionary<TK3, TV>>> dic,
        TK1 key1, TK2 key2, TK3 key3, out TV value)
    {
        if (dic.TryGetValue(key1, out var dic2)) return dic2.TryGetValueDicDic(key2, key3, out value);

        value = default;
        return false;
    }
}