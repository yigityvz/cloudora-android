# iOS sürümünün v3.2'den farkları ve Android v4

> **Android main için okuma notu (24 Kasım2026):** Bu belge iOS/v4 geçmişidir. Güncel taban Bahadır v4.1; Ay61, Shuffle15 ve yalnız No Moves başarısızlığı için [GUNCEL_DURUM](GUNCEL_DURUM.md) ve [V41_TASARIM](V41_TASARIM.md) esas alınır. Görsel/teknik kayıtları yeni kural sanarak eski davranışı geri getirme.

> **Durum:** son hal, 2026-10-19 13:09. Android'e birebir uyarlandı (v4); fark incelemesi (§1–10), ölçüm (§10b), uyarlama (§10c) ve telefon testi (§11). Görsel özet: `Docs/IOS_FARKLARI.html`.
> **v4.1 notu (2026-10-19):** v4.1'de Ç1 (başarısızlık yeniden yalnızca hiç hamle kalmayınca), Ç3 (Ay 61'de, Frost Peaks ile) değişti; Karıştır 15'te; bölümler yeniden kuruldu, `CampaignMoonRollout` kaldırıldı. Ayrıntı `Docs/V41_TASARIM.md`.
> **Kullanıcı kararı:** "ios'da neyse aynısı olsun" — §10'daki çelişkilerin hiçbiri değiştirilmedi; ileride ayrı ayrı karar verilecek.
> **Kaynak:** `https://github.com/yigityvz/cloudora-ios` (kullanıcı verdi), dal `main`, HEAD `1ee4334`. Salt okunur kopya: `iOS-inceleme\cloudora-ios\`.
> **Karşılaştırma tabanı:** bizim v3.2 (`d7c9416`, `cloudora-bahadir` / `bahadir-claude-versions`).
> **Yöntem:** `git diff d7c9416 1ee4334` (net fark) + iOS reposundaki `Docs/IOS_*.md` belgeleri (§1–10, kod çalıştırmadan); ölçüm §10b; Android testleri §10c–11.

---

## 1. Özet

| Commit | Tarih | Ne |
|---|---|---|
| `7026e3a` | 2026-10-16 11:17 | v3.2'nin iOS reposuna aktarımı (birleştirme). Mesaj: "without gameplay changes" — bu commit için doğru. |
| `bee01d3` | 2026-10-16 16:13 | Repo yalnızca iOS: Android build aracı, manifest işleyicisi, Android build profili, `Build-Android.ps1`, `androidjni` modülü, Google Play belgeleri ve Play görselleri silindi. |
| `1ee4334` | 2026-10-19 02:13 | Tek büyük commit (17–19 Ekim çalışmaları): Figma tasarımı, açılış animasyonu, yeni ana sayfa/dünya/duraklatma/sonuç ekranları, cam hazneler, element aktarım ve bitiriş animasyonları, Türkçe arayüz, **oyun kuralı değişiklikleri**, **ekonomi**, **3 dünya / 100 bölüm sınırı**, **Ay 60'tan itibaren** (60–80 yeniden seçildi). |

Net fark (v3.2 → HEAD): 390 dosya, +21.739 / −3.596 satır. Bunların 103'ü kod/ayar/belge, 73'ü Figma kaynak SVG'si (`Docs/DesignSources`), 87'si oyun içi görsel/font (`Resources/Design`, `Resources/Splash`).

**En önemli 5 fark (oynanışı değiştirenler):**
1. Tamamlanmış hazne kilitlenir (yeni hamle kuralı).
2. Başarısızlık = "tahta çözülemez" (eskiden "hiç legal hamle yok").
3. Bitmemiş bölümden Ana Sayfa'ya dönmek 1 can götürür (1–5 dahil).
4. Ekonomi ×10: 100/200/300 ödül, devam 500.
5. Ay 60'ta gelir; 60–80'in 21 tahtası değişti. Oyun 100'de biter (101+ kapalı). Dünyalar 3: tanıtım 1–14, Yeşil Vadi 15–30, Rainy Coast 31–60, Frost Peaks 61–100.

---

## 2. Oynanış kuralları

### 2.1 Tamamlanmış hazne kilidi
- **Ne:** Dolu ve (Gökkuşağı hariç) tek renkli hazne artık kaynak olamaz: seçilemez, boşaltılamaz, karıştırılamaz (`CloudState.IsComplete`, `PuzzleRules.CanApply`, `CloudShuffler.Shuffle`, `PuzzleController.HandleCloudClicked/ShuffleCloud`). Çözücüye `lockCompleted` parametresi eklendi (varsayılan `true`).
- **Neden (iOS belgesi `IOS_COMPLETION_EFFECTS.md`):** tamamlanan haznede bitiriş efekti oynar; kural "görüntü, veri modeli, animasyon ve çözüm aramasında aynı".
- **Etki:** Oyunun durum grafiği değişir; bazı hamleler (tamamlanmış bir haznenin parçalarını başka bir aynı renkli üstün üzerine kısmen taşımak) kalkar. iOS belgesine göre 100 başlangıç tahtasının hepsi yeni kuralla çözülebilir.
- **Bizim tasarımla ilişkisi:** v3 puanları, tuzak/çıkmaz/No Moves ölçüleri **eski kuralla** hesaplandı. iOS'ta ölçüm ve seçim araçları bilerek eski kuralla çalışmaya devam ediyor (`LevelCandidateSelector`, `WriteCalibrationReport`: `lockCompleted:false`), oyun ise yeni kuralla. → **Ölçülen zorluk ile oynanan zorluk ayrışabilir** (Faz 2'de iki kuralla ölçülecek).

### 2.2 Başarısızlık tetikleyicisi
- **Ne:** Eski: hamleden sonra hiç legal hamle yoksa başarısızlık (`HasLegalMove`). Yeni: hamleden sonra arka planda tam durum araması (`PuzzleSolver.ReachabilitySearch`, kare başına ~2 ms, 128 düğüm) çalışır; **çözüme hiçbir yoldan ulaşılamadığı kanıtlanırsa** başarısızlık (`reason=unsolvable`). Sonuçlar 512 girdilik önbellekte (anahtar: kapasite + hazne sayısı + kanonik tahta). Hamle/Undo/Extra/yükleme aramayı iptal eder.
- **Neden (`IOS_UX_REVIEW.md`):** kullanıcı fotoğrafındaki tahtada (`RRR/NNN/NRSSS/SRNS`) legal hamle var ama çözüm yok; oyuncu fark etmeden takılı kalıyordu.
- **Bizim kararlarla çelişki:** v3.1 kararı (tasarım §6b): "Hamle yapılabiliyor ama kazanılamıyorsa No Moves tetiklenmez; oyuncu Undo / Shuffle / Extra kullanır." Yeni kural bunu tersine çeviriyor. Ayrıca v3'ün "No Moves" tasarımı (bantlara dağıtılmış, `NoMovesDepth` ölçüsü, "sürpriz" No Moves) "hamle kalmayan durum" üzerine kurulu; artık her çıkmaz durum başarısızlık sayıldığından No Moves kotaları ve tuzak bölümleri farklı davranır (tuzağa düşen oyuncu daha erken başarısız olur; Undo/Shuffle ile kurtarma şansı yok).
- **Risk (Faz 2'de bakılacak):** büyük tahtalarda (5 renk, 7 hazne, +2 Extra) aramanın telefonda süresi; aramanın sürdüğü sırada oyuncunun hamle yapmaya devam etmesi (iptal/yeniden başlatma döngüsü); çözülemez durumda oyuncu birkaç hamle daha yaptıktan sonra başarısızlık gelmesi.

### 2.3 Ana Sayfa'ya dönüş ve can
- **Ne:** Duraklatmadan Ana Sayfa'ya dönmek: bölüm bitmediyse **1 can harcanır**, bölüm başlangıç tahtasına döner (can 0 ise çıkışa yine izin var, eksiye düşmez). Kazanılmış bölümden dönüş ücretsiz ve sonraki bölümü hazırlar (`ReturnHome`).
- **Bizim kararlarla çelişki:** tasarım §6b/§5: 1–5'te bölüme başlamak ve can ile baştan başlamak can götürmez. Yeni kod yorumu: "Abandoning any unfinished attempt costs one available life, even in tutorials." Ayrıca §15'teki "menüden bölümü baştan başlat" fikrinin karşılığı gibi çalışıyor (ama ücretli).

### 2.4 Diğer kural değişiklikleri
- **100 sınırı:** `WorldCatalog.MaxLevel = 100`; `ProgressionManager` 1–100'e sıkıştırır; 100 bitince kampanya tamamlandı ekranı, 101 yüklenmez (`LevelGenerator` kodda duruyor, erişilemez). Bizde 101+ eski üreticiyle oynanıyordu (bilinen sıçrama §9).
- **Shuffle:** açılma bölümü değişmedi (16). Tamamlanmış hazne seçilemez.
- **Undo:** seçimi de temizliyor; Undo sonrası çözülebilirlik araması yeniden başlar. Bitiriş efekti Undo ile durur.
- **Interstitial reklam ve sonuç paneli:** son hazne tamamlanınca 1,78 sn bitiriş efekti, sonra panel; ödül bu beklemeden önce kaydedilir.
- **`CLOUDORA_DEVICE_QA` derleme tanımı:** release derlemede de sahte reklam servisi (test ödülleri) — iOS cihaz testi için.

---

## 3. Bölümler ve dünyalar

### 3.1 Ay 60'ta (60–80 yeniden seçildi)
- **Ne:** Yeni yayımlama katmanı `Editor/CampaignMoonRollout.cs`: kurucuların (değişmeyen) sonucunu alır, 60–80'deki 21 slotu 5 renkli (Ay dahil) adaylarla yeniden seçer. Koşullar: eski puanın ±0,6'sı, aynı bant/kapasite/boş hazne/dizilim, No Moves kotaları, en fazla eski çözüm +4 hamle, final 60 en az eski uzunlukta (14) ve daha tuzaklı, yeni kuralla çözülebilir, en az 2 haznede Ay. 1–59 ve 81–100 birebir aynı. Veri dosyası araçla yeniden üretildi (`V3LevelData.Generated.cs`: 21 satır değişti).
- **iOS ölçümü (belgeden):** puan farkı en fazla 0,30, ortalama 0,06; 60: 29,15 → 29,01, 14 hamle. 61–80 en kısa çözüm 15–19 hamle (eskisi 12–16).
- **Neden:** "Kullanıcı tercihi: Ay 60'tan itibaren … beşinci olarak eklensin" (`IOS_MOON_FROM_60.md`; kullanıcı = iOS tarafındaki kişi).
- **Bizim kurallarla çelişki:**
  - Yenilik ilk kez bir **Orta**'da gelir (§11c) → Ay **60'ta, Çok zor tema finalinde** geliyor.
  - Yeni renkle boş bulut ve dizilim "nefes alır" (§11c, şablon) → 60–80 çerçeveleri (1 boş, klasik/1 açık) korundu; 5. renk nefes almadan geliyor (81'de nefes için 2 boş / 0 açık idi; 81 artık yenilik değil).
  - Final 60, "parçanın en tuzaklısı" ve sınav; aynı bölüme yeni renk eklenince iki yük üst üste biniyor.
  - Hedef eğrisi tablosu (61–80: 4 renk, ~14–15 hamle) → 5 renk, 15–19 hamle; puan aynı tutulmuş ama uzunluk arttı ("zorluk yalnızca hamle sayısıyla artmamalı", §11j). Kullanıcı testinde 81+ "epey zorlayıcı" bulunmuştu; bu zorluk 60'a çekildi.
  - Puanlar eski kuralla (§2.1) ölçüldü.
- **Belge tutarsızlığı (iOS reposu):** `IOS_DESIGN_IMPLEMENTATION.md` hâlâ "bölüm verileri, çözücü, … ekonomi değiştirilmedi", "40 jeton", "13 dünyalık katalog korunur" diyor; sonraki belgeler (`IOS_QUALITY_CAMPAIGN`, `IOS_UX_REVIEW`, `IOS_COMPLETION_EFFECTS`, `IOS_MOON_FROM_60`) bunları değiştiriyor. `IOS_QUALITY_CAMPAIGN.md` dünya aralıklarını 16–40 / 41–70 / 71–100 veriyor, `IOS_UX_REVIEW.md` 15–30 / 31–60 / 61–100 (kodda bu sonuncusu).

### 3.2 Dünya listesi
- **Ne:** 13 dünya → 3: Yeşil Vadi 15–30, Rainy Coast 31–60, Frost Peaks 61–100; 1–14 dünyasız tanıtım (`IntroLastLevel = 14`). 14 bitince "tanıtım tamamlandı → Yeşil Vadi'yi keşfet" ekranı (`MilestoneScreen`); 30/60/100'de kurtuluş ekranı ve tekrar izleme.
- **Bizim planla ilişkisi:** Faz 5.1 (dünya listesi) bekliyordu; plan: 1–15 "Tanıtım", 16–30 Green Valley. iOS: tanıtım 1–14, Yeşil Vadi 15'ten.
- **Tutarsızlıklar (kod içinde):** öğretici yazılar ve `tutorialFlags` 1–15; 15'in yazısı "İlk dünyanı keşfetmeye hazırsın." ama 15 artık Yeşil Vadi'nin içinde ve "keşfet" ekranı 14'ten sonra çıkıyor; `IOS_DESIGN_IMPLEMENTATION.md` geçişi "15. bölüm sonunda … → bölüm 16" diye anlatıyor. Shuffle 16'da (dünyanın 2. bölümü). Ad dili karışık: "Yeşil Vadi" Türkçe, "Rainy Coast" ve "Frost Peaks" İngilizce; Frost Peaks'in eski "Frozen" etiketi ve Rainy Coast'un "Wind" etiketi duruyor (modifier yok).

### 3.3 Değişmeyenler
- 1–59 ve 81–100 tahtaları, 5'li desen ve bantlar, kurucular (`V3LevelBuilder`, `V3LadderBuilder`, `V3ThemeBuilder`), puan formülü, RNG. (`LevelCandidateSelector` yalnızca `lockCompleted:false` parametresi aldı.)

---

## 4. Ekonomi ve ilerleme

| | v3.2 | iOS |
|---|---|---|
| Orta / Zor / Çok zor ödülü | 10 / 15 / 20 | 100 / 200 / 300 |
| 101+ ödülü | 10 | — (101 yok); kural 5'li döngüyle her bölüme uygulanır |
| Başarısızlıktan devam | 40 | 500 |
| Başlangıç coini | 0 | 0 |
| Ana Sayfa'ya dönüş (bitmemiş) | ücretsiz (menü açılır) | −1 can |
| Reklamla can ("AD +1") | sağ üstte geçici buton | duraklatma menüsünde; can 0 iken ana sayfada "Can Kazan · +1" |

- Oran (devam / Orta ödülü): v3.2'de 4, iOS'ta 5 → devam görece pahalandı.
- Kayıtta coin alanı aynı (`coins`); eski kayıtlardaki coin dönüştürülmez ("eski kazanımlar geriye dönük değiştirilmez").

---

## 5. Arayüz ve tasarım
- **Kaynak:** Figma `PfPK72jwbGPBJ8Ah0RlwWx`; native uGUI; yeni yardımcılar `DesignUI.cs`, `VectorShapes.cs`; font Nunito ExtraBold (OFL lisansı `Docs/DesignSources/Nunito-OFL.txt`). Tasarım birimi 390×844; Retina için metin 3× üretilip ölçekleniyor.
- **Açılış animasyonu** (`OpeningSplash.cs`): 3 sn bulut perdesi + logo; Reduce Motion desteği (iOS'ta yerel UIKit sorgusu `Plugins/iOS/CloudoraAccessibility.mm`).
- **Ana sayfa** (`MainMenuController.cs`): 1–14 bulut arka planı, sonra dünya kartı, yüzde ve sayı (ör. 1/16), kaynak sayaçları, Oyna/Dünyalar; kampanya bitince tamamlandı kartı.
- **Dünyalar ekranı** (`WorldScreenController.cs`): 3 kart, kilitli/aktif/kurtarılmış, ziyaret, kurtuluş tekrarı (`WorldRescueReplay.cs`), dünya atmosferi (`WorldAtmosphere.cs`: yaprak/yağmur/kar).
- **Oynanış** (`GameplayOverlay.cs` büyük ölçüde yeniden yazıldı): üst kaynak çubuğu, zorluk rozeti, Undo · Extra · Shuffle (sıra değişti: bizde Extra · Undo · Shuffle), World/Menu yerine duraklatma; "16. bölümde" yazılı kilitli Shuffle; yardımcı ödül mesajları tam panel yerine kısa bildirim.
- **Bilgi penceresi kilidi:** bizim `ButtonShield` kaldırıldı; pencere girdiyi yakalıyor ve dokunuşta yardımcıyı kullanmadan kapanıyor (`InfoPopupInterceptsInputAndDismissesWithoutUsingHelper`). Davranış amacı korunmuş görünüyor; görsel olarak butonları örtüp örtmediği Faz 2'de bakılacak (§11g kararı: "butonları görsel olarak örtmez").
- **Hazneler** (`CloudContainerView.cs`): bulut yerine cam şişe; ton geçişli dolgu (`LiquidFillGraphic.cs`), vektör maske, yansıma; bulut ayakları kaldırıldı; renkler: rüzgâr mint (bizim v3.2 gümüş #AEB7C2'den farklı — Faz 2'de kontrol).
- **Element simgeleri** (`WeatherGlyph.cs`): 7 element için monoline vektör simge (bizde metin desenleri `*`, `///`, `+`…).
- **Duraklatma / sonuç / başarısızlık panelleri** yeni; kazanınca "Ana Sayfaya Dön".
- **Arayüz dili:** İngilizce → Türkçe (öğretici yazılar, ödül/hata mesajları). Bizim kural: oyuncuya görünen metin için seçenek sunulur; bu metinler kullanıcıya sunulmadan değişmiş olacak (Faz 2 kararı).

