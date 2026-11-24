using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // Faz 2.6 (2026-10-15 20:40, telefonda 83 + 2 Extra + Shuffle ile ~144 sn donma): oyundaki Shuffle ve No Moves dönüşü
    // tam ölçüm çözücüsünü (bütün durum uzayı + zorluk ölçüleri) değil, ilk çözümde duran hızlı kontrolü kullanır.
    public sealed class V32SolverLoadTests
    {
        private const int SolverStateLimit = 200000;   // PuzzleController.SolverStateLimit ile aynı

        private static WeatherType[][] WithExtras(V3LevelEntry entry, int extras) =>
            entry.CreateBoard().Select(c => c.ToArray()).Concat(Enumerable.Range(0, extras).Select(_ => new WeatherType[0])).ToArray();

        [Test]
        public void QuickCheckAgreesWithTheFullSolver()
        {
            // 1–60 tahtaları ve her buluttan birkaç karıştırılmış hali: çözülebilir / çözülemez kararı birebir aynı.
            var random = new System.Random(7);
            int unsolvable = 0;
            foreach (int level in Enumerable.Range(1, 60))
            {
                V3LevelEntry entry = V3LevelData.Find(level);
                WeatherType[][] board = entry.CreateBoard();
                var boards = new List<WeatherType[][]> { board };
                for (int c = 0; c < board.Length; c++)
                for (int k = 0; k < 2; k++)
                {
                    WeatherType[][] copy = board.Select(x => x.ToArray()).ToArray();
                    copy[c] = copy[c].OrderBy(_ => random.Next()).ToArray();
                    boards.Add(copy);
                }
                foreach (WeatherType[][] b in boards)
                {
                    SolveStatus full = PuzzleSolver.Solve(b, entry.Capacity, SolverStateLimit).Status;
                    if (full == SolveStatus.LimitExceeded) continue;
                    Assert.That(PuzzleSolver.CheckSolvable(b, entry.Capacity, SolverStateLimit), Is.EqualTo(full), $"level {level}");
                    if (full == SolveStatus.Unsolvable) unsolvable++;
                }
            }
            Assert.That(unsolvable, Is.GreaterThan(0), "sample must include unsolvable boards");
        }

        [Test]
        public void QuickCheckReportsUnsolvableAndLimit()
        {
            // Hiç hamlesi olmayan, çözülmemiş tahta.
            Assert.That(PuzzleSolver.CheckSolvable(new[] { new[] { WeatherType.Sun, WeatherType.Rain }, new[] { WeatherType.Rain, WeatherType.Sun } }, 2, 1000),
                Is.EqualTo(SolveStatus.Unsolvable));
            // Sınır: çözülmemiş ve tek durumdan fazlasına gitmek gereken tahta, 1 durum sınırıyla.
            V3LevelEntry entry = V3LevelData.Find(83);
            Assert.That(PuzzleSolver.CheckSolvable(entry.CreateBoard(), entry.Capacity, 1), Is.EqualTo(SolveStatus.LimitExceeded));
        }

        [Test]
        public void ShuffleWithExtraCloudsIsFastAndNeverFalselyRejected([Values(81, 83, 85, 90, 95, 100)] int level, [Values(0, 1, 2)] int extras)
        {
            // Telefondaki donma: 83 + 2 Extra (9 bulut) → tahta çözülebilir olduğu halde "güvenli karıştırma yok" ve dakikalar.
            V3LevelEntry entry = V3LevelData.Find(level);
            WeatherType[][] board = WithExtras(entry, extras);
            var sw = Stopwatch.StartNew();
            for (int cloud = 0; cloud < board.Length; cloud++)
            {
                if (!CloudShuffler.CanChange(board[cloud])) continue;
                sw.Restart();
                CloudShuffleResult result = CloudShuffler.Shuffle(board, entry.Capacity, cloud, entry.Seed + 997, SolverStateLimit);
                long ms = sw.ElapsedMilliseconds;
                // Telefon PC'den ~10 kat yavaş: PC'de 100 ms ≈ telefonda 1 sn.
                Assert.That(ms, Is.LessThan(100), $"level {level} +{extras}, cloud {cloud}");
                if (extras > 0)
                    Assert.That(result.Success, Is.True, $"level {level} +{extras}, cloud {cloud}: {result.Rejection}");
                if (result.Success)
                    Assert.That(PuzzleSolver.CheckSolvable(result.Board, entry.Capacity, SolverStateLimit), Is.EqualTo(SolveStatus.Solved));
            }
        }

        [Test]
        public void FailRecoveryWithExtraCloudsIsFast()
        {
            V3LevelEntry entry = V3LevelData.Find(83);
            WeatherType[][] board = WithExtras(entry, 2);
            var sw = Stopwatch.StartNew();
            int index = FailRecovery.NewestSolvableIndex(new[] { board }, entry.Capacity, SolverStateLimit);
            Assert.That(index, Is.EqualTo(0));
            Assert.That(sw.ElapsedMilliseconds, Is.LessThan(100));
        }
    }
}
