using UnityEngine;

public static class NullExtensions
{
    public static bool IsValid<T>(this T @base) where T : class
    {
        if (@base == null) return false;

        if (@base is Object) return @base as Object;

        return true;
    }
}