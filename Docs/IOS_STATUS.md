# iOS aktarım ve tasarım durumu — 2026-10-17

Kaynak: `yigityvz/cloudora-android`, `cloudora-bahadir`, `d7c9416` (v3.2 + belge kayıtları). Hedef: `yigityvz/cloudora-ios`, `main`.

## İlk platform aktarımında korunan kapsam (16 Ekim)
Runtime C# kodu, UI/oynanış kodu, kayıt/jeton/başarısızlık/Shuffle davranışları, sahneler, prefablar, bölüm verileri, görseller, oyun kaynak JSON'ları kaynak sürümle aynıdır. Figma/Stitch tasarımları veya konuşulan yeni oyun kuralları uygulanmadı.

Önceki iOS portunun native haptic adaptörü ve safe-area-only layout yaması taşınmadı; kaynak Android runtime bütünüyle esas alındı. Unity `Handheld.Vibrate` çağrısı korunuyor.

## iOS'a özel farklar
- Platform temizliğinde Android Editor araçları, manifest testi, build profili, Google Play yapılandırması ve kullanılmayan JNI modülü kaldırıldı.
- iOS build aracı ve shell betiği: iPhone, portrait, ARM64/IL2CPP, Metal, iOS 15, mevcut ikon, `com.yigityvz.cloudora`. Ürün adı/versiyon kaynak ayarlarından korunur.
- iOS PlayerSettings; yerel Team/profil/sertifika depoya eklenmez.
- Build belgeleri ve iOS artifact ignore kuralları.

## Doğrulama
| Kontrol | Sonuç | Kanıt |
|---|---|---|
| Oyun runtime ve içerik eşliği | PASS | Scripts, Scenes, Art ve Prefabs için temizlik öncesine göre Git diff boş |
| EditMode, iOS hedefi | PASS | Temizlik sonrası 195 toplam, 191 geçti, 4 bilinçli atlama, 0 başarısız; Logs/ios-editmode.xml |
| 1–100 kalibrasyonu | PASS | 100 ayrı bölüm raporu; Logs/ios-v32-calibration.log; veri yeniden üretilmedi |
| Git LFS bütünlüğü | PASS | 6 tarihsel nesne; fsck OK |
| Unity Xcode export | PASS | 0 hata, 5 ikon sıkıştırma uyarısı; Logs/ios-build.log |
| Native iOS ARM64 derleme | PASS | Xcode BUILD SUCCEEDED, CODE_SIGNING_ALLOWED=NO; Logs/ios-v32-xcode-build.log |
| Güncel sürüm imzalama | PASS | Logs/ios-v32-signed-build.log; yerel Team, BUILD SUCCEEDED |
| iPhone SE kurulumu ve açılış | PASS | Logs/ios-v32-device-install.log / ios-v32-device-launch.log; çalışan süreç doğrulandı |
| Fiziksel oynanış | MANUAL TEST IN PROGRESS | Kullanıcı kısa testi yaptığını bildirdi; kayıt bölüm 10'a ilerlemişti. İsteğiyle yedek alınarak test kaydı 80'e getirildi |
| App Store yayını | NOT RUN | Bu çalışma yalnızca platform aktarımıdır |

Native ürün: `/private/tmp/cloudora-ios-bahadir-20261016/Cloudora.app` (geçici; 2026-10-16 cihaz testi için yerel olarak imzalandı). Bundle `com.yigityvz.cloudora`, version `0.1.0`, build `1`, minimum iOS `15.0`. Unity projesi ve Xcode export kalıcı çalışma kopyasında; geçici ürünü macOS silebilir.

Mevcut 512px ikonun sıkıştırma uyarıları ve Unity'nin ürettiği IL2CPP komutlarındaki eski minimum-OS uyarıları korunmuştur; derleme başarılıdır. Bunları gidermek için sanat veya oyun kaynakları değiştirilmedi. Unity'nin mevcut paket sürümü override uyarıları nedeniyle manifest/lock yeniden pinlenmedi.

