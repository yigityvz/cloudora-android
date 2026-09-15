using System;

namespace Cloudora.Services
{
    public enum RewardedPlacement { Life, ExtraCloud, Undo, SafeShuffle }

    public interface IAdService
    {
        bool IsAvailable { get; }
        void ShowRewarded(RewardedPlacement placement, Action<bool> completed);
        void ShowInterstitial(Action completed = null);
    }
}
