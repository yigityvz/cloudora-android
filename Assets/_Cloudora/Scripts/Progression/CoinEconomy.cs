using System;
using Cloudora.Level;

namespace Cloudora.Progression
{
    // Shared reward/continue policy for every chapter.
    public static class CoinEconomy
    {
        public const int StartingCoins = 0;   // başlangıç coini ileride tutorial görevinde verilecek
        public const int ContinueCost = 500;
        public const int MediumReward = 100;
        public const int HardReward = 200;
        public const int VeryHardReward = 300;
        public const int LegacyReward = MediumReward;   // compatibility alias

        public static int RewardFor(int level)
        {
            if(level<1)return 0;
            // Use the same five-level difficulty cycle for current and future chapters.
            return V3LevelPlan.BandFor((level-1)%5+1) switch
            {
                DifficultyBand.Hard => HardReward,
                DifficultyBand.VeryHard => VeryHardReward,
                _ => MediumReward
            };
        }
    }

    public sealed class CoinWallet
    {
        public int Coins { get; private set; }

        public CoinWallet(int coins) => Coins = Math.Max(0, coins);

        public void Earn(int amount)
        {
            if (amount > 0) Coins += amount;
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || amount > Coins) return false;
            Coins -= amount;
            return true;
        }
    }
}
