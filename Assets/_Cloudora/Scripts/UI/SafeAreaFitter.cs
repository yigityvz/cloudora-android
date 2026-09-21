using UnityEngine;

namespace Cloudora.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rect;
        private Rect _lastSafeArea;
        private Vector2Int _lastScreen;

        public static SafeAreaFitter Attach(GameObject target)
        {
            SafeAreaFitter fitter = target.GetComponent<SafeAreaFitter>() ?? target.AddComponent<SafeAreaFitter>();
            fitter.Apply();
            return fitter;
        }

        public static (Vector2 min, Vector2 max) CalculateAnchors(Rect safeArea, Vector2 screenSize)
        {
            if (screenSize.x <= 0f || screenSize.y <= 0f) return (Vector2.zero, Vector2.one);
            Vector2 min = new(safeArea.xMin / screenSize.x, safeArea.yMin / screenSize.y);
            Vector2 max = new(safeArea.xMax / screenSize.x, safeArea.yMax / screenSize.y);
            return (Vector2.Max(Vector2.zero, min), Vector2.Min(Vector2.one, max));
        }

        private void OnEnable() => Apply();

        private void Update()
        {
            Vector2Int screen = new(Screen.width, Screen.height);
            if (_lastSafeArea != Screen.safeArea || _lastScreen != screen) Apply();
        }

        private void Apply()
        {
            _rect ??= GetComponent<RectTransform>();
            _lastSafeArea = Screen.safeArea;
            _lastScreen = new Vector2Int(Screen.width, Screen.height);
            (Vector2 min, Vector2 max) = CalculateAnchors(_lastSafeArea, _lastScreen);
            _rect.anchorMin = min;
            _rect.anchorMax = max;
            _rect.offsetMin = Vector2.zero;
            _rect.offsetMax = Vector2.zero;
            _rect.localScale = Vector3.one;
        }
    }
}
