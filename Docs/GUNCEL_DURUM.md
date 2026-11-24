# Cloudora — güncel ürün ve teknik durum

**24 Kasım 2026: Yiğit'in son kararıyla güncel taban Bahadır'ın dalıdır.** Kaynak `yigityvz/cloudora-android/cloudora-bahadir@5a838a104cdce93e342c353c930288e08099ceab`; ekip için hedef **aynı reponun `main` dalı**. Bu aktarımda oyun/Editor/test/varlık/paket/ayar dosyaları değiştirilmez. Önceki `cloudora-yigit@cc04e5e` ve `cloudora-ios@1ee4334` iOS referansları artık Android main için zorunlu ürün tabanı değildir.

## Belge önceliği

Güncel kullanıcı görevi → bu dosya → gerçek kod ve ilgili testler → [V41_TASARIM](V41_TASARIM.md) ve [v4.1 görsel harita](V41_BOLUM_HARITASI.html) → tarihsel kayıtlar. Çelişkiyi bildir; başka sürümü kendiliğinden geri getirme. `PROJE_HAFIZASI`, `ClaudoraRelease`, `crVersion` kronolojidir. Eski iOS Ay60/Shuffle16/erken çözümsüzlük anlatımı ve V3'teki101+/13 dünya ifadeleri bugünkü ürün kuralı değildir.

`COIN_EKONOMISI.html` ve `TEMA_ATLASI.html` **fikir/karar sayfalarıdır**, uygulanmış market veya101–500 kampanyası değildir. Daha yüksek sürüm numaralı Efe APK'sı bu kaynak dalın yerine seçilmedi.

## Kimlik, başlangıç ve platform

- Unity **6000.5.9f1**, Universal2D/uGUI; mevcut `Packages/manifest.json` ve lock korunur.
- Ana sahne `Assets/_Cloudora/Scenes/02_Gameplay.unity`; prefab `Assets/_Cloudora/Prefabs/CloudContainer.prefab`. Ana orkestrasyon `PuzzleController.Start/LoadLevel`; UI büyük ölçüde kodla kurulur.
- Android kaynakları: `Build-Android.ps1`, `CloudoraBuildTools`, `CloudoraAndroidManifestPostprocessor` ve Android Build Profile. Android Build Support/SDK/NDK/OpenJDK gerekir. Windows wrapper ASCII ve boşluksuz proje yolu bekler.
- Android build aracı: portre, IL2CPP/ARM64, min API26/target36, `com.yigityvz.cloudora`, bundleVersion0.1.0. Bu paket sürümü, tasarım sürümü v4.1 ve commit birbirinden farklı kavramlardır. QA kimliği/output env değişkenleriyle ayrılabilir.
- iOS kaynak/araçları da dalda bulunur; native çağrı `UNITY_IOS && !UNITY_EDITOR` ile korunur. Bu Android aktarımı **iOS reposunu veya telefondaki kurulumu güncellemez**; yeni v4.1 iPhone uyarlaması/testi yapılmış sayılmaz.

## Güncel kampanya

| Alan | Mevcut davranış | Kaynak |
|---|---|---|
| Sınır | 1–100;101+ oynanamaz | WorldCatalog/ProgressionManager |
| Dünyalar | tanıtım1–14; Yeşil Vadi15–30; Rainy Coast31–60; Frost Peaks61–100 | WorldCatalog |
| Bant | her5 bölüm Orta, Orta, Zor, Orta, Çok Zor | V3LevelPlan |
| Kapasite | 1–15:4;16–80:5;81–100:6 | V41LevelBuilder/yayımlanan tablo |
| Element | Güneş/Yağmur; Kar6, Rüzgâr31, **Ay61** | V3LevelData.Generated.cs |
| Karıştır | **15'te** açılır | V3LevelPlan.ShuffleUnlockLevel |
| Diğer türler | Şimşek/Gökkuşağı enum ve efektleri var; bu100 bölümün paletinde yok | WeatherType/tablo |
| İlk yükleme | 1–15 AuthoredLevelCatalog;16–100 yayımlanmış veriler | PuzzleController.LoadLevel |

