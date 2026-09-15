using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cloudora.Services
{
    public sealed class DebugAnalyticsService : IAnalyticsService
    {
        public void Track(string eventName, IReadOnlyDictionary<string, object> parameters = null)
        {
            var text = new StringBuilder("[Analytics] ").Append(eventName);
            if (parameters != null)
                foreach (KeyValuePair<string, object> item in parameters) text.Append(' ').Append(item.Key).Append('=').Append(item.Value);
            Debug.Log(text.ToString());
        }
    }
}
