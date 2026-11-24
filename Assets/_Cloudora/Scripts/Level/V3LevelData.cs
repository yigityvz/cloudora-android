using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public sealed class V3LevelEntry
    {
        public int Level { get; }
        public DifficultyBand Band { get; }
        public int Capacity { get; }
        public int Seed { get; }
        public float Score { get; }
        public int OptimalMoves { get; }
        public string Board { get; }

        public V3LevelEntry(int level, DifficultyBand band, int capacity, int seed, float score, int optimalMoves, string board)
        {
            Level = level;
            Band = band;
            Capacity = capacity;
            Seed = seed;
            Score = score;
            OptimalMoves = optimalMoves;
            Board = board;
        }

        public WeatherType[][] CreateBoard() => V3BoardText.Parse(Board);
    }

    // Seçilmiş v3 bölümleri. Veriler V3LevelData.Generated.cs içindedir ve araçla üretilir
    // (Cloudora/V3/Bölüm Verisini Üret); elle değiştirilmez — V3LevelBuildTests.GeneratedDataMatchesBuilder kontrol eder.
    public static partial class V3LevelData
    {
        public static V3LevelEntry Find(int level)
        {
            foreach (V3LevelEntry entry in Entries)
                if (entry.Level == level) return entry;
            return null;
        }
    }
}
