# Cloudora — Proje Hafızası (Claude için oturum başlangıcı)

## 0A. Güncel ekip teslimi — 2026-11-24 16:10 +03

- **Yiğit'in son açık kararı: Bahadır'ın dalı son güncel sürüm.** Yetkili kaynak `cloudora-bahadir@5a838a104cdce93e342c353c930288e08099ceab`; ekip hedefi `yigityvz/cloudora-android/main`. Main'e bu kaynak + yeni ekip yönergesinin gönderimi açıkça istendi. Aşağıdaki eski “takım main'ine dokunulmaz / başka remote'a push” iş akışı bu yetkilendirilmiş teslimi engellemez; otomatik olarak başka repolara push da gerektirmez.
- Önce [CLAUDE](../CLAUDE.md), [GUNCEL_DURUM](GUNCEL_DURUM.md), [EKIP_ONBOARDING](EKIP_ONBOARDING.md), tam dosya haritası ve V41_TASARIM okunur. Eski iOS1ee4334/cloudora-yigit@cc04e5e tabanı Android main için artık zorunlu değildir.
- Oyun dosyaları aynen korunur: No Moves fail, Ay61, Shuffle15, kapasite4/5/6, V41LevelBuilder ve yayımlanmış100 tablo. Bu işlemde yeni oyun özelliği/sürüm etiketi yok. iOS reposu/telefon değişmez.
- Kontrol kapsamı kaynak/varlık/ayar hash eşliği, LFS, okuma haritası ve bağlantılardır. Tarihsel197/194/0/3 testi bugün yeniden çalıştırılmış veya yeni APK kurulmuş sayılmaz.
- Yeni açık görev verilene kadar Atlas/coin sayfaları seçim bekleyen fikirler; oyuncu kaydı ve imzalar korunur. Aşağıdaki24 Ekim bölümünü geçmiş bağlam olarak oku.

---

