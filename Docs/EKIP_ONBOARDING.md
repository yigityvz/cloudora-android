# Yeni ekip üyesi ve Claude Code için öğrenme rotası

Bu dosyayı okumaya `../CLAUDE.md` ile başla. Amaç yalnızca oyunu açmak değil, **oyunun hangi sürümünü açtığını, neden böyle çalıştığını ve bir değişikliğin nerelere dokunduğunu** anlayarak devralmaktır. Referans: [GUNCEL_DURUM.md](GUNCEL_DURUM.md).

## Arkadaşına verilecek başlangıç mesajı

Repoyu indirdikten sonra Claude Code'u proje kökünde aç ve şu mesajı ver:

> Bu projeye yeni katıldım. Önce kökteki CLAUDE.md'yi, sonra onun verdiği sırayla Docs/GUNCEL_DURUM.md, Docs/EKIP_ONBOARDING.md ve Docs/DOSYA_OKUMA_HARITASI.md'yi oku. Oyunun güncel tabanı Yiğit'in son kararıyla cloudora-android/cloudora-bahadir@5a838a1; ekip main dalı bunun kaynak teslimidir. Eski iOS1ee4334 veya cloudora-yigit@cc04e5e yönergesini güncel taban sayma. Şimdilik oyun/ayar/paket/kayıt değiştirme ve bölüm üretme. Bütün runtime kodunu, Editor araçlarını, testleri, sahne/prefab/kaynak bağlarını ve belgeleri aşamalar halinde incele. Her aşamada öğrendiklerini bana sade Türkçeyle anlat; gerçek dosya ve test isimlerini göster. Eski belge, güncel kural, test kanıtı ve öneriyi ayır. Okuyamadığın yeri veya çelişkiyi açık yaz. Sonunda mimariyi, oynanışı, bölüm üretimini, ekonomiyi, kaydı, görsel sözleşmeyi, korunacak dosyaları, açık işleri ve güvenli geliştirme/test akışını anlatan bir devir özeti ver. Bir sorum olursa bu bağlam içinde cevapla. Ancak bundan sonra vereceğim geliştirme görevine geç.

Bu bir "öğren ve açıkla" görevidir; Claude Code'un kendi kendine refactor, tüm paketleri güncelleme veya diğer dalı merge etmesi istenmez.

## 0. Doğru kaynakları indir

Yeni çalışma klasörü kullan; başka bir oyunun `Assets` klasörünün üzerine kopyalama.

```sh
git clone --branch main --single-branch https://github.com/yigityvz/cloudora-android.git cloudora
cd cloudora
git lfs install --local
git lfs pull
git lfs fsck
git status --short
git branch --show-current
git log -3 --oneline
python3 Tools/verify_reference_snapshot.py
```

Git, Git LFS ve Python 3.9+ gerekir. Windows'ta `python3` yerine `py -3` kullanılabilir. GitHub ZIP indirmesi `.git` geçmişi ve LFS çözümü bakımından aynı garanti değildir; başlangıç için klon önerilir. Tüm klasörler, `.meta` dosyaları, `Packages` ve `ProjectSettings` birlikte kalmalıdır. `Library`/`Logs`/`Builds` klonda bulunmaması normaldir; Unity bunları üretir. LFS yerine `version https://git-lfs...` metni görüyorsan görseller henüz inmemiştir.

**Bu aşamada ekibe anlat:** Hangi repo/dal/commit'teyiz? Güncel gameplay neden Bahadır'ın v4.1 dalından geliyor? Eski iOS sürümü, main kaynak teslimi ve tarihsel Android QA APK'sı arasındaki fark ne? Bu aktarımda yeni cihaz testi yapıldı mı?

## 1. Kuralları ve belge önceliğini öğren

Sırayla oku:

