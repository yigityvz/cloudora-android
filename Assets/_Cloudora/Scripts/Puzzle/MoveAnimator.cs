using System.Collections;
using Cloudora.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Puzzle
{
    public sealed class MoveAnimator : MonoBehaviour
    {
        public WeatherMotionLibrary MotionLibrary;
        private GameObject _flowHost;
        private WeatherFlowGraphic _flow;
        private bool _libraryLoaded;
        private CloudContainerView _source, _target;

        public IEnumerator Animate(CloudContainerView source, CloudContainerView target, WeatherType type, int count)
        {
            if (!source || !target || source == target || source.IsComplete || count <= 0) yield break;
            var canvas = source.GetComponentInParent<Canvas>();
            if (!canvas) yield break;
            var canvasRect = (RectTransform)canvas.transform;
            float unit = Mathf.Max(.001f, canvasRect.rect.width / 390f);
            if (!MotionLibrary && !_libraryLoaded) { MotionLibrary = Resources.Load<WeatherMotionLibrary>("WeatherMotionLibrary"); _libraryLoaded = true; }
            var profile = MotionLibrary ? MotionLibrary.Resolve(type) : WeatherMotionProfile.Default(type);
            var tint = profile.OverrideTint ? profile.Tint : CloudContainerView.GetColor(type);
            bool reduced = OpeningSplash.ReduceMotionEnabled() || PlayerPrefs.GetInt("Cloudora.ReducedMotion", 0) == 1;
            _source = source; _target = target;
            try
            {
                WeatherFlowGraphic flow = null; RectTransform root = null;
                if (!reduced)
                {
                    if (!_flowHost) { _flowHost = new GameObject("Weather transfer", typeof(RectTransform), typeof(CanvasRenderer), typeof(Canvas)); _flow = _flowHost.AddComponent<WeatherFlowGraphic>(); _flow.raycastTarget = false; }
                    if (_flowHost.transform.parent != canvas.transform) _flowHost.transform.SetParent(canvas.transform, false);
                    root = (RectTransform)_flowHost.transform;
                    root.anchorMin = Vector2.zero; root.anchorMax = Vector2.one; root.offsetMin = root.offsetMax = Vector2.zero;
                    flow = _flow; flow.LaneOffset = 0; _flowHost.SetActive(true);
                    flow.Unit = unit; flow.Tint = tint; flow.Profile = profile; flow.PacketCount = count;
                    flow.SourceMouth = root.InverseTransformPoint(source.MouthPosition);
                    flow.TargetMouth = root.InverseTransformPoint(target.MouthPosition);
                    float vesselWidth = Vector2.Distance(root.InverseTransformPoint(source.VisualBody.TransformPoint(Vector3.zero)), root.InverseTransformPoint(source.VisualBody.TransformPoint(Vector3.right * source.VisualBody.rect.width)));
                    if (Mathf.Abs(flow.SourceMouth.x - flow.TargetMouth.x) < vesselWidth * .65f && Mathf.Abs(flow.SourceMouth.y - flow.TargetMouth.y) > vesselWidth)
                        flow.LaneOffset = (flow.SourceMouth.x > root.rect.center.x ? -1 : 1) * vesselWidth * .85f;
                    float ceiling = canvasRect.rect.yMax - 183 * unit;
                    flow.Apex = Mathf.Max(flow.SourceMouth.y, flow.TargetMouth.y) + Mathf.Min(profile.ArcHeight * unit, Mathf.Max(10 * unit, ceiling - Mathf.Max(flow.SourceMouth.y, flow.TargetMouth.y)));
                }
                float flight = reduced ? .12f : profile.FlightSeconds;
                const float lead=.035f,stagger=.055f,settle=.24f;
                float delay=reduced?0:stagger;
                float total=lead+flight+(count-1)*delay+(reduced?0:settle);
                // The original element-specific packets enter through the mouth. Only the
                // contents' rise/merge is liquid; do not replace the flight with a pipe.
                if(flow)
                {
                    flow.From=root.InverseTransformPoint(source.FluidSurfacePosition(-count*.5f));
                    flow.Landing=root.InverseTransformPoint(target.FluidSurfacePosition(count*.5f));
                }
                for(float elapsed=0;elapsed<total;elapsed+=Mathf.Min(Time.unscaledDeltaTime,.04f))
                {
                    if(!source||!target)yield break;
                    float departed,arrived;
                    TransferVolumes(elapsed-lead,flight,count,delay,reduced,out departed,out arrived);
                    float impactTime=elapsed-lead-flight*.78f;
                    float receivingDuration=flight*.22f+(count-1)*delay;
                    float turbulence=reduced?0:Mathf.Sin(Mathf.Clamp01(impactTime/Mathf.Max(.01f,receivingDuration))*Mathf.PI)*.8f;
                    float sinceEnd=impactTime-receivingDuration;
                    if(!reduced&&sinceEnd>0)turbulence=.3f*Mathf.Exp(-sinceEnd*16);
                    source.SetTransferPreview(type,count,-Mathf.Max(.000001f,departed),elapsed*11,reduced?0:.2f*Mathf.Sin(departed*Mathf.PI));
                    target.SetTransferPreview(type,count,arrived,elapsed*12,turbulence);
                    target.SetTransferHighlight(tint,turbulence*.65f);
                    if(flow){flow.Elapsed=elapsed-lead;flow.FlightSeconds=flight;flow.Stagger=delay;flow.SetVerticesDirty();}
                    yield return null;
                }
            }
            finally { ClearVisuals(); }
        }
        public static void TransferVolumes(float elapsed,float flight,int count,float stagger,bool reduced,out float departed,out float arrived)
        {
            departed=arrived=0;
            for(int i=0;i<count;i++)
            {
                float t=(elapsed-i*stagger)/Mathf.Max(.01f,flight);
                departed+=Mathf.SmoothStep(0,1,reduced?t:t/.23f);
                arrived+=Mathf.SmoothStep(0,1,reduced?t:(t-.78f)/.22f);
            }
            departed/=Mathf.Max(1,count);arrived/=Mathf.Max(1,count);
        }
        private void OnDisable() => ClearVisuals();
        private void OnDestroy() { if (_flowHost) DesignUI.Remove(_flowHost); }
        private void ClearVisuals()
        {
            if (_source) _source.ClearTransferPreview();
            if (_target) { _target.ClearTransferPreview(); _target.SetTransferHighlight(Color.white, 0); }
            if (_flowHost) _flowHost.SetActive(false);
            _source = null; _target = null;
        }
    }

    // Original per-element top-entry motion: symbol, luminous trail, small orbiting motes.
    // The target vessel's separate LiquidFillGraphic provides the fluid rise and merge.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class WeatherFlowGraphic : MaskableGraphic
    {
        public Vector2 From, Landing, SourceMouth, TargetMouth;
        public float Unit=1,Apex,LaneOffset,Elapsed,FlightSeconds=.64f,Stagger=.055f;
        public int PacketCount=1;
        public Color Tint;
        public WeatherMotionProfile Profile;
        private readonly Vector2[] _trailFrom=new Vector2[15],_trailTo=new Vector2[15],_trailNormals=new Vector2[15];
        private static readonly Vector2[] StarPoints={Vector2.up,new(.22f,.22f),Vector2.right,new(.22f,-.22f),Vector2.down,new(-.22f,-.22f),Vector2.left,new(-.22f,.22f)};
        private static readonly Vector2[] BoltPoints={new(-.05f,1.1f),new(-.65f,-.1f),new(-.08f,-.1f),new(-.3f,-1.05f),new(.65f,.28f),new(.12f,.28f)};
        private static readonly Vector2[] EssencePoints={Vector2.up,Vector2.right*.72f,Vector2.down,Vector2.left*.72f};
        private static readonly Color[] Spectrum={new Color32(255,106,125,255),new Color32(255,184,92,255),new Color32(255,226,114,255),new Color32(113,232,173,255),new Color32(117,212,255,255),new Color32(183,145,255,255)};
        public Vector2 Point(float t)
        {
            t=Mathf.Clamp01(t);var exit=SourceMouth+Vector2.up*8*Unit;var entry=TargetMouth+Vector2.up*8*Unit;
            if(t<.20f)return Vector2.Lerp(From,exit,t/.20f);
            if(t>.80f)return Vector2.Lerp(entry,Landing,(t-.80f)/.20f);
            float u=(t-.20f)/.60f,v=1-u;
            return v*v*v*exit+3*v*v*u*new Vector2(exit.x+LaneOffset,Apex)+3*v*u*u*new Vector2(entry.x+LaneOffset,Apex)+u*u*u*entry;
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            for(int packet=0;packet<PacketCount;packet++)
            {
                float raw=(Elapsed-packet*Stagger)/Mathf.Max(.01f,FlightSeconds);if(raw<=0||raw>=1)continue;
                float t=Mathf.SmoothStep(0,1,raw),alpha=Mathf.Min(1,Mathf.Min(raw*12,(1-raw)*10));
                float radius=Profile.SymbolRadius*Unit*(.78f+.22f*Mathf.Sin(raw*Mathf.PI));
                Trail(vh,t,radius,alpha);var p=Point(t);
                p.x+=Mathf.Sin(t*Mathf.PI)*Mathf.Sin(t*Mathf.PI*5+packet*1.7f)*Profile.Drift*3*Unit;
                var bright=Color.Lerp(Tint,Color.white,Profile.Style==WeatherMotionStyle.Lightning?.75f:.4f);bright.a=alpha;
                Disc(vh,p,radius*1.65f,new Color(Tint.r,Tint.g,Tint.b,.09f*alpha));
                Symbol(vh,p,radius,Elapsed*1.7f+packet,bright);
                for(int j=0;j<4;j++)
                {
                    float age=.025f+j*.031f;if(t<age)continue;
                    var mote=Point(t-age)+new Vector2(Mathf.Sin(j*2.4f+t*8),Mathf.Cos(j*1.9f+t*7))*(3+j)*Unit;
                    var c=bright;c.a*=.55f*(1-j/5f);Symbol(vh,mote,radius*(.20f+.025f*(j%2)),-Elapsed+j,c);
                }
                if(raw>.82f){float impact=(raw-.82f)/.18f;var c=bright;c.a*=Mathf.Sin(impact*Mathf.PI)*.65f;Ring(vh,Landing,radius*(1+impact),radius*.23f,.7f*Unit,c);}
            }
        }
        private void Trail(VertexHelper vh,float t,float radius,float alpha)
        {
            bool rainbow=Profile.Style==WeatherMotionStyle.Rainbow;int bands=rainbow?Spectrum.Length:1;float length=rainbow?.34f:.19f;
            for(int j=0;j<15;j++)
            {
                float a=Mathf.Max(0,t-length+j*length/15),b=Mathf.Max(0,t-length+(j+1)*length/15);
                _trailFrom[j]=Point(a);_trailTo[j]=Point(b);var d=_trailTo[j]-_trailFrom[j];_trailNormals[j]=new Vector2(-d.y,d.x).normalized;
            }
            for(int band=0;band<bands;band++)for(int j=0;j<15;j++)
            {
                var from=_trailFrom[j];var to=_trailTo[j];float fade=(j+1)/15f;var normal=_trailNormals[j];float offset=rainbow?(band-2.5f)*1.5f*Unit:0;
                var c=rainbow?Spectrum[band]:Color.Lerp(Tint,Color.white,.25f);c.a=fade*alpha*(rainbow?.75f:.32f);
                if(Profile.Style==WeatherMotionStyle.Lightning){from+=normal*Mathf.Sin(j*2.7f)*radius*.24f*fade;to+=normal*Mathf.Sin((j+1)*2.7f)*radius*.24f*fade;}
                UIShapes.Line(vh,from+normal*offset,to+normal*offset,(rainbow?1.7f*Unit:radius*.65f)*fade,c);
            }
        }
        private void Symbol(VertexHelper vh,Vector2 p,float r,float spin,Color c)
        {
            switch(Profile.Style)
            {
                case WeatherMotionStyle.Sun:
                    Disc(vh,p,r*.48f,c);for(int i=0;i<8;i++){var d=Direction(i*Mathf.PI/4+spin*.32f);UIShapes.Line(vh,p+d*r*.68f,p+d*r,r*.13f,c);}break;
                case WeatherMotionStyle.Rain:
                    var cloud=Color.Lerp(c,new Color(.75f,.84f,.98f,c.a),.6f);Disc(vh,p+new Vector2(-.45f,.18f)*r,r*.48f,cloud);Disc(vh,p+new Vector2(.02f,.45f)*r,r*.61f,cloud);Disc(vh,p+new Vector2(.54f,.14f)*r,r*.44f,cloud);
                    for(int i=0;i<3;i++)Drop(vh,p+new Vector2((i-1)*.56f,-.65f-.16f*Mathf.Sin(spin*2+i))*r,r*.23f,c);break;
                case WeatherMotionStyle.Snow:
                    for(int i=0;i<6;i++){var d=Direction(i*Mathf.PI/3+spin*.45f);var n=new Vector2(-d.y,d.x);UIShapes.Line(vh,p,p+d*r,r*.11f,c);UIShapes.Line(vh,p+d*r*.62f,p+d*r*.38f+n*r*.25f,r*.09f,c);UIShapes.Line(vh,p+d*r*.62f,p+d*r*.38f-n*r*.25f,r*.09f,c);}break;
                case WeatherMotionStyle.Wind:
                    for(int row=0;row<3;row++)
                    {
                        var center=p+new Vector2(r*.15f,(row-1)*r*.5f);UIShapes.Line(vh,center-Vector2.right*r,center,r*.12f,c);
                        for(int j=0;j<10;j++){float a=-Mathf.PI/2+j*Mathf.PI/10,b=a+Mathf.PI/10;UIShapes.Line(vh,center+new Vector2(0,r*.23f)+Direction(a)*r*.23f,center+new Vector2(0,r*.23f)+Direction(b)*r*.23f,r*.11f,c);}
                    }break;
                case WeatherMotionStyle.Moon:
                    for(int i=0;i<24;i++)
                    {
                        float a=(60+i*240f/24)*Mathf.Deg2Rad,b=(60+(i+1)*240f/24)*Mathf.Deg2Rad,u=i/24f,v=(i+1)/24f;
                        Quad(vh,p+Direction(a)*r,p+Direction(b)*r,p+new Vector2(.5f-.83f*Mathf.Sin(v*Mathf.PI),Mathf.Lerp(.866f,-.866f,v))*r,p+new Vector2(.5f-.83f*Mathf.Sin(u*Mathf.PI),Mathf.Lerp(.866f,-.866f,u))*r,c);
                    }Star(vh,p+new Vector2(r*1.1f,r*.65f),r*.25f,c);break;
                case WeatherMotionStyle.Lightning:Fan(vh,p,r,c,BoltPoints);break;
                case WeatherMotionStyle.Rainbow:Star(vh,p,r*.75f,new Color(1,1,1,c.a));break;
                default:Fan(vh,p,r,c,EssencePoints);break;
            }
        }
        private static Vector2 Direction(float a)=>new Vector2(Mathf.Cos(a),Mathf.Sin(a));
        private static void Disc(VertexHelper vh,Vector2 p,float radius,Color c)
        {
            const int steps=20;int n=vh.currentVertCount;vh.AddVert(p,c,Vector2.zero);for(int i=0;i<=steps;i++)vh.AddVert(p+Direction(i*Mathf.PI*2/steps)*radius,c,Vector2.zero);for(int i=0;i<steps;i++)vh.AddTriangle(n,n+i+1,n+i+2);
        }
        private static void Ring(VertexHelper vh,Vector2 p,float rx,float ry,float width,Color c)
        {
            for(int i=0;i<24;i++){var a=Direction(i*Mathf.PI/12);var b=Direction((i+1)*Mathf.PI/12);UIShapes.Line(vh,p+new Vector2(a.x*rx,a.y*ry),p+new Vector2(b.x*rx,b.y*ry),width,c);}
        }
        private static void Star(VertexHelper vh,Vector2 p,float r,Color c)=>Fan(vh,p,r,c,StarPoints);
        private static void Drop(VertexHelper vh,Vector2 p,float r,Color c)
        {
            const int steps=24;int n=vh.currentVertCount;vh.AddVert(p,c,Vector2.zero);
            for(int i=0;i<=steps;i++){float a=i*Mathf.PI*2/steps,x=Mathf.Sin(a)*.72f,y=Mathf.Cos(a);x*=1-Mathf.Max(0,y)*.85f;vh.AddVert(p+new Vector2(x,y)*r,c,Vector2.zero);}
            for(int i=0;i<steps;i++)vh.AddTriangle(n,n+i+1,n+i+2);
        }
        private static void Fan(VertexHelper vh,Vector2 p,float r,Color c,Vector2[] points)
        {
            int n=vh.currentVertCount;vh.AddVert(p,c,Vector2.zero);for(int i=0;i<points.Length;i++)vh.AddVert(p+points[i]*r,c,Vector2.zero);for(int i=0;i<points.Length;i++)vh.AddTriangle(n,n+1+i,n+1+(i+1)%points.Length);
        }
        private static void Quad(VertexHelper vh,Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color tint)
        {
            int n=vh.currentVertCount;vh.AddVert(a,tint,Vector2.zero);vh.AddVert(b,tint,Vector2.zero);vh.AddVert(c,tint,Vector2.zero);vh.AddVert(d,tint,Vector2.zero);vh.AddTriangle(n,n+1,n+2);vh.AddTriangle(n,n+2,n+3);
        }
    }
}
