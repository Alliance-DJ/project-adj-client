using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BuildScript
{
    private static readonly string PRODUCT_NAME = Application.productName;
    private static readonly string VERSION = Application.version;

    public static string APP_FOLDER = Directory.GetCurrentDirectory();

    public static string ANDROID_FOLDER = $"{APP_FOLDER}/Builds/Android";
    public static string ANDROID_DEVELOPMENT_FILE = $"{ANDROID_FOLDER}/Development/{PRODUCT_NAME}_{VERSION}_DEV.apk";
    public static string ANDROID_RELEASE_FILE = $"{ANDROID_FOLDER}/Release/{PRODUCT_NAME}_{VERSION}_REL.apk";

    public static string IOS_FOLDER = $"{APP_FOLDER}/Builds/iOS";
    public static string IOS_DEVELOPMENT_FOLDER = $"{IOS_FOLDER}/Development/{VERSION}";
    public static string IOS_RELEASE_FOLDER = $"{IOS_FOLDER}/Release/{VERSION}";

    private static string[] GetScenes()
    {
        var settingScenes = EditorBuildSettings.scenes;
        List<string> scenes = new List<string>(settingScenes.Length);
        for (int i = 0; i < settingScenes.Length; i++)
        {
            var scene = settingScenes[i];
            if (scene == null || !scene.enabled) continue;

            scenes.Add(scene.path);
        }

        return scenes.ToArray();
    }

    [MenuItem("Build/Development/iOS")]
    public static void IOSDevelopment()
    {
        if (Directory.Exists(IOS_DEVELOPMENT_FOLDER))
            Directory.Delete(IOS_DEVELOPMENT_FOLDER);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        BuildPipeline.BuildPlayer(GetScenes(), IOS_DEVELOPMENT_FOLDER, BuildTarget.iOS, BuildOptions.Development);
    }

    [MenuItem("Build/Release/iOS")]
    public static void IOSRelease()
    {
        if (Directory.Exists(IOS_RELEASE_FOLDER))
            Directory.Delete(IOS_RELEASE_FOLDER);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        if (!success) return;

        BuildPipeline.BuildPlayer(GetScenes(), IOS_RELEASE_FOLDER, BuildTarget.iOS, BuildOptions.None);
    }

    [MenuItem("Build/Development/Android")]
    public static void AndroidDevelopment()
    {
        if (File.Exists(ANDROID_DEVELOPMENT_FILE))
            File.Delete(ANDROID_DEVELOPMENT_FILE);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        BuildPipeline.BuildPlayer(GetScenes(), ANDROID_DEVELOPMENT_FILE, BuildTarget.Android, BuildOptions.Development);
    }

    [MenuItem("Build/Release/Android")]
    public static void AndroidRelease()
    {
        if (File.Exists(ANDROID_RELEASE_FILE))
            File.Delete(ANDROID_RELEASE_FILE);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        if (!success) return;

        BuildPipeline.BuildPlayer(GetScenes(), ANDROID_RELEASE_FILE, BuildTarget.Android, BuildOptions.None);
    }
}