using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cloudora.Modifiers;
using Cloudora.UI;

namespace Cloudora.Puzzle
{
    public class CloudContainerView : MonoBehaviour
    {
        private readonly List<WeatherType> _elements = new();
        private readonly List<Image> _slotImages = new();
        private readonly List<Text> _slotLabels = new();

        private Button _button;

        // Cloud'un dışındaki mavi ana kutu.
        private Image _background;

        // Seçilmediği zamanki normal rengi.
        private Color _normalBackgroundColor;

        private Action<CloudContainerView> _onClicked;
        private Coroutine _feedbackRoutine;
        private bool _hideContents;
        private Text _modifierBadge;
        private bool _visualPrepared;

        public int Capacity { get; private set; }

        public int ElementCount => _elements.Count;

        public IReadOnlyList<WeatherType> Elements => _elements;

        public bool IsEmpty => _elements.Count == 0;

        public bool IsFull => _elements.Count >= Capacity;

        private void Awake()
        {
            EnsureComponents();
        }

        private void EnsureComponents()
        {
            _button = GetComponent<Button>();
            _background = GetComponent<Image>();

            _normalBackgroundColor = _background.color;

            _button.targetGraphic = _background;
            if (!_visualPrepared)
            {
                _visualPrepared = true;
                _background.sprite = CloudoraVisuals.CloudVessel;
                _background.type = Image.Type.Sliced;
                _background.color = new Color(0.93f, 0.97f, 1f, 0.96f);
                Shadow shadow = GetComponent<Shadow>() ?? gameObject.AddComponent<Shadow>();
                shadow.effectColor = new Color(0.15f, 0.28f, 0.42f, 0.16f);
                shadow.effectDistance = new Vector2(0f, -7f);
                _normalBackgroundColor = _background.color;
            }
        }

        public void Initialize(
            int capacity,
            IEnumerable<WeatherType> initialElements,
            Action<CloudContainerView> onClicked)
        {
            EnsureComponents();
            Capacity = capacity;

            _elements.Clear();
            _elements.AddRange(initialElements);

            _onClicked = onClicked;

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleClick);

            BuildSlots();
            RefreshVisuals();
        }

        private void HandleClick()
        {
            _onClicked?.Invoke(this);
        }

        private void BuildSlots()
        {
            _slotImages.Clear();
            _slotLabels.Clear();

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            GameObject slotRoot = new GameObject(
                "Slots",
                typeof(RectTransform),
                typeof(VerticalLayoutGroup));

            slotRoot.transform.SetParent(transform, false);

            RectTransform rootRect =
                slotRoot.GetComponent<RectTransform>();

            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;

            rootRect.offsetMin = new Vector2(15f, 15f);
            rootRect.offsetMax = new Vector2(-15f, -15f);

            VerticalLayoutGroup layout =
                slotRoot.GetComponent<VerticalLayoutGroup>();

            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 8f;

            layout.childAlignment = TextAnchor.UpperCenter;

            layout.childControlWidth = true;
            layout.childControlHeight = true;

            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            for (int i = 0; i < Capacity; i++)
            {
                GameObject slotObject = new GameObject(
                    $"Slot_{i + 1}",
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(LayoutElement));

                slotObject.transform.SetParent(
                    slotRoot.transform,
                    false);

                Image slotImage =
                    slotObject.GetComponent<Image>();

                slotImage.raycastTarget = false;
                slotImage.sprite = CloudoraVisuals.RoundedPanel;
                slotImage.type = Image.Type.Sliced;

                var labelObject = new GameObject("Pattern", typeof(RectTransform), typeof(Text));
                labelObject.transform.SetParent(slotObject.transform, false);
                Text label = labelObject.GetComponent<Text>();
                label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                label.fontSize = 22;
                label.fontStyle = FontStyle.Bold;
                label.alignment = TextAnchor.MiddleCenter;
                label.color = new Color(0.15f, 0.22f, 0.29f, 0.85f);
                label.raycastTarget = false;
                label.rectTransform.anchorMin = Vector2.zero;
                label.rectTransform.anchorMax = Vector2.one;
                label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;

                LayoutElement layoutElement =
                    slotObject.GetComponent<LayoutElement>();

                layoutElement.flexibleHeight = 1f;
                layoutElement.flexibleWidth = 1f;

                _slotImages.Add(slotImage);
                _slotLabels.Add(label);
            }
        }

