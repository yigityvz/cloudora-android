using System;
using Cloudora.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // Presentation only: advancing and rewards remain owned by PuzzleController.
    public sealed class MilestoneScreen : MonoBehaviour
    {
        private RectTransform _screen,_body;
        private RawImage _art;
        private Text _coins,_lives,_status;
        private Action _next,_worlds,_settings;
        private int _completed;
        private bool _advancing;
        private string _displayWorld;private int _stage;private WorldAtmosphere _atmosphere;
        public static MilestoneScreen Create(Transform parent,int completed,Action next,Action worlds,Action settings)
        {
            var root=DesignUI.Rect(parent,"Milestone",0,0,390,844);
            root.anchorMin=Vector2.zero;root.anchorMax=Vector2.one;root.offsetMin=root.offsetMax=Vector2.zero;
            var c=root.gameObject.AddComponent<MilestoneScreen>();c._screen=root;c._completed=completed;c._next=next;c._worlds=worlds;c._settings=settings;
            DesignUI.Panel(root,"Backdrop",0,0,390,1300,"#102f4b","#102f4b",0,"#00000000",0);
            c._art=DesignUI.WorldArt(root,WorldCatalog.ForLevel(completed).Id,true);
            if(completed==WorldCatalog.IntroLastLevel){c._art.texture=Resources.Load<Texture2D>("Design/12cdb");c._art.uvRect=new Rect(0,0,1,1);c._art.rectTransform.sizeDelta=new Vector2(390,1300);DesignUI.Art(root,"5941f",51,93,288,112,true);}
            if(completed!=WorldCatalog.IntroLastLevel)DesignUI.Panel(root,"Restoration dim",0,0,390,1300,"#061d35ad","#061d35ad",0,"#00000000",0);
            c._displayWorld=completed==WorldCatalog.IntroLastLevel?"":WorldCatalog.ForLevel(completed).Id;c._stage=completed==WorldCatalog.IntroLastLevel?0:2;c._atmosphere=WorldAtmosphere.Add(root,c._displayWorld);
            DesignUI.ResourcesBar(root,out c._coins,out c._lives,out c._status);
            DesignUI.SymbolButton(root,"Gear",330,21,40,settings);
            if(completed==WorldCatalog.IntroLastLevel)c.IntroComplete();else {c.Rescued();if(Application.isPlaying)WorldRescueReplay.Show(root,c._displayWorld);}return c;
        }
        public void SetResources(string coins,string lives,string status){_coins.text=coins;_lives.text=lives;_status.text=status;}
        private RectTransform Card(float height,float bottom=82)
        {
            if(_body)DesignUI.Remove(_body.gameObject);
            _body=DesignUI.Panel(_screen,"Milestone card",22,0,346,height).rectTransform;
            _body.anchorMin=_body.anchorMax=new Vector2(.5f,0);_body.pivot=new Vector2(.5f,0);_body.anchoredPosition=new Vector2(0,bottom);
            return _body;
        }
        private void IntroComplete()
        {
            var p=Card(322,167);DesignUI.Art(p,"6ca6c",128,24,90,90);
            DesignUI.Label(p,"Tanıtım tamamlandı!",24,129,298,18,13,DesignUI.C("#ffd464"),TextAnchor.MiddleCenter);
            DesignUI.Label(p,"İlk dünyanı kurtarmaya hazırsın!",24,162,298,64,26,null,TextAnchor.MiddleCenter);
            DesignUI.Button(p,"Yeşil Vadi’yi Keşfet",24,238,298,60,ShowNextWorld,"green");
        }
        private void Rescued()
        {
            var world=WorldCatalog.ForLevel(_completed);var p=Card(445,90);
            DesignUI.Art(p,"4e035",149,23,48,48);
            DesignUI.Label(p,DesignUI.WorldName(world.Id)+" kurtarıldı!",23,86,300,66,27,DesignUI.C("#ffd464"),TextAnchor.MiddleCenter);
            DesignUI.Label(p,Description(world.Id,true),23,154,300,42,16,DesignUI.Muted,TextAnchor.MiddleCenter);
            DesignUI.Label(p,"DÜNYA KURTULUŞU",23,202,220,18,12,DesignUI.Muted);
            DesignUI.Label(p,"%100",265,202,58,18,14,DesignUI.C("#ffd464"),TextAnchor.MiddleRight);
            DesignUI.Panel(p,"Progress",23,227,300,12,"#d5f49a","#75cd67",6,"#497a94",1);
            DesignUI.Button(p,_completed>=WorldCatalog.MaxLevel?"100 bölüm tamamlandı • Ana Sayfa":"Yeni Dünyayı Keşfet",23,254,300,60,ShowNextWorld,"green");
            DesignUI.Button(p,"Dünyalar",23,329,300,44,_worlds);
            if(world.Id=="green-valley"||world.Id=="rainy-coast"||world.Id=="frost-peaks")
                DesignUI.Button(p,"Kurtuluşu tekrar izle",23,388,300,34,()=>WorldRescueReplay.Show(_screen,world.Id));
        }
        private void ShowNextWorld()
        {
            var dim=_screen.Find("Restoration dim");if(dim)DesignUI.Remove(dim.gameObject);
            var next=WorldCatalog.ForLevel(_completed+1);
            _displayWorld=next.Id;_stage=0;_atmosphere.World=next.Id;
            if(_completed>=WorldCatalog.All[WorldCatalog.All.Count-1].LastLevel){Advance();return;}
            var art=DesignUI.WorldArt(_screen,next.Id,false);art.transform.SetSiblingIndex(_art.transform.GetSiblingIndex());DesignUI.Remove(_art.gameObject);_art=art;
            var logo=_screen.Find("5941f");if(logo)DesignUI.Remove(logo.gameObject);
            var p=Card(227);DesignUI.Label(p,DesignUI.WorldName(next.Id),24,30,298,42,31,null,TextAnchor.MiddleCenter);
            DesignUI.Label(p,Description(next.Id,false),24,88,298,48,17,DesignUI.Muted,TextAnchor.MiddleCenter);
            DesignUI.Button(p,$"Oyna — Bölüm {_completed+1}",24,143,298,60,Advance,"green");
        }
        private void Advance(){if(_advancing)return;_advancing=true;_next?.Invoke();_advancing=false;}
        private void LateUpdate(){if(!_body)return;float h=_screen.rect.height;
            if(_art){_art.rectTransform.sizeDelta=new Vector2(390,h);DesignUI.CoverWorld(_art,_displayWorld,_stage,390,h);}
            _body.localScale=Vector3.one*Mathf.Min(1,Mathf.Max(.5f,(h-190)/_body.sizeDelta.y));}
        public static string Description(string world,bool restored)=>world=="green-valley"?(restored?"Dere yeniden akıyor, vadi hayat buldu.":"Bulutları dengele, vadiye yeniden hayat ver."):world=="rainy-coast"?(restored?"Fırtına dindi, kıyı yeniden huzurlu.":"Fırtınayı dindir, kıyıya huzur getir."):world=="frost-peaks"?(restored?"Tipi dindi, dağ yolları yeniden açıldı.":"Zirvelere dengeyi geri getir."):(restored?"Hava dengelendi, dünya yeniden canlandı.":"Hava türlerini birleştir, dünyaya denge getir.");
    }
}