Kapasite, dünyaların eski metadata alanından değil gerçek `LevelDefinition`/tablodan okunur. İlk15 authored kayıt, dünyasız tanıtımın14'te bitmesiyle aynı sınır değildir. Yedi efekt olması bütün elementlerin kampanyada açıldığı anlamına gelmez.

## Hamle, kazanma ve kaybetme

- Kaynak ve hedefe dokunulur. Üstteki aynı tür grup, hedef boş veya üstü uyumluysa kapasiteye sığdığı kadar taşınır. Modifier ve input/animasyon kilitleri de denetlenir.
- Tam dolu/uyumlu tamamlanmış hazne normal aktarma ve Karıştır için kilitlidir. Undo önceki hamleyi geri çevirerek tamamlanmayı kaldırabilir.
- Bütün boş olmayan hazneler tamamlandığında kazanılır; sonuç beklenmeden ödül kaydedilir, bitiriş efektinin ardından panel açılır.
- **v4.1'de kayıp yalnızca legal hamle kalmayınca gerçekleşir.** `PuzzleController.PerformMove` → `HasLegalMove` → `EnterFailState`, analytics `reason=no_moves`. Legal hamle olup çözüm yolu yoksa otomatik kayıp paneli gelmez; oyuncu yardımcı kullanabilir. Bu, seçilen dalın mevcut tasarım kararıdır.
- `PuzzleSolver.ReachabilitySearch` kaynakta/testlerde kalır ama runtime otomatik fail tetikleyicisi değildir. `CheckSolvable`, güvenli Shuffle ve son çözülebilir duruma dönüş ayrı kullanımlardır; ölü kod sanıp topluca silme.
- Oluşturucunun **No Moves en erken4. hamle** hedefi, insan her tahta üzerinde4 hamlede kaybeder veya kaybetmek zorundadır demek değildir.

## Bölüm üretimi ve zorluk

`DeterministicRandom` → `LevelCandidateFactory/Selector` → **Editor/V41LevelBuilder** → `V3LevelTool` → `V3LevelData.Generated.cs` → runtime katalog. İsimlerdeV3 kalması eski V3 tablolarının çalıştığı anlamına gelmez. `V3LevelBuilder/V3LadderBuilder/V3ThemeBuilder/CampaignMoonRollout` bu dalda kaldırılmıştır; iOS rehberinden geri ekleme.

V4.1 kurucusu kilitli hazne kuralıyla aday seçer; tuzak/takılma sınıfları, No Moves derinliği ve nefes bölümleri16/31/61/81 kullanır. Düşünmeden oynayan,2 hamle ve3 hamle ileri bakan sanal oyuncu ölçülür; dolaşma ile başarısızlık ayrılır. NaiveFailureWeight15'tir. Puan formülü, bant eğrileri, derin tuzak hedefleri ve gevşetilen6 bölüm [V41_TASARIM](V41_TASARIM.md)'dedir.

100. bölüm: kapasite6,5 renk,1 boş;23 hamle ve kayıtlı puan42.5042. Hedef44 ile ölçülen puanı eşitleme. Tablodaki100 kaydı elle düzenleme, öğrenme sırasında `GenerateLevelDataBatch` çalıştırma. `RunCalibrationReportBatch/RunV41PreviewBatch/RunV41InventoryReportBatch` salt rapor girişleri ile veri üretimini ayır. Büyük havuzlu builder testleri dakikalar sürebilir. `LevelGenerator` legacy/fallback varlığı101+'ı açmaz.

## Ekonomi, yardımcılar ve kayıt