        private void RefreshVisuals()
        {
            int emptySlotCount =
                Capacity - _elements.Count;

            for (int visualIndex = 0;
                 visualIndex < Capacity;
                 visualIndex++)
            {
                Image slotImage =
                    _slotImages[visualIndex];

                if (visualIndex < emptySlotCount)
                {
                    slotImage.color =
                        new Color(1f, 1f, 1f, 0.08f);
                    _slotLabels[visualIndex].text = string.Empty;

                    continue;
                }

                int elementIndex =
                    Capacity - 1 - visualIndex;

                WeatherType element =
                    _elements[elementIndex];

                slotImage.color = GetColor(element);
                _slotLabels[visualIndex].text = GetPattern(element);
                if (_hideContents && visualIndex > emptySlotCount)
                    slotImage.color = new Color(0.72f, 0.78f, 0.86f, 0.22f);
            }
        }

        public static Color GetColor(
            WeatherType element)
        {
            return element switch
            {
                WeatherType.Sun =>
                    new Color(1f, 0.76f, 0.15f),

                WeatherType.Rain =>
                    new Color(0.20f, 0.55f, 1f),

                WeatherType.Snow =>
                    new Color(0.82f, 0.94f, 1f),

                WeatherType.Wind => new Color(0.56f, 0.84f, 0.78f),
                WeatherType.Moon => new Color(0.56f, 0.49f, 0.95f),
                WeatherType.Lightning => new Color(1f, 0.82f, 0.40f),
                WeatherType.Rainbow => new Color(0.95f, 0.55f, 0.78f),

                _ => Color.magenta
            };
        }

        // Oyuncu bu cloud'u seçtiğinde görünüşünü değiştirir.
        public void SetSelected(bool selected)
        {
            if (selected)
            {
                _background.color =
                    Color.Lerp(
                        _normalBackgroundColor,
                        Color.white,
                        0.35f);
                transform.localScale = Vector3.one * 1.04f;
            }
            else
            {
                _background.color =
                    _normalBackgroundColor;
                transform.localScale = Vector3.one;
            }
        }

        private static string GetPattern(WeatherType type) => type switch
        {
            WeatherType.Sun => "*",
            WeatherType.Rain => "///",
            WeatherType.Snow => "+",
            WeatherType.Wind => "~~~",
            WeatherType.Moon => ")",
            WeatherType.Lightning => "!!",
            WeatherType.Rainbow => "<>",
            _ => "?"
        };

        public WeatherType[] CaptureElements()
        {
            return _elements.ToArray();
        }

        public void RestoreElements(IEnumerable<WeatherType> elements)
        {
            _elements.Clear();
            _elements.AddRange(elements);
            RefreshVisuals();
        }

        public bool TryGetTopElement(
            out WeatherType element)
        {
            if (IsEmpty)
            {
                element = default;
                return false;
            }

            element = _elements[^1];

            return true;
        }

        public bool CanReceive(
            WeatherType element)
        {
            if (IsFull)
            {
                return false;
            }

            if (IsEmpty)
            {
                return true;
            }

            WeatherType topElement =
                _elements[^1];

            return topElement == element || topElement == WeatherType.Rainbow || element == WeatherType.Rainbow;
        }

        private int GetTopGroupCount()
        {
            if (IsEmpty)
            {
                return 0;
            }

            WeatherType topElement =
                _elements[^1];

            int count = 0;

            for (int i = _elements.Count - 1;
                 i >= 0;
                 i--)
            {
                if (_elements[i] != topElement)
                {
                    break;
                }

                count++;
            }

            return count;
        }

        public int TopGroupCount => GetTopGroupCount();

