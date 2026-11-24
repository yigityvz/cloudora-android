using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cloudora.Services
{
    public sealed class DebugAnalyticsService : IAnalyticsService
    {
        public void Track(string eventName, IReadOnlyDictionary<string, object> parameters = null)
        {
            // Kare ve zaman: aynı karede tekrarlanan olaylar log'da ayırt edilebilsin.
            var text = new StringBuilder("[Analytics] frame=").Append(Time.frameCount)
                .Append(" t=").Append(Time.realtimeSinceStartup.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture))
                .Append(' ').Append(eventName);
            if (parameters != null)
                foreach (KeyValuePair<string, object> item in parameters) text.Append(' ').Append(item.Key).Append('=').Append(item.Value);
            Debug.Log(text.ToString());
        }
    }
}
