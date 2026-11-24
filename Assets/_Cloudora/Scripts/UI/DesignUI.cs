using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // Shared Figma tokens; all dimensions are in the 390-point design coordinate system.
    public static class DesignUI
    {
        public static void Remove(GameObject go) { go.SetActive(false); if(Application.isPlaying) UnityEngine.Object.Destroy(go); else UnityEngine.Object.DestroyImmediate(go); }
        public static Color C(string hex) { ColorUtility.TryParseHtmlString(hex, out Color c); return c; }
        public static readonly Color Ink = C("#17454d"), White = C("#f5fcff"), Muted = C("#bad8e8");
        private static readonly Dictionary<string, Sprite> Sprites = new();
        public static Sprite Sprite(string asset)
        {
            if(Sprites.TryGetValue(asset,out var sprite) && sprite) return sprite;
            var texture=Resources.Load<Texture2D>("Design/"+asset);
            if(!texture) return null;
            return Sprites[asset]=UnityEngine.Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(.5f,.5f));
        }
        private static Font _font;
        public static Font Font => _font ? _font : _font = Resources.Load<Font>("Design/Nunito-ExtraBold");
        public static RectTransform Rect(Transform parent, string name, float x, float y, float w, float h)
        {
            var r = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            r.SetParent(parent, false); r.anchorMin = r.anchorMax = new Vector2(0, 1); r.pivot = new Vector2(0, 1);
            r.anchoredPosition = new Vector2(x, -y); r.sizeDelta = new Vector2(w, h); return r;
        }
        public static RectTransform Screen(Transform parent, string name)
        {
            var r = Rect(parent, name, 0, 0, 390, 844);
            r.gameObject.AddComponent<DesignScreenFit>(); return r;
        }
        public static Text Label(Transform p, string value, float x, float y, float w, float h, int size=16, Color? color=null, TextAnchor align=TextAnchor.MiddleLeft)
        {
            var t=Rect(p,"Text",x,y,w,h).gameObject.AddComponent<Text>();
            // The design root is scaled inside the legacy 1080px canvas. Rasterize glyphs
            // at 3x, then size them back down so Retina does not upscale tiny glyphs.
            t.rectTransform.sizeDelta=new Vector2(w*3,h*3);t.rectTransform.localScale=Vector3.one/3;
            t.font=Font; t.text=System.Text.RegularExpressions.Regex.Replace(value??"",@"<size=(\d+)>",m=>"<size="+(int.Parse(m.Groups[1].Value)*3)+">"); t.fontSize=size*3; t.color=color??White; t.alignment=align;
            t.raycastTarget=false; t.horizontalOverflow=HorizontalWrapMode.Wrap; t.verticalOverflow=VerticalWrapMode.Overflow; return t;
        }
        public static RawImage Art(Transform p,string asset,float x,float y,float w,float h,bool cover=false)
        {
            var a=Rect(p,asset,x,y,w,h).gameObject.AddComponent<RawImage>(); a.texture=Resources.Load<Texture2D>("Design/"+asset); a.raycastTarget=false;
            if (cover && a.texture) { float source=(float)a.texture.width/a.texture.height, dest=w/h; a.uvRect=source>dest?new Rect((1-dest/source)/2,0,dest/source,1):new Rect(0,(1-source/dest)/2,1,source/dest); } return a;
        }
        public static DesignPanel Panel(Transform p,string name,float x,float y,float w,float h,string top="#143951",string bottom="#0c2339",float radius=24,string border="#7297ab55",float stroke=1)
        {
            var g=Rect(p,name,x,y,w,h).gameObject.AddComponent<DesignPanel>();
            g.Top=C(top);g.Bottom=C(bottom);g.Border=C(border);g.Radius=radius;g.Stroke=stroke; g.SetAllDirty(); return g;
        }
        public static Button Button(Transform p,string title,float x,float y,float w,float h,Action action,string style="blue")
        {
            bool primary=style=="green"||style=="gold";
            string top=primary?"#29c9b0":style=="disabled"?"#344958":"#24485f";
            string bottom=primary?"#119d99":style=="disabled"?"#243744":"#17344b";
            Panel(p,"Button shadow",x,y+3,w,h,"#061b2c55","#061b2c55",18,"#00000000",0).raycastTarget=false;
            var g=Panel(p,title,x,y,w,h,top,bottom,18,primary?"#a3f2df88":"#93b8c64d",1);
            g.raycastPadding=new Vector4(0,-Mathf.Max(0,(44-h)/2),0,-Mathf.Max(0,(44-h)/2));
            var b=g.gameObject.AddComponent<Button>();b.targetGraphic=g;b.onClick.AddListener(()=>action?.Invoke());
            b.gameObject.AddComponent<ButtonMotion>();
            Label(g.transform,title,12,0,w-24,h,h>=50?18:14,White,TextAnchor.MiddleCenter);
            return b;
        }
        public static Button Icon(Transform p,string asset,float x,float y,float w,float h,Action action)
        {
            var a=Art(p,asset,x,y,w,h);a.raycastTarget=true;var b=a.gameObject.AddComponent<Button>();a.gameObject.AddComponent<ButtonMotion>();b.targetGraphic=a;b.onClick.AddListener(()=>action?.Invoke());return b;
        }
        public static void ResourcesBar(Transform p,out Text coins,out Text lives,out Text status)
        {
            Panel(p,"Coins",18,24,106,34,"#0a2536b3","#0a2536b3",17,"#b6dde244",1);
            Art(p,"acd7e",22,27,28,28);coins=Label(p,"",57,29,64,24,16);
            Panel(p,"Lives",134,24,132,34,"#0a2536b3","#0a2536b3",17,"#b6dde244",1);
            Art(p,"91f4f",142,29,25,25);lives=Label(p,"",174,29,33,24,15);
            status=Label(p,"",211,32,52,18,10,C("#7ae3c5"));
        }
        public static Button SymbolButton(Transform p,string kind,float x,float y,float size,Action action)
        {
            var g=Panel(p,kind+"Button",x,y,size,size,"#285269cc","#15394ecc",14,"#aadce455",1);
            g.raycastPadding=Vector4.one*-Mathf.Max(0,(44-size)/2);
            var icon=Rect(g.transform,"Icon",size*.24f,size*.24f,size*.52f,size*.52f).gameObject.AddComponent<GameIcon>();icon.Kind=kind;icon.color=White;icon.raycastTarget=false;
            var b=g.gameObject.AddComponent<Button>();b.targetGraphic=g;b.onClick.AddListener(()=>action?.Invoke());g.gameObject.AddComponent<ButtonMotion>();return b;
        }
        public static void CoverWorld(RawImage image,string world,int stage,float w,float h)
        {
            if(!image.texture)return;
            bool triptych=world=="green-valley"||world=="rainy-coast"||world=="frost-peaks";
            float span=triptych?1/3.0311f:1;
            float origin=triptych?(.0052f+Mathf.Clamp(stage,0,2)*1.01035f)/3.0311f:0;
            float src=image.texture.width*span/image.texture.height,dest=w/h;
            image.uvRect=src>dest?new Rect(origin+(span-span*dest/src)/2,0,span*dest/src,1):new Rect(origin,(1-src/dest)/2,span,src/dest);
        }

        public static RawImage WorldArt(Transform p,string world,bool restored,float x=0,float y=0,float w=390,float h=650)
        {
            string asset=world=="green-valley"?"2ce2b":world=="rainy-coast"?"232d2":world=="frost-peaks"?"84576":"12cdb";
            var a=Art(p,asset,x,y,w,h,asset=="12cdb");
            CoverWorld(a,asset=="12cdb"?"":world,restored?2:0,w,h);
            return a;
        }
        public static string WorldName(string id)=>id=="green-valley"?"Yeşil Vadi":System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('-',' '));
    }
    public sealed class DesignScreenFit : MonoBehaviour
    {
        public float Height {get;private set;}=844;
        private RectTransform _rect,_parent;private Vector2 _lastSize;private bool _applied;
        private void OnEnable()=>Apply(); private void LateUpdate()=>Apply();
        public void Apply()
        {
            if(!_rect)_rect=(RectTransform)transform;
            if(!_parent||_parent!=transform.parent){_parent=transform.parent as RectTransform;_applied=false;}
            if(!_parent||_parent.rect.width<=0)return;
            var size=_parent.rect.size;if(_applied&&size==_lastSize)return;_applied=true;_lastSize=size;
            float scale=size.x/390f;Height=size.y/scale;
            _rect.localScale=Vector3.one*scale;_rect.sizeDelta=new Vector2(390,Height);
        }
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class DesignPanel : MaskableGraphic
    {
        public Color Top,Bottom,Border; public float Radius=20,Stroke=1;
        private readonly List<Vector2> _points=new(52);
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();Rect r=rectTransform.rect;if(r.width<=0||r.height<=0)return;float rad=Mathf.Min(Radius,Mathf.Min(r.width,r.height)/2);
            var points=_points;points.Clear();
            for(int c=0;c<4;c++){Vector2 center=new Vector2(c==0||c==3?r.xMax-rad:r.xMin+rad,c<2?r.yMax-rad:r.yMin+rad);for(int j=0;j<=12;j++){float a=(c*90+j*7.5f)*Mathf.Deg2Rad;points.Add(center+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*rad);}}
            vh.AddVert(r.center,Color.Lerp(Bottom,Top,.5f),Vector2.zero);
            foreach(var p in points)vh.AddVert(p,Color.Lerp(Bottom,Top,Mathf.InverseLerp(r.yMin,r.yMax,p.y)),Vector2.zero);
            for(int i=0;i<points.Count;i++)vh.AddTriangle(0,(i+1)%points.Count+1,i+1);
            int edge=vh.currentVertCount;
            for(int i=0;i<points.Count;i++){
                var p=points[i];Color col=Stroke>0?Border:Color.Lerp(Bottom,Top,Mathf.InverseLerp(r.yMin,r.yMax,p.y));
                var q=r.center+Vector2.Scale(p-r.center,new Vector2(1+1.5f/r.width,1+1.5f/r.height));
                vh.AddVert(p,col,Vector2.zero);col.a=0;vh.AddVert(q,col,Vector2.zero);
            }
            for(int i=0;i<points.Count;i++){int a=edge+i*2,b=edge+((i+1)%points.Count)*2;vh.AddTriangle(a,a+1,b);vh.AddTriangle(b,a+1,b+1);}
            if(Stroke<=0){ApplyTint(vh);return;}
            int start=vh.currentVertCount;
            for(int i=0;i<points.Count;i++){var p=points[i];var q=r.center+Vector2.Scale(p-r.center,new Vector2(Mathf.Max(0,1-2*Stroke/r.width),Mathf.Max(0,1-2*Stroke/r.height)));vh.AddVert(p,Border,Vector2.zero);vh.AddVert(q,Border,Vector2.zero);}
            for(int i=0;i<points.Count;i++){int a=start+i*2,b=start+((i+1)%points.Count)*2;vh.AddTriangle(a,a+1,b);vh.AddTriangle(b,a+1,b+1);}
            ApplyTint(vh);
        }
        private void ApplyTint(VertexHelper vh){var v=new UIVertex();for(int i=0;i<vh.currentVertCount;i++){vh.PopulateUIVertex(ref v,i);v.color=(Color)v.color*color;vh.SetUIVertex(v,i);}}

    }
    public sealed class EssenceSymbolFit : MonoBehaviour
    {
        private RectTransform _rect,_parent;private Canvas _canvas;
        public static float CalculateSide(Vector2 available,float unitsPerPoint=1)=>Mathf.Max(0,Mathf.Min(Mathf.Min(available.x*.34f,available.y*.50f),18f*unitsPerPoint));
        private void LateUpdate(){if(!_rect)_rect=(RectTransform)transform;if(!_parent)_parent=transform.parent as RectTransform;if(!_parent)return;if(!_canvas)_canvas=GetComponentInParent<Canvas>();float unit=_canvas?((RectTransform)_canvas.transform).rect.width/390f:1;float side=CalculateSide(_parent.rect.size,unit);var size=new Vector2(side,side);if(_rect.sizeDelta!=size)_rect.sizeDelta=size;}
    }

}
