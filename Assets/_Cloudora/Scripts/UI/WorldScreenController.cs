using Cloudora.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class WorldScreenController : MonoBehaviour
    {
        private RectTransform _root,_viewport,_content,_visit;
        private WorldDefinition _current;
        private int _completed;
        private Text _coins,_lives,_status;
        private RawImage _visitArt;
        private string _visitWorld;private bool _visitRestored;private System.Action _playCurrent;
        public void SetPlayAction(System.Action action)=>_playCurrent=action;
        public static WorldScreenController Create(Canvas canvas)
        {
            var host=new GameObject("WorldScreen",typeof(RectTransform));host.transform.SetParent(canvas.transform,false);SafeAreaFitter.Attach(host);
            var c=host.AddComponent<WorldScreenController>();c.Build();c._root.gameObject.SetActive(false);return c;
        }
        public void Toggle(){bool show=!_root.gameObject.activeSelf;if(show)transform.SetAsLastSibling();_root.gameObject.SetActive(show);}
        public void SetResources(int coins,int lives,string countdown){_coins.text=coins.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));_lives.text=$"{lives}/5";_status.text=lives>=5?"DOLU":countdown;}
        public void Refresh(WorldDefinition world,int completedLevel){_current=world;_completed=completedLevel;Rebuild();}
        private void Build()
        {
            _root=DesignUI.Screen(transform,"Worlds");DesignUI.Art(_root,"12cdb",0,0,390,1300,true);
            var bar=DesignUI.Rect(_root,"Resources",83,-4.5f,287,80);DesignUI.ResourcesBar(bar,out _coins,out _lives,out _status);
            DesignUI.SymbolButton(_root,"Back",20,21,40,Toggle);
            _root.gameObject.AddComponent<PanelPresence>();
            DesignUI.Label(_root,"Dünyalar",20,87,350,35,28,null,TextAnchor.MiddleCenter);
            _viewport=DesignUI.Rect(_root,"Viewport",0,134,390,710);_viewport.gameObject.AddComponent<RectMask2D>();
            var image=_viewport.gameObject.AddComponent<Image>();image.color=Color.clear;
            var scroll=_viewport.gameObject.AddComponent<ScrollRect>();scroll.viewport=_viewport;scroll.horizontal=false;scroll.movementType=ScrollRect.MovementType.Clamped;
            _content=DesignUI.Rect(_viewport,"World cards",0,0,390,1136);scroll.content=_content;
        }
        private void LateUpdate(){if(_visitArt&&_visit){var vr=_visit.rect;_visitArt.rectTransform.sizeDelta=new Vector2(390,vr.height);DesignUI.CoverWorld(_visitArt,_visitWorld,_visitRestored?2:0,390,vr.height);}
            if(!_root)return;var fit=_root.GetComponent<DesignScreenFit>();fit.Apply();_viewport.sizeDelta=new Vector2(390,Mathf.Max(200,fit.Height-134));}
        private void Rebuild()
        {
            if(!_content)return;
            for(int j=_content.childCount-1;j>=0;j--)DesignUI.Remove(_content.GetChild(j).gameObject);
            // Keep the existing world catalog and unlocking boundaries; later worlds remain reachable.
            int index=0;
            foreach(var world in WorldCatalog.All)
            {
                bool restored=_completed>=world.LastLevel,unlocked=_completed>=WorldCatalog.IntroLastLevel&&_completed>=world.FirstLevel-1;
                bool active=_current!=null&&_current.Id==world.Id;
                var card=DesignUI.Panel(_content,world.Id,20,index*228,350,214,"#245772","#12354f",23,active?"#ffd464":"#6da1ba",active?2.5f:1.5f);
                var clip=DesignUI.Panel(card.transform,"Image clip",4,4,342,103,"#ffffff","#ffffff",20,"#ffffff",0);
                clip.gameObject.AddComponent<Mask>().showMaskGraphic=false;
                var picture=DesignUI.WorldArt(clip.transform,world.Id,restored,0,0,342,103);
                if(index<3){var uv=picture.uvRect;uv.y=1-(index==0?2.4937f:index==1?1.4509f:1.1335f)/5.534f-1/5.534f;uv.height=1/5.534f;picture.uvRect=uv;}
                DesignUI.Label(card.transform,(index+1).ToString("00"),14,114,24,26,13,DesignUI.C("#ffd464"));
                DesignUI.Label(card.transform,DesignUI.WorldName(world.Id),43,114,245,26,20);
                if(restored||!unlocked)DesignUI.Art(card.transform,restored?"fa776":"9c136",309,117,20,20);
                DesignUI.Label(card.transform,restored?"KURTARILDI":unlocked?"AKTİF":"KİLİTLİ",14,144,150,18,10,DesignUI.C(restored?"#8ed547":"#ffd464"));
                float progress=RestorationManager.GetProgress(world,_completed);
                DesignUI.Label(card.transform,unlocked?$"%{Mathf.RoundToInt(progress*100)}":"Önceki dünyayı kurtar",150,144,185,18,12,DesignUI.Muted,TextAnchor.MiddleRight);
                var selected=world;
                var button=DesignUI.Button(card.transform,restored?"Ziyaret Et":unlocked?"Devam Et":"Kilitli",14,164,320,44,()=>{if(active&&!restored){Toggle();_playCurrent?.Invoke();}else ShowVisit(selected,restored);},unlocked?(active?"green":"blue"):"disabled");button.interactable=unlocked;
                index++;
            }
            _content.sizeDelta=new Vector2(390,index*228+94);
            DesignUI.Label(_content,"Üç dünya · 100 bölüm\nYeni maceralar yakında.",30,index*228+14,330,58,14,DesignUI.Muted,TextAnchor.MiddleCenter);
        }

        private void ShowVisit(WorldDefinition world,bool restored)
        {
            if(_visit)DesignUI.Remove(_visit.gameObject);
            _visit=DesignUI.Screen(transform,"World visit");DesignUI.Panel(_visit,"Backdrop",0,0,390,1300,"#102f4b","#102f4b",0,"#102f4b",0);
            _visitWorld=world.Id;_visitRestored=restored;_visitArt=DesignUI.WorldArt(_visit,world.Id,restored);WorldAtmosphere.Add(_visit,world.Id);DesignUI.Art(_visit,"5941f",51,93,288,112,true);
            DesignUI.ResourcesBar(_visit,out var coins,out var lives,out var status);
            coins.text=_coins.text;lives.text=_lives.text;status.text=_status.text;
            var controls=DesignUI.Rect(_visit,"Visit controls",20,0,350,300);
            controls.anchorMin=controls.anchorMax=new Vector2(0,0);controls.pivot=new Vector2(0,0);controls.anchoredPosition=new Vector2(20,30);
            var card=DesignUI.Panel(controls,"World card",0,0,350,180);
            DesignUI.Label(card.transform,DesignUI.WorldName(world.Id),18,18,280,34,25);
            if(restored)DesignUI.Art(card.transform,"fa776",304,21,24,24);
            DesignUI.Label(card.transform,MilestoneScreen.Description(world.Id,restored),18,62,314,44,15,DesignUI.Muted);
            DesignUI.Label(card.transform,restored?"KURTARILDI":"DÜNYA KURTULUŞU",18,112,250,20,12,DesignUI.C("#8ed547"));
            float progress=RestorationManager.GetProgress(world,_completed);
            DesignUI.Label(card.transform,$"%{Mathf.RoundToInt(progress*100)}",260,112,72,20,14,DesignUI.C("#ffd464"),TextAnchor.MiddleRight);
            DesignUI.Panel(card.transform,"Progress track",18,145,314,12,"#09273c","#09273c",6,"#497a94",1);
            if(progress>0)DesignUI.Panel(card.transform,"Progress",20,147,310*progress,8,"#d5f49a","#75cd67",4,"#ffffff00",0);
            DesignUI.Button(controls,"Dünyalara Dön",0,195,350,60,()=>DesignUI.Remove(_visit.gameObject),"green");
            if(restored&&(world.Id=="green-valley"||world.Id=="rainy-coast"||world.Id=="frost-peaks"))
                DesignUI.Button(controls,"Kurtuluşu tekrar izle",0,270,350,44,()=>WorldRescueReplay.Show(_visit,world.Id));
        }
    }
}
