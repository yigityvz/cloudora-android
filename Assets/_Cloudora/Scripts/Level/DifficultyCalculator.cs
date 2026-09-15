using Cloudora.Core;

namespace Cloudora.Level
{
    public static class DifficultyCalculator
    {
        public static float Calculate(PuzzleState state, int shuffleDepth, int modifierComplexity = 0)
        {
            int elements = 0;
            int emptyClouds = 0;
            int mixedTransitions = 0;
            foreach (CloudState cloud in state.Clouds)
            {
                elements += cloud.Count;
                if (cloud.IsEmpty) emptyClouds++;
                for (int i = 1; i < cloud.Count; i++)
                {
                    if (cloud.Elements[i] != cloud.Elements[i - 1]) mixedTransitions++;
                }
            }

            int capacity = state.Clouds.Count == 0 ? 4 : state.Clouds[0].Capacity;
            return elements * 0.25f + capacity * 2f + state.Clouds.Count * 1.5f +
                   shuffleDepth * 0.45f + mixedTransitions * 2.5f + modifierComplexity * 8f - emptyClouds * 3f;
        }
    }
}
