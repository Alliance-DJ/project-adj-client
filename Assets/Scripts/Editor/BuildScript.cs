using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    private static readonly string PRODUCT_NAME = Application.productName;
    private static readonly string VERSION = Application.version;

    public static readonly string APP_FOLDER = Directory.GetCurrentDirectory();

    public static readonly string APK_NAME = $"{PRODUCT_NAME}_{VERSION}";
    public static readonly string ANDROID_FOLDER = $"{APP_FOLDER}/Builds/Android";
    public static readonly string ANDROID_DEVELOPMENT_FILE = $"{ANDROID_FOLDER}/Development/{APK_NAME}_DEV.apk";
    public static readonly string ANDROID_RELEASE_FILE = $"{ANDROID_FOLDER}/Release/{APK_NAME}_REL.apk";

    public static readonly string IOS_FOLDER = $"{APP_FOLDER}/Builds/iOS";
    public static readonly string IOS_DEVELOPMENT_FOLDER = $"{IOS_FOLDER}/Development/{VERSION}";
    public static readonly string IOS_RELEASE_FOLDER = $"{IOS_FOLDER}/Release/{VERSION}";

    private static string[] GetScenes()
    {
        var settingScenes = EditorBuildSettings.scenes;
        var scenes = new List<string>(settingScenes.Length);
        scenes.AddRange(from scene in settingScenes where scene != null && scene.enabled select scene.path);

        return scenes.ToArray();
    }

    [MenuItem("Build/Development/Android")]
    public static void AndroidDevelopment()
    {
        if (File.Exists(ANDROID_DEVELOPMENT_FILE))
            File.Delete(ANDROID_DEVELOPMENT_FILE);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        var success =
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), ANDROID_DEVELOPMENT_FILE, BuildTarget.Android,
            BuildOptions.Development);
        BuildReport(report);
    }

    [MenuItem("Build/Release/Android")]
    public static void AndroidRelease()
    {
        if (File.Exists(ANDROID_RELEASE_FILE))
            File.Delete(ANDROID_RELEASE_FILE);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        var success =
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        var report =
            BuildPipeline.BuildPlayer(GetScenes(), ANDROID_RELEASE_FILE, BuildTarget.Android, BuildOptions.None);
        BuildReport(report);
    }

    [MenuItem("Build/Development/iOS")]
    public static void IOSDevelopment()
    {
        if (Directory.Exists(IOS_DEVELOPMENT_FOLDER))
            Directory.Delete(IOS_DEVELOPMENT_FOLDER);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        var success = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), IOS_DEVELOPMENT_FOLDER, BuildTarget.iOS,
            BuildOptions.Development);
        BuildReport(report);
    }

    [MenuItem("Build/Release/iOS")]
    public static void IOSRelease()
    {
        if (Directory.Exists(IOS_RELEASE_FOLDER))
            Directory.Delete(IOS_RELEASE_FOLDER);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        var success = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), IOS_RELEASE_FOLDER, BuildTarget.iOS, BuildOptions.None);
        BuildReport(report);
    }

    private static void OpenInMac(string path)
    {
        var openInsidesOfFolder = false;

        var macPath = path.Replace("\\", "/");

        if (Directory.Exists(macPath))
            openInsidesOfFolder = true;

        if (!macPath.StartsWith("\""))
            macPath = "\"" + macPath;

        if (!macPath.EndsWith("\""))
            macPath += "\"";

        var arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

        try
        {
            Process.Start("open", arguments);
        }
        catch (System.ComponentModel.Win32Exception e)
        {
            e.HelpLink = "";
        }
    }

    private static void OpenInWin(string path)
    {
        var openInsidesOfFolder = false;

        var winPath = path.Replace("/", "\\");

        if (Directory.Exists(winPath))
            openInsidesOfFolder = true;

        try
        {
            Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
        catch (System.ComponentModel.Win32Exception e)
        {
            e.HelpLink = "";
        }
    }

    public static void Open(string path)
    {
        if (SystemInfo.operatingSystem.IndexOf("Windows", StringComparison.OrdinalIgnoreCase) != -1)
        {
            OpenInWin(path);
        }
        else if (SystemInfo.operatingSystem.IndexOf("Mac OS", StringComparison.OrdinalIgnoreCase) != -1)
        {
            OpenInMac(path);
        }
    }

    private static void BuildReport(BuildReport report)
    {
        var result = report.summary.result;
        switch (result)
        {
            case BuildResult.Failed:
                Debug.LogError("Build Result : Failed");
                break;

            case BuildResult.Succeeded:
                Debug.Log($"Build Result : {result}");

                var path = report.summary.outputPath;
                if (!string.IsNullOrEmpty(path))
                    Open(path);
                break;

            default:
                Debug.Log($"Build Result : {result}");
                break;
        }
    }
}