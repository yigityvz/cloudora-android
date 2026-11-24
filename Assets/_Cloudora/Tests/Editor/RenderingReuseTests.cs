using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cloudora.Puzzle;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Unity.Profiling;
using System;
using Object=UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    public class RenderingReuseTests
    {
        [Test]
        public void WarmedElementMeshesGenerateNoManagedGarbage()
        {
            using var gc=new ProfilerRecorder(ProfilerCategory.Memory,"GC.Alloc",1,ProfilerRecorderOptions.WrapAroundWhenCapacityReached|ProfilerRecorderOptions.SumAllSamplesInFrame|ProfilerRecorderOptions.CollectOnlyOnCurrentThread);
            gc.Start();var probe=new byte[32768];GC.KeepAlive(probe);gc.Stop();
            if(gc.Count==0||gc.GetSample(0).Count==0)Assert.Ignore("The allocation recorder is unavailable in this test environment.");
            gc.Reset();
            var go=new GameObject("Mesh allocation test",typeof(RectTransform),typeof(CanvasRenderer));var flow=go.AddComponent<WeatherFlowGraphic>();
            try
            {
                flow.From=new Vector2(-130,-40);flow.SourceMouth=new Vector2(-130,30);flow.TargetMouth=new Vector2(120,30);flow.Landing=new Vector2(120,-50);flow.Apex=72;flow.PacketCount=4;flow.Elapsed=.30f;
                var method=typeof(WeatherFlowGraphic).GetMethod("OnPopulateMesh",BindingFlags.NonPublic|BindingFlags.Instance,null,new[]{typeof(VertexHelper)},null);
                var populate=(Action<VertexHelper>)Delegate.CreateDelegate(typeof(Action<VertexHelper>),flow,method);
                using var vh=new VertexHelper();
                foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
                {
                    flow.Profile=WeatherMotionProfile.Default(type);flow.Tint=CloudContainerView.GetColor(type);
                    for(int i=0;i<20;i++)populate(vh);
                    gc.Reset();gc.Start();for(int i=0;i<100;i++)populate(vh);gc.Stop();
                    long allocations=gc.Count>0?gc.GetSample(0).Count:0;
                    Assert.That(allocations,Is.Zero,$"Warmed mesh allocations for {type}");
                }
            }
            finally{Object.DestroyImmediate(go);}
        }

        [UnityTest]
        public IEnumerator RepeatedMovesReuseGlassAndBoundedRegionsAndKeepUndoAccurate()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            var host=new GameObject("Reuse canvas",typeof(RectTransform),typeof(Canvas));
            var source=Make(host.transform,"Source");var target=Make(host.transform,"Target");
            var snapshot=new[]{WeatherType.Rain,WeatherType.Sun,WeatherType.Rain,WeatherType.Sun};
            source.RestoreElements(snapshot);
            var glass=source.VisualBody.Find("Glass").GetEntityId();
            var top=source.VisualBody.Find("Essence regions");
            for(int pass=0;pass<3;pass++)foreach(WeatherType type in System.Enum.GetValues(typeof(WeatherType)))
            {
                source.RestoreElements(new[]{type,type,type,type});source.SetTransferPreview(type,2,-.5f);target.SetTransferPreview(type,2,.5f);source.ClearTransferPreview();target.ClearTransferPreview();
            }
            source.RestoreElements(snapshot);target.RestoreElements(System.Array.Empty<WeatherType>());
            source.TryMoveTopGroupTo(target);source.RestoreElements(snapshot);target.RestoreElements(System.Array.Empty<WeatherType>());
            int children=host.GetComponentsInChildren<Transform>(true).Length;
            for(int i=0;i<60;i++)
            {
                Assert.That(source.TryMoveTopGroupTo(target),Is.True);source.RestoreElements(snapshot);target.RestoreElements(System.Array.Empty<WeatherType>());
                Assert.That(source.CaptureElements(),Is.EqualTo(snapshot));
                Assert.That(source.VisualBody.Find("Glass").GetEntityId(),Is.EqualTo(glass));
                Assert.That(host.GetComponentsInChildren<Transform>(true).Length,Is.EqualTo(children),"A warmed vessel must not recreate its visual tree on each move/undo.");
                Assert.That(top.childCount,Is.LessThanOrEqualTo(source.Capacity+1));
            }
            var mover=host.AddComponent<MoveAnimator>();
            yield return mover.Animate(source,target,WeatherType.Sun,1);
            var flow=host.GetComponentInChildren<WeatherFlowGraphic>(true);var effectId=flow.GetEntityId();
            Assert.That(flow.gameObject.activeSelf,Is.False);
            yield return mover.Animate(source,target,WeatherType.Sun,1);
            Assert.That(host.GetComponentInChildren<WeatherFlowGraphic>(true).GetEntityId(),Is.EqualTo(effectId));
            Object.Destroy(host);yield return null;yield return new ExitPlayMode();
        }

        [Test]
        public void WorldChangeInvalidatesAtmosphereEvenWhenMotionIsStatic()
        {
            var host=new GameObject("Static atmosphere",typeof(RectTransform),typeof(CanvasRenderer));
            try{var air=host.AddComponent<WorldAtmosphere>();int changes=0;air.RegisterDirtyVerticesCallback(()=>changes++);air.World="green-valley";int first=changes;air.World="rainy-coast";Assert.That(changes,Is.GreaterThan(first));first=changes;air.World="rainy-coast";Assert.That(changes,Is.EqualTo(first));}
            finally{Object.DestroyImmediate(host);}
        }

        [Test]
        public void RepeatedFailedStateDoesNotRebuildItsText()
        {
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));
            try
            {
                var overlay=GameplayOverlay.Create(host.GetComponent<Canvas>(),null,null,null,null,null,null,null,null,null,null);
                var options=new FailOptions(true,false,true,500,true);overlay.ShowFail(options);
                int dirties=0;foreach(var text in overlay.GetComponentsInChildren<Text>(true))text.RegisterDirtyVerticesCallback(()=>dirties++);
                for(int i=0;i<120;i++){overlay.ShowFail(options);overlay.SetLives(5,"");overlay.SetBusy(false);}
                int afterWarm=dirties;
                for(int i=0;i<120;i++){overlay.ShowFail(options);overlay.SetLives(5,"");overlay.SetBusy(false);}
                Assert.That(dirties,Is.EqualTo(afterWarm));
                overlay.HideFail();overlay.ShowFail(options);Assert.That(overlay.transform.Find("FailPanel").gameObject.activeSelf,Is.True);
            }
            finally{Object.DestroyImmediate(host);}
        }
        private static CloudContainerView Make(Transform parent,string name)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));go.transform.SetParent(parent,false);((RectTransform)go.transform).sizeDelta=new Vector2(90,187);var c=go.GetComponent<CloudContainerView>();c.Initialize(4,System.Array.Empty<WeatherType>(),null);return c;
        }
    }
}
