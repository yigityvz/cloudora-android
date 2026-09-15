using System.Collections.Generic;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public static class AuthoredLevelCatalog
    {
        private static readonly IReadOnlyList<LevelDefinition> Levels = Build();

        public static int Count => Levels.Count;

        public static LevelDefinition Get(int levelNumber)
        {
            int index = System.Math.Max(1, System.Math.Min(levelNumber, Count)) - 1;
            return Levels[index];
        }

        private static IReadOnlyList<LevelDefinition> Build()
        {
            WeatherType S = WeatherType.Sun;
            WeatherType R = WeatherType.Rain;
            WeatherType N = WeatherType.Snow;
            WeatherType[] E() => System.Array.Empty<WeatherType>();
            return new[]
            {
                L(1, "Tap a cloud", new[] { S, R, S, R }, new[] { R, S, R, S }, E(), E()),
                L(2, "Tap a matching target", new[] { S, S, R, R }, new[] { R, R, S, S }, E(), E()),
                L(3, "Groups move together", new[] { R, S, S, R }, new[] { S, R, R, S }, E(), E()),
                L(4, "Empty clouds create room", new[] { S, R, R, S }, new[] { R, S, S, R }, E(), E()),
                L(5, "Restore the sky", new[] { S, R, S, R }, new[] { R, S, R, S }, E(), E()),
                L(6, null, new[] { S, R, N, S }, new[] { R, N, S, R }, new[] { N, S, R, N }, E(), E()),
                L(7, null, new[] { S, S, R, N }, new[] { R, R, N, S }, new[] { N, N, S, R }, E(), E()),
                L(8, null, new[] { N, R, S, S }, new[] { S, N, R, R }, new[] { R, S, N, N }, E(), E()),
                L(9, null, new[] { R, S, N, R }, new[] { N, R, S, N }, new[] { S, N, R, S }, E(), E()),
                L(10, "Undo is always safe", new[] { S, N, R, S }, new[] { R, S, N, R }, new[] { N, R, S, N }, E(), E()),
                L(11, null, new[] { S, R, R, N }, new[] { N, S, S, R }, new[] { R, N, N, S }, E(), E()),
                L(12, null, new[] { N, S, R, N }, new[] { R, N, S, R }, new[] { S, R, N, S }, E(), E()),
                L(13, null, new[] { R, N, S, S }, new[] { S, R, N, N }, new[] { N, S, R, R }, E(), E()),
                L(14, null, new[] { S, R, N, R }, new[] { R, N, S, N }, new[] { N, S, R, S }, E(), E()),
                L(15, "The world is waking up", new[] { N, R, S, R }, new[] { S, N, R, N }, new[] { R, S, N, S }, E(), E())
            };
        }

        private static LevelDefinition L(int id, string cue, params WeatherType[][] clouds)
        {
            return new LevelDefinition(id, "green-valley", 4, cue, clouds);
        }
    }
}
