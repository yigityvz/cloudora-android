> 19 Ekim 2026 güncellemesi: Ay artık beşinci element olarak 60. bölümde başlar. Bu dosya tarihsel kalibrasyon kararlarını içerir; güncel yayımlama kuralı ve doğrulama: [IOS_MOON_FROM_60.md](IOS_MOON_FROM_60.md).

# v3 1–100 Şablonu (arka plan planı)

> Faz planı: tasarım dokümanı §11l. Bu dosya 1–100'ün genel şablonudur; gerekirse revize edilir (kullanıcı, 2026-10-15).
> Dayanak ölçümler: `Docs/V3_CERCEVE_ENVANTERI.md` (Faz 1.1).
> Durum (2026-10-15 20:20): **Faz 1 onaylandı (17:36); şablon araçla uygulandı.** 1–100 deneme verisi Editor'da test edildi (16–30 17:56, 31–60 18:42, 61–100 20:00), commit yok. Uygulanan plan "Uygulanan plan" başlığında; taslak bölümler tarihçe olarak duruyor. Görsel özet: `Docs/V3_BOLUM_HARITASI.html`.

## İlkeler (kararlar)
- 1–100'de temalar yalnızca görsel; oynanışa etki yok, modifier yok (§11k, §7).
- Her 5 bölümde Orta, Orta, Zor, Orta, Çok zor; zorluk kademeli ve birikimli, parça geçişlerinde sıçrama yok (§2).
- Bir bölümde tek yenilik; yenilik ilk kez Orta'da (§11c). Kapasite ve renk sayısı geri düşmez.
- Zorluk yalnızca hamle sayısıyla artmaz; çözümler absürt uzunluğa ulaşmaz (§11j).
- No Moves zaman zaman, sürpriz şeklinde, bantlara dağılmış (§11i).
- Kilitleyen ("sınav") bölüm yalnızca tema finalinde (§11i).

## Faz 1.2 — Yenilik envanteri ve sırası (onaylandı; uygulanan hali "Uygulanan plan" başlığında)

### Kullanılabilir yenilikler (ölçüme göre)
| Yenilik | Ne değiştirir | Ölçümde etkisi | Sınır |
|---|---|---|---|
| Renk (hava türü) +1 | Görsel çeşitlilik + yeni düşünme | Puan +5–8, en kısa çözüm +4–5 hamle; 4+ renkte 2 boşta da No Moves | Uzunluk: kap. 5'te 4 renk 15, 5 renk 19, 6 renk 23 hamle |
| Kapasite +1 | Daha uzun bulutlar | Puan +2–4, hamle +2–3 | Kap. 6 + 4 renk 18, + 5 renk 23 hamle |
| 1 boş bulut | Tuzak, çıkmaz, No Moves | İnsan için en etkili zorluk (Editor/telefon testleri) | Pratikte yalnızca 3 renkte (4 renkte çok az, sert tahta) |
| Dizilim: serpiştirilmiş → klasik | Başlangıç görünümü ve okuma | Serpiştirilmiş daha kolay (puan −2–3, takılma ve No Moves azalır) | Zorlaştırmaz; geçişi yumuşatır ve çeşitlilik verir |
| No Moves | Yenilgi, özellik ve coin kullanımı | 3 renk + 2 boşta yok; 4 renkte %13, 5 renkte %53 | Kendiliğinden ortaya çıkar; araç dağıtır |
| Tema finali "sınav" | Parça sonunda kilitleyen bölüm | Takılma sınırı yalnızca finalde kalkar | Her parçada 1 bölüm |

### Taslak yerleşim
| Bölümler | Parça (tema) | Renk | Kapasite | Boş | Dizilim | Yenilikler | Not |
|---|---|---|---|---|---|---|---|
| 1–5 | Tanıtım | 2 (Güneş, Yağmur) | 4 | 2 | klasik | — | Sabit (v3) |
| 6–15 | Alışma | 3 (+Kar, 6) | 4 | 2 | klasik | Kar (6) | Sabit (v3) |
| 16–30 | Green Valley | 3 | 4 → 5 (araç seçer; şimdi 19) | 2 → 1 (1 boş: 22; 26, 30) | klasik (**açık soru 1**) | Shuffle (16), kapasite 5, 1 boş | Final 30 "sınav" (1 boş) |
| 31–60 | Tema 2 | 4 (+Rüzgar, 31) | 5 | 2 | serpiştirilmiş → klasik (**açık soru 3**) | Rüzgar, dizilim adımları | No Moves 4 renkle kendiliğinden; 1 boş yalnızca finalde olabilir (4 renkte az tahta) |
| 61–100 | Tema 3 | **açık soru 2** | 5 | 2 | serpiştirilmiş → klasik | **açık soru 2** | Final 100 "sınav" |

