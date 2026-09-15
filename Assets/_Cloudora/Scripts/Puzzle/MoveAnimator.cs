using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Puzzle
{
    public sealed class MoveAnimator : MonoBehaviour
    {
        [SerializeField, Range(0.08f, 0.5f)] private float duration = 0.18f;

        public IEnumerator Animate(CloudContainerView source, CloudContainerView target, WeatherType type, int count)
        {
            Canvas canvas = source.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                yield break;
            }

            var token = new GameObject("MovingWeather", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
            token.transform.SetParent(canvas.transform, false);
            RectTransform rect = token.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(62f + count * 5f, 62f + count * 5f);
            Image image = token.GetComponent<Image>();
            image.color = CloudContainerView.GetColor(type);
            image.raycastTarget = false;

            Vector3 start = source.transform.position;
            Vector3 end = target.transform.position;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Vector3 position = Vector3.Lerp(start, end, t);
                position.y += Mathf.Sin(t * Mathf.PI) * 90f;
                rect.position = position;
                token.transform.localScale = Vector3.one * Mathf.Lerp(0.85f, 1.05f, Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            Destroy(token);
        }
    }
}
