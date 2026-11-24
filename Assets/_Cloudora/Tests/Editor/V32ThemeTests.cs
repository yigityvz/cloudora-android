using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Level;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // Historical calibration plan; current published palette is checked by MoonRolloutTests.
    // Faz 2.2 (V3_SABLON.md): 31–60 (Tema 2) — 4. renk (Rüzgar) 31'de, dizilim yumuşak başlar (0 açık) ve tema boyunca
    // 1 açık (~41) ve klasik (~51) adımlarıyla karışık olarak zorlaşır; hedefler şablonun eğrisinden.
    public sealed class V32ThemeTests
    {
        private static IReadOnlyList<V3LevelResult> _theme2;
        private static IReadOnlyList<V3LevelResult> Theme2 => _theme2 ??= V3LevelBuilder.BuildPart(V3Part.Theme2);

        // ---------- Hedef eğrisi (şablon, Faz 1.3) ----------

        [Test]
        public void TargetsFollowTheTemplateCurve()
        {
            IReadOnlyList<float> t = V3ThemeBuilder.TargetsFor(V3Part.Theme2);
            float T(int level) => t[level - 31];
            Assert.That(t.Count, Is.EqualTo(30));
            Assert.That(T(31), Is.EqualTo(20.0f).Within(1e-3f), "first Medium");
            Assert.That(T(59), Is.EqualTo(24.0f).Within(1e-3f), "last Medium");
            Assert.That(T(33), Is.EqualTo(22.5f).Within(1e-3f), "first Hard");
            Assert.That(T(58), Is.EqualTo(26.5f).Within(1e-3f), "last Hard");
            Assert.That(T(35), Is.EqualTo(25.5f).Within(1e-3f), "first Very Hard");
            Assert.That(T(60), Is.EqualTo(28.5f).Within(1e-3f), "finale");
        }

        [Test]
        public void CurveContinuesFromTransitionWithoutJump()
        {
            // 31'in Orta'sı 16–30'un bütün Orta'larından zor, ama 30'un puanının ×1,10'unu aşmaz.
            float transitionMediumMax = Enumerable.Range(16, 15).Select(V3LevelData.Find).Where(e => e.Band == DifficultyBand.Medium).Max(e => e.Score);
            float level30 = V3LevelData.Find(30).Score;
            float first = V3ThemeBuilder.TargetsFor(V3Part.Theme2)[0];
            Assert.That(first, Is.GreaterThan(transitionMediumMax));
            Assert.That(first, Is.LessThanOrEqualTo(level30 * 1.10f));
        }

        // ---------- Çerçeve ve yenilikler ----------

        [Test]
        public void Theme2UsesFourColorsCapacityFiveAndTwoEmpties()
        {
            Assert.That(Theme2.Select(r => r.Level), Is.EqualTo(Enumerable.Range(31, 30)));
            foreach (V3LevelResult r in Theme2)
            {
                Assert.That(r.Frame.ColorCount, Is.EqualTo(4), $"level {r.Level}");
                Assert.That(r.Frame.Capacity, Is.EqualTo(5), $"level {r.Level}");
                Assert.That(r.Frame.EmptyCount, Is.InRange(1, 2), $"level {r.Level}");
                Assert.That(r.Band, Is.EqualTo(V3LevelPlan.BandFor(r.Level)));
                Assert.That(r.Primary.Result.Status, Is.EqualTo(SolveStatus.Solved));
            }
        }

        [Test]
        public void NewColorAndLayoutStepsComeAloneOnMediumLevels()
        {
            List<V3LevelResult> novel = Theme2.Where(r => r.Novelty != V3Novelty.None).ToList();
            Assert.That(novel.Select(r => r.Novelty), Is.EquivalentTo(new[] { V3Novelty.NewColor, V3Novelty.OneOpenLayout, V3Novelty.ClassicLayout, V3Novelty.FewerEmpty }));
            Assert.That(novel.Single(r => r.Novelty == V3Novelty.NewColor).Level, Is.EqualTo(31));
            foreach (V3LevelResult r in novel) Assert.That(r.Band, Is.EqualTo(DifficultyBand.Medium), $"{r.Novelty} at {r.Level}");
            int oneOpen = novel.Single(r => r.Novelty == V3Novelty.OneOpenLayout).Level;
            int classic = novel.Single(r => r.Novelty == V3Novelty.ClassicLayout).Level;
            Assert.That(oneOpen, Is.InRange(39, 44), "first 1-open layout around 41");
            Assert.That(classic, Is.InRange(49, 54), "first classic layout around 51");
        }

        [Test]
        public void LayoutStartsSoftAndHardensGraduallyWithMixing()
        {
            int oneOpen = Theme2.Single(r => r.Novelty == V3Novelty.OneOpenLayout).Level;
            int classic = Theme2.Single(r => r.Novelty == V3Novelty.ClassicLayout).Level;
            Assert.That(Theme2.Where(r => r.Level < oneOpen).All(r => r.Frame.OpenClouds == 0), Is.True, "soft start: no fully empty cloud");
            Assert.That(Theme2.Where(r => r.Level < classic).All(r => !r.Frame.IsPacked), Is.True, "no classic before its step");
            Assert.That(Theme2.Single(r => r.Level == oneOpen).Frame.OpenClouds, Is.EqualTo(1));
            Assert.That(Theme2.Single(r => r.Level == classic).Frame.IsPacked, Is.True);
            // Karışık geçiş: yeni dizilim sonraki her döngüde görünür; eski dizilimler de kaybolmaz.
            foreach (IGrouping<int, V3LevelResult> cycle in Theme2.Where(r => r.Level > oneOpen).GroupBy(r => (r.Level - 1) / 5).Where(c => c.Min(r => r.Level) > oneOpen))
                Assert.That(cycle.Any(r => r.Frame.OpenClouds >= 1), Is.True, $"cycle {cycle.Key}: 1-open or harder appears");
            foreach (IGrouping<int, V3LevelResult> cycle in Theme2.GroupBy(r => (r.Level - 1) / 5).Where(c => c.Min(r => r.Level) > classic))
                Assert.That(cycle.Any(r => r.Frame.IsPacked), Is.True, $"cycle {cycle.Key}: classic appears");
            Assert.That(Theme2.Where(r => r.Level > classic).Any(r => !r.Frame.IsPacked), Is.True, "softer layouts remain in the mix");
        }

        [Test]
        public void TrapAndPlanningLevelsMixAfterTheEmptyCloudBreathes()
        {
            // Kullanıcı (18:10): 31–59 yalnızca "dikkat/planlama" ekseninde kalmasın, "çıkmaz ve takılma" ile karışık olsun.
            // Yeni renkle boş bulut nefes alır (31–35 hep 2 boş); 1 boş bir Orta'da yenilik olarak geri döner; sonraki her
            // döngüde 1 tuzak bölümü (1 boş) + 4 planlama bölümü (2 boş); final ayrıca "sınav".
            Assert.That(Theme2.Where(r => r.Level <= 35).All(r => r.Frame.EmptyCount == 2), Is.True, "empty cloud breathes with the new color");
            V3LevelResult back = Theme2.Single(r => r.Novelty == V3Novelty.FewerEmpty);
            Assert.That(back.Frame.EmptyCount, Is.EqualTo(1));
            Assert.That(back.Level, Is.InRange(36, 44));
            Assert.That(Theme2.Where(r => r.Level < back.Level).All(r => r.Frame.EmptyCount == 2), Is.True);
            foreach (IGrouping<int, V3LevelResult> cycle in Theme2.Where(r => r.Level != 60).GroupBy(r => (r.Level - 1) / 5).Where(c => c.Min(r => r.Level) > back.Level))
                Assert.That(cycle.Count(r => r.Frame.EmptyCount == 1), Is.EqualTo(1), $"cycle {cycle.Key}: one trap level");
            List<V3LevelResult> traps = Theme2.Where(r => r.Frame.EmptyCount == 1 && r.Level != 60).ToList();
            Assert.That(traps.Select(r => r.Band).Distinct().Count(), Is.GreaterThanOrEqualTo(2), "trap levels are not tied to one band");
            Assert.That(traps.All(r => r.Primary.Result.Metrics.DeadStateRatio > 0), Is.True, "trap levels really have dead ends");
        }

        // ---------- Zorluk ----------

        [Test]
        public void PatternCycleOrderAndSameBandNeverEasier()
        {
            foreach (IGrouping<int, V3LevelResult> cycle in Theme2.GroupBy(r => (r.Level - 1) / 5))
            {
                float medium = cycle.Where(r => r.Band == DifficultyBand.Medium).Max(r => r.Primary.Score);
                float hard = cycle.Single(r => r.Band == DifficultyBand.Hard).Primary.Score;
                float veryHard = cycle.Single(r => r.Band == DifficultyBand.VeryHard).Primary.Score;
                Assert.That(hard, Is.GreaterThan(medium), $"cycle {cycle.Key}");
                Assert.That(veryHard, Is.GreaterThan(hard), $"cycle {cycle.Key}");
            }
            foreach (IGrouping<DifficultyBand, V3LevelResult> band in Theme2.GroupBy(r => r.Band))
            {
                float[] s = band.OrderBy(r => r.Level).Select(r => r.Primary.Score).ToArray();
                for (int i = 1; i < s.Length; i++) Assert.That(s[i], Is.GreaterThanOrEqualTo(s[i - 1]), $"{band.Key} #{i}");
            }
        }

        [Test]
        public void ScoresStayCloseToTargetsAndLengthsAreSane()
        {
            foreach (V3LevelResult r in Theme2)
            {
                Assert.That(Math.Abs(r.Primary.Score - r.Target), Is.LessThanOrEqualTo(V3LadderBuilder.TargetTolerance), $"level {r.Level}");
                Assert.That(r.Primary.Result.Metrics.OptimalMoves, Is.LessThanOrEqualTo(V3LadderBuilder.LongMovesThreshold(r.Frame)), $"level {r.Level}");
            }
        }

        [Test]
        public void NoMovesIsOccasionalAcrossBandsAndOnlyTheFinaleLocks()
        {
            List<V3LevelResult> withNoMoves = Theme2.Where(r => r.Primary.Result.Metrics.NoMovesDepth >= 0).ToList();
            Assert.That(withNoMoves.Count, Is.InRange(V3ThemeBuilder.MinNoMoves(V3Part.Theme2), V3ThemeBuilder.MaxNoMoves(V3Part.Theme2)), string.Join(",", withNoMoves.Select(r => r.Level)));
            Assert.That(withNoMoves.Select(r => r.Band).Distinct().Count(), Is.GreaterThanOrEqualTo(2));
            Assert.That(withNoMoves.Count(r => r.Primary.Result.Metrics.NoMovesDepth == 1), Is.LessThanOrEqualTo(1));
            foreach (V3LevelResult r in Theme2.Where(r => r.Level != 60))
                Assert.That(r.Primary.Result.Metrics.NaiveFailureRate, Is.LessThan(V3LadderBuilder.MaxNaiveFailureBeforeFinale), $"level {r.Level}");
        }

        [Test]
        public void FinaleIsTheHardestTrapOfTheTheme()
        {
            // Tema finali "sınav": tuzağı (sanal oyuncu takılması + çıkmaz) parçadaki bütün bölümlerden fazla.
            float Trap(V3LevelResult r) => r.Primary.Result.Metrics.NaiveFailureRate + r.Primary.Result.Metrics.DeadStateRatio;
            V3LevelResult finale = Theme2.Last();
            Assert.That(Trap(finale), Is.GreaterThan(Theme2.Where(r => r.Level != 60).Max(Trap)));
            // Kullanıcı (18:42): 8 hamlelik final 59'dan kolay geldi → final en az çerçevesinin ortanca uzunluğunda.
            Assert.That(finale.Primary.Result.Metrics.OptimalMoves, Is.GreaterThanOrEqualTo(V3ThemeBuilder.MedianMoves(finale.Frame)));
        }

        [Test]
        public void BoardsAreUniqueValidAndDeterministic()
        {
            var keys = Enumerable.Range(1, 30).Select(V3LevelData.Find).Select(e => PuzzleSolver.CanonicalKey(e.CreateBoard(), e.Capacity)).ToList();
            keys.AddRange(Theme2.Select(r => r.Primary.CanonicalKey));
            Assert.That(keys.Distinct().Count(), Is.EqualTo(keys.Count));
            IReadOnlyList<V3LevelResult> again = V3ThemeBuilder.BuildPart(V3Part.Theme2);
            Assert.That(again.Select(r => r.Primary.Seed), Is.EqualTo(Theme2.Select(r => r.Primary.Seed)));
            foreach (V3LevelResult r in Theme2)
                Assert.That(PuzzleValidator.Validate(r.Primary.ToLevelDefinition(r.Level, "theme-2", null), out string error), Is.True, $"level {r.Level}: {error}");
        }

        [Test]
        public void TransitionPlanIsUntouched()
        {
            // 16–30 kullanıcı testinden geçti (17:56): Tema 2 eklenmesi onu değiştirmemeli.
            IReadOnlyList<V3LevelResult> transition = V3LevelBuilder.BuildPart(V3Part.TransitionTheme);
            foreach (V3LevelResult r in transition)
                Assert.That(r.Primary.Seed, Is.EqualTo(V3LevelData.Find(r.Level).Seed), $"level {r.Level}");
        }

        [Test]
        public void TestedPlanChangesOnlyAtLevel59()
        {
            // Kullanıcı (19:15): 59 hedefine yaklaşsın (Tema 3'ün Orta'ları Tema 2'ninkilerden kolay kalmasın), başka bölüm oynamasın.
            int[] tested = { 28, 340, 82, 43, 275, 244, 4, 203, 320, 17, 195, 271, 157, 54, 270, 324, 124, 231, 157, 284,
                             324, 97, 71, 234, 387, 239, 211, 332, 152, 305 };   // 31–60, 18:21 / 19:00
            foreach (V3LevelResult r in Theme2.Where(r => r.Level != 59))
                Assert.That(r.Primary.Seed, Is.EqualTo(tested[r.Level - 31]), $"level {r.Level}");
            V3LevelResult l59 = Theme2.Single(r => r.Level == 59);
            Assert.That(l59.Frame.EmptyCount, Is.EqualTo(1), "59 stays the cycle's trap");
            Assert.That(Math.Abs(l59.Primary.Score - l59.Target), Is.LessThanOrEqualTo(V3LadderBuilder.TargetWindow));
        }
    }
}
