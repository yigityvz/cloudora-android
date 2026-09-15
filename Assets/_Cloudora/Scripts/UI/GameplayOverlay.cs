using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class GameplayOverlay : MonoBehaviour
    {
        private Button _undoButton;
        private GameObject _completePanel;

        public static GameplayOverlay Create(Canvas canvas, Action onRestart, Action onUndo, Action onContinue)
        {
            var root = new GameObject("GameplayOverlay", typeof(RectTransform));
            root.transform.SetParent(canvas.transform, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = rootRect.offsetMax = Vector2.zero;

            var overlay = root.AddComponent<GameplayOverlay>();
            CreateButton(root.transform, "Restart", new Vector2(-130f, 120f), onRestart);
            overlay._undoButton = CreateButton(root.transform, "Undo", new Vector2(130f, 120f), onUndo);
            overlay._completePanel = CreateCompletePanel(root.transform, onContinue);
            overlay._completePanel.SetActive(false);
            return overlay;
        }

        public void SetUndoAvailable(bool available) => _undoButton.interactable = available;
        public void ShowComplete(bool show) => _completePanel.SetActive(show);

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
