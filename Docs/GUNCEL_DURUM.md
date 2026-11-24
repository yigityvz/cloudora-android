# Cloudora — güncel ürün ve teknik durum

**24 Kasım 2026.** Oyun tabanı: `cloudora-ios/main@1ee4334c4591ddf969c14a549f394833af4ed2ef`. Android kaynak teslimi: `cloudora-android/cloudora-yigit`. Bu teslim oyun, test, Editor, varlık veya Unity ayarlarını değiştirmez; dokümantasyon ve taban doğrulama aracı ekler. `main`e birleştirme bu çalışmanın parçası değildir.

## Belgeler nasıl yorumlanır?

Güncel kullanıcı görevi → bu dosyada sabitlenmiş ürün kararları → gerçek kaynak kodu/ilgili testler → son tarihli konu belgesi → tarihsel belgeler sırasını izle. Kod ile belgede çatışma varsa bunu raporla; kodu otomatik olarak yeniden yazma. Kullanıcının yeni açık kararı elbette eski referansı değiştirebilir.

`PROJE_HAFIZASI`, `IOS_STATUS`, `ClaudoraRelease`, `crVersion` **kronolojik kayıtlardır**; içlerindeki eski bir paragraf tek başına güncel gereksinim değildir. Bazı eski tasarım belgeleri 13 dünya, 101+, 40 coin, Ay81, farklı dünya aralıkları veya kesintisiz sıvı şeridi anlatır. Bunlar geri getirilecek varsayılanlar değildir. `GELISTIRME_ONERILERI` bir öneri arşividir, otomatik yapılacaklar listesi değildir.

## Proje kimliği ve paket

- Unity **6000.5.9f1**; uGUI ve mevcut URP/paket kilidi. Başlangıç sahnesi `Assets/_Cloudora/Scenes/02_Gameplay.unity`; prefab `Assets/_Cloudora/Prefabs/CloudContainer.prefab`.
- Kaynaklar ortak Unity C# kodudur. Mevcut derleme aracı **iOS** içindir: iPhone, iOS 15+, dikey, IL2CPP ARM64, Metal; bundle `com.yigityvz.cloudora`.
- Android reposuna aynı kaynakları koymak hazır Android APK veya Android performans doğrulaması değildir. Android geliştiren kişi Unity Hub'da Android Build Support/SDK/NDK/JDK kurup Android Build Profile'a geçmelidir. Bu kaynak tesliminde Android ayarı, manifesti veya build betiği değiştirilmedi.
- `Assets/Plugins/iOS/CloudoraAccessibility.mm` yalnız iOS klasöründedir; çağrı `UNITY_IOS && !UNITY_EDITOR` ile korunur. Android için iOS native kodunu veya başka platformun imza ayarını taşımaya çalışma.
- APK v4.5.0'da görülen `App/Board/GameScreen/levels` mimarisi bu taban değildir. Burada `PuzzleController/PuzzleRules/V3LevelData` mimarisi korunur.

## Yayımlanan kampanya

| Alan | Güncel durum | Yetkili kaynak |
|---|---|---|
| Oynanabilir sınır | 1–100; 101 yüklenmez | `WorldCatalog`, `ProgressionManager` |
| Tanıtım / dünyalar | 1–14 / 15–30 / 31–60 / 61–100 | `WorldCatalog` |
| Zorluk döngüsü | Orta, Orta, Zor, Orta, Çok Zor; her 5 bölümde | `V3LevelPlan.BandFor` |
| Kapasite | 1–18: 4; 19–100: 5 | `V3LevelData.Generated.cs` |
| Aktif palet | Önce Güneş/Yağmur; Kar6, Rüzgâr31, Ay60; en çok 5 tür | yayımlanmış tahta verisi |
| Ay | 60–100; mevcut dört türe ek beşinci tür | `CampaignMoonRollout`, `MoonRolloutTests` |
| Tanımlı diğer türler | Şimşek/Gökkuşağı dahil enum ve 7 efekt mevcut; mevcut 100 tahtada aktif açılımı yok | `WeatherType`, `WeatherMotionLibrary` |
| Bölüm yükleme | 1–15 `AuthoredLevelCatalog`; kalan yayımlanmış veri `TryGetBuilt`; legacy üretici fallback kodu bulunur | `PuzzleController.LoadLevel` |

Bölüm numarasının, dünyanın ve editör kalibrasyon parçasının sınırlarını eşitleme. `V3Part.Intro` 1–5, Adaptation6–15 gibi ölçüm parçalarıdır; ürünün dünyasız tanıtımı 1–14'tür. `WorldDefinition.Capacity` gibi eski meta bilgisi yerine gerçek bölümün `LevelDefinition.capacity`/tablo girdisi esas alınır.

