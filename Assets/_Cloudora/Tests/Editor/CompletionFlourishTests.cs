using System;
using System.Reflection;
using Unity.Profiling;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cloudora.Puzzle;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object=UnityEngine.Object;
namespace Cloudora.Tests.Editor
{
    public class CompletionFlourishTests
    {
        [Test]
        public void RichCompletionMeshesStayBoundedAndAllocateNoGarbageAfterWarmup()
        {
            using var gc=new ProfilerRecorder(ProfilerCategory.Memory,"GC.Alloc",1,ProfilerRecorderOptions.WrapAroundWhenCapacityReached|ProfilerRecorderOptions.SumAllSamplesInFrame|ProfilerRecorderOptions.CollectOnlyOnCurrentThread);
            gc.Start();var probe=new byte[32768];GC.KeepAlive(probe);gc.Stop();
            bool recorder=gc.Count>0&&gc.GetSample(0).Count>0;gc.Reset();
            var host=new GameObject("Completion benchmark",typeof(RectTransform),typeof(Canvas));((RectTransform)host.transform).sizeDelta=new Vector2(390,844);
            var rect=DesignUI.Rect(host.transform,"Vessel",150,300,74,187);rect.gameObject.AddComponent<Image>();rect.gameObject.AddComponent<Button>();var vessel=rect.gameObject.AddComponent<CloudContainerView>();vessel.Initialize(4,new WeatherType[0],null);
            int previous=PlayerPrefs.GetInt("Cloudora.ReducedMotion",0);PlayerPrefs.SetInt("Cloudora.ReducedMotion",0);
            try
            {
                using var mesh=new VertexHelper();
                for(int element=0;element<7;element++)
                {
                    vessel.RestoreElements(Enumerable.Repeat((WeatherType)element,4));vessel.PlaySolvedFeedback();var effect=vessel.GetComponentInChildren<VesselCompletionFlourish>(true);
                    var method=typeof(VesselCompletionFlourish).GetMethod("OnPopulateMesh",BindingFlags.NonPublic|BindingFlags.Instance,null,new[]{typeof(VertexHelper)},null);
                    var draw=(Action<VertexHelper>)Delegate.CreateDelegate(typeof(Action<VertexHelper>),effect,method);int maximum=0;
                    for(int i=0;i<60;i++){effect.RenderAt(VesselCompletionFlourish.Duration*i/60f);draw(mesh);maximum=Mathf.Max(maximum,mesh.currentVertCount);}
                    Assert.That(maximum,Is.InRange(100,24000),"Bounded completion geometry for "+(WeatherType)element);
                    if(recorder)
                    {
                        gc.Reset();gc.Start();for(int i=0;i<100;i++){effect.RenderAt(VesselCompletionFlourish.Duration*i/100f);draw(mesh);}gc.Stop();
                        long count=gc.Count>0?gc.GetSample(0).Count:0;Assert.That(count,Is.Zero,"Warmed completion allocations for "+(WeatherType)element);
                    }
                }
            }
            finally{PlayerPrefs.SetInt("Cloudora.ReducedMotion",previous);Object.DestroyImmediate(host);}
        }
        [UnityTest]
        public IEnumerator FinishCrownsFitTheHudAndDenseBoardEdges()
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)Assert.Ignore("Graphics required for completion layout captures.");
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);yield return new EnterPlayMode();
            int motion=PlayerPrefs.GetInt("Cloudora.ReducedMotion",0);PlayerPrefs.SetInt("Cloudora.ReducedMotion",0);
            var cameraObject=new GameObject("Camera",typeof(Camera));var camera=cameraObject.GetComponent<Camera>();camera.orthographic=true;camera.orthographicSize=667;camera.clearFlags=CameraClearFlags.SolidColor;
            var rt=new RenderTexture(750,1334,24);camera.targetTexture=rt;
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas),typeof(GraphicRaycaster));var canvas=host.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=10;canvas.scaleFactor=750f/1080;Canvas.ForceUpdateCanvases();
            var board=DesignUI.Rect(canvas.transform,"Layout board",0,0,1080,1920);board.anchorMin=Vector2.zero;board.anchorMax=Vector2.one;board.offsetMin=board.offsetMax=Vector2.zero;board.gameObject.AddComponent<GridLayoutGroup>();var layout=board.gameObject.AddComponent<AdaptiveBoardLayout>();
            var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);overlay.SetLevelInfo(75,"frost-peaks");overlay.SetLives(5,"");overlay.SetCoins(1000);overlay.SetBoosters(3,1,1,true);
            Directory.CreateDirectory("Logs/CompletionReview/layouts");
            foreach(int count in new[]{4,6,10})
            {
                var vessels=new List<CloudContainerView>();
                for(int i=0;i<count;i++)
                {
                    var r=DesignUI.Rect(board,"Layout vessel",0,0,74,187);r.gameObject.AddComponent<Image>();r.gameObject.AddComponent<Button>();var v=r.gameObject.AddComponent<CloudContainerView>();v.Initialize(4,new[]{WeatherType.Sun,WeatherType.Rain},null);vessels.Add(v);
                }
                layout.Configure(count,4);yield return null;Canvas.ForceUpdateCanvases();
                var corners=new Vector3[4];foreach(var v in vessels){v.VisualBody.GetWorldCorners(corners);foreach(var point in corners){var screen=RectTransformUtility.WorldToScreenPoint(camera,point);Assert.That(screen.x,Is.InRange(-.1f,750.1f),"Fixture must use the same canvas scale as gameplay");}}

                foreach(var type in new[]{WeatherType.Sun,WeatherType.Lightning,WeatherType.Rainbow})
                {
                    var v=vessels[0];v.RestoreElements(Enumerable.Repeat(type,4));v.PlaySolvedFeedback();var effect=v.GetComponentInChildren<VesselCompletionFlourish>(true);effect.RenderAt(.62f);Canvas.ForceUpdateCanvases();
                    var field=typeof(VesselCompletionFlourish).GetField("_crown",BindingFlags.NonPublic|BindingFlags.Instance);var unitField=typeof(VesselCompletionFlourish).GetField("_unit",BindingFlags.NonPublic|BindingFlags.Instance);
                    var crown=(Vector2)field.GetValue(effect);float unit=(float)unitField.GetValue(effect);var cr=(RectTransform)canvas.transform;
                    var top=cr.InverseTransformPoint(effect.transform.TransformPoint((crown+Vector2.up*58)*unit));
                    Assert.That(top.y,Is.LessThanOrEqualTo(cr.rect.yMax-185*cr.rect.width/390f+.1f),"Finish crown must not cover difficulty or controls");
                    camera.Render();var previous=RenderTexture.active;RenderTexture.active=rt;var pixels=new Texture2D(750,1334,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,750,1334),0,0);pixels.Apply();File.WriteAllBytes("Logs/CompletionReview/layouts/"+count+"-"+type+".png",pixels.EncodeToPNG());Object.Destroy(pixels);RenderTexture.active=previous;
                    effect.StopEffect();v.RestoreElements(new[]{WeatherType.Sun,WeatherType.Rain});
                }
                foreach(var v in vessels)Object.Destroy(v.gameObject);yield return null;
            }
            PlayerPrefs.SetInt("Cloudora.ReducedMotion",motion);Object.Destroy(host);Object.Destroy(cameraObject);rt.Release();Object.Destroy(rt);yield return null;yield return new ExitPlayMode();
        }
        [UnityTest]
        public IEnumerator AllSevenFinishesKeepTheGlassFixedAndCleanUpOnUndo()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);yield return new EnterPlayMode();
            bool graphics=SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;int previousMotion=PlayerPrefs.GetInt("Cloudora.ReducedMotion",0);PlayerPrefs.SetInt("Cloudora.ReducedMotion",0);Time.timeScale=0;
            var cameraObject=new GameObject("Camera",typeof(Camera));var camera=cameraObject.GetComponent<Camera>();camera.orthographic=true;camera.orthographicSize=667;camera.clearFlags=CameraClearFlags.SolidColor;
            var rt=new RenderTexture(750,1334,24);if(graphics)camera.targetTexture=rt;
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));var canvas=host.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=10;canvas.scaleFactor=750f/390;Canvas.ForceUpdateCanvases();
            var board=DesignUI.Screen(canvas.transform,"Completion board");var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);overlay.SetLevelInfo(75,"frost-peaks");overlay.SetLives(5,"");overlay.SetCoins(1000);overlay.SetBoosters(3,1,1,true);
            var rect=DesignUI.Rect(board,"Completed vessel",145,270,100,205);rect.gameObject.AddComponent<Image>();rect.gameObject.AddComponent<Button>();var vessel=rect.gameObject.AddComponent<CloudContainerView>();vessel.Initialize(4,new WeatherType[0],null);
            var title=DesignUI.Label(board,"",30,494,330,30,17,null,TextAnchor.MiddleCenter);
            var frames=new List<string>();string dir="Logs/CompletionReview";Directory.CreateDirectory(dir);int frame=0;float start=Time.realtimeSinceStartup,last=-1;var names=new[]{"Güneş","Yağmur","Kar","Rüzgâr","Ay","Şimşek","Gökkuşağı"};
            void Capture()
            {
                if(!graphics)return;float time=Time.realtimeSinceStartup-start;if(time-last<1f/15)return;last=time;Canvas.ForceUpdateCanvases();camera.Render();var before=RenderTexture.active;RenderTexture.active=rt;var image=new Texture2D(750,1334,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,750,1334),0,0);image.Apply();string file="frame-"+(frame++).ToString("D4")+".png";File.WriteAllBytes(dir+"/"+file,image.EncodeToPNG());frames.Add(time.ToString("F6",CultureInfo.InvariantCulture)+","+file);Object.Destroy(image);RenderTexture.active=before;
            }
            yield return null; Canvas.ForceUpdateCanvases();start=Time.realtimeSinceStartup;
            for(int i=0;i<7;i++)
            {
                var type=(WeatherType)i;vessel.RestoreElements(Enumerable.Repeat(type,4));title.text=names[i]+" tamamlandı";Canvas.ForceUpdateCanvases();
                var position=vessel.VisualBody.position;var rotation=vessel.VisualBody.localRotation;vessel.PlaySolvedFeedback();
                var effect=vessel.GetComponentInChildren<VesselCompletionFlourish>(true);Assert.That(effect.Element,Is.EqualTo(type));
                float deadline=Time.realtimeSinceStartup+VesselCompletionFlourish.Duration+2;while(effect.IsPlaying&&Time.realtimeSinceStartup<deadline){Assert.That(vessel.VisualBody.position,Is.EqualTo(position));Assert.That(vessel.VisualBody.localRotation,Is.EqualTo(rotation));Assert.That(vessel.transform.localScale,Is.EqualTo(Vector3.one));Assert.That(vessel.ElementCount,Is.EqualTo(4));Assert.That(effect.raycastTarget,Is.False);Capture();yield return null;}
                Assert.That(effect.IsPlaying,Is.False);Assert.That(vessel.GetComponentsInChildren<VesselCompletionFlourish>(true).Length,Is.EqualTo(1));
            }
            vessel.PlaySolvedFeedback();yield return null;vessel.RestoreElements(new[]{WeatherType.Rain});Assert.That(vessel.GetComponentInChildren<VesselCompletionFlourish>(true).IsPlaying,Is.False);
            PlayerPrefs.SetInt("Cloudora.ReducedMotion",1);vessel.RestoreElements(Enumerable.Repeat(WeatherType.Sun,4));vessel.PlaySolvedFeedback();yield return new WaitForSecondsRealtime(.25f);Assert.That(vessel.GetComponentInChildren<VesselCompletionFlourish>(true).IsPlaying,Is.False);
            if(graphics)File.WriteAllLines(dir+"/frames.csv",frames);PlayerPrefs.SetInt("Cloudora.ReducedMotion",previousMotion);Time.timeScale=1;Object.Destroy(host);Object.Destroy(cameraObject);if(graphics)rt.Release();Object.Destroy(rt);yield return null;yield return new ExitPlayMode();
        }
    }
}