Fiziksel testte yeni v3.2 sürümüyle normal hamleler, tek bulut Shuffle, Undo, Extra, Retry, 40 coin/reklamla son çözülebilir duruma dönüş ve başarısızlık kaydının kapat-aç davranışı doğrulanmalıdır. Android dalında bilinen sınırlamalar bu portta düzeltilmedi.

Cihaz testi başlangıcı: 2026-10-16 11:32 +03. Kurulumdan önce mevcut uygulamanın Documents klasörü yerel/ignore edilen Logs altında yedeklendi (bölüm 5, tamamlanan 4, can 5). Uygulama silinmeden güncellendi; kayıt değiştirilmedi. İmzalama bilgileri depoya eklenmedi.

16 Ekim depo temizliği sonrası EditMode testleri tekrar geçti. Önceki Xcode ve cihaz sonuçları temizlik öncesi derlemeye aittir. Unity ortak ayar şemalarındaki platform alanları ve oyun görselleri korunmuştur.

## Açılış ekranı — 16 Ekim 2026

- Figma bulut perdesi ve logo animasyonu eklendi; mevcut ana menü korunur.
- EditMode: 197 toplam, 193 geçti, 4 atlandı, 0 başarısız. Son opaklık düzeltmesinden sonra açılışa ait 2 test tekrar geçti (`Logs/ios-splash-tests.xml`).
- Unity UI önizlemeleri: 390×844, 375×667, 393×852; başlangıç, belirme, bekleme ve azaltılmış hareket kareleri kontrol edildi.
- İlk export, yerel `Library/BuildPlayerData` altında yinelenmiş `TypeDb-All 2.json` dosyaları nedeniyle hata kayıtları üretti. Bu çıktı cihaz kurulumu için kullanılmadı; üretilen önbellek ve export Git dışına yedeklenerek tekrar derlendi.

- Temiz export: `Succeeded`, 0 hata, 5 mevcut ikon uyarısı (`Logs/ios-build.log`). İmzalı Xcode derlemesi `BUILD SUCCEEDED` (`Logs/ios-splash-signed-build.log`); UIKit azaltılmış hareket köprüsü native hedefe dahil.

- Açılış sürümü iPhone’a uygulama silinmeden kuruldu ve başlatıldı; çalışan süreç doğrulandı. Kurulum öncesi cihaz kaydı yedeklendi. Animasyonun gerçek cihazdaki görsel kabulü kullanıcı kontrolünü bekliyor.

### Açılış süresi düzeltmesi — 16 Ekim 2026

Kullanıcının kısa açılış bildiriminden sonra sayaç ilk çizilen kare sonrasında başlatıldı. Soğuk yükleme süresi 3 saniyelik animasyona dahil edilmez. Odaklı 2 açılış testi geçti; Unity iOS export 0 hata, Xcode imzalı build başarılı. Güncelleme mevcut uygulamanın üzerine iPhone’a kuruldu. Fiziksel ekrandaki süre kullanıcı tarafından tekrar kontrol edilecek.

## İlk ekranların Figma aktarımı — 17 Ekim 2026

Ana sayfa, dünya listesi/ziyaret, ayarlar, oynanışın ortak bileşenleri, sonuç panelleri ve dünya geçişleri uGUI ile uygulandı. Oyun kuralları/ekonomi/kayıt formatı korunur; tasarımdaki örnek ekonomi uygulanmaz. Ayrıntı ve kalan görsel kapsam: `IOS_DESIGN_IMPLEMENTATION.md`.

200 EditMode testinin 196’sı geçti, 4 mevcut test atlandı. Unity export başarılı: 0 hata, 5 mevcut ikon uyarısı. 375×667 ve 390×844 önizlemeler incelendi. İmzalı Xcode derlemesi başarılı (`Logs/ios-design-signed-build.log`, `BUILD SUCCEEDED`); ürün `/private/tmp/cloudora-ios-design-20261017/Cloudora.app`. Bu tasarım sürümü henüz cihaza kurulmuş sayılmaz. Cihaz yedeği ilk denemede telefon kilitli olduğu için alınamadı.

