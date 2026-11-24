using System;
using System.Collections.Generic;
using Cloudora.Level;
using Cloudora.Modifiers;
using Cloudora.Puzzle;

namespace Cloudora.Services
{
    [Serializable]
    public sealed class FailedSnapshot
    {
        public string board;
        public ModifierData[] modifiers = Array.Empty<ModifierData>();
    }

    // v3.1: "No Moves" ekranındayken uygulama kapanırsa, açılışta aynı ekran aynı tahta ve Undo geçmişiyle geri gelir.
    // Böylece kapatıp açmak "Retry (−1 can)"ı atlatmaz ve "devam" çözülebilen son duruma yine dönebilir.
    [Serializable]
    public sealed class FailRecord
    {
        public int level;                                          // 0 = kayıt yok
        public string board;                                       // V3BoardText (ekstra bulutlar dahil)
        public int moveCount;
        public FailedSnapshot[] history = Array.Empty<FailedSnapshot>(); // Undo geçmişi, en yeniden en eskiye

        public bool IsEmpty => level <= 0;

        public static FailRecord Capture(int level, WeatherType[][] board,
            IReadOnlyList<(WeatherType[][] Board, ModifierData[] Modifiers)> historyNewestFirst, int moveCount)
        {
            var record = new FailRecord
            {
                level = level,
                board = V3BoardText.Format(board),
                moveCount = moveCount,
                history = new FailedSnapshot[historyNewestFirst.Count]
            };
            for (int i = 0; i < historyNewestFirst.Count; i++)
            {
                record.history[i] = new FailedSnapshot
                {
                    board = V3BoardText.Format(historyNewestFirst[i].Board),
                    modifiers = Clone(historyNewestFirst[i].Modifiers)
                };
            }
            return record;
        }

        // Kayıt bu bölüme ait ve tutarlıysa tahtayı ve geçmişi verir; değilse false (bölüm normal açılır).
        public bool TryRestore(int expectedLevel, out WeatherType[][] restoredBoard,
            out List<(WeatherType[][] Board, ModifierData[] Modifiers)> historyNewestFirst)
        {
            restoredBoard = null;
            historyNewestFirst = null;
            if (IsEmpty || level != expectedLevel || string.IsNullOrEmpty(board)) return false;
            try
            {
                WeatherType[][] current = V3BoardText.Parse(board);
                var list = new List<(WeatherType[][] Board, ModifierData[] Modifiers)>();
                foreach (FailedSnapshot snapshot in history ?? Array.Empty<FailedSnapshot>())
                {
                    WeatherType[][] past = V3BoardText.Parse(snapshot.board ?? string.Empty);
                    if (past.Length != current.Length) return false;
                    list.Add((past, Clone(snapshot.modifiers)));
                }
                restoredBoard = current;
                historyNewestFirst = list;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static ModifierData[] Clone(ModifierData[] source)
        {
            if (source == null) return Array.Empty<ModifierData>();
            var copy = new ModifierData[source.Length];
            for (int i = 0; i < source.Length; i++) copy[i] = source[i].Clone();
            return copy;
        }
    }
}
