using System;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    [Serializable]
    public sealed class LevelDefinition
    {
        public int levelId;
        public string worldId;
        public int capacity;
        public int seed;
        public string tutorialCue;
        public CloudDefinition[] clouds;

        public LevelDefinition(int levelId, string worldId, int capacity, string tutorialCue, params WeatherType[][] clouds)
        {
            this.levelId = levelId;
            this.worldId = worldId;
            this.capacity = capacity;
            seed = 0;
            this.tutorialCue = tutorialCue;
            this.clouds = new CloudDefinition[clouds.Length];
            for (int i = 0; i < clouds.Length; i++)
            {
                this.clouds[i] = new CloudDefinition { elements = (WeatherType[])clouds[i].Clone() };
            }
        }

        public WeatherType[][] CreateBoard()
        {
            var result = new WeatherType[clouds.Length][];
            for (int i = 0; i < clouds.Length; i++)
            {
                result[i] = (WeatherType[])clouds[i].elements.Clone();
            }
            return result;
        }
    }

    [Serializable]
    public sealed class CloudDefinition
    {
        public WeatherType[] elements;
    }
}
