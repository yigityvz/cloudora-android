# Cloudora iOS — proje hafızası

Bu depo yalnızca `yigityvz/cloudora-ios` içindir. Çalışma dalı `main`.

- Unity 6000.5.9f1; iPhone, iOS 15+, IL2CPP ARM64, Metal, dikey ekran.
- Oyunun kodu, sahneleri, bölüm verileri, görselleri ve ekonomisi platform temizliği kapsamında değiştirilmez.
- 17 Ekim: ilk Figma ekran grubu uygulandı; oyun ekonomisi önerileri uygulanmadı. Kalan tasarım kapsamı ve doğrulama `IOS_DESIGN_IMPLEMENTATION.md` içinde.
- İlk 100 bölüm mevcut v3.2 verisini kullanır; sonraki bölümler mevcut üreticiyi kullanır.
- Derleme: `Build-iOS.sh`; ayrıntı `IOS_BUILD.md`, sonuçlar `IOS_STATUS.md`.
- 16 Ekim 2026: imzalı uygulama iPhone'a kuruldu, kullanıcı ilk test adımlarını yaptığını belirtti. İsteğiyle yalnızca yerel cihaz kaydı 80. bölüme alındı; önceki kayıt yedeklendi. Cihaz kayıtları Git dışındadır.
- Eski geliştirme ve aktarım geçmişi Git geçmişinde bulunur. Güncel platform hedefi için bu belge ve iOS belgeleri esas alınır.

## Açılış tasarımı — 16 Ekim

Figma açılışı `OpeningSplash` ile eklendi; `MainMenuController.Create` tek entegrasyon noktasıdır. Bu, 16 Ekim durumudur; 17 Ekim ekran aktarımı ayrıca belgelendi. Oyun kuralları ve kayıtlar korunur. Ayrıntı `IOS_OPENING_SPLASH.md`.

## Kalite ve kapsam düzeltmesi — 17 Ekim

Güncel oynanabilir kapsam artık 100 bölüm / 3 dünyadır: tanıtım 1–15, Yeşil Vadi 16–40, Rainy Coast 41–70, Frost Peaks 71–100. Eski 13 dünya kapsamı güncel değildir. Kalite, akış animasyonu, gerçek yüzde ve kazanma sonrası ana sayfa değişiklikleri `IOS_QUALITY_CAMPAIGN.md` içinde. Bölüm tahtaları ve ekonomi korunur.

## Son UX ve mantık düzeltmesi

Güncel aralıklar: tanıtım 1–14, Yeşil Vadi 15–30, Rainy Coast 31–60, Frost Peaks 61–100. Kayıp koşulu çözümsüzlük kanıtıdır; sadece legal hamle kontrolü kullanılmaz. Bitmemiş bölümden eve dönüş 1 can harcar ve başlangıç tahtasını yükler; kazanma sonrası dönüş ücretsizdir. Arayüz, bulutsuz hazneler ve aktarım yeniden düzenlendi. Güncel ayrıntı/kanıt: `IOS_UX_REVIEW.md`.

## Sabit hazneler ve element hareketi

Aktarım sırasında şişe hareket etmez; yalnızca element taşınır. Seçim ve çözülme ışıkla belirtilir. Yedi elementin ayrı stili, yeni element profilleri ve grafik test kanıtları `IOS_ELEMENT_MOTION.md` içinde. Zorluk rozeti 5 pt yukarı taşındı; tahta zorluğu değişmedi.

## Kaliteyi koruyan performans iyileştirmesi

Cam/bölge/aktarım efektleri tekrar kullanılıyor; çizim listeleri ve geçici diziler kaldırıldı. Kaynak arayüzü yalnızca değişince güncelleniyor. 60 FPS hedefi ve görüntü kalitesi korunuyor. Genel, görsel ve GC.Alloc testleri geçti; ayrıntı `IOS_PERFORMANCE.md`.

## 18 Ekim: modern element simgeleri

Şişe içi yedi simge daha küçük, tutarlı monoline vektörlerle değiştirildi; dolu şişelerde 18 pt üst sınır var. Güneş/kar açık dolguda koyu, diğerleri kırık beyaz. Ayrıntı `IOS_WEATHER_SYMBOLS.md`; 17/17 hedefli test geçti.