17 Ekim cihaz kurulumu: telefon açıldıktan sonra Documents yedeği alındı; yeni sürüm mevcut uygulamanın üzerine kuruldu ve başlatıldı. Çalışan Cloudora süreci doğrulandı. Kurulum öncesi kayıt: bölüm 12, tamamlanan 11, jeton 95, can 5. Açılış sonrası okunan kayıt: bölüm 13, tamamlanan 12, jeton 105, can 5; kayıt sıfırlanmamış ve sonraki bölüme ilerlemiş. Bu aradaki fiziksel etkileşim araçla gözlemlenmedi. Kanıt: `Logs/ios-design-device-install.log`, `Logs/ios-design-device-launch.log`; cihaz kayıtları Git dışındadır. Fiziksel görünüm ve dokunma kabulü kullanıcı kontrolünü bekliyor.

## Retina kalite ve 100 bölüm güncellemesi — 17 Ekim

Yeni kullanıcı isteğiyle oynanabilir katalog üç dünya / 100 bölüm olarak sınırlandı. Retina font çizimi ve görsel import ayarları düzeltildi; akış animasyonu, gerçek dünya yüzdeleri ve kazanma sonrası Ana Sayfaya Dön eklendi. Ayrıntı `IOS_QUALITY_CAMPAIGN.md`. Önceki bölümdeki cihaz kurulumu bu yeni kalite güncellemesini kapsamaz; yeni test/derleme sonucu aşağıya eklenecek.

Kalite güncellemesi test sonucu: 206 toplam, 202 geçti, 4 mevcut atlama, 0 başarısız (`Logs/ios-editmode.xml`). Akış testi sıfır yükseklikteki çizim hatasını yakaladı; koruma eklendikten sonra tam test seti geçti. Yeni Unity iOS export başarılı (0 hata, 6 uyarı: 5 mevcut ikon uyarısı ve akış grafiğinin Start alanı için ad gölgeleme uyarısı). Filigransız yerel cihaz sürümü test ödüllerini `CLOUDORA_DEVICE_QA` ile korur. Native imzalı Release derlemesi hazırlanıyor.

Kalite güncellemesi native Release derlemesi başarılı (`Logs/ios-quality-signed-build.log`, BUILD SUCCEEDED). `/private/tmp/cloudora-ios-quality-20261017/Cloudora.app` mevcut uygulamanın üzerine kuruldu ve başlatıldı (`Logs/ios-quality-device-install.log`, `Logs/ios-quality-device-launch.log`). Kurulum öncesi/sonrası kayıt: currentLevel: 19 → 19, highestCompletedLevel: 18 → 18, coins: 185 → 185, lives: 5 → 5. Görsel kabul ve fiziksel dokunma testi kullanıcıya aittir; cihaz ekranını araçla görüntülemedik.

## Son UX ve çıkmaz kontrolü — 17 Ekim

Güncel dünya aralıkları 15–30, 31–60, 61–100; tanıtım 1–14. `IOS_UX_REVIEW.md` güncel davranışı ve ekran incelemesini kaydeder. Genel test setinde 213 geçti / 4 atlama; ek grafik açık dokunma/aktarım testi ve ek hazne önbellek regresyonu geçti. Unity export başarılı: 0 hata, 5 mevcut ikon uyarısı. Son paket imzalama aşamasında.