1. [GUNCEL_DURUM.md](GUNCEL_DURUM.md) — güncel ürün/teknik sözleşme.
2. [PROJE_HAFIZASI.md](PROJE_HAFIZASI.md) — son kararlara kadar kronoloji; baştaki eski maddeleri tek başına güncel kabul etme.
3. [V41_TASARIM.md](V41_TASARIM.md), [V41_BOLUM_HARITASI.html](V41_BOLUM_HARITASI.html), ardından iOS mimari/konu belgeleri — v4.1 kuralları ve önceki görsel altyapı.
4. [DAL_KARSILASTIRMASI_2026-11-24.md](DAL_KARSILASTIRMASI_2026-11-24.md) — önceki iOS tabanı ile v4.1 farkları; baştaki yeni main kararı esas.

**Anlat:** Oyunun amacı hava türlerini ayırarak dünyaları canlandırmak. Dikey, tek parmak, kaynak ve hedefe dokunmalı bir sıralama bulmacası. Güncel kampanya100 bölüm/3 dünya; yeni APK'daki260 bölüm/26 bölge bu sürümün parçası değil. Kullanıcı kararı ile deneme/öneri ayrımı ve bir belgedeki eski bilginin neden geri dönüş talimatı olmadığı.

## 2. Projeyi aç; giriş ve sahne bağlarını izle

Önce `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/EditorBuildSettings.asset` oku. Unity Hub'da **6000.5.9f1** ile proje kökünü aç; bu aşamada paketleri yükseltme.

Sonra şu sırayı takip et:

1. `Assets/_Cloudora/Scenes/02_Gameplay.unity`: BoardRoot, Canvas, PuzzleController, prefab referanslarını bul. GUID'leri `.meta` ile eşleştir.
2. `Assets/_Cloudora/Prefabs/CloudContainer.prefab` ve `.meta`: görünüm bileşeni ve input referansları.
3. `Scripts/Core/MobileRuntimeBootstrap.cs`:60 FPS hedefi, vSync, kök Canvas koruması.
4. `Scripts/Puzzle/PuzzleController.cs` içinde `Start`, `LoadLevel`, `LoadState`, pause/quit ve save bağlantıları.
5. `Scripts/UI/OpeningSplash.cs` ile `MainMenuController.cs`: başlangıç animasyonu ve ana menü.
6. `Scripts/UI/SafeAreaFitter.cs`, `AdaptiveBoardLayout.cs`, `DesignUI.cs`: güvenli alan, ölçek, font ve programatik UI kurulumu.

Burada ve aşağıda `Scripts/...` yolları `Assets/_Cloudora/Scripts/...` köküne göredir. [Dosya haritası](DOSYA_OKUMA_HARITASI.md) bütün tam yolları içerir.

**Anlat:** Sahnedeki serialize alan ile runtime oluşturulan arayüz arasındaki fark; oyunun hangi kodla başladığı; neden yalnız `Assets` indirmenin yetmediği; neden iOS native plugin Android derlemesine koşulsuz taşınmadığı.

## 3. Saf kuralları öğren; sonra görünümle karşılaştır

Sıra: `Puzzle/WeatherType` → `Core/Move/CloudState/PuzzleState/PuzzleRules` → `Puzzle/CloudContainerView` → bütün `Modifiers/` → `PuzzleController` (bütün dosya) → `Level/PuzzleSolver/CloudShuffler`.

Özellikle `HandleCloudClicked`, `PerformMove`, `HasLegalMove`, `CheckWin`, Undo, fail/restore ve input kilidi ilişkisini izle. **v4.1 fail = legal hamle kalmaması**, çözümsüzlük araması değildir. `ReachabilitySearch` kaynakta kalır ama otomatik fail için çağrılmaz; `CheckSolvable`, Shuffle ve kurtarmanın arama kullanımı ayrıdır. Tamamlanmış hazne kilidi bütün katmanlarda korunur.

**Testler:** `PuzzleFeelTests`, `ReachabilityRegressionTests`, `V31GameplayTests`, `V32NoMovesTests`, `V32InputTests`, `CompletionEconomyTests`, `MoveFlowTests`.

