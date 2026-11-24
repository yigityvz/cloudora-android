using System;

namespace Cloudora.Level
{
    // Bir bölümün çerçevesi: bulut kapasitesi, hava türü (renk) sayısı ve boş bulut sayısı.
    // Oyun tarafında V3LevelPlan kullanır; aday üretimi Editor'daki bölüm aracındadır.
    public readonly struct BoardFrame
    {
        public const int MaxColors = 6; // Sun, Rain, Snow, Wind, Moon, Lightning

        public readonly int Capacity;
        public readonly int ColorCount;
        public readonly int EmptyCount;
        // v3.2 başlangıç dizilimi: tamamen boş başlayan bulut sayısı (0..EmptyCount). EmptyCount ise klasik "dolu + boş" tahta;
        // daha azsa kalan boş alan (EmptyCount × kapasite toplamı korunarak) diğer bulutlara serpiştirilir.
        public readonly int OpenClouds;

        public BoardFrame(int capacity, int colorCount, int emptyCount, int? open = null)
        {
            int openClouds = open ?? emptyCount;
            if (capacity < 2 || capacity > 7) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (colorCount < 2 || colorCount > MaxColors) throw new ArgumentOutOfRangeException(nameof(colorCount));
            if (emptyCount < 0 || emptyCount > 6) throw new ArgumentOutOfRangeException(nameof(emptyCount));
            if (openClouds < 0 || openClouds > emptyCount) throw new ArgumentOutOfRangeException(nameof(open));
            Capacity = capacity;
            ColorCount = colorCount;
            EmptyCount = emptyCount;
            OpenClouds = openClouds;
        }

        public int CloudCount => ColorCount + EmptyCount;
        public bool IsPacked => OpenClouds == EmptyCount;
        public override string ToString() => IsPacked
            ? $"cap{Capacity}/c{ColorCount}/e{EmptyCount}"
            : $"cap{Capacity}/c{ColorCount}/e{EmptyCount}/o{OpenClouds}";
    }
}
