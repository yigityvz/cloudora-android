using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public static class CloudoraVisuals
    {
        private static Sprite _roundedPanel;
        private static Sprite _cloudVessel;
        public static Sprite RoundedPanel => _roundedPanel ??= CreateSprite(false);
        public static Sprite CloudVessel => _cloudVessel ??= CreateSprite(true);

        public static void AddSkyGradient(Canvas canvas)
        {
            if (canvas.transform.Find("SkyGradient") != null) return;
            var go = new GameObject("SkyGradient", typeof(RectTransform), typeof(SkyGradientGraphic));
            go.transform.SetParent(canvas.transform, false);
            go.transform.SetAsFirstSibling();
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private static Sprite CreateSprite(bool cloud)
        {
            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = cloud ? "CloudVessel" : "RoundedPanel" };
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                bool inside = Rounded(x, y, 4, 4, 60, cloud ? 55 : 60, 12);
                if (cloud) inside |= Circle(x, y, 18, 53, 12) || Circle(x, y, 32, 57, 16) || Circle(x, y, 47, 53, 12);
                texture.SetPixel(x, y, inside ? Color.white : Color.clear);
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(16, 16, 16, 16));
        }

        private static bool Rounded(int x, int y, int left, int bottom, int right, int top, int radius)
        {
            float cx = Mathf.Clamp(x, left + radius, right - radius);
            float cy = Mathf.Clamp(y, bottom + radius, top - radius);
            return new Vector2(x - cx, y - cy).sqrMagnitude <= radius * radius;
        }

        private static bool Circle(int x, int y, int cx, int cy, int radius)
            => (x - cx) * (x - cx) + (y - cy) * (y - cy) <= radius * radius;
    }

    public sealed class SkyGradientGraphic : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();
            Rect r = rectTransform.rect;
            Color top = new Color(0.92f, 0.97f, 1f);
            Color bottom = new Color(0.96f, 0.94f, 1f);
            helper.AddVert(new Vector3(r.xMin, r.yMin), bottom, Vector2.zero);
            helper.AddVert(new Vector3(r.xMin, r.yMax), top, Vector2.up);
            helper.AddVert(new Vector3(r.xMax, r.yMax), top, Vector2.one);
            helper.AddVert(new Vector3(r.xMax, r.yMin), bottom, Vector2.right);
            helper.AddTriangle(0, 1, 2);
            helper.AddTriangle(0, 2, 3);
        }
    }
}
