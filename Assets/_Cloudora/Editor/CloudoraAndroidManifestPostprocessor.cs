using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor.Android;
using UnityEngine;

namespace Cloudora.Editor
{
    public sealed class CloudoraAndroidManifestPostprocessor : IPostGenerateGradleAndroidProject
    {
        private static readonly XNamespace AndroidNamespace = "http://schemas.android.com/apk/res/android";
        public int callbackOrder => 1000;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            if (NetworkEnabled()) return;

            string manifestPath = Path.Combine(path, "src", "main", "AndroidManifest.xml");
            if (!File.Exists(manifestPath))
            {
                Debug.LogWarning($"Cloudora manifest postprocessor could not find {manifestPath}");
                return;
            }

            string original = File.ReadAllText(manifestPath);
            string filtered = RemoveInternetPermission(original);
            if (!string.Equals(original, filtered, StringComparison.Ordinal))
            {
                File.WriteAllText(manifestPath, filtered);
                Debug.Log("Cloudora removed INTERNET permission because network services are disabled.");
            }
        }

        public static string RemoveInternetPermission(string xml)
        {
            XDocument document = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
            XElement[] internetPermissions = document.Descendants("uses-permission")
                .Where(element => string.Equals((string)element.Attribute(AndroidNamespace + "name"), "android.permission.INTERNET", StringComparison.Ordinal))
                .ToArray();
            foreach (XElement permission in internetPermissions) permission.Remove();
            return document.ToString(SaveOptions.DisableFormatting);
        }

        private static bool NetworkEnabled()
        {
            string value = Environment.GetEnvironmentVariable("CLOUDORA_ENABLE_NETWORK");
            return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
