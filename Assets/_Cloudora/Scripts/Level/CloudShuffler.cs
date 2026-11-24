using System;
using System.Collections.Generic;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public enum CloudShuffleRejection { None, NotShuffleable, Protected, NoSafeArrangement }

    public readonly struct CloudShuffleResult
    {
        public readonly CloudShuffleRejection Rejection;
        public readonly WeatherType[][] Board;

        public CloudShuffleResult(CloudShuffleRejection rejection, WeatherType[][] board)
        {
            Rejection = rejection;
            Board = board;
        }

        public bool Success => Rejection == CloudShuffleRejection.None;
    }

    // v3.1 Safe Shuffle: seçilen tek bulutun içini karıştırır. Sonuç her zaman farklı bir dizilimdir ve
    // tahtayı çözülebilir bırakır (çözücüyle doğrulanır). Diğer bulutlara ve özel (modifier'lı) bulutlara dokunmaz.
    public static class CloudShuffler
    {
        // Karıştırılınca değişebilir mi: en az iki eleman ve en az iki farklı renk.
        public static bool CanChange(IReadOnlyList<WeatherType> cloud)
        {
            if (cloud == null || cloud.Count < 2) return false;
            for (int i = 1; i < cloud.Count; i++)
                if (cloud[i] != cloud[0]) return true;
            return false;
        }

        public static CloudShuffleResult Shuffle(WeatherType[][] board, int capacity, int cloudIndex, int seed,
            int maxStates, ICollection<int> protectedClouds = null)
        {
            if (cloudIndex < 0 || cloudIndex >= board.Length) throw new ArgumentOutOfRangeException(nameof(cloudIndex));
            if (protectedClouds != null && protectedClouds.Contains(cloudIndex)) return Reject(CloudShuffleRejection.Protected);
            if(new Cloudora.Core.CloudState(capacity,board[cloudIndex]).IsComplete)return Reject(CloudShuffleRejection.NotShuffleable);
            if (!CanChange(board[cloudIndex])) return Reject(CloudShuffleRejection.NotShuffleable);

            List<WeatherType[]> arrangements = DistinctArrangements(board[cloudIndex]);
            var random = new DeterministicRandom(seed);
            for (int i = arrangements.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (arrangements[i], arrangements[j]) = (arrangements[j], arrangements[i]);
            }

            foreach (WeatherType[] arrangement in arrangements)
            {
                var candidate = new WeatherType[board.Length][];
                for (int c = 0; c < board.Length; c++)
                    candidate[c] = c == cloudIndex ? arrangement : (WeatherType[])board[c].Clone();
                if (PuzzleSolver.CheckSolvable(candidate, capacity, maxStates) == SolveStatus.Solved)
                    return new CloudShuffleResult(CloudShuffleRejection.None, candidate);
            }
            return Reject(CloudShuffleRejection.NoSafeArrangement);
        }

        private static CloudShuffleResult Reject(CloudShuffleRejection reason) => new(reason, null);

        // Bulutun mevcut dizilişi dışındaki tüm farklı dizilişleri (tekrarlı elemanlar tek sayılır), sabit sırayla.
        private static List<WeatherType[]> DistinctArrangements(WeatherType[] cloud)
        {
            var counts = new SortedDictionary<WeatherType, int>();
            foreach (WeatherType weather in cloud) counts[weather] = counts.TryGetValue(weather, out int c) ? c + 1 : 1;
            var kinds = new List<WeatherType>(counts.Keys);
            var remaining = new int[kinds.Count];
            for (int i = 0; i < kinds.Count; i++) remaining[i] = counts[kinds[i]];

            var result = new List<WeatherType[]>();
            var current = new WeatherType[cloud.Length];
            Build(0);
            return result;

            void Build(int position)
            {
                if (position == current.Length)
                {
                    if (!Same(current, cloud)) result.Add((WeatherType[])current.Clone());
                    return;
                }
                for (int k = 0; k < kinds.Count; k++)
                {
                    if (remaining[k] == 0) continue;
                    remaining[k]--;
                    current[position] = kinds[k];
                    Build(position + 1);
                    remaining[k]++;
                }
            }
        }

        private static bool Same(WeatherType[] a, WeatherType[] b)
        {
            for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
            return true;
        }
    }

    // Başarısızlıkta "devam": geçmişteki (en yeniden en eskiye) ilk çözülebilir durum.
    public static class FailRecovery
    {
        public static int NewestSolvableIndex(IReadOnlyList<WeatherType[][]> newestFirst, int capacity, int maxStates)
        {
            for (int i = 0; i < newestFirst.Count; i++)
                if (PuzzleSolver.CheckSolvable(newestFirst[i], capacity, maxStates) == SolveStatus.Solved) return i;
            return -1;
        }
    }
}
