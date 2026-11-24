using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Cloudora.Puzzle;
using Cloudora.UI;
using UnityEditor;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace Cloudora.Editor
{
    // CPU mesh construction benchmark; not a device FPS claim. Run before/after with
    // the same editor/fixtures. Warm-up excludes shader/import and initial buffer costs.
    public static class RenderingPerformanceBenchmark
    {
        [Serializable] public class Row { public string name; public int iterations,vertices; public long bytes=-1,allocations=-1; public double milliseconds; }
        [Serializable] public class Report { public string unity;public bool allocationCounterAvailable;public Row[] rows; }
        public static void Before()=>Run("before");
        public static void After()=>Run("after");
        private static void Run(string stage)
        {
            string dir="Logs/PerformanceReview/"+stage;Directory.CreateDirectory(dir);
            using var gc=new ProfilerRecorder(ProfilerCategory.Memory,"GC.Alloc",1,ProfilerRecorderOptions.WrapAroundWhenCapacityReached|ProfilerRecorderOptions.SumAllSamplesInFrame|ProfilerRecorderOptions.CollectOnlyOnCurrentThread);
            gc.Start();var probe=new byte[32768];GC.KeepAlive(probe);gc.Stop();bool gcAvailable=gc.Count>0&&gc.GetSample(0).Count>0;gc.Reset();
            var rows=new List<Row>();var objects=new List<GameObject>();
            T Make<T>(string name) where T:MaskableGraphic
            {
                var go=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer));objects.Add(go);var r=(RectTransform)go.transform;r.sizeDelta=new Vector2(90,190);return go.AddComponent<T>();
            }
            void Measure(string name,MaskableGraphic graphic)
            {
                var method=graphic.GetType().GetMethod("OnPopulateMesh",BindingFlags.NonPublic|BindingFlags.Instance,null,new[]{typeof(VertexHelper)},null);
                var populate=(Action<VertexHelper>)Delegate.CreateDelegate(typeof(Action<VertexHelper>),graphic,method);
                using var vh=new VertexHelper();for(int i=0;i<20;i++)populate(vh);
                const int samples=200;gc.Start();long time=Stopwatch.GetTimestamp();
                for(int i=0;i<samples;i++)populate(vh);
                gc.Stop();long allocations=gcAvailable?(gc.Count>0?gc.GetSample(0).Count:0):-1;gc.Reset();
                rows.Add(new Row{name=name,iterations=samples,vertices=vh.currentVertCount,allocations=allocations,milliseconds=(Stopwatch.GetTimestamp()-time)*1000.0/Stopwatch.Frequency});
            }
            foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
            {
                var flow=Make<WeatherFlowGraphic>("Flow");flow.From=new Vector2(-130,-40);flow.SourceMouth=new Vector2(-130,30);flow.TargetMouth=new Vector2(120,30);flow.Landing=new Vector2(120,-50);flow.Apex=72;flow.Unit=1;flow.PacketCount=4;flow.Elapsed=.30f;flow.Profile=WeatherMotionProfile.Default(type);flow.Tint=CloudContainerView.GetColor(type);Measure("flow-"+type,flow);
            }
            foreach(string world in new[]{"green-valley","rainy-coast","frost-peaks",""})
            {var air=Make<WorldAtmosphere>("Air");air.rectTransform.sizeDelta=new Vector2(390,844);air.World=world;Measure("air-"+world,air);}
            var glass=Make<GlassVesselGraphic>("Glass");glass.Selected=true;glass.SelectionGlow=1;Measure("selected-glass",glass);
            var panel=Make<DesignPanel>("Panel");panel.Top=Color.white;panel.Bottom=Color.blue;panel.Border=Color.cyan;Measure("panel",panel);
            var vesselObject=new GameObject("Vessel reuse",typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));objects.Add(vesselObject);
            var vessel=vesselObject.GetComponent<CloudContainerView>();vessel.Initialize(4,new[]{WeatherType.Rain,WeatherType.Sun,WeatherType.Rain,WeatherType.Sun},null);
            var restore=typeof(CloudContainerView).GetMethod("RefreshVisuals",BindingFlags.NonPublic|BindingFlags.Instance);
            var refresh=(Action)Delegate.CreateDelegate(typeof(Action),vessel,restore);
            for(int i=0;i<3;i++)refresh();gc.Start();long ticks=Stopwatch.GetTimestamp();
            for(int i=0;i<40;i++)refresh();gc.Stop();rows.Add(new Row{name="vessel-refresh",iterations=40,allocations=gcAvailable?(gc.Count>0?gc.GetSample(0).Count:0):-1,milliseconds=(Stopwatch.GetTimestamp()-ticks)*1000.0/Stopwatch.Frequency});
            foreach(var go in objects)Object.DestroyImmediate(go);
            File.WriteAllText(dir+"/metrics.json",JsonUtility.ToJson(new Report{unity=Application.unityVersion,allocationCounterAvailable=gcAvailable,rows=rows.ToArray()},true));
            DesignPreview.RunBatch();
            foreach(string name in new[]{"gameplay-four","gameplay-ten","pause","settings","home-valley"})File.Copy("Logs/DesignPreview/"+name+".png",dir+"/"+name+".png",true);
            UnityEngine.Debug.Log("CLOUDORA_PERFORMANCE_REVIEW "+stage+" complete");
        }
    }
}
