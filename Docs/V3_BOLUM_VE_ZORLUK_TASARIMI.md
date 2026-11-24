> 19 Ekim 2026 güncellemesi: Ay artık beşinci element olarak 60. bölümde başlar. Bu dosya tarihsel kalibrasyon kararlarını içerir; güncel yayımlama kuralı ve doğrulama: [IOS_MOON_FROM_60.md](IOS_MOON_FROM_60.md).

# v3 — Bölüm ve Zorluk Tasarımı (1–100)

Kullanıcının (Bahadır) 2026-10-14'te verdiği tasarım tanımı ve soru-cevaplar. v3 çalışmasının referansıdır; kararlar netleştikçe güncellenir.

---

## 1. Amaç
- İlk 100 bölüm, zorlukları yeniden düzenlenerek **yeniden oluşturulacak**.
- **Oyunun yapısı ve mantığı değişmeyecek.** Değişen şey zorluğun bölümlere dağıtılması: eskiden fiilen rastgeleydi (bant sabit sırayla verilse de rastgele karıştırma yüzünden gerçek zorluk tutmuyordu), artık her bölümün zorluğu belirli olacak.
- v3 yalnızca bu zorluk çalışmasıdır; başka major geliştirme eklenmeyecek.

## 2. Zorluk deseni
Zorluk **her 5 bölümde bir** aynı sırayla tekrar eder. **"Kolay" yoktur; en düşük seviye "Orta"dır.**

| 5'li gruptaki sıra | 1. | 2. | 3. | 4. | 5. |
|---|---|---|---|---|---|
| Zorluk | Orta | Orta | Zor | Orta | Çok zor |

Döngüler: 1–5, 6–10, 11–15, 16–20, … (her döngünün son bölümü "Çok zor").

**Zorluk göreli ve birikimli:** Oyun ilerledikçe ve temalar değiştikçe her bant önceki bölümlerdekinden daha zor olur. Baştaki "Zor", ileride bir bölümde "Orta" seviyesine denk gelebilir. Baştaki "Orta" aslında o kadar zor değildir; kullanıcı sorunsuz geçebilir. (İleride ayrıca tutorial eklenecek; bu yüzden "Kolay" olmaması sorun değil.)

> **Her zaman hatırlanacak ilke (kullanıcı):** Oyun ilerledikçe zorluk etiketleri temalar arasında **aynı şeyi ifade etmez.** Her parçanın kendi Orta/Zor/Çok zor ölçeği vardır ve bu ölçekler **dengeli** kurgulanmalıdır:
> - Genel zorluk parçadan parçaya kademeli artar; parça geçişlerinde ani sıçrama veya düşüş olmaz (ör. yeni parçanın ilk "Orta"sı, öncekinin "Çok zor"undan belirgin şekilde zor olmaz).
> - Bir parçanın "Orta"sı, önceki parçaların "Orta"sından zordur; ileride bir "Orta", baştaki bir "Zor" seviyesine ulaşabilir.
> - Parça içinde de hafif bir artış olabilir; desen (Orta, Orta, Zor, Orta, Çok zor) her yerde korunur.
> - **Alışma etkisi (kullanıcı notu, 2026-10-14):** Oyuncular oynadıkça alışır; aynı ölçülen zorluk ilerleyen bölümlerde daha kolay hissettirir. Parçalar arası artış bu alışmayı da hesaba katmalı.
> - **Artış hızı (kullanıcı, 2026-10-14):** Oyunun başında radikal zorluk artışı olmasın; ileriki parçalarda yavaş yavaş artışlar konur, parçaların kendi içinde de zorlaşır.

## 3. Yöntem (kullanıcı seçimi)
**Tablo + ölçülmüş, seçilmiş seed:**
- Her bölüm için parametreler (kapasite, hava türü sayısı, boş bulut, hedef zorluk) bir tabloda tanımlanır.
- Editor aracı her bölüm için çok sayıda aday tahta üretir; bir çözücü her adayın gerçek zorluğunu ölçer (en kısa çözüm uzunluğu, çıkmaza götüren hamleler vb.).
- Hedef zorluğa en uygun adayın seed'i o bölüme sabitlenir.
- **Zorluk ölçütlerini Claude belirler**, kullanıcı test eder; geri bildirime göre revize edilir.

### Araç (2026-10-14, testli)
- Kod (oyunla birlikte derlenen): `Scripts/Level/PuzzleSolver.cs`, `CloudShuffler.cs`, `DeterministicRandom.cs`, `BoardFrame.cs`, `V3LevelPlan.cs`, `V3BoardText.cs`, `V3LevelData*.cs`. Yalnızca Editor (APK'ya girmez, 2026-10-15'te taşındı): `Editor/LevelCandidateFactory.cs`, `Editor/LevelCandidateSelector.cs`, `Editor/V3LevelBuilder.cs`, `Editor/V3LevelTool.cs`; testler `Tests/Editor/LevelToolTests.cs`.
- Ölçütler: en kısa çözüm (hamle), ulaşılabilir durum sayısı, çıkmaz durum oranı, en kısa yollar üzerinde ilerleten hamle oranı ve tuzak (çıkmaza götüren) hamle oranı, ortalama seçenek; bunlardan birleşik **puan** (ağırlıklar ilk ayar, testlerle kalibre edilecek).
- Sınırlar: modifier'sız tahtalar (1–100 kararıyla uyumlu), en fazla 12 bulut; çok büyük tahtalar durum sınırını aşarsa "ölçülemedi" sayılır ve seçilmez.
- İlk rapor bulguları: 2 boş bulutlu çerçevelerde çıkmaz/tuzak hamle oluşmuyor (zorluk yalnızca çözüm uzunluğu ve yanıltıcılıktan); 1 boş bulutta tuzaklar ve çözümsüz tahtalar ortaya çıkıyor (çözümsüzler araçta elenir). 1–5 çerçevesinde (kapasite 4, 2 renk, 2 boş) yalnızca 37 farklı tahta var; çözüm uzunluğu 3–7 hamle.