## Üretim ve çözücü — hassas alan

`DeterministicRandom` → `LevelCandidateFactory` → `LevelCandidateSelector` → `V3LevelBuilder` / `V3LadderBuilder` / `V3ThemeBuilder` → **`CampaignMoonRollout` yayımlama katmanı** → `V3LevelTool` → `V3LevelData.Generated.cs` → runtime `AuthoredLevelCatalog`.

Ay60 değişikliği yalnız 60–80'in 21 tahtasını yeniden seçmiştir. Tarihsel temel üreticiler, RNG ve puan formülü korunmuştur. Eski tabloları bir Editor menüsüne yanlışlıkla basarak yeniden üretme. Oluşan C# tablo elle yamalanmaz. Final60 14 hamledir; puan toleransı ve eski boş hazne/tuzak planı `IOS_MOON_FROM_60.md` içinde.

**İki ölçüm rejimi var:** Tarihsel kalibrasyon bazı çağrılarda açıkça `lockCompleted:false` kullanır; çalışma anındaki hamle/arama bitmiş hazne kilidiyle çalışır. Bunları tek parametreye indirgemek eski puan/seed seçimlerini bozabilir. `PuzzleSolver.Solve` durum limiti sonucunu çözümsüzlük kanıtı sayma; `ReachabilitySearch` karelere bölünerek devam eder.

## Hamle, bitiş ve çıkmaz

1. Kaynak seçilir; hedef, kapasite ve üst tür/modifier koşulları denetlenir. Üstte aynı türden bitişik grup kapasiteye sığdığı kadar taşınır.
2. `PuzzleController` giriş kilidi, hamle öncesi snapshot, `MoveAnimator`, modifier olayları ve kazanma/çıkmaz kontrollerini yönetir.
3. Bütün boş olmayan hazneler dolu ve hedef uyumluluğundaysa bölüm tamamlanır. Gökkuşağı uyumluluğu `CloudState`/`PuzzleRules`/çözücüde korunur; bunu yeni Prizma dönüşümüyle karıştırma.
4. Tamamlanan hazne normal kaynak ve Shuffle hedefi olamaz. Undo, geçmiş hamleyi geri alıp bitirişi iptal edebilir.
5. Legal hamle varken de çözüm yolu yok olabilir. Güncel kural `ReachabilitySearch` ile çözümsüzlük **kanıtı** aramaktır; yalnız `HasLegalMove` yaklaşımı geri getirilmez. Arama değişen tahta revizyonunda iptal edilir; sonuç önbelleği vardır.
6. Kayıp durumda `FailRecord` tahta, hamle sayısı ve Undo/modifier geçmişini saklar. Kapat-aç ücretsiz retry olmamalıdır.

## Can, haklar, coin ve kayıt

- Başlangıç **0 coin**, **5 can**, **3 Undo / 1 Ek Bulut / 1 Karıştır**. Can yenilenmesi UTC ile **30 dakika**, üst sınır5; eksiye düşmez.
- Ödül Orta **100**, Zor **200**, Çok Zor **300**. Aynı sonuç iki kez ödül üretmemeli. Ödül sonuç animasyonu beklenmeden kaydedilir.
- Ücretli devam **500**; yetersiz bakiye kesilmez. Kod, son çözülebilir snapshot'a geri gider; geçmişte yoksa ekstra hazneleri koruyarak başlangıca döner. **Bu, kendiliğinden yeni hazne satın almak değildir.** Metin/ürün beklentisi değiştirilecekse ayrı karar gerekir.
- Başarısızlıkta retry 1–5 için ücretsiz, sonra1 can. Bitmemiş bölümden **ana sayfaya çıkış** 1–5 dahil1 can; başlangıç tahtasını yükler. Kazanılmış bölümden ana sayfa ücretsizdir.
- Ek Bulut her kullanımda1 hazne; bölüm başına en çok2 ekstra. Limit hak/reklamdan önce denetlenir. Ek hazne ve başarılı Shuffle Undo geçmişini temizler.
- Karıştır **16**'da açılır; yalnız seçilen hazneyi farklı ve güvenli bir sıraya getirir; başarısız adayda hak kesilmez. İkinci tık/boş alan seçim modunu iptal eder.
- `SaveData.CurrentSchemaVersion=2`; `cloudora-save.json` + `.bak`/`.tmp` yönetimi `LocalJsonSaveService` içinde. Alan adlarını keyfi değiştirme. `safeShuffleCharges`, tutorialFlags15, failRecord, current/highest/seed ve tercihler önemlidir.
- `PlayerPrefs`: ses/titreşim ve `Cloudora.ReducedMotion` gibi ayarlar da bulunur. Normal oyun kaydının dışında olabildiğinden yalnız JSON yedeği tüm tercihleri temsil etmeyebilir.

