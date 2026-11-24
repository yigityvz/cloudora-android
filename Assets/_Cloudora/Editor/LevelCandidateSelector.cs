using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    // Hedef zorluk puanı aralığı (uçlar dahil).
    public readonly struct ScoreWindow
    {
        public readonly float Min;
        public readonly float Max;

        public ScoreWindow(float min, float max)
        {
            if (float.IsNaN(min) || float.IsNaN(max) || min > max) throw new ArgumentOutOfRangeException(nameof(min));
            Min = min;
            Max = max;
        }

        public float Center => (Min + Max) * 0.5f;
        public bool Contains(float score) => score >= Min && score <= Max;
        public override string ToString() => $"[{Min:F1}, {Max:F1}]";
    }

    public sealed class LevelCandidate
    {
        public int Seed { get; }
        public BoardFrame Frame { get; }
        public WeatherType[][] Board { get; }
        public string CanonicalKey { get; }
        public PuzzleSolveResult Result { get; }
        public float Score => Result.Metrics.Score;

        public LevelCandidate(int seed, BoardFrame frame, WeatherType[][] board, string canonicalKey, PuzzleSolveResult result)
        {
            Seed = seed;
            Frame = frame;
            Board = board;
            CanonicalKey = canonicalKey;
            Result = result;
        }

        public LevelDefinition ToLevelDefinition(int levelId, string worldId, string tutorialCue)
        {
            if (Result.Status != SolveStatus.Solved) throw new InvalidOperationException("Only solved candidates can become levels.");
            return new LevelDefinition(levelId, worldId, Frame.Capacity, tutorialCue, Board)
            {
                seed = Seed,
                generated = false,
                knownSolution = (Move[])Result.Solution.Clone(),
                difficultyScore = Score
            };
        }
    }

    public sealed class CandidateSelection
    {
        public bool Success { get; internal set; }
        public string Failure { get; internal set; }
        public LevelCandidate Primary { get; internal set; }
        public IReadOnlyList<LevelCandidate> Backups { get; internal set; } = Array.Empty<LevelCandidate>();
        public IReadOnlyList<LevelCandidate> Measured { get; internal set; } = Array.Empty<LevelCandidate>();
        public int Unsolvable { get; internal set; }
        public int LimitExceeded { get; internal set; }
        public int Duplicates { get; internal set; }
    }

    // Çerçeveye uygun adayları ardışık seed'lerle üretir, ölçer; hedef aralığın merkezine en yakın olanı
    // ana tahta, sonrakileri (farklı tahtalar) Shuffle yedeği olarak seçer. Aynı girdi her zaman aynı sonucu verir.
    public static class LevelCandidateSelector
    {
        public static CandidateSelection Select(BoardFrame frame, ScoreWindow window, int firstSeed,
            int candidateCount, int backupCount, int maxStates)
        {
            if (candidateCount < 1) throw new ArgumentOutOfRangeException(nameof(candidateCount));
            if (backupCount < 0) throw new ArgumentOutOfRangeException(nameof(backupCount));

            var selection = new CandidateSelection();
            var measured = new List<LevelCandidate>();
            var seen = new HashSet<string>();
            for (int i = 0; i < candidateCount; i++)
            {
                int seed = firstSeed + i;
                WeatherType[][] board = LevelCandidateFactory.Deal(frame, seed);
                string key = PuzzleSolver.CanonicalKey(board, frame.Capacity);
                if (!seen.Add(key))
                {
                    selection.Duplicates++;
                    continue;
                }

                PuzzleSolveResult result = PuzzleSolver.Solve(board,frame.Capacity,maxStates,lockCompleted:false);
                if (result.Status == SolveStatus.Unsolvable) selection.Unsolvable++;
                if (result.Status == SolveStatus.LimitExceeded) selection.LimitExceeded++;
                measured.Add(new LevelCandidate(seed, frame, board, key, result));
            }
            selection.Measured = measured;

            List<LevelCandidate> fits = measured
                .Where(c => c.Result.Status == SolveStatus.Solved && window.Contains(c.Score))
                .OrderBy(c => Math.Abs(c.Score - window.Center))
                .ThenBy(c => c.Seed)
                .ToList();

            if (fits.Count == 0)
            {
                selection.Failure = $"{frame}: no candidate in {window} " +
                    $"(measured {measured.Count}, unsolvable {selection.Unsolvable}, over limit {selection.LimitExceeded}, duplicates {selection.Duplicates}).";
                return selection;
            }

            selection.Primary = fits[0];
            selection.Backups = fits.Skip(1).Take(backupCount).ToList();
            selection.Success = fits.Count >= 1 + backupCount;
            if (!selection.Success)
                selection.Failure = $"{frame}: only {fits.Count} candidate(s) in {window}, need {1 + backupCount} (primary + backups).";
            return selection;
        }
    }
}
