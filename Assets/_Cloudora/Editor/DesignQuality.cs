using System.IO;
using UnityEditor;
using UnityEngine;
namespace Cloudora.Editor
{
    public static class DesignQuality
    {
        public static void Configure()
        {
            QualitySettings.globalTextureMipmapLimit=0;
            foreach(string path in Directory.GetFiles("Assets/_Cloudora/Resources/Design","*.png"))
            {
                var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType=TextureImporterType.Default;importer.alphaIsTransparency=true;
                importer.mipmapEnabled=false;importer.filterMode=FilterMode.Bilinear;
                importer.npotScale=TextureImporterNPOTScale.None;importer.maxTextureSize=4096;
                importer.textureCompression=TextureImporterCompression.Uncompressed;
                var ios=importer.GetPlatformTextureSettings("iPhone");ios.name="iPhone";ios.overridden=true;ios.maxTextureSize=4096;ios.format=TextureImporterFormat.RGBA32;ios.textureCompression=TextureImporterCompression.Uncompressed;importer.SetPlatformTextureSettings(ios);
                importer.SaveAndReimport();
            }
            AssetDatabase.SaveAssets();
        }
    }
}