- Başlangıç0 coin,5 can; UTC30dk'da1 can, üst sınır5. Geri Al3/Ek Bulut1/Karıştır1.
- Ödüller Orta100/Zor200/Çok Zor300; ücretli devam500. Yetersiz bakiye kesilmez, çift sonuç ödülü üretilmez.
- Devam reklam veya500 coin ile **son çözülebilir snapshot'a döner** (`ResumeFromLastSolvable/FailRecovery`); otomatik ekstra hazne satın alımı değildir. Geçmiş yoksa mevcut kurtarma fallback'i korunur.
- Başarısızlık retry'si1–5 ücretsiz; sonrası1 can. Bitmemiş bölümden ana sayfa1–5 dahil1 can ve başlangıç tahtası; kazanılmış bölümden dönüş ücretsiz.
- Ek Bulut bir kullanımda1, bölüm başına en çok2. Limit ücret/hak/reklamdan önce; ek hazne ve başarılı Shuffle Undo geçmişini temizler. Shuffle tek seçili hazneyi güvenli/farklı sıraya getirir; başarısız aday hak kesmez.
- Hak yenilemeleri reklam arayüzü üzerinden; bu kaynakta gerçek AdMob etkin değildir. Markette coin ile can/yardımcı satıldığı varsayılmaz.
- `SaveData` şema2, `cloudora-save.json` + `.bak/.tmp`, failRecord/Undo/modifier geçmişi ve safeShuffleCharges korunur. PlayerPrefs ses/titreşim/hareket tercihlerini de taşıyabilir. Yeni ekip kaydı veya telefondaki ilerleme öğrenme için sıfırlanmaz.

## Görsel sözleşme

Yaklaşık3sn açılış; sabit cam, elemente özel üstten uçuş/iz, içeride birleşik sıvı yükselişi, küçük vektör ikonlar, hazne altı dekoratif bulut olmaması ve yedi bitiriş korunur. `IOS_ITEM_TRANSFER_RESTORE` dışarıdaki kesintisiz sıvı şeridi denemesini geçersiz kılar; eski `IOS_LIQUID_MOTION` tek başına güncel karar değildir. Ay'ın son bitirişi tutulmadan çıkan kraterli dolunay/ay ışığıdır. Hareketi Azalt ve nesne/Canvas/mesh tekrar kullanımı korunur.

V4.1 öğretici beyaz16 punto, gerektiğinde12'ye küçülen tek satırdır; `AdaptiveBoardLayout.ReservedTop/TutorialClearance` yazı ile tahtayı ayırır. `SetLives` milestone kaynaklarını yeniler. Bu düzeltmeleri iOS controller'ı komple kopyalayarak geri alma. Hedef60FPS bütün telefonlarda ölçülmüş garanti değildir.

## Servis, test kanıtı ve yayın sınırı

Reklam/analytics arayüzleri, FakeAd test servisi, kapalı AdMob adaptörü ve Firebase yoksa debug fallback bulunur. QA defines ve gerçek gelir/mağaza servislerini ayır. BillingMode.json tek başına IAP sistemi değildir.

**Kaynak dalın tarihsel kanıtı:** v4.1 EditMode197 toplam/194 geçti/3 atlandı/0 hata;2 grafik gereksinimi ve1 manuel ölçüm. Belgelerde19 Ekim Editor4 tur ve Galaxy S24 FE'de7 bölüm QA APK testi kayıtlı; başarısızlık paneli telefonda görülmemiş. Eski10.000 içerik testi güncel100'ün görsel/insan zorluğu kabulü değildir. Loglar ve APK Git dışında olduğundan yeni ekip aynı dosyaları klonda bulmayı beklememeli.

**Bu24 Kasım teslimi:** kaynak/ayar/varlık hash eşliği, LFS ve belge bağlantıları kontrol edilir. Unity testleri, yeni APK, iPhone kurulumu veya mağaza yayını bu aktarımda yeniden yapılmaz; önceki kanıtlar bugünkü test gibi sunulmaz. Üretim imzası ve Google Play/App Store yayını ayrı görevdir.

## Sonraki işler

[SONRAKI_ADIMLAR](SONRAKI_ADIMLAR.md): yüksek çözünürlüklü dünya tasarımları, Android gri şerit, çıkmaz yardım vurgusu, hamle logu, gevşetilen6 bölüm, Atlas/coin kararları ve platform kabulü. Bunlar seçilmiş göreve dönüşmeden kendiliğinden uygulanmaz.
