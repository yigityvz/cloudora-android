namespace Cloudora.Services
{
    public static class AnalyticsEvents
    {
        public const string GameStarted = "game_started";
        public const string LevelStarted = "level_started";
        public const string LevelCompleted = "level_completed";
        public const string LevelFailed = "level_failed";
        public const string LevelRestarted = "level_restarted";
        public const string UndoUsed = "undo_used";
        public const string ExtraCloudUsed = "extra_cloud_used";
        public const string SafeShuffleUsed = "safe_shuffle_used";
        public const string WorldStarted = "world_started";
        public const string WorldCompleted = "world_completed";
        public const string ModifierEncountered = "modifier_encountered";
        public const string InterstitialOpportunity = "interstitial_opportunity";
        public const string RewardedOpportunity = "rewarded_opportunity";
        public const string LifeDepleted = "life_depleted";
    }
}