## 4. Bölüm aralıkları ve parçalar
İlk 100 bölüm **parça parça** oluşturulacak; parçalar aralıklara göre ayrılır. Hangi bölümün hangi aralıkta olduğu şimdiden sabit; tema adları, tasarımları ve temaya özel özellikler sonradan eklenecek (adlandırmalar geçici).

| Parça | Bölümler | Geçici ad | Amaç |
|---|---|---|---|
| 1 | 1–5 | Tanıtım | Oyunun tanıtımı; zorlukların gösterilmesi. Tema sayılmaz. |
| 2 | 6–15 | Alışma | Kullanıcının iyice alışması; tutorial'ın tamamlanması. Tema sayılmaz. |
| 3 | 16–30 | Geçiş teması | İlk temanın gelişi; temaları tanıtır ve öğretir. **Shuffle 16'da açılır.** |
| 4 | 31–60 | Tema 2 | İlk gerçek tema; ona göre özellikler (belirlenmedi). |
| 5 | 61–100 | Tema 3 | İkinci tema; ona göre özellikler (belirlenmedi). |

- Dünya kataloğu teknik olarak her bölümün bir dünyaya ait olmasını ister: 1–15 için "Tanıtım" adlı geçici bir kayıt kullanılır (oyuncuya tema olarak sunulmaz), diğerleri yukarıdaki geçici adlarla.
- Her aralık 5'in katında bittiği için her parçanın son bölümü "Çok zor"dur.

## 5. Korunacak mevcut davranışlar
- **L1–5'te Restart can götürmez** (şu anki kural, kalır).
- **Shuffle 16. bölümde açılır** (şu anki gibi).
- Extra Cloud seviye başına en fazla 2, No Moves, can sistemi vb. mevcut kurallar değişmez.
- Çakışma çıkarsa kullanıcıya sorulur.

## 6. Shuffle — yeni tasarım: bulut içi karıştırma (karar, 2026-10-14)
- **Neden:** Kullanıcıya göre ana yazarın asıl amacı bir bulutu kendi içinde karıştırmaktı. Kodda (orijinal `a18a3e4` dahil) Shuffle bütün tahtayı yeni seed'le yeniden üretiyordu; bu karışıklık v2 ve v3 kararlarına taşındı. Yeni tasarımla değiştirilecek.
- **Davranış:**
  1. Shuffle'a basılınca oyun bir bulut seçilmesini ister.
  2. Shuffle'a basıldığında zaten seçili bir bulut varsa o seçim kaldırılır.
  3. Seçilen bulut kendi içinde karıştırılır; oyun aynı tahtayla devam eder (bulut sayısı ve diğer bulutlar değişmez).
- **Shuffle 16. bölümde açılır** (§5, değişmedi).
- **Geçersiz kalan eski kararlar:** her bölüm için yedek seed (bölüm tahtası artık değişmiyor), "Shuffle'da ekstra bulutlar boş kalsın" ve "Shuffle ekstra bulutlara da dağıtır" (v2), Shuffle sayısını yedek seed sayısıyla sınırlama notu. İlgili kodlar kaldırılacak/geri alınacak.
- **Kurallar (kullanıcı yanıtları, 2026-10-14):**
  1. **Güvenli:** Karıştırma sonucu tahta mutlaka çözülebilir kalır (çıkmaza sokan sonuç kabul edilmez) ve bulutun sırası mutlaka değişir (oyuncu hakkının karşılığını almalı).
  2. **Seçilemeyen bulutlar:** boş, tek renkli, tek elemanlı (karıştırılınca değişmeyenler) seçilemez; hak düşmez.
  3. **Hak ve iptal:** Hak, bulut seçilip karıştırma yapılınca düşer; seçim modundayken Shuffle'a tekrar basmak veya boş yere dokunmak iptal eder; ekranda "bir bulut seç" yazısı çıkar.
  4. **Undo:** Shuffle Undo ile geri alınamaz. Shuffle'dan sonra Undo geçmişi temizlenir ve Undo butonu söner (eski Shuffle'daki gibi); sonraki hamleler yine geri alınabilir.
  5. **Açılma koşulu:** bölüm ≥ 16 (önceki "üretilmiş bölüm" koşulu yerine; Shuffle olmayan bölümler karışmasın, kod daha temiz).
  6. **Özel bulutlar:** Modifier'lı bulutlar (Frozen, Locked vb.; 101+) Shuffle'dan etkilenmez — dokunulmaz.
  - Restart'la ilgili sorular (Restart'ın döneceği tahta, karıştırmanın hamle sayılması) kullanıcı Restart'ı kaldırmayı düşündüğü için beklemede.

