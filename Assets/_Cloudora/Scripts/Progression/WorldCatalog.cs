using System.Collections.Generic;

namespace Cloudora.Progression
{
    public static class WorldCatalog
    {
        public static readonly IReadOnlyList<WorldDefinition> All = new[]
        {
            W("green-valley", "Green Valley", 1, 20, 4, "Basic Sort", "#EAF6FF", "#84C98B"),
            W("rainy-coast", "Rainy Coast", 21, 40, 5, "Wind", "#DCEEFF", "#65A8B8"),
            W("frost-peaks", "Frost Peaks", 41, 60, 5, "Frozen", "#E8F8FF", "#B5DDEB"),
            W("sun-desert", "Sun Desert", 61, 80, 6, "Locked", "#FFF0D1", "#E2B36D"),
            W("storm-islands", "Storm Islands", 81, 100, 6, "Fog", "#DDE5F2", "#708AA0"),
            W("blossom-land", "Blossom Land", 101, 125, 6, "Rainbow", "#F6E9FF", "#D68FB1"),
            W("ember-valley", "Ember Valley", 126, 150, 7, "Combinations", "#FFE8DC", "#BC7058"),
            W("moon-garden", "Moon Garden", 151, 175, 7, "Night", "#252C62", "#6670A8"),
            W("sky-kingdom", "Sky Kingdom", 176, 200, 7, "Wind", "#DFF6FF", "#B4CFEA"),
            W("ocean-world", "Ocean World", 201, 250, 7, "Storm Waves", "#D7F5FF", "#4C9CB8"),
            W("aurora-world", "Aurora World", 251, 300, 7, "Wild Visibility", "#E4E7FF", "#75C7B0"),
            W("cosmic-world", "Cosmic World", 301, 350, 7, "Cosmic", "#20244F", "#68599E"),
            W("celestial-rift", "Celestial Rift", 351, 400, 7, "Advanced Cycle", "#302255", "#9A65A5")
        };

        public static WorldDefinition ForLevel(int level)
        {
            foreach (WorldDefinition world in All)
                if (level >= world.FirstLevel && level <= world.LastLevel) return world;
            return All[All.Count - 1];
        }

        private static WorldDefinition W(string id, string name, int first, int last, int capacity, string rule, string sky, string land)
            => new(id, name, first, last, capacity, rule, sky, land);
    }
}