## Görsel sözleşme

- Figma kaynaklı yaklaşık3 saniyelik açılış, ana sayfa/dünyalar/ayarlar/duraklatma/kazanma/kaybetme/100 final ekranları mevcut.
- Şişe **hareket etmez**. Üstten elemente özel uçuş, iz ve parçacık gider. Sadece içerideki aynı renk tek gövdede dalgalanıp yükselir/durulur; ikinci logo veya kutu basamağı yok.
- Hazne altı dekoratif bulut tabanı yok. Şişe içi monoline vektör ikonlar küçük ve dolu haznede18 tasarım puanı sınırındadır.
- Yedi tamamlanma sahnesi ~1,95sn; son sonuç için1,78sn bekleme; azaltılmış harekette kısaltılmış davranış. Ay: tutulma gölgesinden açılan kraterli dolunay ve ay ışığı/takımyıldız. Önceki hilal/yörünge denemesi güncel Ay bitirişi değildir.
- `IOS_ITEM_TRANSFER_RESTORE.md`, önceki `IOS_LIQUID_MOTION.md` içindeki **dışarıda kesintisiz sıvı şeridi** kararını düzeltir. Eski akışa geri dönme; diğer elementlerin bitirişlerini kaldırma.
- Canvas/nesne/mesh havuzları ve kaynak-UI değişim önbelleği korunur. Hedef60 FPS, Retina netlik ve tam kalite; bütün telefonlarda ölçülmüş60 FPS iddiası yoktur.
- Dünya kurtuluş yüzdesi gerçek tamamlanan bölümlerden hesaplanır (`RestorationManager`); kazanma sonrası ana sayfa seçeneği vardır.

## Servisler ve yayın

`IAdService`/`IAnalyticsService` arayüzleri bulunur. `CLOUDORA_DEVICE_QA` veya development editör testinde `FakeAdService` ödülü hemen simüle eder. Normal Release'de mevcut `AdMobAdService` kapalı bir adaptördür; gerçek reklam SDK'sı etkin sayılmaz. Analytics Firebase yoksa yerel debug fallback kullanır; kişisel veri anahtarları filtrelenir. Destek/gizlilik değerleri `Assets/Resources/CloudoraReleaseConfig.json` içindedir; gerçek iletişim/yayın bilgilerini varsayma.

## Kanıt düzeyi

Son kayıtlı tam iOS EditMode sonucu **241 toplam / 235 geçti / 6 atlama / 0 hata** (19 Ekim Ay60 görevi). İki atlama başsız grafik gereksinimiyle ilgilidir; diğerleri eski/manual kontrollerdir. İmzalı Xcode Release export ve iPhone SE kurulumu/açılışı kayıtlıdır (`IOS_MOON_FROM_60.md`). `Logs/` kanıt dosyaları yereldir ve klona dahil değildir; yeni ekip kendi testini üretmelidir.

24 Kasım kaynak eşlemesinde oyun değişmediği için bu sonuçlar **tarihsel referans** olarak aktarılır; bugün yeniden çalıştırılmış Android/iOS testleri gibi sunulmaz. Bugünkü doğrulama korunan kaynak/varlık/ayar hash eşliği, belge/link kapsamı ve LFS bütünlüğüdür. Fiziksel Android, tüm iPhone modelleri, App Store ve Google Play kabulü ayrı adımlardır.

## Bilinen açık noktalar — otomatik düzeltme görevi değildir

- Dünya tanıtımı1–14 olmasına rağmen authored öğretici/flag yapısı1–15 ve15. bölümün eski tanıtım metni durur. Bazı UI kodu15'te dünya adını gizler. Kaynak tesliminde değiştirilmedi.
- `WorldDefinition` eski Capacity/NewRule alanları ile yayımlanmış bölüm verisinin kapasitesini karıştırmamak gerekir; mevcut100'de6 kapasite yoktur.
- Ücretli devamın ekran metni ile geçmişte konuşulan "ekstra hazne" beklentisi ayrı doğrulanmalıdır; mevcut işlev `ResumeFromLastSolvable`'dır.
- Eski kuralla zorluk ölçümü, runtime kilit ve Ay60 seçimi arasındaki farklar belgelidir. "Refactor" adıyla bölüm algoritmasını kalibre etmeden yeniden yazma.
- Gerçek ses içerikleri/SDK/yayın durumu, tüm cihazlarda performans ve henüz otomatik olmayan görsel kabul için `SONRAKI_ADIMLAR.md` esas alınır.
