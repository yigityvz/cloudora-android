using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.UI
{
    public sealed class WorldRescueReplay : MonoBehaviour
    {
        private RawImage _base,_next;private string _world;private WorldAtmosphere _air;
        public static void Show(Transform parent,string world)
        {
            if(parent.Find("Restoration replay"))return;
            var r=DesignUI.Rect(parent,"Restoration replay",0,0,390,844);r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
            var c=r.gameObject.AddComponent<WorldRescueReplay>();c._world=world;
            DesignUI.Panel(r,"Backdrop",0,0,390,1300,"#102f4b","#102f4b",0,"#00000000",0);
            c._base=DesignUI.WorldArt(r,world,false);c._next=DesignUI.WorldArt(r,world,false);c._next.color=new Color(1,1,1,0);
            c._air=WorldAtmosphere.Add(r,world);
            DesignUI.SymbolButton(r,"Close",330,22,40,()=>DesignUI.Remove(r.gameObject));
            c.StartCoroutine(c.Play());
        }
        private void LateUpdate(){var size=((RectTransform)transform).rect.size;_base.rectTransform.sizeDelta=_next.rectTransform.sizeDelta=size;}
        private IEnumerator Play()
        {
            bool reduced=OpeningSplash.ReduceMotionEnabled()||PlayerPrefs.GetInt("Cloudora.ReducedMotion",0)==1;
            yield return null;var size=((RectTransform)transform).rect.size;
            DesignUI.CoverWorld(_base,_world,0,size.x,size.y);
            if(!reduced)yield return new WaitForSecondsRealtime(.35f);
            for(int stage=1;stage<=2;stage++)
            {
                DesignUI.CoverWorld(_next,_world,stage,size.x,size.y);
                float duration=reduced?.12f:1.45f,start=Time.unscaledTime;
                while(Time.unscaledTime-start<duration){_next.color=new Color(1,1,1,Mathf.SmoothStep(0,1,(Time.unscaledTime-start)/duration));yield return null;}
                _next.color=Color.white;_base.uvRect=_next.uvRect;_next.color=new Color(1,1,1,0);
                if(stage==2)_air.World="";
                if(!reduced)yield return new WaitForSecondsRealtime(.35f);
            }
            DesignUI.Remove(gameObject);
        }
    }
}
