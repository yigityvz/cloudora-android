using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        private GameObject _menu;
        private GameObject _settings;
        private Text _progressLabel;

        public void Show() => _menu.SetActive(true);
        public void SetProgress(string world, int level) => _progressLabel.text = $"{world.Replace('-', ' ')}  •  Level {level}".ToUpperInvariant();

        public static MainMenuController Create(Canvas canvas, string world, int level, Action onWorld, Func<bool> sound, Action<bool> setSound, Func<bool> haptics, Action<bool> setHaptics)
        {
            CloudoraVisuals.AddSkyGradient(canvas);
            var host = new GameObject("MainMenuController", typeof(RectTransform));
            host.transform.SetParent(canvas.transform, false);
            var controller = host.AddComponent<MainMenuController>();
            controller.Build(world, level, onWorld, sound, setSound, haptics, setHaptics);
            return controller;
        }

        private void Build(string world, int level, Action onWorld, Func<bool> sound, Action<bool> setSound, Func<bool> haptics, Action<bool> setHaptics)
        {
            _menu = Panel(transform, "MainMenu", new Color(0.91f, 0.97f, 1f, 0.98f));
            Label(_menu.transform, "CLOUDORA", 76, new Vector2(0.1f, 0.76f), new Vector2(0.9f, 0.9f));
            Label(_menu.transform, "Fix the sky. Restore the world.", 30, new Vector2(0.1f, 0.68f), new Vector2(0.9f, 0.77f));
            _progressLabel = Label(_menu.transform, string.Empty, 34, new Vector2(0.12f, 0.5f), new Vector2(0.88f, 0.62f));
            SetProgress(world, level);
            Button(_menu.transform, "CONTINUE", new Vector2(0f, -80f), () => _menu.SetActive(false));
            Button(_menu.transform, "WORLD", new Vector2(0f, -190f), () => { _menu.SetActive(false); onWorld?.Invoke(); });
            Button(_menu.transform, "SETTINGS", new Vector2(0f, -300f), () => _settings.SetActive(true));
            _settings = Panel(transform, "Settings", new Color(0.96f, 0.98f, 1f, 0.99f));
            Label(_settings.transform, "SETTINGS", 58, new Vector2(0.1f, 0.72f), new Vector2(0.9f, 0.88f));
            Button(_settings.transform, "SOUND", new Vector2(0f, 70f), () => setSound(!sound()));
            Button(_settings.transform, "HAPTICS", new Vector2(0f, -50f), () => setHaptics(!haptics()));
            Button(_settings.transform, "CLOSE", new Vector2(0f, -210f), () => _settings.SetActive(false));
            _settings.SetActive(false);
        }

        private static GameObject Panel(Transform parent, string name, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, 0.12f); rect.anchorMax = new Vector2(0.92f, 0.9f); rect.offsetMin = rect.offsetMax = Vector2.zero;
            Image image = go.GetComponent<Image>(); image.sprite = CloudoraVisuals.RoundedPanel; image.type = Image.Type.Sliced; image.color = color;
            go.AddComponent<Shadow>().effectColor = new Color(0.2f, 0.35f, 0.5f, 0.14f);
            return go;
        }

        private static Button Button(Transform parent, string text, Vector2 position, Action action)
        {
            var go = new GameObject(text, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>(); rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f); rect.anchoredPosition = position; rect.sizeDelta = new Vector2(500f, 94f);
            Image image = go.GetComponent<Image>(); image.sprite = CloudoraVisuals.RoundedPanel; image.type = Image.Type.Sliced; image.color = new Color(0.36f, 0.49f, 0.98f);
            go.GetComponent<Button>().onClick.AddListener(() => action?.Invoke());
            Label(go.transform, text, 32, Vector2.zero, Vector2.one).color = Color.white;
            return go.GetComponent<Button>();
        }

        private static Text Label(Transform parent, string value, int size, Vector2 min, Vector2 max)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
            Text text = go.GetComponent<Text>(); text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = size; text.text = value.ToUpperInvariant(); text.alignment = TextAnchor.MiddleCenter; text.color = new Color(0.15f, 0.22f, 0.29f); text.raycastTarget = false;
            text.rectTransform.anchorMin = min; text.rectTransform.anchorMax = max; text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return text;
        }
    }
}
