using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // v3.2 (tasarım §11i-a): "No Moves" ölçütü — başlangıçtan en az kaç hamlede hiç legal hamle kalmayan
    // (başarısızlık ekranını açan) bir duruma düşülebildiği. Puanı değiştirmez.
    public sealed class V32NoMovesTests
    {
        private const int Limit = 300000;

        [Test]
        public void Level24BoardCanFailOnTheFirstMove()
        {
            // 2026-10-15 Editor ve telefon testleri: Kar'ı boş buluta koyunca hiç hamle kalmıyor.
            DifficultyMetrics m = PuzzleSolver.Solve(V3BoardText.Parse("RRRRN/NNSRS/SNSNS/"), 5, Limit).Metrics;
            Assert.That(m.NoMovesDepth, Is.EqualTo(1));
            Assert.That(m.NoMovesStateRatio, Is.GreaterThan(0f));
        }

        [TestCase(4)]
        [TestCase(5)]
        public void ThreeColorsWithTwoEmptiesNeverRunOutOfMoves(int capacity)
        {
            var frame = new BoardFrame(capacity, 3, 2);
            for (int seed = 1; seed <= 30; seed++)
            {
                DifficultyMetrics m = PuzzleSolver.Solve(LevelCandidateFactory.Deal(frame, seed), capacity, Limit).Metrics;
                Assert.That(m.NoMovesDepth, Is.EqualTo(-1), $"{frame} seed {seed}");
                Assert.That(m.NoMovesStateRatio, Is.EqualTo(0f));
            }
        }

        [Test]
        public void NoMovesDepthMatchesIndependentSearch()
        {
            var frame = new BoardFrame(4, 3, 1);
            int withNoMoves = 0;
            for (int seed = 1; seed <= 25; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                int expected = ReferenceNoMovesDepth(board, frame.Capacity);
                Assert.That(PuzzleSolver.Solve(board, frame.Capacity, Limit).Metrics.NoMovesDepth, Is.EqualTo(expected), $"seed {seed}");
                if (expected > 0) withNoMoves++;
            }
            Assert.That(withNoMoves, Is.GreaterThan(0), "sample must contain boards that can run out of moves");
        }

        [Test]
        public void NoMovesDoesNotChangeTheScore()
        {
            // 1–15 sabit: yeni ölçüt puana katılmaz.
            V3LevelEntry entry = V3LevelData.Find(10);
            // Shipped seed/score calibration is archival; gameplay searches use the locked default.
            float score = PuzzleSolver.Solve(entry.CreateBoard(),entry.Capacity,Limit,lockCompleted:false).Metrics.Score;
            Assert.That(score, Is.EqualTo(entry.Score).Within(1e-4f));
        }

        [Test]
        public void ScoreAgreesWithEditorTestOnSingleEmptyBoards()
        {
            // 2026-10-15 12:52 Editor testi: 2 boşlu 20 (Çok zor) 9,8 sn'de rahat geçildi; 1 boşlu 26 (Orta, çıkmaz 0,33) ve
            // 30 (Çok zor, çıkmaz 0,27) tıkandı (43 sn, Shuffle). 24 (çıkmaz 0,08) 19,6 sn. Puan bu sırayı yakalamalı.
            float Score(string board) => PuzzleSolver.Solve(V3BoardText.Parse(board), 5, Limit).Metrics.Score;
            float l20 = Score("RNSNN/SRSRN/NSRRS//");
            float l24 = Score("SRRSR/NSNSN/NSNRR/");
            float l26 = Score("RNNRS/NSRSS/NRNRS/");
            float l30 = Score("NNSRN/RSNRS/SNRSR/");
            Assert.That(l26, Is.GreaterThan(l20), $"26 {l26} vs 20 {l20}");
            Assert.That(l30, Is.GreaterThan(l20), $"30 {l30} vs 20 {l20}");
            Assert.That(l26, Is.GreaterThan(l24), $"26 {l26} vs 24 {l24}");
        }

        // Bağımsız referans: bulut sırası korunarak düz BFS, yalnızca oyunun PuzzleRules'ı ile.
        // Çözülmemiş ve hiç legal hamlesi olmayan ilk duruma uzaklık; yoksa -1.
        private static int ReferenceNoMovesDepth(WeatherType[][] board, int capacity)
        {
            var start = new PuzzleState(board.Select(c => new CloudState(capacity, c)));
            var seen = new HashSet<string> { Text(start) };
            var frontier = new List<PuzzleState> { start };
            for (int depth = 0; frontier.Count > 0; depth++)
            {
                var next = new List<PuzzleState>();
                foreach (PuzzleState state in frontier)
                {
                    List<PuzzleState> children = Children(state).ToList();
                    if (children.Count == 0 && !PuzzleRules.IsSolved(state)) return depth;
                    foreach (PuzzleState child in children)
                        if (seen.Add(Text(child))) next.Add(child);
                }
                frontier = next;
            }
            return -1;
        }

        private static IEnumerable<PuzzleState> Children(PuzzleState state)
        {
            for (int s = 0; s < state.Clouds.Count; s++)
            for (int t = 0; t < state.Clouds.Count; t++)
            {
                CloudState source = state.Clouds[s], target = state.Clouds[t];
                if (s == t || source.IsEmpty) continue;
                var move = new Move(s, t, Math.Min(source.TopGroupCount(), target.Capacity - target.Count), source.Top);
                PuzzleState child = state.Clone();
                if (PuzzleRules.TryApply(child, move)) yield return child;
            }
        }

        private static string Text(PuzzleState state) => string.Join("|", state.Clouds.Select(c => string.Join(",", c.Elements)));
    }
}