### Kararlar (kullanıcı, 2026-10-15 16:28)
1. **16–30'da serpiştirilmiş dizilim bir yenilik gibi** uygun bir yere yerleştirilir; zorluklar kademeli artacak şekilde ayarlanır. Yer, hedefe yakın olduğu sürece araç seçebilir.
2. **61–100: seçenek C** — 5. renk (Ay) **80 civarında** gelir (parçanın yarısında değişiklik görülür). 61'in başında zaten görsel tema var; ek bir yenilik eklenebilirse destek olur (aday: dizilim adımı).
3. **Dizilimi yenilik olarak kullanmak — üç katman birlikte (onay 16:34):**
   - **Temalar arası:** her temanın ana yeniliği (yeni renk) yumuşak dizilimle (0 açık) gelir; 16–30'da serpiştirilmiş dizilim de bir yenilik olarak bir Orta'da, yerini araç seçer.
   - **Tema içi:** dizilim adımları (0 açık → 1 açık → klasik) tema boyunca dağılır; her adım bir Orta'da, tek yenilik.
   - **Karışık geçiş:** yeni dizilim gelince eskisi kaybolmaz, yenisi giderek sıklaşır (1 boş buluttaki gibi) — aralarda kolaylar ve sürpriz kalır, zorluk kademeli artar.
   - Dizilim örnekleri (kap. 4, 3 renk, 2 bulutluk boş yer): klasik = 3 dolu + 2 tamamen boş; 1 açık = 1 tamamen boş + boşluk diğerlerine dağılmış; 0 açık = hiç tamamen boş yok. Ölçüme göre 0 açık en kolay, klasik en zor.
   - **Dizilimin "nefes alması" (onay 16:36):** yeni tema / yeni renkle dizilim yeniden kolay başlar ve kademeli zorlaşır; **ama başladığı kolaylık öncekilerden kolay olmamalı** (yeni temanın en kolay bölümleri, önceki temanın başlangıcından kolay değil).

## Faz 1.3 — Hedef eğrisi (onaylandı 17:26)

### Sorun (Faz 1.1, bulgu 6)
Döngü başı +1 (Çok zor +1,2) ile 100. bölümde Orta ~34, Çok zor ~42 puana çıkılıyor; 2 boşlu tahtalarda puan büyük ölçüde uzunluktan geldiği için bu 5–6 renk ve 19–23+ hamle demek.

### Yaklaşım
Eğri parça parça kurulur; her parçanın hedef aralığı o parçanın çerçevelerinin (renk, dizilim, boş bulut) **ölçülmüş** puan aralığına oturtulur. Böylece zorluk yeni renk, dizilim, 1 boş ve No Moves'tan gelir; uzunluk ancak renk artınca (31'de, 80 civarı) kontrollü artar.

### Ölçülen puan aralıkları (kap. 5, kullanılabilir tahtalar, min–maks)
- 3 renk: 2 boş klasik 11,3–25,3 · 0 açık ~11–19 · 1 boş 7,7–31,5
- 4 renk: 2 boş klasik 16,7–32,2 · 1 açık 16,7–30,9 · 0 açık 15,7–29,1 · 1 boş 13,1–34,8
- 5 renk: 2 boş klasik 21,4–38,3 · 1 açık 18,3–39,5 · 0 açık 19,6–37,2

### Taslak eğri (parça başı/sonu; arası döngü döngü doğrusal)
| Bölümler | Çerçeve | Orta | Zor | Çok zor | En kısa çözüm (tipik) |
|---|---|---|---|---|---|
| 6–15 (sabit) | 3 renk, kap. 4 | 15,1 → 16,8 | 18,1 → 18,8 | 20,2 → 21,3 | 6–10 |
| 16–30 | 3 renk, kap. 4→5, 1 boş | 17,1 → 19,8 | 19,8 → 21,8 | 22,5 → 24,9 | 7–13 |
| 31–60 | 4 renk; 0 açık → 1 açık → klasik | 20,0 → 24,0 | 22,5 → 26,5 | 25,5 → 28,5 | ~12–15 |
| 61–80 | 4 renk; dizilim nefes alır → klasik (+ final 1 boş?) | 24,0 → 26,0 | 26,5 → 28,5 | 28,5 → 30,5 | ~14–15 |
| 81–100 | 5 renk (Ay ~80); 0 açık → klasik | 26,5 → 29,5 | 29,0 → 32,0 | 31,0 → 34,0 | ~17–19 |

- Döngü başı artış: 16–30 ~1,0 · 31–60 ~0,7 · 61–80 ~0,5 · 81–100 ~0,75. Oyunun başında yavaş, sonra hafif hızlanır; renk geçişlerinde puan sıçraması yok (31: 20,0 ≤ 30'un 24,9'u; 81: 26,5 ≤ 80'in 30,5'i).
- Nefes kuralı: her parçanın ilk Orta'sı, önceki parçanın en kolay başlangıcından kolay değil (31: 20,0 > 16'nın 17,1; 61: 24,0 > 31'in 20,0; 81: 26,5 > 61'in 24,0).
- Tema finalleri (30, 60, 80?, 100) "sınav" — eğrinin üstüne çıkabilir (takılma sınırı yok).
- Not: 2 boşlu tahtalarda puan insanın hissettiğinden yüksek (testler) — eğri göreli bir ölçüdür; Faz 4'te insan verisiyle kalibre edilir.
- **Onay (17:26):** eğri kabul — "test ettiğimizde 5 puanlık farkın ne kadar olduğunu göreceğiz" (puan farklarının insan için anlamı Faz 4 testlerinde görülecek). **80 tema finali değil** (normal Çok zor); **Ay (5. renk) 81'de** gelir. Tema finalleri: 30, 60, 100.

