using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // v3 parça kurulumu: 1–100'ün zorluk tablosu, seçilen tahtalar ve oyuna bağlanması.
    public sealed class V3LevelBuildTests
    {
        // 1–15'in öğretici yazıları ve çerçevesi değişmez (tasarım §11).
        private static readonly string[] AuthoredCues =
        {
            "Bir hazneye dokun.", "Aynı hava türünün olduğu hazneyi seç.", "Aynı türden parçalar birlikte akar.", "Boş hazneler sana yer açar.", "Gökyüzünü dengele.",
            null, null, null, null, "Geri Al ile son hamleni geri çevirebilirsin.", null, null, null, null, "İlk dünyanı keşfetmeye hazırsın."
        };

        private static readonly V3Part[] DesignedParts = { V3Part.Intro, V3Part.Adaptation, V3Part.TransitionTheme, V3Part.Theme2, V3Part.Theme3 };
        private static readonly Dictionary<V3Part, IReadOnlyList<V3LevelResult>> Cache = new();

        private static IReadOnlyList<V3LevelResult> Part(V3Part part)
        {
            if (!Cache.TryGetValue(part, out IReadOnlyList<V3LevelResult> results))
                Cache[part] = results = V3LevelBuilder.BuildPart(part);
            return results;
        }

        private static IEnumerable<V3Part> Parts() => DesignedParts;
        private static IReadOnlyList<V3LevelResult> _published;
        private static IReadOnlyList<V3LevelResult> Published => _published ??= CampaignMoonRollout.Apply(Parts().SelectMany(Part));
        private const float MediumSeamTolerance = 0.25f;

        // ---------- Parça kurulumu ----------

        [Test]
        public void IntroUsesFrameAndPattern() => AssertFrameAndPattern(V3Part.Intro, 1, 5, "cap4/c2/e2");

        [Test]
        public void AdaptationUsesFrameAndPattern() => AssertFrameAndPattern(V3Part.Adaptation, 6, 15, "cap4/c3/e2");

        private static void AssertFrameAndPattern(V3Part part, int first, int last, string frame)
        {
            IReadOnlyList<V3LevelResult> results = Part(part);
            Assert.That(results.Select(r => r.Level), Is.EqualTo(Enumerable.Range(first, last - first + 1)));
            foreach (V3LevelResult result in results)
            {
                Assert.That(result.Band, Is.EqualTo(V3LevelPlan.BandFor(result.Level)));
                Assert.That(result.Frame.ToString(), Is.EqualTo(frame), $"level {result.Level}");
                Assert.That(result.Primary.Result.Status, Is.EqualTo(SolveStatus.Solved));
                Assert.That(result.Window.Contains(result.Primary.Score), Is.True, $"level {result.Level}");
            }
        }

        [Test]
        public void BoardsAreDistinctAcrossAllParts()
        {
            List<string> keys = Parts().SelectMany(Part).Select(r => r.Primary.CanonicalKey).ToList();
            Assert.That(keys.Distinct().Count(), Is.EqualTo(keys.Count));
        }

        [TestCaseSource(nameof(Parts))]
        public void HarderBandsScoreHigherWithinEachCycle(V3Part part)
        {
            foreach (IGrouping<int, V3LevelResult> cycle in Part(part).GroupBy(r => (r.Level - 1) / 5))
            {
                float medium = cycle.Where(r => r.Band == DifficultyBand.Medium).Max(r => r.Primary.Score);
                float hard = cycle.Single(r => r.Band == DifficultyBand.Hard).Primary.Score;
                float veryHard = cycle.Single(r => r.Band == DifficultyBand.VeryHard).Primary.Score;
                Assert.That(hard, Is.GreaterThan(medium), $"{part} cycle {cycle.Key}");
                Assert.That(veryHard, Is.GreaterThan(hard), $"{part} cycle {cycle.Key}");
            }
        }

        [TestCaseSource(nameof(Parts))]
        public void SameBandLevelsDoNotGetEasierWithinPart(V3Part part)
        {
            // Alışma etkisi: parça içinde aynı banttaki bölümler hafifçe zorlaşır, asla kolaylaşmaz.
            foreach (IGrouping<DifficultyBand, V3LevelResult> band in Part(part).GroupBy(r => r.Band))
            {
                float[] scores = band.OrderBy(r => r.Level).Select(r => r.Primary.Score).ToArray();
                for (int i = 1; i < scores.Length; i++)
                    Assert.That(scores[i], Is.GreaterThanOrEqualTo(scores[i - 1]), $"{part} {band.Key} #{i}");
            }
        }

        [TestCaseSource(nameof(Parts))]
        public void BandWindowsDoNotOverlap(V3Part part)
        {
            IReadOnlyList<V3LevelResult> results = Part(part);
            if (!float.IsNaN(results[0].Target)) Assert.Ignore("Ladder parts use absolute targets (V32LadderTests), not band windows.");
            ScoreWindow medium = results.First(r => r.Band == DifficultyBand.Medium).Window;
            ScoreWindow hard = results.First(r => r.Band == DifficultyBand.Hard).Window;
            ScoreWindow veryHard = results.First(r => r.Band == DifficultyBand.VeryHard).Window;
            Assert.That(hard.Min, Is.GreaterThan(medium.Max));
            Assert.That(veryHard.Min, Is.GreaterThan(hard.Max));
        }

        [Test]
        public void PartTransitionsAreCumulativeWithoutJumps()
        {
            // Zorluk etiketleri parça parça aynı şeyi ifade etmez (tasarım §2):
            // yeni parçanın Orta'ları öncekinin Orta'larından zor, ama ilk Orta öncekinin Çok zor'undan belirgin şekilde zor değil.
            for (int i = 1; i < DesignedParts.Length; i++)
            {
                IReadOnlyList<V3LevelResult> previous = Part(DesignedParts[i - 1]);
                IReadOnlyList<V3LevelResult> current = Part(DesignedParts[i]);
                float previousMediumMax = previous.Where(r => r.Band == DifficultyBand.Medium).Max(r => r.Primary.Score);
                float currentMediumMin = current.Where(r => r.Band == DifficultyBand.Medium).Min(r => r.Primary.Score);
                float previousFinale = previous.Last().Primary.Score;
                float firstMedium = current.First().Primary.Score;
                // Tema eğrileri sınırda aynı değerde birleşir (ör. 31–60 Orta 20→24, 61–80 Orta 24→26): küçük pay (kullanıcı 19:22).
                Assert.That(currentMediumMin, Is.GreaterThan(previousMediumMax - MediumSeamTolerance), $"{DesignedParts[i]} mediums must be harder");
                Assert.That(firstMedium, Is.LessThanOrEqualTo(previousFinale * 1.10f), $"{DesignedParts[i]} starts with a jump");
            }
        }

        [Test]
        public void TutorialCuesAreDemonstratedByTheBoard()
        {
            IReadOnlyList<V3LevelResult> intro = Part(V3Part.Intro);
            // L3 "Aynı türden parçalar birlikte akar.": en kısa çözümde en az bir grup (2+) hamlesi.
            Assert.That(intro[2].Primary.Result.Solution.Any(m => m.Count >= 2), Is.True);
            // L4 "Boş hazneler sana yer açar.": en kısa çözüm boş bir buluta en az bir hamle yapar.
            Assert.That(UsesEmptyCloud(intro[3].Primary), Is.True);
        }

        [Test]
        public void BuildIsDeterministic()
        {
            foreach (V3Part part in Parts())
            {
                IReadOnlyList<V3LevelResult> again = V3LevelBuilder.BuildPart(part);
                Assert.That(again.Select(r => r.Primary.Seed), Is.EqualTo(Part(part).Select(r => r.Primary.Seed)), part.ToString());
            }
        }

        [Test]
        public void BuiltLevelsAreValidLevelDefinitions()
        {
            foreach (V3LevelResult result in Parts().SelectMany(Part))
            {
                LevelDefinition definition = result.Primary.ToLevelDefinition(result.Level, "intro", null);
                Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, $"level {result.Level}: {error}");
            }
        }

        [Test]
        public void UndesignedPartsAreRejected()
        {
            // 16+ sabit çerçeve kullanmaz (çerçeveyi merdiven/tema kurucusu seçer: V32LadderTests, V32ThemeTests, V32Theme3Tests).
            Assert.Throws<NotSupportedException>(() => V3LevelPlan.FrameFor(16));
            Assert.Throws<NotSupportedException>(() => V3LevelPlan.FrameFor(61));
        }

        // ---------- Üretilen veri dosyası ve oyuna bağlanma ----------

        [Test]
        public void GeneratedDataMatchesBuilder()
        {
            // Veri dosyası elle değiştirilmiş ya da araç değişip yeniden üretilmemişse burada yakalanır.
            Assert.That(V3LevelData.BuiltParts, Is.EqualTo(DesignedParts), "regenerate V3LevelData");
            foreach (V3Part part in V3LevelData.BuiltParts)
            {
                foreach (V3LevelResult result in Published.Where(r=>V3LevelPlan.PartFor(r.Level)==part))
                {
                    V3LevelEntry entry = V3LevelData.Find(result.Level);
                    Assert.That(entry, Is.Not.Null, $"level {result.Level} missing — regenerate V3LevelData");
                    Assert.That(entry.Seed, Is.EqualTo(result.Primary.Seed), $"level {result.Level}");
                    Assert.That(entry.Band, Is.EqualTo(result.Band));
                    Assert.That(entry.Capacity, Is.EqualTo(result.Frame.Capacity));
                    Assert.That(entry.Board, Is.EqualTo(V3BoardText.Format(result.Primary.Board)));
                    Assert.That(entry.Score, Is.EqualTo(result.Primary.Score).Within(1e-4f));
                    Assert.That(entry.OptimalMoves, Is.EqualTo(result.Primary.Result.Metrics.OptimalMoves));
                }
            }
        }

        [Test]
        public void CatalogUsesV3BoardsAndKeepsTutorialFrame()
        {
            for (int level = 1; level <= 15; level++)
            {
                LevelDefinition definition = AuthoredLevelCatalog.Get(level);
                V3LevelEntry entry = V3LevelData.Find(level);
                Assert.That(entry, Is.Not.Null, $"level {level}");
                Assert.That(V3BoardText.Format(definition.CreateBoard()), Is.EqualTo(entry.Board), $"level {level}");
                Assert.That(definition.tutorialCue, Is.EqualTo(AuthoredCues[level - 1]), $"level {level}");
                Assert.That(definition.capacity, Is.EqualTo(4));
                Assert.That(definition.worldId, Is.EqualTo("green-valley"));
                Assert.That(definition.generated, Is.False, "1–15 must stay authored: Shuffle stays locked until 16");
                Assert.That(definition.seed, Is.EqualTo(entry.Seed));
                Assert.That(definition.difficultyScore, Is.EqualTo(entry.Score).Within(1e-4f));
                Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, error);
            }
            Assert.That(AuthoredLevelCatalog.Count, Is.EqualTo(15));
        }

        [Test]
        public void LevelsNotYetRebuiltStayOnOldSystem()
        {
            // 101+ eski üreticiyle kalır (16–100 deneme olarak veride; 61–100 2026-10-15 19:00'dan beri).
            Assert.That(V3LevelData.Find(101), Is.Null);
            Assert.That(AuthoredLevelCatalog.TryGetBuilt(101, out _), Is.False);
            Assert.That(LevelGenerator.Generate(101).generated, Is.True);
            foreach (int level in new[] { 45, 75, 100 })
            {
                Assert.That(AuthoredLevelCatalog.TryGetBuilt(level, out LevelDefinition built), Is.True, $"level {level}");
                Assert.That(built.modifiers, Is.Empty, $"level {level}: 1–100 themes are visual only: no modifiers (§11k)");
            }
        }

        [Test]
        public void TransitionLevelsLoadFromV3Data()
        {
            // v3.2: 16–30 oyunda veri dosyasından gelir (eski üretici değil); Shuffle 16'da açık kalır.
            for (int level = 16; level <= 30; level++)
            {
                V3LevelEntry entry = V3LevelData.Find(level);
                Assert.That(entry, Is.Not.Null, $"level {level} missing — regenerate V3LevelData");
                Assert.That(AuthoredLevelCatalog.TryGetBuilt(level, out LevelDefinition definition), Is.True, $"level {level}");
                Assert.That(V3BoardText.Format(definition.CreateBoard()), Is.EqualTo(entry.Board), $"level {level}");
                Assert.That(definition.capacity, Is.EqualTo(entry.Capacity));
                Assert.That(definition.seed, Is.EqualTo(entry.Seed));
                Assert.That(definition.generated, Is.False);
                Assert.That(definition.tutorialCue, Is.Null);
                Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, error);
            }
            Assert.That(AuthoredLevelCatalog.TryGetBuilt(15, out _), Is.False, "1–15 stay in the authored catalog");
            Assert.That(Cloudora.Puzzle.PuzzleController.IsShuffleUnlocked(16), Is.True);
        }

        // ---------- Tahta metni ----------

        [Test]
        public void BoardTextRoundTrips()
        {
            WeatherType[][] board =
            {
                new[] { WeatherType.Sun, WeatherType.Rain, WeatherType.Snow, WeatherType.Wind },
                new[] { WeatherType.Moon, WeatherType.Lightning, WeatherType.Rainbow },
                Array.Empty<WeatherType>()
            };
            string text = V3BoardText.Format(board);
            Assert.That(text, Is.EqualTo("SRNW/MLB/"));
            Assert.That(V3BoardText.Format(V3BoardText.Parse(text)), Is.EqualTo(text));
            Assert.Throws<FormatException>(() => V3BoardText.Parse("SX/"));
        }

        private static bool UsesEmptyCloud(LevelCandidate candidate)
        {
            var state = new PuzzleState(candidate.Board.Select(c => new CloudState(candidate.Frame.Capacity, c)));
            foreach (Move move in candidate.Result.Solution)
            {
                if (state.Clouds[move.TargetIndex].IsEmpty) return true;
                PuzzleRules.TryApply(state, move);
            }
            return false;
        }
    }
}
