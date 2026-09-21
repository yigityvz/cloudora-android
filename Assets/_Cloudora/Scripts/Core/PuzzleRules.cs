namespace Cloudora.Core
{
    public static class PuzzleRules
    {
        public static bool CanApply(PuzzleState state, Move move)
        {
            if (move.SourceIndex < 0 || move.SourceIndex >= state.Clouds.Count ||
                move.TargetIndex < 0 || move.TargetIndex >= state.Clouds.Count ||
                move.SourceIndex == move.TargetIndex || move.Count <= 0)
            {
                return false;
            }

            CloudState source = state.Clouds[move.SourceIndex];
            CloudState target = state.Clouds[move.TargetIndex];
            int maximumMove = source.IsEmpty ? 0 : System.Math.Min(source.TopGroupCount(), target.Capacity - target.Count);
            return !source.IsEmpty && source.Top == move.Weather &&
                   move.Count == maximumMove &&
                   (target.IsEmpty || target.Top == move.Weather || target.Top == Puzzle.WeatherType.Rainbow || move.Weather == Puzzle.WeatherType.Rainbow);
        }

        public static bool TryApply(PuzzleState state, Move move)
        {
            if (!CanApply(state, move)) return false;
            CloudState source = state.Clouds[move.SourceIndex];
            CloudState target = state.Clouds[move.TargetIndex];
            source.RemoveTop(move.Count);
            target.Add(move.Weather, move.Count);
            return true;
        }

        public static bool IsSolved(PuzzleState state)
        {
            foreach (CloudState cloud in state.Clouds)
            {
                if (cloud.IsEmpty) continue;
                if (!cloud.IsFull) return false;
                Puzzle.WeatherType? resolved = null;
                for (int i = 0; i < cloud.Count; i++)
                {
                    if (cloud.Elements[i] == Puzzle.WeatherType.Rainbow) continue;
                    if (resolved.HasValue && cloud.Elements[i] != resolved.Value) return false;
                    resolved = cloud.Elements[i];
                }
            }
            return true;
        }
    }
}
