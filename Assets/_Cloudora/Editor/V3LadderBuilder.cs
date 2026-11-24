using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudora.Level
{
    // Bir bölümün getirdiği yenilik (çerçeve merdiveninde bir adım).
    public enum V3Novelty { None, FewerEmpty, LargerCapacity, ScatteredLayout, NewColor, OneOpenLayout, ClassicLayout }

    // v3.2 çerçeve merdiveni (tasarım §11b–11e): 16+ parçalarda çerçeveyi araç, her bölümün mutlak hedef puanına göre seçer.
    // - Hedef puan: önceki parçanın her banttaki en yüksek puanından, her 5 bölümde GrowthPerCycle kadar artar.
    // - Kapasite ve renk sayısı geri düşmez; 1 boş bulut bir kez geldikten sonra 2 boşla karışık kullanılabilir,
    //   giriş döngüsünde yalnızca yenilik bölümünde, sonraki her döngüde tam bir fazla olur (1, 2, … — kademeli).
    // - Her yenilik ayrı bir Orta bölümde gelir; ayrılmış bölümlerde (16: Shuffle + tema) yenilik olmaz.
    // - Yeniliklerin hangi bölümlere konacağı, tüm yerleşimler denenerek hedefe en yakın plana göre seçilir.
    // - Hedefe en yakın tahta seçilir; hedefe yakın adaylar (TargetWindow) arasında Çok zor'da 1 boş bulut hafifçe öne alınır.
    //   Faz 2.1: uzunluk tercihi kaldırıldı (bölümleri sıkıcı uzatıyordu); çerçevesinin en uzun %10'luk dilimindeki tahtalar seçilmez.
    // - No Moves (hiç hamle kalmayan duruma düşülebilen) bölümler parça başına belli aralıkta ve birden çok bantta olur.
    // 1–15 bu yoldan geçmez (V3LevelBuilder, sabit).
    public static class V3LadderBuilder
    {
        public const float GrowthPerCycle = 1.0f;
        public const float VeryHardGrowthPerCycle = 1.2f;   // tasarım §11i
        public const float TargetTolerance = 1.5f;
        public const float TargetWindow = 0.6f;             // bu yakınlıktaki adaylar arasında tercih kuralları uygulanır
        public const float MaxProgressForHard = 0.5f;       // kaba yanıltıcılık filtresi (2 boşlu Zor/Çok zor)
        // Kilitleyen bölüm yalnızca tema finalinde (tasarım §11i, 13:12): finalden önce sanal oyuncu takılması bunun altında.
        public const float MaxNaiveFailureBeforeFinale = 0.5f;
        private const float SingleEmptyBonusForVeryHard = 0.8f; // ≈ bir hamle uzunluğundan az: hafif tercih
        internal const float NoMovesBonus = 2.0f;

        private static float Growth(DifficultyBand band) => band == DifficultyBand.VeryHard ? VeryHardGrowthPerCycle : GrowthPerCycle;

        // Sanal oyuncuların %80'inden fazlasının takıldığı tahtalar aşırı cezalandırıcı sayılır (ör. 14.10 raporundaki 132 puan).
        // Daha düşük eşik, 1 boş bulutun tuzaklı (asıl zorluğu veren) tahtalarını da eliyordu.
        public const float MaxNaiveFailureRate = 0.8f;

        private sealed class PartConfig
        {
            public BoardFrame Start;
            public int EndCapacity;
            public bool IntroduceSingleEmpty;
            public int[] ReservedLevels;
            public int MinNoMoves, MaxNoMoves, MinNoMovesBands, MaxFirstMoveNoMoves;
            public int MaxSinglesPerCycle;   // bir döngüde en fazla 1 boşlu bölüm (kademeli artışın tavanı)
            public bool IntroduceScattered;  // Faz 2.3: serpiştirilmiş dizilim bir yenilik olarak gelir, sonra klasikle karışık
            public int MinMixedAfterLayout;  // yenilikten sonra en az bu kadar serpiştirilmiş ve en az bu kadar klasik bölüm
        }

        private static readonly Dictionary<V3Part, PartConfig> Configs = new()
        {
            [V3Part.TransitionTheme] = new PartConfig
            {
                Start = new BoardFrame(4, 3, 2),   // 6–15'in çerçevesi
                EndCapacity = 5,
                IntroduceSingleEmpty = true,
                ReservedLevels = new[] { V3LevelPlan.ShuffleUnlockLevel },
                MinNoMoves = 2, MaxNoMoves = 3, MinNoMovesBands = 2,  // tasarım §11i; 1 boşlu bölüm 3'e inince 3–4 sağlanamadı (13:26)
                MaxFirstMoveNoMoves = 1,                              // ilk hamlede No Moves en fazla 1 bölümde (12:56)
                MaxSinglesPerCycle = 2,                               // ilk tema: 1 boşlu bölümler 1, 2 (13:20)
                IntroduceScattered = true, MinMixedAfterLayout = 2    // Faz 2.3 (V3_SABLON.md karar 1, 3)
            }
        };

        public static bool Supports(V3Part part) => Configs.ContainsKey(part);

        // ---------- Hedef eğrisi ----------

        // Parçadaki her bölümün hedef puanı (ilk bölümden sonuncuya).
        public static IReadOnlyList<float> TargetsFor(V3Part part)
        {
            (int first, int last) = V3LevelPlan.RangeOf(part);
            Dictionary<DifficultyBand, float> anchor = PreviousBandMaxima(first);
            var targets = new List<float>();
            for (int level = first; level <= last; level++)
            {
                int cycle = (level - first) / 5;
                DifficultyBand band = V3LevelPlan.BandFor(level);
                if (band == DifficultyBand.Medium)
                {
                    // Döngüdeki 1., 2. ve 4. bölüm: Orta'lar döngü içinde de hafifçe artar.
                    int position = (level - 1) % 5;
                    int order = position <= 1 ? position : 2;
                    targets.Add(anchor[band] + Growth(band) * (cycle + (order + 1) / 3f));
                }
                else
                {
                    targets.Add(anchor[band] + Growth(band) * (cycle + 1));
                }
            }
            return targets;
        }

        // Önceki bölümlerin (veri dosyasındaki, sabit) her banttaki en yüksek puanı: son 10 bölüm.
        private static Dictionary<DifficultyBand, float> PreviousBandMaxima(int firstLevel)
        {
            var result = new Dictionary<DifficultyBand, float>();
            for (int level = Math.Max(1, firstLevel - 10); level < firstLevel; level++)
            {
                V3LevelEntry entry = V3LevelData.Find(level)
                    ?? throw new InvalidOperationException($"Level {level} has no data; build the previous part first.");
                result[entry.Band] = result.TryGetValue(entry.Band, out float max) ? Math.Max(max, entry.Score) : entry.Score;
            }
            return result;
        }

        // ---------- Parça kurulumu ----------

        public static IReadOnlyList<V3LevelResult> BuildPart(V3Part part)
        {
            if (!Configs.TryGetValue(part, out PartConfig config))
                throw new NotSupportedException($"V3 part {part} is not designed yet.");

            (int first, int last) = V3LevelPlan.RangeOf(part);
            int[] levels = Enumerable.Range(first, last - first + 1).ToArray();
            IReadOnlyList<float> targets = TargetsFor(part);
            HashSet<string> previousBoards = PreviousBoards(first);

            int[] eligible = levels.Where(l => V3LevelPlan.BandFor(l) == DifficultyBand.Medium && !config.ReservedLevels.Contains(l)).ToArray();
            int[] capacityOptions = config.EndCapacity > config.Start.Capacity ? eligible : new[] { 0 };
            // 1 boş bulut son döngüden önce gelir; yoksa parça içinde sıklaşamaz.
            int[] emptyOptions = config.IntroduceSingleEmpty ? eligible.Where(l => (l - 1) / 5 < (last - 1) / 5).ToArray() : new[] { 0 };
            if (config.EndCapacity > config.Start.Capacity + 1)
                throw new NotSupportedException("One capacity step per part is supported.");

            int[] layoutOptions = config.IntroduceScattered ? eligible : new[] { 0 };

            List<V3LevelResult> best = null;
            double bestCost = double.MaxValue;
            foreach (int capacityLevel in capacityOptions)
            foreach (int emptyLevel in emptyOptions)
            foreach (int layoutLevel in layoutOptions)
            {
                // Bir bölümde tek yenilik.
                if (capacityLevel != 0 && capacityLevel == emptyLevel) continue;
                if (layoutLevel != 0 && (layoutLevel == capacityLevel || layoutLevel == emptyLevel)) continue;
                List<V3LevelResult> plan = TryPlan(config, levels, targets, previousBoards, capacityLevel, emptyLevel, layoutLevel, out double cost);
                if (plan == null || cost >= bestCost) continue;
                best = plan;
                bestCost = cost;
            }
            return best ?? throw new InvalidOperationException($"No valid ladder plan for {part}.");
        }

        private static List<V3LevelResult> TryPlan(PartConfig config, int[] levels, IReadOnlyList<float> targets,
            HashSet<string> previousBoards, int capacityLevel, int emptyLevel, int layoutLevel, out double cost)
        {
            int scatteredAfter = 0, packedAfter = 0;
            cost = 0;
            var used = new HashSet<string>(previousBoards);
            var results = new List<V3LevelResult>();
            var lastByBand = new Dictionary<DifficultyBand, float>();
            int capacity = config.Start.Capacity;
            bool singleEmptyArrived = false;
            int requiredSingles = 0, cycleSingles = 0, cycleOfCount = -1;
            float cycleMediumMax = float.MinValue, cycleHard = float.NaN;
            int noMovesCount = 0, firstMoveNoMoves = 0, veryHardLeft = levels.Count(l => V3LevelPlan.BandFor(l) == DifficultyBand.VeryHard), veryHardDouble = 0;
            var noMovesBands = new HashSet<DifficultyBand>();

            for (int i = 0; i < levels.Length; i++)
            {
                int level = levels[i];
                int cycle = (level - 1) / 5;
                if (cycle != cycleOfCount)
                {
                    // 1 boş bulut geldikten sonra her döngü, öncekinden en az bir fazla 1 boşlu bölüm alır.
                    if (cycleOfCount >= 0 && singleEmptyArrived)
                    {
                        requiredSingles = cycleSingles + 1;
                        if (requiredSingles > config.MaxSinglesPerCycle) return null; // erken giriş tavanı aşar: bu plan geçersiz
                    }
                    cycleOfCount = cycle;
                    cycleSingles = 0;
                    cycleMediumMax = float.MinValue;
                    cycleHard = float.NaN;
                }

                DifficultyBand band = V3LevelPlan.BandFor(level);
                float target = targets[i];
                V3Novelty novelty = V3Novelty.None;
                if (level == capacityLevel) { capacity = config.EndCapacity; novelty = V3Novelty.LargerCapacity; }
                if (level == layoutLevel) novelty = V3Novelty.ScatteredLayout;

                int[] empties;
                if (level == emptyLevel) { empties = new[] { 1 }; novelty = V3Novelty.FewerEmpty; }
                else if (!singleEmptyArrived) empties = new[] { config.Start.EmptyCount };
                else
                {
                    // Kademeli (kullanıcı, 13:20): giriş döngüsünde yalnızca yenilik bölümü; sonra her döngüde tam bir fazla.
                    int allowed = cycle == (emptyLevel - 1) / 5 ? 1 : requiredSingles;
                    int remaining = 5 - (level - 1) % 5;
                    bool mustBeSingle = requiredSingles - cycleSingles >= remaining;
                    // Son döngüde 1 boşluk haklarından biri tema finaline ayrılır (final "sınav").
                    int finale = levels[levels.Length - 1];
                    bool reserveForFinale = cycle == (finale - 1) / 5 && level != finale && cycleSingles >= allowed - 1;
                    empties = mustBeSingle ? new[] { 1 }
                        : cycleSingles >= allowed || reserveForFinale ? new[] { config.Start.EmptyCount }
                        : new[] { config.Start.EmptyCount, 1 };
                }

                // Çok zor'ların en az biri 2 boşlu kalır: son Çok zor'a kadar hiç 2 boşlu olmadıysa 1 boşlu seçilemez.
                if (band == DifficultyBand.VeryHard && veryHardLeft == 1 && veryHardDouble == 0)
                    empties = empties.Where(e => e != 1).ToArray();
                bool wantNoMoves = noMovesCount < config.MaxNoMoves && (noMovesCount < config.MinNoMoves || !noMovesBands.Contains(band));

                LevelCandidate pick = null;
                BoardFrame pickFrame = default;
                float pickValue = float.MinValue;
                bool pickInWindow = false;
                foreach (int empty in empties)
                foreach (int open in OpenOptions(empty, level, layoutLevel))
                {
                    var frame = new BoardFrame(capacity, config.Start.ColorCount, empty, open);
                    foreach (LevelCandidate c in Pool(frame))
                    {
                        if (used.Contains(c.CanonicalKey)) continue;
                        if (lastByBand.TryGetValue(band, out float last) && c.Score < last) continue;
                        if (band == DifficultyBand.Hard && c.Score <= cycleMediumMax) continue;
                        if (band == DifficultyBand.VeryHard && !(c.Score > cycleHard)) continue;
                        if (band == DifficultyBand.Medium && !float.IsNaN(cycleHard) && c.Score >= cycleHard) continue;
                        DifficultyMetrics m = c.Result.Metrics;
                        if (band != DifficultyBand.Medium && empty == 2 && m.ProgressMoveRatio > MaxProgressForHard) continue;
                        if (m.NoMovesDepth >= 0 && noMovesCount >= config.MaxNoMoves) continue;
                        if (m.NoMovesDepth == 1 && firstMoveNoMoves >= config.MaxFirstMoveNoMoves) continue;
                        if (m.OptimalMoves > LongMovesThreshold(frame)) continue; // absürt uzunluk yok (Faz 2.1)
                        if (level != levels[levels.Length - 1] && m.NaiveFailureRate >= MaxNaiveFailureBeforeFinale) continue;

                        // Hedefe yakın olanlar arasında tercih; hiç yakın aday yoksa en yakını.
                        float deviation = Math.Abs(c.Score - target);
                        bool inWindow = deviation <= TargetWindow;
                        float value = inWindow
                            ? -deviation +
                              (band == DifficultyBand.VeryHard && empty == 1 ? SingleEmptyBonusForVeryHard : 0f) +
                              (wantNoMoves && m.NoMovesDepth >= 0 ? NoMovesBonus : 0f)
                            : -deviation;
                        bool better = pick == null || (inWindow && !pickInWindow) ||
                                      (inWindow == pickInWindow && (value > pickValue || (value == pickValue && c.Seed < pick.Seed)));
                        if (!better) continue;
                        pick = c; pickFrame = frame; pickValue = value; pickInWindow = inWindow;
                    }
                }
                if (pick == null) return null;

                if (pick.Result.Metrics.NoMovesDepth >= 0) { noMovesCount++; noMovesBands.Add(band); }
                if (pick.Result.Metrics.NoMovesDepth == 1) firstMoveNoMoves++;
                if (band == DifficultyBand.VeryHard) { veryHardLeft--; if (pickFrame.EmptyCount == 2) veryHardDouble++; }
                if (level == emptyLevel) singleEmptyArrived = true;
                if (pickFrame.EmptyCount == 1) cycleSingles++;
                if (layoutLevel != 0 && level > layoutLevel) { if (pickFrame.IsPacked) packedAfter++; else scatteredAfter++; }
                used.Add(pick.CanonicalKey);
                lastByBand[band] = pick.Score;
                if (band == DifficultyBand.Medium) cycleMediumMax = Math.Max(cycleMediumMax, pick.Score);
                if (band == DifficultyBand.Hard) cycleHard = pick.Score;
                cost += (pick.Score - target) * (pick.Score - target);
                results.Add(new V3LevelResult(level, band, pickFrame, new ScoreWindow(target, target), pick, target, novelty));
            }

            // Son döngünün de sıklaşma kuralına uyması gerekir.
            if (singleEmptyArrived && cycleSingles < requiredSingles) return null;
            // No Moves zaman zaman ve birden çok bantta.
            if (noMovesCount < config.MinNoMoves || noMovesBands.Count < config.MinNoMovesBands) return null;
            // Serpiştirilmiş dizilim yenilikten sonra klasikle karışık kullanılır (ikisi de yeniden görünür).
            if (layoutLevel != 0 && (scatteredAfter + 1 < config.MinMixedAfterLayout || packedAfter < config.MinMixedAfterLayout)) return null;
            return results;
        }

        // Bir bölümde denenebilecek dizilimler (tamamen boş bulut sayısı): yenilikten önce yalnızca klasik,
        // yenilik bölümünde yalnızca serpiştirilmiş, sonrasında ikisi karışık.
        private static IEnumerable<int> OpenOptions(int empty, int level, int layoutLevel)
        {
            if (layoutLevel == 0 || level < layoutLevel) return new[] { empty };
            IEnumerable<int> scattered = Enumerable.Range(0, empty);
            return level == layoutLevel ? scattered : scattered.Append(empty);
        }

        internal static HashSet<string> PreviousBoards(int firstLevel)
        {
            var keys = new HashSet<string>();
            for (int level = 1; level < firstLevel; level++)
            {
                V3LevelEntry entry = V3LevelData.Find(level);
                if (entry != null) keys.Add(PuzzleSolver.CanonicalKey(entry.CreateBoard(), entry.Capacity));
            }
            return keys;
        }

        // ---------- Aday havuzları ----------

        private static readonly Dictionary<string, List<LevelCandidate>> Pools = new();

        // Faz 2.1 (tasarım §11j): "absürt uzunluk" sınırı — çerçevenin kullanılabilir tahtalarında en kısa çözümün %90'lık değeri.
        private static readonly Dictionary<string, int> LongMoves = new();

        public static int LongMovesThreshold(BoardFrame frame)
        {
            string key = frame.ToString();
            if (LongMoves.TryGetValue(key, out int cached)) return cached;
            int[] moves = Pool(frame).Select(c => c.Result.Metrics.OptimalMoves).OrderBy(m => m).ToArray();
            return LongMoves[key] = moves[(int)Math.Round(0.9 * (moves.Length - 1))];
        }

        // Çerçevenin çözülebilir ve aşırı cezalandırıcı olmayan adayları (puana göre sıralı); oturum boyunca önbellekte.
        public static IReadOnlyList<LevelCandidate> Pool(BoardFrame frame) => Pool(frame, 0);

        // candidates > 0: o çerçeve için ayrı (daha büyük) havuz — varsayılan havuzu kullanan planlar değişmez.
        public static IReadOnlyList<LevelCandidate> Pool(BoardFrame frame, int candidates)
        {
            string key = candidates > 0 ? frame + "#" + candidates : frame.ToString();
            if (Pools.TryGetValue(key, out List<LevelCandidate> pool)) return pool;
            // 5+ renk + 1 boşta kullanılabilir tahta çok az (envanter: 400'de 11–20) → bu çerçevelerde daha çok seed denenir.
            int count = candidates > 0 ? candidates
                : frame.ColorCount >= 5 && frame.EmptyCount == 1 ? 3000 : V3LevelBuilder.CandidatesPerFrame;
            CandidateSelection all = LevelCandidateSelector.Select(frame, new ScoreWindow(float.MinValue, float.MaxValue),
                V3LevelBuilder.FirstSeed, count, 0, V3LevelBuilder.MaxStates);
            pool = all.Measured
                .Where(c => c.Result.Status == SolveStatus.Solved && c.Result.Metrics.NaiveFailureRate < MaxNaiveFailureRate)
                .OrderBy(c => c.Score)
                .ThenBy(c => c.Seed)
                .ToList();
            Pools[key] = pool;
            return pool;
        }
    }
}
