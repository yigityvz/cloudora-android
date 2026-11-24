using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cloudora.Puzzle;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Cloudora.Tests.Editor
{
    public class PointerMotionVisualTests
    {
        [UnityTest]
        public IEnumerator TransparentVesselReceivesTouchesAndElementTransfersRenderToCompletion()
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)Assert.Ignore("This visual input test requires the graphics-enabled test run.");
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            var cameraGo=new GameObject("Camera",typeof(Camera));var camera=cameraGo.GetComponent<Camera>();camera.orthographic=true;camera.orthographicSize=667;camera.clearFlags=CameraClearFlags.SolidColor;
            var target=new RenderTexture(750,1334,24);camera.targetTexture=target;
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas),typeof(GraphicRaycaster));var canvas=host.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=10;canvas.scaleFactor=750f/390;
            var eventGo=new GameObject("Events",typeof(EventSystem));var events=eventGo.GetComponent<EventSystem>();
            Canvas.ForceUpdateCanvases();var board=DesignUI.Screen(canvas.transform,"Board");int clicks=0;
            WeatherType[][] values={new[]{WeatherType.Rain,WeatherType.Rain,WeatherType.Sun,WeatherType.Sun},new[]{WeatherType.Sun,WeatherType.Sun},new[]{WeatherType.Rain,WeatherType.Rain},new WeatherType[0]};
            var vessels=new List<CloudContainerView>();
            for(int i=0;i<4;i++){var rect=DesignUI.Rect(board,"Vessel"+i,30+i*85,252,74,172);rect.gameObject.AddComponent<Image>();rect.gameObject.AddComponent<Button>();var v=rect.gameObject.AddComponent<CloudContainerView>();v.Initialize(4,values[i],_=>clicks++);vessels.Add(v);}
            var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);overlay.SetLevelInfo(21,"green-valley");overlay.SetCoins(215);overlay.SetLives(5,"");overlay.SetBoosters(3,1,1,true);
            yield return null;Canvas.ForceUpdateCanvases();camera.Render();
            var pointer=new PointerEventData(events){position=RectTransformUtility.WorldToScreenPoint(camera,((RectTransform)vessels[0].transform).TransformPoint(((RectTransform)vessels[0].transform).rect.center))};
            var hits=new List<RaycastResult>();host.GetComponent<GraphicRaycaster>().Raycast(pointer,hits);
            Assert.That(hits.Exists(r=>r.gameObject==vessels[0].gameObject),Is.True,"The transparent vessel must remain a real touch target.");
            ExecuteEvents.Execute(vessels[0].gameObject,pointer,ExecuteEvents.pointerClickHandler);Assert.That(clicks,Is.EqualTo(1));
            string dir="Logs/ElementMotionReview";Directory.CreateDirectory(dir);var times=new List<string>();int frame=0;var corners=new Vector3[4];
            void Capture()
            {
                float elapsed=frame/30f;
                vessels[0].VisualBody.GetWorldCorners(corners);foreach(var corner in corners){float x=RectTransformUtility.WorldToScreenPoint(camera,corner).x;Assert.That(x,Is.InRange(-.1f,750.1f),"The pouring vessel must stay inside the screen.");}
                Canvas.ForceUpdateCanvases();camera.Render();var prev=RenderTexture.active;RenderTexture.active=target;var image=new Texture2D(750,1334,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,750,1334),0,0);image.Apply();
                string file="frame-"+(frame++).ToString("D4")+".png";File.WriteAllBytes(dir+"/"+file,image.EncodeToPNG());times.Add(elapsed.ToString("F6",CultureInfo.InvariantCulture)+","+file);Object.Destroy(image);RenderTexture.active=prev;
            }
            for(float end=Time.realtimeSinceStartup+.2f;Time.realtimeSinceStartup<end;){Capture();yield return null;}
            vessels[0].SetSelected(true);
            var sourcePosition=vessels[0].VisualBody.position;var targetPosition=vessels[1].VisualBody.position;
            var mover=host.AddComponent<MoveAnimator>();var animation=mover.Animate(vessels[0],vessels[1],WeatherType.Sun,2);
            while(animation.MoveNext()){
                Assert.That(vessels[0].VisualBody.position,Is.EqualTo(sourcePosition));Assert.That(vessels[1].VisualBody.position,Is.EqualTo(targetPosition));
                Assert.That(vessels[0].VisualBody.localRotation,Is.EqualTo(Quaternion.identity));Assert.That(vessels[1].VisualBody.localRotation,Is.EqualTo(Quaternion.identity));
                Capture();yield return animation.Current;
            }
            Assert.That(vessels[0].TryMoveTopGroupTo(vessels[1],2),Is.True);vessels[1].PlaySolvedFeedback();
            for(float end=Time.realtimeSinceStartup+.4f;Time.realtimeSinceStartup<end;){Capture();yield return null;}
            File.WriteAllLines(dir+"/frames.csv",times);Assert.That(vessels[1].IsSolved(),Is.True);
            Canvas.ForceUpdateCanvases();camera.Render();var probe=RectTransformUtility.WorldToScreenPoint(camera,vessels[0].FluidSurfacePosition(-1));var previous=RenderTexture.active;RenderTexture.active=target;var pixels=new Texture2D(750,1334,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,750,1334),0,0);pixels.Apply();Color blue=pixels.GetPixel(Mathf.RoundToInt(probe.x),Mathf.RoundToInt(probe.y));Object.Destroy(pixels);RenderTexture.active=previous;
            Assert.That(blue.b-blue.r,Is.GreaterThan(.2f),"The source fluid must still be visibly rendered after returning from the pour.");
            var caption=DesignUI.Label(board,"",30,190,330,30,18,DesignUI.White,TextAnchor.MiddleCenter);
            foreach(WeatherType type in System.Enum.GetValues(typeof(WeatherType)))
            {
                if(type==WeatherType.Sun)continue;
                var bottom=type==WeatherType.Rain?WeatherType.Sun:WeatherType.Rain;
                vessels[0].RestoreElements(type==WeatherType.Rainbow?new[]{WeatherType.Rain,WeatherType.Snow,type,type}:new[]{bottom,bottom,type,type});vessels[1].RestoreElements(new[]{type,type});
                caption.text=type.ToString();vessels[0].SetSelected(true);
                for(float end=Time.realtimeSinceStartup+.12f;Time.realtimeSinceStartup<end;){Capture();yield return null;}
                var effect=mover.Animate(vessels[0],vessels[1],type,2);
                while(effect.MoveNext()){
                    Assert.That(vessels[0].VisualBody.position,Is.EqualTo(sourcePosition));Assert.That(vessels[1].VisualBody.position,Is.EqualTo(targetPosition));
                    Capture();yield return effect.Current;
                }
                Assert.That(vessels[0].TryMoveTopGroupTo(vessels[1],2),Is.True);
                for(float end=Time.realtimeSinceStartup+.18f;Time.realtimeSinceStartup<end;){Capture();yield return null;}
            }
            vessels[2].gameObject.SetActive(false);vessels[3].gameObject.SetActive(false);
            var ar=(RectTransform)vessels[0].transform;var br=(RectTransform)vessels[1].transform;
            ar.sizeDelta=br.sizeDelta=new Vector2(74,122);ar.anchoredPosition=new Vector2(30,-220);br.anchoredPosition=new Vector2(30,-390);
            vessels[0].RestoreElements(new[]{WeatherType.Rain,WeatherType.Snow,WeatherType.Rainbow,WeatherType.Rainbow});vessels[1].RestoreElements(new WeatherType[0]);
            caption.text="Satırlar arası";Canvas.ForceUpdateCanvases();
            var rowAnimation=mover.Animate(vessels[0],vessels[1],WeatherType.Rainbow,2);
            sourcePosition=vessels[0].VisualBody.position;targetPosition=vessels[1].VisualBody.position;
            while(rowAnimation.MoveNext()){
                Assert.That(vessels[0].VisualBody.position,Is.EqualTo(sourcePosition));Assert.That(vessels[1].VisualBody.position,Is.EqualTo(targetPosition));
                Capture();yield return rowAnimation.Current;
            }
            vessels[0].TryMoveTopGroupTo(vessels[1],2);Capture();
            File.WriteAllLines(dir+"/frames.csv",times);
            Object.Destroy(host);Object.Destroy(eventGo);Object.Destroy(cameraGo);target.Release();Object.Destroy(target);yield return null;yield return new ExitPlayMode();
        }
    }
}
