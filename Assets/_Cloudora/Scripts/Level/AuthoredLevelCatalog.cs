using System.Collections.Generic;

namespace Cloudora.Level
{
    public static class AuthoredLevelCatalog
    {
        private static readonly IReadOnlyList<LevelDefinition> Levels = Build();

        public static int Count => Levels.Count;

        public static LevelDefinition Get(int levelNumber)
        {
            int index = System.Math.Max(1, System.Math.Min(levelNumber, Count)) - 1;
            return Levels[index];
        }

        // 1–15: v3'te zorluğu ölçülüp seçilen tahtalar (V3LevelData). Öğretici yazılar, kapasite ve
        // "elle tasarlanmış" durumu (Shuffle 16'ya kadar kilitli) eski bölümlerle aynı.
        // v2'ye kadarki elle yazılmış tahtalar git geçmişinde (etiket v2).
        private static IReadOnlyList<LevelDefinition> Build()
        {
            return new[]
            {
                V(1, "Bir hazneye dokun."),
                V(2, "Aynı hava türünün olduğu hazneyi seç."),
                V(3, "Aynı türden parçalar birlikte akar."),
                V(4, "Boş hazneler sana yer açar."),
                V(5, "Gökyüzünü dengele."),
                V(6, null),
                V(7, null),
                V(8, null),
                V(9, null),
                V(10, "Geri Al ile son hamleni geri çevirebilirsin."),
                V(11, null),
                V(12, null),
                V(13, null),
                V(14, null),
                V(15, "İlk dünyanı keşfetmeye hazırsın.")
            };
        }

        // 16+: v3 bölüm aracıyla kurulmuş parçalar (V3LevelData). Kurulmamış bölümler eski üreticiden gelir.
        // Dünya kimliği şimdilik mevcut dünya listesinden (yeni aralıklara geçiş ayrı adım).
        public static bool TryGetBuilt(int levelNumber, out LevelDefinition definition)
        {
            definition = null;
            if (levelNumber <= Count) return false;
            V3LevelEntry entry = V3LevelData.Find(levelNumber);
            if (entry == null) return false;
            definition = new LevelDefinition(levelNumber, Progression.WorldCatalog.ForLevel(levelNumber).Id, entry.Capacity, null, entry.CreateBoard())
            {
                seed = entry.Seed,
                difficultyScore = entry.Score
            };
            return true;
        }

        private static LevelDefinition V(int id, string cue)
        {
            V3LevelEntry entry = V3LevelData.Find(id)
                ?? throw new System.InvalidOperationException($"V3 level data missing for level {id}; run Cloudora/V3/Bölüm Verisini Üret.");
            return new LevelDefinition(id, "green-valley", entry.Capacity, cue, entry.CreateBoard())
            {
                seed = entry.Seed,
                difficultyScore = entry.Score
            };
        }
    }
}
