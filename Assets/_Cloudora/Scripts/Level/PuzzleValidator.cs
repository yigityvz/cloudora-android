using System.Collections.Generic;
using Cloudora.Core;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    public static class PuzzleValidator
    {
        public static bool Validate(LevelDefinition definition, out string error)
        {
            var counts = new Dictionary<WeatherType, int>();
            var clouds = new List<CloudState>();
            foreach (CloudDefinition cloud in definition.clouds)
            {
                if (cloud.elements.Length > definition.capacity)
                {
                    error = "A cloud exceeds capacity.";
                    return false;
                }

                clouds.Add(new CloudState(definition.capacity, cloud.elements));
                foreach (WeatherType weather in cloud.elements)
                    counts[weather] = counts.TryGetValue(weather, out int count) ? count + 1 : 1;
            }

            int rainbowCount = counts.TryGetValue(WeatherType.Rainbow, out int wildcards) ? wildcards : 0;
            int deficit = 0;
            foreach (KeyValuePair<WeatherType, int> pair in counts)
            {
                if (pair.Key == WeatherType.Rainbow) continue;
                int remainder = pair.Value % definition.capacity;
                if (remainder != 0) deficit += definition.capacity - remainder;
                if (remainder != 0 && rainbowCount == 0)
                {
                    error = "Weather count is not divisible by capacity.";
                    return false;
                }
            }
            if (rainbowCount > 0 && deficit != rainbowCount)
            {
                error = "Rainbow wildcard count does not balance weather deficits.";
                return false;
            }

            var state = new PuzzleState(clouds);
            if (PuzzleRules.IsSolved(state))
            {
                error = "Board starts solved.";
                return false;
            }

            if (definition.knownSolution != null)
            {
                foreach (Move move in definition.knownSolution)
                {
                    if (!PuzzleRules.TryApply(state, move))
                    {
                        error = "Known solution contains an illegal move.";
                        return false;
                    }
                }

                if (!PuzzleRules.IsSolved(state))
                {
                    error = "Known solution does not solve the board.";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
