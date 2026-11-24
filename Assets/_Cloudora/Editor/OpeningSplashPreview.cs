using System.IO;
using Cloudora.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Editor
{
    public static class OpeningSplashPreview
    {
        public static void RunBatch()
        {
            Directory.CreateDirectory("Logs/SplashPreview");
            foreach (string file in Directory.GetFiles("Assets/_Cloudora/Resources/Splash", "*.png"))
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(file);
                importer.textureType = TextureImporterType.Default;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = true;
                importer.filterMode = FilterMode.Trilinear;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.maxTextureSize = 2048;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
            Capture(390, 844, 0f, false, "start");
            Capture(390, 844, .9f, false, "reveal");
            Capture(390, 844, 2f, false, "hold");
            Capture(375, 667, 2f, false, "hold-iphone-se");
            Capture(393, 852, 2f, false, "hold-tall");
            Capture(390, 844, 2f, true, "reduced-motion");
            Debug.Log("CLOUDORA_SPLASH_PREVIEW PASS");
        }

        private static void Capture(int width, int height, float time, bool reduced, string name)
        {
            var cameraObject = new GameObject("SplashPreviewCamera", typeof(Camera));
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.orthographic = true;
            camera.orthographicSize = height * .5f;
            camera.cullingMask = 1 << 31;
            var target = new RenderTexture(width, height, 24);
            camera.targetTexture = target;
            OpeningSplash splash = OpeningSplash.Create(() => true, reduced);
            splash.enabled = false;
            foreach (Transform child in splash.GetComponentsInChildren<Transform>()) child.gameObject.layer = 31;
            Canvas canvas = splash.GetComponent<Canvas>();
            splash.GetComponent<CanvasScaler>().enabled = false;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 10f;
            canvas.scaleFactor = width / 390f;
            Canvas.ForceUpdateCanvases();
            splash.RenderAt(time);
            camera.Render();
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = target;
            var image = new Texture2D(width, height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes("Logs/SplashPreview/" + name + ".png", image.EncodeToPNG());
            RenderTexture.active = previous;
            Object.DestroyImmediate(splash.gameObject);
            Object.DestroyImmediate(cameraObject);
            Object.DestroyImmediate(image);
            target.Release();
            Object.DestroyImmediate(target);
        }
    }
}
