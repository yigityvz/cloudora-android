using System;
using System.Reflection;
using Cloudora.Puzzle;
using Cloudora.UI;
using NUnit.Framework;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    public class LiquidMotionTests
    {
        [Test]
        public void TopEntryConservesVolumeAndNeverFillsBeforeArrival()
        {
            foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))for(int count=1;count<=7;count++)
            {
                float flight=WeatherMotionProfile.Default(type).FlightSeconds,transit=flight*.78f;
                float previous=0,total=flight+(count-1)*.055f+.24f;
                for(int i=0;i<=200;i++)
                {
                    float time=total*i/200;MoveAnimator.TransferVolumes(time,flight,count,.055f,false,out float departed,out float arrived);
                    float left=1-departed,air=departed-arrived;
                    Assert.That(left,Is.InRange(0,1));Assert.That(arrived,Is.GreaterThanOrEqualTo(previous));Assert.That(air,Is.GreaterThanOrEqualTo(-.000001f));
                    if(time<=transit)Assert.That(arrived,Is.EqualTo(0).Within(.000001f));
                    previous=arrived;
                }
                Assert.That(previous,Is.EqualTo(1));
            }
        }
        [Test]
        public void SameElementGrowsOneFluidBodyAndCancellationRestoresTheExactView()
        {
            var host=new GameObject("Fluid preview",typeof(RectTransform),typeof(Canvas));
            var go=new GameObject("Vessel",typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));go.transform.SetParent(host.transform,false);((RectTransform)go.transform).sizeDelta=new Vector2(74,187);var vessel=go.GetComponent<CloudContainerView>();
            try
            {
                foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
                {
                    vessel.Initialize(4,new[]{type,type},null);var fill=go.GetComponentInChildren<LiquidFillGraphic>();var r=(RectTransform)fill.transform;var original=r.anchorMax;
                    vessel.SetTransferPreview(type,2,.5f,1,.8f);
                    Assert.That(r.anchorMax.y,Is.EqualTo(.75f).Within(.00001f));Assert.That(vessel.ElementCount,Is.EqualTo(2));
                    Assert.That(go.GetComponentsInChildren<LiquidFillGraphic>().Length,Is.EqualTo(1),"Same weather must not be stacked into a second block.");
                    Assert.That(go.GetComponentsInChildren<WeatherGlyph>().Length,Is.EqualTo(1),"No duplicated incoming symbol.");
                    vessel.ClearTransferPreview();Assert.That(r.anchorMax,Is.EqualTo(original));Assert.That(fill.Energy,Is.Zero);Assert.That(fill.SurfaceVisible,Is.True);
                }
            }
            finally{Object.DestroyImmediate(host);}
        }
        [Test]
        public void LiquidSurfaceIsBoundedAndItsWarmedMeshAllocatesNoGarbage()
        {
            var go=new GameObject("Liquid mesh",typeof(RectTransform),typeof(CanvasRenderer));((RectTransform)go.transform).sizeDelta=new Vector2(74,90);var fill=go.AddComponent<LiquidFillGraphic>();fill.Top=Color.cyan;fill.Bottom=Color.blue;
            try
            {
                using var vh=new VertexHelper();var method=typeof(LiquidFillGraphic).GetMethod("OnPopulateMesh",BindingFlags.Instance|BindingFlags.NonPublic,null,new[]{typeof(VertexHelper)},null);var draw=(Action<VertexHelper>)Delegate.CreateDelegate(typeof(Action<VertexHelper>),fill,method);
                using var gc=new ProfilerRecorder(ProfilerCategory.Memory,"GC.Alloc",1,ProfilerRecorderOptions.WrapAroundWhenCapacityReached|ProfilerRecorderOptions.SumAllSamplesInFrame|ProfilerRecorderOptions.CollectOnlyOnCurrentThread);
                gc.Start();var probe=new byte[32768];GC.KeepAlive(probe);gc.Stop();bool available=gc.Count>0&&gc.GetSample(0).Count>0;gc.Reset();
                for(int i=0;i<30;i++){fill.SetSurface(true,i*.2f,.8f);draw(vh);}
                gc.Start();for(int i=0;i<100;i++){fill.SetSurface(true,i*.2f,.8f);draw(vh);}gc.Stop();
                if(available)Assert.That(gc.Count>0?gc.GetSample(0).Count:0,Is.Zero);
                Assert.That(vh.currentVertCount,Is.LessThan(1000));
                for(int i=0;i<=100;i++)Assert.That(fill.SurfaceHeight(i/100f),Is.InRange(28.8f,61.2f));
            }
            finally{Object.DestroyImmediate(go);}
        }
    }
}
