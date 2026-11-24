namespace Cloudora.Progression
{
    public sealed class ProgressionManager
    {
        public int CurrentLevel { get; private set; }
        public int HighestCompletedLevel { get; private set; }
        public WorldDefinition CurrentWorld => WorldCatalog.ForLevel(CurrentLevel);

        public ProgressionManager(int currentLevel, int highestCompletedLevel = 0)
        {
            CurrentLevel = System.Math.Clamp(currentLevel,1,WorldCatalog.MaxLevel);
            HighestCompletedLevel = System.Math.Clamp(highestCompletedLevel,0,WorldCatalog.MaxLevel);
        }

        public void CompleteCurrentLevel()
        {
            HighestCompletedLevel = System.Math.Max(HighestCompletedLevel, CurrentLevel);
        }

        public void Advance()
        {
            if (HighestCompletedLevel >= CurrentLevel) CurrentLevel=System.Math.Min(WorldCatalog.MaxLevel,CurrentLevel+1);
        }

        public int ResumeLevel(bool currentAttemptCompleted) => currentAttemptCompleted ? System.Math.Min(WorldCatalog.MaxLevel,CurrentLevel + 1) : CurrentLevel;

        public void DebugJump(int level) => CurrentLevel = System.Math.Clamp(level,1,WorldCatalog.MaxLevel);
    }
}
