# Cloudora — Geliştirme Önerileri (v1 sonrası)

> Hazırlanma: 2026-10-12 · Temel: `bahadir-claude-versions` @ `755cbbc` (v1)
> Kaynaklar: kaynak kodun tamamı, Unity Editor testi, Galaxy S24 FE cihaz testi (logcat + analytics), kullanıcı yorumları (`crVersion.md` → v1).
> Bu doküman bir **öneri listesidir**; hiçbir madde onay olmadan uygulanmaz.

**Efor:** S = birkaç saat · M = 1–2 gün · L = birkaç gün+  ·  **Etki:** ●●● yüksek · ●● orta · ● düşük

---

## Özet — Öncelik tablosu

| # | Öneri | Kategori | Efor | Etki | Kaynak |
|---|---|---|---|---|---|
| D1 | Bulut/arka plan renk kontrastı | Tasarım | S | ●●● | Kullanıcı yorumu |
| D2 | Alt butonları 3+3 ızgaraya hizalama | Tasarım | S | ●●● | Kullanıcı yorumu |
| D3 | Menü/World/Settings için tam opak arka plan | Tasarım | S | ●● | Kullanıcı yorumu |
| D4 | World ekranını anlaşılır yapma | Tasarım | M | ●● | Kullanıcı yorumu |
| F1 | **Ses yok** — gerçek ses efektleri ekleme | Özellik | M | ●●● | Kod incelemesi |
| G1 | Zorluk ve tekrar: üreticiyi yeniden dengeleme | Oynanış | L | ●●● | Kullanıcı yorumu + logcat |
| B1 | Night/Fog gizlemesi desenleri gösteriyor | Hata | S | ●●● | Kod incelemesi |
| B2 | Seviye bitince Undo → çift tamamlanma | Hata | S | ●● | Kod incelemesi |
| D5 | Kısa, hafif titreşim (500 ms yerine) | Tasarım | S | ●● | Logcat |
| D6 | Buton geri bildirimi + hızlı basma koruması | Tasarım | S | ●● | Kullanıcı yorumu |
| Q1 | Modifier-farkında seviye doğrulayıcı | Kalite | M | ●●● | Kod incelemesi |
| F2 | Türkçe dil desteği (yerelleştirme) | Özellik | M | ●● | Mağaza metni TR hazır |

Önerilen sıralama için en alttaki **Versiyon planı**na bakın.

---

## 1. Tasarım ve kullanıcı deneyimi

