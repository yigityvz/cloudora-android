using Cloudora.Puzzle;
using Cloudora.Level;
using Cloudora.Core;
using Cloudora.UI;
using Cloudora.Progression;
using Cloudora.Modifiers;
using Cloudora.Services;
using Cloudora.Editor;
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
            for (int level = 16; level < 2016; level++)
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

        [Test]
        public void PureRulesRejectPartialGroupMoveThatPlayerCannotChoose()
        {
            var state = new PuzzleState(new[]
            {
                new CloudState(4, new[] { WeatherType.Rain, WeatherType.Sun, WeatherType.Sun }),
                new CloudState(4, System.Array.Empty<WeatherType>())
            });

            Assert.That(PuzzleRules.CanApply(state, new Move(0, 1, 1, WeatherType.Sun)), Is.False);
            Assert.That(PuzzleRules.CanApply(state, new Move(0, 1, 2, WeatherType.Sun)), Is.True);
        }

        [Test]
        public void WindMovesOneElementFromModifiedCloud()
        {
            CloudContainerView source = CreateCloud("WindSource", WeatherType.Rain, WeatherType.Sun, WeatherType.Sun);
            CloudContainerView target = CreateCloud("WindTarget");
            var definition = new LevelDefinition(180, "aurora-peaks", 4, string.Empty,
                source.CaptureElements(), target.CaptureElements());
            definition.modifiers = new[] { new ModifierData { type = ModifierType.Wind, cloudIndex = 0 } };
            var runtime = new ModifierRuntime();
            runtime.Initialize(definition, new[] { source, target });

            int count = runtime.AdjustMoveCount(0, 1, source.TopGroupCount);
            Assert.That(count, Is.EqualTo(1));
            Assert.That(source.TryMoveTopGroupTo(target, count), Is.True);
            Assert.That(source.ElementCount, Is.EqualTo(2));
            Assert.That(target.ElementCount, Is.EqualTo(1));
            Assert.That(runtime.AdjustMoveCount(1, 0, 3), Is.EqualTo(3));

            Object.DestroyImmediate(source.gameObject);
            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void SafeAreaAnchorsRespectCutoutInsets()
        {
            (Vector2 min, Vector2 max) = SafeAreaFitter.CalculateAnchors(new Rect(0f, 80f, 1080f, 2240f), new Vector2(1080f, 2400f));
            Assert.That(min.x, Is.EqualTo(0f));
            Assert.That(min.y, Is.EqualTo(80f / 2400f).Within(0.0001f));
            Assert.That(max.x, Is.EqualTo(1f));
            Assert.That(max.y, Is.EqualTo(2320f / 2400f).Within(0.0001f));
        }

        [Test]
        public void StartupRepairsInvisibleRootCanvas()
        {
            var root = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            Canvas canvas = root.GetComponent<Canvas>();
            root.transform.localScale = Vector3.zero;

            Assert.That(MobileRuntimeBootstrap.EnsureCanvasVisible(canvas), Is.True);
            Assert.That(root.transform.localScale, Is.EqualTo(Vector3.one));
            Assert.That(MobileRuntimeBootstrap.EnsureCanvasVisible(canvas), Is.False);

            Object.DestroyImmediate(root);
        }

        [Test]
        public void ManifestFilterRemovesOnlyInternetPermission()
        {
            const string manifest = "<manifest xmlns:android='http://schemas.android.com/apk/res/android'><uses-permission android:name='android.permission.INTERNET'/><uses-permission android:name='android.permission.VIBRATE'/><application/></manifest>";
            string filtered = CloudoraAndroidManifestPostprocessor.RemoveInternetPermission(manifest);
            Assert.That(filtered, Does.Not.Contain("android.permission.INTERNET"));
            Assert.That(filtered, Does.Contain("android.permission.VIBRATE"));
        }

        private static string BoardKey(LevelDefinition definition)
        {
            var parts = new System.Collections.Generic.List<string>();
            foreach (CloudDefinition cloud in definition.clouds)
                parts.Add(string.Join(",", cloud.elements));
            return string.Join("|", parts);
        }

        [TestCase(1080, 1920, 5, 4)]
        [TestCase(1080, 2400, 8, 5)]
        [TestCase(1440, 3200, 10, 7)]
        [TestCase(720, 1280, 12, 6)]
        public void AdaptiveLayoutFitsPortraitBounds(float width, float height, int clouds, int capacity)
        {
            var available = new Vector2(width * 0.9f, height * 0.62f);
            LayoutMetrics metrics = AdaptiveBoardLayout.Calculate(available, clouds, capacity);
            int rows = Mathf.CeilToInt(clouds / (float)metrics.Columns);
            float usedWidth = metrics.CellSize.x * metrics.Columns + metrics.Spacing.x * (metrics.Columns - 1);
            float usedHeight = metrics.CellSize.y * rows + metrics.Spacing.y * (rows - 1);
            Assert.That(usedWidth, Is.LessThanOrEqualTo(available.x + 0.1f));
            Assert.That(usedHeight, Is.LessThanOrEqualTo(available.y + 0.1f));
            Assert.That(metrics.CellSize.x, Is.GreaterThanOrEqualTo(80f));
        }

        [Test]
        public void WorldProgressionCrossesBoundaryAndRestoresInStages()
        {
            WorldDefinition valley = WorldCatalog.ForLevel(1);
            Assert.That(RestorationManager.GetStage(valley, 5), Is.EqualTo(1));
            Assert.That(RestorationManager.GetStage(valley, 20), Is.EqualTo(4));
            Assert.That(RestorationManager.GetProgress(valley, 20), Is.EqualTo(1f));

            var progression = new ProgressionManager(20, 19);
            progression.CompleteCurrentLevel();
            progression.Advance();
            Assert.That(progression.CurrentLevel, Is.EqualTo(21));
            Assert.That(progression.CurrentWorld.Id, Is.EqualTo("rainy-coast"));
        }

        [Test]
        public void CompletedLevelResumesAtNextLevelWithoutAdvancingActiveScreen()
        {
            var progression = new ProgressionManager(20, 19);
            Assert.That(progression.ResumeLevel(false), Is.EqualTo(20));
            progression.CompleteCurrentLevel();
            Assert.That(progression.CurrentLevel, Is.EqualTo(20));
            Assert.That(progression.ResumeLevel(true), Is.EqualTo(21));
            progression.Advance();
            Assert.That(progression.CurrentLevel, Is.EqualTo(21));
            Assert.That(progression.ResumeLevel(false), Is.EqualTo(21));
        }

        [Test]
        public void RainbowIsWildcardWithoutAmbiguousSolvedState()
        {
            var solved = new PuzzleState(new[]
            {
                new CloudState(4, new[] { WeatherType.Sun, WeatherType.Rainbow, WeatherType.Sun, WeatherType.Sun })
            });
            var mixed = new PuzzleState(new[]
            {
                new CloudState(4, new[] { WeatherType.Sun, WeatherType.Rainbow, WeatherType.Rain, WeatherType.Sun })
            });
            Assert.That(PuzzleRules.IsSolved(solved), Is.True);
            Assert.That(PuzzleRules.IsSolved(mixed), Is.False);
        }

        [TestCase(45, ModifierType.Frozen)]
        [TestCase(65, ModifierType.Locked)]
        [TestCase(85, ModifierType.Fog)]
        [TestCase(105, ModifierType.Rainbow)]
        [TestCase(155, ModifierType.Night)]
        [TestCase(180, ModifierType.Wind)]
        public void GeneratorIntroducesExpectedModifier(int level, ModifierType expected)
        {
            LevelDefinition definition = LevelGenerator.Generate(level);
            Assert.That(definition.modifiers, Has.Some.Matches<ModifierData>(m => m.type == expected));
            Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, error);
        }

        [Test]
        public void LivesRegenerateOfflineAndRespectCap()
        {
            System.DateTime start = new System.DateTime(2026, 1, 1, 10, 0, 0, System.DateTimeKind.Utc);
            var lives = new LifeManager(2, start.AddMinutes(30));
            lives.Refresh(start.AddMinutes(95));
            Assert.That(lives.Lives, Is.EqualTo(5));
            Assert.That(lives.NextLifeUtc, Is.EqualTo(default(System.DateTime)));
            Assert.That(lives.TryConsumeRetry(start.AddMinutes(96), false), Is.True);
            Assert.That(lives.Lives, Is.EqualTo(4));
        }

        [Test]
        public void TutorialRetryIsFreeAndBoostersCannotUnderflow()
        {
            var lives = new LifeManager(0);
            Assert.That(lives.TryConsumeRetry(System.DateTime.UtcNow, true), Is.True);
            Assert.That(lives.Lives, Is.EqualTo(0));
            var boosters = new BoosterManager(1, 0, 0);
            Assert.That(boosters.TryUseUndo(), Is.True);
            Assert.That(boosters.TryUseUndo(), Is.False);
            Assert.That(boosters.UndoCharges, Is.Zero);
        }

        [Test]
        public void ZeroLivesBlocksNewNormalAttemptButNotTutorialOrPaidCurrentAttempt()
        {
            System.DateTime now = new System.DateTime(2026, 1, 1, 10, 0, 0, System.DateTimeKind.Utc);
            var lives = new LifeManager(1);
            Assert.That(lives.CanBeginAttempt(now, false), Is.True);
            Assert.That(lives.TryConsumeRetry(now, false), Is.True);
            Assert.That(lives.Lives, Is.Zero);
            Assert.That(lives.CanBeginAttempt(now, false), Is.False);
            Assert.That(lives.CanBeginAttempt(now, true), Is.True);
            lives.Grant(1);
            Assert.That(lives.CanBeginAttempt(now, false), Is.True);
        }

        [Test]
        public void ModifierSnapshotAndExtraCloudPreserveDynamicCounters()
        {
            CloudContainerView first = CreateCloud("First", WeatherType.Sun);
            CloudContainerView second = CreateCloud("Second", WeatherType.Rain);
            CloudContainerView extra = CreateCloud("Extra");
            var definition = new LevelDefinition(85, "storm-islands", 4, string.Empty,
                new[] { WeatherType.Sun }, new[] { WeatherType.Rain });
            definition.modifiers = new[]
            {
                new ModifierData { type = ModifierType.Frozen, cloudIndex = 0, counter = 2 },
                new ModifierData { type = ModifierType.Fog, cloudIndex = 1, counter = 1 }
            };
            var runtime = new ModifierRuntime();
            runtime.Initialize(definition, new[] { first, second });
            ModifierData[] before = runtime.CaptureSnapshot();
            runtime.OnSuccessfulMove(1, 0);
            runtime.UpdateClouds(new[] { first, second, extra });
            ModifierData[] afterExtra = runtime.CaptureSnapshot();
            Assert.That(afterExtra[0].counter, Is.EqualTo(1));
            Assert.That(afterExtra[1].counter, Is.Zero);
            runtime.RestoreSnapshot(before);
            ModifierData[] restored = runtime.CaptureSnapshot();
            Assert.That(restored[0].counter, Is.EqualTo(2));
            Assert.That(restored[1].counter, Is.EqualTo(1));
            Object.DestroyImmediate(first.gameObject);
            Object.DestroyImmediate(second.gameObject);
            Object.DestroyImmediate(extra.gameObject);
        }

        [Test]
        public void SaveRoundTripsAndCorruptPrimaryFallsBackToBackup()
        {
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cloudora-save-test-" + System.Guid.NewGuid().ToString("N"));
            string path = System.IO.Path.Combine(directory, "save.json");
            var service = new LocalJsonSaveService(path);
            var first = SaveData.Defaults();
            first.currentLevel = 42;
            first.lives = 2;
            service.Save(first);
            first.currentLevel = 43;
            service.Save(first);

            SaveData loaded = service.Load();
            Assert.That(loaded.currentLevel, Is.EqualTo(43));
            System.IO.File.WriteAllText(path, "{broken json");
            loaded = service.Load();
            Assert.That(loaded.currentLevel, Is.EqualTo(42));
            Assert.That(loaded.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            System.IO.Directory.Delete(directory, true);
        }

        [Test]
        public void ValidSchemaSaveStillNormalizesUnsafeFields()
        {
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cloudora-save-normalize-" + System.Guid.NewGuid().ToString("N"));
            string path = System.IO.Path.Combine(directory, "save.json");
            System.IO.Directory.CreateDirectory(directory);
            System.IO.File.WriteAllText(path, "{\"schemaVersion\":2,\"currentLevel\":0,\"lives\":99,\"nextLifeUtcTicks\":9223372036854775807,\"undoCharges\":-4,\"extraCloudCharges\":-3,\"safeShuffleCharges\":-2,\"tutorialFlags\":[true]}" );

            SaveData loaded = new LocalJsonSaveService(path).Load();
            Assert.That(loaded.currentLevel, Is.EqualTo(1));
            Assert.That(loaded.lives, Is.EqualTo(LifeManager.MaxLives));
            Assert.That(loaded.nextLifeUtcTicks, Is.Zero);
            Assert.That(loaded.undoCharges, Is.Zero);
            Assert.That(loaded.extraCloudCharges, Is.Zero);
            Assert.That(loaded.safeShuffleCharges, Is.Zero);
            Assert.That(loaded.tutorialFlags.Length, Is.EqualTo(15));
            Assert.That(loaded.tutorialFlags[0], Is.True);
            System.IO.Directory.Delete(directory, true);
        }

        [Test]
        public void AnalyticsRejectsPersonalDataKeys()
        {
            var safe = new System.Collections.Generic.Dictionary<string, object> { ["level"] = 12, ["world"] = "green-valley" };
            Assert.DoesNotThrow(() => AnalyticsPrivacy.Validate(AnalyticsEvents.LevelStarted, safe));
            var unsafeValues = new System.Collections.Generic.Dictionary<string, object> { ["email_address"] = "hidden" };
            Assert.Throws<System.ArgumentException>(() => AnalyticsPrivacy.Validate(AnalyticsEvents.LevelStarted, unsafeValues));
        }

        [Test]
        public void AdPolicyProtectsEarlyLevelsAndFrequencyCaps()
        {
            System.DateTime now = new System.DateTime(2026, 1, 1, 10, 0, 0, System.DateTimeKind.Utc);
            var policy = new AdPlacementPolicy(System.TimeSpan.FromMinutes(5));
            for (int level = 1; level <= 5; level++) Assert.That(policy.RegisterCompletion(level, now), Is.False);
            Assert.That(policy.RegisterCompletion(6, now), Is.False);
            Assert.That(policy.RegisterCompletion(7, now), Is.False);
            Assert.That(policy.RegisterCompletion(8, now), Is.False);
            Assert.That(policy.RegisterCompletion(9, now), Is.True);
            for (int level = 10; level <= 13; level++) Assert.That(policy.RegisterCompletion(level, now.AddMinutes(1)), Is.False);
            Assert.That(policy.RegisterCompletion(14, now.AddMinutes(6)), Is.True);
            Assert.That(new AdMobAdService().IsAvailable, Is.False);
        }

        [Test]
        public void DifficultyWaveHasFinaleAndEasyWorldReset()
        {
            Assert.That(BalanceProfile.BandFor(20), Is.EqualTo(DifficultyBand.WorldFinale));
            Assert.That(BalanceProfile.BandFor(21), Is.EqualTo(DifficultyBand.Easy));
            Assert.That(BalanceProfile.BandFor(22), Is.EqualTo(DifficultyBand.Easy));
            Assert.That(BalanceProfile.ShuffleDepth(100), Is.GreaterThan(BalanceProfile.ShuffleDepth(101)));
            Assert.That(BalanceProfile.DurationTarget(10), Is.EqualTo((30, 60)));
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
