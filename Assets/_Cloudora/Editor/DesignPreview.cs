using System.IO;
using System.Reflection;
using Cloudora.UI;
using Cloudora.Puzzle;
using Cloudora.Progression;
using Cloudora.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.Editor
{
    public static class DesignPreview
    {
        public static void RunBatch()
        {
            DesignQuality.Configure();Directory.CreateDirectory("Logs/DesignPreview");
            Capture(375,667,1,"intro-se");Capture(750,1334,21,"home-valley");Capture(390,844,45,"home-coast");Capture(390,844,75,"home-peaks");
            Capture(375,667,21,"gameplay-four");Capture(750,1334,21,"gameplay-flow");Capture(375,667,21,"gameplay-se");Capture(390,844,45,"gameplay-seven");Capture(390,844,75,"gameplay-ten");
            foreach(string name in new[]{"pause","fail","win","settings","worlds","block"})Capture(375,667,21,name);
            Capture(375,667,14,"milestone-intro");Capture(390,844,30,"milestone-world");Capture(375,667,100,"campaign-end");Debug.Log("CLOUDORA_DESIGN_PREVIEW PASS");
        }
        private static void InvokeLate(MonoBehaviour b)=>b.GetType().GetMethod("LateUpdate",BindingFlags.NonPublic|BindingFlags.Instance)?.Invoke(b,null);
        private static void Capture(int width,int height,int level,string name)
        {
            var cameraGo=new GameObject("PreviewCamera",typeof(Camera));var camera=cameraGo.GetComponent<Camera>();camera.clearFlags=CameraClearFlags.SolidColor;camera.orthographic=true;camera.orthographicSize=height*.5f;camera.cullingMask=1<<31;
            var target=new RenderTexture(width,height,24);camera.targetTexture=target;
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas),typeof(GraphicRaycaster));var canvas=host.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=10;canvas.scaleFactor=width/390f;
            Canvas.ForceUpdateCanvases();
            var menu=MainMenuController.Create(canvas,WorldCatalog.ForLevel(level).Id,level,()=>{},()=>true,v=>{},()=>true,v=>{});menu.SetResources(215,5,"",name=="campaign-end"?100:level-1);
            foreach(var splash in Object.FindObjectsByType<OpeningSplash>(FindObjectsSortMode.None))Object.DestroyImmediate(splash.gameObject);
            if(name=="settings")menu.ShowSettings();
            if(name=="worlds"){var worlds=WorldScreenController.Create(canvas);worlds.Refresh(WorldCatalog.ForLevel(45),40);worlds.SetResources(215,5,"");worlds.Toggle();}
            if(name.StartsWith("gameplay")||name=="pause"||name=="win"||name=="fail"||name=="block")
            {
                menu.gameObject.SetActive(false);
                var board=DesignUI.Screen(canvas.transform,"Board preview");var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);
                overlay.SetLevelInfo(level,WorldCatalog.ForLevel(level).Id);overlay.SetCoins(215);overlay.SetLives(5,"");overlay.SetBoosters(3,0,1,true);
                int count=name=="gameplay-four"?4:name=="gameplay-seven"?7:name=="gameplay-ten"?10:6;
                int capacity=count==4?5:count>=7?6:4;float h=height*390f/width;
                var metrics=AdaptiveBoardLayout.Calculate(new Vector2(351,h*.76f-174)*(1080f/390),count,capacity);
                Vector2 size=metrics.CellSize*(390f/1080),gap=metrics.Spacing*(390f/1080);int columns=metrics.Columns,rows=Mathf.CeilToInt(count/(float)columns);
                float left=(390-columns*size.x-(columns-1)*gap.x)/2,top=h*.24f+(h*.76f-174-rows*size.y-(rows-1)*gap.y)/2;
                var dead=V3BoardText.Parse("RRR/NNN/NRSSS/SRNS");
                for(int i=0;i<count;i++)
                {
                    var rect=DesignUI.Rect(board,"Vessel",left+i%columns*(size.x+gap.x),top+i/columns*(size.y+gap.y),size.x,size.y);rect.gameObject.AddComponent<Image>();rect.gameObject.AddComponent<Button>();var v=rect.gameObject.AddComponent<CloudContainerView>();
                    v.Initialize(capacity,count==4?dead[i]:i<count-2?new[]{WeatherType.Rain,WeatherType.Rain,WeatherType.Sun,WeatherType.Snow}:new WeatherType[0],null);
                }
                if(name=="pause")overlay.transform.Find("PausePanel").gameObject.SetActive(true);
                if(name=="win")overlay.ShowComplete(true,CoinEconomy.RewardFor(level));if(name=="fail")overlay.ShowFail(new FailOptions(true,false,true,CoinEconomy.ContinueCost,false));if(name=="block")overlay.ShowBlock("Canın Kalmadı","Yeni canın dolmasını bekleyebilir veya can kazanabilirsin.");
            }
            if(name.StartsWith("milestone")){menu.gameObject.SetActive(false);var screen=DesignUI.Screen(canvas.transform,"Milestone preview");var m=MilestoneScreen.Create(screen,level,()=>{},()=>{},()=>{});m.SetResources("215","5/5","DOLU");}
            foreach(var safe in host.GetComponentsInChildren<SafeAreaFitter>()){safe.enabled=false;var r=(RectTransform)safe.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;}
            Canvas.ForceUpdateCanvases();foreach(var fit in host.GetComponentsInChildren<DesignScreenFit>())fit.Apply();Canvas.ForceUpdateCanvases();
            foreach(var b in host.GetComponentsInChildren<MonoBehaviour>())if(b is MainMenuController||b is GameplayOverlay||b is GameplayBackdrop||b is WorldScreenController||b is EssenceSymbolFit||b is DesignModalFit||b is MilestoneScreen)InvokeLate(b);
            Canvas.ForceUpdateCanvases();
            if(name=="gameplay-flow")
            {
                var vessels=host.GetComponentsInChildren<CloudContainerView>();var body=vessels[0].VisualBody;var to=vessels[4].VisualBody;
                var flowGo=new GameObject("Pour preview",typeof(RectTransform),typeof(CanvasRenderer));flowGo.transform.SetParent(canvas.transform,false);var fr=(RectTransform)flowGo.transform;fr.anchorMin=Vector2.zero;fr.anchorMax=Vector2.one;fr.offsetMin=fr.offsetMax=Vector2.zero;
                var flow=flowGo.AddComponent<WeatherFlowGraphic>();flow.Unit=((RectTransform)canvas.transform).rect.width/390f;flow.From=fr.InverseTransformPoint(vessels[0].FluidSurfacePosition(-.5f));flow.SourceMouth=fr.InverseTransformPoint(vessels[0].MouthPosition);flow.TargetMouth=fr.InverseTransformPoint(vessels[4].MouthPosition);flow.Apex=Mathf.Max(flow.SourceMouth.y,flow.TargetMouth.y)+36*flow.Unit;flow.Elapsed=.32f;flow.Profile=WeatherMotionProfile.Default(WeatherType.Snow);flow.Tint=CloudContainerView.GetColor(WeatherType.Snow);flow.Landing=fr.InverseTransformPoint(vessels[4].FluidSurfacePosition(.5f));
                vessels[0].SetSelected(true);
                vessels[0].SetTransferPreview(WeatherType.Snow,1,-.5f);vessels[4].SetTransferPreview(WeatherType.Snow,1,.5f);
            }
            foreach(var t in host.GetComponentsInChildren<Transform>())t.gameObject.layer=31;Canvas.ForceUpdateCanvases();camera.Render();var previous=RenderTexture.active;RenderTexture.active=target;
            var image=new Texture2D(width,height,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,width,height),0,0);image.Apply();File.WriteAllBytes("Logs/DesignPreview/"+name+".png",image.EncodeToPNG());RenderTexture.active=previous;
            Object.DestroyImmediate(host);Object.DestroyImmediate(cameraGo);Object.DestroyImmediate(image);target.Release();Object.DestroyImmediate(target);
        }
    }
}
