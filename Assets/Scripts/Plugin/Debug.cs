using System;
using UnityEngine;

using Object = UnityEngine.Object;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

/// 
/// It overrides UnityEngine.Debug to mute debug messages completely on a platform-specific basis.
/// 
/// Putting this inside of 'Plugins' foloder is ok.
/// 
/// Important:
///     Other preprocessor directives than 'UNITY_EDITOR' does not correctly work.
/// 
/// Note:
///     [Conditional] attribute indicates to compilers that a method call or attribute should be 
///     ignored unless a specified conditional compilation symbol is defined.
/// 
/// See Also: 
///     http://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute.aspx
/// 
/// 2012.11. @kimsama
/// 
public static class Debug
{
    public static bool developerConsoleVisible
    {
        get { return UnityEngine.Debug.developerConsoleVisible; }
        set { UnityEngine.Debug.developerConsoleVisible = value; }
    }

    public static ILogger unityLogger
    { get { return UnityEngine.Debug.unityLogger; } }

    public static bool isDebugBuild
    { get { return UnityEngine.Debug.isDebugBuild; } }

    [Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
    public static ILogger logger
    { get { return UnityEngine.Debug.logger; } }

    #region Assert
    
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message, Object context)
        => UnityEngine.Debug.Assert(condition, message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, object message, Object context)
        => UnityEngine.Debug.Assert(condition, message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message)
        => UnityEngine.Debug.Assert(condition, message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, object message)
        => UnityEngine.Debug.Assert(condition, message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, Object context)
        => UnityEngine.Debug.Assert(condition, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
        => UnityEngine.Debug.Assert(condition);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
    public static void Assert(bool condition, string format, params object[] args)
        => UnityEngine.Debug.AssertFormat(condition, format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void AssertFormat(bool condition, string format, params object[] args)
        => UnityEngine.Debug.AssertFormat(condition, format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        => UnityEngine.Debug.AssertFormat(condition, context, format, args);

    #endregion

    public static void Break()
        => UnityEngine.Debug.Break();

    public static void ClearDeveloperConsole()
        => UnityEngine.Debug.ClearDeveloperConsole();

    public static void DebugBreak()
        => UnityEngine.Debug.DebugBreak();

    #region DrawLine

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        => UnityEngine.Debug.DrawLine(start, end, color, duration);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
        => UnityEngine.Debug.DrawLine(start, end, color);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 0.0f, bool depthTest = true)
        => UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end)
        => UnityEngine.Debug.DrawLine(start, end);

    #endregion

    #region DrawRay

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        => UnityEngine.Debug.DrawRay(start, dir, color, duration);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default, float duration = 0.0f, bool depthTest = true)
        => UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir)
        => UnityEngine.Debug.DrawRay(start, dir);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
        => UnityEngine.Debug.DrawRay(start, dir, color);

    #endregion

    #region Log

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, UnityEngine.Object context)
        => UnityEngine.Debug.Log(message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Log(object message)
        => UnityEngine.Debug.Log(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogFormat(Object context, string format, params object[] args)
        => UnityEngine.Debug.LogFormat(context, format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
        => UnityEngine.Debug.LogFormat(format, args);

    #endregion

    #region LogAssertion

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogAssertion(object message, Object context)
        => UnityEngine.Debug.LogAssertion(message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogAssertion(object message)
        => UnityEngine.Debug.LogAssertion(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogAssertionFormat(Object context, string format, params object[] args)
        => UnityEngine.Debug.LogAssertionFormat(context, format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogAssertionFormat(string format, params object[] args)
        => UnityEngine.Debug.LogAssertionFormat(format, args);

    #endregion

    #region LogError

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogError(object message, Object context)
        => UnityEngine.Debug.LogError(message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogError(object message)
        => UnityEngine.Debug.LogError(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string format, params object[] args)
        => UnityEngine.Debug.LogErrorFormat(format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(Object context, string format, params object[] args)
        => UnityEngine.Debug.LogErrorFormat(context, format, args);

    #endregion

    #region LogException

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogException(Exception exception, Object context)
        => UnityEngine.Debug.LogException(exception, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogException(Exception exception)
        => UnityEngine.Debug.LogException(exception);

    #endregion

    #region LogWarning

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
        => UnityEngine.Debug.LogWarning(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context)
        => UnityEngine.Debug.LogWarning(message, context);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args)
        => UnityEngine.Debug.LogWarningFormat(format, args);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(Object context, string format, params object[] args)
        => UnityEngine.Debug.LogWarningFormat(context, format, args);

    #endregion
}