# Görüntü kalitesi, akış ve 100 bölüm — 17 Ekim 2026

Bu değişiklik, kullanıcının cihaz ekran görüntüleri ve yeni 100 bölüm / 3 dünya kapsamı üzerine yapıldı.

## Oynanabilir kapsam

| Aşama | Bölümler | Sayı |
|---|---:|---:|
| Dünyasız tanıtım | 1–15 | 15 |
| Yeşil Vadi | 16–40 | 25 |
| Rainy Coast | 41–70 | 30 |
| Frost Peaks | 71–100 | 30 |

`WorldCatalog` yalnızca üç dünya sunar. `ProgressionManager` yükleme, devam, kayıt ve debug atlamasında 100 sınırını uygular. 100 tamamlandığında üçüncü dünya kurtarılır, ana sayfada kampanya tamamlandı kartı gösterilir; 101 yüklenmez. Sonraki içerik üreticileri gelecekte kullanılabilmek üzere kaynakta durur ama bu sürümün oyun akışında erişilemez.

İlk 100 bölümün V3 tahta verisi yeniden üretilmedi; parça dizilimleri, çözümler ve bölüm ekonomisi korunur. Dünyalar artık yeni aralıklara göre etiketlenir. Tanıtımın 15 bölümü Yeşil Vadi yüzdesine sayılmaz. Yüzde = dünya içinde tamamlanan bölüm / dünyadaki bölüm sayısı; ana sayfada sayı da (örneğin 1/25) gösterilir.

## Görüntü ve etkileşim

- Önceki Very Low kalite seviyesinin `globalTextureMipmapLimit: 1` ayarı kaynakları yarı çözünürlüğe indiriyordu. Limit kaldırıldı.
- Tasarım görselleri iOS'ta 4096 üst sınır, orijinal NPOT boyut, RGBA32 ve bilinear filtre ile yüklenir. UI görsellerinde mipmap kapatıldı; orijinal görsel boyutu korunur, yapay detay üretilmez.
- Eski 1080 koordinatlı canvas içindeki 390 birimlik arayüz kökü, küçük glyph'leri büyütüyordu. Ortak Text bileşeni 3× glyph üretip ölçeği geri indirir; görsel punto/boyut aynı kalır, Retina netliği artar.
- Hazne dolguları ton geçişli ve vektör maskelidir; düşük çözünürlüklü yuvarlak maske kaldırıldı.
- Aktarım: kavisli renk akışı, yumuşak ışık, hareketli küçük parıltılar, hafif kaynak hazne eğilmesi, azalan/artan dolgu. Azaltılmış harekette kısa, hareketsiz geçiş vardır.
- Animasyon yalnızca geçici görselleri değiştirir. Hamle verisi, Undo kaydı ve kazanma kontrolü mevcut controller tarafından bir kez uygulanır. Sıfır yüksekliğe inen dolgu grafiği güvenli biçimde çizilmez.
- Öğretici ve ödül mesajları Türkçeleştirildi.
- Kazanma paneline Ana Sayfaya Dön eklendi. Kazanılmış bölümden eve dönüş sonraki bölümü hazırlar; tekrar ödül vermez.
- Cihaz export'u Development Build filigranı içermez. `CLOUDORA_DEVICE_QA` derleme tanımı mevcut test reklam/ödül davranışını korur; mağaza yayını için kullanılmaz.

## Kanıtlar

- Retina 750×1334 ve daha küçük önizlemeler: `Logs/DesignPreview/retina-home.png`, `gameplay-retina.png`, `gameplay-flow.png`, `win.png`.
- Kampanya testleri tüm 16–100 aralığını, üç dünya sınırlarını, her bölüm yüzdesini, 100 sonu kayıt sınırını, kazanma/ev callback'ini kapsar.
- Akış testi timeScale=0 iken tamamlanmayı, geçici çizimlerin temizlenmesini ve controller uygulayana kadar tahta verisinin değişmemesini kontrol eder.
- Cihaz kurulumundan önce Documents yedeği: `Logs/device-backup-before-quality-20261017` (Git dışında).
- Son test/build/cihaz sonuçları: `IOS_STATUS.md`.
