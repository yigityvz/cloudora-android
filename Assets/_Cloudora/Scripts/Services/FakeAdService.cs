using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cloudora.Services
{
    public sealed class FakeAdService : IAdService
    {
        private readonly IAnalyticsService _analytics;
        public bool IsAvailable => true;

        public FakeAdService(IAnalyticsService analytics) => _analytics = analytics;

        public void ShowRewarded(RewardedPlacement placement, Action<bool> completed)
        {
            _analytics.Track(AnalyticsEvents.RewardedOpportunity, new Dictionary<string, object> { ["placement"] = placement.ToString() });
            Debug.Log($"[FakeAd] rewarded placement={placement}; reward granted immediately.");
            completed?.Invoke(true);
        }

        public void ShowInterstitial(Action completed = null)
        {
            _analytics.Track(AnalyticsEvents.InterstitialOpportunity);
            Debug.Log("[FakeAd] interstitial opportunity simulated.");
            completed?.Invoke();
        }
    }
}