### D1 — Bulut / arka plan renk kontrastı (S, ●●●)
- **Sorun:** Bulut gövdesi `(0.93, 0.97, 1.0)`, gökyüzü degradesinin üstü `(0.92, 0.97, 1.0)` → neredeyse aynı renk; göz bulutu ayırt edemiyor (kullanıcının en çok dikkatini çeken nokta).
- **Öneri:** Gökyüzünü bir-iki ton koyulaştırmak ve/veya buluta belirgin kenar çizgisi + daha güçlü gölge; boş slotların rengini de koyulaştırmak (şu an `alpha 0.08`). Dünya temasına göre (her dünyanın `SkyColor`'ı var) arka planı renklendirmek.
- **Dosyalar:** `CloudoraVisuals.cs`, `CloudContainerView.cs`.

### D2 — Alt butonların hizalanması (S, ●●●)
- **Sorun:** Butonlar elle verilmiş koordinatlarda: üst sıra −130 / +130 / +390, alt sıra −390 / −155 / +350 → 4'lü ızgarada boşluk kalmış gibi kayık görünüyor.
- **Öneri:** İki sıralı, 3+3 eşit aralıklı `GridLayoutGroup`/`HorizontalLayoutGroup`; güvenli alan ve ekran genişliğine göre ölçeklenen buton boyutları; mümkünse ikon + kısa etiket.
- **Dosya:** `GameplayOverlay.cs`.

### D3 — Menü ekranlarında arkadaki oyunun görünmemesi (S, ●●)
- **Sorun:** Main Menu / Settings / World panelleri ekranın ~%84'ünü kaplıyor (anchor 0.08–0.92); kenarlardan oyun görünüyor.
- **Öneri:** Panellerin arkasına tam ekran, opak (veya güçlü bulanık/karartılmış) bir arka plan katmanı; panel açıkken oyun girdisini de bloklar.
- **Dosyalar:** `MainMenuController.cs`, `WorldScreenController.cs`.

### D4 — World ekranının anlaşılır olması (M, ●●)
- **Sorun:** World butonu düz renkli bir panel, bir dikdörtgen ve "Restoration %X / Stage n/4 / Next rule" metni gösteriyor; ne işe yaradığı anlaşılmıyor.
- **Öneri:** Dünya haritası benzeri ilerleme ekranı: 13 dünyanın listesi (kilitli/açık), mevcut dünyada ilerleme çubuğu, restorasyon aşamalarının görsel olarak "canlanan" bir manzarayla gösterilmesi, sıradaki kuralın kısa açıklaması; "Kapat" butonu (şu an panele dokununca kapanıyor, bu da belirsiz).

### D5 — Titreşim (S, ●●)
- **Sorun:** Her geçersiz hamlede `Handheld.Vibrate()` → Samsung'da 500 ms titreşim (testte 43 kez).
- **Öneri:** Android `Vibrator`/`VibrationEffect` ile 20–40 ms hafif "tık"; başarılı hamle ve bulut tamamlanmasında farklı, çok kısa desenler.

### D6 — Buton geri bildirimi ve hızlı basma koruması (S, ●●)
- **Gözlem:** Restart basıldığında görsel bir onay yok; art arda basmalar (kasıtlı test) her seferinde işlem/analytics üretiyor.
- **Öneri:** Basma animasyonu (ölçek/renk), kısa bir bekleme süresi (debounce), Restart için "emin misin?" yalnızca can harcanacaksa.

### D7 — Tipografi ve görsel cila (M, ●)
- Unity'nin yerleşik `LegacyRuntime.ttf` fontu ve eski `UI.Text` kullanılıyor. TextMeshPro + özel bir font, keskin metin ve Türkçe karakter güvencesi sağlar.
- Hava desenleri metin (`*`, `///`, `+`, `~~~`) — gerçek ikonlar (güneş, yağmur damlası, kar tanesi…) hem renk körlüğü erişilebilirliğini korur hem de çok daha şık görünür.
- Seviye geçişlerinde ve dünya geçişlerinde kısa animasyonlar.

---

## 2. Oynanış, içerik ve zorluk

### G1 — Zorluk ve "kendini tekrarlama" hissi (L, ●●●)
- **Kanıt:** Seviyeler 6–25 sn / 6–13 hamlede çözüldü; tasarım hedefi 30–60 sn (≤20) ve 60–180 sn (21–100). Kullanıcı: "basit bir matematikle aynı veya çok benzer hamlelerle geçilebiliyor".
- **Kök nedenler (koddan):**
  - `LevelGenerator`: hava türü sayısı `3 + (seviye−16)/35` → **seviye 16–50 arası yalnızca 3 tür**, 51–85 arası 4 tür. 3 tür + 2 boş bulut çok dar bir problem uzayı.
  - Tahta çözülmüş halden geriye doğru rastgele karıştırılıyor; başarısız adımlar atlandığı için gerçek karıştırma derinliği hedefin altında kalabiliyor ve çözüm yolları birbirine benziyor.
  - Zorluk puanı yalnızca yüzeysel özelliklerden hesaplanıyor (eleman sayısı, geçişler); gerçek çözüm zorluğunu (minimum hamle, çıkmaz sayısı) ölçmüyor.
- **Öneri:**
  1. Tür sayısını daha erken artırmak (ör. 21'de 4, 41'de 5); boş bulut sayısını zorluk bandına bağlamak.
  2. Basit bir çözücü (BFS/A*) ile **minimum çözüm uzunluğu** ve çıkmaz sayısını hesaplayıp zorluk puanına katmak; hedef bandın altında kalan seviyeleri reddetmek.
  3. Ardışık seviyelerde benzer düzenleri engellemek (tahta "imzası" ile tekrar kontrolü).
  4. Cihazda gerçek süre ölçümleriyle bantları yeniden kalibre etmek (analytics zaten `duration_seconds` topluyor).

### G2 — Modifier dengesi (M, ●●)
- Rainy Coast (21–40) "Next rule: Wind" diyor ama bu aralıkta hiç modifier yok; dünya kataloğu ile üretici uyumlu hale getirilmeli.
- Wind (176–200 ve 200+) çözüm doğrulamasında dikkate alınmıyor (bkz. Q1).

### G3 — Booster ekonomisi (S, ●●)
- Dev build'de sahte ödül anında verildiği için Extra Cloud fiilen sınırsız → testte zorluk yanıltıcı görünüyor. Dev build'e de bir "gerçekçi ödül" modu (bekleme süresi/limit) eklenebilir.
- Extra Cloud seviyeyi ciddi kolaylaştırıyor; seviye başına kullanım sınırı düşünülebilir.

---

## 3. Yeni özellikler

### F1 — Ses efektleri ve müzik (M, ●●●)
- **Durum:** Oyunda **hiç ses yok.** `GameFeedbackService` çalışma anında `AddComponent` ile oluşturuluyor; ses klipleri hiçbir yerde atanmıyor ve `Assets/_Cloudora/Audio` klasörü boş. Ayarlardaki "SOUND: ON/OFF" düğmesi fiilen hiçbir şeyi değiştirmiyor.
- **Öneri:** Seçme, taşıma, geçersiz hamle, bulut tamamlama, seviye tamamlama sesleri (lisanssız/özgün); hafif bir arka plan müziği + ayrı müzik/efekt ayarı. Klipler `Resources` veya sahnedeki bir bileşen üzerinden bağlanmalı.

### F2 — Türkçe dil desteği (M, ●●)
- Tüm arayüz metinleri kod içinde sabit İngilizce. Mağaza metninin Türkçesi hazır. Basit bir anahtar–metin tablosu (EN/TR) ve cihaz diline göre seçim.

### F3 — Oyun modları (L, ●●)
- **Günlük bulmaca:** tarih tabanlı seed ile herkes için aynı günlük seviye (üretici zaten deterministik, altyapı hazır).
- **Zamanlı / hamle sınırlı mod:** "sakin" ana moddan ayrı, isteğe bağlı meydan okuma.
- **Zen / sonsuz mod:** can harcamadan rahat oyun.

### F4 — İlerleme ve motivasyon (M, ●●)
- Seviye sonu yıldız/puan (hamle sayısına göre), istatistik ekranı (çözülen seviye, en iyi süre), başarımlar.
- Seviye seçme ekranı (biten seviyeleri tekrar oynama).

### F5 — İpucu (Hint) booster'ı (M, ●●)
- Bilinen çözüm (`knownSolution`) zaten saklanıyor; bir sonraki doğru hamleyi göstermek düşük maliyetli.

### F6 — Gelişmiş öğretici (S–M, ●)
- Şu an sadece üst kısımda kısa metin ipucu var. İlk seviyelerde parmak animasyonuyla "buna dokun → şuna dokun" yönlendirmesi; yeni modifier ilk kez çıktığında kısa açıklama kartı.

---

## 4. Optimizasyon ve performans

| # | Konu | Efor | Etki |
|---|---|---|---|
| O1 | `PuzzleController.Update` her karede can metni için yeni string oluşturuyor → yalnızca değer değişince güncellemek (GC baskısı). | S | ● |
| O2 | `CloudContainerView.RefreshVisuals` her harekette tüm slotları yeniden boyuyor; `SetModifierState` her çağrıda yeniden çiziyor — değişiklik olduğunda güncelleme. | S | ● |
| O3 | Her Undo/booster/ayar değişikliğinde kayıt dosyası diske yazılıyor (tmp + yedek kopya). Kayıtları biriktirip pause/seviye sonu/belirli aralıkla yazmak. | S | ● |
| O4 | APK boyutu ~45 MB (dev). Kullanılmayan paketler (2D animation/aseprite/psd importer/spriteshape/tilemap, visual scripting, AI navigation, timeline) kaldırılıp boyut ve build süresi küçültülebilir; release'te code stripping seviyesi artırılabilir. | M | ●● |
| O5 | Pil/ısı: oyun statikken 60 FPS hedefi sürekli; boşta `Application.targetFrameRate`'i düşürmek veya `OnDemandRendering` kullanmak. | S | ●● |
| O6 | Profiler ile cihazda ölçüm (CPU/GPU kare süresi, bellek) — şu an ölçülmüş bir FPS verisi yok. | S | ●● |

---

## 5. Kod kalitesi ve test

| # | Konu | Efor | Etki |
|---|---|---|---|
| Q1 | `PuzzleValidator` modifier'ları (Frozen/Locked/Wind) hesaba katmıyor; "10.000/10.000 geçti" sonucu modifier'lı seviyelerin oynanabilirliğini kanıtlamıyor. Modifier kurallarını saf C# çekirdeğe taşıyıp doğrulayıcıyı bunlarla çalıştırmak. | M | ●●● |
| Q2 | Oyun kuralı iki yerde: saf `PuzzleRules`/`CloudState` ve Unity tarafında `CloudContainerView` (`CanReceive`, `TryMoveTopGroupTo`, `IsSolved`). Tek kaynağa indirmek (view yalnızca durumu göstersin) hata riskini azaltır. | M | ●● |
| Q3 | `PuzzleController` 508 satır: oyun akışı, can, booster, reklam, analytics, kayıt bir arada. Sorumlulukları ayırmak (ör. `LevelSession`, `RewardFlow`). | M | ●● |
| Q4 | Tüm arayüz kodla oluşturuluyor ve sahne/prefab yok → tasarım değişikliği için kod düzenlemek gerekiyor. Ana ekranların prefab'a taşınması tasarım iterasyonunu hızlandırır. | L | ●● |
| Q5 | PlayMode testleri yok (release checklist'te de açık). Menü → oyun → seviye tamamlama akışı için birkaç PlayMode testi; ayrıca `SkyGradient` hatası gibi sorunları yakalardı. | M | ●● |
| Q6 | Test/oyun kodu için Assembly Definition (`.asmdef`) yok → derleme süresi ve bağımlılık sınırları. | S | ● |
| Q7 | Ses/titreşim ayarı hem PlayerPrefs'te hem JSON kayıtta; tek kaynak olmalı. | S | ● |
| Q8 | Boş klasörler (`Audio`, `Materials`, `ScriptableObjects`, `UI`) — ya kullanılmalı (ör. seviye/dünya verisini ScriptableObject'e taşımak) ya da temizlenmeli. | S | ● |

---

## 6. Bilinen hatalar

| # | Hata | Efor | Etki |
|---|---|---|---|
| B1 | Night/Fog gizli slotlarda rengi soluklaştırıyor ama desen yazısını (`*`, `///`…) göstermeye devam ediyor → gizli içerik okunabiliyor (`CloudContainerView.cs:203-205`). | S | ●●● |
| B2 | Seviye bittikten sonra Undo yapılabiliyor; yeniden çözülünce `CheckWin` tekrar çalışıyor (çift analytics, reklam sayacı, kayıt). | S | ●● |
| B3 | `RestartLevel` seviye başlangıç zamanını sıfırlamıyor → süre analytics'i yanlış. | S | ● |
| B4 | Rainy Coast "Next rule: Wind" diyor ama modifier yok (G2). | S | ● |

---

## 7. Yayın hazırlığı (mağaza için)
- Upload keystore ve imzalama, gizlilik politikası URL'si + destek e-postası (`CloudoraReleaseConfig.json` boş), gerçek cihaz ekran görüntüleri, Play Console beyanları (Data safety, içerik derecelendirmesi).
- Gerçek reklam (AdMob) ve analytics (Firebase) entegrasyonu ancak sahip onayı ve gizlilik incelemesiyle.
- Fiziksel cihaz QA matrisi (farklı ekran oranları, eski/düşük donanım, uzun oturum).
- Not: Kod ana yazara (yigityvz) ait ve repoda lisans dosyası yok — yayın öncesi netleştirilmeli.

---

## Önerilen versiyon planı

| Versiyon | Tema | İçerik | Tahmini efor |
|---|---|---|---|
| **v2** | Görsel ve UX düzeltmeleri | D1, D2, D3, D5, D6 + hatalar B1, B2, B3 | ~1–2 gün |
| **v3** | Ses ve anlaşılırlık | F1 (ses), D4 (World ekranı), F2 (Türkçe), F6 (öğretici) | ~3–4 gün |
| **v4** | Oynanış derinliği | G1 (zorluk/tekrar), G2, Q1 (modifier-farkında doğrulama), F5 (ipucu) | ~1 hafta |
| **v5** | Yeni modlar ve ilerleme | F3 (günlük bulmaca vb.), F4 (yıldız/istatistik) | ~1 hafta |
| Sürekli | Kalite ve performans | O1–O6, Q2–Q8 — ilgili versiyonların içine dağıtılabilir | — |

Her versiyon: geliştirme → Editor testi → cihaz testi → kullanıcı onayı → `bahadir-claude-versions` dalında commit + etiket + `crVersion.md` kaydı.
