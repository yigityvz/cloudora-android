using System;

namespace Cloudora.Services
{
    public sealed class AdPlacementPolicy
    {
        private readonly TimeSpan _minimumInterval;
        private DateTime _lastInterstitialUtc;
        private int _eligibleCompletions;

        public AdPlacementPolicy(TimeSpan? minimumInterval = null)
        {
            _minimumInterval = minimumInterval ?? TimeSpan.FromMinutes(5);
        }

        public bool RegisterCompletion(int level, DateTime utcNow)
        {
            if (level <= 5) return false;
            _eligibleCompletions++;
            if (_eligibleCompletions < 4) return false;
            if (_lastInterstitialUtc != default && utcNow - _lastInterstitialUtc < _minimumInterval) return false;
            _eligibleCompletions = 0;
            _lastInterstitialUtc = utcNow;
            return true;
        }
    }
}
