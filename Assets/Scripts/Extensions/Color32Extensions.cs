// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Extension methods for Unity's Color32 struct
/// </summary>
public static class Color32Extensions
{
    public static Color PremultiplyAlpha(Color col)
    {
        col.r *= col.a;
        col.g *= col.a;
        col.b *= col.a;

        return col;
    }

    public static Color32 PremultiplyAlpha(Color32 col)
    {
        Color floatCol = col;
        return PremultiplyAlpha(floatCol);
    }

    /// <summary>
    /// Creates a Color from a hexcode string
    /// </summary>
    public static Color ParseHexCode(string hexString)
    {
        if (hexString.StartsWith("#"))
        {
            hexString = hexString.Substring(1);
        }

        if (hexString.StartsWith("0x"))
        {
            hexString = hexString.Substring(2);
        }

        if (hexString.Length == 6)
        {
            hexString += "FF";
        }

        if (hexString.Length != 8)
        {
            throw new ArgumentException($"{hexString} is not a valid color string.");
        }

        var r = byte.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
        var g = byte.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
        var b = byte.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
        var a = byte.Parse(hexString.Substring(6, 2), NumberStyles.HexNumber);

        const float maxRgbValue = 255;
        var c = new Color(r / maxRgbValue, g / maxRgbValue, b / maxRgbValue, a / maxRgbValue);
        return c;
    }
}