using Cloudora.Puzzle;

namespace Cloudora.Modifiers
{
    public interface ICloudModifierRule
    {
        ModifierType Type { get; }
        bool CanUseAsSource(ModifierData data, CloudContainerView[] clouds);
        bool CanUseAsTarget(ModifierData data, CloudContainerView[] clouds);
        void OnSuccessfulMove(ModifierData data, int sourceIndex, int targetIndex);
        bool HidesContents(ModifierData data);
    }

    public interface IMoveCountModifier
    {
        int AdjustMoveCount(ModifierData data, int sourceIndex, int targetIndex, int proposedCount);
    }
}
