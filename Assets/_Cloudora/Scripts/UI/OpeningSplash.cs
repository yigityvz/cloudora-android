using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    /// <summary>Figma cloud-curtain intro. Owns only its overlay; never writes game state.</summary>
    public sealed class OpeningSplash : MonoBehaviour
    {
        public const float Duration = 3f;
        private CanvasGroup _content;
        private Image _inputBlocker;
        private RawImage _glow, _sun, _logo, _glint;
        private RectTransform _frontLeft, _frontRight, _backLeft, _backRight;
        private Texture2D _sky;
        private Func<bool> _ready;
        private bool _reducedMotion;
        private float _elapsed, _exitTime = -1f;
        private bool _clockStarted;
        private double _lastFrameTime;
        private readonly System.Collections.Generic.List<Texture2D> _textures = new();

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern int CloudoraReduceMotionEnabled();
#endif
        public static OpeningSplash Create(Func<bool> ready, bool? reducedMotion = null)
        {
            var host = new GameObject("OpeningSplash", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = host.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30000;
            CanvasScaler scaler = host.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(390f, 844f);
            scaler.matchWidthOrHeight = 0f;
            var splash = host.AddComponent<OpeningSplash>();
            splash._ready = ready;
            splash._reducedMotion = reducedMotion ?? (ReduceMotionEnabled() || PlayerPrefs.GetInt("Cloudora.ReducedMotion", 0) == 1);
            try { splash.Build(); splash.RenderAt(0f); }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Destroy(host); // Missing art must never lock the player out of the menu.
            }
            return splash;
        }

        public static bool ReduceMotionEnabled()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return CloudoraReduceMotionEnabled() != 0;
#else
            return false;
#endif
        }

        private void Build()
        {
            // A separate input blocker remains opaque to raycasts throughout the crossfade.
            var blocker = new GameObject("InputBlocker", typeof(RectTransform), typeof(Image));
            blocker.transform.SetParent(transform, false);
            Stretch(blocker.GetComponent<RectTransform>());
            _inputBlocker = blocker.GetComponent<Image>();
            _inputBlocker.color = _reducedMotion ? Html("#8CD6EB") : Color.clear;
            var content = new GameObject("Splash_Content", typeof(RectTransform), typeof(CanvasGroup));
            content.transform.SetParent(transform, false);
            Stretch(content.GetComponent<RectTransform>());
            _content = content.GetComponent<CanvasGroup>();
            _content.blocksRaycasts = false;

            _sky = new Texture2D(1, 256, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, name = "SplashSkyGradient" };
            Color top = Html("#8CD6EB"), middle = Html("#B3E6E9"), bottom = Html("#45AABC");
            for (int y = 0; y < 256; y++)
            {
                float t = 1f - y / 255f;
                _sky.SetPixel(0, y, t <= .42f ? Color.Lerp(top, middle, t / .42f) : Color.Lerp(middle, bottom, (t - .42f) / .58f));
            }
            _sky.Apply(false, true);
            var sky = new GameObject("Background_Sky", typeof(RectTransform), typeof(RawImage));
            sky.transform.SetParent(content.transform, false);
            RectTransform skyRect = sky.GetComponent<RectTransform>();
            Stretch(skyRect);
            skyRect.offsetMin = new Vector2(-60f, -160f);
            skyRect.offsetMax = new Vector2(60f, 160f);
            sky.GetComponent<RawImage>().texture = _sky;
            sky.GetComponent<RawImage>().raycastTarget = false;

            _glow = Layer(content.transform, "Sun_Glow", 480f, 480f, 0f, 53f);
            // Figma exports the 42px disc with 3px export padding on each side.
            _sun = Layer(content.transform, "Sun_Disc", 48f * 52f / 42f, 48f * 52f / 42f, 0f, 31f);
            _backRight = Layer(content.transform, "Cloud_Back_Right", 700f, 1050f, 205f, -3f, true).rectTransform;
            _backLeft = Layer(content.transform, "Cloud_Back_Left", 700f, 1050f, -205f, -3f, true).rectTransform;
            Alpha(_backRight.GetComponent<RawImage>(), .67f);
            Alpha(_backLeft.GetComponent<RawImage>(), .67f);
            _logo = Layer(content.transform, "Logo_CLOUDORA", 288f, 112f, 0f, 22f);
            // Match Figma's object-cover crop without modifying the original logo raster.
            float crop = (288f / _logo.texture.width) * _logo.texture.height;
            _logo.uvRect = new Rect(0f, (1f - 112f / crop) * .5f, 1f, 112f / crop);
            _glint = Layer(content.transform, "Glint", 18f, 18f, 124f, 59f);
            _frontRight = Layer(content.transform, "Cloud_Front_Right", 800f, 1200f, 127f, 0f, true).rectTransform;
            _frontLeft = Layer(content.transform, "Cloud_Front_Left", 800f, 1200f, -138f, 0f, true).rectTransform;
        }

        private RawImage Layer(Transform parent, string name, float width, float height, float x, float y, bool contain = false)
        {
            Texture2D texture = Resources.Load<Texture2D>("Splash/" + name);
            if (texture == null) throw new InvalidOperationException("Missing splash asset: " + name);
            _textures.Add(texture);
            var node = new GameObject(name, typeof(RectTransform), typeof(RawImage));
            node.transform.SetParent(parent, false);
            RawImage image = node.GetComponent<RawImage>();
            image.texture = texture; image.raycastTarget = false;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(.5f, .5f);
            if (contain)
            {
                float scale = Mathf.Min(width / texture.width, height / texture.height);
                width = texture.width * scale; height = texture.height * scale;
            }
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = new Vector2(x, y);
            return image;
        }

        private System.Collections.IEnumerator Start()
        {
            // Let the first splash frame render before starting its visible duration.
            // The first Unity delta can contain the entire cold-start/import cost.
            yield return null;
            _lastFrameTime = Time.realtimeSinceStartupAsDouble;
            _clockStarted = true;
        }

        private void OnApplicationPause(bool paused)
        {
            _lastFrameTime = Time.realtimeSinceStartupAsDouble;
        }

        private void Update()
        {
            if (!_clockStarted) return;
            double now = Time.realtimeSinceStartupAsDouble;
            _elapsed += (float)(now - _lastFrameTime);
            _lastFrameTime = now;
            float earliestExit = _reducedMotion ? 2.75f : 2.55f;
            if (_exitTime < 0f && _elapsed >= earliestExit && (_ready == null || _ready())) _exitTime = _elapsed;
            float fade = _exitTime < 0f ? 0f : Ease((_elapsed - _exitTime) / (Duration - earliestExit), false);
            RenderAt(_elapsed, fade);
            if (fade >= 1f) Destroy(gameObject);
        }

        /// <summary>Deterministic sampling shared by the live timeline and Editor visual QA.</summary>
        public void RenderAt(float seconds, float fade = 0f)
        {
            float opening = _reducedMotion ? 1f : Segment(seconds, .25f, 1.45f, true);
            float reveal = _reducedMotion ? 1f : Segment(seconds, .55f, 1.3f, true);
            float light = _reducedMotion ? 1f : Segment(seconds, .8f, 1.5f, false);
            float hold = !_reducedMotion && seconds > Duration && fade == 0f ? Hold(seconds - Duration) : 0f;
            _frontLeft.anchoredPosition = new Vector2(-138f - 205f * opening - 2f * hold, 0f);
            _frontRight.anchoredPosition = new Vector2(127f + 205f * opening + 2f * hold, 0f);
            _backLeft.anchoredPosition = new Vector2(-205f - 130f * opening - .7f * hold, -3f);
            _backRight.anchoredPosition = new Vector2(205f + 130f * opening + .7f * hold, -3f);
            _logo.rectTransform.anchoredPosition = new Vector2(4.32f * (1f - reveal), 22f - 15.68f * (1f - reveal));
            _logo.rectTransform.localScale = Vector3.one * Mathf.Lerp(.97f, 1f, reveal);
            Alpha(_logo, reveal);
            Alpha(_glow, Mathf.Lerp(.15f, 1f, light));
            Alpha(_sun, Mathf.Lerp(.08f, .45f, light));
            float glint = seconds < 1.55f ? .9f * Segment(seconds, 1.4f, 1.55f, false) : .9f * (1f - Segment(seconds, 1.55f, 1.8f, false));
            Alpha(_glint, _reducedMotion ? 0f : glint);
            _glint.rectTransform.anchoredPosition = new Vector2(118f + 12f * Segment(seconds, 1.4f, 1.8f, true), 59f);
            if (_reducedMotion)
            {
                Color backdrop = Html("#8CD6EB");
                backdrop.a = 1f - Mathf.Clamp01(fade);
                _inputBlocker.color = backdrop;
            }
            _content.alpha = (1f - Mathf.Clamp01(fade)) * (_reducedMotion ? Segment(seconds, 0f, .25f, false) : 1f);
        }

        private static float Hold(float time)
        {
            float phase = Mathf.Repeat(time, 4f);
            return phase <= 2f ? Ease(phase / 2f, false) : 1f - Ease((phase - 2f) / 2f, false);
        }
        private static float Segment(float t, float from, float to, bool easeOut) => Ease((t - from) / (to - from), easeOut);
        // Invert the cubic's x coordinate; evaluating y(t) directly would change Figma's timing.
        private static float Ease(float t, bool easeOut)
        {
            t = Mathf.Clamp01(t);
            if (t == 0f || t == 1f) return t;
            float x1 = easeOut ? .22f : .4f, x2 = easeOut ? .36f : .2f;
            float y1 = easeOut ? 1f : 0f;
            float lo = 0f, hi = 1f;
            for (int i = 0; i < 16; i++)
            {
                float mid = (lo + hi) * .5f;
                if (Cubic(mid, x1, x2) < t) lo = mid; else hi = mid;
            }
            return Cubic((lo + hi) * .5f, y1, 1f);
        }
        private static float Cubic(float t, float p1, float p2) => 3f * (1f - t) * (1f - t) * t * p1 + 3f * (1f - t) * t * t * p2 + t * t * t;
        private static void Alpha(RawImage image, float alpha) => image.color = new Color(1f, 1f, 1f, alpha);
        private static Color Html(string hex) { ColorUtility.TryParseHtmlString(hex, out Color color); return color; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = rect.offsetMax = Vector2.zero; }
        private void OnDestroy()
        {
            if (_sky != null)
            {
                if (Application.isPlaying) Destroy(_sky); else DestroyImmediate(_sky);
            }
            foreach (Texture2D texture in _textures) if (texture != null) Resources.UnloadAsset(texture);
        }
    }
}
