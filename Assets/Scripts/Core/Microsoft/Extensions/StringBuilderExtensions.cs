// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Text;

/// <summary>
/// <see cref="StringBuilder"/> Extensions.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Append new line for current Environment to this StringBuilder buffer
    /// </summary>
    public static StringBuilder AppendNewLine(this StringBuilder sb)
    {
        sb.Append(Environment.NewLine);
        return sb;
    }
}