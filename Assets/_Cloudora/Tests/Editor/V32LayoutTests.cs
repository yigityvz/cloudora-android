using System;
using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // v3.2 (tasarım §11j): farklı başlangıç dizilimleri. Boş alan miktarı aynı (EmptyCount × kapasite), ama bulutlara
    // farklı dağılır: OpenClouds kadar bulut tamamen boş, kalan boşluk diğer bulutlara serpiştirilir.
    public sealed class V32LayoutTests
    {
        private const int Limit = 300000;

        [Test]
        public void DefaultLayoutIsTheClassicPackedBoard()
        {
            // 1–15 ve önceki seçimler değişmesin: varsayılan dizilim eski "dolu + boş" tahtadır.
            var frame = new BoardFrame(4, 3, 2);
            Assert.That(frame.OpenClouds, Is.EqualTo(2));
            Assert.That(frame.ToString(), Is.EqualTo("cap4/c3/e2"));
            WeatherType[][] board = LevelCandidateFactory.Deal(frame, 7);
            Assert.That(board.Take(3).All(c => c.Length == 4), Is.True);
            Assert.That(board.Skip(3).All(c => c.Length == 0), Is.True);
        }

        [TestCase(4, 3, 2, 1)]
        [TestCase(4, 3, 2, 0)]
        [TestCase(5, 3, 2, 1)]
        [TestCase(5, 3, 2, 0)]
        [TestCase(5, 3, 1, 0)]
        public void ScatteredLayoutKeepsPiecesAndFreeSpace(int capacity, int colors, int empties, int open)
        {
            var frame = new BoardFrame(capacity, colors, empties, open);
            Assert.That(frame.ToString(), Is.EqualTo($"cap{capacity}/c{colors}/e{empties}/o{open}"));
            for (int seed = 1; seed <= 40; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                Assert.That(board.Length, Is.EqualTo(colors + empties), $"seed {seed}");
                Assert.That(board.All(c => c.Length <= capacity), Is.True);
                Assert.That(board.Count(c => c.Length == 0), Is.EqualTo(open), $"seed {seed}: fully empty clouds");
                Assert.That(board.Sum(c => capacity - c.Length), Is.EqualTo(empties * capacity), "free space unchanged");
                foreach (IGrouping<WeatherType, WeatherType> g in board.SelectMany(c => c).GroupBy(w => w))
                    Assert.That(g.Count(), Is.EqualTo(capacity), $"seed {seed}: {g.Key}");
                Assert.That(board.SelectMany(c => c).Distinct().Count(), Is.EqualTo(colors));
                var state = new PuzzleState(board.Select(c => new CloudState(capacity, c)));
                Assert.That(PuzzleRules.IsSolved(state), Is.False, $"seed {seed} starts solved");
            }
        }

        [Test]
        public void ScatteredLayoutIsDeterministicAndVaried()
        {
            var frame = new BoardFrame(5, 3, 2, 0);
            Assert.That(V3BoardText.Format(LevelCandidateFactory.Deal(frame, 11)), Is.EqualTo(V3BoardText.Format(LevelCandidateFactory.Deal(frame, 11))));
            int shapes = Enumerable.Range(1, 60)
                .Select(s => string.Join(",", LevelCandidateFactory.Deal(frame, s).Select(c => c.Length).OrderBy(n => n)))
                .Distinct().Count();
            Assert.That(shapes, Is.GreaterThan(3), "fill levels should vary between boards");
        }

        [Test]
        public void ScatteredBoardsAreSolvedWithTheGameRules()
        {
            var frame = new BoardFrame(4, 3, 2, 0);
            int solved = 0;
            for (int seed = 1; seed <= 30; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                PuzzleSolveResult result = PuzzleSolver.Solve(board, frame.Capacity, Limit);
                if (result.Status != SolveStatus.Solved) continue;
                solved++;
                var state = new PuzzleState(board.Select(c => new CloudState(frame.Capacity, c)));
                foreach (Move move in result.Solution) Assert.That(PuzzleRules.TryApply(state, move), Is.True, $"seed {seed}");
                Assert.That(PuzzleRules.IsSolved(state), Is.True, $"seed {seed}");
                Assert.That(result.Solution.Length, Is.EqualTo(result.Metrics.OptimalMoves));
            }
            Assert.That(solved, Is.GreaterThan(0));
        }

        [Test]
        public void InvalidOpenCloudCountsAreRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(4, 3, 2, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoardFrame(4, 3, 2, -1));
        }
    }
}