### 18 Ekim — bitiriş efekti ve ekonomi
Tamamlanan hazne yedi elemente özel 1,15 sn yerel efekt gösterir; cam hareket etmez. Tamamlanan kaynaktan aktarma/Shuffle hem UI hem kurallar/çözüm aramasında kilitlidir. Son efektin ardından sonuç açılır; ödül beklemeden önce kaydedilir. Orta 100 / Zor 200 / Çok Zor 300 altın; ücretli devam tüm bölümlerde 500. Bölüm verileri ve geçmiş kazanımlar değiştirilmedi. Ayrıntı `IOS_COMPLETION_EFFECTS.md`.

### 18 Ekim — bitiriş sahnelerinin ikinci tasarımı
Kullanıcı ilk bitiriş efektlerini çok basit buldu. VesselCompletionFlourish artık 1,95 sn hazırlık/element sahnesi/sönüm içerir: güneş doğuşu, sağanak-su tacı, büyüyen kristal, hazne boyunca yapraklı girdap, dolu hilal-yörünge, hazne boyunca ana çakma, altı renkli yay. Cam sabit; aktarım/oyun/ekonomi korunur. ResultDelay 1,78 sn (azaltılmış hareket .20). İlk genel 10 ve son 3 görsel/performans testi geçti. Ayrıntı `IOS_COMPLETION_EFFECTS.md`.

### 18 Ekim — sıvı aktarımı, ay tutulması, cihaz 60
MoveAnimator paket/simge uçurmaz; kesintisiz yumuşak kenarlı sıvı şeridi kaynak/hedef yüzeylerine bağlıdır. LiquidFillGraphic yalnızca aktarımda dalgalanır; aynı element var olan bölgede uzar, çift simge/blok/dikiş oluşmaz. Game-state commit denetleyicide aynı kalır. Ay bitirişi orbit/hilal yerine gölgeden çıkan kraterli dolunay ve ışık yansımasıdır. Son yeni 7 ve zamanlama/dokunma 5 test geçti; ilk regresyonda diğer 11 geçti. Kullanıcı 100 bölümü tamamladı; tam kayıt Logs/device-backup-before-liquid-20261018 yedeğinde. İsteğiyle cihaz currentLevel60/highest59'a alınır; 3630 altın, 5 can ve yardımcı haklar korunur. Ayrıntı IOS_LIQUID_MOTION.md.

### Önemli kullanıcı düzeltmesi — üstten ekleme korunacak
Kesintisiz sıvı şeridi yanlış yorumdu ve kaldırıldı. Önceki elemente özel uçuş/simge/iz ve üstten giriş geri getirildi. SADECE hazne içindeki aynı renk tek gövdede sıvı gibi yükselir; ikinci blok/logo oluşmaz. Diğer onaylanmış bitiriş tasarımlarına dokunulmaz; ay için istenen yeni dolunay korunur. Telefondaki Cloudora.ReducedMotion=1 bitirişleri gizliyordu, talebe göre oyun tercihi 0 yapılır. Kayıt61/highest60/3430coin/5can korunur. Gerçek denetleyici hamlesi yedi bitirişi tetikleyerek geçti. Ayrıntı IOS_ITEM_TRANSFER_RESTORE.md.

## 19 Ekim: Ay 60'ta beşinci element

Kullanıcı beşinci element seçeneğini açıkça seçti. Ay 81 yerine 60'ta başlar. Yalnızca 60–80 yeniden seçildi; diğer 79 bölüm birebir aynı. Temel üretici/RNG/puanlama/solver değişmedi; yayımlama katmanı CampaignMoonRollout ve araç çıktısı güncellendi. Puan farkı en fazla0,3025 (izin ±0,6), bantlar/boş hazne planı/çıkmaz kotaları ve runtime çözülebilirliği doğrulandı. 60 hâlâ14 hamle. Genel test235 geçti/6 atlama/0 hata. Ayrıntı IOS_MOON_FROM_60.md. Diğer hareket/bitirişlere ve cihaz tercihlerine dokunulmadı.

## 19 Ekim: GitHub kaynak teslimi

Kullanıcı güncel oyunun cloudora-ios reposuna pushlanmasını istedi. Teslim hedefi origin/main; açılıştan Ay60 güncellemesine kadar mevcut kaynaklar, kaynak görseller/metalar, testler ve iOS belgeleri birlikte commit edilir. Bu teslimde oyun davranışı değiştirilmez. Logs/Builds/Library, cihaz kayıtları ve imzalama bilgileri paylaşılmaz; Unity Project Auditor’ın numaralı yerel kopyaları ignore edilir. Son doğrulama: 235 geçti, 6 atlandı, 0 hata; iOS Release cihaz kurulumu başarılı.
