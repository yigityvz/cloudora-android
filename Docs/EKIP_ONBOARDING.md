# Yeni ekip üyesi ve Claude Code için öğrenme rotası

Bu dosyayı okumaya `../CLAUDE.md` ile başla. Amaç yalnızca oyunu açmak değil, **oyunun hangi sürümünü açtığını, neden böyle çalıştığını ve bir değişikliğin nerelere dokunduğunu** anlayarak devralmaktır. Referans: [GUNCEL_DURUM.md](GUNCEL_DURUM.md).

## Arkadaşına verilecek başlangıç mesajı

Repoyu indirdikten sonra Claude Code'u proje kökünde aç ve şu mesajı ver:

> Bu projeye yeni katıldım. Önce kökteki CLAUDE.md'yi, sonra onun verdiği sırayla Docs/GUNCEL_DURUM.md, Docs/EKIP_ONBOARDING.md ve Docs/DOSYA_OKUMA_HARITASI.md'yi oku. Oyunun yetkili tabanı Yiğit'in telefona kurulan iOS 1ee4334 sürümü; cloudora-android/cloudora-yigit bunun ortak Unity kaynak teslimidir. Başka dalı veya daha yüksek numaralı APK'yı taban seçme. Şimdilik oyun/ayar/paket/kayıt değiştirme ve bölüm üretme. Bütün runtime kodunu, Editor araçlarını, testleri, sahne/prefab/kaynak bağlarını ve belgeleri aşamalar halinde incele. Her aşamada öğrendiklerini bana sade Türkçeyle anlat; gerçek dosya ve test isimlerini göster. Eski belge, güncel kural, test kanıtı ve öneriyi ayır. Okuyamadığın yeri veya çelişkiyi açık yaz. Sonunda mimariyi, oynanışı, bölüm üretimini, ekonomiyi, kaydı, görsel sözleşmeyi, korunacak dosyaları, açık işleri ve güvenli geliştirme/test akışını anlatan bir devir özeti ver. Bir sorum olursa bu bağlam içinde cevapla. Ancak bundan sonra vereceğim geliştirme görevine geç.

Bu bir "öğren ve açıkla" görevidir; Claude Code'un kendi kendine refactor, tüm paketleri güncelleme veya diğer dalı merge etmesi istenmez.

## 0. Doğru kaynakları indir

Yeni çalışma klasörü kullan; başka bir oyunun `Assets` klasörünün üzerine kopyalama.

```sh
git clone --branch cloudora-yigit --single-branch https://github.com/yigityvz/cloudora-android.git cloudora-yigit
cd cloudora-yigit
git lfs install --local
git lfs pull
git lfs fsck
git status --short
git branch --show-current
git log -3 --oneline
python3 Tools/verify_reference_snapshot.py
```

Git, Git LFS ve Python 3.9+ gerekir. Windows'ta `python3` yerine `py -3` kullanılabilir. GitHub ZIP indirmesi `.git` geçmişi ve LFS çözümü bakımından aynı garanti değildir; başlangıç için klon önerilir. Tüm klasörler, `.meta` dosyaları, `Packages` ve `ProjectSettings` birlikte kalmalıdır. `Library`/`Logs`/`Builds` klonda bulunmaması normaldir; Unity bunları üretir. LFS yerine `version https://git-lfs...` metni görüyorsan görseller henüz inmemiştir.

**Bu aşamada ekibe anlat:** Hangi repo/dal/commit'teyiz? Güncel gameplay nereden geliyor? Neden Android repo adı tek başına başka Android sürümünü seçme gerekçesi değil? Ortak kaynak, doğrulanmış iOS derlemesi ve henüz doğrulanmamış Android APK arasındaki fark ne?

## 1. Kuralları ve belge önceliğini öğren

Sırayla oku:

