using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class WorldAtmosphere : MaskableGraphic
    {
        [SerializeField] private string _world;
        public string World{get=>_world;set{if(_world==value)return;_world=value;SetVerticesDirty();}}
        private float _last,_nextPreferenceCheck;private bool _reduced;
        protected override void OnEnable(){base.OnEnable();ReadPreference();}
        private void ReadPreference(){_reduced=OpeningSplash.ReduceMotionEnabled()||PlayerPrefs.GetInt("Cloudora.ReducedMotion",0)==1;_nextPreferenceCheck=Time.unscaledTime+.5f;}
        public static WorldAtmosphere Add(Transform parent,string world)
        {
            var r=DesignUI.Rect(parent,"World atmosphere",0,0,0,0);r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
            r.gameObject.AddComponent<Canvas>();
            var a=r.gameObject.AddComponent<WorldAtmosphere>();a.World=world;a.raycastTarget=false;return a;
        }
        private void Update(){if(Time.unscaledTime>=_nextPreferenceCheck){bool old=_reduced;ReadPreference();if(old!=_reduced)SetVerticesDirty();}if(_reduced||Time.unscaledTime-_last<.033f)return;_last=Time.unscaledTime;SetVerticesDirty();}
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var r=rectTransform.rect;if(r.width<=0||r.height<=0)return;
            float scale=r.width/390f;float time=Application.isPlaying?Time.unscaledTime:0;
            if(_reduced)time=0;
            for(int i=0;i<(World=="rainy-coast"?30:18);i++)
            {
                float speed=World=="rainy-coast"?.36f:World=="frost-peaks"?.035f:.024f;
                float x=Mathf.Repeat(i*.61803f+Mathf.Sin(time*.22f+i)*.035f,1)*r.width+r.xMin;
                float y=r.yMax-Mathf.Repeat(i*.173f+time*speed,1)*r.height;
                Vector2 c=new Vector2(x,y);float size=(1.4f+(i%4)*.55f)*scale;
                if(World=="rainy-coast")UIShapes.Line(vh,c,c+new Vector2(-3,-14)*scale,.65f*scale,new Color(.70f,.89f,1,.18f));
                else if(World=="frost-peaks")UIShapes.Ellipse(vh,c,size*.6f,size*.6f,0,new Color(.9f,.98f,1,.28f),true);
                else if(World=="green-valley")UIShapes.Polygon(vh,c+new Vector2(-size,0),c+new Vector2(0,size*.45f),c+new Vector2(size,0),c+new Vector2(0,-size*.45f),new Color(.72f,.92f,.52f,.20f));
                else UIShapes.Ellipse(vh,c,size*.7f,size*.7f,0,new Color(.85f,.98f,1,.13f),true);
            }
        }
    }
    public sealed class GameplayBackdrop : MonoBehaviour
    {
        private RawImage _art;private WorldAtmosphere _atmosphere;private string _world="";private int _stage;private Vector2 _lastSize=new(-1,-1);
        public static GameplayBackdrop Create(Canvas canvas)
        {
            var r=DesignUI.Rect(canvas.transform,"Gameplay scenery",0,0,0,0);r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;r.SetAsFirstSibling();
            var c=r.gameObject.AddComponent<GameplayBackdrop>();c._art=DesignUI.Art(r,"12cdb",0,0,0,0);c._art.rectTransform.anchorMin=Vector2.zero;c._art.rectTransform.anchorMax=Vector2.one;c._art.rectTransform.offsetMin=c._art.rectTransform.offsetMax=Vector2.zero;
            var tone=DesignUI.Panel(r,"Readability shade",0,0,0,0,"#07172f35","#081a2ac4",0,"#00000000",0);tone.rectTransform.anchorMin=Vector2.zero;tone.rectTransform.anchorMax=Vector2.one;tone.rectTransform.offsetMin=tone.rectTransform.offsetMax=Vector2.zero;tone.raycastTarget=false;
            c._atmosphere=WorldAtmosphere.Add(r,"");return c;
        }
        public void SetWorld(int level,string world)
        {
            _world=level<15?"":world;_lastSize=new Vector2(-1,-1);
            string asset=_world=="green-valley"?"2ce2b":_world=="rainy-coast"?"232d2":_world=="frost-peaks"?"84576":"12cdb";
            _art.texture=Resources.Load<Texture2D>("Design/"+asset);_atmosphere.World=_world;
            float progress=Cloudora.Progression.RestorationManager.GetProgress(Cloudora.Progression.WorldCatalog.ForLevel(level),level-1);_stage=progress>=1?2:progress>=.5f?1:0;
        }
        private void LateUpdate(){var r=((RectTransform)transform).rect;if(_lastSize==r.size)return;_lastSize=r.size;DesignUI.CoverWorld(_art,_world,_stage,r.width,r.height);}
    }
}