        public bool TryMoveTopGroupTo(
            CloudContainerView target)
        {
            if (target == this)
            {
                return false;
            }

            if (IsEmpty)
            {
                return false;
            }

            WeatherType movingElement =
                _elements[^1];

            if (!target.CanReceive(movingElement))
            {
                return false;
            }

            int groupCount =
                GetTopGroupCount();

            int availableSpace =
                target.Capacity - target.ElementCount;

            int moveCount =
                Mathf.Min(groupCount, availableSpace);

            if (moveCount <= 0)
            {
                return false;
            }

            for (int i = 0; i < moveCount; i++)
            {
                _elements.RemoveAt(
                    _elements.Count - 1);

                target._elements.Add(
                    movingElement);
            }

            RefreshVisuals();
            target.RefreshVisuals();

            return true;


        }

        public bool IsSolved()
        {
            // Boş cloud sorun değildir.
            if (IsEmpty)
            {
                return true;
            }

            // İçinde element varsa ama tamamen dolu değilse
            // henüz çözülmüş değildir.
            if (!IsFull)
            {
                return false;
            }

            WeatherType? resolved = null;
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i] == WeatherType.Rainbow) continue;
                if (resolved.HasValue && _elements[i] != resolved.Value) return false;
                resolved = _elements[i];
            }

            return true;
        }

        public void SetModifierState(ModifierType type, bool hideContents, bool blocked)
        {
            _hideContents = hideContents;
            if (_modifierBadge == null)
            {
                var badge = new GameObject("ModifierBadge", typeof(RectTransform), typeof(Text));
                badge.transform.SetParent(transform, false);
                _modifierBadge = badge.GetComponent<Text>();
                _modifierBadge.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                _modifierBadge.fontSize = 24;
                _modifierBadge.alignment = TextAnchor.UpperCenter;
                _modifierBadge.raycastTarget = false;
                _modifierBadge.rectTransform.anchorMin = new Vector2(0f, 0.82f);
                _modifierBadge.rectTransform.anchorMax = Vector2.one;
                _modifierBadge.rectTransform.offsetMin = _modifierBadge.rectTransform.offsetMax = Vector2.zero;
            }
            _modifierBadge.text = type == ModifierType.None ? string.Empty : type.ToString().ToUpperInvariant();
            _modifierBadge.color = blocked ? new Color(0.85f, 0.32f, 0.38f) : new Color(0.15f, 0.22f, 0.29f);
            RefreshVisuals();
        }

        public void PlayInvalidFeedback()
        {
            StartFeedback(InvalidFeedbackRoutine());
        }

        public void PlaySolvedFeedback()
        {
            StartFeedback(SolvedFeedbackRoutine());
        }

        private void StartFeedback(IEnumerator routine)
        {
            if (_feedbackRoutine != null)
            {
                StopCoroutine(_feedbackRoutine);
                transform.localScale = Vector3.one;
            }

            _feedbackRoutine = StartCoroutine(routine);
        }

        private IEnumerator InvalidFeedbackRoutine()
        {
            RectTransform rect = (RectTransform)transform;
            Vector2 origin = rect.anchoredPosition;
            const float duration = 0.18f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float strength = Mathf.Lerp(12f, 0f, elapsed / duration);
                rect.anchoredPosition = origin + Vector2.right * Mathf.Sin(elapsed * 75f) * strength;
                yield return null;
            }

            rect.anchoredPosition = origin;
            _feedbackRoutine = null;
        }

        private IEnumerator SolvedFeedbackRoutine()
        {
            const float duration = 0.34f;
            float elapsed = 0f;
            Color originColor = _background.color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float wave = Mathf.Sin(elapsed / duration * Mathf.PI);
                transform.localScale = Vector3.one * (1f + wave * 0.08f);
                _background.color = Color.Lerp(originColor, new Color(0.75f, 1f, 0.9f), wave * 0.55f);
                yield return null;
            }

            transform.localScale = Vector3.one;
            _background.color = originColor;
            _feedbackRoutine = null;
        }
    }
}
