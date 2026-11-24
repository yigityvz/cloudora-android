# Hazne bitirişi ve altın kuralları — 18 Ekim 2026

Tamamlanan haznede güneş ışınları, yağmur ve halka, kar kristalleri, rüzgâr kıvrımları, ay yıldızları, dallanan şimşek veya gökkuşağı yayı gösterilir. Cam sabit kalır; efekt yerel, yeniden kullanılan bir vektör katmanıdır. Küçük şişe içi simgeler korunur. Azaltılmış hareket ayarında kısa bir kenar parlaması kullanılır. Son haznenin efekti sonuç paneli açılmadan gösterilir; ödül bu beklemeden önce kaydedilir.

Tam dolu ve hedef kurallarına göre tamamlanmış hazne seçilemez, boşaltılamaz veya karıştırılamaz. Kural görüntü, veri modeli, animasyon ve çalışma anındaki çözüm aramasında aynıdır. Gökkuşağı uyumluluğu korunur. Geri Al, önceki hamleyi geri çevirebilir; bu durumda bitiriş efekti durdurulur.

Bütün bölümlerde orta zorluk 100, zor 200, çok zor 300 altın verir. Beşli mevcut zorluk döngüsü ve bölüm verileri korunur; aynı kural gelecekteki bölüm numaralarına da uygulanır. Başarısızlık sonrası ücretli devam 500 altındır. Yetersiz bakiyeden kesinti yapılmaz, aynı zafer veya devam işlemi iki kez ücret/ödül oluşturmaz. Eski kazanımlar geriye dönük değiştirilmez.

İlk 100 bölümün başlangıç tahtaları yeni kilit kuralıyla çözülebilir. Tarihsel bölüm seçimi/kalibrasyonu yalnızca arşiv amaçlı kilitsiz ölçümü açıkça kullanır; çalışma anındaki çözüm araması kilitlidir. Bölüm tahtaları yeniden üretilmedi.

Doğrulama: grafik açık genel testte 222 geçti, 4 mevcut atlama; tarihsel ölçümle ilgili beş başarısızlık ayrı düzeltmeden sonra hedefli 44/44 testte geçti (3 mevcut atlama). `Logs/completion-final.xml` ve `Logs/completion-authoring.xml`. Grafik testi yedi elementi, sabit camı, değişmeyen içeriği, efekt tekrar kullanımını, Geri Al ve azaltılmış hareketi kontrol eder. Ödül testi 1–10000 döngüsünü, gerçek denetleyicide 100/200/300 ödemeyi ve 499/500 bakiye sınırını kontrol eder. Görsel kayıt `Logs/CompletionReview/completion-preview.mp4`.

Son efekt yaşam döngüsü düzeltmesinden sonra grafik açık dört hedefli test yeniden geçti: `Logs/completion-lifecycle.xml`.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release derlemesi başarılı (`Logs/completion-xcode.log`). Güncel paket `/private/tmp/cloudora-ios-completion-20261018/Cloudora.app` iPhone SE’ye mevcut uygulamanın üzerine kuruldu ve devicectl ile başlatıldı. Kurulum öncesi/sonrası kayıt aynı: bölüm 44, tamamlanan 43, altın 430, can 4 (`Logs/completion-save-check.json`). Kurulum/açılış başarılı; fiziksel efekt görünümü ve oyun hissi kullanıcı kontrolünü bekler.

## İkinci görsel tasarım — daha güçlü element sahneleri

Basit simge/ring bitirişi kaldırıldı. Her bitiriş 1,95 saniyelik hazırlık, ortaya çıkış ve sönüm sahnesi çizer. Son hazneden sonra sonuç paneli 1,78 saniye bekler; ödülün önceden kaydedilmesi korunur. Diğer haznelerdeki bitirişler yeni hamleyi bekletmez.

- Güneş: ayrılan bulutlar, dolu sıcak ışık küresi, farklı uzunlukta ışınlar ve altın tozu.
- Yağmur: oluşan katmanlı bulut, damla sağanağı ve ağızda sıçrayan su tacı/dalga halkaları.
- Kar: sırayla büyüyen altı kollu dallı kristal, camda kırağı ışıltıları ve dağılan kristaller.
- Rüzgâr: hazneyi dipten yukarı saran üç ışıklı sarmal ve dönen yapraklar.
- Ay: dolu hilal silüeti, mor gece halesi, yörüngede ilerleyen yıldız ve takımyıldız çizgileri.
- Şimşek: elektrik yüklenmesi, fırtına bulutundan hazne boyunca inen dallı çakma, kenarlarda artık elektrik ve kıvılcım tacı.
- Gökkuşağı: hazneden yükselen altı renkli şerit, açılan renkli yay, bulut uçları ve prizmatik yıldızlar.

Işıklar vertex renk gradyanlarıdır; bulanık dokular veya ekran geneline flaş kullanılmaz. Tek efekt/Canvas havuzu ve sabit cam korunur. Geometri sınırlıdır; ısıtılmış çizimde managed tahsis kontrolü ve 24.000 vertex tavanı test edilir. Bu editör kontrolü bütün telefonlarda sabit FPS garantisi değildir. Küçük/dolu şişelerdeki simgeler ve aktarım animasyonu değişmedi. Efekt tacı başlığın altında ve ekranın içinde tutulur; 4/6/10 hazneli gerçek 1080 referans Canvas yerleşimleriyle kontrol edilir. Azaltılmış hareket kısa kenar parlamasını kullanmaya devam eder.

Kanıtlar: `Logs/completion-cinematic.xml` içinde 10/10 hedefli test geçti (ekonomi, sabit cam, efekt tekrar kullanımı, Geri Al, azaltılmış hareket, raycast ve performans). Son görsel/kenar testleri `Logs/completion-cinematic-final.xml`; hareketli önizleme `Logs/CompletionReview/cinematic-completion-preview.mp4`, yerleşim kareleri `Logs/CompletionReview/layouts`.

Son efekt testleri: `Logs/completion-cinematic-final.xml` 3/3 geçti. Kapsam: yedi element, sabit cam/veri, tekrar kullanılan efekt, Geri Al, azaltılmış hareket, ısıtılmış çizimde GC.Alloc ve geometri sınırı; 4/6/10 haznede başlık/ekran yerleşimi. Kalabalık tahta testi oyun sahnesindeki 1080 referans Canvas ölçeğini kullanır; dokuz görsel çıktı incelendi.

İkinci tasarım cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/completion-cinematic-xcode.log`). `/private/tmp/cloudora-ios-cinematic-20261018/Cloudora.app` iPhone SE’ye silmeden kuruldu ve devicectl ile açıldı. Önce/sonra kayıt aynı: bölüm 46, tamamlanan 45, altın 830, can 5 (`Logs/completion-cinematic-save-check.json`). Cihazda açılış doğrulandı; gerçek telefon üzerindeki görsel beğeni/FPS ölçümü kullanıcı testine açıktır.