Son UX paketi: Unity export 0 hata / 5 mevcut ikon uyarısı; imzalı Release Xcode derlemesi başarılı (`Logs/ios-ux-signed-build.log`). iPhone’a mevcut uygulamanın üzerine kuruldu ve çalışan süreç doğrulandı (`Logs/ios-ux-device-install.log`, `Logs/ios-ux-device-launch.log`). Kurulum öncesi/sonrası kayıt: currentLevel: 21 → 22, highestCompletedLevel: 20 → 21, coins: 215 → 225, lives: 5 → 5. `Logs/MotionReview/pour-preview.mp4` gerçek editör çalışmasından alınan animasyon önizlemesidir. Cihazdaki görsel kabul kullanıcı kontrolünü bekler.

## Son element aktarımı güncellemesi

Şişeler sabitken elemente özel kavisli aktarım ve ışıklı seçim uygulandı. Yeni bölüm ve element profili desteği, regresyon ve görsel test ayrıntıları: [IOS_ELEMENT_MOTION.md](IOS_ELEMENT_MOTION.md).

## Kaliteyi koruyan performans iyileştirmesi

Cam/bölge/aktarım efektleri tekrar kullanılıyor; çizim listeleri ve geçici diziler kaldırıldı. Kaynak arayüzü yalnızca değişince güncelleniyor. 60 FPS hedefi ve görüntü kalitesi korunuyor. Genel, görsel ve GC.Alloc testleri geçti; ayrıntı `IOS_PERFORMANCE.md`.

## 18 Ekim: modern element simgeleri

Şişe içi yedi simge daha küçük, tutarlı monoline vektörlerle değiştirildi; dolu şişelerde 18 pt üst sınır var. Güneş/kar açık dolguda koyu, diğerleri kırık beyaz. Ayrıntı `IOS_WEATHER_SYMBOLS.md`; 17/17 hedefli test geçti.

## 18 Ekim: hazne bitirişi ve ekonomi

Yedi elemente özel yerel bitiriş animasyonu, tamamlanan haznenin boşaltılmasının engellenmesi ve 100/200/300 altın ödül / 500 altın ücretli devam kuralları uygulandı. İlk 100 bölümün çözülebilirliği doğrulandı; bölüm verileri korunur. Ayrıntılar `IOS_COMPLETION_EFFECTS.md`.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release derlemesi başarılı (`Logs/completion-xcode.log`). Güncel paket `/private/tmp/cloudora-ios-completion-20261018/Cloudora.app` iPhone SE’ye mevcut uygulamanın üzerine kuruldu ve devicectl ile başlatıldı. Kurulum öncesi/sonrası kayıt aynı: bölüm 44, tamamlanan 43, altın 430, can 4 (`Logs/completion-save-check.json`). Kurulum/açılış başarılı; fiziksel efekt görünümü ve oyun hissi kullanıcı kontrolünü bekler.

## Bitiriş efektlerinin ikinci tasarımı — 18 Ekim

Yedi elementte 1,95 saniyelik hazırlık/ortaya çıkış/sönüm sahneleri eklendi; basit simge bitirişi yerine dolu gök cisimleri, hava olayları, hazne boyunca şimşek ve sarmal ışıklar kullanılıyor. Ayrıntı `IOS_COMPLETION_EFFECTS.md`. Bu değişiklik oyun kuralları, bölüm verileri ve ekonomi üzerinde değişiklik yapmaz.

