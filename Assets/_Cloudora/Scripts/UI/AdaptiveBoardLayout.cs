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
            _rect.anchorMin = new Vector2(0.05f, 0.2f);
            _rect.anchorMax = new Vector2(0.95f, 0.82f);
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
            int columns = cloudCount <= 6 ? 3 : cloudCount <= 10 ? 4 : 5;
            int rows = Mathf.CeilToInt(cloudCount / (float)columns);
            float gap = Mathf.Clamp(available.x * 0.018f, 10f, 28f);
            float width = (available.x - gap * (columns - 1)) / columns;
            float preferredAspect = Mathf.Lerp(1.5f, 2.05f, (capacity - 4f) / 3f);
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
