using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;

namespace Cloudora.Level
{
    public sealed class V3LevelResult
    {
        public int Level { get; }
        public DifficultyBand Band { get; }
        public BoardFrame Frame { get; }
        public ScoreWindow Window { get; }
        public LevelCandidate Primary { get; }
        public float Target { get; }        // çerçeve merdiveninde (16+) bölümün mutlak hedef puanı; 1–15'te NaN
        public V3Novelty Novelty { get; }   // bölümün getirdiği yenilik (16+)

        public V3LevelResult(int level, DifficultyBand band, BoardFrame frame, ScoreWindow window, LevelCandidate primary,
            float target = float.NaN, V3Novelty novelty = V3Novelty.None)
        {
            Level = level;
            Band = band;
            Frame = frame;
            Window = window;
            Primary = primary;
            Target = target;
            Novelty = novelty;
        }
    }

    // Bir parçanın bölümlerini kurar: her çerçeve için adayları ölçer, bant aralıklarını dağılımdan çıkarır,
    // her bölüme aralıktan (öğretici yazıya uygun, parçada tekrar etmeyen) bir tahta seçer.
    // Aynı banttaki bölümler parça içinde aralığın alt ucundan üst ucuna doğru hafifçe zorlaşır.
    public static class V3LevelBuilder
    {
        public const int FirstSeed = 1;
        public const int CandidatesPerFrame = 400;
        public const int MaxStates = 300000;

        public static IReadOnlyList<V3LevelResult> BuildPart(V3Part part)
        {
            // 16+: çerçeveyi merdiven seçer (v3.2). 1–15 aşağıdaki sabit yoldan, değişmeden kurulur.
            if (V3LadderBuilder.Supports(part)) return V3LadderBuilder.BuildPart(part);
            // 31+: tema kurucusu (Faz 2.2).
            if (V3ThemeBuilder.Supports(part)) return V3ThemeBuilder.BuildPart(part);

            (int first, int last) = V3LevelPlan.RangeOf(part);
            int[] levels = Enumerable.Range(first, last - first + 1).ToArray();
            BoardFrame[] frames = levels.Select(V3LevelPlan.FrameFor).ToArray(); // tasarlanmamış parça burada durur

            var pools = new Dictionary<string, List<LevelCandidate>>();
            var windows = new Dictionary<string, Dictionary<DifficultyBand, ScoreWindow>>();
            foreach (BoardFrame frame in frames)
            {
                string key = frame.ToString();
                if (pools.ContainsKey(key)) continue;
                List<LevelCandidate> solved = Measure(frame);
                pools[key] = solved;
                windows[key] = WindowsFor(solved);
            }

            var used = new HashSet<string>();
            var occurrences = new Dictionary<string, int>();
            var results = new List<V3LevelResult>();
            for (int i = 0; i < levels.Length; i++)
            {
                int level = levels[i];
                BoardFrame frame = frames[i];
                DifficultyBand band = V3LevelPlan.BandFor(level);
                ScoreWindow window = windows[frame.ToString()][band];

                string slot = frame + "|" + band;
                int total = levels.Where((l, j) => frames[j].ToString() == frame.ToString() && V3LevelPlan.BandFor(l) == band).Count();
                occurrences.TryGetValue(slot, out int occurrence);
                occurrences[slot] = occurrence + 1;
                float target = window.Min + (occurrence + 0.5f) / total * (window.Max - window.Min);

                List<LevelCandidate> ranked = pools[frame.ToString()]
                    .Where(c => window.Contains(c.Score) && !used.Contains(c.CanonicalKey) && DemonstratesCue(level, c))
                    .OrderBy(c => Math.Abs(c.Score - target))
                    .ThenBy(c => c.Seed)
                    .ToList();
                if (ranked.Count == 0)
                    throw new InvalidOperationException($"Level {level} ({band}, {frame}): no candidate in {window}.");

                LevelCandidate primary = ranked[0];
                used.Add(primary.CanonicalKey);
                results.Add(new V3LevelResult(level, band, frame, window, primary));
            }
            return results;
        }

        private static List<LevelCandidate> Measure(BoardFrame frame)
        {
            CandidateSelection all = LevelCandidateSelector.Select(frame, new ScoreWindow(float.MinValue, float.MaxValue),
                FirstSeed, CandidatesPerFrame, 0, MaxStates);
            return all.Measured
                .Where(c => c.Result.Status == SolveStatus.Solved)
                .OrderBy(c => c.Score)
                .ThenBy(c => c.Seed)
                .ToList();
        }

        // Bant aralıkları adayların puan dağılımından; aralıklar çakışmasın diye her biri öncekinin üstünden başlar.
        private static Dictionary<DifficultyBand, ScoreWindow> WindowsFor(List<LevelCandidate> sorted)
        {
            if (sorted.Count < 10) throw new InvalidOperationException($"Too few solvable candidates ({sorted.Count}) to form bands.");
            float[] scores = sorted.Select(c => c.Score).ToArray();
            var result = new Dictionary<DifficultyBand, ScoreWindow>();
            float floor = float.MinValue;
            foreach (DifficultyBand band in new[] { DifficultyBand.Medium, DifficultyBand.Hard, DifficultyBand.VeryHard })
            {
                (float low, float high) = V3LevelPlan.QuantilesFor(band);
                float min = At(scores, low);
                float max = At(scores, high);
                if (min <= floor) min = scores.FirstOrDefault(s => s > floor);
                if (min <= floor) throw new InvalidOperationException($"No scores left above {floor} for {band}.");
                if (max < min) max = min;
                result[band] = new ScoreWindow(min, max);
                floor = max;
            }
            return result;
        }

        private static float At(float[] sorted, float quantile) =>
            sorted[(int)Math.Round(quantile * (sorted.Length - 1))];

        // Öğretici yazının gösterdiği şey tahtada gerçekten olmalı.
        private static bool DemonstratesCue(int level, LevelCandidate candidate)
        {
            Move[] solution = candidate.Result.Solution;
            switch (level)
            {
                case 3: // "Groups move together"
                    return solution.Any(m => m.Count >= 2);
                case 4: // "Empty clouds create room"
                    var state = new PuzzleState(candidate.Board.Select(c => new CloudState(candidate.Frame.Capacity, c)));
                    foreach (Move move in solution)
                    {
                        if (state.Clouds[move.TargetIndex].IsEmpty) return true;
                        PuzzleRules.TryApply(state, move);
                    }
                    return false;
                default:
                    return true;
            }
        }
    }
}
