using System;
using System.Collections.Generic;
using Cloudora.Core;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public static class LevelGenerator
    {
        private static readonly WeatherType[] WeatherOrder =
        {
            WeatherType.Sun, WeatherType.Rain, WeatherType.Snow, WeatherType.Wind,
            WeatherType.Moon, WeatherType.Lightning
        };

        public static LevelDefinition Generate(int levelNumber, int? seedOverride = null)
        {
            int capacity = levelNumber <= 20 ? 4 : levelNumber <= 60 ? 5 : levelNumber <= 125 ? 6 : 7;
            int typeCount = Math.Min(WeatherOrder.Length, 3 + Math.Max(0, levelNumber - 16) / 35);
            int emptyCount = levelNumber % 10 == 9 ? 1 : 2;
            int seed = seedOverride ?? DeterministicSeed(levelNumber);
            int shuffleDepth = 12 + Math.Min(70, levelNumber / 2);

            for (int attempt = 0; attempt < 24; attempt++)
            {
                var random = new Random(seed + attempt * 7919);
                var clouds = new List<CloudState>();
                for (int i = 0; i < typeCount; i++)
                    clouds.Add(new CloudState(capacity, Filled(WeatherOrder[i], capacity)));
                for (int i = 0; i < emptyCount; i++)
                    clouds.Add(new CloudState(capacity, Array.Empty<WeatherType>()));

                var solution = new List<Move>();
                for (int step = 0; step < shuffleDepth; step++)
                {
                    if (!TryReverseStep(clouds, random, out Move inverse)) continue;
                    solution.Insert(0, inverse);
                }

                var definition = FromState(levelNumber, seed, capacity, clouds, solution, shuffleDepth);
                if (PuzzleValidator.Validate(definition, out _)) return definition;
            }

            throw new InvalidOperationException($"Could not generate valid level {levelNumber} with seed {seed}.");
        }

        public static int DeterministicSeed(int levelNumber)
        {
            unchecked { return 0x43D0A + levelNumber * 73856093; }
        }

        private static bool TryReverseStep(IReadOnlyList<CloudState> clouds, Random random, out Move inverse)
        {
            for (int attempt = 0; attempt < 40; attempt++)
            {
                int sourceIndex = random.Next(clouds.Count);
                int targetIndex = random.Next(clouds.Count);
                if (sourceIndex == targetIndex) continue;
                CloudState source = clouds[sourceIndex];
                CloudState target = clouds[targetIndex];
                if (source.IsEmpty || target.IsFull) continue;

                int group = source.TopGroupCount();
                int max = Math.Min(group, target.Capacity - target.Count);
                if (max <= 0) continue;
                int count = random.Next(1, max + 1);
                if (count == group && count < source.Count) count--;
                if (count <= 0) continue;

                WeatherType weather = source.Top;
                source.RemoveTop(count);
                target.Add(weather, count);
                inverse = new Move(targetIndex, sourceIndex, count, weather);
                return true;
            }

            inverse = default;
            return false;
        }

        private static LevelDefinition FromState(int level, int seed, int capacity, IReadOnlyList<CloudState> clouds, List<Move> solution, int depth)
        {
            var arrays = new WeatherType[clouds.Count][];
            for (int i = 0; i < clouds.Count; i++) arrays[i] = clouds[i].ToArray();
            var definition = new LevelDefinition(level, WorldId(level), capacity, null, arrays)
            {
                seed = seed,
                generated = true,
                knownSolution = solution.ToArray()
            };
            definition.difficultyScore = DifficultyCalculator.Calculate(new PuzzleState(ToStates(definition)), depth);
            return definition;
        }

        private static IEnumerable<CloudState> ToStates(LevelDefinition definition)
        {
            foreach (CloudDefinition cloud in definition.clouds)
                yield return new CloudState(definition.capacity, cloud.elements);
        }

        private static WeatherType[] Filled(WeatherType weather, int capacity)
        {
            var result = new WeatherType[capacity];
            for (int i = 0; i < capacity; i++) result[i] = weather;
            return result;
        }

        private static string WorldId(int level) => level <= 20 ? "green-valley" : level <= 40 ? "rainy-coast" :
            level <= 60 ? "frost-peaks" : level <= 80 ? "sun-desert" : level <= 100 ? "storm-islands" :
            level <= 125 ? "blossom-land" : level <= 150 ? "ember-valley" : level <= 175 ? "moon-garden" :
            level <= 200 ? "sky-kingdom" : level <= 250 ? "ocean-world" : level <= 300 ? "aurora-world" :
            level <= 350 ? "cosmic-world" : level <= 400 ? "celestial-rift" : "endless-skies";
    }
}
