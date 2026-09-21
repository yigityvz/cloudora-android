using System;
using UnityEngine;

namespace Cloudora.Services
{
    [Serializable]
    public sealed class ReleaseConfigurationData
    {
        public string privacyPolicyUrl;
        public string supportEmail;
    }

    public static class ReleaseConfiguration
    {
        private const string ResourceName = "CloudoraReleaseConfig";
        private static ReleaseConfigurationData _current;

        public static ReleaseConfigurationData Current => _current ??= Load();
        public static bool HasPrivacyPolicy => Uri.TryCreate(Current.privacyPolicyUrl, UriKind.Absolute, out Uri uri) && uri.Scheme == Uri.UriSchemeHttps;
        public static bool HasSupportEmail => !string.IsNullOrWhiteSpace(Current.supportEmail) && Current.supportEmail.Contains("@");

        public static void Reload() => _current = Load();

        private static ReleaseConfigurationData Load()
        {
            TextAsset asset = Resources.Load<TextAsset>(ResourceName);
            if (asset == null) return new ReleaseConfigurationData();
            try { return JsonUtility.FromJson<ReleaseConfigurationData>(asset.text) ?? new ReleaseConfigurationData(); }
            catch (ArgumentException) { return new ReleaseConfigurationData(); }
        }
    }
}
