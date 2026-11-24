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

        // Kenarı yumuşatılmış (anti-aliased) daire; Sliced kullanılınca hap şekli verir (rozetler için).
        private static Sprite _pill;
        public const float PillRadius = 31f;
        public static Sprite Pill => _pill ??= CreatePill();

        // "♥" karakteri yerleşik fontta yok (Android'de çizilmiyor); kalp ikon olarak üretilir.
        private static Sprite _heart;
        public static Sprite Heart => _heart ??= CreateHeart();

        private static Sprite CreateHeart()
        {
            const int size = 64;
            const int samples = 4;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "Heart",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                int inside = 0;
                for (int sy = 0; sy < samples; sy++)
                for (int sx = 0; sx < samples; sx++)
                {
                    // Kalp eğrisi: (u²+v²−1)³ − u²v³ ≤ 0, [-1.3, 1.3] aralığına ölçeklenmiş.
                    float u = ((x + (sx + 0.5f) / samples) / size - 0.5f) * 2.6f;
                    float v = ((y + (sy + 0.5f) / samples) / size - 0.45f) * 2.6f;
                    float a = u * u + v * v - 1f;
                    if (a * a * a - u * u * v * v * v <= 0f) inside++;
                }
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, inside / (float)(samples * samples)));
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        private static Sprite CreatePill()
        {
            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "Pill",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };
            const float center = (size - 1) / 2f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float distance = new Vector2(x - center, y - center).magnitude;
                float alpha = Mathf.Clamp01(size / 2f - 0.5f - distance + 0.5f);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect,
                new Vector4(PillRadius, PillRadius, PillRadius, PillRadius));
        }

        public static void AddSkyGradient(Canvas canvas)
        {
            if (canvas.transform.Find("SkyGradient") != null) return;
            var go = new GameObject("SkyGradient", typeof(RectTransform), typeof(CanvasRenderer), typeof(SkyGradientGraphic));
            go.transform.SetParent(canvas.transform, false);
            go.transform.SetAsFirstSibling();
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        // Menü ekranlarının arkası için tam ekran, opak gökyüzü katmanı; alttaki oyuna dokunmayı da engeller.
        public static GameObject CreateBackdrop(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(SkyGradientGraphic));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            return go;
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
            // Bulutlar (beyaz) ile ayırt edilebilsin diye belirgin gökyüzü mavisi.
            Color top = new Color(0.56f, 0.76f, 0.96f);
            Color bottom = new Color(0.80f, 0.85f, 0.98f);
            helper.AddVert(new Vector3(r.xMin, r.yMin), bottom, Vector2.zero);
            helper.AddVert(new Vector3(r.xMin, r.yMax), top, Vector2.up);
            helper.AddVert(new Vector3(r.xMax, r.yMax), top, Vector2.one);
            helper.AddVert(new Vector3(r.xMax, r.yMin), bottom, Vector2.right);
            helper.AddTriangle(0, 1, 2);
            helper.AddTriangle(0, 2, 3);
        }
    }
}
