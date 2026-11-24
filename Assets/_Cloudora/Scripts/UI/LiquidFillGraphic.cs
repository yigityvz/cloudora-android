using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // The contents remain stratified by element, but their exposed surface behaves as liquid.
    // Only a transfer updates the wave: idle vessels do not rebuild meshes every frame.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class LiquidFillGraphic : MaskableGraphic
    {
        public Color Top, Bottom;
        public bool SurfaceVisible = true;
        public float Phase, Energy;
        public void SetSurface(bool visible, float phase = 0, float energy = 0)
        {
            if (SurfaceVisible == visible && Mathf.Approximately(Phase, phase) && Mathf.Approximately(Energy, energy)) return;
            SurfaceVisible = visible; Phase = phase; Energy = energy; SetVerticesDirty();
        }
        public float SurfaceHeight(float x)
        {
            var r = rectTransform.rect;
            if (!SurfaceVisible) return r.yMax;
            float unit = r.width / 54f;
            float meniscus = (.8f * Mathf.Pow((x - .5f) * 2, 2) - .4f) * unit;
            float wave = (Mathf.Sin(x * 8 - Phase) * .68f + Mathf.Sin(x * 15 + Phase * .73f) * .32f) * Energy * 2.8f * unit;
            return r.yMax + Mathf.Clamp(meniscus + wave, -r.height * .18f, r.height * .18f);
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); var r = rectTransform.rect;
            if (r.width <= .001f || r.height <= .001f) return;
            const int steps = 28;
            for (int i = 0; i <= steps; i++)
            {
                float u = i / (float)steps, x = Mathf.Lerp(r.xMin, r.xMax, u), y = SurfaceHeight(u);
                // A restrained horizontal sheen gives the fluid volume without block borders.
                float sheen = Mathf.Exp(-Mathf.Pow((u - .24f) / .14f, 2)) * .065f;
                var high = Color.Lerp(Top, Color.white, sheen); var low = Color.Lerp(Bottom, Color.white, sheen * .35f);
                vh.AddVert(new Vector2(x, r.yMin), low, Vector2.zero);
                vh.AddVert(new Vector2(x, y), high, Vector2.zero);
                if (i > 0) { int n = i * 2; vh.AddTriangle(n - 2, n, n - 1); vh.AddTriangle(n - 1, n, n + 1); }
            }
            if (!SurfaceVisible) return;
            float width = Mathf.Min(r.width / 54f * .75f, r.height * .12f);
            var shine = Color.Lerp(Top, Color.white, .55f); shine.a = .5f;
            for (int i = 0; i < steps; i++)
            {
                float u = i / (float)steps, v = (i + 1f) / steps;
                UIShapes.Line(vh, new Vector2(Mathf.Lerp(r.xMin, r.xMax, u), SurfaceHeight(u) - width), new Vector2(Mathf.Lerp(r.xMin, r.xMax, v), SurfaceHeight(v) - width), width, shine);
            }
        }
    }
}
