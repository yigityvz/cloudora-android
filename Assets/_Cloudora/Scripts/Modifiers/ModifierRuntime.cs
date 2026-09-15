using System.Collections.Generic;
using Cloudora.Level;
using Cloudora.Puzzle;

namespace Cloudora.Modifiers
{
    public sealed class ModifierRuntime
    {
        private readonly List<ModifierData> _data = new();
        private readonly Dictionary<ModifierType, ICloudModifierRule> _rules = new();
        private CloudContainerView[] _clouds;

        public ModifierRuntime()
        {
            Register(new FrozenModifierRule());
            Register(new LockedModifierRule());
            Register(new FogModifierRule());
            Register(new RainbowModifierRule());
            Register(new WindModifierRule());
            Register(new NightModifierRule());
        }

        public void Register(ICloudModifierRule rule) => _rules[rule.Type] = rule;

        public void Initialize(LevelDefinition level, CloudContainerView[] clouds)
        {
            _clouds = clouds;
            _data.Clear();
            if (level.modifiers != null)
                foreach (ModifierData item in level.modifiers) _data.Add(item.Clone());
            ApplyVisuals();
        }

        public bool CanMove(int source, int target)
        {
            foreach (ModifierData data in _data)
            {
                if (!_rules.TryGetValue(data.type, out ICloudModifierRule rule)) continue;
                if (data.cloudIndex == source && !rule.CanUseAsSource(data, _clouds)) return false;
                if (data.cloudIndex == target && !rule.CanUseAsTarget(data, _clouds)) return false;
            }
            return true;
        }

        public void OnSuccessfulMove(int source, int target)
        {
            foreach (ModifierData data in _data)
                if (_rules.TryGetValue(data.type, out ICloudModifierRule rule)) rule.OnSuccessfulMove(data, source, target);
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            if (_clouds == null) return;
            for (int i = 0; i < _clouds.Length; i++) _clouds[i].SetModifierState(ModifierType.None, false, false);
            foreach (ModifierData data in _data)
            {
                if (data.cloudIndex < 0 || data.cloudIndex >= _clouds.Length || !_rules.TryGetValue(data.type, out ICloudModifierRule rule)) continue;
                bool blocked = !rule.CanUseAsSource(data, _clouds) || !rule.CanUseAsTarget(data, _clouds);
                _clouds[data.cloudIndex].SetModifierState(data.type, rule.HidesContents(data), blocked);
            }
        }
    }
}
