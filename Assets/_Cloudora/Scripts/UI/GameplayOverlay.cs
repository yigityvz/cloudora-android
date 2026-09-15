using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class GameplayOverlay : MonoBehaviour
    {
        private Button _undoButton;
        private GameObject _completePanel;
        private Text _levelLabel;
        private Text _tutorialLabel;
        private Text _livesLabel;
        private Text _boosterLabel;
        private GameObject _blockingPanel;

        public static GameplayOverlay Create(Canvas canvas, Action onRestart, Action onUndo, Action onContinue, Action onWorld, Action onExtraCloud, Action onSafeShuffle)
        {
            var root = new GameObject("GameplayOverlay", typeof(RectTransform));
            root.transform.SetParent(canvas.transform, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = rootRect.offsetMax = Vector2.zero;

            var overlay = root.AddComponent<GameplayOverlay>();
            overlay._levelLabel = CreateTopLabel(root.transform, "Level 1", new Vector2(0f, -80f), 42);
            overlay._tutorialLabel = CreateTopLabel(root.transform, string.Empty, new Vector2(0f, -145f), 30);
            overlay._livesLabel = CreateTopLabel(root.transform, "♥ 5", new Vector2(390f, -80f), 34);
            CreateButton(root.transform, "Restart", new Vector2(-130f, 120f), onRestart);
            overlay._undoButton = CreateButton(root.transform, "Undo", new Vector2(130f, 120f), onUndo);
            Button worldButton = CreateButton(root.transform, "World", new Vector2(390f, 120f), onWorld);
            worldButton.GetComponent<RectTransform>().sizeDelta = new Vector2(190f, 88f);
            Button extraButton = CreateButton(root.transform, "Extra", new Vector2(-390f, 22f), onExtraCloud);
            extraButton.GetComponent<RectTransform>().sizeDelta = new Vector2(190f, 76f);
            Button shuffleButton = CreateButton(root.transform, "Shuffle", new Vector2(-155f, 22f), onSafeShuffle);
            shuffleButton.GetComponent<RectTransform>().sizeDelta = new Vector2(220f, 76f);
            overlay._boosterLabel = CreateTopLabel(root.transform, string.Empty, new Vector2(250f, -150f), 25);
            overlay._completePanel = CreateCompletePanel(root.transform, onContinue);
            overlay._blockingPanel = CreateBlockingPanel(root.transform);
            overlay._completePanel.SetActive(false);
            overlay._blockingPanel.SetActive(false);
            return overlay;
        }

        public void SetUndoAvailable(bool available) => _undoButton.interactable = available;
        public void ShowComplete(bool show) => _completePanel.SetActive(show);
        public void SetLevelInfo(int level, string world) => _levelLabel.text = $"{ToTitle(world)}  •  Level {level}";
        public void SetTutorialCue(string cue) => _tutorialLabel.text = string.IsNullOrWhiteSpace(cue) ? string.Empty : $"☝  {cue}";
        public void SetLives(int lives, string countdown) => _livesLabel.text = lives >= 5 ? "♥ 5" : $"♥ {lives}  {countdown}";
        public void SetBoosters(int undo, int extra, int shuffle) => _boosterLabel.text = $"Undo {undo}  •  Extra {extra}  •  Shuffle {shuffle}";
        public void ShowBlock(string title, string detail)
        {
            _blockingPanel.SetActive(true);
            Text[] labels = _blockingPanel.GetComponentsInChildren<Text>();
            labels[0].text = title;
            labels[1].text = detail;
        }
        public void HideBlock() => _blockingPanel.SetActive(false);

        private static Text CreateTopLabel(Transform parent, string value, Vector2 position, int size)
        {
            Text text = CreateText(parent, value, size, TextAnchor.MiddleCenter);
            RectTransform rect = text.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(880f, 60f);
            rect.anchoredPosition = position;
            text.color = new Color(0.15f, 0.22f, 0.29f);
            return text;
        }

        private static string ToTitle(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "Cloudora"
                : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.Replace('-', ' '));
        }

        private static Button CreateButton(Transform parent, string label, Vector2 position, Action action)
        {
            var go = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(220f, 88f);
            go.GetComponent<Image>().color = new Color(0.36f, 0.49f, 0.98f, 0.95f);
            Button button = go.GetComponent<Button>();
            button.onClick.AddListener(() => action?.Invoke());
            CreateText(go.transform, label, 34, TextAnchor.MiddleCenter);
            return button;
        }

        private static GameObject CreateCompletePanel(Transform parent, Action onContinue)
        {
            var panel = new GameObject("LevelCompletePanel", typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(760f, 440f);
            panel.GetComponent<Image>().color = new Color(0.96f, 0.98f, 1f, 0.98f);

            Text title = CreateText(panel.transform, "Sky Balanced!", 58, TextAnchor.MiddleCenter);
            title.color = new Color(0.15f, 0.22f, 0.29f);
            title.rectTransform.anchorMin = new Vector2(0.1f, 0.55f);
            title.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;
            CreateButton(panel.transform, "Continue", new Vector2(0f, 58f), onContinue);
            return panel;
        }

        private static GameObject CreateBlockingPanel(Transform parent)
        {
            var panel = new GameObject("BlockingPanel", typeof(RectTransform), typeof(Image), typeof(Button));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(760f, 380f);
            panel.GetComponent<Image>().color = new Color(0.96f, 0.98f, 1f, 0.99f);
            panel.GetComponent<Button>().onClick.AddListener(() => panel.SetActive(false));
            Text title = CreateText(panel.transform, "No Moves", 50, TextAnchor.MiddleCenter);
            title.color = new Color(0.15f, 0.22f, 0.29f);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.55f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.88f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;
            Text detail = CreateText(panel.transform, "Use a safe option below", 29, TextAnchor.MiddleCenter);
            detail.color = new Color(0.25f, 0.34f, 0.44f);
            detail.rectTransform.anchorMin = new Vector2(0.08f, 0.18f);
            detail.rectTransform.anchorMax = new Vector2(0.92f, 0.55f);
            detail.rectTransform.offsetMin = detail.rectTransform.offsetMax = Vector2.zero;
            return panel;
        }

        private static Text CreateText(Transform parent, string value, int size, TextAnchor alignment)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            Text text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }
    }
}
