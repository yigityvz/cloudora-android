using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // v3 bölüm aracı testleri: çözücü, aday üretici, seçici ve 1–100 planı.
    public sealed class LevelToolTests
    {
        private const WeatherType S = WeatherType.Sun;
        private const WeatherType R = WeatherType.Rain;
        private const int Limit = 200000;

        private static WeatherType[] E() => Array.Empty<WeatherType>();

        // ---------- Çözücü ----------

        [Test]
        public void SolvedBoardNeedsZeroMoves()
        {
            PuzzleSolveResult result = PuzzleSolver.Solve(new[] { new[] { S, S }, new[] { R, R }, E() }, 2, Limit);
            Assert.That(result.Status, Is.EqualTo(SolveStatus.Solved));
            Assert.That(result.Metrics.OptimalMoves, Is.EqualTo(0));
            Assert.That(result.Solution, Is.Empty);
        }

        [Test]
        public void OneMoveBoardIsSolvedInOneMove()
        {
            WeatherType[][] board = { new[] { S }, new[] { S }, new[] { R, R } };
            PuzzleSolveResult result = PuzzleSolver.Solve(board, 2, Limit);
            Assert.That(result.Status, Is.EqualTo(SolveStatus.Solved));
            Assert.That(result.Metrics.OptimalMoves, Is.EqualTo(1));
            AssertReplaySolves(board, 2, result.Solution);
        }

        [Test]
        public void BoardWithoutLegalMovesIsUnsolvable()
        {
            PuzzleSolveResult result = PuzzleSolver.Solve(new[] { new[] { S, R }, new[] { R, S } }, 2, Limit);
            Assert.That(result.Status, Is.EqualTo(SolveStatus.Unsolvable));
            Assert.That(result.Metrics.OptimalMoves, Is.EqualTo(-1));
            Assert.That(result.Metrics.ReachableStates, Is.EqualTo(1));
            Assert.That(result.Metrics.DeadStateRatio, Is.EqualTo(1f));
            Assert.That(result.Solution, Is.Empty);
        }

        [Test]
        public void SolverRejectsMalformedBoards()
        {
            Assert.Throws<ArgumentException>(() => PuzzleSolver.Solve(new[] { new[] { S, S, S } }, 2, Limit));
            Assert.Throws<ArgumentException>(() => PuzzleSolver.Solve(Array.Empty<WeatherType[]>(), 2, Limit));
            Assert.Throws<ArgumentOutOfRangeException>(() => PuzzleSolver.Solve(new[] { new[] { S } }, 8, Limit));
            Assert.Throws<ArgumentException>(() => PuzzleSolver.Solve(Enumerable.Repeat(E(), 13).ToArray(), 4, Limit));
            Assert.Throws<ArgumentOutOfRangeException>(() => PuzzleSolver.Solve(new[] { new[] { S } }, 4, 0));
        }

        [Test]
        public void SolverDoesNotMutateInputBoard()
        {
            WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(4, 3, 2), 11);
            string before = BoardText(board);
            PuzzleSolver.Solve(board, 4, Limit);
            Assert.That(BoardText(board), Is.EqualTo(before));
        }

        [Test]
        public void SolutionsReplayWithGameRulesAndHaveOptimalLength()
        {
            int solved = 0;
            foreach (BoardFrame frame in new[] { new BoardFrame(4, 2, 2), new BoardFrame(4, 3, 2), new BoardFrame(4, 3, 1) })
            {
                for (int seed = 1; seed <= 60; seed++)
                {
                    WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                    PuzzleSolveResult result = PuzzleSolver.Solve(board, frame.Capacity, Limit);
                    Assert.That(result.Status, Is.Not.EqualTo(SolveStatus.LimitExceeded), $"{frame} seed {seed}");
                    if (result.Status != SolveStatus.Solved) continue;
                    solved++;
                    Assert.That(result.Solution.Length, Is.EqualTo(result.Metrics.OptimalMoves), $"{frame} seed {seed}");
                    AssertReplaySolves(board, frame.Capacity, result.Solution);
                }
            }
            Assert.That(solved, Is.GreaterThan(100));
        }

        [Test]
        public void OptimalMovesMatchIndependentReferenceSearch()
        {
            foreach (BoardFrame frame in new[] { new BoardFrame(3, 2, 1), new BoardFrame(4, 2, 2), new BoardFrame(4, 3, 2), new BoardFrame(3, 3, 1) })
            {
                for (int seed = 1; seed <= 40; seed++)
                {
                    WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                    PuzzleSolveResult result = PuzzleSolver.Solve(board, frame.Capacity, Limit);
                    int reference = ReferenceShortestSolution(board, frame.Capacity);
                    Assert.That(result.Metrics.OptimalMoves, Is.EqualTo(reference), $"{frame} seed {seed}");
                    Assert.That(result.Status, Is.EqualTo(reference >= 0 ? SolveStatus.Solved : SolveStatus.Unsolvable));
                }
            }
        }

        [Test]
        public void CloudOrderDoesNotChangeMetrics()
        {
            for (int seed = 1; seed <= 30; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(4, 3, 2), seed);
                WeatherType[][] reversed = board.Reverse().ToArray();
                DifficultyMetrics a = PuzzleSolver.Solve(board, 4, Limit).Metrics;
                DifficultyMetrics b = PuzzleSolver.Solve(reversed, 4, Limit).Metrics;
                Assert.That(b, Is.EqualTo(a), $"seed {seed}");
                Assert.That(PuzzleSolver.CanonicalKey(reversed, 4), Is.EqualTo(PuzzleSolver.CanonicalKey(board, 4)));
            }
        }

        [Test]
        public void SolverIsDeterministic()
        {
            WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(4, 3, 2), 5);
            PuzzleSolveResult a = PuzzleSolver.Solve(board, 4, Limit);
            PuzzleSolveResult b = PuzzleSolver.Solve(board, 4, Limit);
            Assert.That(b.Metrics, Is.EqualTo(a.Metrics));
            Assert.That(MovesText(b.Solution), Is.EqualTo(MovesText(a.Solution)));
        }

        [Test]
        public void StateLimitIsRespectedAndReported()
        {
            WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(5, 4, 2), 3);
            PuzzleSolveResult result = PuzzleSolver.Solve(board, 5, 50);
            Assert.That(result.Status, Is.EqualTo(SolveStatus.LimitExceeded));
            Assert.That(result.Metrics.ReachableStates, Is.LessThanOrEqualTo(50));
            Assert.That(result.Solution, Is.Empty);
        }

        [Test]
        public void MetricRatiosStayInRange()
        {
            for (int seed = 1; seed <= 40; seed++)
            {
                DifficultyMetrics m = PuzzleSolver.Solve(LevelCandidateFactory.Deal(new BoardFrame(4, 3, 2), seed), 4, Limit).Metrics;
                Assert.That(m.DeadStateRatio, Is.InRange(0f, 1f));
                Assert.That(m.ProgressMoveRatio, Is.InRange(0f, 1f));
                Assert.That(m.TrapMoveRatio, Is.InRange(0f, 1f));
                Assert.That(m.ReachableStates, Is.GreaterThan(0));
                if (m.OptimalMoves > 0) Assert.That(m.AverageChoices, Is.GreaterThanOrEqualTo(1f));
                Assert.That(m.NaiveFailureRate, Is.InRange(0f, 1f));
            }
        }

        [Test]
        public void ScoreRisesWithLengthTrapsMisdirectionAndNaivePlay()
        {
            var baseline = new DifficultyMetrics(5, 100, 0.1f, 0.5f, 0.1f, 4f, 1f, 0.1f);
            Assert.That(new DifficultyMetrics(9, 100, 0.1f, 0.5f, 0.1f, 4f, 1f, 0.1f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(new DifficultyMetrics(5, 100, 0.1f, 0.5f, 0.4f, 4f, 1f, 0.1f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(new DifficultyMetrics(5, 100, 0.1f, 0.2f, 0.1f, 4f, 1f, 0.1f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(new DifficultyMetrics(5, 100, 0.6f, 0.5f, 0.1f, 4f, 1f, 0.1f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(new DifficultyMetrics(5, 100, 0.1f, 0.5f, 0.1f, 4f, 3f, 0.1f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(new DifficultyMetrics(5, 100, 0.1f, 0.5f, 0.1f, 4f, 1f, 0.5f).Score, Is.GreaterThan(baseline.Score));
            Assert.That(float.IsNaN(new DifficultyMetrics(-1, 1, 1f, 0f, 0f, 0f, 0f, 0f).Score), Is.True);
        }

        // ---------- Sanal oyuncu ----------

        [Test]
        public void NaivePlayerSolvesObviousBoardWithoutExtraMoves()
        {
            DifficultyMetrics m = PuzzleSolver.Solve(new[] { new[] { S }, new[] { S }, new[] { R, R } }, 2, Limit).Metrics;
            Assert.That(m.NaiveExcessMoves, Is.EqualTo(0f));
            Assert.That(m.NaiveFailureRate, Is.EqualTo(0f));
        }

        [Test]
        public void NaivePlayerMetricsStayInRange()
        {
            foreach (BoardFrame frame in new[] { new BoardFrame(4, 3, 2), new BoardFrame(4, 3, 1) })
            {
                for (int seed = 1; seed <= 30; seed++)
                {
                    PuzzleSolveResult result = PuzzleSolver.Solve(LevelCandidateFactory.Deal(frame, seed), 4, Limit);
                    DifficultyMetrics m = result.Metrics;
                    if (result.Status != SolveStatus.Solved)
                    {
                        Assert.That(m.NaiveExcessMoves, Is.EqualTo(0f));
                        Assert.That(m.NaiveFailureRate, Is.EqualTo(0f));
                        continue;
                    }
                    Assert.That(m.NaiveExcessMoves, Is.GreaterThanOrEqualTo(0f), $"{frame} seed {seed}");
                    Assert.That(m.NaiveFailureRate, Is.InRange(0f, 1f), $"{frame} seed {seed}");
                }
            }
        }

        [Test]
        public void NaivePlayerFailsMoreWhereTrapsExist()
        {
            // 2 boş bulutta çıkmaz yok, 1 boş bulutta var: sanal oyuncu 1 boş bulutta daha sık takılmalı.
            float twoEmpty = AverageFailure(new BoardFrame(4, 3, 2));
            float oneEmpty = AverageFailure(new BoardFrame(4, 3, 1));
            Assert.That(oneEmpty, Is.GreaterThan(twoEmpty));
        }

        [Test]
        public void NaivePlayerAgreesWithDeviceTestData()
        {
            // 2026-10-14 Editor + telefon testleri: L4 "RSRS/RRSS" ve L5 "RSRS/RSRS", L2 "SRSS/SRRR"den
            // her iki testte de daha fazla fazla-hamleyle oynandı. Ölçüm bu sırayı yakalamalı.
            float l2 = Naive("SRSS/SRRR//");
            float l4 = Naive("RSRS/RRSS//");
            float l5 = Naive("RSRS/RSRS//");
            Assert.That(l4, Is.GreaterThan(l2), $"L4 {l4} vs L2 {l2}");
            Assert.That(l5, Is.GreaterThan(l2), $"L5 {l5} vs L2 {l2}");
        }

        [Test]
        public void SensiblePlayerBeatsRandomPlayer()
        {
            // Hamle tercihleri (tamamla, istifle, bozma…) gerçekten etkili olmalı; yoksa ölçüm rastgele oyundan farksızdır.
            var frame = new BoardFrame(4, 3, 2);
            float sensible = 0f, random = 0f;
            for (int seed = 1; seed <= 40; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                sensible += PuzzleSolver.Solve(board, 4, Limit).Metrics.NaiveExcessMoves;
                random += PuzzleSolver.Solve(board, 4, Limit, NaivePlayerStyle.Random).Metrics.NaiveExcessMoves;
            }
            Assert.That(sensible, Is.LessThan(random * 0.6f), $"sensible {sensible / 40f:F2} vs random {random / 40f:F2}");
        }

        private static float Naive(string board) =>
            PuzzleSolver.Solve(V3BoardText.Parse(board), 4, Limit).Metrics.NaiveExcessMoves;

        private static float AverageFailure(BoardFrame frame)
        {
            var rates = new List<float>();
            for (int seed = 1; seed <= 60; seed++)
            {
                PuzzleSolveResult result = PuzzleSolver.Solve(LevelCandidateFactory.Deal(frame, seed), frame.Capacity, Limit);
                if (result.Status == SolveStatus.Solved) rates.Add(result.Metrics.NaiveFailureRate);
            }
            return rates.Average();
        }

        // ---------- Aday üretici ----------

        [Test]
        public void DealMatchesFrameExactly()
        {
            WeatherType[] colors = { WeatherType.Sun, WeatherType.Rain, WeatherType.Snow, WeatherType.Wind, WeatherType.Moon, WeatherType.Lightning };
            foreach (BoardFrame frame in new[] { new BoardFrame(4, 2, 2), new BoardFrame(4, 3, 2), new BoardFrame(5, 4, 1), new BoardFrame(7, 6, 2) })
            {
                for (int seed = 1; seed <= 50; seed++)
                {
                    WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                    Assert.That(board.Length, Is.EqualTo(frame.ColorCount + frame.EmptyCount));
                    for (int i = 0; i < frame.ColorCount; i++) Assert.That(board[i].Length, Is.EqualTo(frame.Capacity), $"{frame} seed {seed}");
                    for (int i = frame.ColorCount; i < board.Length; i++) Assert.That(board[i], Is.Empty);

                    var counts = new Dictionary<WeatherType, int>();
                    foreach (WeatherType weather in board.SelectMany(cloud => cloud))
                        counts[weather] = counts.TryGetValue(weather, out int c) ? c + 1 : 1;
                    Assert.That(counts.Keys, Is.EquivalentTo(colors.Take(frame.ColorCount)));
                    Assert.That(counts.Values.All(c => c == frame.Capacity), Is.True);

                    var state = new PuzzleState(board.Select(cloud => new CloudState(frame.Capacity, cloud)));
                    Assert.That(PuzzleRules.IsSolved(state), Is.False, $"{frame} seed {seed} starts solved");
                }
            }
        }

        [Test]
        public void DealIsDeterministicAndSeedSensitive()
        {
            var frame = new BoardFrame(4, 3, 2);
            var distinct = new HashSet<string>();
            for (int seed = 1; seed <= 30; seed++)
            {
                Assert.That(BoardText(LevelCandidateFactory.Deal(frame, seed)), Is.EqualTo(BoardText(LevelCandidateFactory.Deal(frame, seed))));
                distinct.Add(BoardText(LevelCandidateFactory.Deal(frame, seed)));
            }
            Assert.That(distinct.Count, Is.GreaterThanOrEqualTo(25));
        }

        [Test]
        public void RandomSequenceIsFixedAcrossPlatforms()
        {
            // Değerler Python ile bağımsız SplitMix64 hesabından; değişirse kayıtlı seed'ler başka tahta üretir.
            AssertSequence(12345, 761, 578, 264, 274, 235, 992);
            AssertSequence(-7, 682, 220, 497, 318, 962, 175);
        }

        [Test]
        public void FrameRejectsInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(1, 2, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(8, 2, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(4, 1, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(4, 7, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(4, 3, -1));
        }

        // ---------- Seçici ----------

        [Test]
        public void SelectionPicksInWindowCandidatesWithDistinctBackups()
        {
            var frame = new BoardFrame(4, 3, 2);
            CandidateSelection selection = LevelCandidateSelector.Select(frame, new ScoreWindow(0f, 1000f), 1, 80, 2, Limit);
            Assert.That(selection.Success, Is.True, selection.Failure);
            Assert.That(selection.Backups.Count, Is.EqualTo(2));

            var chosen = new[] { selection.Primary }.Concat(selection.Backups).ToList();
            Assert.That(chosen.Select(c => c.CanonicalKey).Distinct().Count(), Is.EqualTo(3));
            foreach (LevelCandidate candidate in chosen)
            {
                Assert.That(candidate.Result.Status, Is.EqualTo(SolveStatus.Solved));
                Assert.That(candidate.Score, Is.InRange(0f, 1000f));
                Assert.That(BoardText(candidate.Board), Is.EqualTo(BoardText(LevelCandidateFactory.Deal(frame, candidate.Seed))));
            }

            CandidateSelection again = LevelCandidateSelector.Select(frame, new ScoreWindow(0f, 1000f), 1, 80, 2, Limit);
            Assert.That(again.Primary.Seed, Is.EqualTo(selection.Primary.Seed));
            Assert.That(again.Backups.Select(c => c.Seed), Is.EqualTo(selection.Backups.Select(c => c.Seed)));
        }

        [Test]
        public void SelectionPrefersScoreClosestToWindowCenter()
        {
            CandidateSelection all = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), new ScoreWindow(0f, 1000f), 1, 80, 0, Limit);
            float[] scores = all.Measured.Where(c => c.Result.Status == SolveStatus.Solved).Select(c => c.Score).OrderBy(s => s).ToArray();
            var window = new ScoreWindow(scores[scores.Length / 4], scores[scores.Length * 3 / 4]);

            CandidateSelection selection = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), window, 1, 80, 1, Limit);
            Assert.That(selection.Success, Is.True, selection.Failure);
            float best = Math.Abs(selection.Primary.Score - window.Center);
            foreach (LevelCandidate candidate in selection.Measured.Where(c => c.Result.Status == SolveStatus.Solved && window.Contains(c.Score)))
                Assert.That(best, Is.LessThanOrEqualTo(Math.Abs(candidate.Score - window.Center) + 1e-4f));
            Assert.That(Math.Abs(selection.Backups[0].Score - window.Center), Is.GreaterThanOrEqualTo(best - 1e-4f));
        }

        [Test]
        public void SelectionFailsLoudlyWhenWindowIsUnreachable()
        {
            CandidateSelection selection = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), new ScoreWindow(5000f, 6000f), 1, 40, 2, Limit);
            Assert.That(selection.Success, Is.False);
            Assert.That(selection.Primary, Is.Null);
            Assert.That(selection.Failure, Is.Not.Empty);
        }

        [Test]
        public void SelectionFailsWhenBackupsAreMissing()
        {
            // Tek uygun aday varken 2 yedek istenirse başarı sayılmamalı (Shuffle için yedek şart).
            CandidateSelection all = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), new ScoreWindow(0f, 1000f), 1, 80, 0, Limit);
            LevelCandidate top = all.Measured.Where(c => c.Result.Status == SolveStatus.Solved).OrderByDescending(c => c.Score).First();
            var narrow = new ScoreWindow(top.Score, top.Score);
            CandidateSelection selection = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), narrow, 1, 80, 50, Limit);
            Assert.That(selection.Success, Is.False);
            Assert.That(selection.Failure, Is.Not.Empty);
        }

        [Test]
        public void SelectionSkipsCandidatesOverTheStateLimit()
        {
            CandidateSelection selection = LevelCandidateSelector.Select(new BoardFrame(5, 4, 2), new ScoreWindow(0f, 1000f), 1, 10, 0, 5);
            Assert.That(selection.Success, Is.False);
            Assert.That(selection.LimitExceeded, Is.EqualTo(10));
        }

        [Test]
        public void SelectedCandidateBecomesValidLevelDefinition()
        {
            CandidateSelection selection = LevelCandidateSelector.Select(new BoardFrame(4, 3, 2), new ScoreWindow(0f, 1000f), 1, 40, 0, Limit);
            LevelDefinition definition = selection.Primary.ToLevelDefinition(7, "intro", "cue");
            Assert.That(PuzzleValidator.Validate(definition, out string error), Is.True, error);
            Assert.That(definition.levelId, Is.EqualTo(7));
            Assert.That(definition.capacity, Is.EqualTo(4));
            Assert.That(definition.tutorialCue, Is.EqualTo("cue"));
            Assert.That(definition.seed, Is.EqualTo(selection.Primary.Seed));
            Assert.That(definition.generated, Is.False);
            Assert.That(definition.knownSolution.Length, Is.EqualTo(selection.Primary.Result.Metrics.OptimalMoves));
            Assert.That(BoardText(definition.CreateBoard()), Is.EqualTo(BoardText(selection.Primary.Board)));
        }

        // ---------- 1–100 planı ----------

        [Test]
        public void V3PatternRepeatsEveryFiveLevels()
        {
            DifficultyBand[] cycle = { DifficultyBand.Medium, DifficultyBand.Medium, DifficultyBand.Hard, DifficultyBand.Medium, DifficultyBand.VeryHard };
            for (int level = 1; level <= 100; level++)
                Assert.That(V3LevelPlan.BandFor(level), Is.EqualTo(cycle[(level - 1) % 5]), $"level {level}");
            Assert.That(Enumerable.Range(1, 100).Any(level => V3LevelPlan.BandFor(level) == DifficultyBand.Easy), Is.False);
        }

        [Test]
        public void V3PartsCoverOneToHundred()
        {
            Assert.That(V3LevelPlan.PartFor(1), Is.EqualTo(V3Part.Intro));
            Assert.That(V3LevelPlan.PartFor(5), Is.EqualTo(V3Part.Intro));
            Assert.That(V3LevelPlan.PartFor(6), Is.EqualTo(V3Part.Adaptation));
            Assert.That(V3LevelPlan.PartFor(15), Is.EqualTo(V3Part.Adaptation));
            Assert.That(V3LevelPlan.PartFor(16), Is.EqualTo(V3Part.TransitionTheme));
            Assert.That(V3LevelPlan.PartFor(30), Is.EqualTo(V3Part.TransitionTheme));
            Assert.That(V3LevelPlan.PartFor(31), Is.EqualTo(V3Part.Theme2));
            Assert.That(V3LevelPlan.PartFor(60), Is.EqualTo(V3Part.Theme2));
            Assert.That(V3LevelPlan.PartFor(61), Is.EqualTo(V3Part.Theme3));
            Assert.That(V3LevelPlan.PartFor(100), Is.EqualTo(V3Part.Theme3));
            Assert.Throws<ArgumentOutOfRangeException>(() => V3LevelPlan.PartFor(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => V3LevelPlan.PartFor(101));
            Assert.Throws<ArgumentOutOfRangeException>(() => V3LevelPlan.BandFor(101));
            // Her parçanın son bölümü "Çok zor" olmalı.
            foreach (int end in new[] { 5, 15, 30, 60, 100 })
            {
                Assert.That(V3LevelPlan.BandFor(end), Is.EqualTo(DifficultyBand.VeryHard), $"part end {end}");
                if (end < 100) Assert.That(V3LevelPlan.PartFor(end + 1), Is.Not.EqualTo(V3LevelPlan.PartFor(end)));
            }
        }

        // ---------- Yardımcılar ----------

        private static void AssertReplaySolves(WeatherType[][] board, int capacity, Move[] solution)
        {
            var state = new PuzzleState(board.Select(cloud => new CloudState(capacity, cloud)));
            foreach (Move move in solution)
                Assert.That(PuzzleRules.TryApply(state, move), Is.True, "Solution contains an illegal move");
            Assert.That(PuzzleRules.IsSolved(state), Is.True, "Solution does not solve the board");
        }

        // Bağımsız referans: bulut sırası korunarak düz BFS, yalnızca oyunun kendi PuzzleRules'ı ile.
        private static int ReferenceShortestSolution(WeatherType[][] board, int capacity)
        {
            var start = new PuzzleState(board.Select(cloud => new CloudState(capacity, cloud)));
            if (PuzzleRules.IsSolved(start)) return 0;
            var seen = new HashSet<string> { StateText(start) };
            var frontier = new List<PuzzleState> { start };
            for (int depth = 1; frontier.Count > 0; depth++)
            {
                var next = new List<PuzzleState>();
                foreach (PuzzleState state in frontier)
                {
                    for (int s = 0; s < state.Clouds.Count; s++)
                    for (int t = 0; t < state.Clouds.Count; t++)
                    {
                        CloudState source = state.Clouds[s];
                        CloudState target = state.Clouds[t];
                        if (s == t || source.IsEmpty) continue;
                        var move = new Move(s, t, Math.Min(source.TopGroupCount(), target.Capacity - target.Count), source.Top);
                        PuzzleState child = state.Clone();
                        if (!PuzzleRules.TryApply(child, move)) continue;
                        if (PuzzleRules.IsSolved(child)) return depth;
                        if (seen.Add(StateText(child))) next.Add(child);
                    }
                }
                frontier = next;
            }
            return -1;
        }

        private static void AssertSequence(int seed, params int[] expected)
        {
            var random = new DeterministicRandom(seed);
            Assert.That(expected.Select(_ => random.Next(1000)).ToArray(), Is.EqualTo(expected));
        }

        private static string StateText(PuzzleState state) => string.Join("|", state.Clouds.Select(c => string.Join(",", c.Elements)));
        private static string BoardText(WeatherType[][] board) => string.Join("|", board.Select(c => string.Join(",", c)));
        private static string MovesText(Move[] moves) => string.Join(";", moves.Select(m => $"{m.SourceIndex}>{m.TargetIndex}x{m.Count}"));
    }
}
