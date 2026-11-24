using System;

namespace Cloudora.Services
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 2;
        public int schemaVersion = CurrentSchemaVersion;
        public int currentLevel = 1;
        public int highestCompletedLevel;
        public int lives = 5;
        public long nextLifeUtcTicks;
        public int undoCharges = 3;
        public int extraCloudCharges = 1;
        public int safeShuffleCharges = 1;
        public int coins = Progression.CoinEconomy.StartingCoins;
        public bool soundEnabled = true;
        public bool hapticsEnabled = true;
        public bool[] tutorialFlags = new bool[15];
        public int lastGeneratedSeed;
        public FailRecord failRecord = new();

        public static SaveData Defaults() => new();
    }
}
