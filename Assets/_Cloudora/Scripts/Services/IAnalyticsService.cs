using System.Collections.Generic;

namespace Cloudora.Services
{
    public interface IAnalyticsService
    {
        void Track(string eventName, IReadOnlyDictionary<string, object> parameters = null);
    }
}
