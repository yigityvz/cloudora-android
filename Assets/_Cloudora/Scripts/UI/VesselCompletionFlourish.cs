using Cloudora.Puzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // A small weather spectacle: charge through the vessel, reveal, then a soft afterglow.
    // All geometry is pooled in this single non-interactive canvas; the glass never moves.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class VesselCompletionFlourish : MaskableGraphic
    {
        public const float Duration = 1.95f;
        public static bool Reduced => OpeningSplash.ReduceMotionEnabled() || PlayerPrefs.GetInt("Cloudora.ReducedMotion", 0) == 1;
        public static float ResultDelay => Reduced ? .20f : 1.78f;
        public bool IsPlaying { get; private set; }
        public WeatherType Element { get; private set; }
        public float Age { get; private set; }
        private CloudContainerView _vessel;
        private Canvas _rootCanvas;
        private Vector2 _mouth, _bottom, _crown;
        private float _unit = 1, _started, _halfWidth;
        private bool _reduced;
        private static readonly Vector2[] Circle = MakeCircle();
        private static readonly Vector3[] Craters = { new(-.35f,.3f,.18f), new(.3f,.4f,.12f), new(.22f,-.32f,.22f), new(-.45f,-.28f,.09f), new(.43f,.03f,.08f), new(-.06f,.56f,.08f), new(-.03f,-.6f,.1f) };
        private static readonly Color Gold = new Color32(255, 205, 83, 255), Cream = new Color32(255, 247, 200, 255),
            Ice = new Color32(172, 241, 255, 255), Mint = new Color32(151, 255, 205, 255),
            Violet = new Color32(194, 172, 255, 255), Electric = new Color32(147, 180, 255, 255);
        private static readonly Color[] Spectrum = { new Color32(255, 125, 159, 255), new Color32(255, 184, 106, 255), new Color32(255, 233, 132, 255), new Color32(128, 243, 194, 255), new Color32(139, 218, 255, 255), new Color32(204, 167, 255, 255) };
        private static Vector2[] MakeCircle()
        {
            var result = new Vector2[49];
            for (int i = 0; i < result.Length; i++) result[i] = Dir(i * Mathf.PI / 24);
            return result;
        }
        public void Play(CloudContainerView vessel, WeatherType type)
        {
            _vessel = vessel; Element = type; _rootCanvas = vessel.GetComponentInParent<Canvas>().rootCanvas;
            _reduced = Reduced; Age = 0; _started = Time.unscaledTime; IsPlaying = true;
            gameObject.SetActive(true); RenderAt(0);
        }
        public void StopEffect()
        {
            if (_vessel) _vessel.SetTransferHighlight(Color.white, 0);
            IsPlaying = false;
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
        protected override void OnDisable()
        {
            IsPlaying = false;
            if (_vessel) _vessel.SetTransferHighlight(Color.white, 0);
            base.OnDisable();
        }
        private void Update()
        {
            if (!_vessel || !_vessel.IsComplete) { StopEffect(); return; }
            Age = Time.unscaledTime - _started;
            if (Age >= (_reduced ? .18f : Duration)) { StopEffect(); return; }
            RenderAt(Age);
        }
        public void RenderAt(float seconds)
        {
            if (!_vessel) return;
            Age = seconds;
            var body = _vessel.VisualBody; var r = rectTransform;
            var cr = (RectTransform)_rootCanvas.transform;
            float canvasUnit = cr.rect.width / 390f;
            _unit = Mathf.Max(.001f, Mathf.Min(canvasUnit, body.rect.width / 74f));
            r.offsetMin = new Vector2(-82, -22) * _unit; r.offsetMax = new Vector2(82, 122) * _unit;
            _mouth = (Vector2)r.InverseTransformPoint(_vessel.MouthPosition) / _unit;
            _bottom = (Vector2)r.InverseTransformPoint(body.TransformPoint(new Vector3(body.rect.center.x, body.rect.yMin + body.rect.width * .17f))) / _unit;
            _halfWidth = body.rect.width / _unit * .38f;
            float ceiling = r.InverseTransformPoint(cr.TransformPoint(new Vector3(cr.rect.center.x, cr.rect.yMax - 185 * canvasUnit))).y / _unit;
            _crown = _mouth + Vector2.up * 44;
            _crown.y = Mathf.Min(_crown.y, ceiling - 58);
            // Keep the crown off the HUD and inside the screen, even in a dense board.
            float left = r.InverseTransformPoint(cr.TransformPoint(new Vector3(cr.rect.xMin, 0))).x / _unit;
            float right = r.InverseTransformPoint(cr.TransformPoint(new Vector3(cr.rect.xMax, 0))).x / _unit;
            _crown.x = Mathf.Clamp(_crown.x, left + 63, right - 63);
            float t = Mathf.Clamp01(seconds / (_reduced ? .18f : Duration));
            var tint = Accent(Element);
            _vessel.SetTransferHighlight(tint, _reduced ? Mathf.Sin(t * Mathf.PI) : Envelope(t, .02f, .14f, .7f, 1) * .92f);
            SetVerticesDirty();
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); if (!IsPlaying || _reduced) return;
            float t = Mathf.Clamp01(Age / Duration);
            float life = Envelope(t, 0, .12f, .65f, 1), reveal = Ease((t - .13f) / .29f);
            var accent = Accent(Element);
            // The rising charge makes a completed vessel feel sealed and alive, not poured.
            Charge(vh, t, accent);
            Glow(vh, _crown, new Vector2(61, 55), A(new Color(.015f, .05f, .12f), life * .42f));
            Glow(vh, _crown, new Vector2(52, 48), A(accent, life * .24f));
            switch (Element)
            {
                case WeatherType.Sun: Sunrise(vh, t, life, reveal); break;
                case WeatherType.Rain: Rainfall(vh, t, life, reveal); break;
                case WeatherType.Snow: Crystal(vh, t, life, reveal); break;
                case WeatherType.Wind: Vortex(vh, t, life, reveal); break;
                case WeatherType.Moon: LunarEclipse(vh, t, life, reveal); break;
                case WeatherType.Lightning: Thunder(vh, t, life, reveal); break;
                case WeatherType.Rainbow: Prism(vh, t, life, reveal); break;
                default: Spark(vh, _crown, 18 * reveal, 0, A(accent, life)); break;
            }
            // Tiny ascending embers carry the last light away rather than abruptly disappearing.
            float tail = Envelope(t, .4f, .58f, .78f, 1);
            for (int i = 0; i < 9; i++)
            {
                float p = Mathf.Clamp01((t - .38f - i * .014f) / .6f);
                var point = _crown + new Vector2(Mathf.Sin(i * 2.4f) * (15 + p * 34), p * 28 - 9);
                Spark(vh, point, (1.1f + i % 3 * .5f) * Mathf.Sin(p * Mathf.PI), p * .3f, A(accent, tail * .85f));
            }
        }
        private void Charge(VertexHelper vh, float t, Color tint)
        {
            float p = Ease(t / .27f), a = Envelope(t, 0, .08f, .22f, .43f);
            var c = Vector2.Lerp(_bottom, _mouth, p);
            Glow(vh, c, new Vector2(_halfWidth * 1.45f, 21), A(tint, a * .32f));
            Ellipse(vh, c, _halfWidth * 1.08f, 5, 1.3f, A(Color.white, a * .8f));
            for (int i = 0; i < 10; i++)
            {
                float q = Mathf.Clamp01((t - i * .008f) / .34f);
                var point = Vector2.Lerp(_bottom, _mouth, q) + new Vector2(Mathf.Sin(i * 2.4f) * _halfWidth * .78f, -i % 3 * 5);
                Glow(vh, point, Vector2.one * 3, A(tint, a * .6f));
                Spark(vh, point, .8f, 0, A(Color.white, a));
            }
        }
        private void Sunrise(VertexHelper vh, float t, float life, float reveal)
        {
            var sun = _crown + Vector2.down * (1 - reveal) * 24;
            float a = life * reveal;
            Glow(vh, sun, Vector2.one * 64, A(Gold, a * .42f));
            // Broad light shafts and narrow rotating rays have separate speeds and lengths.
            for (int i = 0; i < 12; i++)
            {
                float angle = i * Mathf.PI / 6 + t * .24f;
                float radius = (i % 2 == 0 ? 57 : 43) * reveal;
                Ray(vh, sun, angle, 16, radius, i % 2 == 0 ? .065f : .035f, A(Cream, a * .28f));
                Line(vh, sun + Dir(angle) * 26, sun + Dir(angle) * (radius - 4), 1.2f, A(Gold, a * .7f));
            }
            Ellipse(vh, sun, 26 + 4 * reveal, 26 + 4 * reveal, .7f, A(Gold, a * .32f));
            Disc(vh, sun, 19.5f * reveal, Cream, A(Gold, a), a);
            Glow(vh, sun + new Vector2(-6, 7), Vector2.one * 12, A(Color.white, a * .65f));
            float clouds = Envelope(t, 0, .1f, .3f, .57f);
            Cloud(vh, _crown + new Vector2(-14 - reveal * 32, -16), .67f, A(new Color(.86f, .95f, 1), clouds * .85f));
            Cloud(vh, _crown + new Vector2(17 + reveal * 30, -20), .54f, A(new Color(1, .92f, .73f), clouds * .85f));
            for (int i = 0; i < 7; i++)
            {
                float q = Ease((t - .27f - i * .014f) / .45f); float ang = i * 2.39996f;
                Spark(vh, sun + Dir(ang) * (26 + q * 25), (1.7f + i % 2) * Mathf.Sin(q * Mathf.PI), ang, A(Cream, life));
            }
        }
        private void Rainfall(VertexHelper vh, float t, float life, float reveal)
        {
            var cloud = _crown + Vector2.up * 13;
            Glow(vh, cloud, new Vector2(43, 28), A(Ice, life * .33f));
            Cloud(vh, cloud, .9f * (.68f + .32f * reveal), A(new Color(.68f, .83f, 1), life));
            Cloud(vh, cloud + new Vector2(-3, 4), .62f * (.68f + .32f * reveal), A(new Color(.94f, .99f, 1), life * .94f));
            float rainfall = Envelope(t, .13f, .26f, .56f, .84f);
            float floor = Mathf.Min(_mouth.y - 2, cloud.y - 29);
            for (int i = 0; i < 16; i++)
            {
                float q = Mathf.Repeat(t * 2.7f + i * .173f, 1);
                var p = new Vector2(cloud.x + Mathf.Sin(i * 5.4f) * 22, Mathf.Lerp(cloud.y - 5, floor, q));
                float a = Mathf.Sin(q * Mathf.PI) * rainfall;
                Line(vh, p + new Vector2(1.2f, 5), p, 1.35f, A(Ice, a * .65f));
                Drop(vh, p, 1.4f + i % 3 * .3f, A(Color.white, a));
            }
            var pool = new Vector2(_mouth.x, floor);
            float splash = Ease((t - .30f) / .37f), splashAlpha = Envelope(t, .28f, .38f, .58f, .92f);
            for (int i = 0; i < 3; i++) Ellipse(vh, pool - Vector2.up * i * 3, (8 + splash * 36) - i * 5, 2 + splash * 6, 1 - i * .15f, A(Ice, splashAlpha * (1 - i * .23f)));
            for (int i = 0; i < 12; i++)
            {
                float q = Mathf.Clamp01((t - .31f - i % 3 * .025f) / .48f), direction = i % 2 == 0 ? -1 : 1;
                var p = pool + new Vector2(direction * (6 + q * (14 + i * 1.4f)), Mathf.Sin(q * Mathf.PI) * (15 + i % 4 * 4) - q * 7);
                Drop(vh, p, (1.3f + i % 3 * .5f) * Mathf.Sin(q * Mathf.PI), A(Color.white, splashAlpha));
            }
        }
        private void Crystal(VertexHelper vh, float t, float life, float reveal)
        {
            float a = life * reveal; var c = _crown; float spin = -.12f + t * .2f;
            Glow(vh, c, Vector2.one * 52, A(Ice, a * .5f));
            for (int arm = 0; arm < 6; arm++)
            {
                float ang = arm * Mathf.PI / 3 + spin; var d = Dir(ang); var n = new Vector2(-d.y, d.x);
                float growth = Ease((t - .1f - arm * .018f) / .26f), len = 29 * growth;
                Beam(vh, c, c + d * len, 1.65f, A(Ice, a));
                for (int branch = 0; branch < 3; branch++)
                {
                    float at = 9 + branch * 6, branchGrowth = Mathf.Clamp01((len - at) / 7);
                    var fork = c + d * at;
                    Beam(vh, fork, fork + (d * 6 + n * (7 - branch)) * branchGrowth, 1.05f, A(Color.white, a));
                    Beam(vh, fork, fork + (d * 6 - n * (7 - branch)) * branchGrowth, 1.05f, A(Ice, a));
                }
                Diamond(vh, c + d * (len + 5), 2.8f * growth, ang, A(Color.white, a));
            }
            for (int i = 0; i < 16; i++)
            {
                float p = Ease((t - .3f - i % 3 * .025f) / .58f), ang = i * 2.39996f + t * .22f;
                var point = c + Dir(ang) * (16 + p * 44) - Vector2.up * p * p * 15;
                float size = (1.4f + i % 3 * .8f) * Mathf.Sin(p * Mathf.PI);
                Spark(vh, point, size, ang, A(Ice, life));
            }
            float ring = Ease((t - .25f) / .5f);
            Arc(vh, c, 24 + ring * 23, 24 + ring * 23, .2f, Mathf.PI * 2, -.2f, .7f, A(Ice, life * .35f));
            // A few facets form down the glass, like frost sealing the finished weather.
            for (int i = 0; i < 6; i++)
            {
                float q = (i + .5f) / 6;
                var point = Vector2.Lerp(_mouth, _bottom, q) + Vector2.right * (i % 2 == 0 ? -1 : 1) * _halfWidth * .75f;
                Spark(vh, point, 2 + Mathf.Sin(t * 4 + i) * .7f, spin, A(Ice, Envelope(t, .25f, .42f, .64f, .95f) * .65f));
            }
        }
        private void Vortex(VertexHelper vh, float t, float life, float reveal)
        {
            // Three luminous ribbons spiral from the bottom into a widening crown.
            for (int strand = 0; strand < 3; strand++)
            {
                Vector2 previous = Vector2.zero;
                for (int i = 0; i <= 52; i++)
                {
                    float q = i / 52f, angle = q * Mathf.PI * 4.5f - t * 9 + strand * 2.094f;
                    float radius = Mathf.Lerp(_halfWidth * .55f, 40, q) * (.65f + reveal * .35f);
                    var p = Vector2.Lerp(_bottom, _crown + Vector2.up * 19, q) + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * 6);
                    float a = life * Mathf.Sin(q * Mathf.PI) * (.38f + .62f * Mathf.Clamp01(Mathf.Sin(angle)));
                    if (i > 0) Beam(vh, previous, p, .85f + q * 1.15f, A(strand == 1 ? Color.white : Mint, a));
                    previous = p;
                }
            }
            Glow(vh, _crown, new Vector2(38, 30), A(Mint, life * .24f));
            for (int i = 0; i < 10; i++)
            {
                float q = Mathf.Repeat(t * .82f + i * .137f, 1), angle = q * Mathf.PI * 4 - t * 5 + i;
                var p = Vector2.Lerp(_bottom, _crown + Vector2.up * 35, q) + new Vector2(Mathf.Cos(angle) * (18 + q * 24), Mathf.Sin(angle) * 7);
                Leaf(vh, p, 3.8f + i % 2, angle + .8f, A(i % 2 == 0 ? Mint : Cream, life * Mathf.Sin(q * Mathf.PI)));
            }
        }
        private void LunarEclipse(VertexHelper vh, float t, float life, float reveal)
        {
            // An eclipse opens into a textured full moon. Its reflection pours starlight
            // down the glass; there is no planetary orbit or reused transfer emblem.
            float a=life*reveal, radius=23*reveal;
            var moon=_crown+Vector2.down*(1-reveal)*14;
            float opening=Ease((t-.22f)/.40f), shadowX=Mathf.Lerp(-2,53,opening);
            Glow(vh,moon,Vector2.one*58,A(Violet,a*.42f));
            float silver=Envelope(t,.34f,.53f,.72f,1);
            var foot=_bottom+Vector2.up*8;
            Quad(vh,moon+new Vector2(-7,-15),moon+new Vector2(7,-15),foot+Vector2.right*_halfWidth*.8f,foot-Vector2.right*_halfWidth*.8f,A(new Color(.84f,.9f,1),silver*.19f),A(Violet,0));
            Glow(vh,Vector2.Lerp(_mouth,_bottom,.45f),new Vector2(_halfWidth*1.6f,(_mouth.y-_bottom.y)*.58f),A(Violet,silver*.15f));
            LunarDisc(vh,moon,radius,shadowX,a);
            for(int i=0;i<Craters.Length;i++)
            {
                var crater=Craters[i];var offset=new Vector2(crater.x,crater.y)*radius;
                float lit=Mathf.SmoothStep(0,1,(Vector2.Distance(offset,new Vector2(shadowX,3))-radius*1.07f+3)/6);
                var point=moon+offset;
                Disc(vh,point,crater.z*radius,new Color(.62f,.69f,.83f),A(new Color(.74f,.8f,.91f),a*lit*.65f),a*lit*.65f);
                Arc(vh,point,crater.z*radius,crater.z*radius,.5f,2.2f,0,.55f,A(Color.white,a*lit*.5f));
            }
            // A silver terminator catches light as the shadow recedes.
            float edge=Envelope(t,.21f,.32f,.47f,.65f);
            Arc(vh,moon,radius+1,radius+1,-1.2f,2.4f,0,.8f,A(new Color(.91f,.94f,1),edge*.8f));
            for(int i=0;i<15;i++)
            {
                float q=Mathf.Repeat(t*.85f+i*.173f,1);
                var point=Vector2.Lerp(_mouth,_bottom,q)+new Vector2(Mathf.Sin(i*2.4f+q*3)*_halfWidth*.75f,0);
                float shine=Mathf.Sin(q*Mathf.PI)*silver;
                Spark(vh,point,(.75f+i%3*.45f)*Mathf.Sin(q*Mathf.PI),0,A(i%3==0?Cream:Ice,shine*.8f));
            }
            // A constellation writes itself into the sky, then drifts away with the glow.
            for(int i=0;i<7;i++)
            {
                float angle=i*2.39996f+.6f;var point=moon+Dir(angle)*(31+i%3*9);
                float write=Ease((t-.34f-i*.025f)/.18f), twinkle=.65f+.35f*Mathf.Sin(t*10+i*1.7f);
                Spark(vh,point,(1.1f+i%3*.65f)*write,0,A(new Color(.93f,.96f,1),life*write*twinkle));
                if(i>0&&i<4){var previous=moon+Dir((i-1)*2.39996f+.6f)*(31+(i-1)%3*9);Line(vh,previous,Vector2.Lerp(previous,point,write),.55f,A(Violet,life*write*.46f));}
            }
            Ellipse(vh,_mouth,Mathf.Min(_halfWidth*1.1f,32),3,.9f,A(new Color(.88f,.91f,1),silver*.6f));
        }
        private void LunarDisc(VertexHelper vh,Vector2 center,float radius,float shadowX,float alpha)
        {
            if(radius<.05f||alpha<.003f)return;
            const int rings=8,steps=48;
            for(int ring=0;ring<=rings;ring++)for(int i=0;i<=steps;i++)
            {
                float r=ring/(float)rings;var p=Circle[i]*radius*r;
                float lit=Mathf.SmoothStep(0,1,(Vector2.Distance(p,new Vector2(shadowX,3))-radius*1.07f+2.5f)/5);
                float sphere=Mathf.Sqrt(Mathf.Max(0,1-r*r));
                var pearl=Color.Lerp(new Color(.66f,.73f,.9f),new Color(.98f,.99f,1),sphere*.83f);
                var color=Color.Lerp(new Color(.08f,.10f,.22f),pearl,lit);color.a=alpha;
                vh.AddVert((center+p)*_unit,color,Vector2.zero);
                if(ring>0&&i>0){int n=vh.currentVertCount-1,b=n-steps-1;vh.AddTriangle(b-1,n-1,b);vh.AddTriangle(b,n-1,n);}
            }
        }
        private void Thunder(VertexHelper vh, float t, float life, float reveal)
        {
            float charge = Envelope(t, 0, .11f, .22f, .31f), strike = Envelope(t, .23f, .25f, .31f, .48f);
            var storm = _crown + Vector2.up * 14;
            Cloud(vh, storm, .92f, A(new Color(.25f, .31f, .57f), life * .92f));
            Glow(vh, storm, new Vector2(37, 19), A(Electric, charge * .45f));
            for (int i = 0; i < 3; i++)
            {
                var a = _mouth + new Vector2((i - 1) * 11, -25 - i * 13);
                var b = storm + new Vector2((i - 1) * 14, -4);
                Bolt(vh, a, b, i, .75f, A(Electric, charge * .9f));
            }
            Glow(vh, _mouth, new Vector2(66, 60), A(Electric, strike * .48f));
            Glow(vh, (_mouth + _bottom) * .5f, new Vector2(_halfWidth * 1.6f, (_mouth.y - _bottom.y) * .65f), A(Electric, strike * .35f));
            Bolt(vh, storm - Vector2.up * 3, _bottom + Vector2.up * 12, 2, 2.6f, A(Color.white, strike));
            Bolt(vh, _crown + new Vector2(-34, 13), _mouth + new Vector2(-13, -6), 4, 1.2f, A(Electric, strike * .9f));
            Bolt(vh, _crown + new Vector2(32, 7), _mouth + new Vector2(12, -13), 7, 1.4f, A(Ice, strike));
            float aftermath = Envelope(t, .29f, .37f, .55f, .95f), expansion = Ease((t - .28f) / .45f);
            for (int i = 0; i < 2; i++) Ellipse(vh, _mouth - Vector2.up * (5 + i * 4), 9 + expansion * (43 - i * 8), 3 + expansion * (7 - i * 2), 1.25f, A(Electric, aftermath * (1 - i * .3f)));
            for (int i = 0; i < 16; i++)
            {
                float q = Mathf.Clamp01((t - .3f - i % 3 * .018f) / .57f), angle = i * 2.39996f;
                var p = _mouth + Dir(angle) * (7 + q * 49) + Vector2.up * (Mathf.Sin(q * Mathf.PI) * 17 - q * q * 22);
                Line(vh, p, p - Dir(angle) * (2 + 3 * (1 - q)), .9f, A(i % 2 == 0 ? Ice : Violet, aftermath * (1 - q)));
                Spark(vh, p, 1.4f * (1 - q), angle, A(Color.white, aftermath));
            }
            // Residual arcs travel down both sides, visibly sealing this particular vessel.
            for (int i = 0; i < 2; i++)
            {
                float side = i == 0 ? -1 : 1;
                Bolt(vh, _mouth + Vector2.right * side * _halfWidth, _bottom + Vector2.right * side * _halfWidth * .7f, 3 + i, .65f, A(Electric, aftermath * .55f));
            }
        }
        private void Prism(VertexHelper vh, float t, float life, float reveal)
        {
            float ascent = Envelope(t, 0, .1f, .33f, .61f);
            for (int band = 0; band < 6; band++)
            {
                Vector2 previous = Vector2.zero;
                for (int i = 0; i <= 28; i++)
                {
                    float q = i / 28f, angle = q * Mathf.PI * 2.5f - t * 7 + band * .14f;
                    var p = Vector2.Lerp(_bottom, _crown, q) + Vector2.right * Mathf.Sin(angle) * (_halfWidth * .7f + q * 4);
                    if (i > 0) Line(vh, previous, p, 1.3f, A(Spectrum[band], ascent * Mathf.Sin(q * Mathf.PI) * .8f));
                    previous = p;
                }
            }
            float arch = Ease((t - .18f) / .35f), a = life * reveal;
            var origin = _crown + Vector2.down * 14;
            for (int band = 0; band < 6; band++)
            {
                float radius = 44 - band * 4;
                Arc(vh, origin, radius, radius * .85f, 0, Mathf.PI * arch, 0, 3.1f, A(Spectrum[band], a));
            }
            Glow(vh, origin + Vector2.up * 17, new Vector2(36, 28), A(Cream, a * .12f));
            Cloud(vh, origin + Vector2.left * 39, .4f, A(Color.white, a * .8f));Cloud(vh, origin + Vector2.right * 39, .4f, A(Color.white, a * .8f));
            for (int i = 0; i < 18; i++)
            {
                float q = Mathf.Clamp01((t - .32f - i % 4 * .024f) / .57f), ang = i * 2.39996f;
                var p = _crown + new Vector2(Mathf.Cos(ang) * (10 + q * 48), Mathf.Sin(ang) * (8 + q * 32) + q * 8);
                Spark(vh, p, (1.5f + i % 3 * .6f) * Mathf.Sin(q * Mathf.PI), t + ang, A(Spectrum[i % 6], life));
            }
            Spark(vh, origin + Vector2.up * 17, 7 * Mathf.Sin(Mathf.Clamp01((t - .27f) / .65f) * Mathf.PI), 0, A(Color.white, a));
        }
        private static Color Accent(WeatherType type) => type switch { WeatherType.Sun => Gold, WeatherType.Rain => Ice, WeatherType.Snow => Ice, WeatherType.Wind => Mint, WeatherType.Moon => Violet, WeatherType.Lightning => Electric, _ => Cream };
        private static Color A(Color c, float a) { c.a = Mathf.Clamp01(a); return c; }
        private static float Ease(float t) { t = Mathf.Clamp01(t); return 1 - Mathf.Pow(1 - t, 3); }
        private static float Envelope(float t, float begin, float inEnd, float outStart, float end) => Mathf.SmoothStep(0, 1, Mathf.InverseLerp(begin, inEnd, t)) * (1 - Mathf.SmoothStep(0, 1, Mathf.InverseLerp(outStart, end, t)));
        private static Vector2 Dir(float a) => new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        private void Line(VertexHelper vh, Vector2 a, Vector2 b, float width, Color c) { if (c.a > .003f && width > .001f) UIShapes.Line(vh, a * _unit, b * _unit, width * _unit, c); }
        private void Beam(VertexHelper vh, Vector2 a, Vector2 b, float width, Color c) { Line(vh, a, b, width * 4, A(c, c.a * .09f)); Line(vh, a, b, width, c); }
        private void Quad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color inside, Color outside)
        {
            if (inside.a < .003f && outside.a < .003f) return;
            int n = vh.currentVertCount; vh.AddVert(a * _unit, inside, Vector2.zero); vh.AddVert(b * _unit, inside, Vector2.zero); vh.AddVert(c * _unit, outside, Vector2.zero); vh.AddVert(d * _unit, outside, Vector2.zero); vh.AddTriangle(n, n + 1, n + 2); vh.AddTriangle(n, n + 2, n + 3);
        }
        // Vertex gradients provide genuinely soft light, without blurred bitmap textures or allocations.
        private void Glow(VertexHelper vh, Vector2 c, Vector2 radius, Color color)
        {
            if (color.a < .003f) return;
            int n = vh.currentVertCount; vh.AddVert(c * _unit, color, Vector2.zero);
            for (int ring = 1; ring <= 3; ring++)
            {
                float q = ring / 3f; var tint = A(color, color.a * (1 - q) * (1 - q));
                for (int i = 0; i < 49; i++) vh.AddVert((c + Vector2.Scale(Circle[i], radius) * q) * _unit, tint, Vector2.zero);
            }
            for (int i = 0; i < 48; i++) vh.AddTriangle(n, n + 1 + i, n + 2 + i);
            for (int ring = 0; ring < 2; ring++) for (int i = 0; i < 48; i++)
            {
                int a = n + 1 + ring * 49 + i, b = a + 49; vh.AddTriangle(a, b, a + 1); vh.AddTriangle(a + 1, b, b + 1);
            }
        }
        private void Disc(VertexHelper vh, Vector2 c, float radius, Color inner, Color outer, float alpha)
        {
            if (radius < .01f || alpha < .003f) return;
            int n = vh.currentVertCount; vh.AddVert(c * _unit, A(inner, alpha), Vector2.zero);
            for (int i = 0; i < 49; i++) vh.AddVert((c + Circle[i] * radius) * _unit, outer, Vector2.zero);
            for (int i = 0; i < 49; i++) vh.AddVert((c + Circle[i] * (radius + .65f)) * _unit, A(outer, 0), Vector2.zero);
            for (int i = 0; i < 48; i++) { vh.AddTriangle(n, n + i + 1, n + i + 2); int a = n + 1 + i, b = a + 49; vh.AddTriangle(a, b, a + 1); vh.AddTriangle(a + 1, b, b + 1); }
        }
        private void Ellipse(VertexHelper vh, Vector2 c, float rx, float ry, float width, Color color) => Arc(vh, c, rx, ry, 0, Mathf.PI * 2, 0, width, color);
        private void Arc(VertexHelper vh, Vector2 c, float rx, float ry, float start, float sweep, float tilt, float width, Color color)
        {
            if (color.a < .003f || sweep < .001f) return;
            var axis = Dir(tilt); Vector2 Point(float a) { var p = new Vector2(Mathf.Cos(a) * rx, Mathf.Sin(a) * ry); return c + new Vector2(p.x * axis.x - p.y * axis.y, p.x * axis.y + p.y * axis.x); }
            var previous = Point(start); int steps = Mathf.Max(8, Mathf.CeilToInt(sweep * 8));
            for (int i = 1; i <= steps; i++) { var p = Point(start + sweep * i / steps); Line(vh, previous, p, width, color); previous = p; }
        }
        private void Ray(VertexHelper vh, Vector2 c, float angle, float from, float to, float spread, Color tint) => Quad(vh, c + Dir(angle - spread) * from, c + Dir(angle + spread) * from, c + Dir(angle + spread) * to, c + Dir(angle - spread) * to, tint, A(tint, 0));
        private void Spark(VertexHelper vh, Vector2 c, float radius, float spin, Color tint)
        {
            if (radius < .05f || tint.a < .003f) return;
            Glow(vh, c, Vector2.one * radius * 3, A(tint, tint.a * .22f));
            Diamond(vh, c, radius, spin, tint); Diamond(vh, c, radius * .42f, spin + Mathf.PI / 4, A(tint, tint.a * .8f));
        }
        private void Diamond(VertexHelper vh, Vector2 c, float radius, float spin, Color tint)
        {
            Vector2 d = Dir(spin), n = new Vector2(-d.y, d.x);
            Quad(vh, c + n * radius, c + d * radius * .32f, c - n * radius, c - d * radius * .32f, tint, A(tint, tint.a * .35f));
        }
        private void Cloud(VertexHelper vh, Vector2 c, float scale, Color tint)
        {
            Disc(vh, c + new Vector2(-17, -2) * scale, 10 * scale, tint, tint, tint.a);
            Disc(vh, c + Vector2.up * 4 * scale, 15 * scale, tint, tint, tint.a);
            Disc(vh, c + new Vector2(17, -2) * scale, 11 * scale, tint, tint, tint.a);
            Disc(vh, c + Vector2.down * 4 * scale, 12 * scale, tint, tint, tint.a);
        }
        private void Drop(VertexHelper vh, Vector2 c, float radius, Color tint)
        {
            if (radius < .05f || tint.a < .003f) return;
            Disc(vh, c, radius, tint, A(tint, tint.a * .7f), tint.a);
            Quad(vh, c + Vector2.up * radius * 2.3f, c + Vector2.right * radius, c - Vector2.up * radius * .5f, c - Vector2.right * radius, tint, A(tint, tint.a * .8f));
        }
        private void Leaf(VertexHelper vh, Vector2 c, float radius, float angle, Color tint)
        {
            Vector2 d = Dir(angle), n = new Vector2(-d.y, d.x);
            Quad(vh, c + d * radius, c + n * radius * .5f, c - d * radius, c - n * radius * .5f, tint, A(tint, tint.a * .55f));
            Line(vh, c - d * radius * .65f, c + d * radius * .65f, .5f, A(Color.white, tint.a * .6f));
        }
        private void Bolt(VertexHelper vh, Vector2 start, Vector2 end, int seed, float width, Color tint)
        {
            var previous = start;
            for (int i = 1; i <= 10; i++)
            {
                float p = i / 10f; var point = Vector2.Lerp(start, end, p);
                if (i < 10) point.x += Mathf.Sin(i * 2.43f + seed * 3.1f) * (5 + width);
                Beam(vh, previous, point, width, A(tint, tint.a * .85f));
                Line(vh, previous, point, width * .36f, A(Color.white, tint.a)); previous = point;
                if (i % 3 == 1) Beam(vh, point, point + new Vector2(Mathf.Sin(seed + i) * 12, -8), width * .5f, A(tint, tint.a * .55f));
            }
        }
    }
}
