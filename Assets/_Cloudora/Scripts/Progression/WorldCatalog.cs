using System.Collections.Generic;

namespace Cloudora.Progression
{
    public static class WorldCatalog
    {
        public const int MaxLevel = 100;
        public const int IntroLastLevel = 14;
        public static readonly IReadOnlyList<WorldDefinition> All = new[]
        {
            W("green-valley", "Yeşil Vadi", 15, 30, 4, "Basic Sort", "#EAF6FF", "#84C98B"),
            W("rainy-coast", "Rainy Coast", 31, 60, 5, "Wind", "#DCEEFF", "#65A8B8"),
            W("frost-peaks", "Frost Peaks", 61, 100, 6, "Frozen", "#E8F8FF", "#B5DDEB")
        };

        public static WorldDefinition ForLevel(int level)
        {
            if(level<=IntroLastLevel) return All[0];
            foreach (WorldDefinition world in All)
                if (level >= world.FirstLevel && level <= world.LastLevel) return world;
            return All[All.Count - 1];
        }

        private static WorldDefinition W(string id, string name, int first, int last, int capacity, string rule, string sky, string land)
            => new(id, name, first, last, capacity, rule, sky, land);
    }
}
