using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Cloudora.UI
{
    public static class UIShapes
    {
        private static readonly Vector2[] Circle=MakeCircle();
        private static Vector2[] MakeCircle(){var p=new Vector2[48];for(int i=0;i<p.Length;i++){float a=i*Mathf.PI*2/p.Length;p[i]=new Vector2(Mathf.Cos(a),Mathf.Sin(a));}return p;}
        public static void Polygon(VertexHelper vh,Vector2 a,Vector2 b,Vector2 c,Color color)
        {
            int n=vh.currentVertCount;vh.AddVert((a+b+c)/3,color,Vector2.zero);vh.AddVert(a,color,Vector2.zero);vh.AddVert(b,color,Vector2.zero);vh.AddVert(c,color,Vector2.zero);vh.AddTriangle(n,n+1,n+2);vh.AddTriangle(n,n+2,n+3);vh.AddTriangle(n,n+3,n+1);
        }
        public static void Polygon(VertexHelper vh,Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color color)
        {
            int n=vh.currentVertCount;vh.AddVert((a+b+c+d)/4,color,Vector2.zero);vh.AddVert(a,color,Vector2.zero);vh.AddVert(b,color,Vector2.zero);vh.AddVert(c,color,Vector2.zero);vh.AddVert(d,color,Vector2.zero);for(int i=0;i<4;i++)vh.AddTriangle(n,n+1+i,n+1+(i+1)%4);
        }
        public static void Polygon(VertexHelper vh,IList<Vector2> points,Color color)
        {
            if(points.Count<3)return;int n=vh.currentVertCount;Vector2 center=Vector2.zero;for(int i=0;i<points.Count;i++)center+=points[i];center/=points.Count;
            vh.AddVert(center,color,Vector2.zero);for(int i=0;i<points.Count;i++)vh.AddVert(points[i],color,Vector2.zero);
            for(int i=0;i<points.Count;i++)vh.AddTriangle(n,n+1+i,n+1+(i+1)%points.Count);
        }
        public static void Line(VertexHelper vh,Vector2 a,Vector2 b,float width,Color color)
        {
            Vector2 axis=new Vector2(-(b-a).y,(b-a).x).normalized,normal=axis*width*.5f;int n=vh.currentVertCount;
            Color fade=color;fade.a=0;float aa=Mathf.Min(.8f,width*.4f);Vector2 outer=axis*(width*.5f+aa);
            vh.AddVert(a+outer,fade,Vector2.zero);vh.AddVert(a+normal,color,Vector2.zero);vh.AddVert(a-normal,color,Vector2.zero);vh.AddVert(a-outer,fade,Vector2.zero);
            vh.AddVert(b+outer,fade,Vector2.zero);vh.AddVert(b+normal,color,Vector2.zero);vh.AddVert(b-normal,color,Vector2.zero);vh.AddVert(b-outer,fade,Vector2.zero);
            for(int i=0;i<3;i++){vh.AddTriangle(n+i,n+4+i,n+1+i);vh.AddTriangle(n+1+i,n+4+i,n+5+i);}
        }

        public static void Ellipse(VertexHelper vh,Vector2 center,float rx,float ry,float stroke,Color color,bool fill=false)
        {
            const int steps=48;
            Vector2 Point(int i)=>center+Vector2.Scale(Circle[i],new Vector2(rx,ry));
            if(fill)
            {
                int start=vh.currentVertCount;Vector2 average=Vector2.zero;
                for(int i=0;i<steps;i++)average+=Point(i);average/=steps;
                vh.AddVert(average,color,Vector2.zero);
                for(int i=0;i<steps;i++)vh.AddVert(Point(i),color,Vector2.zero);
                for(int i=0;i<steps;i++)vh.AddTriangle(start,start+1+i,start+1+(i+1)%steps);
            }
            else for(int i=0;i<steps;i++)Line(vh,Point(i),Point((i+1)%steps),stroke,color);
        }
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class GameIcon : MaskableGraphic
    {
        public string Kind;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var r=rectTransform.rect;float s=Mathf.Min(r.width,r.height)/32f;var c=r.center;
            void L(float a,float b,float x,float y)=>UIShapes.Line(vh,c+new Vector2(a,b)*s,c+new Vector2(x,y)*s,2.3f*s,color);
            if(Kind=="Undo"){
                for(int i=0;i<24;i++){float t=i/24f,u=(i+1)/24f;Vector2 P(float z){float v=1-z;return new Vector2(v*v*v*0+3*v*v*z*14+3*v*z*z*14+z*z*z*1,v*v*v*7+3*v*v*z*7-3*v*z*z*11-z*z*z*11);}UIShapes.Line(vh,c+P(t)*s,c+P(u)*s,2.5f*s,color);}
                L(-10,7,1,7);UIShapes.Polygon(vh,new[]{c+new Vector2(-13,7)*s,c+new Vector2(-5,13)*s,c+new Vector2(-5,1)*s},color);
            }
            else if(Kind=="Shuffle"){L(-12,-8,-5,-8);L(-5,-8,6,8);L(6,8,12,8);L(8,12,12,8);L(12,8,8,4);L(-12,8,-5,8);L(-5,8,-1,2);L(3,-3,6,-8);L(6,-8,12,-8);L(8,-4,12,-8);L(12,-8,8,-12);}
            else if(Kind=="Extra"){L(-9,10,-9,-9);L(-9,-9,9,-9);L(9,-9,9,10);L(-11,10,11,10);L(0,5,0,-5);L(-5,0,5,0);}
            else if(Kind=="Wind"){for(int row=0;row<3;row++){float y=(row-1)*7;L(-12,y,5-row*3,y);for(int i=0;i<12;i++){float a=(-90+i*15)*Mathf.Deg2Rad,b=(-90+(i+1)*15)*Mathf.Deg2Rad;UIShapes.Line(vh,c+new Vector2(5-row*3+Mathf.Cos(a)*4,y+4+Mathf.Sin(a)*4)*s,c+new Vector2(5-row*3+Mathf.Cos(b)*4,y+4+Mathf.Sin(b)*4)*s,2*s,color);}}}
            else if(Kind=="Pause"){L(-5,-9,-5,9);L(5,-9,5,9);}
            else if(Kind=="Close"){L(-7,-7,7,7);L(-7,7,7,-7);}
            else if(Kind=="Back"){L(5,10,-5,0);L(-5,0,5,-10);}
            else if(Kind=="Home"){L(-12,0,0,10);L(0,10,12,0);L(-8,2,-8,-10);L(-8,-10,8,-10);L(8,-10,8,2);}
            else if(Kind=="Play"){UIShapes.Polygon(vh,new[]{c+new Vector2(-6,-10)*s,c+new Vector2(-6,10)*s,c+new Vector2(10,0)*s},color);}
            else if(Kind=="Check"){L(-9,0,-2,-7);L(-2,-7,10,8);}
            else if(Kind=="Gear") {UIShapes.Ellipse(vh,c,8*s,8*s,3*s,color);for(int i=0;i<8;i++){float a=i*Mathf.PI/4;var d=new Vector2(Mathf.Cos(a),Mathf.Sin(a));UIShapes.Line(vh,c+d*9*s,c+d*12*s,4*s,color);}}
        }
    }
    public sealed class ButtonMotion : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
    {
        private float _target=1;private Vector3 _rest;private Button _button;
        private void Awake(){_rest=transform.localScale;_button=GetComponent<Button>();}
        private void OnDisable(){if(_rest!=Vector3.zero)transform.localScale=_rest;_target=1;}
        public void OnPointerDown(PointerEventData e){if(_button&&_button.interactable)_target=.96f;}
        public void OnPointerUp(PointerEventData e)=>_target=1;
        public void OnPointerExit(PointerEventData e)=>_target=1;
        private void Update(){var target=_rest*_target;var current=transform.localScale;if(current==target)return;var next=Vector3.Lerp(current,target,1-Mathf.Exp(-Time.unscaledDeltaTime*24));transform.localScale=(next-target).sqrMagnitude<.000001f?target:next;}
    }
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class PanelPresence : MonoBehaviour
    {
        private CanvasGroup _group;private float _start;private bool _finished;
        private void OnEnable(){_group=GetComponent<CanvasGroup>();if(!_group)_group=gameObject.AddComponent<CanvasGroup>();_start=Time.unscaledTime;_finished=false;_group.alpha=Application.isPlaying?0:1;}
        private void Update(){if(!_group||_finished)return;float t=(Time.unscaledTime-_start)/.18f;_group.alpha=Mathf.SmoothStep(0,1,t);_finished=t>=1;}
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class GlassVesselGraphic : MaskableGraphic
    {
        public bool Selected,Solved;
        public float SelectionGlow,TransferGlow;
        public Color TransferTint=Color.white;
        private readonly List<Vector2> _outline=new(36);
        private static readonly Color SelectionColor=DesignUI.C("#ffe3a0"),SolvedColor=DesignUI.C("#79f0cc"),CheckColor=DesignUI.C("#13bda0");
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();Rect r=rectTransform.rect;if(r.width<=0||r.height<=0)return;float w=r.width,s=w/90f,top=r.yMax-8*s,bottom=r.yMin+7*s,rad=w*.40f;
            var p=_outline;p.Clear();p.Add(new Vector2(-rad,top));p.Add(new Vector2(-rad,bottom+rad));
            for(int i=0;i<=32;i++){float a=(180+i*180f/32)*Mathf.Deg2Rad;p.Add(new Vector2(Mathf.Cos(a)*rad,bottom+rad+Mathf.Sin(a)*rad));}p.Add(new Vector2(rad,top));
            UIShapes.Polygon(vh,p,new Color(.65f,.92f,1,.065f));
            Color rim=Solved?SolvedColor:new Color(.80f,.96f,1,.65f);
            float selected=Mathf.Max(Selected?.3f:0,SelectionGlow);
            rim=Color.Lerp(rim,SelectionColor,selected);
            rim=Color.Lerp(rim,Color.Lerp(TransferTint,Color.white,.65f),TransferGlow);
            float halo=Mathf.Max(selected,TransferGlow);
            for(int i=0;i<p.Count-1;i++){
                if(halo>.001f)UIShapes.Line(vh,p[i],p[i+1],8*s,new Color(rim.r,rim.g,rim.b,.10f*halo));
                UIShapes.Line(vh,p[i],p[i+1],3*s,new Color(rim.r,rim.g,rim.b,.16f+.12f*halo));UIShapes.Line(vh,p[i],p[i+1],(1.65f+.55f*halo)*s,rim);
            }
            UIShapes.Ellipse(vh,new Vector2(0,top),w*.435f,3.1f*s,(1.8f+.5f*halo)*s,Color.Lerp(new Color(.9f,1,1,.9f),rim,halo));
            if(selected>.001f){var tip=new Vector2(0,top+11*s);UIShapes.Polygon(vh,tip+new Vector2(-3,3)*s,tip+new Vector2(3,3)*s,tip-new Vector2(0,2)*s,new Color(rim.r,rim.g,rim.b,selected));}
            UIShapes.Ellipse(vh,new Vector2(0,top),w*.41f,2.0f*s,0,new Color(.02f,.12f,.2f,.12f),true);
            UIShapes.Line(vh,new Vector2(-w*.29f,top-9*s),new Vector2(-w*.29f,bottom+rad*.65f),2.4f*s,new Color(1,1,1,.36f));
            UIShapes.Line(vh,new Vector2(w*.30f,top-10*s),new Vector2(w*.30f,bottom+rad),.7f*s,new Color(1,1,1,.16f));
            if(Solved){Vector2 c=new Vector2(w*.28f,top+9*s);UIShapes.Ellipse(vh,c,6*s,6*s,0,CheckColor,true);UIShapes.Line(vh,c+new Vector2(-3,0)*s,c+new Vector2(-.5f,-2)*s,1.3f*s,Color.white);UIShapes.Line(vh,c+new Vector2(-.5f,-2)*s,c+new Vector2(3,2)*s,1.3f*s,Color.white);}
        }
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class VesselFillMask : MaskableGraphic
    {
        private readonly List<Vector2> _outline=new(40);
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var r=rectTransform.rect;if(r.width<=0||r.height<=0)return;
            float radius=Mathf.Min(r.width*.5f,r.height*.5f);var p=_outline;p.Clear();p.Add(new Vector2(r.xMin,r.yMax));p.Add(new Vector2(r.xMin,r.yMin+radius));
            for(int i=0;i<=36;i++){float a=(180+i*180f/36)*Mathf.Deg2Rad;p.Add(new Vector2(r.center.x+Mathf.Cos(a)*radius,r.yMin+radius+Mathf.Sin(a)*radius));}
            p.Add(new Vector2(r.xMax,r.yMax));UIShapes.Polygon(vh,p,Color.white);
        }
    }

}