> Her oturumun başında **ilk iş bu dosyayı oku.** Projeyi anlamak için nelere bakılacağını, hangi kuralların geçerli olduğunu ve işin nerede kaldığını özetler. Ayrıntı her zaman kaynak dosyadadır; burada yazanla kaynak çelişirse **kaynak doğrudur**, bu dosya düzeltilir.
> Son güncelleme: 2026-10-24 11:04 (2026-10-24 docs commit'i ve push öncesi).

---

## 0. Nerede kaldık (2026-10-24) — yeni oturumda önce bunu oku
- **v4.1 = v4 (iOS uyarlaması) üzerine bölümlerin yeni zorlukla yeniden kurulması.** **Kararlar, kurallar ve sonuçlar: `Docs/V41_TASARIM.md`** (önce onu oku); 100 bölümün görsel özeti `Docs/V41_BOLUM_HARITASI.html`. Ayrıntılı konuşma ve gerekçeler `ClaudoraRelease.md` 2026-10-19 17:28 → 23:00.
- **Durum:** v4.1 commit `5cae26a` + etiket `v4.1` (`bahadir-claude-versions`); **2026-10-19 23:03'te iki hedefe push edildi** (`upstream` `cloudora-bahadir` + `origin` `main`, etiket `v4.1`; önceki `6bfd142`). **2026-10-24:** push sonrası belge güncellemeleri + `Docs/TEMA_ATLASI.html` + `Docs/COIN_EKONOMISI.html` tek bir docs commit'inde (bu dosya da içinde; `git log --oneline -3`), kullanıcı onayıyla iki hedefe push edildi (yeni etiket yok). Yeni oturumda `git ls-remote` ile doğrula.
- **Testler:** EditMode **197 / 194 / 0 / 3** (atlanan: 2 grafik gerektiren, 1 elle ölçüm). Kurulum testi ~4–6 dk sürer (100'ün 300.000'lik havuzu); `V3LevelBuildTests` sınıfında `[Timeout(900000)]`.
- **Oyun testleri (2026-10-19):** Editor'de 4 tur (13 + 7 + 6 + 1 bölüm) ve telefonda 7 bölüm; çökme yok. Son tur yorumları: "düşünmeye itti, beğendim", final 100 "zorlandım, doğru tercihleri yapmazsan tuzakta kalıyorsun". Başarısızlık ekranı telefonda görülmedi (No Moves'a düşülmedi).
- **APK:** `Builds/Android/Cloudora-v4.1-qa.apk` ("Cloudora v4.1", `com.yigityvz.cloudora.v41`, QA, 70 MB); telefonda v4'ün yanında. **Ekip için:** `..\APK-ekip\Cloudora-v4.1.apk` + `KURULUM.txt` (v4.1; SHA-256 `767f28c9…`); `Cloudora-v4.apk` önceki sürüm olarak orada.
- **Yeni iş (2026-10-24, fikir aşaması, kod yok):** 101–500 dünyaları — temalar 40'ar (belki sonra 50), her dünyada tema + feature + yeni element, elementler rotasyonla (tahtada en çok 5–6 renk). Fikir sayfası `Docs/TEMA_ATLASI.html` (+ claude.ai Artifact `https://claude.ai/artifact/BF7Zfaht3rRVqXJB1YPcWd`): 18 tema, 91 feature, 19 element × 4 çizim × 4 renk. Kullanıcı seçimini bekliyor (sayfadaki "Seçimlerimi kopyala").
- **Coin çalışması (2026-10-24, fikir, kod yok):** `Docs/COIN_EKONOMISI.html` (+ Artifact `https://claude.ai/artifact/NDx1ocDRYUs1Lo1mfseBYR`): 43 harcama seçeneği, önerilen 8'li set, kazanç önerileri, denge hesaplayıcısı. Reklamla alınanlar bugünkü gibi kalıyor (kullanıcı). Seçim bekliyor.
- **Başka makineden / bulut oturumundan devam (2026-10-24, kullanıcı bilgisayarı kapattı):** repo `bahadir-aktas/cloudora-android` `main` (= `upstream` `cloudora-bahadir`). Orada Unity, testler, APK ve telefon yok → §1 adım 8–9 yapılamaz, kullanıcıya söyle; yalnızca belge/kod/fikir işi. Claude'un yerel kalıcı hafızası orada yok; kuralları §2'den al, ek olarak: Efe'nin APK'ları (`..\Efe - APK lar`) yalnızca incelenir, onaysız hiçbir şey eklenmez; oyuncuya görünen metinde 3–4 seçenek; kullanıcının hızlı art arda basmaları kasıtlı buton testidir. Artifact'ler claude.ai hesabında (linkler yukarıda); güncellemek için `Docs/*.html`'den baştaki/sondaki HTML iskeleti çıkarılıp aynı URL'ye yayınlanır. Push yine her seferinde ayrı onayla.
- **Sıradaki / açık (kullanıcı kararı bekleyen):** Dünya Atlası ve Coin Ekonomisi seçimleri (sayfalardaki "Seçimlerimi kopyala" metni); temalar ve yüksek çözünürlüklü dünya resimleri ("daha sonra"); kurtuluş ekranında üst/alt gri şerit ("şimdilik boş ver"); çıkmazda mantıklı çözümün parlaması (fikir, `V41_TASARIM.md` §11); oyunun hamleleri loga yazması (kalibrasyon için).
- **Cihaz/kayıt durumu:** Editor kaydı 100 bitmiş (coin 7350); test öncesi yedek `%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\yedek-editor-v41-test-oncesi-20261019-1933\` (L81). Editor'de "Azaltılmış hareket" açık (kullanıcı tercihi; `RenderingReuseTests` artık bundan bağımsız). Telefonda v4.1 kaydı 100 bitmiş (telefon testinin logundan; telefon şu an bağlı değil). Unity Editor kapalı. 2026-10-24 oturum başı: testler 197 / 194 / 0 / 3, kalibrasyon v4.1 verisiyle 100/100 aynı.

## 1. Oturum başında okuma sırası
1. **Bu dosya** (önce §0).
2. `ClaudoraRelease.md` (repo kökü) — yapılan her işlemin kaydı, en yeni en üstte.
3. **`Docs/V41_TASARIM.md`** — v4.1 kararları: kurallar, ölçü (iki düşünen oyuncu), takılma deseni, No Moves, puan eğrisi, test sonuçları, ileride yapılacaklar.
4. `Docs/IOS_FARKLARI.md` — v4'ün (iOS'tan birebir) v3.2'den farkları; v4.1'de Ç1 (başarısızlık kuralı) ve Ç3 (Ay 60) değişti (başındaki not).
5. `crVersion.md` (repo kökü) — onaylanmış versiyonlar (v1 … v4.1), **baştan sona** oku.
6. Tarihçe olarak: `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md`, `Docs/V3_SABLON.md`, `Docs/V3_BOLUM_HARITASI.html` (v3.2 bölümleri; v4.1'de hepsi yeniden kuruldu).
7. İşe göre: ilgili kod dosyaları (bkz. §6) ve iOS'tan gelen `Docs/IOS_*.md`.
8. **Çalıştırarak doğrula (kullanıcı kuralı, her oturum):** Unity Editor kapalıyken (kilit: `Temp\UnityLockfile`; açıksa `CloseMainWindow` ile kapat) sırayla:
   - EditMode testleri: `Unity.exe -batchmode -nographics -projectPath <proje> -runTests -testPlatform EditMode -testResults Logs\<ad>.xml -logFile Logs\<ad>.log` → XML'deki `total/passed/failed/skipped`. Beklenen (v4.1): **197 / 194 / 0 / 3**.
   - Bölümlerin ölçümü: `-executeMethod Cloudora.Editor.V3LevelTool.RunCalibrationReportBatch` (`CLOUDORA_V3_CAL` satırları; kilitli kural, düşünen oyuncu 3 hamle). Önizleme: `RunV41PreviewBatch` → `Logs/v41-preview.csv` (veri yazmaz, ~6 dk). Veri üretimi: `GenerateLevelDataBatch` (veri değişmemeli; `GeneratedDataMatchesBuilder` korur). Envanter: `RunV41InventoryReportBatch`.
   - Harita: `python -I Logs/araclar/make_v41_html.py` (önizleme CSV'sinden).
   - Unity yolu: `C:\Program Files\Unity\Hub\Editor\6000.5.9f1\Editor\Unity.exe`.
9. **Editor oyun testi (kullanıcı kuralı, her oturum):** Claude oynayamaz; kullanıcı oynar, Claude hazırlar ve log'dan doğrular.
   - Unity'yi `-logFile <proje>\Logs\Editor.log` ile aç; kaydı yedekleyip (SHA) `currentLevel` / `highestCompletedLevel`'i ayarla (`%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\`, `.json` + `.bak`, `failRecord` boş).
   - Sonra `Logs/Editor.log`'dan doğrula: `[Analytics]` satırları (frame, t), exception; önizleme CSV'siyle (en kısa çözüm, takılmalar) karşılaştır, tabloyu `ClaudoraRelease.md`'ye yaz. **Takılma sınıfını oyuncuya önceden söyleme** (42'de "tuzaksız" demek oyunu etkiledi).
   - **`Logs/Editor.log` Editor her açılışta üzerine yazılır** → testleri `Logs/oyun-testleri/` altına tarihli adla kopyala.
   - Editor açıkken batch, build ve `Assets/` yazma yok; Play'deyken kod değiştirme.

## 2. Çalışma kuralları (kullanıcı: Bahadır; Türkçe konuşur)
- Promptları uygula; **gerçek karar noktalarında onay iste**. Oyuncuya görünen metinler için 3–4 somut seçenek sun.
- **Her işlem** tarih-saatle `ClaudoraRelease.md`'ye, en yeni en üstte, Türkçe. **Saati her zaman `date` / `Get-Date` ile al, tahmin etme** (2026-10-19'da birkaç kez tahmini saat yazılıp düzeltildi). Kişisel yol yazma (`%USERPROFILE%`).
- Uzun işlerde ara ara yüzde ver.
- **Versiyon:** yalnızca kullanıcı onayıyla; `bahadir-claude-versions` dalında commit + açıklamalı etiket, `crVersion.md`'ye giriş. Sıradaki ara sürüm v4.2; bir sonraki major değişiklik v5. `ClaudoraRelease.md` her versiyonla birlikte commit'lenir.
- **Push: her seferinde ayrı, açık onay.** Hedefler: `upstream` (yigityvz/cloudora-android) `cloudora-bahadir` + `origin` (bahadir-aktas/cloudora-android) `main`, etiketlerle. Force yok; takım reposunun `main`'ine dokunulmaz.
- **Her push'tan önce proje hafızasını güncelle** (özellikle §0) ve commit'le; push bu commit'le birlikte.
- Commit'ler değiştirilmez (amend/rebase yok). Çok satırlı mesaj için `git commit -F`.
- Gördüğün her tutarsızlığı/çakışmayı (küçük de olsa) kullanıcıya söyle — **kendi hatalarını da** (2026-10-19: onaylanan düşünme derinliği 3 kodda 2 kalmıştı). İstisna: `paylasim/DEVRALMA.md` (eski, dokunma).
- Kullanıcı testlerinde art arda hızlı basmalar kasıtlı buton testi olabilir — sor. AFK süreleri zorluk yorumuna katılmaz.
- Dış kaynağı (repo, APK) kullanıcı verir; kendin bulup klonlama.
- Oyunda görünen metinde ASCII dışı sembol kullanma; Android'de görünmez → sprite ikon.
- **Satır sonları:** C# dosyalarının çoğu CRLF; Git Bash `sed -i` CRLF'yi LF'ye çeviriyor → C# düzenlemesinde Python (bayt bayt, `\r\n` koruyarak) ya da Edit aracı. Yeni C# dosyaları CRLF. Python'da kullanıcı adındaki "ı" yüzünden POSIX yolu bozulabilir → Windows yolu (`cygpath -w`).

## 3. Kaynaklar — ne nerede yazıyor
| Kaynak | İçerik | Durum |
|---|---|---|
| `Docs/V41_TASARIM.md` | v4.1: amaç, kural değişiklikleri, bölüm yapısı, ölçü, takılma deseni ve aralıkları, No Moves, puan eğrisi, test sonuçları, ileride | **Güncel, v4.1 referansı** |
| `Docs/COIN_EKONOMISI.html` | Coin harcama seçenekleri (8 grup), kazanç önerileri, gerçek para notları, 1–500 denge hesaplayıcısı, çakışmalar | Fikir, seçim bekliyor; asıl kaynak bu dosya (Artifact'e iskelet çıkarılarak yayınlanır) |
| `Docs/TEMA_ATLASI.html` | 101–500 fikirleri: plan (10×40 / 5×40+4×50), 18 tema (yigityvz'in 13 dünyasından 10'u + 8 yeni), feature varyasyonları (hamle/süre/gecikme/kodda hazır), element çizim ve renkleri, rotasyon, çakışmalar | Fikir, seçim bekliyor. Asıl kaynak bu dosya (scratchpad geçici); Artifact'i güncellemek için baştaki/sondaki HTML iskeleti çıkarılıp aynı URL'ye yayınlanır |
| `Docs/V41_BOLUM_HARITASI.html` | 100 bölüm: özet, puan/hedef ve takılma grafikleri, kurallar, filtreli kartlar (tahta çizimi, çerçeve, takılma 3 oyuncu, çıkmaz, No Moves) | Güncel; `Logs/araclar/make_v41_html.py` ile üretilir |
| `Docs/IOS_FARKLARI.md` + `.html` | v4: iOS sürümünün v3.2'den farkları, çelişkiler Ç1–Ç10, ölçüm, telefon testi | v4 referansı (v4.1 notu başında) |
| `Docs/IOS_*.md` (iOS'tan) | yigityvz'in iOS kayıtları | Kaynak belge; aralarında eskimiş ifadeler var |
| `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md`, `V3_SABLON.md`, `V3_BOLUM_HARITASI.html`, `V3_CERCEVE_ENVANTERI.md` | v3/v3.2 tasarımı, şablonu, haritası, çerçeve envanteri | **Tarihçe** (v4.1'de bölümler yeniden kuruldu; envanter eski formülle) |
| `ClaudoraRelease.md` | Her işlemin kaydı; test tabloları, kararların gerekçeleri | Güncel |
| `crVersion.md` | v1 … v4.1 | Güncel |
| `README.md` | Proje tanıtımı | v4.1 maddesi var |
| `Docs/Cloudora 1–100 Şablonu.html` | Eski Faz 1.4 sayfası | **Commit'e alınmadı** (kullanıcı kararı) |
| `Docs/GELISTIRME_ONERILERI.md`, `PROJECT_PROGRESS.md`, `ANDROID_BUILD.md`, `GOOGLE_PLAY_SUBMISSION.md`, `RELEASE_CHECKLIST.md`, `PRIVACY_RELEASE_REVIEW.md` | Eski öneriler, ana yazarın geçmişi, yayın süreçleri | Tarihsel |
| `paylasim/DEVRALMA.md` | Eski devralma notu | **Kullanılmıyor** |
| `Logs/oyun-testleri/` (git dışı) | Tarihli test logları (bkz. §8) | |
| `Logs/araclar/` (git dışı) | `make_v41_html.py` (v4.1 harita); eski v3 betikleri (`make_map.py`, `make_html.py`, `update_sablon.py`, `fix59.py`) | |
| `Logs/v41-preview-*.csv`, `Logs/v41-envanter-*` (git dışı) | v4.1 önizlemeleri (`-1` ilk kurulum, `-2` No Moves 5, `-3` öneri 3, `-4` son hal = veri; `-nm4`, `-nm4b`, `-b2`, `-b2b` simülasyonlar) ve envanterler (derinlik 2/3) | |
| `..\iOS-inceleme\`, `..\Efe-inceleme\` (repo dışı) | iOS reposu kopyası ve ölçümleri; Efe APK incelemesi | Tarihsel kaynak |
| `..\APK-ekip\` (repo dışı) | Ekip için `Cloudora-v4.1.apk` + `KURULUM.txt` (+ önceki `Cloudora-v4.apk`) | v4.1 |
| Hafıza dizini (`%USERPROFILE%\.claude\projects\…\memory\`) | Kullanıcı tercihleri ve kurallar | Her oturumda otomatik yüklenir |

## 4. Proje özeti
- Unity **6000.5.9f1**, Android, dikey, tek parmakla hava sıralama bulmacası. Ana yazar **yigityvz**; Bahadır katkıda bulunan. Tek sahne `Assets/_Cloudora/Scenes/02_Gameplay.unity`; arayüz kodla.
- **Dal:** `bahadir-claude-versions`. Etiketler: v1 (`adaa0b8`), v2 (`61088e4`), v3 (`b0b2f13`), v3.1 (`ae54acc`), v3.2 (`73611d2`), v4 (`0604618`), **v4.1** (`5cae26a`). Hepsi iki repoya push edildi.
- **iOS sürümü:** `yigityvz/cloudora-ios`; v4 bunun `1ee4334`'ünün Android karşılığı.
- **Telefon (Galaxy S24 FE, adb `R5CXA3032LP`):** "Cloudora" (v1), "Cloudora v2", "Cloudora v3", "Cloudora v4", **"Cloudora v4.1"** (`com.yigityvz.cloudora.v41`) ve Efe'nin `claodeera` paketi yan yana.

## 5. Oyun kuralları (v4.1; kod: `Scripts/Core/PuzzleRules.cs`, `CloudState.cs`, `Puzzle/PuzzleController.cs`)
- Hazneler alttan üste dizili hava parçası yığınları; kapasite 4 (1–15), 5 (16–80), 6 (81–100). Renkler: Güneş, Yağmur (1–5), + Kar (6), + Rüzgâr (31, mint), + Ay (61, Frost Peaks ile). Şimşek ve Gökkuşağı 1–100'de yok.
- Hamle: en üstteki aynı renkli grup **sığdığı kadar** taşınır; hedef boş ya da üstü aynı renk. **Tamamlanmış (dolu, tek renk) hazne kilitli** (v4).
- Kazanma: boş olmayan her hazne dolu ve tek renk; 1,78 sn bitiriş efekti, sonra sonuç paneli.
- **Başarısızlık (v4.1, eski kurala dönüş): yalnızca hiç legal hamle kalmayınca** ("No Moves"; tamamlanmış hazne hamle sayılmaz). Hamle olup çözüm yoksa ekran gelmez; oyuncu Geri Al / Karıştır / Ek Bulut kullanır. Ekran: "Yapılacak hamle kalmadı. Baştan dene veya son güvenli hamlene dön." · Yeniden Başlat (−1 can; 1–5 ücretsiz) · **"Reklam İzle ve Devam Et"** · "Devam Et · 500 jeton" → çözülebilen son duruma (`FailRecovery`). Analytics `reason=no_moves`.
- **Yardımcılar:** Geri Al (başlangıç 3, reklam +3), Ek Bulut (başlangıç 1, reklam +1, bölüm başına en fazla 2), **Karıştır 15'ten** (başlangıç 1, reklam +1).
- **Can:** en fazla 5, 30 dk'da 1; bitmemiş bölümden Ana Sayfa −1 can (1–5 dahil). **Coin:** Orta 100, Zor 200, Çok zor 300; devam 500. "Can Kazan · +1": ana sayfa (can 0) ve duraklatma.
- **Dünyalar:** tanıtım 1–14, Yeşil Vadi 15–30, Rainy Coast 31–60, Frost Peaks 61–100; oyun 100'de biter.
- **Öğretici yazılar:** 1–5, 10, 15 ("Yeni: Karıştır bir hazneyi karıştırır."); 16 punto, beyaz, tek satır (sığmazsa 12'ye küçülür); yazılı bölümde tahta yazının altından başlar (`AdaptiveBoardLayout.ReservedTop`).
- Arayüz Türkçe; Ayarlarda "Azaltılmış hareket". Modifier'lar kodda var, 1–100'de kullanılmıyor.

## 6. Kod haritası (`Assets/_Cloudora/`)
| Yer | Ne |
|---|---|
| `Scripts/Core/` | Saf kurallar |
| `Scripts/Puzzle/PuzzleController.cs` | Ana akış; `LoadLevel` (1–15 `AuthoredLevelCatalog.Get`, 16–100 `TryGetBuilt`); `HasLegalMove` (başarısızlık); `IsShuffleUnlocked` |
| `Scripts/Puzzle/CloudContainerView.cs`, `MoveAnimator.cs`, `WeatherMotionLibrary.cs` | Hazne görünümü, renkler, aktarım animasyonu |
| `Scripts/Level/V3LevelPlan.cs` | Parçalar (Intro 1–5, Adaptation 6–14, GreenValley 15–30, RainyCoast 31–60, FrostPeaks 61–100), 5'li bant deseni, `ShuffleUnlockLevel = 15` |
| `Scripts/Level/PuzzleSolver.cs` | Ölçüm çözücüsü `Solve` (kilitli kural varsayılan; puan; düşünmeden / **3 hamle** / **2 hamle** ileri bakan sanal oyuncular; dolaşma ayrı) ve oyun içi `CheckSolvable`; `ReachabilitySearch` (v4'ten, oyunda kullanılmıyor) |
| `Scripts/Level/BoardFrame.cs`, `V3LevelData*.cs`, `AuthoredLevelCatalog.cs` | Çerçeve; seçilmiş 1–100 (`.Generated.cs` araçla, elle değiştirilmez); 1–15 öğretici yazılar |
| `Scripts/Level/CloudShuffler.cs` | Bulut içi Karıştır + `FailRecovery` |
| `Scripts/Level/LevelGenerator.cs` | Eski üretici (101+, oyunda erişilmez) |
| `Scripts/Progression/`, `Scripts/Services/` | Can, coin, yardımcılar, `WorldCatalog`; kayıt, `FailRecord`, reklam, analytics |
| `Scripts/UI/GameplayOverlay.cs`, `AdaptiveBoardLayout.cs`, `MilestoneScreen.cs` … | HUD, başarısızlık paneli, öğretici yazı; tahta yerleşimi (`ReservedTop`); kurtuluş ekranı (can sayacı `SetLives`'tan güncellenir) |
| **`Editor/V41LevelBuilder.cs`** | **v4.1 kurucusu:** plan (renk/kapasite/1 boş/yenilikler/nefes), puan eğrileri, takılma sınıfları ve döngü dağılımları (`TrapPlan`, `CycleAssignments`), aralıklar, düşünen sınırları, derin tuzak (`DeepEnough`), No Moves ≥ 4, havuzlar (2 boş 400; 1 boş 20.000; 5 renk 1 boş 100.000; 100 için ayrı 300.000) |
| `Editor/V3LevelTool.cs` | Menüler/batch: veri üretimi, `RunV41PreviewBatch`, envanterler, kalibrasyon |
| `Editor/LevelCandidateFactory.cs`, `LevelCandidateSelector.cs` | Aday tahtalar (dağınık dahil), ölçüm |
| `Editor/CloudoraBuildTools.cs`, `DesignQuality.cs` | Android build (dev / QA / Play), tasarım görseli ayarı |
| `Tests/Editor/` | `V3LevelBuildTests` (v4.1 kurucu ve veri), `LevelToolTests` (çözücü, sanal oyuncular), `PuzzleFeelTests`, `V31GameplayTests`, `V32*Tests`, iOS'tan gelen testler |
| Kaldırıldı (v4.1; git geçmişinde) | `V3LevelBuilder`, `V3LadderBuilder`, `V3ThemeBuilder`, `CampaignMoonRollout` ve testleri |

## 7. Bölüm tasarımı (v4.1) — özet (ayrıntı `Docs/V41_TASARIM.md`)
- Her 5 bölümde Orta, Orta, Zor, Orta, Çok zor; her döngüde Orta < Zor < Çok zor; aynı bant parça içinde en çok 1,0 geri düşebilir.
- **Puan** = en kısa ×1 + tuzak hamle ×15 + yanıltıcılık ×10 + çıkmaz ×20 + fazla hamle ×1,5 + **takılma ×15**. Eğri (Orta / Zor / Çok zor): 1–5 11,5→12,5 / 13,5 / 14,5 · 6–14 15,5→17 / 17,5→18,5 / 19 · 15–30 18→22,5 / 20,5→24,5 / 20→26, final 29 · 31–60 23→28 / 26→31 / 28→33, final 35 · 61–100 28→36 / 31→39 / 33→41, final 100: 44.
- **Takılma:** 20'de yüksek (1 boş gelir); 21+ her döngüde 2 yüksek, 2 orta, 1 sıfır (rastgele; sıfır Çok zor'da değil); nefes bölümleri 16/31/61/81 sıfır. Aralıklar: yüksek %35→40 … %68→78, orta %12→20 … %35→45, sıfır ≤ %3, final %60–80.
- **Düşünen oyuncular:** 3 hamle ileri bakan ≤ %10 (final ≤ %20; 100 %10–25). 31–60 yüksek takılma ve 60: 2 hamle ileri bakan ≥ %10. 61–100 yüksek: 3 hamle ileri bakan %5–15. 15–30 derin tuzak şartı yok (küçük tahtada derin tuzak = erken No Moves).
- **No Moves:** en erken 4. hamle; 1 boşlu her bölüm No Moves'a çıkabilir. 100: 1 boşlu, gevşetilmez.
- **Son veri:** 85 bölüm hedefe ±1 (en büyük sapma 1,91); gevşetilen 6 (64, 74, 84, 89, 94, 99); No Moves'lu 47 (en erken 4. hamle); 1 boşlu 40; dağınık 6 (16, 31, 61, 81, 82, 86). 100: düşünmeden %78,5, 2 hamle %23,5, 3 hamle %15, 23 hamle.

## 8. Build, test ve log notları
- Build öncesi Unity Editor kapalı. **v4.1 QA:** `GRADLE_USER_HOME=C:\Dev\gradle-cloudora`, `CLOUDORA_DEV_APP_ID=com.yigityvz.cloudora.v41`, `CLOUDORA_DEV_PRODUCT_NAME="Cloudora v4.1"`, `CLOUDORA_BUILD_OUTPUT=Builds/Android/Cloudora-v4.1-qa.apk` → `./Build-Android.ps1 -Mode qa -ProjectPath <proje>` (arka planda, ~5 dk). v4 için `...cloudora.v4`. Build `ProjectSettings.asset`'e keystore/boşluk gürültüsü yazar → commit dışı.
- adb: `...\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe`. Kurulum `adb install` (Güvenli Klasör kullanıcısı 150 için `SecurityException` satırı zararsız). Telefon kaydı: `/sdcard/Android/data/<paket>/files/cloudora-save.json` (+`.bak`): `am force-stop` → `pull` → düzenle → ikisine `push` → `monkey -p <paket> -c android.intent.category.LAUNCHER 1`. Git Bash'te `MSYS_NO_PATHCONV=1`, yerel dosyada Windows yolu.
- **logcat:** `adb logcat -v time -s Unity:V` arka planda dosyaya; Unity batch adb sunucusunu yeniden başlatır → sonra yeniden başlat. Her açılışta zararsız `AssetPackManager` hatası.
- Editor kaydı: `%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\cloudora-save.json` (+`.bak`); değiştirmeden önce `yedek-*`. Editor PlayerPrefs: `HKCU:\Software\Unity\UnityEditor\YigitYvz\Cloudora` (`Cloudora.ReducedMotion` vb.). **Scratchpad geçicidir.**
- **Loglar (`Logs/oyun-testleri/`, 2026-10-19):** `…-2013-editor-v41-kalibrasyon.log` (13 bölüm), `…-2112-editor-v41-test2.log` (+`-2116-…-son`), `…-2216-editor-v41-test3.log`, `…-2242-editor-v41-test4-100-son.log`, `…-telefon-v41-qa.txt`, `…-telefon-v4-qa.txt`; daha eskileri v3.2 dönemi.
- Commit dışı: `ProjectSettings.asset` (build gürültüsü), `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset` (Unity'nin eklediği `rid` satırı), `.vscode/`, `Docs/Cloudora 1–100 Şablonu.html`.

## 9. Açık konular ve bilinen tutarsızlıklar
- **Temalar ve dünya resimleri** (kullanıcı: "daha sonra"): dünya görselleri, arka planlar, tema adları/tasarımı; resimler düşük çözünürlüklü (aşama ~772×1300, S24 FE'de ~1,8× büyüyor) → yigityvz'den Figma 2×/3×.
- **Kurtuluş ekranında üst/alt gri şerit** (Android) — kullanıcı "şimdilik boş ver".
- **Çıkmazı fark ettirme** (`V41_TASARIM.md` §11): oyuncu çözemediğini anlamakta zorlanabilir; ileride mantıklı çözüm parlasın.
- **Hamle logu yok:** oyun hamleleri tek tek yazmıyor; kalibrasyon için eklenebilir.
- **Gevşetilen 6 bölüm** (5 renk): takılma aralığının biraz altında / düşünen sınırının biraz üstünde; havuz büyütülerek azaltılabilir.
- **Başarısızlık ekranı** v4.1 metinleriyle telefonda görülmedi (Editor'de görüldü).
- v3'ten kalanlar: Shuffle çözücüsünün büyük tahtada 200.000 sınırı (ölçümde sorun yok), World ekranı / Shuffle seçim modu tartışması, zorluk sembolü sprite değil yazı.
- **Zararsız kalıntılar:** `PuzzleSolver.ReachabilitySearch` (oyunda kullanılmıyor, testleri var), `LevelCandidateSelector` `backupCount`, `LevelDefinition.difficultyBand`.

## 10. Bu dosyanın bakımı
Önemli bir karar, yeni bir kaynak doküman, parça tamamlanması ya da oturum kapanışında bu dosya (özellikle §0) güncellenir ve değişiklik `ClaudoraRelease.md`'ye yazılır.
