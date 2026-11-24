using System;
using Cloudora.Services;
using Cloudora.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        private RectTransform _menu, _settings, _controls;
        private Text _coins, _lives, _status;
        private string _world; private int _level, _completed;
        private Action _onWorld,_onLifeAd;
        private Func<bool> _sound,_haptics;private Action<bool> _setSound,_setHaptics;
        private RawImage _background;
        private DesignPanel _fade;
        private Button _playButton;private int _availableLives=5;
        private WorldAtmosphere _atmosphere;private bool _layoutDirty=true;private float _layoutHeight=-1;
        public void Hide()=>_menu.gameObject.SetActive(false);
        public void Show() { _menu.gameObject.SetActive(true); transform.SetAsLastSibling(); }
        public void SetProgress(string world,int level) { _world=world;_level=level;RebuildControls(); }
        public void SetResources(int coins,int lives,string countdown,int completed)
        {
            _availableLives=lives;
            _coins.text=coins.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
            _lives.text=$"{lives}/5";_status.text=lives>=5?"DOLU":countdown;
            if(_completed!=completed){_completed=completed;RebuildControls();}
            if(_playButton){bool refill=lives<=0&&_level>5;_playButton.interactable=!refill||_onLifeAd!=null;_playButton.GetComponentInChildren<Text>().text=refill?"Can Kazan · +1":$"Oyna · Bölüm {_level}";}
        }
        public static MainMenuController Create(Canvas canvas,string world,int level,Action onWorld,Func<bool> sound,Action<bool> setSound,Func<bool> haptics,Action<bool> setHaptics,Action onLifeAd=null)
        {
            var host=new GameObject("MainMenuController",typeof(RectTransform));host.transform.SetParent(canvas.transform,false);SafeAreaFitter.Attach(host);
            var c=host.AddComponent<MainMenuController>();c._world=world;c._level=level;c._onWorld=onWorld;c._onLifeAd=onLifeAd;c._sound=sound;c._setSound=setSound;c._haptics=haptics;c._setHaptics=setHaptics;c.Build();
            OpeningSplash.Create(()=>c!=null&&c._menu!=null);return c;
        }
        private void Build()
        {
            _menu=DesignUI.Screen(transform,"MainMenu");
            DesignUI.Panel(_menu,"Backdrop",0,0,390,1300,"#102f4b","#102f4b",0,"#102f4b",0);
            _background=DesignUI.Art(_menu,"12cdb",0,0,390,844,true);
            _fade=DesignUI.Panel(_menu,"Landscape fade",0,490,390,354,"#102f4b00","#102f4b",0,"#00000000",0);_fade.raycastTarget=false;
            _atmosphere=WorldAtmosphere.Add(_menu,"");
            DesignUI.Art(_menu,"5941f",51,93,288,112,true);
            DesignUI.ResourcesBar(_menu,out _coins,out _lives,out _status);
            DesignUI.SymbolButton(_menu,"Gear",330,21,40,ShowSettings);
            _menu.gameObject.AddComponent<PanelPresence>();
            RebuildControls();
        }
        private void LateUpdate()
        {
            if(!_menu||!_controls||!_menu.gameObject.activeInHierarchy)return;
            var fit=_menu.GetComponent<DesignScreenFit>();fit.Apply();float h=fit.Height;if(!_layoutDirty&&h==_layoutHeight)return;_layoutDirty=false;_layoutHeight=h;
            _background.rectTransform.sizeDelta=new Vector2(390,h);
            bool intro=_level<=WorldCatalog.IntroLastLevel;float progress=RestorationManager.GetProgress(WorldCatalog.ForLevel(_level),_completed);
            DesignUI.CoverWorld(_background,intro?"":_world,progress>=1?2:progress>=.5f?1:0,390,h);
            _fade.gameObject.SetActive(true);_fade.rectTransform.anchoredPosition=Vector2.zero;_fade.rectTransform.sizeDelta=new Vector2(390,h);
            _atmosphere.World=intro?"":_world;
            _controls.anchoredPosition=new Vector2(20,-Mathf.Max(235,h-(intro?270:286)));
        }

        private void RebuildControls()
        {
            _layoutDirty=true;
            if(!_menu)return;
            if(_controls){_controls.gameObject.SetActive(false);DesignUI.Remove(_controls.gameObject);}
            _controls=DesignUI.Rect(_menu,"Home controls",20,530,350,260);
            bool intro=_level<=Cloudora.Progression.WorldCatalog.IntroLastLevel;
            if(_completed>=WorldCatalog.MaxLevel)
            {
                _background.texture=Resources.Load<Texture2D>("Design/84576");_background.uvRect=new Rect(2.0259f/3.0311f,0,1/3.0311f,1);
                var finish=DesignUI.Panel(_controls,"Campaign complete",0,0,350,250);
                DesignUI.Label(finish.transform,"100 bölüm tamamlandı!",20,20,310,60,26,null,TextAnchor.MiddleCenter);
                DesignUI.Label(finish.transform,"Üç dünyaya dengeyi geri getirdin. Yeni maceralar yakında!",20,91,310,60,16,DesignUI.Muted,TextAnchor.MiddleCenter);
                DesignUI.Button(finish.transform,"Dünyaları Ziyaret Et",20,170,310,60,()=>_onWorld?.Invoke(),"green");return;
            }
            if(intro)
            {
                _background.texture=Resources.Load<Texture2D>("Design/12cdb");_background.uvRect=new Rect(0,0,1,1);
                var p=DesignUI.Panel(_controls,"Intro card",0,0,350,244);
                DesignUI.Label(p.transform,"Gökyüzünü tanı",18,18,314,31,25);
                _playButton=DesignUI.Button(p.transform,$"Oyna — Bölüm {_level}",18,76,314,52,PlayOrRefill,"gold");
                DesignUI.Button(p.transform,"Dünyalar",18,146,314,42,()=>{_onWorld?.Invoke();},"disabled");
                DesignUI.Art(p.transform,"2ceb3",99,158,16,16);
                DesignUI.Label(p.transform,"15. bölümde açılır",18,207,314,16,11,DesignUI.Muted,TextAnchor.MiddleCenter);
            }
            else
            {
                string a=_world=="green-valley"?"2ce2b":_world=="rainy-coast"?"232d2":_world=="frost-peaks"?"84576":"12cdb";
                _background.texture=Resources.Load<Texture2D>("Design/"+a);_background.uvRect=a=="12cdb"?new Rect(0,0,1,1):new Rect(.0052f/3.0311f,0,1/3.0311f,1);
                var p=DesignUI.Panel(_controls,"World card",0,0,350,260);
                DesignUI.Label(p.transform,DesignUI.WorldName(_world),18,15,314,34,25);
                string desc=_world=="green-valley"?"Vadiye hayat ver":_world=="rainy-coast"?"Kıyıya huzuru geri getir":_world=="frost-peaks"?"Zirvelere dengeyi geri getir":"Gökyüzüne dengeyi geri getir";
                DesignUI.Label(p.transform,desc,18,62,314,22,15,DesignUI.Muted);
                float progress=RestorationManager.GetProgress(WorldCatalog.ForLevel(_level),_completed);
                DesignUI.Label(p.transform,$"DÜNYA KURTULUŞU • {Mathf.Clamp(_completed-WorldCatalog.ForLevel(_level).FirstLevel+1,0,WorldCatalog.ForLevel(_level).LevelCount)}/{WorldCatalog.ForLevel(_level).LevelCount}",18,94,250,20,12,DesignUI.Muted);
                DesignUI.Label(p.transform,$"%{Mathf.RoundToInt(progress*100)}",260,94,72,20,14,DesignUI.C("#ffd464"),TextAnchor.MiddleRight);
                DesignUI.Panel(p.transform,"Progress track",18,121,314,12,"#09273c","#09273c",6,"#497a94",1);
                if(progress>0)DesignUI.Panel(p.transform,"Progress fill",20,123,310*progress,8,"#d5f49a","#75cd67",4,"#ffffff00",0);
                _playButton=DesignUI.Button(p.transform,$"Oyna · Bölüm {_level}",18,148,314,52,PlayOrRefill,"gold");
                DesignUI.Button(p.transform,"Dünyalar",18,214,314,32,()=>_onWorld?.Invoke());
            }
        }
        private void PlayOrRefill()
        {
            _menu.gameObject.SetActive(false);
            if(_availableLives<=0&&_level>5)_onLifeAd?.Invoke();
        }
        public void ShowSettings()
        {
            transform.SetAsLastSibling();
            if(_settings){_settings.gameObject.SetActive(true);return;}
            _settings=DesignUI.Screen(transform,"Settings");
            DesignUI.Panel(_settings,"Dim",0,0,390,1300,"#061725bb","#061725bb",0,"#00000000",0);
            var panel=DesignUI.Panel(_settings,"Card",22,200,346,358);
            panel.rectTransform.anchorMin=panel.rectTransform.anchorMax=panel.rectTransform.pivot=new Vector2(.5f,.5f);panel.rectTransform.anchoredPosition=Vector2.zero;
            _settings.gameObject.AddComponent<DesignModalFit>();_settings.gameObject.AddComponent<PanelPresence>();
            DesignUI.Label(panel.transform,"Ayarlar",24,23,230,34,25);
            DesignUI.SymbolButton(panel.transform,"Close",282,20,40,()=>_settings.gameObject.SetActive(false));
            Toggle(panel.transform,"Ses",88,_sound,_setSound);
            Toggle(panel.transform,"Titreşim",155,_haptics,_setHaptics);
            Toggle(panel.transform,"Azaltılmış Hareket",222,()=>PlayerPrefs.GetInt("Cloudora.ReducedMotion",0)==1,v=>{PlayerPrefs.SetInt("Cloudora.ReducedMotion",v?1:0);PlayerPrefs.Save();});
            DesignUI.Panel(panel.transform,"Divider",24,289,298,1,"#8faab333","#8faab333",0,"#00000000",0);
            if(ReleaseConfiguration.HasSupportEmail)DesignUI.Button(panel.transform,"Destek",24,306,140,32,()=>Application.OpenURL("mailto:"+ReleaseConfiguration.Current.supportEmail));
            if(ReleaseConfiguration.HasPrivacyPolicy)DesignUI.Button(panel.transform,"Gizlilik",180,306,142,32,()=>Application.OpenURL(ReleaseConfiguration.Current.privacyPolicyUrl));
            if(!ReleaseConfiguration.HasSupportEmail&&!ReleaseConfiguration.HasPrivacyPolicy)DesignUI.Label(panel.transform,"CLOUDORA",24,308,298,22,11,DesignUI.Muted,TextAnchor.MiddleCenter);
        }

        private static void Toggle(Transform p,string title,float y,Func<bool> get,Action<bool> set)
        {
            DesignUI.Label(p,title,24,y,220,24,16);
            DesignUI.Label(p,title=="Ses"?"Oyun içi sesler":title=="Titreşim"?"Dokunma geri bildirimi":"Hareketli efektleri sadeleştir",24,y+27,240,18,11,DesignUI.Muted);
            var status=DesignUI.Label(p,"",229,y,40,44,10,DesignUI.Muted,TextAnchor.MiddleRight);
            Button button=null;button=DesignUI.Icon(p,get()?"88c3d":"1603c",276,y+8,48,28,()=>{set(!get());button.GetComponent<RawImage>().texture=Resources.Load<Texture2D>("Design/"+(get()?"88c3d":"1603c"));status.text="";});
            // The visible switch is 28pt high; its row is the full 44pt touch target.
            button.GetComponent<RawImage>().raycastPadding=new Vector4(0,-8,0,-8);
        }
    }
}