## 6b. v3.1: Restart kalkıyor, "No Moves = başarısızlık", coin (karar, 2026-10-14)
- **Sürüm:** Shuffle'ın yeniden tasarımı (§6), Restart'ın kaldırılması, başarısızlık ekranı ve coin **v3.1** olarak şimdi yapılır ve commit'lenir; push kullanıcı onayıyla, vakti gelince.
- **Restart:** Restart ve ona bağlı, başka şeyi etkilemeyen bütün özellikler kaldırılır (buton, can rozeti, ilgili mesajlar, Restart'ın ekstra bulutları koruması, `level_restarted`). **Can sistemi kalır.**
- **No Moves = bölüm başarısız** (tetikleyici değişmez: hiç legal hamle kalmaması). Bu ekranda Undo / Extra Cloud / Shuffle sunulmaz. Üç seçenek:
  1. **Can:** 1 can kaybederek bölüme sıfırdan başlamak (ekstra bulutlar silinir).
  2. **Reklam:** reklam izleyerek devam.
  3. **Coin:** coin vererek devam.
  - "Devam" = **çözüme ulaşılabilen son duruma** dönmek (gerekirse birkaç hamle geri); orada en az bir doğru yol mutlaka vardır.
- **Hamle yapılabiliyor ama kazanılamıyorsa** (çıkmaz, No Moves tetiklenmez): oyuncu Undo / Shuffle / Extra Cloud kullanabilir veya ileride eklenecek "menüden bölümü baştan başlat" seçeneğini kullanır. Not: Hakları ve reklamı olmayan oyuncu, menü seçeneği eklenene kadar bu durumda takılabilir.
- **1–5 (tutorial):** Bölüme başlamak ve "can ile baştan başlamak" can götürmez.
- **Reklamla can alma:** başka bir yere taşınacak (ileride). Restart üzerinden olan yol kalkar.
- **Coin (ilk ayar, sonra düzenlenecek):** bölüm sonunda kazanılır; başlangıç coini ileride tutorial görevinde verilecek (şimdilik 0).
- **Buton düzeni:** üst sıra Extra · Undo · Shuffle; alt sıra World (Extra ile Undo'nun tam ortası) · Menu (Undo ile Shuffle'ın tam ortası); satırlar hizalı.

## 7. Modifier'lar
- **İlk 100 bölümde şimdilik modifier olmayacak** (daha stabil çalışmak için). Temaya özel özellikler belirlenince eklenecek.
- Mevcut modifier kuralları kodda kalır (`Modifiers/`), yalnızca 1–100'e atanmaz. Eski atamaların arşivi:

| Eski aralık | Eski dünya | Modifier | Parametre (`LevelGenerator.ModifiersFor`) |
|---|---|---|---|
| 21–40 | Rainy Coast | (ekranda "Wind", ama atanmıyordu) | — |
| 41–60 | Frost Peaks | Frozen | son bulut, `counter = 2`; ayrıca bu aralıkta boş bulut +1 ve son bulut geri-karıştırmaya katılmaz |
| 61–80 | Sun Desert | Locked | son bulut, `parameter = 0`; boş bulut +1, son bulut karıştırmaya katılmaz |
| 81–100 | Storm Islands | Fog | ilk bulut, `counter = 1` |
| 101–150 | Blossom Land / Ember Valley | Rainbow | ilk bulut; ayrıca ilk bulutun en üst slotuna 1 Rainbow parçası |
| 151–175 | Moon Garden | Night | ilk bulut |
| 176–200 | Sky Kingdom | Wind | ilk bulut |
| 201+ | Ocean → Celestial | Karma | çift → Fog, tek → Night (`counter = 1`); 3'e bölünen → Rainbow, diğer → Wind (bulut 1) |

Eski kapasite/tür eşikleri (arşiv): kapasite ≤20: 4, ≤60: 5, ≤125: 6, sonrası 7; hava türü sayısı `3 + (bölüm−16)/35` (51'de 4, 86'da 5, 121'de 6); boş bulut 2, sonu 9 olanlarda 1.

## 8. 101 ve sonrası
Şimdilik odak ilk 100 bölüm; 101+ mevcut sistemle kalır, ileride ayrıca ele alınacak.

## 9. Zorluk sembolü (kullanıcı önerisi)
- Bölümün zorluğunu gösteren işaret/sembol **v3'e dahil**, ancak bölümlerin zorluk çalışması tamamlandıktan sonra yapılacak.
- Unicode sembol değil, sprite ikon kullanılmalı (Android'de yerleşik fontta görünmüyor — bkz. `CloudoraVisuals.Heart`).

## 10. Çalışma ve sürüm
- Ayrı dal yok: çalışma doğrudan `bahadir-claude-versions` üzerinde.
- Unity ve telefon testlerinin tamamı başarıyla geçerse **v3** olarak commit + etiket; push yine kullanıcıya sorularak (iki hedef: `upstream cloudora-bahadir` + `origin main`).

## 11. İlk 15 bölüm (karar: aynı çerçevede tahtaları yeniden seçmek)
- Kurallar ve çerçeve aynı kalır: elle tasarlanmış bölüm formatı, kapasite 4, renk sayısı (1–5: 2, 6–15: 3), öğretici yazılar ve bölüm numaraları, tutorial kayıtları, Restart/Shuffle kuralları.
- Değişen yalnızca tahtaların içeriği: kurallar aynı tutularak çok sayıda aday denenir, zorlukları ölçülür ve desene uyanlar seçilir.
- Seçilen tahtanın, öğretici yazının gösterdiği şeyi gerçekten içerdiği de kontrol edilir (ör. 3. bölümde grup hareketi).
- **Boş bulut (karar, 2026-10-14):** 1–15'te hep 2 boş bulut (çerçeveye dahil). 1 boş bulut kademeli olarak geçiş temasında (16+) gelir. Bu yüzden 1–15'te zorluk farkı çözüm uzunluğu ve yanıltıcı hamlelerden gelir (2 boş bulutta çıkmaz oluşmaz); kabul edildi.

## 11b. 16–30 çerçevesi (karar, 2026-10-15)
- **Kapasite eski çerçeveyle korunur:** 16–20'de 4, 21–30'da 5 (eski `LevelGenerator` eşiği).
- **Boş bulut kademeli iner:** 2 → 1, parça boyunca 1 boş bulut giderek sıklaşır (eski çerçevedeki yalnızca 19 ve 29'da 1 boş bulut düzeni kullanılmaz). Kullanıcı: "Bunu güzel tasarlarsak oyunun zorluğu ve kullanıcıyı çekmesi daha güzel oturur."
- **Tasarımda netleşecek:** renk sayısı (eski çerçevede 3); 1 boş bulutun hangi bölümlerde başlayıp nasıl sıklaşacağı; 21'deki kapasite artışı (4 → 5) ile boş bulut azalmasının üst üste binmemesi (sıçrama riski); çerçeve değiştiğinde puanların karşılaştırılması (bkz. `PROJE_HAFIZASI.md` §9).
- Bölüm tasarımına kullanıcı onayıyla geçilecek.

## 11c. Çerçeve merdiveni: hava türleri, kapasite, boş bulut (karar, 2026-10-15)
- **Çerçeveyi araç seçer, zorluk puanına göre:** renk (hava türü) sayısı, 1 boş bulutun yeri/sıklığı ve geçişler aracın puan hedefine göre yerleştirilir. Aracın bu şekilde değiştirilmesi planlandı (henüz yapılmadı).
- **Geri düşmeme:** Kapasite ve hava türü sayısı bir kez arttı mı geri düşmez.
- **İstisna — boş bulut "nefes alır":** Yeni bir hava türü geldiğinde boş bulut geçici olarak 2'ye dönebilir, sonra yine kademeli 1'e iner. Amaç kademeli artışı korumak ve çeşitlilik.
- **Hava türleri 1–100'e kademeli yayılır.** Oyunda zaten 6 tür var (Güneş `*`, Yağmur `///`, Kar `+`, Rüzgar `~~~`, Ay `)`, Şimşek `!!`) + Gökkuşağı joker. Kabaca dağılım (kullanıcı "genel anlamda güzel" dedi; çerçeveyi çoğunlukla koruyarak kademeli artış için değişiklik gerekirse **kullanıcıya sorulur**):

| Bölümler | Parça | Tür | Kapasite | Boş bulut |
|---|---|---|---|---|
| 1–5 | Tanıtım | 2 | 4 | 2 |
| 6–15 | Alışma | 3 (+Kar) | 4 | 2 |
| 16–30 | Geçiş teması | 3 | 4 → 5 (21) | 2 → 1 kademeli |
| 31–60 | Tema 2 | 4 (+Rüzgar) | 5 | yeni türle 2, sonra kademeli 1 |
| 61–100 | Tema 3 | 5 (+Ay), belki 6 (+Şimşek) | 5 → 6? | yeni türle 2, sonra kademeli 1 |

- **Kapasite geçiş noktası (2026-10-15 10:48):** 21 sabit değil; araç zorluğa göre seçer (geri düşmeme kuralıyla). Bu, §11b'deki "21–30: 5" ifadesinin yerini alır: kapasite 5'e 16–30 içinde aracın seçtiği bölümde çıkar.
- **Yenilik kuralı (karar):** Bir bölümde yalnızca tek yenilik (kapasite artışı, yeni tür, boş bulut azalması) gelir; yeni bir öğe ilk kez bir **Orta** bölümde çıkar.
- **Renkler (karar, v3.2'de uygulanacak):** Kar → cyan (kar rengi); Rüzgar → gümüş grisi (beyaz bulut gövdesinden ayrılsın); Şimşek → gece mavisi, Yağmur'dan ayırt edilecek kadar koyu; koyu parçalarda desen yazısı beyaz.
  - **Seçilen tonlar (2026-10-15, maket "Cloudora Hava Renkleri"):** Kar **#7FE3F5** (buz cyan) · Rüzgar **#AEB7C2** (gümüş) · Şimşek **#25358A** (lacivert, beyaz desen). Diğerleri aynı: Güneş #FFC226, Yağmur #338CFF, Ay #8F7DF2, Gökkuşağı #F28CC7 (`CloudContainerView.GetColor`).
- **v3.2 testi baştan başlar:** renkler 1–15'i de etkilediği için v3.2'nin Editor/telefon testinde kayıt sıfırlanıp 1. bölümden başlanacak (kayıt önce yedeklenir).

## 11e. v3.2 çalışma planı (karar, 2026-10-15 11:00)
- **1–15 sabit kalır:** tahtaları yeniden üretilmez; yeni araç mantığı (çok çerçeveli seçim, mutlak hedef puan) yalnızca 16+ için çalışır. 1–15'in puanları 16–30'un başlangıç noktasıdır; geçiş yumuşak olmalı ve kademeli artış sürmeli.
- **Hedef eğrisi:** ani artış yok, aşırı yavaş da değil — dengeyi Claude önerir. Eğri bölüm bölüm tablo (ve grafik) olarak kullanıcıya gösterilir; **onaydan sonra** üretim yapılır.
- **Sıra (her adımdan önce kullanıcıya sorulur):** (1) renk değişikliği → (2) aracın değiştirilmesi (çerçeve merdiveni, geri düşmeme, boş bulutun nefes alması, tek yenilik kuralı, mutlak hedef puan, aykırı değer eleme; önce testler) → (3) hedef eğrisi tablosu ve onayı → (4) 16–30 üretimi + dünya listesi (16–30 Green Valley, 1–15 "Tanıtım") → (5) kayıt sıfırlanıp L1'den Editor testi, sonra telefon testi.

## 11f. Çerçeve merdiveni aracı — uygulanan kurallar (2026-10-15, önizleme kullanıcı onayı bekliyor)
Kod: `Editor/V3LadderBuilder.cs` (1–15 eski yoldan, değişmeden). Önizleme: **Cloudora/V3/Önizleme: Geçiş Teması (16–30)** → `Logs/v3-preview-TransitionTheme.csv`; veri dosyasına yazmaz.
- **Hedef eğrisi:** her bandın başlangıcı önceki 10 bölümdeki en yüksek puanı (6–15: Orta 16,80 · Zor 18,79 · Çok zor 21,35); her 5 bölümde **+1,0** (`GrowthPerCycle`). Döngüdeki üç Orta kendi içinde 1/3'er artar. 16–30 hedefleri: Orta 17,13 → 19,80; Zor 19,79 → 21,79; Çok zor 22,35 → 24,35.
- **Seçim:** her bölüme, izin verilen çerçevelerin adaylarından hedefe en yakın tahta (eşitlikte 2 boş bulut, sonra küçük seed). Döngü sırası, aynı bandın kolaylaşmaması ve 1–30 boyunca tahta tekrarı olmaması seçim sırasında zorunlu.
- **Yeniliklerin yeri:** kapasite artışı ve ilk 1 boş bulut için bütün Orta bölüm yerleşimleri denenir, hedeften toplam sapması en küçük plan seçilir. 16 ayrılmış (Shuffle + tema). İlk 1 boş bulut son döngüden önce gelir.
- **Sıklaşma:** 1 boş bulut geldikten sonra her döngüde öncekinden en az bir fazla 1 boşlu bölüm.
- **Aşırı tahtalar:** sanal oyuncunun %80'inden fazlasının takıldığı tahtalar elenir (ilk denemedeki %50 eşiği 1 boş bulutun tuzaklı tahtalarını da eliyordu).
- **Henüz yok:** yeni hava türü adımı ve boş bulutun "nefes alması" (Tema 2 ile eklenecek).

## 11g. v3.2'de düzeltilecek: bilgi pencereleri butonları kapatmalı (kullanıcı, 2026-10-15) — **uygulandı 12:30** (`GameplayOverlay` "ButtonShield"; test `V32InputTests.InfoPopupLocksBottomButtonsWithoutCoveringThem`; oyunda görsel kontrol bir sonraki Editor testinde)
- **Sorun:** "Undo Refilled / Reward granted" gibi bilgi pencereleri açıkken alttaki butonlar (Extra, Undo, Shuffle, World, Menu) kullanılabiliyor.
- **Neden (kod):** `GameplayOverlay.CreateBlockingPanel` karartmayı alttan butonların yüksekliği kadar boş bırakıyor (`bottomInset`). Bu, v2'de bilerek yapılmıştı ("mesaj o butonları kullanmayı söylüyor").
- **Karar (kullanıcı düzeltmesiyle, 11:34):** Mesajlar butonları **görsel olarak örtmez** (karartma yine butonların üstünde biter), ama pencere açıkken butonlar **çalışmaz**: buton alanının üstünde görünmez bir dokunma katmanı olur; oraya (veya pencereye) dokunmak yalnızca pencereyi kapatır, butonun işini yapmaz. Pencere kapandıktan sonra butonlar normal çalışır. Örnek: "Undo Refilled" açıkken Undo'ya basmak hak harcamaz, yalnızca pencereyi kapatır. Kapsam: `ShowBlock` ile açılan bütün bilgi pencereleri. Test oturumu bitmeden koda dokunulmaz.

- **Zamana bağlı "hızlı basma koruması" eklenmez (kullanıcı, 12:28):** spam denemesinde gerçek basışlar 0,146 sn arayla geldi; koruma onları da yutar. Sorun yalnızca bu pencere düzeltmesiyle çözülür. (Kanıt: Game penceresinde 19 basış = 19 işlem, girdi çift işlenmiyor.)
- **Aynı sorun Extra'da (kullanıcı, 11:50):** "Extra Cloud Refilled" penceresi açıkken Extra'ya tekrar basmak hem pencereyi kapatıyor hem bulutu ekliyor. İstenen: ilk basış yalnızca pencereyi kapatır; kullanmak isteyen tekrar basar. (Yukarıdaki karar bunu da kapsar.)

## 11h. Tartışılacak / iyileştirilecek arayüz notları (kullanıcı, 2026-10-15 Editor testi)
- **World ekranı:** World açıkken ortaya (panele) dokunmak ekranı kapatıp bölüme döndürüyor (kod: panel ve arka plan `Toggle`). World'ün kapsamı değerlendirilirken tartışılacak.
- **Shuffle seçim modu daha belirgin olmalı:** şu an yalnızca turuncu "Select a cloud to shuffle" yazısı var.
- **Kar rengi** 6. bölümde yeni haliyle görüldü (doğrulandı).

## 11i. Editor testi sonrası kararlar (kullanıcı, 2026-10-15 11:56)
- **(a) "No Moves" kontrollü tasarlanır ve bölümlere yayılır:** Oyuncu her zaman karşılaşmaz ama zaman zaman karşılaşır — yenilgiyi tatsın, özellikleri (Undo/Shuffle/Extra, devam) kullansın, birikimini (coin) harcasın. Araca ölçüt eklenecek (başlangıçtan kaç hamlede hiç hamle kalmayan duruma düşülebildiği); yenilik bölümünde 1–2 hamlede başarısızlık olmamalı (Editor testinde 24'te ilk hamlede mümkündü).
- **(b) Çok zor'lar biraz daha zor olmalı, ama "Çok zor = 1 boş bulut" kısıtı konmaz.** 2 boş bulutlu Çok zor'u (ör. 25, 9,9 sn) başka yollarla zorlaştırmanın yolu bulunacak.
- **(c) Kalibrasyon için telefon testi de yapılacak;** sanal oyuncu ağırlıkları ona göre değerlendirilecek.
- **Netleşme (12:01):**
  - Çok zor için **daha uzun ve yanıltıcı tahta** tercih edilir (uzun vadede de işe yarar).
  - Hedef puanı ayırmak tek başına çözüm değil ("puanda zor ama gerçekte kolay" bölümler doğabilir); yalnızca Çok zor'ların döngü başı artışı **1,0 yerine 1,2** olabilir.
  - **No Moves belirli bir bölüm tipine bağlanmaz,** bölümlere karışık yayılır; sürpriz olmalı, oyuncu hangi bölümlerde No Moves olmadığını çıkaramamalı.
  - Sıra: önce telefon testi (16–30, telefondaki v3 kaydından devam, sıfırlama yok), sonra araç bu kararlara göre revize edilir.
- **Telefon testi sonrası netleşme (12:21):**
  - **(a) No Moves:** öneri uygun. 3 renk + 2 boşta No Moves imkânsız olduğundan başlarda yalnızca 1 boşlu bölümlerde olur; yeni renk gelince kısıt kalkar ve dağılım serbestleşir — endişe gerekmez. **24'te (yenilik bölümü) No Moves çıkabilmesi sorun değil** — önceki "yenilik bölümünde 1–2 hamlede başarısızlık olmasın" önerisi geçersiz.
  - **(b) Uzun ve yanıltıcı tahta tercihi yalnızca Çok zor için değil, Orta ve Zor için de** (monotonlaşmayacak düzeyde). Aralarda kolaylar olsun; Çok zor'la farkları belirgin kalsın. Orta'larda artış sağlamakta sorun çıkarsa ileride ayrıca düşünülür.
- **Araç revizyonu kararları (12:37):** Veri, 2 boşlu tahtaların "yanıltıcılık" ölçütünün zaten yüksek olduğunu ve insan için kolay 25'i ayırmadığını gösterdi; bu yüzden:
  1. **Uzunluk tercihi:** hedefe yakın adaylar arasında en kısa çözümü uzun olan; Çok zor'da güçlü, Zor'da orta, Orta'da hafif; döngünün ilk Orta'sında tercih yok (aralarda kolaylar). Yanıltıcılık kaba filtre olarak kalır.
  2. **Çok zor'ların döngü başı artışı 1,2** (diğer bantlar 1,0).
  3. **No Moves dağılımı:** parça başına belli aralıkta (16–30: 3–4 bölüm), bantlara dağıtılmış (hepsi aynı bantta olmaz).
  4. **Çok zor'da 1 boş bulut hafifçe öne alınır, tamamen değil** (parçanın Çok zor'larından en az biri 2 boşlu kalır).
- **Kalibrasyon ve tema finali (13:12):** Çıkmaz ağırlığı 5 → 20 (Editor testi). 24/25/28 bilerek No Moves denemeleri dışında "normal". **30 gibi çok zor bir bölüm temanın son bölümlerinde gelebilir, ama her Çok zor böyle olursa sorun** — tema finali dışındaki Çok zor'lar 30 kadar ağır olmamalı. **Uygulandı (13:16):** parçanın son bölümü dışında sanal oyuncu takılması < 0,5 (`MaxNaiveFailureBeforeFinale`).
- **Kademeli 1 boş bulut düzeltmesi (kullanıcı onayı, 13:29):** 13:16 planı fazla 1 boşlu üretti (son 7'nin 5'i). Yeni kurallar: giriş döngüsünde yalnızca yenilik bölümü, sonra her döngüde tam bir fazla; **ilk temada döngü başına en fazla 2** (16–30: 22; 26 ve 30); son döngüde bir 1 boşluk hakkı **tema finaline** ayrılır (final "sınav"). Bu yüzden No Moves aralığı 16–30'da **2–3 bölüm** (yalnızca 1 boşlularda mümkün).

## 11j. Zorluk yalnızca hamle sayısıyla artmamalı (kullanıcı, 2026-10-15 13:44)
- **Gözlem (Editor testi 13:39):** 1 boşlular zor; 2 boşlular "çok hamleli" hissettirdi. "Oyunun uzun vadede sadece hamle sayısının artmasıyla zorlaşması sıkıcı hale getirir."
- **Hamle üst sınırı:** şimdilik konmaz, ama çözümler absürt uzunluklara ulaşmamalı (bu süreçte; ileride yeniden bakılır).
- **Yenilik bölümünde ilk hamlede No Moves** (16–30'da 22) sorun değil, kalabilir.
- **Farklı başlangıç dizilimleri olacak:** şu an her tahta "dolu renkli bulutlar + boş bulutlar" diye başlıyor; yarı dolu / düzensiz başlangıçlar eklenecek. Çok sayıda test koşulup ölçüme göre yazılacak.
- **Tema özellikleri (modifier) ve üretim:** kullanıcı, üretime dahil edilmesi gereken bir etken mi, sonradan eklenebilir mi diye sordu (yanıt işlem kaydında, 13:44).

## 11k. Çerçeve: ilk 100 bölümün planlama süreci (kullanıcı, 2026-10-15 15:05)
- **İlk 100 bölümde temalar yalnızca görsel;** bölümlere (oynanışa) etki etmez. (§7 ile tutarlı: 1–100'de modifier yok; "temaya özel özellikler" 1–100 için düştü.)
- **Araç üzerinden tahmin ve test süreci:** 1–100'ün tamamı için bir zorluk planı yapılır, kademeli artar; süreç simüle edilir, bölümler zorluk puanına göre ayarlanır.
- **Fazlara bölünür, adım adım ilerlenir.**
- **Arka planda 1–100'ün genel şablonu** tutulur; gerekirse revize edilir.
- Kullanıcının soruları (aynı mesaj): aracın özellikleri ve odağı; ağırlık çarpanından sonra ne değişti; araç bölümleri zordan çok sıkıcı hissettiriyor — araç işlemlerine göre yeniden düzenleme, bazı şeylerin kaldırılması; 6–15 hangi özelliklerle yapıldı, sonra neler eklendi. (Yanıt işlem kaydında, 15:05.)

## 11l. 1–100 faz planı (kullanıcı onayı, 2026-10-15 16:04)
Her adımın sonunda kullanıcıya sunulur; onay olmadan sonraki adıma geçilmez. Her kod adımı önce testle.

**Faz 1 — Şablon**
- 1.1 **(bitti 16:17, sonuçlar `Docs/V3_CERCEVE_ENVANTERI.md`)** Ölçüm envanteri: 1–100'de kullanılabilecek bütün çerçevelerin (kapasite 4–6, 3–6 renk, 1–2 boş, klasik/serpiştirilmiş) puan, hamle, çıkmaz, takılma, No Moves dağılımları; çözücü süresi/sınır kontrolü.
- 1.2 Yenilik envanteri ve sırası: kapasite, renk, boş bulut, dizilim adımlarının 1–100'e taslak yerleşimi (§11c tablosundan).
- 1.3 Hedef eğrisi taslağı (bant bazlı artış, parça geçişleri, tema finalleri) — tablo + grafik.
- 1.4 Bölüm bölüm şablon (bant, parça, görsel tema, planlanan yenilik, çerçeve sınırları, hedef) — inceleme sayfası.
- 1.5 Onay.

**Faz 1 onaylandı (17:36)** — şablon: `Docs/V3_SABLON.md`.

**Faz 2 — Araç düzenlemesi**
- 2.1 **(bitti 17:36)** Uzunluk tercihinin kaldırılması; yerine "absürt uzunluk" sınırı (çerçevenin %90'lık en kısa çözüm değeri).
- 2.2 Renk ekleme adımı + boş bulutun "nefes alması". **31–60 kısmı bitti (18:05):** `Editor/V3ThemeBuilder.cs` (16–30'dan ayrı); önizleme `Logs/v3-preview-Theme2.csv`. 61–100 sonra.
- 2.3 **(bitti 17:44; kullanıcı kararıyla 2.2'den önce)** Serpiştirilmiş dizilimin merdivene eklenmesi (yumuşak 1 boş, Orta'larda çeşitlilik): bir Orta'da yenilik, sonra klasikle karışık.
- 2.4 Aracın şablondan beslenmesi (yeniliklerin şablondaki aralıklarda yerleşmesi).
- 2.5 ~~2 boşlu tahtalarda puan–insan uyumunun ele alınması (veriye göre).~~ **Atlandı (kullanıcı, 2026-10-15 20:20):** "gözüme batan bir şey olmadı". Finallerin ve 5 renkli planlamanın insan için zorluğu ayrı madde olarak §15'te.
- 2.6 Büyük çerçevelerde hız/sınır kontrolü. **Ertelendi (kullanıcı, 20:20: "ileriye dönük not düş, sonra bakalım")** — not §15'te.

**Faz 3 — Simülasyon**
- 3.1 1–100'ün tamamının veri yazmadan simülasyonu: bölüm tablosu, zorluk eğrisi grafiği, yenilik ve No Moves haritası, kural ihlali raporu.
- 3.2 Kontroller: absürt uzunluk, monotonluk göstergeleri (art arda benzer tahtalar), 1 boş / 2 boş / serpiştirilmiş dağılımı.
- 3.3 İnceleme → şablon revizyonu → yeniden simülasyon.

**Faz 4 — Parça parça oyun testi** — 4.1 Editor kısmı öne alındı ve geçti (17:56: kullanıcı 16–30'u beğendi; serpiştirilmiş dizilim "oyuna renk katmış", 1 boş kademeli, sıkıcılık azaldı, 30 en zoru). Telefon testi ve veriye kalıcı yazma sonra.
- 4.1 16–30: veriye yazma, Editor testi (16'dan), telefon testi, kalibrasyon. 4.2 31–60. 4.3 61–100 (test planı §12).

**Faz 5 — Kapanış**
- 5.1 Dünya listesi (1–15 "Tanıtım", 16–30 Green Valley, diğerleri geçici adlar). 5.2 Dokümanlar, `crVersion.md`. 5.3 v3.2 commit (onayla). 5.4 Push (ayrı onayla).

**Mevcut durum (16:04):** 16–30 deneme verisi oyuna bağlı, commit yok; renkler, pencere düzeltmesi, girdi kaydı, No Moves ölçütü, serpiştirilmiş dizilim kodda.

## 11d. 16–30 teması (karar, 2026-10-15)
- **16–30 (Geçiş teması) = Green Valley** (kesinleşti). Tasarımı (görsel, temaya özel özellikler) sonra yapılacak.
- Not: Kodda `green-valley` dünya kimliği bugün 1–20'yi kapsıyor ve 1–15 de bu kimliği kullanıyor (`AuthoredLevelCatalog`, test `CatalogUsesV3BoardsAndKeepsTutorialFrame`). Dünya listesi yeni aralıklara geçirilirken 1–15'in geçici "Tanıtım" kaydına alınması (§4) ve testin buna göre güncellenmesi gerekir.

## 12. Test planı
| Parça | Unity testi | Telefon testi |
|---|---|---|
| 1–5 (Tanıtım) | Parça bitince | Parça bitince |
| 6–15 (Alışma) | Parça bitince | Parça bitince |
| 16–30 (Geçiş teması) | Parça bitince | Parça bitince |
| 31–60 (Tema 2) | Parça bitince | Parça bitince, ama tüm bölümler bitirilmez |
| 61–100 (Tema 3) | Parça bitince | Önceki testte kalınan yerden devam, birkaç bölüm test edilir |

Test geri bildirimine göre zorluk ölçütleri revize edilir; ardından sonraki parçaya geçilir.

## 13. Parça durumu
| Parça | Durum | Not |
|---|---|---|
| 1–5 Tanıtım | **Tamamlandı** (2026-10-14): Unity + telefon testleri geçti | Bant aralıkları: Orta %30–60, Zor %70–85, Çok zor %90–100 (çerçevenin aday dağılımında). Sanal oyuncu ölçümüyle kalibre edilip yeniden seçildi (L1, L3, L4 değişti). İnsan verisi: Orta'larda 0–1, Zor/Çok zor'da 1–2 fazla hamle. |
| 6–15 Alışma | **Tamamlandı** (2026-10-14): Unity + telefon testleri geçti | Çerçeve: kapasite 4, 3 renk, 2 boş; varsayılan bant dilimleri. İnsan verisi (2 test ort.): Orta 0,9 < Zor 1,5 < Çok zor 1,75 fazla hamle; 2. döngüde Zor/Çok zor zayıf ayrışıyor — kullanıcı kararıyla kabul: oyunun başı, radikal artış istenmiyor, ikinci çözümün (ezber) etkisi büyük. |
| 16–30 Geçiş teması (Green Valley) | **Tamamlandı** (2026-10-15, v3.2): Editor (17:56) + telefon | `Editor/V3LadderBuilder.cs`. Serpiştirilmiş dizilim 17, kapasite 5 19, tek boş bulut 21 (tuzaklar 21, 26, 30). Kullanıcı: dizilim "oyuna renk katmış", sıkıcılık azaldı, 30 en zoru. |
| 31–60 Tema 2 | **Tamamlandı** (2026-10-15, v3.2): Editor (18:42) + telefon | `Editor/V3ThemeBuilder.cs`. Rüzgar 31, tek boş 36, 1 açık 39, klasik 49; döngüde 1 tuzak. Kullanıcı: "geçişler sırıtmadı, bulmaca hissi verdi". Sonradan final 60 (ortanca uzunluk + en tuzaklı) ve yalnızca 59 düzeltildi. |
| 61–100 Tema 3 | **Tamamlandı** (2026-10-15, v3.2): Editor (20:00) + telefon | `Editor/V3ThemeBuilder.cs`. Klasik 74; Ay 81; tek boş 87; 1 açık 89; klasik 94; tuzaklar döngüde 1 → 2, Zor/Çok zor tuzak 75, 78, 95, 98. Kullanıcı: 78, 97, 98 düşündürücü/keyifli; 81+ belirgin zorlayıcı. Finaller (60, 100) insan için kolay kaldı (§15). |

Uygulanan plan bölüm bölüm: `Docs/V3_SABLON.md` ("Uygulanan plan"); görsel özet `Docs/V3_BOLUM_HARITASI.html`.

## 14. İleride eklenecek özellikler (v3 kapsamı dışında)
- **Coin sistemi (kullanıcı, 2026-10-14):** Oyuna coin sistemi eklenecek. Detaylar (nasıl kazanılacağı, nereye harcanacağı, booster/can/reklam ekonomisiyle ilişkisi) henüz belirlenmedi. İlgili: `GELISTIRME_ONERILERI.md` G3 (booster ekonomisi). Not: Coin ile alınan booster'lar (Extra Cloud, Shuffle, Undo) bölüm zorluğunu etkiler; tasarlanırken v3 zorluk dengesi göz önünde tutulmalı.

## 15. Açık konular
- **Faz 2.6 — oyun içi çözücü sınırı (ertelendi, 2026-10-15 20:20):** Shuffle (`CloudShuffler`) ve No Moves sonrası "çözülebilen son duruma dönüş" (`FailRecovery.NewestSolvableIndex`) oyunda çözücüyü `PuzzleController.SolverStateLimit = 200000` durum sınırıyla çalıştırıyor. 5 renk + kapasite 5 (81–100, 7 bulut) ve ileride 6 renk / kapasite 6'da: (1) sınır aşılırsa çözülebilir bir tahta "çözülemez" sayılabilir (Shuffle adayı reddedilir, dönüş daha eski bir duruma gider); (2) telefonda süre ve donma riski. Yapılacak: 81–100 tahtalarında ve Shuffle sonrası durumlarda gereken durum sayısını ölçmek, telefonda süreyi ölçmek (telefon testinde logcat'ten gözlenebilir), gerekirse sınırı çerçeveye göre ayarlamak veya aramayı kareler arasına bölmek.
- **Tahta ölçeği (kullanıcı, 2026-10-15 20:00/20:20 — sonra düşünülecek):** 81'den itibaren 7 bulutta tahta Editor'da küçük hissettirdi; 87'de 6 bulutta "görüntü büyüdü, gözüm açıldı". Bulut sayısına göre ölçek/yerleşim düzeni ele alınacak; telefonda da bakılacak. **Telefon (20:21):** 7 bulutta da okunabiliyor, ama kullanıcı "uzun vadede yine de değiştirelim" dedi. **Telefon (20:50):** 87'de (6 bulut) tahta belirgin büyük; Extra bulut alınınca bariz bir küçülme oluyor (bulut sayısı arttıkça ölçek düşüyor) — ölçek düzenlemesinde Extra durumu da ele alınmalı.
- **Ölçü insanla uyuşmuyor: finaller ve 5 renkli planlama (kullanıcı, 2026-10-15 20:05 — "kaydet, ileride yaparız"):** Finali seçerken kullanılan "takılma" (saf oyuncunun başarısızlık oranı) dikkatli bir insanı iyi tahmin etmiyor. 60 (takılma 0,65) ve 100 (0,78) Editor testinde en kısa çözümle ve hızla bitti (100: 31 sn, 18/18 hamle); asıl zorlayanlar 59, 78, 83 (takılma 0,13, Shuffle), 90 (0,46, tuzaksız; 2 Shuffle) oldu. Öneri (Faz 3 içinde): oyun kayıtlarındaki süre, fazla hamle ve Shuffle/Undo/Extra kullanımı ölçülerle karşılaştırılıp insanı zorlayan ortak özellikler (ör. 5 renkte planlama yükü, güvenli görünen ama yanlış hamle sayısı) puana eklenir. Hızlı alternatif: her final için 2–3 aday, kullanıcı oynayıp seçer.
- **Kapat-aç açığı — karar (a), uygulandı (2026-10-15):** "No Moves" ekranındayken uygulama kapanırsa başarısızlık kayda yazılır (bölüm, tahta — ekstra bulutlar dahil, Undo geçmişi + modifier durumu, hamle sayısı; `FailRecord`). Açılışta aynı ekran aynı seçeneklerle geri gelir; "devam" çözülebilen son duruma yine döner. Kayıt başka bölüme aitse veya bozuksa yok sayılır. Retry/devam seçilince kayıt temizlenir. Not: Başarısız olmadan bölüm ortasında kapatılırsa bölüm hâlâ sıfırdan başlar (orijinal davranış).
- **"No Moves"a ulaşılabilirlik araçta tasarım ölçütü olmalı (kullanıcı, 2026-10-15, ileride):** Araçtan önceki bölümlerde oyuncunun "No Moves" (başarısızlık) ekranına düşmesi neredeyse imkânsızdı (yalnızca 1 boş bulutlu 19, 29… gibi; 3 renk + 2 boş bulutta matematiksel olarak imkânsız). Araç bunu bilinçli düzenleyecek şekilde revize edilebilir. Not: çözücü şu an "çıkmaz" (çözüme ulaşamayan) durumları sayıyor, ama bunların hangilerinin hiç hamle kalmayan (başarısızlığı tetikleyen) durum olduğunu ayrıca ölçmüyor — ayrı bir ölçüt olarak eklenebilir.
- **Geçici geliştirme butonu:** sağ üstte "AD +1" (reklam izle → +1 can); ileride yeri/şekli değişecek, reklamla can alma asıl yerine taşınacak.
- **Öğretici yazılar fark edilmiyor (kullanıcı, 2026-10-14):** 1–5 testlerinde L1–L5'teki yazılar ("Tap a cloud", "Groups move together"…) hiç dikkat çekmedi, göze çarpmadı. Tutorial ileride ayrıca ele alınacak; o zaman yazıların görünürlüğü (konum, boyut, vurgu) düzeltilmeli. v3'te dokunulmadı.
- Shuffle sayısı sınırı (bkz. §6).
- Temaların adları, tasarımları ve özellikleri.
