using System;

namespace Cloudora.Level
{
    // v3: 1–100 parçaları (geçici adlar) — bkz. Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md
    public enum V3Part { Intro, Adaptation, TransitionTheme, Theme2, Theme3 }

    public static class V3LevelPlan
    {
        public const int LastLevel = 100;
        public const int ShuffleUnlockLevel = 16;

        public static V3Part PartFor(int level)
        {
            Check(level);
            return level <= 5 ? V3Part.Intro :
                level <= 15 ? V3Part.Adaptation :
                level <= 30 ? V3Part.TransitionTheme :
                level <= 60 ? V3Part.Theme2 : V3Part.Theme3;
        }

        public static (int First, int Last) RangeOf(V3Part part) => part switch
        {
            V3Part.Intro => (1, 5),
            V3Part.Adaptation => (6, 15),
            V3Part.TransitionTheme => (16, 30),
            V3Part.Theme2 => (31, 60),
            _ => (61, 100)
        };

        // Her 5 bölümde: Orta, Orta, Zor, Orta, Çok zor. "Kolay" yok.
        public static DifficultyBand BandFor(int level)
        {
            Check(level);
            return ((level - 1) % 5) switch
            {
                2 => DifficultyBand.Hard,
                4 => DifficultyBand.VeryHard,
                _ => DifficultyBand.Medium
            };
        }

        // Bölümün tahta çerçevesi. 1–15 eski elle tasarlanmış bölümlerin çerçevesini korur (kapasite 4, 2 boş bulut).
        // Henüz tasarlanmamış parçalar bilerek hata verir.
        public static BoardFrame FrameFor(int level)
        {
            return PartFor(level) switch
            {
                V3Part.Intro => new BoardFrame(4, 2, 2),
                V3Part.Adaptation => new BoardFrame(4, 3, 2),
                V3Part part => throw new NotSupportedException($"V3 part {part} (level {level}) is not designed yet.")
            };
        }

        // Bandın, parçadaki aday zorluk dağılımında kapladığı yüzdelik dilim (puana göre sıralı).
        public static (float Low, float High) QuantilesFor(DifficultyBand band) => band switch
        {
            DifficultyBand.Medium => (0.30f, 0.60f),
            DifficultyBand.Hard => (0.70f, 0.85f),
            DifficultyBand.VeryHard => (0.90f, 1.00f),
            _ => throw new ArgumentOutOfRangeException(nameof(band), "v3 uses Medium, Hard and VeryHard only.")
        };

        private static void Check(int level)
        {
            if (level < 1 || level > LastLevel) throw new ArgumentOutOfRangeException(nameof(level));
        }
    }
}