1. [GUNCEL_DURUM.md](GUNCEL_DURUM.md) — güncel ürün/teknik sözleşme.
2. [PROJE_HAFIZASI.md](PROJE_HAFIZASI.md) — son kararlara kadar kronoloji; baştaki eski maddeleri tek başına güncel kabul etme.
3. [IOS_ARCHITECTURE.md](IOS_ARCHITECTURE.md), [IOS_STATUS.md](IOS_STATUS.md) ve [IOS_DEVICE_QA.md](IOS_DEVICE_QA.md) — mimari, hangi tarihte hangi doğrulama yapıldı ve hangi kabul testleri açık.
4. [DAL_KARSILASTIRMASI_2026-11-24.md](DAL_KARSILASTIRMASI_2026-11-24.md) — diğer dallar ve özellikle taşınmayacak davranış farklılıkları.

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

Sıra:

1. `Puzzle/WeatherType.cs` — enum sırası da verinin parçasıdır.
2. `Core/Move.cs`, `CloudState.cs`, `PuzzleState.cs`, `PuzzleRules.cs` — kapasite, üst grup, aktarım, klonlama, bitmiş hazne ve kazanma uyumluluğu.
3. `Puzzle/CloudContainerView.cs` — aynı davranışın Unity tarafı, seçim/dolum/restore ve görünüm önizlemesi.
4. `Modifiers/ModifierType.cs`, `ModifierData.cs`, `ICloudModifierRule.cs`, `CloudModifierRules.cs`, `ModifierRuntime.cs` — korunan hazneler, sayaçlar, gizleme, hareket kısıtları ve snapshot.
5. `Puzzle/PuzzleController.cs` dosyasının geri kalanı: `HandleCloudClicked`, `PerformMove`, `CheckWin`, `Undo`, `BeginSolvabilityCheck`, `CheckCurrentBoard`, iptal/revizyon mantığı.
6. `Level/PuzzleSolver.cs`: çözüm/durum limiti, kanonik anahtar, hamle üretimi, tamamlanmış hazne kilidi, karelere bölünmüş erişilebilirlik, `FailRecovery`.

**İlgili testler:** `PuzzleFeelTests`, `ReachabilityRegressionTests`, `V31GameplayTests`, `V32NoMovesTests`, `V32InputTests`, `CompletionEconomyTests`, `MoveFlowTests`.

**Anlat ve örnekle:** Bir legal hamle neden bütün bulmacanın çözülebilir olduğunu göstermez? Tamamlanan hazne neden kaynak olamaz, ama Undo önceki tamamlamayı geri çevirebilir? Arama limiti neden "kaybettin" sayılmaz? Animasyon ve modelin aynı anda iki hamle alması nasıl engellenir? Yeni modifier eklenirse yalnız görsele badge koymanın neden yeterli olmadığı.

## 4. Bölüm verisi ve üreticiyi ayrı katmanlar olarak oku

Önce veri okuma tarafı:

1. `Level/LevelDefinition.cs`, `BoardFrame.cs`, `V3BoardText.cs`, `V3LevelData.cs`.
2. `V3LevelData.Generated.cs` **100 kaydın tamamı**: bölüm/bant/kapasite/seed/score/çözüm uzunluğu/tahta. Elle yazma.
3. `AuthoredLevelCatalog.cs`, `V3LevelPlan.cs`: öğretici metni, ilk15 yüklemesi, güncel100 verisine erişim,5'li bant döngüsü.
4. `Level/DeterministicRandom.cs`, `BalanceProfile.cs`, `DifficultyCalculator.cs`, `LevelGenerator.cs`, `PuzzleValidator.cs`: legacy/fallback üretim ve doğrulama. Kodda bulunması101+'ın şu an açık olduğu anlamına gelmez.

Sonra Editor yayımlama tarafı:

5. `Editor/LevelCandidateFactory.cs`, `LevelCandidateSelector.cs`.
6. `Editor/V3LevelBuilder.cs`, `V3LadderBuilder.cs`, `V3ThemeBuilder.cs`.
7. `Editor/CampaignMoonRollout.cs` **en son**; tarihsel builder çıktısına Ay60 kuralını uygular.
8. `Editor/V3LevelTool.cs` — üretme, önizleme ve kalibrasyon komutlarını ayır. İnceleme için üretme menüsünü çalıştırma.
9. `Editor/ContentValidationTool.cs` — legacy10.000 doğrulamanın kapsamı; güncel100 tabloyu ve UI'ı tek başına doğruladığını sanma.

