namespace Cloudora.Progression
{
    public sealed class ProgressionManager
    {
        public int CurrentLevel { get; private set; }
        public int HighestCompletedLevel { get; private set; }
        public WorldDefinition CurrentWorld => WorldCatalog.ForLevel(CurrentLevel);

        public ProgressionManager(int currentLevel, int highestCompletedLevel = 0)
        {
            CurrentLevel = System.Math.Max(1, currentLevel);
            HighestCompletedLevel = System.Math.Max(0, highestCompletedLevel);
        }

        public void CompleteCurrentLevel()
        {
            HighestCompletedLevel = System.Math.Max(HighestCompletedLevel, CurrentLevel);
        }

        public void Advance()
        {
            if (HighestCompletedLevel >= CurrentLevel) CurrentLevel++;
        }

        public void DebugJump(int level) => CurrentLevel = System.Math.Max(1, level);
    }
}
