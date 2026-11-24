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
        private readonly List<EssenceRegion> _regions=new();
        private EssenceRegion _incoming;
        private int _activeRegionCount;
        private sealed class EssenceRegion
        {
            public RectTransform Rect;public LiquidFillGraphic Fill;public WeatherGlyph Glyph;
            public WeatherType Type;public bool Hidden,Styled;
        }
        private RectTransform _essenceRoot;

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
        private RectTransform _visualBody;
        private GlassVesselGraphic _glass;
        private bool _selected;
        private float _selectionGlow;
        private VesselCompletionFlourish _completion;
        public RectTransform VisualBody=>_visualBody;
        private RectTransform _transferRegion,_transferTop;
        private Vector2 _transferTopMax;
        private EssenceRegion _receivingTop;
        private Vector2 _receivingTopMax;

        public int Capacity { get; private set; }

        public int ElementCount => _elements.Count;

        public IReadOnlyList<WeatherType> Elements => _elements;

        public bool IsEmpty => _elements.Count == 0;

        public bool IsFull => _elements.Count >= Capacity;
        public bool IsComplete => !IsEmpty&&IsSolved();

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
                _background.sprite=null;
                _background.type=Image.Type.Simple;
                _background.color=Color.clear;
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
            string glass=capacity==5?"c190e":capacity==6?"91521":capacity>=7?"b3e5d":"b44ef";
            _background.sprite=null;

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
            for(int i=transform.childCount-1;i>=0;i--){DesignUI.Remove(transform.GetChild(i).gameObject);}
            _regions.Clear();_incoming=null;_transferTop=null;_transferRegion=null;_receivingTop=null;_activeRegionCount=0;
            _visualBody=DesignUI.Rect(transform,"Vessel body",0,0,0,0);_visualBody.anchorMin=Vector2.zero;_visualBody.anchorMax=Vector2.one;_visualBody.pivot=new Vector2(.5f,.5f);_visualBody.offsetMin=_visualBody.offsetMax=Vector2.zero;
            _essenceRoot=DesignUI.Rect(_visualBody,"Essence regions",0,0,0,0);
            _essenceRoot.anchorMin=new Vector2(.14f,9f/(Capacity*36+43));_essenceRoot.anchorMax=new Vector2(.86f,1-13f/(Capacity*36+43));_essenceRoot.offsetMin=_essenceRoot.offsetMax=Vector2.zero;
            var mask=_essenceRoot.gameObject.AddComponent<VesselFillMask>();mask.raycastTarget=false;_essenceRoot.gameObject.AddComponent<Mask>().showMaskGraphic=false;
            var glassRect=DesignUI.Rect(_visualBody,"Glass",0,0,0,0);glassRect.pivot=new Vector2(.5f,.5f);glassRect.anchorMin=Vector2.zero;glassRect.anchorMax=Vector2.one;glassRect.offsetMin=glassRect.offsetMax=Vector2.zero;
            _glass=glassRect.gameObject.AddComponent<GlassVesselGraphic>();_glass.raycastTarget=false;_glass.Selected=_selected;_glass.SelectionGlow=_selectionGlow;
        }
        private EssenceRegion CreateRegion(string name)
        {
            var r=DesignUI.Rect(_essenceRoot,name,0,0,0,0);r.offsetMin=r.offsetMax=Vector2.zero;
            var fill=r.gameObject.AddComponent<LiquidFillGraphic>();fill.raycastTarget=false;
            return new EssenceRegion{Rect=r,Fill=fill};
        }
        private void StyleRegion(EssenceRegion region,WeatherType type,bool hidden,bool incoming=false)
        {
            if(region.Styled&&region.Type==type&&region.Hidden==hidden)return;
            region.Styled=true;region.Type=type;region.Hidden=hidden;
            if(!incoming)region.Rect.name="Essence "+type;
            Color c=hidden?new Color(.72f,.78f,.86f,.22f):GetColor(type);
            region.Fill.Top=hidden?c:Color.Lerp(c,Color.white,.16f);region.Fill.Bottom=hidden?c:Color.Lerp(c,Color.black,.12f);region.Fill.SetVerticesDirty();
            if(!hidden&&!region.Glyph){var r=DesignUI.Rect(region.Rect,"Weather symbol",0,0,18,18);CenterSymbol(r);region.Glyph=r.gameObject.AddComponent<WeatherGlyph>();region.Glyph.raycastTarget=false;}
            if(region.Glyph){region.Glyph.SetElement(type);if(region.Glyph.gameObject.activeSelf==hidden)region.Glyph.gameObject.SetActive(!hidden);}
        }
        private static void CenterSymbol(RectTransform r){r.anchorMin=r.anchorMax=new Vector2(.5f,.5f);r.pivot=new Vector2(.5f,.5f);r.anchoredPosition=Vector2.zero;r.gameObject.AddComponent<EssenceSymbolFit>();}
        private void RefreshVisuals()
        {
            if(!_essenceRoot)return;
            ClearTransferPreview();int group=0;
            for(int i=0;i<_elements.Count;)
            {
                WeatherType type=_elements[i];int count=1;while(i+count<_elements.Count&&_elements[i+count]==type)count++;
                if(group==_regions.Count)_regions.Add(CreateRegion("Essence"));
                var region=_regions[group++];StyleRegion(region,type,_hideContents&&i+count<_elements.Count);
                region.Rect.anchorMin=new Vector2(0,i/(float)Capacity);region.Rect.anchorMax=new Vector2(1,(i+count)/(float)Capacity);
                region.Fill.SetSurface(i+count==_elements.Count);
                if(!region.Rect.gameObject.activeSelf)region.Rect.gameObject.SetActive(true);
                i+=count;
            }
            _activeRegionCount=group;
            for(int i=group;i<_regions.Count;i++)if(_regions[i].Rect.gameObject.activeSelf)_regions[i].Rect.gameObject.SetActive(false);
            bool solved=IsComplete;if(!solved&&_completion)_completion.StopEffect();
            if(_glass.Solved!=solved){_glass.Solved=solved;_glass.SetVerticesDirty();}
        }

        public Vector3 MouthPosition => _visualBody ? _visualBody.TransformPoint(new Vector3(_visualBody.rect.center.x, _visualBody.rect.yMax - _visualBody.rect.width / 90f * 8, 0)) : transform.position;

        public void SetTransferHighlight(Color tint, float strength)
        {
            if (!_glass) return;
            strength=Mathf.Clamp01(strength);
            if(_glass.TransferTint==tint&&Mathf.Approximately(_glass.TransferGlow,strength))return;
            _glass.TransferTint = tint; _glass.TransferGlow = strength; _glass.SetVerticesDirty();
        }

        public Vector3 FluidSurfacePosition(float additional=0)
        {
            if(!_essenceRoot)return transform.position;var r=_essenceRoot.rect;
            return _essenceRoot.TransformPoint(new Vector3(r.center.x,Mathf.Lerp(r.yMin,r.yMax,(ElementCount+additional)/Capacity),0));
        }
        public void SetTransferPreview(WeatherType type,int count,float progress,float phase=0,float energy=0)
        {
            if(!_essenceRoot)return;
            if(progress<0)
            {
                if(!_transferTop&&_activeRegionCount>0){_transferTop=_regions[_activeRegionCount-1].Rect;_transferTopMax=_transferTop.anchorMax;}
                if(_transferTop)
                {
                    _transferTop.anchorMax=new Vector2(1,Mathf.Max(_transferTop.anchorMin.y,_transferTopMax.y+progress*count/Capacity));
                    _regions[_activeRegionCount-1].Fill.SetSurface(true,phase,energy*.45f);
                }
            }
            else if(progress>0)
            {
                // Grow the existing fluid body: no second rectangle, gradient seam or duplicate icon.
                var top=_activeRegionCount>0?_regions[_activeRegionCount-1]:null;
                if(top!=null&&top.Type==type&&!top.Hidden)
                {
                    if(_receivingTop==null){_receivingTop=top;_receivingTopMax=top.Rect.anchorMax;}
                    top.Rect.anchorMax=new Vector2(1,_receivingTopMax.y+progress*count/Capacity);
                    top.Fill.SetSurface(true,phase,energy);
                }
                else
                {
                    if(_incoming==null)_incoming=CreateRegion("Incoming weather");
                    _transferRegion=_incoming.Rect;StyleRegion(_incoming,type,false,true);
                    if(!_transferRegion.gameObject.activeSelf)_transferRegion.gameObject.SetActive(true);
                    _transferRegion.anchorMin=new Vector2(0,ElementCount/(float)Capacity);_transferRegion.anchorMax=new Vector2(1,(ElementCount+progress*count)/Capacity);_transferRegion.offsetMin=_transferRegion.offsetMax=Vector2.zero;
                    _incoming.Fill.SetSurface(true,phase,energy);
                    if(top!=null)top.Fill.SetSurface(false);
                }
            }
        }
        public void ClearTransferPreview()
        {
            if(_transferTop)_transferTop.anchorMax=_transferTopMax;
            if(_receivingTop!=null)_receivingTop.Rect.anchorMax=_receivingTopMax;
            if(_transferRegion&&_transferRegion.gameObject.activeSelf)_transferRegion.gameObject.SetActive(false);
            for(int i=0;i<_activeRegionCount;i++)_regions[i].Fill.SetSurface(i==_activeRegionCount-1);
            _transferTop=null;_transferRegion=null;_receivingTop=null;
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

                // Güncel hava paleti: Kar cyan, Rüzgar mint, Şimşek gece mavisi.
                WeatherType.Snow => new Color32(0x7F, 0xE3, 0xF5, 0xFF),
                WeatherType.Wind => new Color32(0x43, 0xB8, 0xA0, 0xFF),
                WeatherType.Moon => new Color(0.56f, 0.49f, 0.95f),
                WeatherType.Lightning => new Color32(0x25, 0x35, 0x8A, 0xFF),
                WeatherType.Rainbow => new Color(0.95f, 0.55f, 0.78f),

                _ => Color.magenta
            };
        }

        private static readonly Color DarkPatternColor = new Color(0.15f, 0.22f, 0.29f, 0.85f);

        // Koyu renkli parçada (Şimşek) desen beyaz, diğerlerinde koyu.
        public static Color GetPatternColor(WeatherType element)
        {
            Color color = GetColor(element);
            float brightness = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            return brightness < 0.43f ? Color.white : DarkPatternColor;
        }

        // Oyuncu bu cloud'u seçtiğinde görünüşünü değiştirir.
        public void SetSelected(bool selected)
        {
            if(selected&&IsComplete)return;
            if(_selected==selected)return;_selected=selected;
            if(_glass){_glass.Selected=selected;_glass.SetVerticesDirty();}
        }

        private void Update()
        {
            float next=Mathf.MoveTowards(_selectionGlow,_selected?1:0,Time.unscaledDeltaTime*9);
            if(Mathf.Approximately(next,_selectionGlow))return;
            _selectionGlow=next;
            if(_glass){_glass.SelectionGlow=next;_glass.SetVerticesDirty();}
        }

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
            CloudContainerView target, int maxCount = int.MaxValue)
        {
            if (IsComplete || target == this)
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
                Mathf.Min(groupCount,Mathf.Min(availableSpace,maxCount));

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
                _modifierBadge.font = DesignUI.Font;
                _modifierBadge.fontSize = 20;
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
            if(!IsComplete)return;
            WeatherType element=WeatherType.Rainbow;
            foreach(var type in _elements)if(type!=WeatherType.Rainbow){element=type;break;}
            if(!_completion)
            {
                var r=DesignUI.Rect(_visualBody,"Element completion",0,0,0,0);r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
                r.gameObject.AddComponent<Canvas>();_completion=r.gameObject.AddComponent<VesselCompletionFlourish>();_completion.raycastTarget=false;
            }
            _completion.Play(this,element);
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

    }
}
