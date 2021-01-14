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
    /// <see cref="System.Collections.Generic.List{T}.Sort"/> at the end.
    /// </summary>
    /// <typeparam name="TElement">The type of element in the collection.</typeparam>
    /// <param name="elements">The collection of sorted elements to be inserted into.</param>
    /// <param name="toInsert">The element to insert.</param>
    /// <param name="comparer">The comparer to use when sorting or null to use <see cref="System.Collections.Generic.Comparer{T}.Default"/>.</param>
    public static int SortedInsert<TElement>(this List<TElement> elements, TElement toInsert, IComparer<TElement> comparer = null)
    {
        var effectiveComparer = comparer ?? Comparer<TElement>.Default;

        if (Application.isEditor)
        {
            for (int iElement = 0; iElement < elements.Count - 1; iElement++)
            {
                var element = elements[iElement];
                var nextElement = elements[iElement + 1];

                if (effectiveComparer.Compare(element, nextElement) > 0)
                {
                    Debug.LogWarning("Elements must already be sorted to call this method.");
                    break;
                }
            }
        }

        int searchResult = elements.BinarySearch(toInsert, effectiveComparer);

        int insertionIndex = searchResult >= 0
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
            if (element != null)
            {
                element.Dispose();
            }
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
        for (int iElement = 0; iElement < elements.Count; iElement++)
        {
            var element = elements[iElement];

            if (element != null)
            {
                element.Dispose();
            }
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
        T[] output = new T[input.Count];
        input.Values.CopyTo(output, 0);
        return output;
    }

    public static void AddDicList<K, V>(this Dictionary<K, List<V>> dic, K key, V value)
    {
        if (!dic.TryGetValue(key, out List<V> r))
        {
            r = new List<V>();
            dic[key] = r;
        }
        r.Add(value);
    }

    public static void AddDicHashSet<K, V>(this Dictionary<K, HashSet<V>> dic, K key, V value)
    {
        if (!dic.TryGetValue(key, out HashSet<V> r))
        {
            r = new HashSet<V>();
            dic[key] = r;
        }
        r.Add(value);
    }

    public static void AddDicDic<K1, K2, V>(this Dictionary<K1, Dictionary<K2, V>> dic, K1 key1, K2 key2, V value)
    {
        if (!dic.TryGetValue(key1, out Dictionary<K2, V> dic2))
        {
            dic2 = new Dictionary<K2, V>();
            dic[key1] = dic2;
        }
        dic2[key2] = value;
    }

    public static void AddDicDicList<K1, K2, V>(this Dictionary<K1, Dictionary<K2, List<V>>> dic, K1 key1, K2 key2, V value)
    {
        if (!dic.TryGetValue(key1, out Dictionary<K2, List<V>> dic2))
        {
            dic2 = new Dictionary<K2, List<V>>();
            dic[key1] = dic2;
        }
        dic2.AddDicList(key2, value);
    }

    public static bool TryGetValueDicDic<K1, K2, V>(this Dictionary<K1, Dictionary<K2, V>> dic, K1 key1, K2 key2, out V value)
    {
        if (!dic.TryGetValue(key1, out Dictionary<K2, V> dic2))
        {
            value = default;
            return false;
        }
        return dic2.TryGetValue(key2, out value);
    }

    public static void AddDic3<K1, K2, K3, V>(this Dictionary<K1, Dictionary<K2, Dictionary<K3, V>>> dic, K1 key1, K2 key2, K3 key3, V value)
    {
        if (!dic.TryGetValue(key1, out Dictionary<K2, Dictionary<K3, V>> dic2))
        {
            dic2 = new Dictionary<K2, Dictionary<K3, V>>();
            dic[key1] = dic2;
        }
        dic2.AddDicDic(key2, key3, value);
    }

    public static bool TryGetValueDic3<K1, K2, K3, V>(this Dictionary<K1, Dictionary<K2, Dictionary<K3, V>>> dic, K1 key1, K2 key2, K3 key3, out V value)
    {
        if (!dic.TryGetValue(key1, out Dictionary<K2, Dictionary<K3, V>> dic2))
        {
            value = default;
            return false;
        }

        return dic2.TryGetValueDicDic(key2, key3, out value);
    }
}
