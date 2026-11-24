using System;
using System.IO;
using System.Reflection;
using Cloudora.Puzzle;
using Cloudora.UI;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;
namespace Cloudora.Editor
{
    public static class WeatherSymbolPreview
    {
        public static void RunBatch()
        {
            DesignPreview.RunBatch();Directory.CreateDirectory("Logs/WeatherSymbols");
            foreach(string name in new[]{"gameplay-four","gameplay-ten","gameplay-se"})File.Copy("Logs/DesignPreview/"+name+".png","Logs/WeatherSymbols/"+name+".png",true);
            Capture(false);Capture(true);Debug.Log("CLOUDORA_WEATHER_SYMBOL_PREVIEW PASS");
        }
        private static void Capture(bool full)
        {
            var cameraObject=new GameObject("Symbol camera",typeof(Camera));var camera=cameraObject.GetComponent<Camera>();camera.orthographic=true;camera.orthographicSize=667;camera.clearFlags=CameraClearFlags.SolidColor;
            var target=new RenderTexture(750,1334,24);camera.targetTexture=target;
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));var canvas=host.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=10;canvas.scaleFactor=750f/390;
            Canvas.ForceUpdateCanvases();var board=DesignUI.Screen(canvas.transform,"Seven elements");
            var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);overlay.SetLevelInfo(75,"frost-peaks");overlay.SetCoins(310);overlay.SetLives(5,"");overlay.SetBoosters(3,1,1,true);
            for(int i=0;i<7;i++)
            {
                var type=(WeatherType)i;float left=i<4?24+i*88:68+(i-4)*88;
                var rect=DesignUI.Rect(board,"Vessel "+type,left,i<4?217:397,78,147);rect.gameObject.AddComponent<Image>();rect.gameObject.AddComponent<Button>();var v=rect.gameObject.AddComponent<CloudContainerView>();
                var bottom=type==WeatherType.Rain?WeatherType.Snow:WeatherType.Rain;
                v.Initialize(4,full?new[]{type,type,type,type}:new[]{bottom,bottom,type,type},null);
            }
            Canvas.ForceUpdateCanvases();foreach(var fit in host.GetComponentsInChildren<DesignScreenFit>())fit.Apply();Canvas.ForceUpdateCanvases();
            foreach(var b in host.GetComponentsInChildren<MonoBehaviour>())if(b is EssenceSymbolFit||b is GameplayOverlay||b is GameplayBackdrop)b.GetType().GetMethod("LateUpdate",BindingFlags.NonPublic|BindingFlags.Instance)?.Invoke(b,null);
            Canvas.ForceUpdateCanvases();camera.Render();var previous=RenderTexture.active;RenderTexture.active=target;var image=new Texture2D(750,1334,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,750,1334),0,0);image.Apply();File.WriteAllBytes("Logs/WeatherSymbols/"+(full?"all-full":"all-mixed")+".png",image.EncodeToPNG());RenderTexture.active=previous;
            Object.DestroyImmediate(host);Object.DestroyImmediate(cameraObject);Object.DestroyImmediate(image);target.Release();Object.DestroyImmediate(target);
        }
    }
}
