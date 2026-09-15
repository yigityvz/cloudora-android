using System;

namespace Cloudora.Modifiers
{
    [Serializable]
    public sealed class ModifierData
    {
        public ModifierType type;
        public int cloudIndex;
        public int parameter;
        public int counter;

        public ModifierData Clone() => (ModifierData)MemberwiseClone();
    }
}
