using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor.Android;
using UnityEngine;
using Cloudora.Services;

namespace Cloudora.Editor
{
    public static class CloudoraBuildTools
    {
        private const string GameplayScene = "Assets/_Cloudora/Scenes/02_Gameplay.unity";

        private sealed class SigningState
        {
            public bool useCustomKeystore;
            public string keystoreName;
            public string keystorePass;
            public string keyaliasName;
            public string keyaliasPass;
        }

        [MenuItem("Cloudora/Build/Android Development APK")]
        public static void BuildDevelopmentApk() => Build(false, false);

        [MenuItem("Cloudora/Build/Android Release AAB")]
        public static void BuildReleaseAab() => Build(true, false);

        [MenuItem("Cloudora/Build/Android Play AAB")]
        public static void BuildPlayAab() => Build(true, true);

        private static void Build(bool release, bool play)
        {
            SigningState signingState = CaptureSigningState();
            BuildReport report = null;
            string output = null;
            try
            {
                ConfigureAndroid(release, play);
                string fallback = Path.Combine("Builds", "Android", play ? "Cloudora-0.1.0-play.aab" : release ? "Cloudora-0.1.0.aab" : "Cloudora-dev.apk");
                output = Environment.GetEnvironmentVariable("CLOUDORA_BUILD_OUTPUT") ?? fallback;
                string directory = Path.GetDirectoryName(output);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                var options = new BuildPlayerOptions
                {
                    scenes = new[] { GameplayScene },
                    locationPathName = output,
                    target = BuildTarget.Android,
                    options = release ? BuildOptions.CompressWithLz4HC : BuildOptions.Development
                };
                report = BuildPipeline.BuildPlayer(options);
            }
            finally
            {
                RestoreSigningState(signingState);
            }

            Debug.Log($"CLOUDORA_BUILD result={report.summary.result} output={Path.GetFullPath(output)} size={report.summary.totalSize} warnings={report.summary.totalWarnings} errors={report.summary.totalErrors}");
            if (report.summary.result != BuildResult.Succeeded && Application.isBatchMode) EditorApplication.Exit(1);
            if (Application.isBatchMode) EditorApplication.Exit(0);
        }

        private static void ConfigureAndroid(bool appBundle, bool play)
        {
            PlayerSettings.companyName = "YigitYvz";
            PlayerSettings.productName = "Cloudora";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.yigityvz.cloudora");
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.Android.bundleVersionCode = ReadVersionCode();
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)26;
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;
            PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.forceInternetPermission = false;
            ConfigureSigning(play);
            ConfigureIcons();
            EditorUserBuildSettings.buildAppBundle = appBundle;
            if (appBundle) EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(GameplayScene, true) };
        }

        private static int ReadVersionCode()
        {
            string value = Environment.GetEnvironmentVariable("CLOUDORA_VERSION_CODE");
            if (string.IsNullOrWhiteSpace(value)) return 1;
            if (!int.TryParse(value, out int versionCode) || versionCode <= 0)
                throw new BuildFailedException("CLOUDORA_VERSION_CODE must be a positive integer.");
            return versionCode;
        }

        private static void ConfigureSigning(bool play)
        {
            if (!play)
            {
                PlayerSettings.Android.useCustomKeystore = false;
                return;
            }

            ReleaseConfiguration.Reload();
            if (!ReleaseConfiguration.HasPrivacyPolicy || !ReleaseConfiguration.HasSupportEmail)
                throw new BuildFailedException("Play build requires an HTTPS privacyPolicyUrl and a supportEmail in Assets/Resources/CloudoraReleaseConfig.json.");

            string keystorePath = Environment.GetEnvironmentVariable("CLOUDORA_KEYSTORE_PATH");
            string keystorePass = Environment.GetEnvironmentVariable("CLOUDORA_KEYSTORE_PASS");
            string alias = Environment.GetEnvironmentVariable("CLOUDORA_KEY_ALIAS");
            string aliasPass = Environment.GetEnvironmentVariable("CLOUDORA_KEY_ALIAS_PASS");
            if (string.IsNullOrWhiteSpace(keystorePath) || !File.Exists(keystorePath) ||
                string.IsNullOrWhiteSpace(keystorePass) || string.IsNullOrWhiteSpace(alias) || string.IsNullOrWhiteSpace(aliasPass))
                throw new BuildFailedException("Play build requires CLOUDORA_KEYSTORE_PATH, CLOUDORA_KEYSTORE_PASS, CLOUDORA_KEY_ALIAS, and CLOUDORA_KEY_ALIAS_PASS. The keystore path must exist.");

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = Path.GetFullPath(keystorePath);
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = alias;
            PlayerSettings.Android.keyaliasPass = aliasPass;
        }

        private static SigningState CaptureSigningState() => new()
        {
            useCustomKeystore = PlayerSettings.Android.useCustomKeystore,
            keystoreName = PlayerSettings.Android.keystoreName,
            keystorePass = PlayerSettings.Android.keystorePass,
            keyaliasName = PlayerSettings.Android.keyaliasName,
            keyaliasPass = PlayerSettings.Android.keyaliasPass
        };

        private static void RestoreSigningState(SigningState state)
        {
            PlayerSettings.Android.useCustomKeystore = state.useCustomKeystore;
            PlayerSettings.Android.keystoreName = state.keystoreName;
            PlayerSettings.Android.keystorePass = state.keystorePass;
            PlayerSettings.Android.keyaliasName = state.keyaliasName;
            PlayerSettings.Android.keyaliasPass = state.keyaliasPass;
        }

        private static void ConfigureIcons()
        {
            Texture2D foreground = LoadIcon("cloudora-adaptive-foreground-512.png");
            Texture2D background = LoadIcon("cloudora-adaptive-background-512.png");
            SetIcons(AndroidPlatformIconKind.Adaptive, background, foreground);
        }

        private static Texture2D LoadIcon(string fileName)
        {
            string path = "Assets/_Cloudora/Art/" + fileName;
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null) throw new FileNotFoundException($"Android icon asset is missing: {path}");
            return texture;
        }

        private static void SetIcons(PlatformIconKind kind, params Texture2D[] textures)
        {
            PlatformIcon[] slots = PlayerSettings.GetPlatformIcons(NamedBuildTarget.Android, kind);
            foreach (PlatformIcon slot in slots) slot.SetTextures(textures);
            PlayerSettings.SetPlatformIcons(NamedBuildTarget.Android, kind, slots);
        }
    }
}
