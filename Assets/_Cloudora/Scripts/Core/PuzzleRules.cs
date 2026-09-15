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
            return !source.IsEmpty && source.Top == move.Weather &&
                   move.Count <= source.TopGroupCount() &&
                   move.Count <= target.Capacity - target.Count &&
                   (target.IsEmpty || target.Top == move.Weather);
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
                for (int i = 1; i < cloud.Count; i++)
                {
                    if (cloud.Elements[i] != cloud.Elements[0]) return false;
                }
            }
            return true;
        }
    }
}
