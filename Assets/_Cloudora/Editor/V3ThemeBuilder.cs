using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudora.Level
{
    // Faz 2.2 (V3_SABLON.md): 31+ parçaların kurucusu. 16–30'un merdiveninden (V3LadderBuilder) ayrı tutulur ki kullanıcı
    // testinden geçmiş 16–30 planı değişmesin; aday havuzları, eşikler ve önceki tahtalar oradan paylaşılır.
    // - Hedef: şablonun eğrisi (her bölüm parçası için Orta / Zor / Çok zor başı → sonu, döngüler boyunca doğrusal).
    // - Renk: bölüm parçasının ilk bölümünde yeni renk (yenilik, Orta); renk sayısı geri düşmez.
    // - Dizilim (üç katman): yeni tema / renkle yumuşak başlar (nefes alma), 1 açık ve klasik adımlarla zorlaşır;
    //   her adım tek başına bir Orta'da, şablondaki pencere içinde; adımdan sonra yeni dizilim her döngüde görünür,
    //   eski dizilimler de karışımda kalır.
    // - Tuzak (1 boş) + planlama (2 boş) karışımı (kullanıcı 18:10, 18:45): yeni renkle boş bulut nefes alır; 1 boş bir
    //   Orta'da yenilik olarak geri döner; her döngüdeki tuzak bölümü sayısı bölüm parçasının tablosundan, giderek artar.
    // - Ortak kurallar: hedefe en yakın tahta, döngü sırası, aynı bant kolaylaşmaz, tahta tekrarı yok, absürt uzunluk yok,
    //   No Moves zaman zaman ve bantlara dağılmış, kilitleyen bölüm yalnızca parça finalinde; final tuzaklı ve kısa değil.
    public static class V3ThemeBuilder
    {
        private sealed class LayoutStep
        {
            public int Open;
            public V3Novelty Novelty;
            public int WindowFirst, WindowLast;
        }

        private sealed class Segment
        {
            public int First, Last, Colors;
            public int[] StartOpens;
            public V3Novelty StartNovelty;
            public LayoutStep[] Steps;
            public (float From, float To) Medium, Hard, VeryHard;
            // 1 boşun geri döndüğü pencere (0: dönüş yok, tuzaklar tablodaki gibi baştan); dönüş bölümü kendi döngüsünün tuzağıdır.
            public int ReturnFirst, ReturnLast;
            public int[] TrapsByCycle;   // her döngüde tam bu kadar 1 boşlu bölüm (final ve dönüş bölümü hariç)
            // Döngünün tuzaklarından biri olarak 1 boşlu olması gereken Zor/Çok zor bölümler (kullanıcı 19:00: "hepsine olmasa da bazılarına").
            public int[] HardTrapLevels = Array.Empty<int>();
        }

        private sealed class ThemeConfig
        {
            public int Capacity, Empties;
            public Segment[] Segments;
            public int MinNoMoves, MaxNoMoves, MinNoMovesBands, MaxFirstMoveNoMoves;
            public bool FinaleMayUseSingleEmpty;   // tema finali "sınav": 1 boşlu tahta da aday (V3_SABLON.md)
            // Döngünün tuzak kotası dolmadıysa hedefe yakın 1 boşlu adaya verilen tercih: tuzaklar döngünün sonuna
            // (Çok zor'a) yığılmasın. Tema 3'te gerekli (61–100 döngüde 2 tuzak); Tema 2'de 0 (test edilmiş plan korunur).
            public float EarlyTrapPreference;
            // 1 boşlu (tuzak) çerçeveler için havuz büyüklüğü (0: varsayılan). Tema 3'te 4 renk + 1 boşta, hedefe yakın ve
            // kilitlemeyen tahta 400 adayda yetmedi (2026-10-15: 70/75/80'de aday yok).
            public int TrapPoolCandidates;
            // Hedef penceresinde aday yoksa tuzak çerçevesi bu büyüklükte bir havuzla yeniden aranır (0: yok). Tema 2'de yalnızca
            // 59'u etkiler (eski seçim hedeften 1,21 uzaktı; kullanıcı 19:15: "sadece o oynasın").
            public int FallbackTrapPoolCandidates;
        }

        // Tema finalinde hedefe yakın adaylar arasında tuzağı (takılma + çıkmaz) fazla olan tercih edilir.
        private const float FinaleTrapPreference = 3.0f;

        private static readonly Dictionary<V3Part, ThemeConfig> Configs = new()
        {
            [V3Part.Theme2] = new ThemeConfig
            {
                Capacity = 5, Empties = 2,
                MinNoMoves = 3, MaxNoMoves = 6, MinNoMovesBands = 2, MaxFirstMoveNoMoves = 1, FinaleMayUseSingleEmpty = true,
                FallbackTrapPoolCandidates = 3000,
                Segments = new[]
                {
                    new Segment
                    {
                        First = 31, Last = 60, Colors = 4, StartOpens = new[] { 0 }, StartNovelty = V3Novelty.NewColor,
                        Steps = new[]
                        {
                            new LayoutStep { Open = 1, Novelty = V3Novelty.OneOpenLayout, WindowFirst = 39, WindowLast = 44 },
                            new LayoutStep { Open = 2, Novelty = V3Novelty.ClassicLayout, WindowFirst = 49, WindowLast = 54 }
                        },
                        Medium = (20.0f, 24.0f), Hard = (22.5f, 26.5f), VeryHard = (25.5f, 28.5f),
                        ReturnFirst = 36, ReturnLast = 44, TrapsByCycle = new[] { 0, 0, 1, 1, 1, 1 }
                    }
                }
            },
            [V3Part.Theme3] = new ThemeConfig
            {
                Capacity = 5, Empties = 2,
                MinNoMoves = 5, MaxNoMoves = 12, MinNoMovesBands = 2, MaxFirstMoveNoMoves = 2, FinaleMayUseSingleEmpty = true,
                EarlyTrapPreference = 1.0f, TrapPoolCandidates = 3000,
                Segments = new[]
                {
                    // 61–80: 4 renk; yeni tema (görsel) ile dizilim nefes alır (0/1 açık), klasik ~71; tuzak döngüde 1 → 2.
                    new Segment
                    {
                        First = 61, Last = 80, Colors = 4, StartOpens = new[] { 0, 1 }, StartNovelty = V3Novelty.None,
                        Steps = new[] { new LayoutStep { Open = 2, Novelty = V3Novelty.ClassicLayout, WindowFirst = 69, WindowLast = 74 } },
                        Medium = (24.0f, 26.0f), Hard = (26.5f, 28.5f), VeryHard = (28.5f, 30.5f),
                        TrapsByCycle = new[] { 1, 1, 2, 2 }, HardTrapLevels = new[] { 75, 78 }
                    },
                    // 81–100: Ay (5. renk); dizilim 0 açıkla başlar, 1 açık ~86, klasik ~91; boş bulut 81–85 nefes alır,
                    // 1 boş 86–89'da döner, sonra döngüde 2 tuzak.
                    new Segment
                    {
                        First = 81, Last = 100, Colors = 5, StartOpens = new[] { 0 }, StartNovelty = V3Novelty.NewColor,
                        Steps = new[]
                        {
                            new LayoutStep { Open = 1, Novelty = V3Novelty.OneOpenLayout, WindowFirst = 84, WindowLast = 89 },
                            new LayoutStep { Open = 2, Novelty = V3Novelty.ClassicLayout, WindowFirst = 89, WindowLast = 94 }
                        },
                        Medium = (26.5f, 29.5f), Hard = (29.0f, 32.0f), VeryHard = (31.0f, 34.0f),
                        ReturnFirst = 86, ReturnLast = 89, TrapsByCycle = new[] { 0, 0, 2, 2 }, HardTrapLevels = new[] { 95, 98 }
                    }
                }
            }
        };

        public static bool Supports(V3Part part) => Configs.ContainsKey(part);
        public static int MinNoMoves(V3Part part) => Configs[part].MinNoMoves;
        public static int MaxNoMoves(V3Part part) => Configs[part].MaxNoMoves;

        // Çerçevenin kullanılabilir tahtalarında en kısa çözümün ortancası (final bundan kısa olamaz; kullanıcı 18:42).
        private static readonly Dictionary<string, int> Medians = new();

        public static int MedianMoves(BoardFrame frame)
        {
            string key = frame.ToString();
            if (Medians.TryGetValue(key, out int cached)) return cached;
            int[] moves = V3LadderBuilder.Pool(frame).Select(c => c.Result.Metrics.OptimalMoves).OrderBy(m => m).ToArray();
            return Medians[key] = moves[moves.Length / 2];
        }

        // ---------- Hedef eğrisi ----------

        public static IReadOnlyList<float> TargetsFor(V3Part part)
        {
            ThemeConfig config = Configs[part];
            var targets = new List<float>();
            foreach (Segment s in config.Segments)
            {
                int cycles = (s.Last - s.First + 1) / 5;
                for (int level = s.First; level <= s.Last; level++)
                {
                    int cycle = (level - s.First) / 5, position = (level - 1) % 5;
                    switch (V3LevelPlan.BandFor(level))
                    {
                        case DifficultyBand.Medium:
                            int order = position == 0 ? 0 : position == 1 ? 1 : 2;
                            targets.Add(Lerp(s.Medium, (cycle * 3 + order) / (float)(3 * cycles - 1)));
                            break;
                        case DifficultyBand.Hard:
                            targets.Add(Lerp(s.Hard, cycle / (float)(cycles - 1)));
                            break;
                        default:
                            targets.Add(Lerp(s.VeryHard, cycle / (float)(cycles - 1)));
                            break;
                    }
                }
            }
            return targets;
        }

        private static float Lerp((float From, float To) range, float t) => range.From + (range.To - range.From) * t;

        // ---------- Parça kurulumu ----------

        public static IReadOnlyList<V3LevelResult> BuildPart(V3Part part)
        {
            if (!Configs.TryGetValue(part, out ThemeConfig config))
                throw new NotSupportedException($"V3 part {part} is not designed yet.");
            (int first, int last) = V3LevelPlan.RangeOf(part);
            int[] levels = Enumerable.Range(first, last - first + 1).ToArray();
            IReadOnlyList<float> targets = TargetsFor(part);
            HashSet<string> previousBoards = V3LadderBuilder.PreviousBoards(first);

            int[] MediumsIn(int from, int to) => Enumerable.Range(from, to - from + 1)
                .Where(l => V3LevelPlan.BandFor(l) == DifficultyBand.Medium && config.Segments.All(s => s.First != l)).ToArray();
            // Olaylar: bütün dizilim adımları (parça sırasıyla), sonra her bölüm parçasının 1 boş dönüşü (yoksa 0).
            List<LayoutStep> steps = config.Segments.SelectMany(s => s.Steps).ToList();
            List<int[]> options = steps.Select(step => MediumsIn(step.WindowFirst, step.WindowLast)).ToList();
            options.AddRange(config.Segments.Select(s => s.ReturnFirst > 0 ? MediumsIn(s.ReturnFirst, s.ReturnLast) : new[] { 0 }));

            List<V3LevelResult> best = null;
            int[] bestEvents = null;
            double bestCost = double.MaxValue;
            var failures = new Dictionary<string, int>();
            foreach (int[] events in Combinations(options))
            {
                int[] placement = events.Take(steps.Count).ToArray();
                int[] returns = events.Skip(steps.Count).ToArray();
                if (!Valid(config, steps, placement, returns)) continue;
                List<V3LevelResult> plan = TryPlan(config, levels, targets, previousBoards, steps, placement, returns, false, out double cost, out string failure);
                if (plan == null) { failures[failure] = failures.TryGetValue(failure, out int n) ? n + 1 : 1; continue; }
                if (cost >= bestCost) continue;
                best = plan;
                bestEvents = events;
                bestCost = cost;
            }
            // Yedek havuz plan seçimini değiştirmesin: yalnızca seçilen planın üzerinde, hedeften uzak tuzak bölümleri için.
            if (best != null && config.FallbackTrapPoolCandidates > 0)
                best = TryPlan(config, levels, targets, previousBoards, steps, bestEvents.Take(steps.Count).ToArray(),
                    bestEvents.Skip(steps.Count).ToArray(), true, out _, out _) ?? best;
            return best ?? throw new InvalidOperationException($"No valid theme plan for {part}. Most common: " +
                string.Join("; ", failures.OrderByDescending(f => f.Value).Take(5).Select(f => $"{f.Key} ×{f.Value}")));
        }

        // Aynı bölüm parçasındaki dizilim adımları artan sırada; bir bölümde tek yenilik.
        private static bool Valid(ThemeConfig config, List<LayoutStep> steps, int[] placement, int[] returns)
        {
            foreach (Segment s in config.Segments)
            {
                int[] own = steps.Select((step, k) => (step, k)).Where(x => s.Steps.Contains(x.step)).Select(x => placement[x.k]).ToArray();
                for (int i = 1; i < own.Length; i++) if (own[i] <= own[i - 1]) return false;
            }
            int[] all = placement.Concat(returns.Where(r => r > 0)).ToArray();
            return all.Distinct().Count() == all.Length;
        }

        private static IEnumerable<int[]> Combinations(List<int[]> options, int index = 0)
        {
            if (index == options.Count) { yield return Array.Empty<int>(); yield break; }
            foreach (int level in options[index])
            foreach (int[] rest in Combinations(options, index + 1))
                yield return new[] { level }.Concat(rest).ToArray();
        }

        // Dizilimin zorluk sırası (2 boşluk açık bulut ölçeğinde): klasik = Empties, serpiştirilmiş = açık bulut sayısı.
        private static int LayoutRank(BoardFrame frame, int empties) => frame.IsPacked ? empties : frame.OpenClouds;

        private static List<V3LevelResult> TryPlan(ThemeConfig config, int[] levels, IReadOnlyList<float> targets,
            HashSet<string> previousBoards, List<LayoutStep> steps, int[] placement, int[] returns, bool useFallbackPool, out double cost, out string failure)
        {
            cost = 0;
            failure = null;
            var used = new HashSet<string>(previousBoards);
            var results = new List<V3LevelResult>();
            var lastByBand = new Dictionary<DifficultyBand, float>();
            float cycleMediumMax = float.MinValue, cycleHard = float.NaN;
            int cycleOfCount = -1, noMovesCount = 0, firstMoveNoMoves = 0, cycleTraps = 0, cycleHardestOpen = -1;
            var noMovesBands = new HashSet<DifficultyBand>();
            int finale = levels[levels.Length - 1];
            bool softAfterLastStep = steps.Count == 0;
            float maxTrapSoFar = 0f;   // final, parçadaki bütün bölümlerden tuzaklı olmalı (V32ThemeTests.FinaleIsTheHardestTrapOfTheTheme)

            for (int i = 0; i < levels.Length; i++)
            {
                int level = levels[i];
                int cycle = (level - 1) / 5, position = (level - 1) % 5;
                int segmentIndex = Array.FindIndex(config.Segments, s => s.First <= level && level <= s.Last);
                Segment segment = config.Segments[segmentIndex];
                int returnLevel = returns[segmentIndex];
                int segmentCycle = (level - segment.First) / 5;
                if (cycle != cycleOfCount)
                {
                    cycleOfCount = cycle;
                    cycleMediumMax = float.MinValue;
                    cycleHard = float.NaN;
                    cycleHardestOpen = -1;
                    cycleTraps = 0;
                }

                DifficultyBand band = V3LevelPlan.BandFor(level);
                float target = targets[i];

                // Dizilim: bölüm parçasının başlangıcı + yerleşmiş adımlar; adım bölümünde yalnızca yenisi.
                V3Novelty novelty = level == segment.First ? segment.StartNovelty : V3Novelty.None;
                var opens = new SortedSet<int>(segment.StartOpens);
                int newestStepOpen = -1, newestStepLevel = 0;
                for (int k = 0; k < steps.Count; k++)
                {
                    if (!segment.Steps.Contains(steps[k]) || placement[k] > level) continue;
                    if (placement[k] == level) { opens = new SortedSet<int> { steps[k].Open }; novelty = steps[k].Novelty; }
                    else opens.Add(steps[k].Open);
                    if (placement[k] > newestStepLevel) { newestStepLevel = placement[k]; newestStepOpen = steps[k].Open; }
                }
                // Karışık geçiş: en son adımdan tamamen sonra gelen her döngüde yeni dizilim (ya da daha zoru) en az bir kez görünür.
                if (newestStepOpen >= 0 && cycle * 5 + 1 > newestStepLevel && position == 4 && cycleHardestOpen < newestStepOpen)
                    opens = new SortedSet<int>(opens.Where(o => o >= newestStepOpen));

                // Boş bulut: dönüşten önce 2 (dönüşü olmayan parçada tablo baştan geçerli); dönüş bölümünde 1 (yenilik);
                // dönüş döngüsünde başka tuzak yok; sonraki döngülerde tam TrapsByCycle tuzak (final hariç).
                // Döngüde ileride zorunlu tuzağı olan Zor/Çok zor bölümler kotadan ayrılır.
                int quota = segment.TrapsByCycle[segmentCycle] - segment.HardTrapLevels.Count(l => l > level && (l - 1) / 5 == cycle);
                int[] empties;
                if (segment.HardTrapLevels.Contains(level)) empties = new[] { 1 };
                else if (level == finale) empties = config.FinaleMayUseSingleEmpty ? new[] { config.Empties, 1 } : new[] { config.Empties };
                else if (returnLevel > 0 && level < returnLevel) empties = new[] { config.Empties };
                else if (level == returnLevel) { empties = new[] { 1 }; novelty = V3Novelty.FewerEmpty; }
                else if (returnLevel > 0 && cycle == (returnLevel - 1) / 5) empties = new[] { config.Empties };
                else
                {
                    // Zorunlu Zor/Çok zor tuzakları olan parçada diğer tuzaklar yalnızca Orta'da (geri kalan Zor/Çok zor planlama).
                    bool mediumOnly = segment.HardTrapLevels.Length > 0;
                    int remaining = Enumerable.Range(level, 5 - position).Count(l => l != finale && !segment.HardTrapLevels.Contains(l) &&
                                                                                     (!mediumOnly || V3LevelPlan.BandFor(l) == DifficultyBand.Medium));
                    empties = cycleTraps >= quota || (mediumOnly && band != DifficultyBand.Medium) ? new[] { config.Empties }
                        : quota - cycleTraps >= remaining ? new[] { 1 }
                        : new[] { config.Empties, 1 };
                }
                // 1 boşlu tahtada dizilim: klasik (tamamen boş tek bulut) yalnızca klasik adımdan sonra; yoksa serpiştirilmiş.
                var frames = empties.SelectMany(e => e == config.Empties
                    ? opens.Select(o => new BoardFrame(config.Capacity, segment.Colors, e, o))
                    : new[] { opens.Contains(config.Empties) ? 1 : -1, opens.Contains(0) ? 0 : -1 }
                        .Where(o => o >= 0).Select(o => new BoardFrame(config.Capacity, segment.Colors, e, o))).ToList();

                LevelCandidate pick = null;
                BoardFrame pickFrame = default;
                float pickValue = float.MinValue;
                bool pickInWindow = false;
                bool wantNoMoves = noMovesCount < config.MaxNoMoves && (noMovesCount < config.MinNoMoves || !noMovesBands.Contains(band));
                for (int pass = 0; pass < 2; pass++)
                foreach (BoardFrame frame in frames)
                {
                    if (pass == 1 && (!useFallbackPool || pickInWindow || level == finale || config.FallbackTrapPoolCandidates == 0 || frame.EmptyCount != 1)) continue;
                    int longMoves = V3LadderBuilder.LongMovesThreshold(frame);
                    int minMoves = level == finale ? MedianMoves(frame) : 0;
                    int poolSize = pass == 1 ? config.FallbackTrapPoolCandidates : frame.EmptyCount == 1 ? config.TrapPoolCandidates : 0;
                    foreach (LevelCandidate c in V3LadderBuilder.Pool(frame, poolSize))
                    {
                        if (used.Contains(c.CanonicalKey)) continue;
                        if (lastByBand.TryGetValue(band, out float last) && c.Score < last) continue;
                        if (band == DifficultyBand.Hard && c.Score <= cycleMediumMax) continue;
                        if (band == DifficultyBand.VeryHard && !(c.Score > cycleHard)) continue;
                        if (band == DifficultyBand.Medium && !float.IsNaN(cycleHard) && c.Score >= cycleHard) continue;
                        DifficultyMetrics m = c.Result.Metrics;
                        if (m.NoMovesDepth >= 0 && noMovesCount >= config.MaxNoMoves) continue;
                        if (m.NoMovesDepth == 1 && firstMoveNoMoves >= config.MaxFirstMoveNoMoves) continue;
                        if (m.OptimalMoves > longMoves || m.OptimalMoves < minMoves) continue;
                        if (level == finale && m.NaiveFailureRate + m.DeadStateRatio <= maxTrapSoFar) continue;
                        if (level != finale && m.NaiveFailureRate >= V3LadderBuilder.MaxNaiveFailureBeforeFinale) continue;

                        float deviation = Math.Abs(c.Score - target);
                        bool inWindow = deviation <= V3LadderBuilder.TargetWindow;
                        float value = inWindow
                            ? -deviation + (wantNoMoves && m.NoMovesDepth >= 0 ? V3LadderBuilder.NoMovesBonus : 0f) +
                              (level == finale ? FinaleTrapPreference * (m.NaiveFailureRate + m.DeadStateRatio) : 0f) +
                              // Tercihin başladığı yer döngüden döngüye kayar (0, 1, 2. bölüm): tuzaklar tek banda yığılmaz.
                              (frame.EmptyCount == 1 && level != finale && cycleTraps < quota && position >= cycle % 3 ? config.EarlyTrapPreference : 0f)
                            : -deviation;
                        bool better = pick == null || (inWindow && !pickInWindow) ||
                                      (inWindow == pickInWindow && (value > pickValue || (value == pickValue && c.Seed < pick.Seed)));
                        if (!better) continue;
                        pick = c; pickFrame = frame; pickValue = value; pickInWindow = inWindow;
                    }
                }
                if (pick == null) { failure = $"no board at {level} ({band}, empties {string.Join("/", empties)}, opens {string.Join("/", opens)})"; return null; }

                if (pick.Result.Metrics.NoMovesDepth >= 0) { noMovesCount++; noMovesBands.Add(band); }
                if (pick.Result.Metrics.NoMovesDepth == 1) firstMoveNoMoves++;
                if (level != finale) maxTrapSoFar = Math.Max(maxTrapSoFar, pick.Result.Metrics.NaiveFailureRate + pick.Result.Metrics.DeadStateRatio);
                if (pickFrame.EmptyCount == 1 && level != finale && level != returnLevel) cycleTraps++;
                int layoutRank = LayoutRank(pickFrame, config.Empties);
                cycleHardestOpen = Math.Max(cycleHardestOpen, layoutRank);
                if (newestStepLevel != 0 && level > newestStepLevel && layoutRank < newestStepOpen) softAfterLastStep = true;
                used.Add(pick.CanonicalKey);
                lastByBand[band] = pick.Score;
                if (band == DifficultyBand.Medium) cycleMediumMax = Math.Max(cycleMediumMax, pick.Score);
                if (band == DifficultyBand.Hard) cycleHard = pick.Score;
                cost += (pick.Score - target) * (pick.Score - target);
                results.Add(new V3LevelResult(level, band, pickFrame, new ScoreWindow(target, target), pick, target, novelty));
            }

            if (noMovesCount < config.MinNoMoves || noMovesBands.Count < config.MinNoMovesBands) { failure = $"no-moves {noMovesCount}/{noMovesBands.Count} bands"; return null; }
            if (!softAfterLastStep) { failure = "no soft layout after last step"; return null; } // eski (yumuşak) dizilimler karışımda kalır
            return results;
        }
    }
}
