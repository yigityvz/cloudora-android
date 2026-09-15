namespace Cloudora.Level
{
    public enum DifficultyBand { Easy, Medium, Hard, VeryHard, WorldFinale }

    public static class BalanceProfile
    {
        public static DifficultyBand BandFor(int level)
        {
            Progression.WorldDefinition world = Progression.WorldCatalog.ForLevel(level);
            if (level == world.LastLevel) return DifficultyBand.WorldFinale;
            int local = level - world.FirstLevel;
            if (local <= 1) return DifficultyBand.Easy;
            return (local % 6) switch
            {
                0 => DifficultyBand.Easy,
                1 => DifficultyBand.Medium,
                2 => DifficultyBand.Hard,
                3 => DifficultyBand.Medium,
                4 => DifficultyBand.VeryHard,
                _ => DifficultyBand.Medium
            };
        }

        public static int ShuffleDepth(int level)
        {
            int capacityBonus = level <= 20 ? 0 : level <= 60 ? 4 : level <= 125 ? 8 : 12;
            return BandFor(level) switch
            {
                DifficultyBand.Easy => 12 + capacityBonus,
                DifficultyBand.Medium => 20 + capacityBonus,
                DifficultyBand.Hard => 30 + capacityBonus,
                DifficultyBand.VeryHard => 42 + capacityBonus,
                DifficultyBand.WorldFinale => 52 + capacityBonus,
                _ => 20
            };
        }

        public static (int MinSeconds, int MaxSeconds) DurationTarget(int level)
            => level <= 20 ? (30, 60) : level <= 100 ? (60, 180) : (180, 300);
    }
}