İkinci tasarım cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/completion-cinematic-xcode.log`). `/private/tmp/cloudora-ios-cinematic-20261018/Cloudora.app` iPhone SE’ye silmeden kuruldu ve devicectl ile açıldı. Önce/sonra kayıt aynı: bölüm 46, tamamlanan 45, altın 830, can 5 (`Logs/completion-cinematic-save-check.json`). Cihazda açılış doğrulandı; gerçek telefon üzerindeki görsel beğeni/FPS ölçümü kullanıcı testine açıktır.

## Sıvı aktarımı ve yeni ay sahnesi — 18 Ekim

Aktarım artık tek kesintisiz sıvı şeridi; kaynak/hedef yüzeyleri eşzamanlı değişir, aynı element tek renk gövdesinde büyür. Hareketli yüzey/durulma ve menisküs eklendi. Ay bitirişi gölgeden çıkan kraterli dolunay ve hazneye yansıyan ay ışığı olarak yeniden tasarlandı. Test/cihaz ayrıntısı `IOS_LIQUID_MOTION.md`.

Cihaz sonucu: Unity export 0 hata / 5 mevcut ikon uyarısı, imzalı Xcode Release başarılı (`Logs/liquid-xcode.log`). `/private/tmp/cloudora-ios-liquid-20261018/Cloudora.app` iPhone SE’ye silmeden kuruldu ve açıldı. Kullanıcı isteğiyle canlı kayıt bölüm 60 / tamamlanan 59 olarak ayarlandı; 3630 altın, 5 can, yardım hakları ve tercihler korundu. 100/100 bitmiş kayıt ayrı yedekte durur. Açılış sonrası okuma ile doğrulandı (`Logs/liquid-level60-check.json`). Fiziksel görsel beğeni ve cihaz FPS ölçümü kullanıcı kontrolüne açıktır.

## Üstten ekleme geri getirildi — 18 Ekim

Kullanıcının düzeltmesiyle elemente özel simge/iz/parçacıklı üstten giriş geri getirildi; sadece hazne içindeki yükselme/birleşme sıvıdır. Yedi bitiriş gerçek hamle testiyle doğrulandı; sahnelerin kodu korunur. Telefonda efektleri kısaltan Cloudora.ReducedMotion=1 tespit edildi. Ayrıntı `IOS_ITEM_TRANSFER_RESTORE.md`.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/item-restore-xcode.log`). `/private/tmp/cloudora-ios-item-restore-20261018/Cloudora.app` iPhone SE’ye silmeden kuruldu ve başlatıldı. Yalnızca oyunun Cloudora.ReducedMotion tercihi 1→0 yapıldı; açılış sonrası tekrar okundu ve 0 doğrulandı. Kayıt dosyasına yazılmadı. İş sırasında oyun kaydı bölüm61/highest60'tan bölüm62/highest61'e ilerlemiş, altın3430→3530 ve Undo3→2 olmuş; can5, diğer haklar aynı. Bu ilerleme geri alınmadı (`Logs/item-restore-save-check.json`). Fiziksel görsel kabul kullanıcı testine açıktır.

## 19 Ekim: Ay 60'ta beşinci element

Kullanıcı beşinci element seçeneğini açıkça seçti. Ay 81 yerine 60'ta başlar. Yalnızca 60–80 yeniden seçildi; diğer 79 bölüm birebir aynı. Temel üretici/RNG/puanlama/solver değişmedi; yayımlama katmanı CampaignMoonRollout ve araç çıktısı güncellendi. Puan farkı en fazla0,3025 (izin ±0,6), bantlar/boş hazne planı/çıkmaz kotaları ve runtime çözülebilirliği doğrulandı. 60 hâlâ14 hamle. Genel test235 geçti/6 atlama/0 hata. Ayrıntı IOS_MOON_FROM_60.md. Diğer hareket/bitirişlere ve cihaz tercihlerine dokunulmadı.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/moon-rollout-20261019/xcode.log`). `/private/tmp/cloudora-ios-moon60-20261019/Cloudora.app` iPhone SE’ye silmeden kuruldu ve devicectl ile açıldı. İş sırasında kayıt bölüm68/highest67/4430 altından bölüm69/highest68/4630 altına ilerledi; fark 68'in 200 altın ödülüyle uyumlu. Can5 ve yardım hakları aynı. Açılış sonrası lastGeneratedSeed353 yeni69. bölümle eşleşti (eski69 seed101); yeni Ay paletli veri cihazda yüklenmiş. Kayıt dosyası ve tercihler elle değiştirilmedi. Kanıt `Logs/moon-rollout-20261019/device-save-check.json`. Fiziksel görsel kabul kullanıcı kontrolüne açıktır.
