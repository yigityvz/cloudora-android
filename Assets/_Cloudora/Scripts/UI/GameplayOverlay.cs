using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // "No Moves" (bölüm başarısız) ekranında gösterilecek seçenekler.
    public readonly struct FailOptions : IEquatable<FailOptions>
    {
        public readonly bool RetryAvailable;  // can var (ya da 1–5'te ücretsiz)
        public readonly bool RetryFree;       // 1–5: can götürmez
        public readonly bool AdAvailable;
        public readonly int CoinCost;
        public readonly bool CoinsAffordable;

        public FailOptions(bool retryAvailable, bool retryFree, bool adAvailable, int coinCost, bool coinsAffordable)
        {
            RetryAvailable = retryAvailable;
            RetryFree = retryFree;
            AdAvailable = adAvailable;
            CoinCost = coinCost;
            CoinsAffordable = coinsAffordable;
        }

        public bool Equals(FailOptions o) => RetryAvailable == o.RetryAvailable && RetryFree == o.RetryFree &&
                                             AdAvailable == o.AdAvailable && CoinCost == o.CoinCost && CoinsAffordable == o.CoinsAffordable;
        public override bool Equals(object obj) => obj is FailOptions other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(RetryAvailable, RetryFree, AdAvailable, CoinCost, CoinsAffordable);
    }

    public sealed class GameplayOverlay : MonoBehaviour
    {
        private Button _undoButton,_extraButton,_shuffleButton,_retryButton,_adButton,_coinsButton;
        private Text _boardHint,_difficultyLabel,_shuffleName;
        private DesignPanel _difficulty;
        private Text _coinsLabel,_livesLabel,_status,_levelLabel,_worldLabel,_tutorialLabel,_modeLabel,_adHintLabel;
        private Text _undoBadge,_extraBadge,_shuffleBadge,_completeReward,_completeTitle,_failDetail,_blockTitle,_blockDetail;
        private GameObject _completePanel,_failPanel,_blockingPanel,_pausePanel;
        private RectTransform _helpers;
        private int _level;private int _shownLives=int.MinValue;private string _shownCountdown;private bool _busyKnown,_busy;private FailOptions _shownFail;private bool _failKnown;private float _layoutHeight=-1;
        private MilestoneScreen _milestone;
        private GameplayBackdrop _backdrop;
        private Button _pauseButton;
        private Text _pauseHomeLabel,_failHomeLabel;
        private CanvasGroup _helperGroup;
        private GameObject _toast;
        private Coroutine _toastRoutine;
        public static GameplayOverlay Create(Canvas canvas,Action onUndo,Action onContinue,Action onWorld,Action onExtraCloud,Action onSafeShuffle,Action onMenu,Action onRetry,Action onContinueWithAd,Action onContinueWithCoins,Action onLifeAd,Action onSettings=null)
        {
            var host=new GameObject("GameplaySafeArea",typeof(RectTransform));host.transform.SetParent(canvas.transform,false);SafeAreaFitter.Attach(host);
            var root=DesignUI.Screen(host.transform,"GameplayOverlay");var o=root.gameObject.AddComponent<GameplayOverlay>();
            o._backdrop=GameplayBackdrop.Create(canvas);
            DesignUI.ResourcesBar(root,out o._coinsLabel,out o._livesLabel,out o._status);
            o._worldLabel=DesignUI.Label(root,"",30,84,330,18,12,DesignUI.Muted,TextAnchor.MiddleCenter);
            o._levelLabel=DesignUI.Label(root,"",30,108,330,34,26,null,TextAnchor.MiddleCenter);
            o._tutorialLabel=DesignUI.Label(root,"",28,155,334,35,12,DesignUI.Muted,TextAnchor.MiddleCenter);
            o._modeLabel=DesignUI.Label(root,"",28,155,334,35,12,DesignUI.C("#ffd464"),TextAnchor.MiddleCenter);
            o._difficulty=DesignUI.Panel(root,"Difficulty",145,146,100,22,"#366c7788","#23475788",11,"#a8d5df44",1);o._difficulty.raycastTarget=false;
            o._difficultyLabel=DesignUI.Label(o._difficulty.transform,"",0,0,100,22,10,null,TextAnchor.MiddleCenter);

            o._boardHint=DesignUI.Label(root,"Aynı hava türlerini bir araya getir.",28,650,334,20,12,DesignUI.Muted,TextAnchor.MiddleCenter);
            o._helpers=DesignUI.Rect(root,"Helpers",20,710,350,92);
            o._helperGroup=o._helpers.gameObject.AddComponent<CanvasGroup>();
            o._undoButton=Helper(o._helpers,"Undo","Geri Al","Undo",0,onUndo,out o._undoBadge);
            o._extraButton=Helper(o._helpers,"Extra","Ek Bulut","Extra",121,onExtraCloud,out o._extraBadge);
            o._shuffleButton=Helper(o._helpers,"Shuffle","Karıştır","Shuffle",242,onSafeShuffle,out o._shuffleBadge);
            o._shuffleName=o._shuffleButton.GetComponentsInChildren<Text>()[0];
            o._pausePanel=o.Modal("PausePanel",376,out var pause);
            DesignUI.SymbolButton(pause,"Pause",24,22,42,()=>o._pausePanel.SetActive(false));
            DesignUI.Label(pause,"Duraklatıldı",82,21,240,30,24);
            DesignUI.Label(pause,"Hazır olduğunda devam edebilirsin.",82,53,240,20,12,DesignUI.Muted);
            DesignUI.Button(pause,"Devam Et",24,98,298,52,()=>o._pausePanel.SetActive(false),"green");
            DesignUI.Button(pause,"Dünyalar",24,164,142,44,()=>{o._pausePanel.SetActive(false);onWorld?.Invoke();});
            DesignUI.Button(pause,"Ayarlar",180,164,142,44,onSettings);
            var home=DesignUI.Button(pause,"Ana Sayfaya Dön · −1 can",24,222,298,48,()=>{o._pausePanel.SetActive(false);onMenu?.Invoke();});home.name="PauseHomeButton";o._pauseHomeLabel=home.GetComponentInChildren<Text>();
            DesignUI.Label(pause,"Bölüm baştan başlar.",24,278,298,20,12,DesignUI.Muted,TextAnchor.MiddleCenter);
            DesignUI.Button(pause,"Can Kazan · +1",24,314,298,40,onLifeAd).name="LifeAdButton";
            o._pauseButton=DesignUI.SymbolButton(root,"Pause",330,21,40,()=>o._pausePanel.SetActive(true));
            o._completePanel=o.Modal("LevelCompletePanel",400,out var complete);
            DesignUI.Art(complete,"a5f3a",145,22,56,56);
            o._completeTitle=DesignUI.Label(complete,"",24,94,298,60,24,null,TextAnchor.MiddleCenter);
            DesignUI.Label(complete,"Gökyüzü biraz daha aydınlandı.",24,157,298,23,14,DesignUI.Muted,TextAnchor.MiddleCenter);
            DesignUI.Art(complete,"4c96c",117,195,32,32);o._completeReward=DesignUI.Label(complete,"",158,192,100,38,27,DesignUI.C("#ffd884"));
            DesignUI.Button(complete,"Devam Et",24,256,298,52,()=>o.ContinueWithMilestone(onContinue,onWorld,onSettings),"green");
            DesignUI.Button(complete,"Ana Sayfa",24,326,142,44,onMenu).name="WinHomeButton";
            DesignUI.Button(complete,"Dünyalar",180,326,142,44,onWorld);
            o._failPanel=o.Modal("FailPanel",452,out var fail);
            DesignUI.Art(fail,"22aa2",149,20,48,48);
            DesignUI.Label(fail,"Bölüm kaybedildi",24,84,298,34,24,DesignUI.C("#ffb3ab"),TextAnchor.MiddleCenter);
            o._failDetail=DesignUI.Label(fail,"",24,130,298,42,14,DesignUI.Muted,TextAnchor.MiddleCenter);
            o._coinsButton=DesignUI.Button(fail,"Güvenli Hamleye Dön",24,192,298,50,onContinueWithCoins,"green");o._coinsButton.name="CoinsButton";
            o._retryButton=DesignUI.Button(fail,"Yeniden Başlat",24,256,298,48,onRetry);o._retryButton.name="RetryButton";
            o._adButton=DesignUI.Button(fail,"Ödülle Devam Et",24,318,298,44,onContinueWithAd);o._adButton.name="AdButton";
            o._failHomeLabel=DesignUI.Button(fail,"Ana Sayfa · −1 can",24,380,298,44,onMenu).GetComponentInChildren<Text>();
            o._blockingPanel=o.Modal("BlockingPanel",264,out var block);
            o._blockingPanel.AddComponent<Button>().onClick.AddListener(o.HideBlock);
            o._blockTitle=DesignUI.Label(block,"",24,30,298,54,22,DesignUI.C("#ffd464"),TextAnchor.MiddleCenter);
            o._blockDetail=DesignUI.Label(block,"",24,102,298,74,15,DesignUI.Muted,TextAnchor.MiddleCenter);
            DesignUI.Button(block,"Anladım",24,194,298,46,()=>o.HideBlock());
            o._pausePanel.SetActive(false);o._completePanel.SetActive(false);o._failPanel.SetActive(false);o._blockingPanel.SetActive(false);
            return o;
        }
        private void ContinueWithMilestone(Action next,Action worlds,Action settings)
        {
            var world=Cloudora.Progression.WorldCatalog.ForLevel(_level);
            if(_level!=Cloudora.Progression.WorldCatalog.IntroLastLevel && _level!=world.LastLevel){next?.Invoke();return;}
            if(_milestone) return;
            _milestone=MilestoneScreen.Create(transform,_level,next,worlds,settings);
            _milestone.SetResources(_coinsLabel.text,_livesLabel.text,_status.text);
        }
        private GameObject Modal(string name,float height,out RectTransform card)
        {
            var root=DesignUI.Rect(transform,name,0,0,390,844);root.anchorMin=Vector2.zero;root.anchorMax=Vector2.one;root.offsetMin=root.offsetMax=Vector2.zero;
            var dim=root.gameObject.AddComponent<Image>();dim.color=new Color(.02f,.1f,.18f,.68f);
            card=DesignUI.Panel(root,"Card",22,0,346,height).rectTransform;card.anchorMin=card.anchorMax=new Vector2(.5f,.5f);card.pivot=new Vector2(.5f,.5f);card.anchoredPosition=Vector2.zero;
            root.gameObject.AddComponent<DesignModalFit>();root.gameObject.AddComponent<PanelPresence>();return root.gameObject;
        }
        private static Button Helper(Transform parent,string name,string label,string asset,float x,Action action,out Text badge)
        {
            var face=DesignUI.Panel(parent,name+"Button",x,0,108,88,"#214b60ee","#123247ee",20,"#c2edf044",1);
            var b=face.gameObject.AddComponent<Button>();b.targetGraphic=face;
            b.onClick.AddListener(()=>{if(Debug.isDebugBuild)Debug.Log($"[Input] button={name} frame={Time.frameCount} t={Time.realtimeSinceStartup.ToString("0.000",System.Globalization.CultureInfo.InvariantCulture)}");action?.Invoke();});
            face.gameObject.AddComponent<ButtonMotion>();
            var icon=DesignUI.Rect(face.transform,"Icon",38,14,32,32).gameObject.AddComponent<GameIcon>();icon.Kind=asset;icon.color=DesignUI.White;icon.raycastTarget=false;
            DesignUI.Label(face.transform,label,6,56,96,22,12,null,TextAnchor.MiddleCenter);
            var bubble=DesignUI.Panel(face.transform,"CountBadge",83,-5,21,21,"#ffe1a0","#f4c566",11,"#fff0ca88",.8f);bubble.raycastTarget=false;
            badge=DesignUI.Label(bubble.transform,"",0,0,21,21,11,DesignUI.Ink,TextAnchor.MiddleCenter);return b;
        }

        private void LateUpdate()
        {
            var fit=GetComponent<DesignScreenFit>();fit.Apply();float h=fit.Height;if(h==_layoutHeight)return;_layoutHeight=h;
            _helpers.anchoredPosition=new Vector2(20,-(h-112));_boardHint.rectTransform.anchoredPosition=new Vector2(28,-(h-150));
        }
        public void SetBusy(bool busy){if(_busyKnown&&_busy==busy)return;_busyKnown=true;_busy=busy;_pauseButton.interactable=!busy;_helperGroup.interactable=!busy;}
        public void SetUndoAvailable(bool available)=>_undoButton.interactable=available;
        public void SetShuffleAvailable(bool available)=>_shuffleButton.interactable=available;
        public void SetExtraAvailable(bool available){var g=_extraButton.GetComponent<CanvasGroup>();if(!g)g=_extraButton.gameObject.AddComponent<CanvasGroup>();g.alpha=available?1:.45f;}
        public void SetLevelInfo(int level,string world){_backdrop.SetWorld(level,world);_level=level;_shuffleName.text=level<16?"16. bölümde":"Karıştır";var band=Cloudora.Level.V3LevelPlan.BandFor(Mathf.Clamp(level,1,100));_difficultyLabel.text=band==Cloudora.Level.DifficultyBand.VeryHard?"ÇOK ZOR":band==Cloudora.Level.DifficultyBand.Hard?"ZOR":"ORTA";UpdateDifficulty();_levelLabel.text=$"Bölüm {level}";_worldLabel.text=level<=15?"":DesignUI.WorldName(world);}
        public void SetTutorialCue(string cue){_tutorialLabel.text=cue??"";UpdateDifficulty();}
        public void SetModeHint(string hint){_modeLabel.text=hint??"";UpdateDifficulty();}
        private void UpdateDifficulty(){_difficulty.gameObject.SetActive(_level>=15&&string.IsNullOrEmpty(_tutorialLabel.text)&&string.IsNullOrEmpty(_modeLabel.text));}
        public void SetLives(int lives,string countdown){if(_shownLives==lives&&_shownCountdown==countdown)return;_shownLives=lives;_shownCountdown=countdown;_livesLabel.text=$"{lives}/5";_status.text=lives>=5?"DOLU":countdown;_pauseHomeLabel.text=lives>0?"Ana Sayfaya Dön · −1 can":"Ana Sayfaya Dön · Bölümü bırak";_failHomeLabel.text=lives>0?"Ana Sayfa · −1 can":"Ana Sayfa · Bölümü bırak";}
        public void SetCoins(int coins){_coinsLabel.text=coins.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));if(_milestone)_milestone.SetResources(_coinsLabel.text,_livesLabel.text,_status.text);}
        public void SetBoosters(int undo,int extra,int shuffle,bool adAvailable){_undoBadge.text=Charge(undo,adAvailable);_extraBadge.text=Charge(extra,adAvailable);_shuffleBadge.text=Charge(shuffle,adAvailable);}
        private static string Charge(int value,bool ad)=>value<=0&&ad?"+":value.ToString();
        public void ShowComplete(bool show,int coinsEarned=0){if(!show && _milestone){DesignUI.Remove(_milestone.gameObject);_milestone=null;}_completePanel.SetActive(show);_completeTitle.text=$"Bölüm {_level} tamamlandı!";_completeReward.text=$"+{coinsEarned}";}
        public void ShowFail(FailOptions o){if(_failKnown&&_shownFail.Equals(o)&&_failPanel.activeSelf)return;_shownFail=o;_failKnown=true;_failPanel.SetActive(true);_retryButton.interactable=o.RetryAvailable;_adButton.interactable=o.AdAvailable;_coinsButton.interactable=o.CoinsAffordable;_retryButton.GetComponentInChildren<Text>().text=o.RetryFree?"Yeniden Başlat • Ücretsiz":"Yeniden Başlat · −1 can";_coinsButton.GetComponentInChildren<Text>().text=$"Devam Et · {o.CoinCost} jeton";_failDetail.text="Bu dizilimden çözüme ulaşılamıyor. Baştan dene veya son güvenli hamlene dön.";}
        public void HideFail()=>_failPanel.SetActive(false);
        public void ShowBlock(string title,string detail){
            if(title.Contains("Yenilendi")){HideBlock();ShowToast(title,detail);return;}
_blockingPanel.transform.SetAsLastSibling();_blockingPanel.SetActive(true);_blockTitle.text=title;_blockDetail.text=detail;}
        private void ShowToast(string title,string detail)
        {
            if(_toastRoutine!=null)StopCoroutine(_toastRoutine);
            if(_toast)DesignUI.Remove(_toast);
            float h=GetComponent<DesignScreenFit>().Height;
            var p=DesignUI.Panel(transform,"Reward toast",28,h-228,334,66,"#173d4ef2","#0c273bf2",18,"#4ad2b566",1);p.raycastTarget=false;_toast=p.gameObject;
            DesignUI.Label(p.transform,title,16,10,302,22,14);DesignUI.Label(p.transform,detail,16,34,302,24,11,DesignUI.Muted);
            _toastRoutine=StartCoroutine(ClearToast());
        }
        private System.Collections.IEnumerator ClearToast(){yield return new WaitForSecondsRealtime(2.5f);if(_toast)DesignUI.Remove(_toast);_toastRoutine=null;}
        public void HideBlock()=>_blockingPanel.SetActive(false);
    }
    public sealed class DesignModalFit : MonoBehaviour
    {
        private void LateUpdate(){var root=(RectTransform)transform;var card=transform.Find("Card") as RectTransform;if(!card)return;float scale=Mathf.Min(1,(root.rect.height-80)/card.sizeDelta.y);card.localScale=Vector3.one*Mathf.Max(.5f,scale);}
    }
}
