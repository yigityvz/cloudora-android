namespace Cloudora.Progression
{
    public static class RestorationManager
    {
        public static float GetProgress(WorldDefinition world, int highestCompletedLevel)
        {
            int completed = System.Math.Max(0, System.Math.Min(world.LevelCount, highestCompletedLevel - world.FirstLevel + 1));
            return completed / (float)world.LevelCount;
        }

        public static int GetStage(WorldDefinition world, int highestCompletedLevel)
        {
            float progress = GetProgress(world, highestCompletedLevel);
            return System.Math.Min(4, (int)System.Math.Ceiling(progress * 4f));
        }
    }
}
