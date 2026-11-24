using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Level;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // v3.2: çerçeve merdiveni ile 16–30 (tasarım §11b–11e). Araç çerçeveyi zorluk hedefine göre seçer;
    // kapasite geri düşmez, bir bölümde tek yenilik olur ve yenilik ilk kez Orta'da gelir. 1–15 sabit kalır.
    public sealed class V32LadderTests
    {
        private static IReadOnlyList<V3LevelResult> _transition;
        private static IReadOnlyList<V3LevelResult> Transition => _transition ??= V3LevelBuilder.BuildPart(V3Part.TransitionTheme);

        private static IEnumerable<V3LevelEntry> Adaptation =>
            Enumerable.Range(6, 10).Select(V3LevelData.Find);

        // ---------- Hedef eğrisi ----------

        [Test]
        public void TargetsContinueSmoothlyFromAdaptation()
        {
            IReadOnlyList<float> targets = V3LadderBuilder.TargetsFor(V3Part.TransitionTheme);
            Assert.That(targets.Count, Is.EqualTo(15));
            float adaptationMediumMax = Adaptation.Where(e => e.Band == DifficultyBand.Medium).Max(e => e.Score);
            float level15 = V3LevelData.Find(15).Score;
            Assert.That(targets[0], Is.GreaterThan(adaptationMediumMax), "16 must be harder than every Medium of 6–15");
            Assert.That(targets[0], Is.LessThanOrEqualTo(level15 * 1.10f), "16 must not jump above 15");
        }

        [Test]
        public void TargetsRiseWithinEachBandAndKeepCycleOrder()
        {
            IReadOnlyList<float> targets = V3LadderBuilder.TargetsFor(V3Part.TransitionTheme);
            int[] levels = Enumerable.Range(16, 15).ToArray();
            foreach (IGrouping<DifficultyBand, int> band in levels.GroupBy(V3LevelPlan.BandFor))
            {
                float[] values = band.Select(l => targets[l - 16]).ToArray();
                for (int i = 1; i < values.Length; i++) Assert.That(values[i], Is.GreaterThan(values[i - 1]), $"{band.Key} #{i}");
            }
            foreach (IGrouping<int, int> cycle in levels.GroupBy(l => (l - 1) / 5))
            {
                float medium = cycle.Where(l => V3LevelPlan.BandFor(l) == DifficultyBand.Medium).Max(l => targets[l - 16]);
                float hard = targets[cycle.Single(l => V3LevelPlan.BandFor(l) == DifficultyBand.Hard) - 16];
                float veryHard = targets[cycle.Single(l => V3LevelPlan.BandFor(l) == DifficultyBand.VeryHard) - 16];
                Assert.That(hard, Is.GreaterThan(medium), $"cycle {cycle.Key}");
                Assert.That(veryHard, Is.GreaterThan(hard), $"cycle {cycle.Key}");
            }
        }

        [Test]
        public void VeryHardTargetsGrowFasterThanOtherBands()
        {
            // Tasarım §11i: Çok zor her döngüde 1,2, diğer bantlar 1,0 artar.
            IReadOnlyList<float> t = V3LadderBuilder.TargetsFor(V3Part.TransitionTheme);
            float T(int level) => t[level - 16];
            Assert.That(T(25) - T(20), Is.EqualTo(1.2f).Within(1e-4f));
            Assert.That(T(30) - T(25), Is.EqualTo(1.2f).Within(1e-4f));
            Assert.That(T(23) - T(18), Is.EqualTo(1.0f).Within(1e-4f));
            Assert.That(T(21) - T(16), Is.EqualTo(1.0f).Within(1e-4f));
        }

        // ---------- Kurulan bölümler ----------

        [Test]
        public void SolutionsNeverReachAbsurdLengths()
        {
            // Faz 2.1 (tasarım §11j): uzunluk tercihi kaldırıldı; hiçbir bölümün en kısa çözümü kendi çerçevesindeki
            // kullanılabilir tahtaların en uzun %10'luk dilimine girmez.
            foreach (V3LevelResult r in Transition)
                Assert.That(r.Primary.Result.Metrics.OptimalMoves, Is.LessThanOrEqualTo(V3LadderBuilder.LongMovesThreshold(r.Frame)),
                    $"level {r.Level} ({r.Frame})");
        }

        [Test]
        public void NoMovesAppearsSometimesAndAcrossBands()
        {
            // Tasarım §11i: No Moves zaman zaman (16–30'da 2–3 bölüm; yalnızca 3 bölüm 1 boşlu), hepsi aynı bantta değil.
            List<V3LevelResult> withNoMoves = Transition.Where(r => r.Primary.Result.Metrics.NoMovesDepth >= 0).ToList();
            Assert.That(withNoMoves.Count, Is.InRange(2, 3), string.Join(",", withNoMoves.Select(r => r.Level)));
            Assert.That(withNoMoves.Select(r => r.Band).Distinct().Count(), Is.GreaterThanOrEqualTo(2));
            // İlk hamlede No Moves en fazla 1 bölümde (Editor testinde üçünde de ilk hamlede düşüldü — sürpriz değildi).
            Assert.That(withNoMoves.Count(r => r.Primary.Result.Metrics.NoMovesDepth == 1), Is.LessThanOrEqualTo(1));
        }

        [Test]
        public void OnlyThePartFinaleMayLockThePlayer()
        {
            // Tasarım §11i (13:12): 30 gibi kilitleyen bölüm yalnızca tema finalinde; diğerlerinde sanal oyuncu takılması < 0,5
            // (Editor testi: 25 "normal" 0,395; 30 "kilitledi" 0,665).
            foreach (V3LevelResult r in Transition.Where(r => r.Level != 30))
                Assert.That(r.Primary.Result.Metrics.NaiveFailureRate, Is.LessThan(V3LadderBuilder.MaxNaiveFailureBeforeFinale), $"level {r.Level}");
        }

        [Test]
        public void VeryHardPrefersSingleEmptyButNotAlways()
        {
            List<V3LevelResult> veryHard = Transition.Where(r => r.Band == DifficultyBand.VeryHard).ToList();
            Assert.That(veryHard.Any(r => r.Frame.EmptyCount == 2), Is.True, "not every Very Hard may use a single empty cloud");
        }

        [Test]
        public void MisleadingFilterKeepsObviousBoardsOutOfHardBands()
        {
            // Kaba yanıltıcılık filtresi: hamlelerinin çoğu doğrudan ilerleten 2 boşlu tahta Zor/Çok zor olamaz.
            foreach (V3LevelResult r in Transition.Where(r => r.Band != DifficultyBand.Medium && r.Frame.EmptyCount == 2))
                Assert.That(r.Primary.Result.Metrics.ProgressMoveRatio, Is.LessThanOrEqualTo(V3LadderBuilder.MaxProgressForHard), $"level {r.Level}");
        }

        [Test]
        public void TransitionFollowsPatternAndCycleOrder()
        {
            Assert.That(Transition.Select(r => r.Level), Is.EqualTo(Enumerable.Range(16, 15)));
            foreach (V3LevelResult r in Transition)
            {
                Assert.That(r.Band, Is.EqualTo(V3LevelPlan.BandFor(r.Level)));
                Assert.That(r.Primary.Result.Status, Is.EqualTo(SolveStatus.Solved), $"level {r.Level}");
            }
            foreach (IGrouping<int, V3LevelResult> cycle in Transition.GroupBy(r => (r.Level - 1) / 5))
            {
                float medium = cycle.Where(r => r.Band == DifficultyBand.Medium).Max(r => r.Primary.Score);
                float hard = cycle.Single(r => r.Band == DifficultyBand.Hard).Primary.Score;
                float veryHard = cycle.Single(r => r.Band == DifficultyBand.VeryHard).Primary.Score;
                Assert.That(hard, Is.GreaterThan(medium), $"cycle {cycle.Key}");
                Assert.That(veryHard, Is.GreaterThan(hard), $"cycle {cycle.Key}");
            }
        }

        [Test]
        public void SameBandNeverGetsEasierAndStartIsSmooth()
        {
            foreach (IGrouping<DifficultyBand, V3LevelResult> band in Transition.GroupBy(r => r.Band))
            {
                float[] scores = band.OrderBy(r => r.Level).Select(r => r.Primary.Score).ToArray();
                for (int i = 1; i < scores.Length; i++) Assert.That(scores[i], Is.GreaterThanOrEqualTo(scores[i - 1]), $"{band.Key} #{i}");
            }
            float adaptationMediumMax = Adaptation.Where(e => e.Band == DifficultyBand.Medium).Max(e => e.Score);
            Assert.That(Transition.Where(r => r.Band == DifficultyBand.Medium).Min(r => r.Primary.Score), Is.GreaterThan(adaptationMediumMax));
            Assert.That(Transition[0].Primary.Score, Is.LessThanOrEqualTo(V3LevelData.Find(15).Score * 1.10f), "16 starts with a jump");
        }

        [Test]
        public void ScoresStayCloseToTargets()
        {
            foreach (V3LevelResult r in Transition)
                Assert.That(Math.Abs(r.Primary.Score - r.Target), Is.LessThanOrEqualTo(V3LadderBuilder.TargetTolerance), $"level {r.Level}");
        }

        // ---------- Çerçeve merdiveni ----------

        [Test]
        public void CapacityAndColorsNeverFallBack()
        {
            BoardFrame previous = V3LevelPlan.FrameFor(15);
            foreach (V3LevelResult r in Transition)
            {
                Assert.That(r.Frame.Capacity, Is.GreaterThanOrEqualTo(previous.Capacity), $"level {r.Level}");
                Assert.That(r.Frame.ColorCount, Is.GreaterThanOrEqualTo(previous.ColorCount), $"level {r.Level}");
                previous = r.Frame;
            }
        }

        [Test]
        public void PartReachesCapacityFiveAndSingleEmptyCloud()
        {
            Assert.That(Transition.Last().Frame.Capacity, Is.EqualTo(5));
            Assert.That(Transition.All(r => r.Frame.ColorCount == 3), Is.True, "16–30 keeps three weather types");
            Assert.That(Transition.Any(r => r.Frame.EmptyCount == 1), Is.True);
            Assert.That(Transition.All(r => r.Frame.EmptyCount is 1 or 2), Is.True);
        }

        [Test]
        public void EachNoveltyComesAloneOnAMediumLevel()
        {
            List<V3LevelResult> novel = Transition.Where(r => r.Novelty != V3Novelty.None).ToList();
            Assert.That(novel.Select(r => r.Novelty), Is.EquivalentTo(new[] { V3Novelty.FewerEmpty, V3Novelty.LargerCapacity, V3Novelty.ScatteredLayout }));
            foreach (V3LevelResult r in novel)
            {
                Assert.That(r.Band, Is.EqualTo(DifficultyBand.Medium), $"{r.Novelty} at {r.Level}");
                Assert.That(r.Level, Is.Not.EqualTo(V3LevelPlan.ShuffleUnlockLevel), "16 already brings Shuffle and the theme");
            }
            V3LevelResult fewer = novel.Single(r => r.Novelty == V3Novelty.FewerEmpty);
            V3LevelResult larger = novel.Single(r => r.Novelty == V3Novelty.LargerCapacity);
            Assert.That(fewer.Frame.EmptyCount, Is.EqualTo(1));
            Assert.That(Transition.Where(r => r.Level < fewer.Level).All(r => r.Frame.EmptyCount == 2), Is.True, "first single-empty level is the novelty");
            Assert.That(Transition.Where(r => r.Level < larger.Level).All(r => r.Frame.Capacity == 4), Is.True);
            Assert.That(Transition.Where(r => r.Level >= larger.Level).All(r => r.Frame.Capacity == 5), Is.True);
        }

        [Test]
        public void ScatteredLayoutArrivesOnceThenMixesWithClassic()
        {
            // Faz 2.3 (V3_SABLON.md karar 1 ve 3): serpiştirilmiş dizilim bir yenilik gibi tek başına bir Orta'da gelir;
            // öncesinde hep klasik, sonrasında klasikle karışık (aralarda kolaylar, sürpriz).
            V3LevelResult intro = Transition.Single(r => r.Novelty == V3Novelty.ScatteredLayout);
            Assert.That(intro.Frame.IsPacked, Is.False, "the novelty level shows the new layout");
            Assert.That(Transition.Where(r => r.Level < intro.Level).All(r => r.Frame.IsPacked), Is.True);
            List<V3LevelResult> after = Transition.Where(r => r.Level >= intro.Level).ToList();
            Assert.That(after.Count(r => !r.Frame.IsPacked), Is.GreaterThanOrEqualTo(2), "scattered appears again after its introduction");
            Assert.That(after.Count(r => r.Frame.IsPacked), Is.GreaterThanOrEqualTo(2), "classic boards stay in the mix");
        }

        [Test]
        public void SingleEmptyCloudBecomesMoreFrequent()
        {
            int first = Transition.First(r => r.Novelty == V3Novelty.FewerEmpty).Level;
            int firstCycle = (first - 1) / 5;
            int[] perCycle = Transition.GroupBy(r => (r.Level - 1) / 5).Where(c => c.Key >= firstCycle)
                .Select(c => c.Count(r => r.Frame.EmptyCount == 1)).ToArray();
            Assert.That(perCycle.Length, Is.GreaterThanOrEqualTo(2), "single empty must arrive early enough to grow");
            // Kademeli (kullanıcı, 13:20): giriş döngüsünde yalnızca yenilik bölümü, sonra her döngüde tam bir fazla (1, 2, …).
            Assert.That(perCycle[0], Is.EqualTo(1), "intro cycle has only the novelty level");
            for (int i = 1; i < perCycle.Length; i++) Assert.That(perCycle[i], Is.EqualTo(perCycle[i - 1] + 1), $"cycle +{i}");
            // İlk tema: döngü başına en fazla 2 → 21–25'te 1, 26–30'da 2, toplam 3.
            Assert.That(perCycle.Max(), Is.LessThanOrEqualTo(2));
            Assert.That(Transition.Count(r => r.Frame.EmptyCount == 1), Is.EqualTo(3));
            // Tema finali "sınav" olarak 1 boşlu kalır (kullanıcı 13:12: 30 gibi zor bölüm temanın sonunda gelebilir).
            Assert.That(Transition.Last().Frame.EmptyCount, Is.EqualTo(1));
        }

        // ---------- Tahta kalitesi ----------

        [Test]
        public void BoardsAreDistinctFromOneToThirty()
        {
            var keys = new List<string>();
            for (int level = 1; level <= 15; level++)
            {
                V3LevelEntry e = V3LevelData.Find(level);
                keys.Add(PuzzleSolver.CanonicalKey(e.CreateBoard(), e.Capacity));
            }
            keys.AddRange(Transition.Select(r => r.Primary.CanonicalKey));
            Assert.That(keys.Distinct().Count(), Is.EqualTo(keys.Count));
        }

        [Test]
        public void OutlierBoardsAreNeverChosen()
        {
            // Sanal oyuncunun yarısından fazlasının takıldığı tahtalar (ör. 14.10 raporundaki 132 puanlık aday) seçilmez.
            foreach (V3LevelResult r in Transition)
                Assert.That(r.Primary.Result.Metrics.NaiveFailureRate, Is.LessThan(V3LadderBuilder.MaxNaiveFailureRate), $"level {r.Level}");
        }

        [Test]
        public void TransitionIsDeterministicAndValid()
        {
            IReadOnlyList<V3LevelResult> again = V3LadderBuilder.BuildPart(V3Part.TransitionTheme);
            Assert.That(again.Select(r => r.Primary.Seed), Is.EqualTo(Transition.Select(r => r.Primary.Seed)));
            Assert.That(again.Select(r => r.Frame.ToString()), Is.EqualTo(Transition.Select(r => r.Frame.ToString())));
            foreach (V3LevelResult r in Transition)
            {
                LevelDefinition definition = r.Primary.ToLevelDefinition(r.Level, "green-valley", null);
                Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, $"level {r.Level}: {error}");
            }
        }

        [Test]
        public void LaterThemesUseTheThemeBuilder()
        {
            // 31–100 V3ThemeBuilder ile kuruluyor (Faz 2.2); 16–30 bu merdivende kalır.
            Assert.That(V3LadderBuilder.Supports(V3Part.Theme2) || V3LadderBuilder.Supports(V3Part.Theme3), Is.False);
            Assert.That(V3ThemeBuilder.Supports(V3Part.Theme2) && V3ThemeBuilder.Supports(V3Part.Theme3), Is.True);
        }
    }
}
