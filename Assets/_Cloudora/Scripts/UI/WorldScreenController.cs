using Cloudora.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class WorldScreenController : MonoBehaviour
    {
        private GameObject _panel;
        private Image _land;
        private Text _title;
        private Text _progress;

        public static WorldScreenController Create(Canvas canvas)
        {
            var host = new GameObject("WorldScreen", typeof(RectTransform));
            host.transform.SetParent(canvas.transform, false);
            SafeAreaFitter.Attach(host);
            var controller = host.AddComponent<WorldScreenController>();
            controller.Build();
            controller._panel.SetActive(false);
            return controller;
        }

        public void Toggle() => _panel.SetActive(!_panel.activeSelf);

        public void Refresh(WorldDefinition world, int completedLevel)
        {
            float value = RestorationManager.GetProgress(world, completedLevel);
            int stage = RestorationManager.GetStage(world, completedLevel);
            _title.text = world.DisplayName;
            _progress.text = $"Restoration  {Mathf.RoundToInt(value * 100f)}%\nStage {stage}/4  •  Next rule: {world.NewRule}";
            _panel.GetComponent<Image>().color = world.SkyColor;
            _land.color = Color.Lerp(new Color(0.55f, 0.58f, 0.62f), world.LandColor, value);
        }

        private void Build()
        {
            _panel = new GameObject("WorldPanel", typeof(RectTransform), typeof(Image), typeof(Button));
            _panel.transform.SetParent(transform, false);
            RectTransform rect = _panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, 0.15f);
            rect.anchorMax = new Vector2(0.92f, 0.85f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            _panel.GetComponent<Button>().onClick.AddListener(Toggle);

            _title = Label(_panel.transform, 54, new Vector2(0.1f, 0.76f), new Vector2(0.9f, 0.92f));
            _progress = Label(_panel.transform, 32, new Vector2(0.1f, 0.08f), new Vector2(0.9f, 0.28f));
            _land = new GameObject("RestoredLand", typeof(RectTransform), typeof(Image)).GetComponent<Image>();
            _land.transform.SetParent(_panel.transform, false);
            RectTransform landRect = _land.rectTransform;
            landRect.anchorMin = new Vector2(0.12f, 0.32f);
            landRect.anchorMax = new Vector2(0.88f, 0.7f);
            landRect.offsetMin = landRect.offsetMax = Vector2.zero;
            _land.raycastTarget = false;
        }

        private static Text Label(Transform parent, int size, Vector2 min, Vector2 max)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            Text text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(0.15f, 0.22f, 0.29f);
            text.raycastTarget = false;
            text.rectTransform.anchorMin = min;
            text.rectTransform.anchorMax = max;
            text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return text;
        }
    }
}