**Belgeler:** `V3_SABLON`, `V3_BOLUM_VE_ZORLUK_TASARIMI`, `V3_CERCEVE_ENVANTERI`, `V3_BOLUM_HARITASI.html` tarihsel tasarım/ölçüm; **güncel palet için `IOS_MOON_FROM_60`** ve tablo esas alınır.

**Testler:** `LevelToolTests`, `V3LevelBuildTests`, `V32LadderTests`, `V32ThemeTests`, `V32Theme3Tests`, `V32SolverLoadTests`, `CampaignReleaseTests`, `MoonRolloutTests`. Manuel probe ve grafik isteyen testleri haritadan ayrıca bul.

**Anlat:** Başlangıçta neden her seferinde rastgele yeni bulmaca üretilmediği; aynı seed'in önemi; published data ile builder'ın niçin ayrı olduğu; Ay60'ın dört elementi değiştirmeden nasıl beşinci olduğu; tarihsel `lockCompleted:false` kalibrasyonu ile runtime kilitli kuralların ayrımı. Bir bölümü "kolaylaştırmak" adına seed/puan/formül veya kapasiteyi kendi başına değiştirme.

## 5. Ekonomi, kayıt ve kurtarma akışını uçtan uca izle

Sıra:

1. `Progression/WorldDefinition.cs`, `WorldCatalog.cs`, `ProgressionManager.cs`, `RestorationManager.cs`.
2. `Progression/LifeManager.cs`, `CoinEconomy.cs`, `BoosterManager.cs`.
3. `Services/ISaveService.cs`, `SaveData.cs`, `LocalJsonSaveService.cs`, `FailRecord.cs`.
4. `Level/CloudShuffler.cs`.
5. `PuzzleController` içindeki `CurrentFailOptions`, `RetryAfterFail`, `ContinueWithCoins`, `ContinueWithAd`, `ResumeFromLastSolvable`, `ReturnHome`, `UseExtraCloud`, `UseSafeShuffle`, `ShuffleCloud`, `SaveProgress`, `RestoreFailState`.

**Testler:** `AbandonAttemptTests`, `MainMenuLifeTests`, `CompletionEconomyTests`, `CampaignReleaseTests`, `V31GameplayTests`, `V32InputTests`, `V32NoMovesTests`.

**Anlat:** Başarısızlık retry'si ile ana sayfaya çıkışın can ücretleri farklıdır. 500 coin devam, son çözülebilir duruma dönüş yapar; ekstra hazne düğmesi ayrı bir işlevdir. Shuffle yalnız1 hazneye uygulanır,16'da açılır; yanlış/başarısız seçim hak götürmez. UTC can yenilenmesi ve sınırlara clamp; kayıp ekranını kapat-aç ile atlatmama; `.bak` kurtarması; eski JSON alanlarını koruma; test kaydı ile oyuncu kaydını ayırma.

## 6. Görsel dili ve kabul edilmiş animasyonu oku

Önce güncel sözleşme belgelerini bu sırayla oku:

1. `IOS_DESIGN_IMPLEMENTATION.md` (ilk ekran aktarımı; eski ekonomi/dünya ifadelerini tarihsel say).
2. `IOS_QUALITY_CAMPAIGN.md` → `IOS_UX_REVIEW.md` (son dünya aralıkları ve çözümsüzlük).
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

### Android ekip üyesi

- Unity6000.5.9f1 için Android Build Support + SDK/NDK/OpenJDK kur.
- Proje kökünü aç, `02_Gameplay` sahnesini kullan; platformu Android'e geçir. iOS export menüsünü Android için çalıştırma.
- Ortak kodun Editor testini çalıştır. Android ayarını/manifestini değiştirmek gerekiyorsa bunu ayrı platform uyarlaması olarak incele; v4.1 oyun algoritmasını birlikte getirme.
- Bu teslimde Android CLI build betiği/profili eklenmedi ve APK üretilmedi. İstenen Android QA paketi için script/izin/imza/servis tanımı ve cihaz testi ayrı iş olarak kaydedilir.
- Başka platforma geçmek çalışma kopyasının ayarlarını değiştirebilir; bu değişiklikleri sırf Unity üretti diye otomatik commit etme.

