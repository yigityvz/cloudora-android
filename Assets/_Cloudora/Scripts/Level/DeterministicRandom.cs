using System;

namespace Cloudora.Level
{
    // SplitMix64: her platformda (Editor, Mono, IL2CPP) aynı diziyi üretir; System.Random'a güvenilmez.
    // Dizi değişirse kayıtlı seed'ler başka tahtalar üretir — LevelToolTests.RandomSequenceIsFixedAcrossPlatforms korur.
    public sealed class DeterministicRandom
    {
        private const ulong Golden = 0x9E3779B97F4A7C15UL;
        private ulong _state;

        public DeterministicRandom(int seed)
        {
            unchecked { _state = (ulong)(uint)seed ^ Golden; }
        }

        public ulong NextULong()
        {
            unchecked
            {
                _state += Golden;
                ulong z = _state;
                z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
                z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
                return z ^ (z >> 31);
            }
        }

        public int Next(int maxExclusive)
        {
            if (maxExclusive <= 0) throw new ArgumentOutOfRangeException(nameof(maxExclusive));
            return (int)(NextULong() % (ulong)maxExclusive);
        }
    }
}
