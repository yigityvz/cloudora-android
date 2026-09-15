using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudora.Services
{
    public sealed class FirebaseAnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsService _fallback;
        private readonly MethodInfo _logEvent;
        public bool IsFirebaseAvailable => _logEvent != null;

        public FirebaseAnalyticsService(IAnalyticsService fallback = null)
        {
            _fallback = fallback ?? new DebugAnalyticsService();
            Type type = Type.GetType("Firebase.Analytics.FirebaseAnalytics, Firebase.App", false) ??
                        Type.GetType("Firebase.Analytics.FirebaseAnalytics, Firebase.Analytics", false);
            _logEvent = type?.GetMethod("LogEvent", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
        }

        public void Track(string eventName, IReadOnlyDictionary<string, object> parameters = null)
        {
            AnalyticsPrivacy.Validate(eventName, parameters);
            if (_logEvent != null) _logEvent.Invoke(null, new object[] { eventName });
            _fallback.Track(eventName, parameters);
        }
    }

    public static class AnalyticsPrivacy
    {
        private static readonly string[] ForbiddenKeys = { "name", "email", "phone", "address", "user_id", "device_id" };

        public static void Validate(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(eventName)) throw new ArgumentException("Event name is required.");
            if (parameters == null) return;
            foreach (string key in parameters.Keys)
                foreach (string forbidden in ForbiddenKeys)
                    if (key.ToLowerInvariant().Contains(forbidden)) throw new ArgumentException($"Personal-data analytics key rejected: {key}");
        }
    }
}