## 9. Tüm dosyaların okunduğunu görünür hale getir

[Dosya haritası](DOSYA_OKUMA_HARITASI.md) içindeki **her C# satırını** bir okuma iş listesine al. Her dosya için: sorumluluk, giriş/çıkış, state yan etkisi, kritik bağımlılık, ilgili test ve değişiklik riski yaz. Generated tabloda100 kaydı, sahne/prefabda GUID bağlarını, görsel/fontta LFS ve importer ayarlarını kontrol et. Salt sınıf adına bakıp dosyayı okundu sayma.

Ayrıca bütün metin belgelerini "güncel sözleşme / konu kanıtı / tarihsel ölçüm / öneri / mağaza taslağı" olarak sınıflandır. Tasarım SVG/PNG'lerini satır satır kod gibi okumak yerine hangi UI'da kullanıldığını, boyut/import/font lisansını kontrol et; `.meta`ları atma.

Claude Code bağlamı yetmezse, inceleme ilerlemesini **hangi dosya/bölüme kadar okunduğu** ile özetle ve kalanlardan devam et. Her şeyin okunduğunu kanıtlamadan tamlık iddiası kurma. İlk öğrenme turunda kaynakta değişiklik yapma; ekip üyesine vereceğin okuma özeti yeterlidir.

## 10. Devir sonunda cevaplanacak sorular

- [ ] Güncel taban neden `1ee4334`? GitHub repo, dal ve oyun sürümü ne fark taşır?
- [ ] İlk sahneden ana menü/oyna/sonuç/kayıt akışı nasıl ilerler?
- [ ]100 sınırı nerede, dünyaların aralıkları ne, tanıtım neden14'te bitiyor?
- [ ] Yayımlanan tablo, legacy üretici ve Editor yayımlama aracı neden üç ayrı şey?
- [ ] Ay hangi bölümde ve kaçıncı element; onu koruyan test hangisi?
- [ ] Legal hamle ile çözülebilirlik arasındaki farkı örnekleyebilir misin?
- [ ] Bitmiş hazne kilidi çekirdek, UI, Shuffle ve çözücüde nasıl korunuyor?
- [ ]100/200/300 ve500 nerede; çift ödül/kesinti nasıl engelleniyor?
- [ ] Retry muafiyeti ile bitmemiş bölümden eve çıkış neden aynı can kuralı değil?
- [ ] Ek Bulut/Shuffle geçmişi, hata kaydı ve son güvenli duruma dönüş nasıl ilişkili?
- [ ] Sıvı içerik, element uçuşu ve tamamlanma efekti hangi dosyalarda?
- [ ] Sistemin ve oyunun azaltılmış hareket tercihleri nasıl okunuyor?
- [ ] Hangi test gerçekten çalıştı, hangisi atlandı, hangisi yalnızca tarihsel?
- [ ] Hangi belgelerde eski kararı bugünkü kural sanma riski var?
- [ ] Android v4.1'den hangi düzeltmeler aday, hangi davranışlar onay olmadan taşınamaz?
- [ ] Yeni görevde hangi dosyalara dokunmak gerekir; hangileri kapsam dışı kalmalı?

## 11. İlk geliştirme görevi geldiğinde

İlgili dosya ve testleri tekrar aç → etkiyi ve sınırı kısaca anlat → yetkilendirilmiş işi uygula → anlamlı test/görsel/platform kontrolü → kod/varlık diff'ini gözden geçir → güncel durum/hafıza/değişiklik kaydı → istenen dala commit/push. Sırf test sonucu daha kolay yeşil olsun diye beklenen değerleri, seed'leri veya korunan manifesto özetlerini değiştirme.

Bütün `main`lere, başka dalın algoritmasına, gerçek telefondaki ilerlemeye, imzalara veya SDK'lara geniş bir izin verilmiş varsayma. Kullanıcının açık yeni görevini ise gereksiz izin döngüsüne sokmadan tamamla.
