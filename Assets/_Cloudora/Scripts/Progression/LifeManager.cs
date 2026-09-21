using System;

namespace Cloudora.Progression
{
    public sealed class LifeManager
    {
        public const int MaxLives = 5;
        public static readonly TimeSpan RegenerationInterval = TimeSpan.FromMinutes(30);
        public int Lives { get; private set; }
        public DateTime NextLifeUtc { get; private set; }

        public LifeManager(int lives = MaxLives, DateTime nextLifeUtc = default)
        {
            Lives = Math.Max(0, Math.Min(MaxLives, lives));
            NextLifeUtc = nextLifeUtc;
        }

        public void Refresh(DateTime utcNow)
        {
            if (Lives >= MaxLives) { Lives = MaxLives; NextLifeUtc = default; return; }
            if (NextLifeUtc == default) { NextLifeUtc = utcNow + RegenerationInterval; return; }
            while (Lives < MaxLives && utcNow >= NextLifeUtc) { Lives++; NextLifeUtc += RegenerationInterval; }
            if (Lives >= MaxLives) NextLifeUtc = default;
        }

        public bool TryConsumeRetry(DateTime utcNow, bool tutorialGrace)
        {
            Refresh(utcNow);
            if (tutorialGrace) return true;
            if (Lives <= 0) return false;
            Lives--;
            if (NextLifeUtc == default) NextLifeUtc = utcNow + RegenerationInterval;
            return true;
        }

        public bool CanBeginAttempt(DateTime utcNow, bool tutorialGrace)
        {
            Refresh(utcNow);
            return tutorialGrace || Lives > 0;
        }

        public void Grant(int amount)
        {
            Lives = Math.Min(MaxLives, Lives + Math.Max(0, amount));
            if (Lives >= MaxLives) NextLifeUtc = default;
        }

        public TimeSpan TimeUntilNext(DateTime utcNow)
        {
            Refresh(utcNow);
            return Lives >= MaxLives ? TimeSpan.Zero : (NextLifeUtc > utcNow ? NextLifeUtc - utcNow : TimeSpan.Zero);
        }
    }
}