**Anlat:** Legal hamle ile çözüm yolu neden farklı? v4.1 neden hamle varken panel açmıyor? Yardımcılar nasıl kurtarır? Tamamlanmış hazne neden kaynak olamaz ama Undo geri çevirebilir? Arama limitini neden matematiksel çözümsüzlük saymıyoruz? Modifier/input/model/animasyon nasıl senkron kalır?

## 4. Bölüm verisi ve üreticiyi ayrı katmanlar olarak oku

1. Önce `Docs/V41_TASARIM.md` tamamı ve `Docs/V41_BOLUM_HARITASI.html`: band/part, tuzak sınıfı, nefes, No Moves derinliği, hedef/ölçülen farkı.
2. `Level/LevelDefinition`, `BoardFrame`, `V3BoardText`, `V3LevelData` ve **`V3LevelData.Generated.cs`100 kaydın tamamı**. Kapasite4/5/6 ve Ay61 palet sınırlarını doğrula.
3. `AuthoredLevelCatalog`, `V3LevelPlan`: ilk15 öğreticiler, Shuffle15, dünya sınırları ve tablo erişimi. V3 adları güncel v4.1 verisini engellemez.
4. `DeterministicRandom`, `BalanceProfile`, `DifficultyCalculator`, `LevelGenerator`, `PuzzleValidator`: legacy/fallback kapsamını öğren;101+ hâlen kapalı.
5. `Editor/LevelCandidateFactory/Selector` → **`Editor/V41LevelBuilder` bütün metotlar**: aday havuzu, TrapPlan/CycleAssignments, DeepEnough, nefes16/31/61/81, No Moves≥4,100 finali, tier/gevşetme.
6. `PuzzleSolver`: kilitli ölçüm, puan ağırlıkları (NaiveFailureWeight15), düşünmeden/2 ileri/3 ileri sanal oyuncular ve wander ayrımı.
7. `Editor/V3LevelTool`: salt rapor/preview/inventory ile veri üretimi komutları ayrıdır. Öğrenirken Generate çalıştırma. `ContentValidationTool` eski10.000 stres testidir.

`CampaignMoonRollout/V3LevelBuilder/V3LadderBuilder/V3ThemeBuilder` ve eski builder testlerinin bir bölümü bu dalda yoktur; iOS'tan geri getirme. Güncel testler: `V3LevelBuildTests`, `LevelToolTests`, `V32SolverLoadTests`, `CampaignReleaseTests`; haritadaki bütün diğer test/probe dosyalarını da oku.

**Anlat:** Yayımlanan veri neden her açılışta rastgele üretilmez? Determinizm ve100 kayıt nasıl korunur?100 finalin hedef44, ölçülen42.5 olması ne demek? Düşünen oyuncu ölçüleri insan zorluğu garantisi midir? Gevşetilen6 bölüm hangileri? V3 belgelerini neden güncel veri sanmıyoruz?

## 5. Ekonomi, kayıt ve kurtarma akışını uçtan uca izle

Sıra:

1. `Progression/WorldDefinition.cs`, `WorldCatalog.cs`, `ProgressionManager.cs`, `RestorationManager.cs`.
2. `Progression/LifeManager.cs`, `CoinEconomy.cs`, `BoosterManager.cs`.
3. `Services/ISaveService.cs`, `SaveData.cs`, `LocalJsonSaveService.cs`, `FailRecord.cs`.
4. `Level/CloudShuffler.cs`.
5. `PuzzleController` içindeki `CurrentFailOptions`, `RetryAfterFail`, `ContinueWithCoins`, `ContinueWithAd`, `ResumeFromLastSolvable`, `ReturnHome`, `UseExtraCloud`, `UseSafeShuffle`, `ShuffleCloud`, `SaveProgress`, `RestoreFailState`.

**Testler:** `AbandonAttemptTests`, `MainMenuLifeTests`, `CompletionEconomyTests`, `CampaignReleaseTests`, `V31GameplayTests`, `V32InputTests`, `V32NoMovesTests`.