## 6. Animasyon ve efektler
- **Aktarım** (`MoveAnimator.cs`, `WeatherMotionLibrary.cs`): şişeler sabit; element kendi simgesi, izi ve parçacıklarıyla kavis çizip hedefin ağzından girer; hazne içi seviye sıvı gibi yükselir/durulur. Element başına profil (süre, yay, savrulma); yeni elementler için varsayılan profil. Uzun karede ilerleme en fazla 40 ms.
- **Hazne bitirişi** (`VesselCompletionFlourish.cs`, 446 satır): 7 element için 1,95 sn sahne (güneş küresi, sağanak, kristal, sarmal, dolunay, şimşek, gökkuşağı yayı); vertex gradyanı; 24.000 vertex tavanı; son haznede sonuç paneli 1,78 sn bekler.
- **Azaltılmış hareket:** oyun tercihi `Cloudora.ReducedMotion` (PlayerPrefs) + iOS sistem ayarı (yalnızca açılışta).

## 7. Performans
- Hamlede cam ve dolgu nesneleri yok edilip yaratılmıyor, havuzdan tekrar kullanılıyor; dekoratif atmosfer ayrı Canvas; sayaçlar yalnızca değişince yazılıyor (`PuzzleController.Update`: v1'den beri açık olan "her karede string" sorununu da kapatıyor).
- Kalite ayarı: `globalTextureMipmapLimit` 1 → 0 (görseller tam çözünürlük; bellek artar). Hedef 60 FPS.
- iOS ölçümleri yalnızca Editor CPU/GC ölçümü; cihaz FPS/termal ölçümü yapılmamış (belgede açıkça yazıyor).

## 8. Platform (iOS'a özel) ve Android'e etkisi

| Değişiklik | iOS | Android uyarlamasında |
|---|---|---|
| `CloudoraBuildTools.cs`, `CloudoraAndroidManifestPostprocessor.cs`, `Build-Android.ps1`, `Android™` build profili, `BillingMode.json`, `androidjni` modülü, `ANDROID_BUILD.md`, `GOOGLE_PLAY_SUBMISSION.md`, Play görselleri | silindi | **geri getirilmeli** (bizim v3.2'den) |
| `CloudoraIosBuildTools.cs`, `Build-iOS.sh`, `Plugins/iOS/CloudoraAccessibility.mm`, iOS ikonları/PlayerSettings | eklendi | Android'de derlenmez/etkisiz; azaltılmış hareket için Android karşılığı kararı (Faz 3) |
| Otomatik döndürme: yalnızca dikey | `allowedAutorotate*` 0 | Android'e de uygun görünüyor (bizde ters dikey/yatay açıktı) |
| `targetDevice` 2 → 0 | yalnızca iPhone | Android'i etkilemez |
| `CLOUDORA_DEVICE_QA` | test reklamı, filigransız | Arkadaşlara APK kararında kullanılabilir (Faz 6) |
| Titreşim | `Handheld.Vibrate` korunmuş; eski iOS haptic adaptörü (`HapticFeedback.cs`) taşınmamış | Android'de aynı |

## 9. Testler
- Yeni test dosyaları (14): `AbandonAttemptTests`, `CampaignReleaseTests`, `CompletionEconomyTests`, `CompletionFlourishTests`, `DesignNavigationTests`, `ElementMotionTests`, `LiquidMotionTests`, `MainMenuLifeTests`, `MoonRolloutTests`, `MoveFlowTests`, `OpeningSplashTests`, `PointerMotionVisualTests`, `ReachabilityRegressionTests`, `RenderingReuseTests`.
- Bizim testlerden değişenler (8): `V31GameplayTests` (buton düzeni, "AD +1" yeri, bilgi penceresi), `V32InputTests` (`ButtonShield` testi yerine pencerenin girdiyi yakalaması), `V3LevelBuildTests` (dünya sınırları 20/21 → 40/41 ve 30/31), `PuzzleFeelTests` (Android manifest testi silindi), `V32VisualTests`, `V32NoMovesTests`, `V32ThemeTests`, `V32Theme3Tests` (birer satır).
- iOS'un bildirdiği son sonuç: 241 test, 235 geçti, 0 başarısız, 6 atlandı (2'si grafik gerektiren). Bizim v3.2: 196 / 192 / 0 / 4. Toplam `[Test]` sayısı (HEAD): 167 öznitelik, 27 dosya (parametreli testler sayıyı artırıyor).

## 10. Bizim tasarım kurallarıyla çelişkiler (karar gerektirenler)

| # | Konu | Bizim karar (kaynak) | iOS | Önem |
|---|---|---|---|---|
| Ç1 | Başarısızlık tetikleyicisi | Yalnızca hiç hamle kalmazsa (§6b) | Çözülemez olunca | Yüksek — No Moves tasarımı ve tuzak bölümleri |
| Ç2 | Tamamlanmış hazne kilidi | Yok; ölçüler kilitsiz | Kilitli; ölçüler hâlâ kilitsiz | Yüksek — puanların geçerliliği |
| Ç3 | Ay 60'ta | 81'de, Orta'da, nefesle (§11c, şablon) | 60 (Çok zor final), nefessiz | Yüksek — zorluk eğrisi |
| Ç4 | 1–5 can kuralı | Ücretsiz (§5, §6b) | Ana Sayfa'ya dönüş 1 can (1–5 dahil) | Orta |
| Ç5 | Ekonomi | 10/15/20, devam 40 (ilk ayar) | 100/200/300, devam 500 | Orta (bizde de "ilk ayar") |
| Ç6 | Dünya aralıkları | Plan: 1–15 Tanıtım, 16–30 Green Valley (§11d) | 1–14, 15–30 | Düşük–orta; 15'in öğretici yazısıyla tutarsız |
| Ç7 | 101+ | Eski üreticiyle oynanır | Kapalı, oyun 100'de biter | Düşük (101 sıçraması zaten sorundu) |
| Ç8 | Arayüz dili | İngilizce; oyuncu metinleri seçenekle | Türkçe, seçeneksiz | Düşük–orta |
| Ç9 | Bilgi penceresi | Butonları görsel olarak örtmez (§11g) | Girdiyi yakalar; görünüm Faz 2'de | Düşük |
| Ç10 | Rüzgâr rengi | Gümüş #AEB7C2 (v3.2) | Mint | Düşük |

**Durum (v4):** hepsi iOS'taki gibi uygulandı (kullanıcı kararı). Ölçüm sonrası önem: Ç1 yüksek (sahada doğrulandı, §11), Ç2 düşük (kilit zorluğu neredeyse değiştirmiyor, §10b), Ç3 orta (puan aynı, çözüm 3–4 hamle uzun), Ç9 düşük (pencere girdiyi yakalıyor; kullanıcı testinde sorun görülmedi).

## 10b. Faz 2 ölçümü (2026-10-19 11:48)
- **Yöntem:** scratchpad'de konsol programı; iOS'un `PuzzleSolver.cs`, `Move.cs`, `WeatherType.cs`, `DeterministicRandom.cs`, `V3BoardText.cs` dosyaları yalnızca okunarak bağlandı. iOS ve v3.2 veri dosyaları metin olarak okundu. Her bölüm: çözücü iki kuralla (kilitsiz / kilitli) + kendi BFS'im (başarısızlık derinlikleri) + oyun içi `ReachabilitySearch` süresi (en yakın 20 + rastgele 20 çıkmaz durum, ayrıca başlangıç + 2 Extra). Ham veri: `iOS-inceleme\olcum\olcum.csv` (program `Program.cs`).
- **Doğrulama:** dosyadaki puanlar kilitsiz ölçümle aynı (en büyük fark 0,01). Örnekteki bütün çıkmazlarda oyun içi arama "çözülemez" dedi (BFS ile uyumlu).

**Kilit kuralı (Ç2) — etkisi küçük:** 100 bölümün hiçbirinde en kısa çözüm değişmiyor; puan 68 bölümde biraz düşüyor (en çok −0,83, L9; ortalama küçük), hiçbirinde artmıyor. Yani kilit kuralı zorluk eğrisini bozmuyor, çok az kolaylaştırıyor.

**Başarısızlık kuralı (Ç1) — etkisi büyük:**
| | Eski kural (hiç hamle yok) | Yeni kural (çözülemez) |
|---|---|---|
| Başarısızlığın mümkün olduğu bölüm | 26 | **61** |
| 1. hamlede başarısızlık mümkün | 26, 50, 78 | **26, 50, 55, 78, 95, 98, 100** |
- 35 bölümde başarısızlık yalnızca yeni kuralla ortaya çıkıyor (çoğu 4–5 renkli 2 boşlu planlama bölümleri: 32, 34, 35, 37, 38, 40, 42 … 96). v3'te bunlar "No Moves yok, çıkmaza girersen Undo/Shuffle" bölümleriydi.
- Tuzak bölümlerinde başarısızlık öne geliyor: 98 (5 → 1. hamle), 100 (3 → 1), 30 (4 → 2), 55 (2 → 1), 95 (2 → 1).
- v3'ün "No Moves bantlara dağıtılmış, parça başına belli sayıda" kotası artık anlamını yitiriyor; fiilen 61 bölümde başarısızlık var.

**Ay 60 (Ç3):** 60–80'in puanları eskisine çok yakın (fark ≤ 0,3) ama en kısa çözüm çoğunda arttı: 61–80 eskiden 12–16, şimdi 15–19 hamle (ör. 65: 15 → 19, 70: 15 → 19, 75: 12 → 16, 72/73: 13 → 17). Puan aynı kalırken uzunluk arttı; puanın içinde uzunluk dışındaki zorluk (yanıltıcılık, sanal oyuncu) azalmış.

**Oyun içi arama performansı:** sorun görülmedi. Çıkmaz durumdan başlayan aramalar PC'de 1 karede (≤128 düğüm, <0,1 ms) bitti; başlangıç + 2 Extra tahtasında en çok 6,1 ms (telefonda ~10 kat → ~60 ms, kare başına 2 ms bütçeyle birkaç kareye yayılır).

**Hafif kod incelemesi (hata avı):** çökme ya da veri bozan bir hata görülmedi. Notlar: başarısızlık analytics nedeni `no_moves` → `unsolvable` (log analizlerimiz buna göre güncellenmeli); arama yalnızca hamle/Undo/Extra sonrası çalışıyor (Shuffle ve "devam" sonrası çalışmıyor — ikisi de çözülebilir durumu garanti ettiği için doğru); son hazne bitince 1,78 sn girdi kilitli.

## 10c. Android'e uyarlama (Faz 3, 2026-10-19 11:59)
Kullanıcı kararı: **iOS'ta neyse aynısı** (Ç1–Ç10 dahil, hiçbiri değiştirilmedi); farklar belgede işaretlenir, kullanıcı test eder.
- iOS'ta eklenen/değişen 361 dosya birebir alındı (bayt bayt kontrol).
- **Android için bilerek iOS'tan farklı kalanlar:** Android build aracı, manifest işleyicisi ve testi, `Build-Android.ps1`, Android build profili, `BillingMode.json`, `androidjni` modülü, Android/Play belgeleri ve Play görselleri (iOS bunları silmişti).
- **Android'e eklenen:** `Android Device QA APK` derleme modu = iOS cihaz derlemesinin karşılığı (Development filigranı yok, `CLOUDORA_DEVICE_QA` ile sahte reklam/test ödülleri).
- **Platform farkı (davranış):** iOS'ta açılış animasyonu sistemin "Hareketi Azalt" ayarını da okur; Android'de yalnızca oyun içi tercih (`Cloudora.ReducedMotion`) geçerli.
- **Testler:** 242 / 236 geçti / 0 başarısız / 6 atlandı (iOS ile aynı sonuç + Android manifest testi).
- **Doku ayarı (12:31):** iOS'ta 78 tasarım görseli 4096 / RGBA32 / sıkıştırmasız; Android'de ayar yoktu (2048 + sıkıştırma). Android'e de aynısı verildi (`.meta` + `DesignQuality.cs`).
- **Açık iş — dünya resimleri düşük çözünürlüklü (kullanıcı: "daha sonra"):** üçlü resimler 2340×1300 → aşama başına ~772×1300; iPhone SE'de ~1:1, S24 FE (1080×2340) gibi büyük ekranlarda ~1,8× büyüyüp bulanık. Yüksek çözünürlüklü dışa aktarım gerekiyor (iOS'un büyük iPhone'larını da etkiler).
- **Metin netliği:** başarısızlık ekranındaki "Ödülle devam et" = reklam izleyerek devam; metinde "reklam" geçmiyor.

## 11. Telefon testi (Android, Galaxy S24 FE, "Cloudora v4", 2026-10-19 12:06–12:58)
Kullanıcı oynadı; kayıt bölüm atlamaları adb ile (her seferinde yedek). Log: `Logs/oyun-testleri/2026-10-19-telefon-v4-qa.txt`.

| Test | Sonuç |
|---|---|
| Genel (A1–A17: geri tuşu, yerleşim, arka plan/kapat-aç, açılış, kilit, bitiriş efekti, aktarım, Undo/Extra, Shuffle kilidi, coin, Ana Sayfa can, sıfır can, duraklatma, Dünyalar, Ayarlar, Türkçe) | Kullanıcı: "gözüme bir şey çarpmadı" |
| B1 14 → 15 | "Tanıtım tamamlandı" ekranı çıktı; 15'in öğretici yazısı üst haznelerin hemen üstünde |
| B2 16–17 Shuffle | Çalışıyor |
| B3 21 tuzak | 2 hamlede `unsolvable`, ekran hemen; coinle (500) ve "Ödülle devam et" (= reklam) 1 hamle geri, çözülebilir duruma dönüyor |
| B4 30 → 31 | Kurtuluş ekranı; dünya geçiş animasyonu yok (iOS'ta da yok) |
| B5 35 | Normal oynanışla geçildi (79 sn, 20 hamle; en kısa 16). Bilerek çıkmaz denemesi yapılmadı |
| B6 59 → 60 | 59'da 1 başarısızlık; **60'ta 4 başarısızlık (6., 9., 9., 10. hamle), 4 reklamla devam, Undo hiç kullanılamadı**; 54 sn, 16 hamle. Kullanıcı: "60 zor olabilir … geri alma vs işe yarayabilir". Kurtuluş ekranı ve "Kurtuluşu tekrar izle" görünür (ekran görüntüsü); animasyon "Azaltılmış hareket" açıkken ~1 sn, kapatınca "güzeldi" |
| B7 81–83 | 7 hazne + Extra + art arda Shuffle (0,5–0,7 sn aralıkla): "donmadı, akıcı, sorunsuz" |
| B8 99 → 100 | "Frost Peaks kurtarıldı", "100 bölüm tamamlandı • Ana Sayfa", 3 dünya %100, "Üç dünya · 100 bölüm. Yeni maceralar yakında." |

**Testte bulunanlar**
- **Ç1 sahada doğrulandı:** başarısızlık ekranı tahta çözülemez olduğu anda (hamle kalmışken) geliyor ve ekranda Undo/Extra yok → oyuncu kendi araçlarıyla kurtulamıyor, reklam/coin/cana yönleniyor. Eski kuralla 60'ta bu ekran hiç çıkmazdı (60'ta hamlesiz durum yok). Karar sonraya: (a) böyle kalsın, (b) başarısızlık ekranına Undo/Extra, (c) eski kural.
- **Android'e özel, düzeltildi:** tasarım görsellerinin Android doku ayarı (bkz. §10c).
- **Açık — dünya resimleri düşük çözünürlüklü** (bkz. §10c; kullanıcı "daha sonra"). **Planlanan:** temalar (dünya görselleri, arka planlar, tema adları/tasarımı) ileride güncellenecek (kullanıcı, 2026-10-19 13:11).
- **iOS'tan gelen küçük hata:** kurtuluş ekranındaki can sayacı donuk kalıyor (`MilestoneScreen.SetResources` yalnızca açılışta; `PuzzleController.Update` dünya ekranı ve ana sayfayı güncelliyor ama bu ekranı güncellemiyor). Ekranda dakikalarca 03:22 / 15:24 kaldı.
- **Android'e özel yerleşim notu:** kurtuluş ekranında üstte bildirim çubuğu yüksekliğinde ve altta ince gri şerit; arka plan oraya uzanmıyor.
- **Metin netliği:** "Ödülle devam et" reklam olduğunu söylemiyor; Extra başlangıç hakkı (1) kullanıcıya 3 gibi geldi (Undo 3) — değer değişmedi, belki arayüzde ayırt etmek zor.

---

## Ek A. Dosya listesi (v3.2 → `1ee4334`, `.meta` hariç)

Durum: A eklendi, M değişti, D silindi, R taşındı. Satır: + eklenen / − silinen.

### Oyun kodu (Scripts) (26)

| Durum | Dosya | + | − |
|---|---|---|---|
| M | `Assets/_Cloudora/Scripts/Core/CloudState.cs` | 10 | 0 |
| M | `Assets/_Cloudora/Scripts/Core/PuzzleRules.cs` | 1 | 0 |
| M | `Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs` | 7 | 7 |
| M | `Assets/_Cloudora/Scripts/Level/CloudShuffler.cs` | 1 | 0 |
| M | `Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs` | 66 | 27 |
| M | `Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs` | 21 | 21 |
| M | `Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs` | 9 | 8 |
| M | `Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs` | 5 | 5 |
| M | `Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs` | 6 | 13 |
| M | `Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs` | 140 | 166 |
| M | `Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs` | 194 | 25 |
| M | `Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs` | 120 | 39 |
| A | `Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs` | 62 | 0 |
| M | `Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs` | 11 | 7 |
| A | `Assets/_Cloudora/Scripts/UI/DesignUI.cs` | 156 | 0 |
| M | `Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs` | 135 | 428 |
| A | `Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs` | 53 | 0 |
| M | `Assets/_Cloudora/Scripts/UI/MainMenuController.cs` | 114 | 67 |
| A | `Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs` | 79 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/OpeningSplash.cs` | 215 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/VectorShapes.cs` | 134 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs` | 446 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs` | 117 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs` | 57 | 0 |
| A | `Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs` | 39 | 0 |
| M | `Assets/_Cloudora/Scripts/UI/WorldScreenController.cs` | 70 | 67 |

### Editor araçları (11)

| Durum | Dosya | + | − |
|---|---|---|---|
| A | `Assets/_Cloudora/Editor/CampaignMoonRollout.cs` | 119 | 0 |
| D | `Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs` | 0 | 52 |
| D | `Assets/_Cloudora/Editor/CloudoraBuildTools.cs` | 0 | 185 |
| A | `Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs` | 78 | 0 |
| A | `Assets/_Cloudora/Editor/DesignPreview.cs` | 70 | 0 |
| A | `Assets/_Cloudora/Editor/DesignQuality.cs` | 24 | 0 |
| M | `Assets/_Cloudora/Editor/LevelCandidateSelector.cs` | 1 | 1 |
| A | `Assets/_Cloudora/Editor/OpeningSplashPreview.cs` | 72 | 0 |
| A | `Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs` | 65 | 0 |
| M | `Assets/_Cloudora/Editor/V3LevelTool.cs` | 4 | 3 |
| A | `Assets/_Cloudora/Editor/WeatherSymbolPreview.cs` | 39 | 0 |

### Testler (22)

| Durum | Dosya | + | − |
|---|---|---|---|
| A | `Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs` | 51 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs` | 59 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs` | 110 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs` | 115 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs` | 29 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs` | 91 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs` | 70 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs` | 26 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs` | 74 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs` | 39 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs` | 51 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs` | 91 | 0 |
| M | `Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs` | 8 | 17 |
| A | `Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs` | 60 | 0 |
| A | `Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs` | 109 | 0 |
| M | `Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs` | 14 | 29 |
| M | `Assets/_Cloudora/Tests/Editor/V32InputTests.cs` | 11 | 37 |
| M | `Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs` | 2 | 1 |
| M | `Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs` | 1 | 0 |
| M | `Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs` | 1 | 0 |
| M | `Assets/_Cloudora/Tests/Editor/V32VisualTests.cs` | 9 | 9 |
| M | `Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs` | 7 | 5 |

### Platform, ayar, kök dosyalar (20)

| Durum | Dosya | + | − |
|---|---|---|---|
| M | `.gitignore` | 19 | 0 |
| A | `Assets/Plugins/iOS/CloudoraAccessibility.mm` | 6 | 0 |
| D | `Assets/Resources/BillingMode.json` | 0 | 1 |
| D | `Assets/Settings/Build Profiles/Android™.asset` | 0 | 61 |
| D | `Build-Android.ps1` | 0 | 39 |
| A | `Build-iOS.sh` | 34 | 0 |
| M | `ClaudoraRelease.md` | 26 | 1416 |
| M | `Packages/manifest.json` | 0 | 1 |
| M | `Packages/packages-lock.json` | 0 | 8 |
| M | `ProjectSettings/ProjectSettings.asset` | 255 | 143 |
| M | `ProjectSettings/QualitySettings.asset` | 1 | 1 |
| M | `README.md` | 48 | 22 |
| R | `StoreAssets/Artwork/cloudora-feature-1024x500.png` | 0 | 0 |
| R | `StoreAssets/Artwork/cloudora-icon-512.png` | 0 | 0 |
| D | `StoreAssets/Export-PlayAssets.ps1` | 0 | 57 |
| D | `StoreAssets/Play/cloudora-adaptive-background-512.png` | 0 | 3 |
| D | `StoreAssets/Play/cloudora-adaptive-foreground-512.png` | 0 | 3 |
| M | `StoreAssets/README.md` | 4 | 7 |
| M | `StoreAssets/STORE_COPY.md` | 1 | 1 |
| M | `crVersion.md` | 6 | 248 |

### Belgeler (23)

| Durum | Dosya | + | − |
|---|---|---|---|
| D | `Docs/ANDROID_BUILD.md` | 0 | 31 |
| D | `Docs/GOOGLE_PLAY_SUBMISSION.md` | 0 | 41 |
| A | `Docs/IOS_ARCHITECTURE.md` | 10 | 0 |
| A | `Docs/IOS_BUILD.md` | 82 | 0 |
| A | `Docs/IOS_COMPLETION_EFFECTS.md` | 35 | 0 |
| A | `Docs/IOS_DESIGN_IMPLEMENTATION.md` | 36 | 0 |
| A | `Docs/IOS_DEVICE_QA.md` | 43 | 0 |
| A | `Docs/IOS_ELEMENT_MOTION.md` | 43 | 0 |
| A | `Docs/IOS_ITEM_TRANSFER_RESTORE.md` | 15 | 0 |
| A | `Docs/IOS_LIQUID_MOTION.md` | 17 | 0 |
| A | `Docs/IOS_MOON_FROM_60.md` | 33 | 0 |
| A | `Docs/IOS_OPENING_SPLASH.md` | 26 | 0 |
| A | `Docs/IOS_PERFORMANCE.md` | 38 | 0 |
| A | `Docs/IOS_QUALITY_CAMPAIGN.md` | 36 | 0 |
| A | `Docs/IOS_STATUS.md` | 117 | 0 |
| A | `Docs/IOS_UX_REVIEW.md` | 46 | 0 |
| A | `Docs/IOS_WEATHER_SYMBOLS.md` | 21 | 0 |
| M | `Docs/PRIVACY_RELEASE_REVIEW.md` | 4 | 17 |
| M | `Docs/PROJECT_PROGRESS.md` | 2 | 78 |
| M | `Docs/PROJE_HAFIZASI.md` | 55 | 150 |
| M | `Docs/RELEASE_CHECKLIST.md` | 8 | 30 |
| M | `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md` | 2 | 0 |
| M | `Docs/V3_SABLON.md` | 2 | 0 |

### Oyun görselleri ve font (Resources/Design, Resources/Splash) (87)

Hepsi: 87 A. Dosya adları depoda; tek tek listelenmedi.

### Figma kaynakları (Docs/DesignSources) (73)

Hepsi: 73 A. Dosya adları depoda; tek tek listelenmedi.
