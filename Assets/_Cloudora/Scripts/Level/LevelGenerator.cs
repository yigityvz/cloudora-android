using System;
using System.Collections.Generic;
using Cloudora.Core;
using Cloudora.Puzzle;
using Cloudora.Modifiers;

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
            if (levelNumber >= 41 && levelNumber <= 80) emptyCount = Math.Max(2, emptyCount) + 1;
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

                if (levelNumber >= 101)
                    clouds[0].SetAt(capacity - 1, WeatherType.Rainbow);

                var solution = new List<Move>();
                int usableCloudCount = levelNumber >= 41 && levelNumber <= 80 ? clouds.Count - 1 : clouds.Count;
                for (int step = 0; step < shuffleDepth; step++)
                {
                    if (!TryReverseStep(clouds, usableCloudCount, random, out Move inverse)) continue;
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

        private static bool TryReverseStep(IReadOnlyList<CloudState> clouds, int usableCloudCount, Random random, out Move inverse)
        {
            for (int attempt = 0; attempt < 40; attempt++)
            {
                int sourceIndex = random.Next(usableCloudCount);
                int targetIndex = random.Next(usableCloudCount);
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
            definition.modifiers = ModifiersFor(level, clouds.Count);
            definition.difficultyScore = DifficultyCalculator.Calculate(new PuzzleState(ToStates(definition)), depth);
            return definition;
        }

        private static ModifierData[] ModifiersFor(int level, int cloudCount)
        {
            if (level >= 41 && level <= 60)
                return new[] { new ModifierData { type = ModifierType.Frozen, cloudIndex = cloudCount - 1, counter = 2 } };
            if (level >= 61 && level <= 80)
                return new[] { new ModifierData { type = ModifierType.Locked, cloudIndex = cloudCount - 1, parameter = 0 } };
            if (level >= 81 && level <= 100)
                return new[] { new ModifierData { type = ModifierType.Fog, cloudIndex = 0, counter = 1 } };
            if (level >= 101 && level <= 150)
                return new[] { new ModifierData { type = ModifierType.Rainbow, cloudIndex = 0 } };
            if (level >= 151 && level <= 175)
                return new[] { new ModifierData { type = ModifierType.Night, cloudIndex = 0 } };
            if (level >= 176 && level <= 200)
                return new[] { new ModifierData { type = ModifierType.Wind, cloudIndex = 0 } };
            if (level > 200)
                return new[]
                {
                    new ModifierData { type = level % 2 == 0 ? ModifierType.Fog : ModifierType.Night, cloudIndex = 0, counter = 1 },
                    new ModifierData { type = level % 3 == 0 ? ModifierType.Rainbow : ModifierType.Wind, cloudIndex = Math.Min(1, cloudCount - 1) }
                };
            return Array.Empty<ModifierData>();
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
