using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Cloudora.Editor
{
    public static class CloudoraIosBuildTools
    {
        public const string GameplayScene = "Assets/_Cloudora/Scenes/02_Gameplay.unity";
        [MenuItem("Cloudora/Build/iOS Development Xcode Project")]
        public static void BuildDevelopmentXcodeProject()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS))
                throw new BuildFailedException("Install iOS Build Support for Unity 6000.5.9f1 in Unity Hub.");
            Configure();
            string output = Path.GetFullPath("Builds/iOS/Cloudora");
            // Never append to an owner-signed Xcode project or overwrite its local signing.
            if (Directory.Exists(output) && Directory.GetFileSystemEntries(output).Length > 0)
                throw new BuildFailedException("Move the existing Builds/iOS/Cloudora output aside before regenerating.");
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { GameplayScene }, locationPathName = output,
                target = BuildTarget.iOS, options = BuildOptions.None,
                extraScriptingDefines = new[] { "CLOUDORA_DEVICE_QA" }
            });
            Debug.Log($"CLOUDORA_IOS_BUILD result={report.summary.result} output={output} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings}");
            if (report.summary.result != BuildResult.Succeeded)
                throw new BuildFailedException("iOS Xcode export failed; inspect the Unity build log.");
        }

        [MenuItem("Cloudora/Build/Configure iOS Development")]
        public static void Configure()
        {
            // Product identity/version and all gameplay assets remain as imported from Android.
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, "com.yigityvz.cloudora");
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetArchitecture(NamedBuildTarget.iOS, 1); // ARM64
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.iOS, ManagedStrippingLevel.Low);
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new[] { GraphicsDeviceType.Metal });
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;
            PlayerSettings.statusBarHidden = true;
            PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneOnly;
            PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            // Preserve source deployment baseline; store/device policy remains owner-reviewed.
            PlayerSettings.iOS.targetOSVersionString = "15.0";
            PlayerSettings.iOS.buildNumber = ReadBuildNumber(Environment.GetEnvironmentVariable("CLOUDORA_IOS_BUILD_NUMBER"));
            PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Suspend;
            PlayerSettings.iOS.appleDeveloperTeamID = string.Empty;
            PlayerSettings.iOS.iOSManualProvisioningProfileID = string.Empty;
            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_Cloudora/Art/cloudora-icon-512.png");
            if (icon == null) throw new BuildFailedException("Cloudora icon is missing.");
            int[] sizes = PlayerSettings.GetIconSizes(NamedBuildTarget.iOS, IconKind.Application);
            var icons = new Texture2D[sizes.Length];
            for (int i = 0; i < icons.Length; i++) icons[i] = icon;
            PlayerSettings.SetIcons(NamedBuildTarget.iOS, icons, IconKind.Application);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(GameplayScene, true) };
            AssetDatabase.SaveAssets();
        }

        public static string ReadBuildNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "1";
            if (!int.TryParse(value, out int number) || number <= 0)
                throw new BuildFailedException("CLOUDORA_IOS_BUILD_NUMBER must be a positive integer.");
            return number.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
