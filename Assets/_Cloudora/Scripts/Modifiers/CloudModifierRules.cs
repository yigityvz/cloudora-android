using Cloudora.Puzzle;

namespace Cloudora.Modifiers
{
    public abstract class PassiveModifierRule : ICloudModifierRule
    {
        public abstract ModifierType Type { get; }
        public virtual bool CanUseAsSource(ModifierData data, CloudContainerView[] clouds) => true;
        public virtual bool CanUseAsTarget(ModifierData data, CloudContainerView[] clouds) => true;
        public virtual void OnSuccessfulMove(ModifierData data, int sourceIndex, int targetIndex) { }
        public virtual bool HidesContents(ModifierData data) => false;
    }

    public sealed class FrozenModifierRule : PassiveModifierRule
    {
        public override ModifierType Type => ModifierType.Frozen;
        public override bool CanUseAsSource(ModifierData data, CloudContainerView[] clouds) => data.counter <= 0;
        public override void OnSuccessfulMove(ModifierData data, int sourceIndex, int targetIndex)
        {
            if (data.counter > 0) data.counter--;
        }
    }

    public sealed class LockedModifierRule : PassiveModifierRule
    {
        public override ModifierType Type => ModifierType.Locked;
        private static bool IsUnlocked(ModifierData data, CloudContainerView[] clouds)
            => data.parameter >= 0 && data.parameter < clouds.Length && !clouds[data.parameter].IsEmpty && clouds[data.parameter].IsSolved();
        public override bool CanUseAsSource(ModifierData data, CloudContainerView[] clouds) => IsUnlocked(data, clouds);
        public override bool CanUseAsTarget(ModifierData data, CloudContainerView[] clouds) => IsUnlocked(data, clouds);
    }

    public sealed class FogModifierRule : PassiveModifierRule
    {
        public override ModifierType Type => ModifierType.Fog;
        public override bool HidesContents(ModifierData data) => data.counter > 0;
        public override void OnSuccessfulMove(ModifierData data, int sourceIndex, int targetIndex)
        {
            if (sourceIndex == data.cloudIndex || targetIndex == data.cloudIndex) data.counter = 0;
        }
    }

    public sealed class RainbowModifierRule : PassiveModifierRule { public override ModifierType Type => ModifierType.Rainbow; }
    public sealed class WindModifierRule : PassiveModifierRule, IMoveCountModifier
    {
        public override ModifierType Type => ModifierType.Wind;
        public int AdjustMoveCount(ModifierData data, int sourceIndex, int targetIndex, int proposedCount)
            => sourceIndex == data.cloudIndex ? System.Math.Min(1, proposedCount) : proposedCount;
    }
    public sealed class NightModifierRule : PassiveModifierRule
    {
        public override ModifierType Type => ModifierType.Night;
        public override bool HidesContents(ModifierData data) => true;
    }
}
