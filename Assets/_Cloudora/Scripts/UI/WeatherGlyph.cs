using Cloudora.Puzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // One optical weight and a 24-unit grid across the complete weather family.
    // Code-drawn vectors remain sharp on Retina; meshes allocate no temporary arrays.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class WeatherGlyph : MaskableGraphic
    {
        public WeatherType Element;
        private Vector2 _center;
        private float _scale, _stroke;
        private readonly Vector2[] _path=new Vector2[49];
        private static readonly Vector2[] CapCircle=Circle();
        private static Vector2[] Circle(){var p=new Vector2[12];for(int i=0;i<p.Length;i++){float a=i*Mathf.PI*2/p.Length;p[i]=new Vector2(Mathf.Cos(a),Mathf.Sin(a));}return p;}

        public static Color Ink(WeatherType type)=>type switch
        {
            WeatherType.Sun=>new Color32(0x77,0x51,0x20,0xEF),
            WeatherType.Snow=>new Color32(0x20,0x6B,0x82,0xF2),
            _=>new Color32(0xF3,0xFB,0xFF,0xF2)
        };

        public void SetElement(WeatherType type)
        {
            if(Element==type&&color==Ink(type))return;
            Element=type;color=Ink(type);SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var r=rectTransform.rect;float side=Mathf.Min(r.width,r.height);if(side<=0)return;
            _center=r.center;_scale=side/24f;_stroke=1.65f*_scale;
            switch(Element)
            {
                case WeatherType.Sun:
                    for(int i=0;i<=48;i++){float a=i*Mathf.PI/24;_path[i]=new Vector2(Mathf.Cos(a),Mathf.Sin(a))*4.3f;}Stroke(vh,49,true);
                    for(int i=0;i<8;i++){float a=i*Mathf.PI/4;var d=new Vector2(Mathf.Cos(a),Mathf.Sin(a));Line(vh,d*7.1f,d*9.3f);}
                    break;
                case WeatherType.Rain:
                    Curve(vh,new(0,9),new(-1,6),new(-6.2f,.6f),new(-6.2f,-2.8f));
                    Curve(vh,new(-6.2f,-2.8f),new(-6.2f,-10),new(6.2f,-10),new(6.2f,-2.8f));
                    Curve(vh,new(6.2f,-2.8f),new(6.2f,.6f),new(1,6),new(0,9));
                    // A small internal reflection gives the drop depth without a solid blob.
                    Curve(vh,new(-3.4f,-3),new(-3.4f,-4.5f),new(-2.6f,-5.5f),new(-1.5f,-5.7f));
                    break;
                case WeatherType.Snow:
                    for(int i=0;i<6;i++)
                    {
                        float a=i*Mathf.PI/3;var d=new Vector2(Mathf.Cos(a),Mathf.Sin(a));var n=new Vector2(-d.y,d.x);
                        Line(vh,Vector2.zero,d*8.7f);Line(vh,d*4.7f,d*6.6f+n*2.1f);Line(vh,d*4.7f,d*6.6f-n*2.1f);
                    }
                    break;
                case WeatherType.Wind:
                    Line(vh,new(-9,4),new(3.5f,4));Curve(vh,new(3.5f,4),new(8.5f,4),new(8.5f,10),new(4.2f,9));
                    Line(vh,new(-7,0),new(7,0));
                    Line(vh,new(-9,-4),new(.5f,-4));Curve(vh,new(.5f,-4),new(6,-4),new(6,-9),new(2.5f,-9));
                    break;
                case WeatherType.Moon:
                    Curve(vh,new(4.5f,8.4f),new(-9.8f,9.8f),new(-10,-10),new(4.5f,-8.4f));
                    Curve(vh,new(4.5f,-8.4f),new(-3.8f,-5.6f),new(-3.8f,5.6f),new(4.5f,8.4f));
                    // An offset star balances the crescent's optical center.
                    Line(vh,new(7,2.7f),new(7,6.3f));Line(vh,new(5.2f,4.5f),new(8.8f,4.5f));
                    break;
                case WeatherType.Lightning:
                    Line(vh,new(2.3f,9.3f),new(-6.4f,-.9f));Line(vh,new(-6.4f,-.9f),new(-.6f,-.9f));
                    Line(vh,new(-.6f,-.9f),new(-2.3f,-9.3f));Line(vh,new(-2.3f,-9.3f),new(6.4f,1.6f));
                    Line(vh,new(6.4f,1.6f),new(.7f,1.6f));Line(vh,new(.7f,1.6f),new(2.3f,9.3f));
                    break;
                case WeatherType.Rainbow:
                    Arc(vh,8.6f,-3.6f);Arc(vh,5.4f,-3.6f);Arc(vh,2.2f,-3.6f);
                    break;
                default:
                    Line(vh,new(0,8.5f),new(6.5f,0));Line(vh,new(6.5f,0),new(0,-8.5f));Line(vh,new(0,-8.5f),new(-6.5f,0));Line(vh,new(-6.5f,0),new(0,8.5f));
                    break;
            }
        }
        private Vector2 P(Vector2 p)=>_center+p*_scale;
        private void Line(VertexHelper vh,Vector2 a,Vector2 b)
        {
            a=P(a);b=P(b);UIShapes.Line(vh,a,b,_stroke,color);Cap(vh,a);Cap(vh,b);
        }
        private void Cap(VertexHelper vh,Vector2 p)
        {
            int n=vh.currentVertCount;vh.AddVert(p,color,Vector2.zero);
            for(int i=0;i<CapCircle.Length;i++)vh.AddVert(p+CapCircle[i]*(_stroke*.5f),color,Vector2.zero);
            for(int i=0;i<CapCircle.Length;i++)vh.AddTriangle(n,n+1+i,n+1+(i+1)%CapCircle.Length);
        }
        private void Curve(VertexHelper vh,Vector2 a,Vector2 b,Vector2 c,Vector2 d)
        {
            for(int i=0;i<=16;i++){float t=i/16f,u=1-t;_path[i]=u*u*u*a+3*u*u*t*b+3*u*t*t*c+t*t*t*d;}
            Stroke(vh,17,false);
        }
        // A continuous antialiased strip avoids overlapping seams on small curves.
        private void Stroke(VertexHelper vh,int count,bool closed)
        {
            int start=vh.currentVertCount;var fade=color;fade.a=0;float half=_stroke*.5f,aa=_stroke*.4f;
            for(int i=0;i<count;i++)
            {
                int previous=i==0?(closed?count-2:0):i-1,next=i==count-1?(closed?1:count-1):i+1;
                var tangent=_path[next]-_path[previous];var normal=new Vector2(-tangent.y,tangent.x).normalized;var point=P(_path[i]);
                vh.AddVert(point+normal*(half+aa),fade,Vector2.zero);vh.AddVert(point+normal*half,color,Vector2.zero);
                vh.AddVert(point-normal*half,color,Vector2.zero);vh.AddVert(point-normal*(half+aa),fade,Vector2.zero);
            }
            for(int i=0;i<count-1;i++)for(int j=0;j<3;j++){int a=start+i*4+j,b=a+4;vh.AddTriangle(a,b,a+1);vh.AddTriangle(a+1,b,b+1);}
            if(!closed){Cap(vh,P(_path[0]));Cap(vh,P(_path[count-1]));}
        }

        private void Arc(VertexHelper vh,float radius,float y)
        {
            for(int i=0;i<=24;i++){float a=i*Mathf.PI/24;_path[i]=new Vector2(Mathf.Cos(a)*radius,y+Mathf.Sin(a)*radius);}
            Stroke(vh,25,false);
        }
    }
}
