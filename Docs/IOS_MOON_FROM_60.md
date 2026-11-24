# Ay: beşinci element 60. bölümden itibaren — 19 Ekim 2026

Kullanıcı tercihi: Ay 60'tan itibaren mevcut dört elemente beşinci olarak eklensin; zorluk hedeflerine göre doğrulansın. Önceki başlangıç 81 idi. Güncel yayımlanan katalogda 60–100 Güneş/Yağmur/Kar/Rüzgâr/Ay içerir; mevcut dört element kaldırılmaz.

## Kapsam ve mimari

Yalnızca 60–80 arası 21 tahta yeniden seçildi. 1–59 ile 81–100 arasındaki 79 bölümün board/seed/score/optimal-moves/capacity/band alanları eski veriyle birebir aynı. V3LevelBuilder, V3ThemeBuilder, LevelCandidateSelector ve PuzzleSolver dosyalarının görev başı/sonu SHA256 özetleri aynı; rastgele sayı dizisi ve zorluk formülü değişmedi.

Editor'daki CampaignMoonRollout, tarihsel kurucuların sonucuna uygulanan ayrı bir yayımlama kuralıdır. V3LevelTool veri üretimi ve güncel önizlemelerinde bu kuralı kullanır. Tarihsel builder testleri kalibrasyon referansını korur; MoonRolloutTests yayımlanan güncel kataloğu kontrol eder. Veri dosyası araçla yeniden üretildi; elle yazılmadı.

## Korunan hedefler

- Her etkilenen slot eski ölçülmüş zorluk puanının ±0,6 aralığında seçilir. Gerçek en büyük fark 0,30250; ortalama mutlak fark 0,05975.
- Orta < Zor < Çok Zor döngüsü ve aynı parçada aynı bandın azalmaması korunur. Bantlar/altın ödülleri değişmez.
- Kapasite 5; tuzak/planlama bölümlerinin boş hazne ve tam boş başlangıç sayıları aynı kalır. Çıkmaz toplamları ve ilk hamlede çıkmaz kotaları tarihsel üreticinin sınırlarında, birden çok banda yayılmış kalır.
- Aşırı uzun adaylar alınmaz; yeni çözüm eski bölümden en fazla dört hamle uzundur. Final 60'ın alt sınırı eski finalin 14 hamlelik uzunluğudur; sırf beş renkli havuzun medyanı 17 diye final zorunlu olarak uzatılmaz. Eski finalden kısa değildir ve önceki 31–59'dan daha güçlü tuzak ölçütünü korur.
- 60'ın ölçümü: eski 29,14876 / 14 hamle; yeni 29,00601 / 14 hamle. Yeni seed1962. Ay beş parça olarak birden çok hazneye dağılır, hazır tamamlanmış başlamaz.
- Başlangıçta 100 farklı tahta vardır. Beş rengin her birinden kapasite kadar parça vardır. 60–80'in tamamı güncel kilitli-hazne kurallarıyla çözülebilir; veri yayımlanmadan bu kontrol yapılır.

İlk aramada eski finalin tüm korumalarını yeni havuz medyanıyla birlikte şart koşmak uygun aday vermedi; mevcut bölüm verisine yazılmadı. Eski finalin çözüm uzunluğu korunarak yeniden seçim yapıldı. Puan toleransı/çıkmaz kotaları gevşetilmedi. Gerekirse yalnızca yeni palet için aday havuzu genişletilir; ölçüm formülü ve rastgele sayı algoritması aynı kalır.

Bunlar sayısal/regresyon kontrolleridir. Beşinci renk bazı bölümlerde ek hamle gerektirir; her insan için aynı çözme süresini garanti etmez.

## Kanıtlar

- Logs/moon-rollout-20261019/baseline.json ve published.json: 100 bölümün önce/sonra alanları.
- comparison.csv: her bölümün puan/hamle/seed farkı.
- algorithm-sha256.json: korunan dört temel kaynak dosyanın özetleri.
- regression.xml: 241 toplam; 235 geçti, 0 başarısız, 6 atlama. İki grafik testi başsız çalıştırmada atlandı; diğer dört mevcut atlamadır.

Aktarım, sıvı yükselmesi, yedi bitiriş ve oyun tercihleri bu görevde değiştirilmedi. Cihaz kaydına elle yazılmaz; kurulum mevcut ilerleme üzerine yapılır.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/moon-rollout-20261019/xcode.log`). `/private/tmp/cloudora-ios-moon60-20261019/Cloudora.app` iPhone SE’ye silmeden kuruldu ve devicectl ile açıldı. İş sırasında kayıt bölüm68/highest67/4430 altından bölüm69/highest68/4630 altına ilerledi; fark 68'in 200 altın ödülüyle uyumlu. Can5 ve yardım hakları aynı. Açılış sonrası lastGeneratedSeed353 yeni69. bölümle eşleşti (eski69 seed101); yeni Ay paletli veri cihazda yüklenmiş. Kayıt dosyası ve tercihler elle değiştirilmedi. Kanıt `Logs/moon-rollout-20261019/device-save-check.json`. Fiziksel görsel kabul kullanıcı kontrolüne açıktır.
