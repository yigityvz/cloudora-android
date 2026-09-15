using System;
using UnityEngine;

namespace Cloudora.Progression
{
    [Serializable]
    public sealed class WorldDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int FirstLevel { get; }
        public int LastLevel { get; }
        public int Capacity { get; }
        public string NewRule { get; }
        public Color SkyColor { get; }
        public Color LandColor { get; }

        public int LevelCount => LastLevel - FirstLevel + 1;

        public WorldDefinition(string id, string name, int first, int last, int capacity, string rule, string skyHex, string landHex)
        {
            Id = id;
            DisplayName = name;
            FirstLevel = first;
            LastLevel = last;
            Capacity = capacity;
            NewRule = rule;
            ColorUtility.TryParseHtmlString(skyHex, out Color sky);
            ColorUtility.TryParseHtmlString(landHex, out Color land);
            SkyColor = sky;
            LandColor = land;
        }
    }
}
