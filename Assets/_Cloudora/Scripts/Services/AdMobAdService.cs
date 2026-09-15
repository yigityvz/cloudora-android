using System;
using UnityEngine;

namespace Cloudora.Services
{
    // Deliberately disabled. Real SDK/configuration requires explicit owner approval.
    public sealed class AdMobAdService : IAdService
    {
        public bool IsAvailable => false;
        public void ShowRewarded(RewardedPlacement placement, Action<bool> completed)
        {
            Debug.LogWarning("AdMob is disabled in this build.");
            completed?.Invoke(false);
        }
        public void ShowInterstitial(Action completed = null) => completed?.Invoke();
    }
}
