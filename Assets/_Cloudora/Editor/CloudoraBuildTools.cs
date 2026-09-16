using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEngine;

namespace Cloudora.Editor
{
    public static class CloudoraBuildTools
    {
        private const string GameplayScene = "Assets/_Cloudora/Scenes/02_Gameplay.unity";

        [MenuItem("Cloudora/Build/Android Development APK")]
        public static void BuildDevelopmentApk() => Build(false);

        [MenuItem("Cloudora/Build/Android Release AAB")]
        public static void BuildReleaseAab() => Build(true);

        private static void Build(bool release)
        {
            ConfigureAndroid(release);
            string extension = release ? ".aab" : ".apk";
            string fallback = Path.Combine("Builds", "Android", release ? "Cloudora-0.1.0.aab" : "Cloudora-dev.apk");
            string output = Environment.GetEnvironmentVariable("CLOUDORA_BUILD_OUTPUT") ?? fallback;
            string directory = Path.GetDirectoryName(output);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { GameplayScene },
                locationPathName = output,
                target = BuildTarget.Android,
                options = release ? BuildOptions.CompressWithLz4HC : BuildOptions.Development
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            Debug.Log($"CLOUDORA_BUILD result={report.summary.result} output={Path.GetFullPath(output)} size={report.summary.totalSize} warnings={report.summary.totalWarnings} errors={report.summary.totalErrors}");
            if (report.summary.result != BuildResult.Succeeded && Application.isBatchMode) EditorApplication.Exit(1);
            if (Application.isBatchMode) EditorApplication.Exit(0);
        }

        private static void ConfigureAndroid(bool appBundle)
        {
            PlayerSettings.companyName = "YigitYvz";
            PlayerSettings.productName = "Cloudora";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.yigityvz.cloudora");
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)26;
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            EditorUserBuildSettings.buildAppBundle = appBundle;
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(GameplayScene, true) };
        }
    }
}
