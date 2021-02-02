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
    private static readonly string ProductName = Application.productName;
    private static readonly string Version = Application.version;

    private static readonly string AppFolder = Directory.GetCurrentDirectory();

    private static readonly string APKName = $"{ProductName}_{Version}";
    private static readonly string AndroidFolder = $"{AppFolder}/Builds/Android";
    private static readonly string AndroidDevelopmentFile = $"{AndroidFolder}/Development/{APKName}_DEV.apk";
    private static readonly string AndroidReleaseFile = $"{AndroidFolder}/Release/{APKName}_REL.apk";

    private static readonly string IOSFolder = $"{AppFolder}/Builds/iOS";
    private static readonly string IOSDevelopmentFolder = $"{IOSFolder}/Development/{Version}";
    private static readonly string IOSReleaseFolder = $"{IOSFolder}/Release/{Version}";

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
        if (File.Exists(AndroidDevelopmentFile))
            File.Delete(AndroidDevelopmentFile);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        var success =
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), AndroidDevelopmentFile, BuildTarget.Android,
            BuildOptions.Development);
        BuildReport(report);
    }

    [MenuItem("Build/Release/Android")]
    public static void AndroidRelease()
    {
        if (File.Exists(AndroidReleaseFile))
            File.Delete(AndroidReleaseFile);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        var success =
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        var report =
            BuildPipeline.BuildPlayer(GetScenes(), AndroidReleaseFile, BuildTarget.Android, BuildOptions.None);
        BuildReport(report);
    }

    [MenuItem("Build/Development/iOS")]
    public static void IOSDevelopment()
    {
        if (Directory.Exists(IOSDevelopmentFolder))
            Directory.Delete(IOSDevelopmentFolder);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        var success = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), IOSDevelopmentFolder, BuildTarget.iOS,
            BuildOptions.Development);
        BuildReport(report);
    }

    [MenuItem("Build/Release/iOS")]
    public static void IOSRelease()
    {
        if (Directory.Exists(IOSReleaseFolder))
            Directory.Delete(IOSReleaseFolder);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        var success = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        var report = BuildPipeline.BuildPlayer(GetScenes(), IOSReleaseFolder, BuildTarget.iOS, BuildOptions.None);
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

    private static void Open(string path)
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