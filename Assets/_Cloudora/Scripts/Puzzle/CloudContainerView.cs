using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Puzzle
{
    public class CloudContainerView : MonoBehaviour
    {
        private readonly List<WeatherType> _elements = new();
        private readonly List<Image> _slotImages = new();

        private Button _button;

        // Cloud'un dışındaki mavi ana kutu.
        private Image _background;

        // Seçilmediği zamanki normal rengi.
        private Color _normalBackgroundColor;

        private Action<CloudContainerView> _onClicked;

        public int Capacity { get; private set; }

        public int ElementCount => _elements.Count;

        public bool IsEmpty => _elements.Count == 0;

        public bool IsFull => _elements.Count >= Capacity;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _background = GetComponent<Image>();

            _normalBackgroundColor = _background.color;

            _button.targetGraphic = _background;
        }

        public void Initialize(
            int capacity,
            IEnumerable<WeatherType> initialElements,
            Action<CloudContainerView> onClicked)
        {
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

                LayoutElement layoutElement =
                    slotObject.GetComponent<LayoutElement>();

                layoutElement.flexibleHeight = 1f;
                layoutElement.flexibleWidth = 1f;

                _slotImages.Add(slotImage);
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

                    continue;
                }

                int elementIndex =
                    Capacity - 1 - visualIndex;

                WeatherType element =
                    _elements[elementIndex];

                slotImage.color =
                    GetWeatherColor(element);
            }
        }

        private static Color GetWeatherColor(
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
            }
            else
            {
                _background.color =
                    _normalBackgroundColor;
            }
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

            return topElement == element;
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

            WeatherType firstElement = _elements[0];

            // Bütün elementler aynı tür mü?
            for (int i = 1; i < _elements.Count; i++)
            {
                if (_elements[i] != firstElement)
                {
                    return false;
                }
            }

            return true;
        }
    }
}