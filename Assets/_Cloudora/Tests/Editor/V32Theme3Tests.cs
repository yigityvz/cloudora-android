using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Level;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // Historical calibration plan; current published palette is checked by MoonRolloutTests.
    // Faz 2.2 (V3_SABLON.md, kullanıcı 18:45): 61–100 (Tema 3).
    // 61–80: 4 renk, yeni tema (görsel) ile dizilim nefes alır (0/1 açık), klasik ~71; tuzak bölümleri döngüde 1 → 2.
    // 81–100: Ay (5. renk) 81'de, dizilim 0 açıkla başlar, 1 açık ~86, klasik ~91; boş bulut 81–85 nefes alır,
    // 1 boş 86–89'da geri döner, sonra döngüde 2 tuzak. Final 100 "sınav".
    public sealed class V32Theme3Tests
    {
        private static IReadOnlyList<V3LevelResult> _theme3;
        private static IReadOnlyList<V3LevelResult> Theme3 => _theme3 ??= V3LevelBuilder.BuildPart(V3Part.Theme3);
        private static int Traps(IEnumerable<V3LevelResult> levels) => levels.Count(r => r.Frame.EmptyCount == 1);

        [Test]
        public void TargetsFollowTheTemplateCurve()
        {
            IReadOnlyList<float> t = V3ThemeBuilder.TargetsFor(V3Part.Theme3);
            float T(int level) => t[level - 61];
            Assert.That(t.Count, Is.EqualTo(40));
            Assert.That(T(61), Is.EqualTo(24.0f).Within(1e-3f));
            Assert.That(T(79), Is.EqualTo(26.0f).Within(1e-3f));
            Assert.That(T(80), Is.EqualTo(30.5f).Within(1e-3f));
            Assert.That(T(81), Is.EqualTo(26.5f).Within(1e-3f));
            Assert.That(T(98), Is.EqualTo(32.0f).Within(1e-3f));
            Assert.That(T(100), Is.EqualTo(34.0f).Within(1e-3f));
        }

        [Test]
        public void ColorsCapacityAndNovelties()
        {
            Assert.That(Theme3.Select(r => r.Level), Is.EqualTo(Enumerable.Range(61, 40)));
            foreach (V3LevelResult r in Theme3)
            {
                Assert.That(r.Frame.ColorCount, Is.EqualTo(r.Level <= 80 ? 4 : 5), $"level {r.Level}");
                Assert.That(r.Frame.Capacity, Is.EqualTo(5));
                Assert.That(r.Primary.Result.Status, Is.EqualTo(SolveStatus.Solved));
            }
            List<V3LevelResult> novel = Theme3.Where(r => r.Novelty != V3Novelty.None).ToList();
            Assert.That(novel.Select(r => r.Novelty), Is.EquivalentTo(new[]
                { V3Novelty.ClassicLayout, V3Novelty.NewColor, V3Novelty.OneOpenLayout, V3Novelty.ClassicLayout, V3Novelty.FewerEmpty }));
            foreach (V3LevelResult r in novel) Assert.That(r.Band, Is.EqualTo(DifficultyBand.Medium), $"{r.Novelty} at {r.Level}");
            Assert.That(novel.Single(r => r.Novelty == V3Novelty.NewColor).Level, Is.EqualTo(81));
            Assert.That(novel.Single(r => r.Novelty == V3Novelty.OneOpenLayout).Level, Is.InRange(84, 89));
            Assert.That(novel.Where(r => r.Novelty == V3Novelty.ClassicLayout).Select(r => r.Level).OrderBy(l => l).ToArray(),
                Has.Exactly(1).InRange(69, 74).And.Exactly(1).InRange(89, 94));
            Assert.That(novel.Single(r => r.Novelty == V3Novelty.FewerEmpty).Level, Is.InRange(86, 89));
        }

        [Test]
        public void LayoutBreathesWithTheNewThemeAndTheNewColor()
        {
            int classicA = Theme3.Single(r => r.Novelty == V3Novelty.ClassicLayout && r.Level <= 80).Level;
            Assert.That(Theme3.Where(r => r.Level < classicA).All(r => !r.Frame.IsPacked), Is.True, "61: soft (0/1 open) start");
            int oneOpenB = Theme3.Single(r => r.Novelty == V3Novelty.OneOpenLayout).Level;
            Assert.That(Theme3.Where(r => r.Level >= 81 && r.Level < oneOpenB).All(r => r.Frame.OpenClouds == 0), Is.True, "81: 0-open start");
        }

        [Test]
        public void TrapLevelsBecomeMoreFrequentAndBreatheWithTheNewColor()
        {
            // Kullanıcı (18:45): tuzak bölümleri oyun ilerledikçe sıklaşsın; tuzakların çoğunda takılmadı.
            int[] PerCycle(int from) => Enumerable.Range(0, 4).Select(c => Traps(Theme3.Where(r =>
                r.Level >= from + c * 5 && r.Level < from + c * 5 + 5 && r.Level != 100))).ToArray();
            Assert.That(PerCycle(61), Is.EqualTo(new[] { 1, 1, 2, 2 }), "61–80");
            Assert.That(Theme3.Where(r => r.Level >= 81 && r.Level <= 85).All(r => r.Frame.EmptyCount == 2), Is.True, "empty cloud breathes with Moon");
            int[] b = PerCycle(81);
            Assert.That(b[0], Is.EqualTo(0));
            Assert.That(b[1], Is.EqualTo(1), "86–90: the return level only");
            Assert.That(b[2], Is.EqualTo(2));
            Assert.That(b[3], Is.EqualTo(2));
            Assert.That(Theme3.Where(r => r.Frame.EmptyCount == 1 && r.Level != 100).Select(r => r.Band).Distinct().Count(), Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public void SomeHardAndVeryHardLevelsAreTraps()
        {
            // Kullanıcı (19:00): Zor/Çok zor bölümlerin hepsine olmasa da bazılarına tuzak. 2 tuzaklı her döngüde biri
            // Zor ya da Çok zor (sırayla): 75 ÇZ, 78 Z, 95 ÇZ, 98 Z; geri kalan Zor/Çok zor planlama bölümü kalır.
            int[] hardTraps = Theme3.Where(r => r.Level != 100 && r.Band != DifficultyBand.Medium && r.Frame.EmptyCount == 1)
                .Select(r => r.Level).ToArray();
            Assert.That(hardTraps, Is.EquivalentTo(new[] { 75, 78, 95, 98 }));
            foreach (int level in hardTraps)
            {
                DifficultyMetrics m = Theme3.Single(r => r.Level == level).Primary.Result.Metrics;
                Assert.That(m.DeadStateRatio, Is.GreaterThan(0.1f), $"level {level}: a real trap");
            }
        }

        [Test]
        public void CycleOrderSameBandAndTargets()
        {
            foreach (IGrouping<int, V3LevelResult> cycle in Theme3.GroupBy(r => (r.Level - 1) / 5))
            {
                float medium = cycle.Where(r => r.Band == DifficultyBand.Medium).Max(r => r.Primary.Score);
                float hard = cycle.Single(r => r.Band == DifficultyBand.Hard).Primary.Score;
                float veryHard = cycle.Single(r => r.Band == DifficultyBand.VeryHard).Primary.Score;
                Assert.That(hard, Is.GreaterThan(medium), $"cycle {cycle.Key}");
                Assert.That(veryHard, Is.GreaterThan(hard), $"cycle {cycle.Key}");
            }
            foreach (IGrouping<DifficultyBand, V3LevelResult> band in Theme3.GroupBy(r => r.Band))
            {
                float[] s = band.OrderBy(r => r.Level).Select(r => r.Primary.Score).ToArray();
                for (int i = 1; i < s.Length; i++) Assert.That(s[i], Is.GreaterThanOrEqualTo(s[i - 1]), $"{band.Key} #{i}");
            }
            foreach (V3LevelResult r in Theme3)
            {
                Assert.That(Math.Abs(r.Primary.Score - r.Target), Is.LessThanOrEqualTo(V3LadderBuilder.TargetTolerance), $"level {r.Level}");
                Assert.That(r.Primary.Result.Metrics.OptimalMoves, Is.LessThanOrEqualTo(V3LadderBuilder.LongMovesThreshold(r.Frame)), $"level {r.Level}");
            }
        }

        [Test]
        public void FinaleIsALongEnoughTrapAndOnlyItLocks()
        {
            V3LevelResult finale = Theme3.Last();
            Assert.That(finale.Primary.Result.Metrics.OptimalMoves, Is.GreaterThanOrEqualTo(V3ThemeBuilder.MedianMoves(finale.Frame)));
            float Trap(V3LevelResult r) => r.Primary.Result.Metrics.NaiveFailureRate + r.Primary.Result.Metrics.DeadStateRatio;
            Assert.That(Trap(finale), Is.GreaterThan(Theme3.Where(r => r.Level != 100).Max(Trap)));
            foreach (V3LevelResult r in Theme3.Where(r => r.Level != 100))
                Assert.That(r.Primary.Result.Metrics.NaiveFailureRate, Is.LessThan(V3LadderBuilder.MaxNaiveFailureBeforeFinale), $"level {r.Level}");
        }

        [Test]
        public void NoMovesIsOccasionalAcrossBands()
        {
            List<V3LevelResult> withNoMoves = Theme3.Where(r => r.Primary.Result.Metrics.NoMovesDepth >= 0).ToList();
            Assert.That(withNoMoves.Count, Is.InRange(V3ThemeBuilder.MinNoMoves(V3Part.Theme3), V3ThemeBuilder.MaxNoMoves(V3Part.Theme3)), string.Join(",", withNoMoves.Select(r => r.Level)));
            Assert.That(withNoMoves.Select(r => r.Band).Distinct().Count(), Is.GreaterThanOrEqualTo(2));
            Assert.That(withNoMoves.Count(r => r.Primary.Result.Metrics.NoMovesDepth == 1), Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void BoardsAreUniqueValidAndDeterministic()
        {
            var keys = Enumerable.Range(1, 60).Select(V3LevelData.Find).Select(e => PuzzleSolver.CanonicalKey(e.CreateBoard(), e.Capacity)).ToList();
            keys.AddRange(Theme3.Select(r => r.Primary.CanonicalKey));
            Assert.That(keys.Distinct().Count(), Is.EqualTo(keys.Count));
            IReadOnlyList<V3LevelResult> again = V3ThemeBuilder.BuildPart(V3Part.Theme3);
            Assert.That(again.Select(r => r.Primary.Seed), Is.EqualTo(Theme3.Select(r => r.Primary.Seed)));
            foreach (V3LevelResult r in Theme3)
                Assert.That(PuzzleValidator.Validate(r.Primary.ToLevelDefinition(r.Level, "theme-3", null), out string error), Is.True, $"level {r.Level}: {error}");
        }
    }
}
