using Cloudora.Puzzle;
using Cloudora.Level;
using Cloudora.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Tests.Editor
{
    public sealed class PuzzleFeelTests
    {
        [Test]
        public void GroupMoveAndSnapshotRestoreRemainConsistent()
        {
            CloudContainerView source = CreateCloud("Source", WeatherType.Rain, WeatherType.Sun, WeatherType.Sun);
            CloudContainerView target = CreateCloud("Target");
            WeatherType[] snapshot = source.CaptureElements();

            Assert.That(source.TopGroupCount, Is.EqualTo(2));
            Assert.That(source.TryMoveTopGroupTo(target), Is.True);
            Assert.That(source.ElementCount, Is.EqualTo(1));
            Assert.That(target.ElementCount, Is.EqualTo(2));

            source.RestoreElements(snapshot);
            target.RestoreElements(System.Array.Empty<WeatherType>());
            Assert.That(source.CaptureElements(), Is.EqualTo(snapshot));
            Assert.That(target.IsEmpty, Is.True);

            Object.DestroyImmediate(source.gameObject);
            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void InvalidDestinationDoesNotMutateState()
        {
            CloudContainerView source = CreateCloud("Source", WeatherType.Sun);
            CloudContainerView target = CreateCloud("Target", WeatherType.Rain);

            Assert.That(source.TryMoveTopGroupTo(target), Is.False);
            Assert.That(source.CaptureElements(), Is.EqualTo(new[] { WeatherType.Sun }));
            Assert.That(target.CaptureElements(), Is.EqualTo(new[] { WeatherType.Rain }));

            Object.DestroyImmediate(source.gameObject);
            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void AuthoredTutorialLevelsHaveValidElementCounts()
        {
            Assert.That(AuthoredLevelCatalog.Count, Is.EqualTo(15));
            for (int level = 1; level <= AuthoredLevelCatalog.Count; level++)
            {
                LevelDefinition definition = AuthoredLevelCatalog.Get(level);
                Assert.That(definition.levelId, Is.EqualTo(level));
                Assert.That(definition.capacity, Is.InRange(4, 7));

                var counts = new System.Collections.Generic.Dictionary<WeatherType, int>();
                foreach (CloudDefinition cloud in definition.clouds)
                {
                    Assert.That(cloud.elements.Length, Is.LessThanOrEqualTo(definition.capacity));
                    foreach (WeatherType weather in cloud.elements)
                    {
                        counts[weather] = counts.TryGetValue(weather, out int count) ? count + 1 : 1;
                    }
                }

                foreach (int count in counts.Values)
                {
                    Assert.That(count, Is.EqualTo(definition.capacity), $"Level {level} has invalid weather totals");
                }
            }
        }

        [Test]
        public void GeneratedLevelsAreDeterministicAndKnownSolvable()
        {
            for (int level = 16; level < 516; level++)
            {
                LevelDefinition first = LevelGenerator.Generate(level);
                LevelDefinition second = LevelGenerator.Generate(level);
                Assert.That(PuzzleValidator.Validate(first, out string error), Is.True, $"Level {level}: {error}");
                Assert.That(first.seed, Is.EqualTo(second.seed));
                Assert.That(BoardKey(first), Is.EqualTo(BoardKey(second)), $"Level {level} was not deterministic");
                Assert.That(first.difficultyScore, Is.GreaterThan(0f));
            }
        }

        [Test]
        public void PureRulesApplyGroupedMoveAndDetectSolved()
        {
            var state = new PuzzleState(new[]
            {
                new CloudState(4, new[] { WeatherType.Sun, WeatherType.Sun }),
                new CloudState(4, new[] { WeatherType.Sun, WeatherType.Sun }),
                new CloudState(4, System.Array.Empty<WeatherType>())
            });

            Assert.That(PuzzleRules.TryApply(state, new Move(0, 1, 2, WeatherType.Sun)), Is.True);
            Assert.That(PuzzleRules.IsSolved(state), Is.True);
        }

        private static string BoardKey(LevelDefinition definition)
        {
            var parts = new System.Collections.Generic.List<string>();
            foreach (CloudDefinition cloud in definition.clouds)
                parts.Add(string.Join(",", cloud.elements));
            return string.Join("|", parts);
        }

        private static CloudContainerView CreateCloud(string name, params WeatherType[] elements)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            CloudContainerView view = go.AddComponent<CloudContainerView>();
            view.Initialize(4, elements, _ => { });
            return view;
        }
    }
}
