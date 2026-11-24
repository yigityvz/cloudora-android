using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class AdaptiveBoardLayout : MonoBehaviour
    {
        private GridLayoutGroup _grid;
        private RectTransform _rect;
        private int _cloudCount;
        private int _capacity;
        private Vector2 _lastSize;

        public void Configure(int cloudCount, int capacity)
        {
            _cloudCount = cloudCount;
            _capacity = Mathf.Clamp(capacity, 4, 7);
            _grid = GetComponent<GridLayoutGroup>();
            _rect = (RectTransform)transform;
            _rect.anchorMin = new Vector2(0.05f, 0.25f);
            _rect.anchorMax = new Vector2(0.95f, 0.76f);
            _rect.offsetMin = _rect.offsetMax = Vector2.zero;
            Apply();
        }

        private void Update()
        {
            if (_rect != null && _rect.rect.size != _lastSize) Apply();
        }

        private void Apply()
        {
            if (_grid == null || _rect == null || _cloudCount <= 0) return;
            (Vector2 safeMin, Vector2 safeMax) = SafeAreaFitter.CalculateAnchors(Screen.safeArea, new Vector2(Screen.width, Screen.height));
            var parent=(RectTransform)_rect.parent;
            float safeHeight=parent.rect.height*(safeMax.y-safeMin.y);
            float designScale=parent.rect.width*(safeMax.x-safeMin.x)/390f;
            float lower=Mathf.Max(.25f,174f*designScale/Mathf.Max(1,safeHeight));
            _rect.anchorMin = new Vector2(Mathf.Lerp(safeMin.x, safeMax.x, 0.05f), Mathf.Lerp(safeMin.y, safeMax.y, lower));
            _rect.anchorMax = new Vector2(Mathf.Lerp(safeMin.x, safeMax.x, 0.95f), Mathf.Lerp(safeMin.y, safeMax.y, 0.76f));
            _lastSize = _rect.rect.size;
            LayoutMetrics metrics = Calculate(_lastSize, _cloudCount, _capacity);
            _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _grid.constraintCount = metrics.Columns;
            _grid.cellSize = metrics.CellSize;
            _grid.spacing = metrics.Spacing;
            _grid.childAlignment = TextAnchor.MiddleCenter;
        }

        public static LayoutMetrics Calculate(Vector2 available, int cloudCount, int capacity)
        {
            int columns = cloudCount <= 4 ? Mathf.Max(1,cloudCount) : cloudCount <= 6 ? 3 : cloudCount<=8?4:cloudCount<=10?5:6;
            int rows = Mathf.CeilToInt(cloudCount / (float)columns);
            float gap=available.x/350f*(columns>=5?10:18);
            float width = (available.x - gap * (columns - 1)) / columns;
            float preferredAspect = (capacity * 36f + 43f) / 90f;
            float height = Mathf.Min(width * preferredAspect, (available.y - gap * (rows - 1)) / rows);
            width = Mathf.Min(width, height / preferredAspect);
            return new LayoutMetrics(columns, new Vector2(Mathf.Max(80f, width), Mathf.Max(150f, height)), new Vector2(gap, gap));
        }
    }

    public readonly struct LayoutMetrics
    {
        public int Columns { get; }
        public Vector2 CellSize { get; }
        public Vector2 Spacing { get; }

        public LayoutMetrics(int columns, Vector2 cellSize, Vector2 spacing)
        {
            Columns = columns;
            CellSize = cellSize;
            Spacing = spacing;
        }
    }
}
