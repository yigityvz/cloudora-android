using System;
using System.Collections.Generic;
using Cloudora.Core;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public enum SolveStatus { Solved, Unsolvable, LimitExceeded }

    // Sanal oyuncunun tarzı. Sensible: ölçümde kullanılan. Random: yalnızca testlerde karşılaştırma için.
    public enum NaivePlayerStyle { Sensible, Random }

    // Bir tahtanın ölçülmüş zorluğu. Oranlar 0–1 arası; Score seçimde kullanılan tek sayı.
    public readonly struct DifficultyMetrics : IEquatable<DifficultyMetrics>
    {
        // Ağırlıklar ilk ayardır; telefon testlerinden sonra kalibre edilecek.
        public const float MoveWeight = 1f;
        public const float TrapWeight = 15f;
        public const float MisdirectionWeight = 10f;
        // v3.2 kalibrasyonu (2026-10-15 Editor testi): 1 boşlu bölümlerde insan süresi çıkmaz oranıyla birlikte arttı
        // (0,08 → 20 sn … 0,33 → tıkanma); 5 ile 26 (Orta) puanda 20'nin (Çok zor) altında kalıyordu. 2 boşlu tahtalarda
        // çıkmaz olmadığı için 1–15 puanları değişmez.
        public const float DeadStateWeight = 20f;
        public const float NaiveExcessWeight = 1.5f;
        public const float NaiveFailureWeight = 10f;

        public readonly int OptimalMoves;        // en kısa çözüm uzunluğu (-1: çözümsüz)
        public readonly int ReachableStates;     // ulaşılabilir farklı tahta durumu
        public readonly float DeadStateRatio;    // çözüme artık ulaşamayan durumların oranı
        public readonly float ProgressMoveRatio; // en kısa yollar üzerinde, çözüme yaklaştıran hamlelerin ortalama oranı
        public readonly float TrapMoveRatio;     // en kısa yollar üzerinde, çıkmaza götüren hamlelerin oranı
        public readonly float AverageChoices;    // en kısa yollar üzerinde, durum başına ortalama legal hamle
        public readonly float NaiveExcessMoves;  // sanal oyuncunun, çözdüğü oyunlarda en kısa çözüme göre ortalama fazla hamlesi
        public readonly float NaiveFailureRate;  // sanal oyuncunun takıldığı (çıkmaz / hamle sınırı) oyun oranı
        public readonly int NoMovesDepth;        // v3.2: hiç legal hamle kalmayan (No Moves) duruma en kısa uzaklık (-1: yok); puana katılmaz
        public readonly float NoMovesStateRatio; // v3.2: ulaşılabilir durumlar içinde No Moves durumlarının oranı; puana katılmaz

        public DifficultyMetrics(int optimalMoves, int reachableStates, float deadStateRatio,
            float progressMoveRatio, float trapMoveRatio, float averageChoices,
            float naiveExcessMoves = 0f, float naiveFailureRate = 0f, int noMovesDepth = -1, float noMovesStateRatio = 0f)
        {
            NoMovesDepth = noMovesDepth;
            NoMovesStateRatio = noMovesStateRatio;
            NaiveExcessMoves = naiveExcessMoves;
            NaiveFailureRate = naiveFailureRate;
            OptimalMoves = optimalMoves;
            ReachableStates = reachableStates;
            DeadStateRatio = deadStateRatio;
            ProgressMoveRatio = progressMoveRatio;
            TrapMoveRatio = trapMoveRatio;
            AverageChoices = averageChoices;
        }

        public float Score => OptimalMoves < 0
            ? float.NaN
            : OptimalMoves * MoveWeight + TrapMoveRatio * TrapWeight +
              (1f - ProgressMoveRatio) * MisdirectionWeight + DeadStateRatio * DeadStateWeight +
              NaiveExcessMoves * NaiveExcessWeight + NaiveFailureRate * NaiveFailureWeight;

        public bool Equals(DifficultyMetrics other) =>
            OptimalMoves == other.OptimalMoves && ReachableStates == other.ReachableStates &&
            DeadStateRatio.Equals(other.DeadStateRatio) && ProgressMoveRatio.Equals(other.ProgressMoveRatio) &&
            TrapMoveRatio.Equals(other.TrapMoveRatio) && AverageChoices.Equals(other.AverageChoices) &&
            NaiveExcessMoves.Equals(other.NaiveExcessMoves) && NaiveFailureRate.Equals(other.NaiveFailureRate) &&
            NoMovesDepth == other.NoMovesDepth && NoMovesStateRatio.Equals(other.NoMovesStateRatio);

        public override bool Equals(object obj) => obj is DifficultyMetrics other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(HashCode.Combine(OptimalMoves, ReachableStates, DeadStateRatio, ProgressMoveRatio, TrapMoveRatio, AverageChoices, NaiveExcessMoves, NaiveFailureRate), NoMovesDepth, NoMovesStateRatio);

        public override string ToString() =>
            $"moves={OptimalMoves} states={ReachableStates} dead={DeadStateRatio:F3} progress={ProgressMoveRatio:F3} trap={TrapMoveRatio:F3} choices={AverageChoices:F2} naive_excess={NaiveExcessMoves:F2} naive_fail={NaiveFailureRate:F3} no_moves_depth={NoMovesDepth} no_moves_states={NoMovesStateRatio:F3} score={Score:F2}";
    }

    public sealed class PuzzleSolveResult
    {
        public SolveStatus Status { get; }
        public DifficultyMetrics Metrics { get; }
        public Move[] Solution { get; }

        public PuzzleSolveResult(SolveStatus status, DifficultyMetrics metrics, Move[] solution)
        {
            Status = status;
            Metrics = metrics;
            Solution = solution;
        }
    }

    // Full graph uses current locked-completion rules by default. lockCompleted=false is
    // only for reproducing the shipped campaign's historical authoring calibration.
    // Tam durum uzayı araması (modifier'sız tahtalar için). Kurallar PuzzleRules ile birebir aynıdır:
    // hamle, kaynağın üst grubunu hedefin alabildiği kadar taşır; hedef boş, aynı renk ya da Rainbow olmalı.
    // Bulut sırası zorluğu değiştirmediği için durumlar sıralanmış bulut kodlarıyla (kanonik) tekilleştirilir.
    public static class PuzzleSolver
    {
        private const int Unreached = -1;

        public static PuzzleSolveResult Solve(IReadOnlyList<WeatherType[]> board, int capacity, int maxStates,
            NaivePlayerStyle naiveStyle = NaivePlayerStyle.Sensible, bool lockCompleted = true)
        {
            int[] start = EncodeBoard(board, capacity);
            if (maxStates < 1) throw new ArgumentOutOfRangeException(nameof(maxStates));

            var index = new Dictionary<StateKey, int>();
            var keys = new List<StateKey>();
            var states = new List<int[]>();
            var depth = new List<int>();
            var edges = new List<int[]>();
            Register(start, 0);

            var children = new List<int>();
            for (int node = 0; node < states.Count; node++)
            {
                children.Clear();
                int[] current = states[node];
                for (int s = 0; s < current.Length; s++)
                for (int t = 0; t < current.Length; t++)
                {
                    if (!TryMove(current, s, t, capacity, out int newSource, out int newTarget, out _,lockCompleted)) continue;
                    StateKey key = Key(current, s, newSource, t, newTarget);
                    if (!index.TryGetValue(key, out int child))
                    {
                        if (states.Count >= maxStates)
                            return new PuzzleSolveResult(SolveStatus.LimitExceeded,
                                new DifficultyMetrics(-1, states.Count, 0f, 0f, 0f, 0f), Array.Empty<Move>());
                        child = Register(With(current, s, newSource, t, newTarget), depth[node] + 1);
                    }
                    children.Add(child);
                }
                edges.Add(children.ToArray());
            }

            int[] toGoal = DistancesToGoal(states, edges, capacity);
            int count = states.Count;
            int dead = 0;
            foreach (int d in toGoal) if (d == Unreached) dead++;
            float deadRatio = dead / (float)count;
            int optimal = toGoal[0];

            // No Moves: çözülmemiş ve hiç legal hamlesi olmayan durum (oyun burada başarısızlık ekranını açar).
            // depth BFS sırasıyla verildiği için en kısa uzaklıktır.
            int noMovesDepth = -1, noMovesStates = 0;
            for (int i = 0; i < count; i++)
            {
                if (edges[i].Length != 0 || toGoal[i] == 0) continue;
                noMovesStates++;
                if (noMovesDepth < 0 || depth[i] < noMovesDepth) noMovesDepth = depth[i];
            }
            float noMovesRatio = noMovesStates / (float)count;

            if (optimal == Unreached)
                return new PuzzleSolveResult(SolveStatus.Unsolvable,
                    new DifficultyMetrics(-1, count, deadRatio, 0f, 0f, 0f, 0f, 0f, noMovesDepth, noMovesRatio), Array.Empty<Move>());
            if (optimal == 0)
                return new PuzzleSolveResult(SolveStatus.Solved,
                    new DifficultyMetrics(0, count, deadRatio, 1f, 0f, 0f, 0f, 0f, noMovesDepth, noMovesRatio), Array.Empty<Move>());

            // En kısa yollar üzerindeki durumlar: başlangıçtan uzaklık + çözüme uzaklık = en kısa çözüm.
            // Toplama, sonuç bulut sırasından bağımsız olsun diye kanonik anahtar sırasıyla yapılır.
            var onPath = new List<int>();
            for (int i = 0; i < count; i++)
                if (toGoal[i] > 0 && depth[i] + toGoal[i] == optimal) onPath.Add(i);
            onPath.Sort((a, b) => keys[a].CompareTo(keys[b]));

            double progressSum = 0;
            long trapMoves = 0, legalMoves = 0;
            foreach (int i in onPath)
            {
                int good = 0, traps = 0;
                foreach (int c in edges[i])
                {
                    if (toGoal[c] == Unreached) traps++;
                    else if (toGoal[c] == toGoal[i] - 1) good++;
                }
                progressSum += good / (double)edges[i].Length;
                trapMoves += traps;
                legalMoves += edges[i].Length;
            }

            (float naiveExcess, float naiveFailure) = SimulateNaivePlayer(states, keys, index, toGoal, capacity, optimal, naiveStyle,lockCompleted);
            var metrics = new DifficultyMetrics(optimal, count, deadRatio,
                (float)(progressSum / onPath.Count), trapMoves / (float)legalMoves, legalMoves / (float)onPath.Count,
                naiveExcess, naiveFailure, noMovesDepth, noMovesRatio);
            return new PuzzleSolveResult(SolveStatus.Solved, metrics, BuildSolution(start, capacity, index, toGoal,lockCompleted));

            int Register(int[] state, int stateDepth)
            {
                StateKey key = Key(state);
                index.Add(key, states.Count);
                keys.Add(key);
                states.Add(state);
                depth.Add(stateDepth);
                return states.Count - 1;
            }
        }

        // ---- Sanal oyuncu: ileriyi hesaplamayan, ilk bakışta mantıklı görünen hamleyi seçen oyuncu. ----
        // Her hamleye basit bir çekicilik puanı verilir (bulutu tamamlamak, aynı renge istiflemek iyi; tamamlanmış
        // bulutu bozmak, bütün bir bulutu boşa taşımak, son hamleyi geri almak kötü) ve hamle bu puanlara göre
        // olasılıkla seçilir. Seçenekler kanonik sırayla ele alınır, böylece sonuç bulut sırasından bağımsızdır.
        // Çıkmaz bir duruma düşmek ya da hamle sınırını aşmak "takılma" sayılır.
        public const int NaiveRuns = 200;
        private const double ReversalFactor = 0.05; // ≈ e^-3: az önceki duruma geri dönmek çok nadir

        private static (float Excess, float Failure) SimulateNaivePlayer(List<int[]> states, List<StateKey> keys,
            Dictionary<StateKey, int> index, int[] toGoal, int capacity, int optimal, NaivePlayerStyle style,bool lockCompleted)
        {
            var options = new Dictionary<int, (int[] Children, double[] Weights)>();
            var random = new DeterministicRandom(keys[0].GetHashCode());
            int moveCap = optimal * 4 + 20;
            long excess = 0;
            int solved = 0, failed = 0;
            for (int run = 0; run < NaiveRuns; run++)
            {
                int node = 0, previous = -1, moves = 0;
                bool stuck = false;
                while (toGoal[node] != 0)
                {
                    if (toGoal[node] == Unreached || moves >= moveCap) { stuck = true; break; }
                    if (!options.TryGetValue(node, out var choice))
                        options[node] = choice = NaiveOptions(states[node], keys[node], index, capacity, style,lockCompleted);
                    if (choice.Children.Length == 0) { stuck = true; break; }

                    double total = 0;
                    for (int i = 0; i < choice.Children.Length; i++) total += Weight(choice, i, previous);
                    double pick = random.NextULong() / (double)ulong.MaxValue * total;
                    int next = choice.Children[choice.Children.Length - 1];
                    for (int i = 0; i < choice.Children.Length; i++)
                    {
                        pick -= Weight(choice, i, previous);
                        if (pick <= 0) { next = choice.Children[i]; break; }
                    }
                    previous = node;
                    node = next;
                    moves++;
                }
                if (stuck) failed++;
                else { solved++; excess += moves - optimal; }
            }
            float averageExcess = solved > 0 ? excess / (float)solved : moveCap - optimal;
            return (averageExcess, failed / (float)NaiveRuns);
        }

        private static double Weight((int[] Children, double[] Weights) choice, int i, int previous) =>
            choice.Children[i] == previous ? choice.Weights[i] * ReversalFactor : choice.Weights[i];

        // Durumdan gidilebilen farklı tahtalar (kanonik sırayla) ve her birine giden en çekici hamlenin ağırlığı.
        private static (int[] Children, double[] Weights) NaiveOptions(int[] state, StateKey self,
            Dictionary<StateKey, int> index, int capacity, NaivePlayerStyle style,bool lockCompleted)
        {
            var best = new Dictionary<StateKey, double>();
            for (int s = 0; s < state.Length; s++)
            for (int t = 0; t < state.Length; t++)
            {
                if (!TryMove(state, s, t, capacity, out int newSource, out int newTarget, out Move move,lockCompleted)) continue;
                StateKey key = Key(state, s, newSource, t, newTarget);
                if (key.Equals(self)) continue; // tahtayı değiştirmeyen hamle (bütün bulutu boş buluta taşımak)
                double weight = style == NaivePlayerStyle.Random ? 1.0 : Math.Exp(Appeal(state[s], state[t], move.Count, capacity));
                if (!best.TryGetValue(key, out double current) || weight > current) best[key] = weight;
            }

            var ordered = new List<StateKey>(best.Keys);
            ordered.Sort((a, b) => a.CompareTo(b));
            var children = new int[ordered.Count];
            var weights = new double[ordered.Count];
            for (int i = 0; i < ordered.Count; i++)
            {
                children[i] = index[ordered[i]];
                weights[i] = best[ordered[i]];
            }
            return (children, weights);
        }

        private static double Appeal(int source, int target, int amount, int capacity)
        {
            int sourceCount = CountOf(source), targetCount = CountOf(target);
            int sourceGroup = TopGroup(source);
            bool targetMonochrome = targetCount == 0 || TopGroup(target) == targetCount;
            double appeal = 0;
            if (targetCount + amount == capacity && targetMonochrome) appeal += 4; // bulutu tamamlar
            if (targetCount > 0) appeal += 2;                                      // aynı renge istifler
            else appeal += sourceGroup == sourceCount ? -3 : 1;                     // boş buluta: bütün bulutu taşımak anlamsız, açmak faydalı
            if (sourceCount == capacity && sourceGroup == capacity) appeal -= 4;   // tamamlanmış bulutu bozar
            if (amount < sourceGroup) appeal -= 1;                                  // grubu böler
            return appeal;
        }

        // Bulut sırasından bağımsız tahta anahtarı (aynı tahtanın farklı dizilişleri aynı anahtarı verir).
        // Oyun içi hızlı kontrol (Shuffle, No Moves dönüşü): yalnızca "çözülebilir mi". Derinlik öncelikli arar ve ilk
        // çözümde durur; zorluk ölçüsü hesaplamaz. Solve bütün durum uzayını gezdiği için büyük tahtada (9 bulut) sınırı
        // aşıp çözülebilir tahtayı "LimitExceeded" sayıyordu ve telefonda dakikalarca sürüyordu (2026-10-15, Faz 2.6).
        // Önce dolu bir buluta yapılan hamleler denenir (boş buluta taşımak en son).
        public static SolveStatus CheckSolvable(IReadOnlyList<WeatherType[]> board, int capacity, int maxStates)
        {
            int[] start = EncodeBoard(board, capacity);
            if (maxStates < 1) throw new ArgumentOutOfRangeException(nameof(maxStates));
            if (IsSolved(start, capacity)) return SolveStatus.Solved;

            var seen = new HashSet<StateKey> { Key(start) };
            var stack = new Stack<int[]>();
            stack.Push(start);
            var toEmpty = new List<int[]>();
            var toFilled = new List<int[]>();
            while (stack.Count > 0)
            {
                int[] current = stack.Pop();
                toEmpty.Clear();
                toFilled.Clear();
                for (int s = 0; s < current.Length; s++)
                for (int t = 0; t < current.Length; t++)
                {
                    if (!TryMove(current, s, t, capacity, out int newSource, out int newTarget, out _)) continue;
                    StateKey key = Key(current, s, newSource, t, newTarget);
                    if (seen.Contains(key)) continue;
                    if (seen.Count >= maxStates) return SolveStatus.LimitExceeded;
                    seen.Add(key);
                    int[] next = With(current, s, newSource, t, newTarget);
                    if (IsSolved(next, capacity)) return SolveStatus.Solved;
                    (CountOf(current[t]) == 0 ? toEmpty : toFilled).Add(next);
                }
                // Yığın: son eklenen önce açılır → dolu buluta hamleler önce.
                foreach (int[] next in toEmpty) stack.Push(next);
                foreach (int[] next in toFilled) stack.Push(next);
            }
            return SolveStatus.Unsolvable;
        }

        // Resumable exact search. Exhaustion proves a loss; a per-frame time budget
        // only pauses the search and can never be confused with an unsolvable board.
        public sealed class ReachabilitySearch
        {
            private readonly int _capacity;
            private readonly Stack<int[]> _frontier = new();
            private readonly HashSet<StateKey> _seen = new();
            private readonly List<int[]> _empty = new(), _filled = new();
            public SolveStatus Status { get; private set; } = SolveStatus.LimitExceeded;
            public int VisitedStates => _seen.Count;
            public ReachabilitySearch(IReadOnlyList<WeatherType[]> board, int capacity)
            {
                _capacity=capacity;var start=EncodeBoard(board,capacity);
                if(IsSolved(start,capacity)){Status=SolveStatus.Solved;return;}
                _frontier.Push(start);_seen.Add(Key(start));
            }
            public SolveStatus Step(int maxNodes=128, double milliseconds=2)
            {
                if(Status!=SolveStatus.LimitExceeded)return Status;
                var clock=System.Diagnostics.Stopwatch.StartNew();
                for(int node=0;node<maxNodes && _frontier.Count>0;node++)
                {
                    var current=_frontier.Pop();_empty.Clear();_filled.Clear();
                    for(int a=0;a<current.Length;a++)for(int b=0;b<current.Length;b++)
                    {
                        if(!TryMove(current,a,b,_capacity,out int src,out int dst,out _))continue;
                        var key=Key(current,a,src,b,dst);if(!_seen.Add(key))continue;
                        var next=With(current,a,src,b,dst);
                        if(IsSolved(next,_capacity))return Status=SolveStatus.Solved;
                        (CountOf(current[b])==0?_empty:_filled).Add(next);
                    }
                    foreach(var next in _empty)_frontier.Push(next);
                    foreach(var next in _filled)_frontier.Push(next);
                    if(clock.Elapsed.TotalMilliseconds>=milliseconds)break;
                }
                if(_frontier.Count==0)Status=SolveStatus.Unsolvable;
                return Status;
            }
        }

        // Canonical board codes pad missing columns with zero. For cross-attempt
        // caching the number of vessels must also be included: an extra empty
        // vessel can turn an otherwise identical dead board into a solvable one.
        public static string ReachabilityKey(IReadOnlyList<WeatherType[]> board,int capacity)=>capacity+":"+board.Count+":"+CanonicalKey(board,capacity);
        public static string CanonicalKey(IReadOnlyList<WeatherType[]> board, int capacity) => Key(EncodeBoard(board, capacity)).ToString();

        // Gerçek bulut sırasıyla, her adımda çözüme bir adım yaklaştıran ilk hamle seçilir (deterministik).
        private static Move[] BuildSolution(int[] start, int capacity, Dictionary<StateKey, int> index, int[] toGoal,bool lockCompleted)
        {
            var solution = new List<Move>();
            int[] current = start;
            int remaining = toGoal[0];
            while (remaining > 0)
            {
                bool advanced = false;
                for (int s = 0; s < current.Length && !advanced; s++)
                for (int t = 0; t < current.Length && !advanced; t++)
                {
                    if (!TryMove(current, s, t, capacity, out int newSource, out int newTarget, out Move move,lockCompleted)) continue;
                    if (toGoal[index[Key(current, s, newSource, t, newTarget)]] != remaining - 1) continue;
                    solution.Add(move);
                    current = With(current, s, newSource, t, newTarget);
                    remaining--;
                    advanced = true;
                }
                if (!advanced) throw new InvalidOperationException("Solver distance table is inconsistent.");
            }
            return solution.ToArray();
        }

        private static int[] DistancesToGoal(List<int[]> states, List<int[]> edges, int capacity)
        {
            int count = states.Count;
            var parents = new List<int>[count];
            for (int i = 0; i < count; i++) parents[i] = new List<int>();
            for (int i = 0; i < count; i++)
                foreach (int c in edges[i])
                    if (c != i) parents[c].Add(i);

            var distance = new int[count];
            var queue = new Queue<int>();
            for (int i = 0; i < count; i++)
            {
                distance[i] = IsSolved(states[i], capacity) ? 0 : Unreached;
                if (distance[i] == 0) queue.Enqueue(i);
            }

            while (queue.Count > 0)
            {
                int node = queue.Dequeue();
                foreach (int parent in parents[node])
                {
                    if (distance[parent] != Unreached) continue;
                    distance[parent] = distance[node] + 1;
                    queue.Enqueue(parent);
                }
            }
            return distance;
        }

        // ---- Bulut kodlaması: bit 0–2 eleman sayısı, sonra alttan üste her eleman 3 bit (WeatherType 0–6). ----

        private static int[] EncodeBoard(IReadOnlyList<WeatherType[]> board, int capacity)
        {
            if (capacity < 1 || capacity > 7) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (board == null || board.Count == 0) throw new ArgumentException("Board has no clouds.", nameof(board));
            if (board.Count > StateKey.MaxClouds) throw new ArgumentException($"At most {StateKey.MaxClouds} clouds are supported.", nameof(board));
            var codes = new int[board.Count];
            for (int c = 0; c < board.Count; c++)
            {
                WeatherType[] cloud = board[c] ?? throw new ArgumentException("Cloud is null.", nameof(board));
                if (cloud.Length > capacity) throw new ArgumentException("A cloud exceeds capacity.", nameof(board));
                int code = cloud.Length;
                for (int k = 0; k < cloud.Length; k++) code |= (int)cloud[k] << (3 + 3 * k);
                codes[c] = code;
            }
            return codes;
        }

        private static int CountOf(int code) => code & 7;
        private static int At(int code, int k) => (code >> (3 + 3 * k)) & 7;

        private static int TopGroup(int code)
        {
            int count = CountOf(code);
            int top = At(code, count - 1);
            int group = 1;
            for (int k = count - 2; k >= 0 && At(code, k) == top; k--) group++;
            return group;
        }

        private static bool TryMove(int[] state, int s, int t, int capacity, out int newSource, out int newTarget, out Move move,bool lockCompleted=true)
        {
            newSource = newTarget = 0;
            move = default;
            if (s == t) return false;
            int source = state[s], target = state[t];
            int sourceCount = CountOf(source), targetCount = CountOf(target);
            if (sourceCount == 0 || (lockCompleted&&IsCompleteCloud(source,capacity))) return false;

            int weather = At(source, sourceCount - 1);
            int amount = Math.Min(TopGroup(source), capacity - targetCount);
            if (amount <= 0) return false;
            int rainbow = (int)WeatherType.Rainbow;
            if (targetCount > 0)
            {
                int targetTop = At(target, targetCount - 1);
                if (targetTop != weather && targetTop != rainbow && weather != rainbow) return false;
            }

            int remaining = sourceCount - amount;
            newSource = (((source >> 3) & ((1 << (3 * remaining)) - 1)) << 3) | remaining;
            newTarget = (target & ~7) | (targetCount + amount);
            for (int k = targetCount; k < targetCount + amount; k++) newTarget |= weather << (3 + 3 * k);
            move = new Move(s, t, amount, (WeatherType)weather);
            return true;
        }

        // PuzzleRules.IsSolved ile aynı: boş olmayan her bulut dolu ve (Rainbow hariç) tek renk.
        private static bool IsCompleteCloud(int code,int capacity)
        {
            int count=CountOf(code);if(count!=capacity)return false;int resolved=-1;
            for(int i=0;i<count;i++){int type=At(code,i);if(type==(int)WeatherType.Rainbow)continue;if(resolved>=0&&resolved!=type)return false;resolved=type;}
            return true;
        }
        private static bool IsSolved(int[] state,int capacity)
        {
            foreach(int code in state)if(CountOf(code)>0&&!IsCompleteCloud(code,capacity))return false;
            return true;
        }

        private static int[] With(int[] state, int s, int newSource, int t, int newTarget)
        {
            var next = (int[])state.Clone();
            next[s] = newSource;
            next[t] = newTarget;
            return next;
        }

        private static StateKey Key(int[] state) => Key(state, -1, 0, -1, 0);

        // Hamle sonrası durumun anahtarı, dizi kopyalamadan: s/t bulutlarının yeni kodları yerine konur.
        private static StateKey Key(int[] state, int s, int newSource, int t, int newTarget)
        {
            Span<int> sorted = stackalloc int[state.Length];
            for (int i = 0; i < state.Length; i++)
            {
                int value = i == s ? newSource : i == t ? newTarget : state[i];
                int j = i - 1;
                while (j >= 0 && sorted[j] > value) { sorted[j + 1] = sorted[j]; j--; }
                sorted[j + 1] = value;
            }
            return new StateKey(sorted);
        }

        // Sıralanmış bulut kodları (24 bit) iki tanesi bir ulong'a paketlenir; metin anahtardan çok daha hızlı.
        private readonly struct StateKey : IEquatable<StateKey>, IComparable<StateKey>
        {
            public const int MaxClouds = 12;
            private readonly ulong _a, _b, _c, _d, _e, _f;

            public StateKey(ReadOnlySpan<int> sortedCodes)
            {
                Span<ulong> packed = stackalloc ulong[MaxClouds / 2];
                for (int i = 0; i < sortedCodes.Length; i++)
                    packed[i / 2] |= ((ulong)(uint)sortedCodes[i] + 1UL) << (i % 2 * 32);
                _a = packed[0]; _b = packed[1]; _c = packed[2]; _d = packed[3]; _e = packed[4]; _f = packed[5];
            }

            public bool Equals(StateKey o) => _a == o._a && _b == o._b && _c == o._c && _d == o._d && _e == o._e && _f == o._f;
            public override bool Equals(object obj) => obj is StateKey other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    ulong h = 1469598103934665603UL;
                    h = (h ^ _a) * 1099511628211UL; h = (h ^ _b) * 1099511628211UL; h = (h ^ _c) * 1099511628211UL;
                    h = (h ^ _d) * 1099511628211UL; h = (h ^ _e) * 1099511628211UL; h = (h ^ _f) * 1099511628211UL;
                    return (int)(h ^ (h >> 32));
                }
            }

            public int CompareTo(StateKey o)
            {
                int c = _a.CompareTo(o._a); if (c != 0) return c;
                c = _b.CompareTo(o._b); if (c != 0) return c;
                c = _c.CompareTo(o._c); if (c != 0) return c;
                c = _d.CompareTo(o._d); if (c != 0) return c;
                c = _e.CompareTo(o._e); if (c != 0) return c;
                return _f.CompareTo(o._f);
            }

            public override string ToString() => $"{_a:x}.{_b:x}.{_c:x}.{_d:x}.{_e:x}.{_f:x}";
        }
    }
}
