using Cloudora.UI;
using System.Linq;
using Cloudora.Puzzle;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    // v3.2: hava türü renkleri (tasarım §11c) — Kar cyan, Rüzgar gümüş, Şimşek gece mavisi + beyaz desen.
    public sealed class V32VisualTests
    {
        [TestCase(WeatherType.Snow, "#7FE3F5")]
        [TestCase(WeatherType.Wind, "#43B8A0")]
        [TestCase(WeatherType.Lightning, "#25358A")]
        [TestCase(WeatherType.Sun, "#FFC226")]
        [TestCase(WeatherType.Rain, "#338CFF")]
        [TestCase(WeatherType.Moon, "#8F7DF2")]
        public void WeatherColorsMatchChosenPalette(WeatherType weather, string hex)
        {
            Assert.That(ColorUtility.TryParseHtmlString(hex, out Color expected), Is.True);
            Color actual = CloudContainerView.GetColor(weather);
            Assert.That(actual.r, Is.EqualTo(expected.r).Within(0.004f), $"{weather} r");
            Assert.That(actual.g, Is.EqualTo(expected.g).Within(0.004f), $"{weather} g");
            Assert.That(actual.b, Is.EqualTo(expected.b).Within(0.004f), $"{weather} b");
        }

        [Test]
        public void WeatherColorsAreClearlyDistinct()
        {
            // Eskiden Şimşek ile Güneş (0,257) ve Kar ile Rüzgar (0,274) neredeyse aynıydı; her çift belirgin şekilde ayrı olmalı.
            WeatherType[] types = { WeatherType.Sun, WeatherType.Rain, WeatherType.Snow, WeatherType.Wind, WeatherType.Moon, WeatherType.Lightning };
            foreach (WeatherType a in types)
            foreach (WeatherType b in types.Where(t => t > a))
            {
                Color x = CloudContainerView.GetColor(a), y = CloudContainerView.GetColor(b);
                float distance = Mathf.Sqrt((x.r - y.r) * (x.r - y.r) + (x.g - y.g) * (x.g - y.g) + (x.b - y.b) * (x.b - y.b));
                Assert.That(distance, Is.GreaterThan(0.3f), $"{a} vs {b}");
            }
        }

        [Test]
        public void WeatherUsesDistinctModernSymbolsWithoutInterceptingInput()
        {
            var go = new GameObject("Cloud", typeof(RectTransform), typeof(Image), typeof(Button));
            try
            {
                CloudContainerView view = go.AddComponent<CloudContainerView>();
                view.Initialize(4, new[] { WeatherType.Lightning, WeatherType.Sun, WeatherType.Rain }, _ => { });
                WeatherGlyph[] symbols=go.GetComponentsInChildren<WeatherGlyph>();
                Assert.That(symbols.Length,Is.EqualTo(3));
                foreach(var type in new[]{WeatherType.Lightning,WeatherType.Sun,WeatherType.Rain}){
                    var icon=symbols.Single(x=>x.Element==type);Assert.That(icon.raycastTarget,Is.False);
                }

            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
