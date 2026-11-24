using System;
using UnityEngine;

namespace Cloudora.Puzzle
{
    public enum WeatherMotionStyle { Essence, Sun, Rain, Snow, Wind, Moon, Lightning, Rainbow }

    [Serializable]
    public struct WeatherMotionProfile
    {
        public WeatherType Element;
        public WeatherMotionStyle Style;
        [Range(.4f, 1.2f)] public float FlightSeconds;
        [Range(12, 72)] public float ArcHeight;
        [Range(5, 16)] public float SymbolRadius;
        [Range(0, 1)] public float Drift;
        public bool OverrideTint;
        public Color Tint;

        public WeatherMotionProfile Validated()
        {
            var p = this;
            p.FlightSeconds = Mathf.Clamp(p.FlightSeconds, .4f, 1.2f);
            p.ArcHeight = Mathf.Clamp(p.ArcHeight, 12, 72);
            p.SymbolRadius = Mathf.Clamp(p.SymbolRadius, 5, 16);
            p.Drift = Mathf.Clamp01(p.Drift);
            return p;
        }

        public static WeatherMotionProfile Default(WeatherType element)
        {
            var p = new WeatherMotionProfile { Element = element, FlightSeconds = .64f, ArcHeight = 40, SymbolRadius = 11, Drift = .25f };
            switch (element)
            {
                case WeatherType.Sun: p.Style = WeatherMotionStyle.Sun; break;
                case WeatherType.Rain: p.Style = WeatherMotionStyle.Rain; p.FlightSeconds = .60f; break;
                case WeatherType.Snow: p.Style = WeatherMotionStyle.Snow; p.FlightSeconds = .74f; p.Drift = .65f; break;
                case WeatherType.Wind: p.Style = WeatherMotionStyle.Wind; p.FlightSeconds = .62f; p.Drift = .9f; break;
                case WeatherType.Moon: p.Style = WeatherMotionStyle.Moon; p.FlightSeconds = .72f; break;
                case WeatherType.Lightning: p.Style = WeatherMotionStyle.Lightning; p.FlightSeconds = .48f; p.Drift = 0; break;
                case WeatherType.Rainbow: p.Style = WeatherMotionStyle.Rainbow; p.FlightSeconds = .76f; p.ArcHeight = 44; break;
                default: p.Style = WeatherMotionStyle.Essence; break;
            }
            return p;
        }
    }

    // Optional overrides live in Resources/WeatherMotionLibrary.asset. The same resolver is
    // used by every board, regardless of its level, capacity or number of vessels.
    [CreateAssetMenu(menuName = "Cloudora/Weather Motion Library")]
    public sealed class WeatherMotionLibrary : ScriptableObject
    {
        public WeatherMotionProfile[] Profiles = Array.Empty<WeatherMotionProfile>();
        public WeatherMotionProfile Resolve(WeatherType element)
        {
            if (Profiles != null)
                foreach (var profile in Profiles)
                    if (profile.Element == element) return profile.Validated();
            return WeatherMotionProfile.Default(element);
        }
    }
}