**Anlat:** Başarısızlık retry'si ile ana sayfaya çıkışın can ücretleri farklıdır. 500 coin devam, son çözülebilir duruma dönüş yapar; ekstra hazne düğmesi ayrı bir işlevdir. Shuffle yalnız1 hazneye uygulanır,15'te açılır; yanlış/başarısız seçim hak götürmez. UTC can yenilenmesi ve sınırlara clamp; kayıp ekranını kapat-aç ile atlatmama; `.bak` kurtarması; eski JSON alanlarını koruma; test kaydı ile oyuncu kaydını ayırma.

## 6. Görsel dili ve kabul edilmiş animasyonu oku

Önce güncel sözleşme belgelerini bu sırayla oku:

1. `IOS_DESIGN_IMPLEMENTATION.md` (ilk ekran aktarımı; eski ekonomi/dünya ifadelerini tarihsel say). Önce `V41_TASARIM.md` §2'deki UI düzeltmelerini oku.
2. `IOS_QUALITY_CAMPAIGN.md` → `IOS_UX_REVIEW.md` (görsel geçmiş; otomatik çözümsüzlük kuralı v4.1'de değişmiştir).
3. `IOS_ELEMENT_MOTION.md` → `IOS_WEATHER_SYMBOLS.md`.
4. `IOS_COMPLETION_EFFECTS.md` (yedi bitiriş; içeride ilk/ikinci tasarım ayrımı).
5. `IOS_LIQUID_MOTION.md` **son karar değil** → mutlaka **`IOS_ITEM_TRANSFER_RESTORE.md`** (üstten uçuşun geri getirilmesi ve yalnız iç dolumun sıvı olması).
6. `IOS_PERFORMANCE.md`, `IOS_OPENING_SPLASH.md`.

Kod sırası:

- `UI/DesignUI.cs`, `VectorShapes.cs`, `WeatherGlyph.cs`, `LiquidFillGraphic.cs`.
- `Puzzle/CloudContainerView.cs`, `WeatherMotionLibrary.cs`, `MoveAnimator.cs`.
- `UI/VesselCompletionFlourish.cs`: her elementin sahnesi ve son Ay tasarımı.
- `UI/WorldAtmosphere.cs`, `WorldRescueReplay.cs`, `MilestoneScreen.cs`.
- `UI/GameplayOverlay.cs`, `MainMenuController.cs`, `WorldScreenController.cs`, `CloudoraVisuals.cs`, `SafeAreaFitter.cs`, `AdaptiveBoardLayout.cs`, `OpeningSplash.cs`.
- `Assets/Plugins/iOS/CloudoraAccessibility.mm`; `Resources/Splash`, `Resources/Design` görsel/fontları, import `.meta`ları ve `Docs/DesignSources/Nunito-OFL.txt`.
- `Editor/DesignQuality.cs`, `DesignPreview.cs`, `OpeningSplashPreview.cs`, `WeatherSymbolPreview.cs`, `RenderingPerformanceBenchmark.cs` — Editor önizlemesi ile cihaz kanıtını ayır.

**Testler:** `OpeningSplashTests`, `DesignNavigationTests`, `ElementMotionTests`, `LiquidMotionTests`, `CompletionFlourishTests`, `MoveFlowTests`, `PointerMotionVisualTests`, `RenderingReuseTests`, `V32LayoutTests`, `V32VisualTests`.

**Anlat:** Sabit cam, üstten giren element ve içeride yükselen sıvı üç ayrı katmandır. Aynı renkte ikinci kutu/logo neden yok? Tamamlanma efekti ile aktarım efekti farklıdır. Son sonuç neden beklerken ödül hemen kaydedilir? Hareketi Azalt neden efektlerin daha kısa görünmesine neden olabilir? Kaliteyi düşürmeden havuz/Canvas/mesh tekrar kullanımını nasıl koruruz? Tek Editor GC ölçümünü bütün telefonlara60 FPS garantisi diye sunma.

## 7. Servis ve yayın sınırlarını öğren

- `Services/IAdService`, `FakeAdService`, `AdMobAdService`, `AdPlacementPolicy`.
- `Services/IAnalyticsService`, `DebugAnalyticsService`, `FirebaseAnalyticsService`, `AnalyticsEvents` ve privacy doğrulaması.
- `Services/GameFeedbackService`, `ReleaseConfiguration`; `Assets/Resources/CloudoraReleaseConfig.json`.
- `Docs/RELEASE_CHECKLIST.md`, `Docs/PRIVACY_RELEASE_REVIEW.md`, `StoreAssets/README.md`, `StoreAssets/STORE_COPY.md`.

**Anlat:** FakeAd gerçek reklam değildir; normal Release adaptörü şu anda kapalıdır. Kaynakta arayüz bulunması AdMob/Firebase SDK kuruldu demek değildir. Kişisel veri, destek/gizlilik adresi, mağaza görseli ve imza için varsayımlarla yayın yapılmaz. Eski mağaza metni ve ikonlar çalışma artefaktıdır; üretim onayı değildir.

## 8. Doğru test ve çalıştırma biçimi

### Unity Editor (her ekip üyesinin ilk doğrulaması)

Ana sahneyi aç → Console derleme hatası yok → Play Mode → açılış/ana menü → bir geçerli ve geçersiz hamle → Undo → duraklatma → menü → kayıt davranışı. EditMode için Window/Test Runner'ı aç; `Tests/Editor` kapsamını çalıştır. Grafik gerektiren testler için grafik açık oturum gerekir. Başsız/manual/Explicit atlamaları açık kaydet.

### iOS (macOS + Xcode)

`Docs/IOS_BUILD.md` ve `Build-iOS.sh` oku; aynı projeyi açık tutan Unity oturumunu kapattıktan sonra:

```sh
./Build-iOS.sh test
# Değişikliğin kapsamı gerektiriyorsa, ayrı aşama:
./Build-iOS.sh build
```

Unity export, native Xcode compile, imzalama, cihaz kurulumu, cihazda açılış ve fiziksel oynanış altı ayrı kanıttır. Yerel Team/sertifika/cihaz yedeği Git'e konmaz. Mevcut oyuncu kaydı korunarak kurulur. İmzalı çıktı üretilmeden "telefonda test edildi" denmez.

### Android — güncel ana platform

`Docs/ANDROID_BUILD.md`, `GOOGLE_PLAY_SUBMISSION.md`, `Build-Android.ps1`, `Editor/CloudoraBuildTools.cs`, `CloudoraAndroidManifestPostprocessor.cs`, Android Build Profile ve platform `.meta` override'larını oku. Wrapper Windows içindir; ASCII/boşluksuz proje yolu ve geçici TEMP/TMP yönetimi vardır. Mac'te aynı PowerShell komutunu varsayımla çalıştırma; Unity menüsünü/modülleri incele.

- Unity6000.5.9f1 + Android Build Support/SDK/NDK/OpenJDK kur. Ana sahneyi aç, Console/Play Mode/EditMode testleriyle başla.
- Dahili cihaz testi için `Cloudora/Build/Android Device QA APK`; batch örneği Windows'ta `./Build-Android.ps1 -Mode qa -ProjectPath C:\Dev\Cloudora`.
- QA `CLOUDORA_DEVICE_QA` fake ödülleridir; gerçek reklam/üretim imzası/Play yayını değildir. Yan yana test paketi kimliği/output env değişkenlerini bilinçli seç; mevcut kullanıcı kaydını ezme.
- v4.1 tarihsel test sonucu197 toplam/194 geçti/3 atlama/0 hata. Bunu yeni makinede çalıştırılmış sonuç sayma. Builder testleri büyük havuz nedeniyle dakikalar sürer.
- APK build, kurulum, launch, gerçek oynama, background/foreground ve kayıt yenileme ayrı doğrulamalardır. Bu main kaynak aktarımında yeni APK/iPhone kurulumu yapılmaz.

## 9. Tüm dosyaların okunduğunu görünür hale getir

[Dosya haritası](DOSYA_OKUMA_HARITASI.md) içindeki **her C# satırını** bir okuma iş listesine al. Her dosya için: sorumluluk, giriş/çıkış, state yan etkisi, kritik bağımlılık, ilgili test ve değişiklik riski yaz. Generated tabloda100 kaydı, sahne/prefabda GUID bağlarını, görsel/fontta LFS ve importer ayarlarını kontrol et. Salt sınıf adına bakıp dosyayı okundu sayma.

Ayrıca bütün metin belgelerini "güncel sözleşme / konu kanıtı / tarihsel ölçüm / öneri / mağaza taslağı" olarak sınıflandır. Tasarım SVG/PNG'lerini satır satır kod gibi okumak yerine hangi UI'da kullanıldığını, boyut/import/font lisansını kontrol et; `.meta`ları atma.

Claude Code bağlamı yetmezse, inceleme ilerlemesini **hangi dosya/bölüme kadar okunduğu** ile özetle ve kalanlardan devam et. Her şeyin okunduğunu kanıtlamadan tamlık iddiası kurma. İlk öğrenme turunda kaynakta değişiklik yapma; ekip üyesine vereceğin okuma özeti yeterlidir.

## 10. Devir sonunda cevaplanacak sorular

- [ ] Güncel taban neden `cloudora-bahadir@5a838a1`? GitHub repo, dal ve oyun sürümü ne fark taşır?
- [ ] İlk sahneden ana menü/oyna/sonuç/kayıt akışı nasıl ilerler?
- [ ]100 sınırı nerede, dünyaların aralıkları ne, tanıtım neden14'te bitiyor?
- [ ] Yayımlanan tablo, legacy üretici ve Editor yayımlama aracı neden üç ayrı şey?
- [ ] Ay61/Shuffle15/kapasite6 sınırları nerede ve hangi testler koruyor?
- [ ] Legal hamle ile çözülebilirlik arasındaki fark ne; v4.1 hangi durumda otomatik kayıp gösterir?
- [ ] Bitmiş hazne kilidi çekirdek, UI, Shuffle ve çözücüde nasıl korunuyor?
- [ ]100/200/300 ve500 nerede; çift ödül/kesinti nasıl engelleniyor?
- [ ] Retry muafiyeti ile bitmemiş bölümden eve çıkış neden aynı can kuralı değil?
- [ ] Ek Bulut/Shuffle geçmişi, hata kaydı ve son güvenli duruma dönüş nasıl ilişkili?
- [ ] Sıvı içerik, element uçuşu ve tamamlanma efekti hangi dosyalarda?
- [ ] Sistemin ve oyunun azaltılmış hareket tercihleri nasıl okunuyor?
- [ ] Hangi test gerçekten çalıştı, hangisi atlandı, hangisi yalnızca tarihsel?
- [ ] Hangi belgelerde eski kararı bugünkü kural sanma riski var?
- [ ] V4.1 zaten main tabanıyken hangi iOS davranışları tarihsel; Atlas/coin önerileri neden uygulanmış sayılmaz?
- [ ] Yeni görevde hangi dosyalara dokunmak gerekir; hangileri kapsam dışı kalmalı?

## 11. İlk geliştirme görevi geldiğinde

İlgili dosya ve testleri tekrar aç → etkiyi ve sınırı kısaca anlat → yetkilendirilmiş işi uygula → anlamlı test/görsel/platform kontrolü → kod/varlık diff'ini gözden geçir → güncel durum/hafıza/değişiklik kaydı → istenen dala commit/push. Sırf test sonucu daha kolay yeşil olsun diye beklenen değerleri, seed'leri veya korunan manifesto özetlerini değiştirme.

Bütün `main`lere, başka dalın algoritmasına, gerçek telefondaki ilerlemeye, imzalara veya SDK'lara geniş bir izin verilmiş varsayma. Kullanıcının açık yeni görevini ise gereksiz izin döngüsüne sokmadan tamamla.
