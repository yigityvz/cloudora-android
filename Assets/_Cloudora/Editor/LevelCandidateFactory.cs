using System;
using System.Collections.Generic;
using Cloudora.Core;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    // Bölüm aracı (yalnızca Editor): aday tahta — elle tasarlanmış bölümlerle aynı biçim, renkli bulutlar dolu ve
    // karışık, boş bulutlar sonda.
    public static class LevelCandidateFactory
    {
        // LevelGenerator ve AuthoredLevelCatalog ile aynı renk sırası (1–5: Güneş/Yağmur, 6+: Kar…).
        internal static readonly WeatherType[] Colors =
        {
            WeatherType.Sun, WeatherType.Rain, WeatherType.Snow, WeatherType.Wind,
            WeatherType.Moon, WeatherType.Lightning
        };

        public static WeatherType[][] Deal(BoardFrame frame, int seed)
        {
            if (!frame.IsPacked) return DealScattered(frame, seed);
            var random = new DeterministicRandom(seed);
            var pool = new WeatherType[frame.ColorCount * frame.Capacity];
            for (int attempt = 0; attempt < 64; attempt++)
            {
                for (int i = 0; i < pool.Length; i++) pool[i] = Colors[i / frame.Capacity];
                for (int i = pool.Length - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    (pool[i], pool[j]) = (pool[j], pool[i]);
                }

                var board = new WeatherType[frame.CloudCount][];
                for (int c = 0; c < frame.CloudCount; c++)
                {
                    board[c] = c < frame.ColorCount ? new WeatherType[frame.Capacity] : Array.Empty<WeatherType>();
                    if (c < frame.ColorCount) Array.Copy(pool, c * frame.Capacity, board[c], 0, frame.Capacity);
                }

                if (!StartsSolved(board, frame.Capacity)) return board;
            }

            throw new InvalidOperationException($"Could not deal an unsolved board for {frame} seed {seed}.");
        }

        // v3.2 farklı başlangıç: OpenClouds bulut tamamen boş; kalan boş alan dolu bulutlardan rastgele tek tek alınır
        // (her bulutta en az bir parça kalır). Parça sayıları ve toplam boş alan klasik tahtayla aynıdır.
        private static WeatherType[][] DealScattered(BoardFrame frame, int seed)
        {
            var random = new DeterministicRandom(seed);
            var pool = new WeatherType[frame.ColorCount * frame.Capacity];
            int filled = frame.CloudCount - frame.OpenClouds;
            int free = (frame.EmptyCount - frame.OpenClouds) * frame.Capacity;
            for (int attempt = 0; attempt < 64; attempt++)
            {
                for (int i = 0; i < pool.Length; i++) pool[i] = Colors[i / frame.Capacity];
                for (int i = pool.Length - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    (pool[i], pool[j]) = (pool[j], pool[i]);
                }

                var counts = new int[filled];
                for (int c = 0; c < filled; c++) counts[c] = frame.Capacity;
                for (int removed = 0; removed < free; )
                {
                    int c = random.Next(filled);
                    if (counts[c] <= 1) continue;
                    counts[c]--;
                    removed++;
                }

                var board = new WeatherType[frame.CloudCount][];
                int next = 0;
                for (int c = 0; c < frame.CloudCount; c++)
                {
                    int count = c < filled ? counts[c] : 0;
                    board[c] = new WeatherType[count];
                    Array.Copy(pool, next, board[c], 0, count);
                    next += count;
                }

                if (!StartsSolved(board, frame.Capacity)) return board;
            }

            throw new InvalidOperationException($"Could not deal an unsolved board for {frame} seed {seed}.");
        }

        private static bool StartsSolved(WeatherType[][] board, int capacity)
        {
            var clouds = new List<CloudState>(board.Length);
            foreach (WeatherType[] cloud in board) clouds.Add(new CloudState(capacity, cloud));
            return PuzzleRules.IsSolved(new PuzzleState(clouds));
        }
    }
}
