using System.Collections.Generic;
using System.Linq;

namespace Cloudora.Core
{
    public sealed class PuzzleState
    {
        private readonly List<CloudState> _clouds;
        public IReadOnlyList<CloudState> Clouds => _clouds;

        public PuzzleState(IEnumerable<CloudState> clouds)
        {
            _clouds = new List<CloudState>(clouds);
        }

        public PuzzleState Clone() => new(_clouds.Select(cloud => cloud.Clone()));
    }
}
