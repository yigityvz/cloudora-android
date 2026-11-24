# UX ve çözümsüzlük düzeltmesi — 17 Ekim 2026

Güncel kapsam: tanıtım **1–14**, Yeşil Vadi **15–30**, Rainy Coast **31–60**, Frost Peaks **61–100**. Önceki belgelerdeki 16–40/41–70/71–100 aralıklarının yerini alır.

## Mantık ve can

- Kayıp koşulu artık yalnızca legal hamle kalmaması değildir. Normal hamlelerle hiçbir çözüme ulaşılamadığı kanıtlandığında bölüm kaybedilir.
- Runtime kontrolü, mevcut PuzzleRules ile aynı kodlamayı kullanan kesin, kanonik durum aramasıdır. Döngüler tekilleştirilir. Ana kare başına yaklaşık 2 ms bütçe ile devam eder; süre bütçesinin dolması **kayıp sayılmaz**.
- Yeni hamle, Undo, ek hazne, Shuffle, bölüm yükleme veya eve dönüş eski analizi iptal eder. Sonuç sadece aynı tahta sürümü için uygulanabilir.
- Sonuç önbelleği kapasiteyi **ve hazne sayısını** içerir. Aynı renk dizilimine eklenen boş hazne eski çözümsüzlük sonucunu kullanmaz.
- Fotoğraftaki kapasite-5 tahta (`RRR/NNN/NRSSS/SRNS`) için sarılar arasında legal hamle vardır; buna rağmen çözüm yoktur. Regresyon testi hem bu durumu hem ek boş hazneyle tekrar çözülebilir hale gelmesini kapsar.
- İlk 100 başlangıç tahtasının tamamı runtime aramasıyla çözülebilir bulundu. Tahta verileri yeniden üretilmedi.
- Bitmemiş bölümden Ana Sayfa: bir mevcut can harcanır; başlangıç tahtası, yardımcı durumu ve başarısızlık kaydı sıfırlanır. Can sıfırsa negatif can oluşturulmaz; çıkış yine mümkündür. Kazanılmış bölümden eve dönüş ücretsizdir ve sonraki bölümü hazırlar. Can sıfır olduğunda ana sayfadaki birincil eylem Can Kazan · +1 olur; oyuncu can kazanma seçeneğine erişimini kaybetmez.

## Görsel ve etkileşim denetimi

| Alan | Uygulanan düzeltme |
|---|---|
| Ortak kontroller | Teal birincil eylemler, daha ince kenarlar, tutarlı köşe ve boşluklar, basma geri bildirimi; küçük görsel kontrollerde geniş dokunma alanı |
| Ana sayfa | Tek alt kartta dünya bilgisi, sayısal ilerleme, Oyna ve Dünyalar; tam ekran resim oranı korunur |
| Oynanış | Dünyaya göre arka plan; yaprak, yağmur veya kar atmosferi; gerçek zorluk rozeti; 4/6/7/10 hazneye göre yerleşim |
| Yardımcılar | Hizalı simgeler ve sayaçlar; sürekli reklam açıklaması ve AD rozeti kaldırıldı; hak sıfırken artı işareti |
| Hazneler | Bulut ayakları kaldırıldı; vektör cam, yumuşak taban maskesi, ince yansıma; rüzgâr mint renk ve rüzgâr simgesiyle gösterilir |
| Aktarım | Şişeler sabit; yalnızca element kavisli yoldan taşınır. Türe özel efektler ve gecikmeli seviye dolumu. Güncel ayrıntı: `IOS_ELEMENT_MOTION.md` |
| Hareket güvenliği | Şişe konum/açı/ölçeği sabittir; seçim ve tamamlanma ışıkla belirtilir. Geçici render sırası değişimi yoktur |
| Duraklatma | Baskın Devam Et; yan yana Dünyalar/Ayarlar; açık −1 can ve bölümün baştan başlayacağı bilgisi |
| Kazanma/kaybetme | Küçük, tutarlı eylem hiyerarşisi; kazanma sonrası ücretsiz Ana Sayfa; gerçek kayıp ve kurtarma seçenekleri |
| Ayarlar | Boş alan azaltıldı; kısa açıklamalı satırlar ve 44 pt erişim alanı |
| Dünyalar | Üç kart, daha kısa son bölüm; aktif dünyada Devam Et gerçekten oynanışa götürür |
| Kurtuluş | Üç resim durumu arasında yumuşak geçiş; azaltılmış harekette kısa, sade geçiş |
| Bildirimler | Yardımcı kazanma mesajları oyunu kesen tam panel yerine kısa bildirim olarak gösterilir |

Referans video: 341,3 saniyenin tamamı 2 saniye aralıklı zaman çizelgesiyle; aktarım, kurtuluş ve son bölüm ayrıntılı karelerle incelendi. Kayıt içindeki kişi/diğer görüntüler oyun varlığı olarak alınmadı.

Kararlarda [Apple oyun kontrolleri](https://developer.apple.com/design/human-interface-guidelines/game-controls) ve [hareket rehberi](https://developer.apple.com/design/human-interface-guidelines/motion) referans alındı: yeterli dokunma alanı, eyleme bağlı kısa geri bildirim ve azaltılmış hareket desteği.

## Doğrulama kanıtları

- Genel set: 217 test; 213 geçti, 4 mevcut atlama (`Logs/ios-editmode.xml`).
- Grafik açık özel test: gerçek raycast ve tıklama, aktarımın tamamı, çerçeve dışına çıkmama, dönüş sonrası kaynak sıvının görünürlüğü (`Logs/ux-pointer-motion.xml`).
- Ek önbellek regresyonu ve 100 tahta kontrolü: 5/5 geçti (`Logs/ux-reachability-tests.xml`).
- Sıfır canda ana sayfa eylemi ve can yenilenince Oyna dönüşü: 1/1 geçti (`Logs/ux-main-menu-life.xml`).
- Ana sayfaya çıkış, bir can, başlangıç tahtası, kazanma sonrası ücretsiz geçiş gerçek PuzzleController ile test edildi.
- 375×667, 390×844, 750×1334 ekran render'ları: `Logs/DesignPreview`.
- Çalışan aktarımın kareleri ve kısa videosu: `Logs/MotionReview/pour-preview.mp4`.
- Bu kayıtlar editör doğrulamasıdır; fiziksel cihaz görsel kabulü ayrıca kullanıcı tarafından yapılır. Cihaz kurulum sonucu `IOS_STATUS.md` içinde.