### Açık sorular (kullanıcıya) — hepsi cevaplandı (16:28–16:36, bkz. "Kararlar")
1. **16–30'da serpiştirilmiş dizilim:** 22'deki ilk 1 boş serpiştirilmiş (yumuşak) mi gelsin, 2 boşlu Orta'ların bir kısmı serpiştirilmiş mi olsun? (13:52'de soruldu, cevap bekliyor.)
2. **61–100'ün ana yeniliği:** (A) 5. renk (Ay) 61'de — kap. 5'te 5 renk ~19 hamle (uzun); (B) 4 renkte kalıp zorluğu dizilim, No Moves ve yer yer 1 boş (4 renk) ile artırmak — uzunluk sabit kalır ama yenilik azalır; (C) 5. renk geç (ör. 81'de), 61–80 4 renk + dizilim/No Moves.
3. **Dizilim adımlarını yenilik olarak kullanmak:** her renk girişinde önce serpiştirilmiş (0 açık), sonra 1 açık, sonra klasik (her adım bir Orta'da, tek yenilik). Görsel olarak fark edilir (tamamen boş bulutlar ortaya çıkar) ve zorluğu uzunluğa dokunmadan kademeli artırır.

## Uygulanan plan (2026-10-15 20:20, araçtan; Faz 1.4 taslağının yerine)

> Kaynak: `Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs` ve `Logs/v3-preview-*.csv`. Hedef: aracın hedef puanı (1–15 sabit). Puan: seçilen tahtanın puanı. Dizilim: "0 açık / 1 açık" serpiştirilmiş, "klasik" tamamen boş bulutlu. Tuzak: tek boş bulut. No Moves: hamle kalmayan duruma en erken bu hamlede ulaşılabilir (1–15 ölçülmedi).
> Taslaktan farklar: 16–30'da ilk tek boş 22 değil **21**, serpiştirilmiş dizilim **17**'de; 31–60'ta 1 açık **39** (taslak 41), klasik **49** (51), tek boş **36**'da döner ve döngüde 1 tuzak (taslakta 31–60 hep 2 boştu; kullanıcı 18:10 tuzak + planlama karışımı istedi); 61–80'de klasik **74** (71), tuzaklar döngüde 1 → 2, 75 ve 78 Zor/Çok zor tuzak (kullanıcı 19:00); 81–100'de tek boş **87**'de döner, 1 açık **89** (86), klasik **94** (91), 95 ve 98 Zor/Çok zor tuzak; finaller (30, 60, 100) tek boş ve parçanın en tuzaklısı. 59 sonradan yalnızca o değişecek şekilde düzeltildi (19:20).

| Bölüm | Parça | Bant | Hedef | Puan | Renk | Kap. | Boş | Dizilim | En kısa | Tuzak | No Moves | Yenilik / not |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Tanıtım | Orta | — | 10,40 | 2 | 4 | 2 | klasik | 4 |  |  | öğretici yazılar 1–5 |
| 2 | Tanıtım | Orta | — | 11,50 | 2 | 4 | 2 | klasik | 4 |  |  |  |
| 3 | Tanıtım | Zor | — | 13,70 | 2 | 4 | 2 | klasik | 5 |  |  |  |
| 4 | Tanıtım | Orta | — | 11,90 | 2 | 4 | 2 | klasik | 4 |  |  |  |
| 5 | Tanıtım | Çok zor | — | 15,40 | 2 | 4 | 2 | klasik | 7 |  |  |  |
| 6 | Alışma | Orta | — | 15,10 | 3 | 4 | 2 | klasik | 6 |  |  | Kar (3. renk) |
| 7 | Alışma | Orta | — | 15,50 | 3 | 4 | 2 | klasik | 10 |  |  |  |
| 8 | Alışma | Zor | — | 18,10 | 3 | 4 | 2 | klasik | 8 |  |  |  |
| 9 | Alışma | Orta | — | 15,80 | 3 | 4 | 2 | klasik | 6 |  |  |  |
| 10 | Alışma | Çok zor | — | 20,20 | 3 | 4 | 2 | klasik | 10 |  |  | "Undo is always safe" |
| 11 | Alışma | Orta | — | 16,10 | 3 | 4 | 2 | klasik | 10 |  |  |  |
| 12 | Alışma | Orta | — | 16,50 | 3 | 4 | 2 | klasik | 7 |  |  |  |
| 13 | Alışma | Zor | — | 18,80 | 3 | 4 | 2 | klasik | 9 |  |  |  |
| 14 | Alışma | Orta | — | 16,80 | 3 | 4 | 2 | klasik | 7 |  |  |  |
| 15 | Alışma | Çok zor | — | 21,30 | 3 | 4 | 2 | klasik | 10 |  |  | "The world is waking up" |
| 16 | Green Valley | Orta | 17,13 | 17,20 | 3 | 4 | 2 | klasik | 7 |  |  | Shuffle açılır; Green Valley (görsel) |
| 17 | Green Valley | Orta | 17,47 | 17,50 | 3 | 4 | 2 | 1 açık | 7 |  |  | serpiştirilmiş dizilim |
| 18 | Green Valley | Zor | 19,79 | 19,80 | 3 | 4 | 2 | klasik | 9 |  |  |  |
| 19 | Green Valley | Orta | 17,80 | 17,80 | 3 | 5 | 2 | klasik | 11 |  |  | kapasite 5 |
| 20 | Green Valley | Çok zor | 22,55 | 22,50 | 3 | 5 | 2 | klasik | 10 |  |  |  |
| 21 | Green Valley | Orta | 18,13 | 18,10 | 3 | 5 | 1 | 0 açık | 10 | T | 2. hamle | ilk tek boş bulut |
| 22 | Green Valley | Orta | 18,47 | 18,50 | 3 | 5 | 2 | klasik | 8 |  |  |  |
| 23 | Green Valley | Zor | 20,79 | 20,80 | 3 | 5 | 2 | klasik | 9 |  |  |  |
| 24 | Green Valley | Orta | 18,80 | 18,80 | 3 | 5 | 2 | 1 açık | 8 |  |  |  |
| 25 | Green Valley | Çok zor | 23,75 | 23,70 | 3 | 5 | 2 | klasik | 12 |  |  |  |
| 26 | Green Valley | Orta | 19,13 | 19,10 | 3 | 5 | 1 | klasik | 10 | T | 1. hamle |  |
| 27 | Green Valley | Orta | 19,47 | 19,50 | 3 | 5 | 2 | 1 açık | 12 |  |  |  |
| 28 | Green Valley | Zor | 21,79 | 21,80 | 3 | 5 | 2 | klasik | 10 |  |  |  |
| 29 | Green Valley | Orta | 19,80 | 19,80 | 3 | 5 | 2 | 1 açık | 9 |  |  |  |
| 30 | Green Valley | Çok zor | 24,95 | 24,90 | 3 | 5 | 1 | 0 açık | 11 | T | 4. hamle | tema finali (sınav) |
| 31 | Tema 2 | Orta | 20,00 | 20,00 | 4 | 5 | 2 | 0 açık | 12 |  |  | Rüzgar (4. renk); boş bulut 2 (nefes) |
| 32 | Tema 2 | Orta | 20,23 | 20,20 | 4 | 5 | 2 | 0 açık | 13 |  |  |  |
| 33 | Tema 2 | Zor | 22,50 | 22,40 | 4 | 5 | 2 | 0 açık | 16 |  | 3. hamle |  |
| 34 | Tema 2 | Orta | 20,47 | 20,50 | 4 | 5 | 2 | 0 açık | 14 |  |  |  |
| 35 | Tema 2 | Çok zor | 25,50 | 25,10 | 4 | 5 | 2 | 0 açık | 16 |  |  |  |
| 36 | Tema 2 | Orta | 20,71 | 20,60 | 4 | 5 | 1 | 0 açık | 14 | T | 3. hamle | tek boş bulut geri döner |
| 37 | Tema 2 | Orta | 20,94 | 20,90 | 4 | 5 | 2 | 0 açık | 12 |  |  |  |
| 38 | Tema 2 | Zor | 23,30 | 23,30 | 4 | 5 | 2 | 0 açık | 14 |  |  |  |
| 39 | Tema 2 | Orta | 21,18 | 21,20 | 4 | 5 | 2 | 1 açık | 13 |  |  | ilk 1 açık |
| 40 | Tema 2 | Çok zor | 26,10 | 26,10 | 4 | 5 | 2 | 1 açık | 14 |  |  |  |
| 41 | Tema 2 | Orta | 21,41 | 21,50 | 4 | 5 | 1 | 0 açık | 13 | T | 2. hamle |  |
| 42 | Tema 2 | Orta | 21,65 | 21,60 | 4 | 5 | 2 | 0 açık | 15 |  |  |  |
| 43 | Tema 2 | Zor | 24,10 | 24,10 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 44 | Tema 2 | Orta | 21,88 | 21,90 | 4 | 5 | 2 | 0 açık | 14 |  |  |  |
| 45 | Tema 2 | Çok zor | 26,70 | 26,70 | 4 | 5 | 2 | 1 açık | 14 |  |  |  |
| 46 | Tema 2 | Orta | 22,12 | 22,10 | 4 | 5 | 2 | 0 açık | 15 |  |  |  |
| 47 | Tema 2 | Orta | 22,35 | 22,40 | 4 | 5 | 2 | 0 açık | 13 |  |  |  |
| 48 | Tema 2 | Zor | 24,90 | 24,90 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 49 | Tema 2 | Orta | 22,59 | 22,60 | 4 | 5 | 2 | klasik | 14 |  |  | ilk klasik |
| 50 | Tema 2 | Çok zor | 27,30 | 27,40 | 4 | 5 | 1 | klasik | 16 | T | 1. hamle |  |
| 51 | Tema 2 | Orta | 22,82 | 22,80 | 4 | 5 | 2 | klasik | 15 |  |  |  |
| 52 | Tema 2 | Orta | 23,06 | 23,10 | 4 | 5 | 2 | klasik | 14 |  |  |  |
| 53 | Tema 2 | Zor | 25,70 | 25,70 | 4 | 5 | 2 | klasik | 13 |  |  |  |
| 54 | Tema 2 | Orta | 23,29 | 23,30 | 4 | 5 | 2 | 1 açık | 14 |  |  |  |
| 55 | Tema 2 | Çok zor | 27,90 | 28,00 | 4 | 5 | 1 | klasik | 15 | T | 2. hamle |  |
| 56 | Tema 2 | Orta | 23,53 | 23,50 | 4 | 5 | 2 | 0 açık | 16 |  |  |  |
| 57 | Tema 2 | Orta | 23,77 | 23,80 | 4 | 5 | 2 | 0 açık | 15 |  |  |  |
| 58 | Tema 2 | Zor | 26,50 | 26,50 | 4 | 5 | 2 | klasik | 13 |  |  |  |
| 59 | Tema 2 | Orta | 24,00 | 24,20 | 4 | 5 | 1 | klasik | 13 | T | 2. hamle |  |
| 60 | Tema 2 | Çok zor | 28,50 | 29,10 | 4 | 5 | 1 | klasik | 14 | T |  | tema finali (sınav) |
| 61 | Tema 3 (1. yarı) | Orta | 24,00 | 24,00 | 4 | 5 | 1 | 0 açık | 15 | T | 5. hamle | Tema 3 (görsel); dizilim nefes alır (0/1 açık) |
| 62 | Tema 3 (1. yarı) | Orta | 24,18 | 24,20 | 4 | 5 | 2 | 0 açık | 15 |  |  |  |
| 63 | Tema 3 (1. yarı) | Zor | 26,50 | 26,50 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 64 | Tema 3 (1. yarı) | Orta | 24,36 | 24,40 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 65 | Tema 3 (1. yarı) | Çok zor | 28,50 | 28,60 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 66 | Tema 3 (1. yarı) | Orta | 24,55 | 24,50 | 4 | 5 | 1 | 0 açık | 13 | T | 2. hamle |  |
| 67 | Tema 3 (1. yarı) | Orta | 24,73 | 24,70 | 4 | 5 | 2 | 0 açık | 14 |  |  |  |
| 68 | Tema 3 (1. yarı) | Zor | 27,17 | 27,20 | 4 | 5 | 2 | 1 açık | 14 |  |  |  |
| 69 | Tema 3 (1. yarı) | Orta | 24,91 | 24,90 | 4 | 5 | 2 | 1 açık | 16 |  |  |  |
| 70 | Tema 3 (1. yarı) | Çok zor | 29,17 | 29,20 | 4 | 5 | 2 | 1 açık | 15 |  |  |  |
| 71 | Tema 3 (1. yarı) | Orta | 25,09 | 25,10 | 4 | 5 | 1 | 0 açık | 14 | T | 4. hamle |  |
| 72 | Tema 3 (1. yarı) | Orta | 25,27 | 25,30 | 4 | 5 | 2 | 1 açık | 13 |  |  |  |
| 73 | Tema 3 (1. yarı) | Zor | 27,83 | 27,90 | 4 | 5 | 2 | 1 açık | 13 |  |  |  |
| 74 | Tema 3 (1. yarı) | Orta | 25,45 | 25,40 | 4 | 5 | 2 | klasik | 16 |  | 4. hamle | ilk klasik |
| 75 | Tema 3 (1. yarı) | Çok zor | 29,83 | 29,80 | 4 | 5 | 1 | 0 açık | 12 | T | 3. hamle | ilk Çok zor tuzak |
| 76 | Tema 3 (1. yarı) | Orta | 25,64 | 25,50 | 4 | 5 | 1 | 0 açık | 13 | T |  |  |
| 77 | Tema 3 (1. yarı) | Orta | 25,82 | 25,80 | 4 | 5 | 2 | klasik | 16 |  |  |  |
| 78 | Tema 3 (1. yarı) | Zor | 28,50 | 28,40 | 4 | 5 | 1 | klasik | 16 | T | 2. hamle | ilk Zor tuzak |
| 79 | Tema 3 (1. yarı) | Orta | 26,00 | 26,00 | 4 | 5 | 2 | klasik | 16 |  |  |  |
| 80 | Tema 3 (1. yarı) | Çok zor | 30,50 | 30,60 | 4 | 5 | 2 | 1 açık | 16 |  |  |  |
| 81 | Tema 3 (2. yarı) | Orta | 26,50 | 26,50 | 5 | 5 | 2 | 0 açık | 17 |  |  | Ay (5. renk); boş bulut 2 (nefes) |
| 82 | Tema 3 (2. yarı) | Orta | 26,77 | 26,80 | 5 | 5 | 2 | 0 açık | 18 |  |  |  |
| 83 | Tema 3 (2. yarı) | Zor | 29,00 | 29,00 | 5 | 5 | 2 | 0 açık | 18 |  |  |  |
| 84 | Tema 3 (2. yarı) | Orta | 27,05 | 27,10 | 5 | 5 | 2 | 0 açık | 18 |  |  |  |
| 85 | Tema 3 (2. yarı) | Çok zor | 31,00 | 31,10 | 5 | 5 | 2 | 0 açık | 19 |  |  |  |
| 86 | Tema 3 (2. yarı) | Orta | 27,32 | 27,30 | 5 | 5 | 2 | 0 açık | 19 |  |  |  |
| 87 | Tema 3 (2. yarı) | Orta | 27,59 | 27,50 | 5 | 5 | 1 | 0 açık | 19 | T | 6. hamle | tek boş bulut geri döner |
| 88 | Tema 3 (2. yarı) | Zor | 30,00 | 30,00 | 5 | 5 | 2 | 0 açık | 18 |  |  |  |
| 89 | Tema 3 (2. yarı) | Orta | 27,86 | 27,90 | 5 | 5 | 2 | 1 açık | 18 |  |  | ilk 1 açık |
| 90 | Tema 3 (2. yarı) | Çok zor | 32,00 | 32,00 | 5 | 5 | 2 | 1 açık | 17 |  |  |  |
| 91 | Tema 3 (2. yarı) | Orta | 28,14 | 28,10 | 5 | 5 | 1 | 0 açık | 17 | T | 6. hamle |  |
| 92 | Tema 3 (2. yarı) | Orta | 28,41 | 28,40 | 5 | 5 | 2 | 1 açık | 18 |  |  |  |
| 93 | Tema 3 (2. yarı) | Zor | 31,00 | 31,00 | 5 | 5 | 2 | 1 açık | 18 |  |  |  |
| 94 | Tema 3 (2. yarı) | Orta | 28,68 | 28,70 | 5 | 5 | 2 | klasik | 16 |  |  | ilk klasik |
| 95 | Tema 3 (2. yarı) | Çok zor | 33,00 | 33,40 | 5 | 5 | 1 | 0 açık | 19 | T | 2. hamle |  |
| 96 | Tema 3 (2. yarı) | Orta | 28,95 | 28,90 | 5 | 5 | 2 | 0 açık | 19 |  |  |  |
| 97 | Tema 3 (2. yarı) | Orta | 29,23 | 29,20 | 5 | 5 | 1 | 0 açık | 19 | T | 3. hamle |  |
| 98 | Tema 3 (2. yarı) | Zor | 32,00 | 31,90 | 5 | 5 | 1 | 0 açık | 18 | T | 5. hamle |  |
| 99 | Tema 3 (2. yarı) | Orta | 29,50 | 29,50 | 5 | 5 | 2 | 1 açık | 19 |  |  |  |
| 100 | Tema 3 (2. yarı) | Çok zor | 34,00 | 34,00 | 5 | 5 | 1 | klasik | 18 | T | 3. hamle | tema finali (sınav) |

## Faz 1.4 — Bölüm bölüm şablon (taslak, 2026-10-15 17:28 — tarihçe; uygulanan plan yukarıda)

> İnceleme sayfası (grafik + filtreli tablo): https://claude.ai/artifact/PDN14bYos5oRJkJLCjHeMW (özel).
> Hedef formülü (31–100): Orta = başlangıç + (son − başlangıç) × (döngü × 3 + sıra) / (3 × döngü sayısı − 1); Zor ve Çok zor döngüler boyunca doğrusal. 1–15 veri dosyasındaki puanlar; 16–30 bugünkü deneme planının hedefleri (araç).
> 16–30 satırları serpiştirilmiş dizilim yeniliği eklenince (Faz 2.3) değişecek. 31–100 için "yenilik" bölümleri araç tarafından ±2 bölüm içinde, Orta bölüme yerleştirilecek.

| Bölüm | Parça | Bant | Hedef puan | Renk | Kap. | Boş | Dizilim (plan) | Yenilik / not | Kaynak |
|---|---|---|---|---|---|---|---|---|---|
| 1 | Tanıtım | Orta | 10,38 | 2 | 4 | 2 | klasik |  | sabit (v3) |
| 2 | Tanıtım | Orta | 11,55 | 2 | 4 | 2 | klasik |  | sabit (v3) |
| 3 | Tanıtım | Zor | 13,69 | 2 | 4 | 2 | klasik |  | sabit (v3) |
| 4 | Tanıtım | Orta | 11,91 | 2 | 4 | 2 | klasik |  | sabit (v3) |
| 5 | Tanıtım | Çok zor | 15,44 | 2 | 4 | 2 | klasik |  | sabit (v3) |
| 6 | Alışma | Orta | 15,13 | 3 | 4 | 2 | klasik | Kar (3. renk) | sabit (v3) |
| 7 | Alışma | Orta | 15,46 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 8 | Alışma | Zor | 18,12 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 9 | Alışma | Orta | 15,78 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 10 | Alışma | Çok zor | 20,24 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 11 | Alışma | Orta | 16,13 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 12 | Alışma | Orta | 16,47 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 13 | Alışma | Zor | 18,79 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 14 | Alışma | Orta | 16,8 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 15 | Alışma | Çok zor | 21,35 | 3 | 4 | 2 | klasik |  | sabit (v3) |
| 16 | Green Valley | Orta | 17,13 | 3 | 4 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) | Shuffle açılır | deneme verisi (13:27) |
| 17 | Green Valley | Orta | 17,47 | 3 | 4 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 18 | Green Valley | Zor | 19,79 | 3 | 4 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 19 | Green Valley | Orta | 17,8 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) | kapasite 5 | deneme verisi (13:27) |
| 20 | Green Valley | Çok zor | 22,55 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 21 | Green Valley | Orta | 18,13 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 22 | Green Valley | Orta | 18,47 | 3 | 5 | 1 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) | ilk 1 boş bulut | deneme verisi (13:27) |
| 23 | Green Valley | Zor | 20,79 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 24 | Green Valley | Orta | 18,8 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 25 | Green Valley | Çok zor | 23,75 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 26 | Green Valley | Orta | 19,13 | 3 | 5 | 1 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 27 | Green Valley | Orta | 19,47 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 28 | Green Valley | Zor | 21,79 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 29 | Green Valley | Orta | 19,8 | 3 | 5 | 2 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) |  | deneme verisi (13:27) |
| 30 | Green Valley | Çok zor | 24,95 | 3 | 5 | 1 | klasik (serpiştirilmiş yenilik: araç yerleştirecek) | tema finali (sınav) | deneme verisi (13:27) |
| 31 | Tema 2 | Orta | 20,0 | 4 | 5 | 2 | 0 açık | Rüzgar (4. renk) + 0 açık dizilim | plan |
| 32 | Tema 2 | Orta | 20,24 | 4 | 5 | 2 | 0 açık |  | plan |
| 33 | Tema 2 | Zor | 22,5 | 4 | 5 | 2 | 0 açık |  | plan |
| 34 | Tema 2 | Orta | 20,47 | 4 | 5 | 2 | 0 açık |  | plan |
| 35 | Tema 2 | Çok zor | 25,5 | 4 | 5 | 2 | 0 açık |  | plan |
| 36 | Tema 2 | Orta | 20,71 | 4 | 5 | 2 | 0 açık |  | plan |
| 37 | Tema 2 | Orta | 20,94 | 4 | 5 | 2 | 0 açık |  | plan |
| 38 | Tema 2 | Zor | 23,3 | 4 | 5 | 2 | 0 açık |  | plan |
| 39 | Tema 2 | Orta | 21,18 | 4 | 5 | 2 | 0 açık |  | plan |
| 40 | Tema 2 | Çok zor | 26,1 | 4 | 5 | 2 | 0 açık |  | plan |
| 41 | Tema 2 | Orta | 21,41 | 4 | 5 | 2 | 0 açık + 1 açık (yeni) | ilk 1 açık (araç ±2 bölüm) | plan |
| 42 | Tema 2 | Orta | 21,65 | 4 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 43 | Tema 2 | Zor | 24,1 | 4 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 44 | Tema 2 | Orta | 21,88 | 4 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 45 | Tema 2 | Çok zor | 26,7 | 4 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 46 | Tema 2 | Orta | 22,12 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 47 | Tema 2 | Orta | 22,35 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 48 | Tema 2 | Zor | 24,9 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 49 | Tema 2 | Orta | 22,59 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 50 | Tema 2 | Çok zor | 27,3 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 51 | Tema 2 | Orta | 22,82 | 4 | 5 | 2 | 1 açık + klasik (yeni) | ilk klasik (araç ±2 bölüm) | plan |
| 52 | Tema 2 | Orta | 23,06 | 4 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 53 | Tema 2 | Zor | 25,7 | 4 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 54 | Tema 2 | Orta | 23,29 | 4 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 55 | Tema 2 | Çok zor | 27,9 | 4 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 56 | Tema 2 | Orta | 23,53 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 57 | Tema 2 | Orta | 23,76 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 58 | Tema 2 | Zor | 26,5 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 59 | Tema 2 | Orta | 24,0 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 60 | Tema 2 | Çok zor | 28,5 | 4 | 5 | 2 | klasik ağırlıklı | tema finali (sınav) | plan |
| 61 | Tema 3 (1. yarı) | Orta | 24,0 | 4 | 5 | 2 | nefes: 0/1 açık | yeni tema (görsel) + dizilim nefes alır | plan |
| 62 | Tema 3 (1. yarı) | Orta | 24,18 | 4 | 5 | 2 | nefes: 0/1 açık |  | plan |
| 63 | Tema 3 (1. yarı) | Zor | 26,5 | 4 | 5 | 2 | nefes: 0/1 açık |  | plan |
| 64 | Tema 3 (1. yarı) | Orta | 24,36 | 4 | 5 | 2 | nefes: 0/1 açık |  | plan |
| 65 | Tema 3 (1. yarı) | Çok zor | 28,5 | 4 | 5 | 2 | nefes: 0/1 açık |  | plan |
| 66 | Tema 3 (1. yarı) | Orta | 24,55 | 4 | 5 | 2 | 1 açık ağırlıklı | 1 açık sıklaşır | plan |
| 67 | Tema 3 (1. yarı) | Orta | 24,73 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 68 | Tema 3 (1. yarı) | Zor | 27,17 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 69 | Tema 3 (1. yarı) | Orta | 24,91 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 70 | Tema 3 (1. yarı) | Çok zor | 29,17 | 4 | 5 | 2 | 1 açık ağırlıklı |  | plan |
| 71 | Tema 3 (1. yarı) | Orta | 25,09 | 4 | 5 | 2 | 1 açık + klasik | ilk klasik | plan |
| 72 | Tema 3 (1. yarı) | Orta | 25,27 | 4 | 5 | 2 | 1 açık + klasik |  | plan |
| 73 | Tema 3 (1. yarı) | Zor | 27,83 | 4 | 5 | 2 | 1 açık + klasik |  | plan |
| 74 | Tema 3 (1. yarı) | Orta | 25,45 | 4 | 5 | 2 | 1 açık + klasik |  | plan |
| 75 | Tema 3 (1. yarı) | Çok zor | 29,83 | 4 | 5 | 2 | 1 açık + klasik |  | plan |
| 76 | Tema 3 (1. yarı) | Orta | 25,64 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 77 | Tema 3 (1. yarı) | Orta | 25,82 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 78 | Tema 3 (1. yarı) | Zor | 28,5 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 79 | Tema 3 (1. yarı) | Orta | 26,0 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 80 | Tema 3 (1. yarı) | Çok zor | 30,5 | 4 | 5 | 2 | klasik ağırlıklı |  | plan |
| 81 | Tema 3 (2. yarı) | Orta | 26,5 | 5 | 5 | 2 | 0 açık (Ay ile nefes) | Ay (5. renk) + 0 açık dizilim | plan |
| 82 | Tema 3 (2. yarı) | Orta | 26,77 | 5 | 5 | 2 | 0 açık (Ay ile nefes) |  | plan |
| 83 | Tema 3 (2. yarı) | Zor | 29,0 | 5 | 5 | 2 | 0 açık (Ay ile nefes) |  | plan |
| 84 | Tema 3 (2. yarı) | Orta | 27,05 | 5 | 5 | 2 | 0 açık (Ay ile nefes) |  | plan |
| 85 | Tema 3 (2. yarı) | Çok zor | 31,0 | 5 | 5 | 2 | 0 açık (Ay ile nefes) |  | plan |
| 86 | Tema 3 (2. yarı) | Orta | 27,32 | 5 | 5 | 2 | 0 açık + 1 açık (yeni) | ilk 1 açık (araç ±2 bölüm) | plan |
| 87 | Tema 3 (2. yarı) | Orta | 27,59 | 5 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 88 | Tema 3 (2. yarı) | Zor | 30,0 | 5 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 89 | Tema 3 (2. yarı) | Orta | 27,86 | 5 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 90 | Tema 3 (2. yarı) | Çok zor | 32,0 | 5 | 5 | 2 | 0 açık + 1 açık (yeni) |  | plan |
| 91 | Tema 3 (2. yarı) | Orta | 28,14 | 5 | 5 | 2 | 1 açık + klasik (yeni) | ilk klasik (araç ±2 bölüm) | plan |
| 92 | Tema 3 (2. yarı) | Orta | 28,41 | 5 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 93 | Tema 3 (2. yarı) | Zor | 31,0 | 5 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 94 | Tema 3 (2. yarı) | Orta | 28,68 | 5 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 95 | Tema 3 (2. yarı) | Çok zor | 33,0 | 5 | 5 | 2 | 1 açık + klasik (yeni) |  | plan |
| 96 | Tema 3 (2. yarı) | Orta | 28,95 | 5 | 5 | 2 | klasik ağırlıklı |  | plan |
| 97 | Tema 3 (2. yarı) | Orta | 29,23 | 5 | 5 | 2 | klasik ağırlıklı |  | plan |
| 98 | Tema 3 (2. yarı) | Zor | 32,0 | 5 | 5 | 2 | klasik ağırlıklı |  | plan |
| 99 | Tema 3 (2. yarı) | Orta | 29,5 | 5 | 5 | 2 | klasik ağırlıklı |  | plan |
| 100 | Tema 3 (2. yarı) | Çok zor | 34,0 | 5 | 5 | 2 | klasik ağırlıklı | tema finali (sınav) | plan |
