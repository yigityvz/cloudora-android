namespace Cloudora.Progression
{
    public sealed class BoosterManager
    {
        public int UndoCharges { get; private set; }
        public int ExtraCloudCharges { get; private set; }
        public int SafeShuffleCharges { get; private set; }

        public BoosterManager(int undo = 3, int extraCloud = 1, int safeShuffle = 1)
        {
            UndoCharges = System.Math.Max(0, undo);
            ExtraCloudCharges = System.Math.Max(0, extraCloud);
            SafeShuffleCharges = System.Math.Max(0, safeShuffle);
        }

        public bool TryUseUndo()
        {
            if (UndoCharges <= 0) return false;
            UndoCharges--;
            return true;
        }

        public bool TryUseExtraCloud()
        {
            if (ExtraCloudCharges <= 0) return false;
            ExtraCloudCharges--;
            return true;
        }

        public bool TryUseSafeShuffle()
        {
            if (SafeShuffleCharges <= 0) return false;
            SafeShuffleCharges--;
            return true;
        }
        public void GrantUndo(int amount = 3) => UndoCharges += System.Math.Max(0, amount);
        public void GrantExtraCloud(int amount = 1) => ExtraCloudCharges += System.Math.Max(0, amount);
        public void GrantSafeShuffle(int amount = 1) => SafeShuffleCharges += System.Math.Max(0, amount);

    }
}
