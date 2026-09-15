using System;
using Cloudora.Puzzle;

namespace Cloudora.Core
{
    [Serializable]
    public readonly struct Move
    {
        public readonly int SourceIndex;
        public readonly int TargetIndex;
        public readonly int Count;
        public readonly WeatherType Weather;

        public Move(int sourceIndex, int targetIndex, int count, WeatherType weather)
        {
            SourceIndex = sourceIndex;
            TargetIndex = targetIndex;
            Count = count;
            Weather = weather;
        }
    }
}
