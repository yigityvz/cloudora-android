# Cloudora iOS değişiklik kaydı

## 2026-10-19 — güncel iOS kaynak sürümü

- Açılış, ana ekran, dünyalar ve oyun arayüzü; sabit hazneler, elemente özel aktarım ve yedi bitiriş efekti bir araya getirildi.
- İlk 100 bölüm, gerçek dünya ilerlemesi, çözümsüzlük tespiti, can/ödül kuralları ve Ay’ın 60. bölümden itibaren beşinci element olması dahil edildi.
- Son EditMode doğrulaması: 235 geçti, 6 atlandı, 0 hata. Unity iOS export, imzalı Xcode Release derlemesi ve mevcut kayıt korunarak iPhone kurulumu başarılı.
- GitHub kaynak teslimi yalnızca iOS projesini içerir; derlemeler, cihaz kayıtları, yerel imzalama bilgileri ve Project Auditor kopyaları kapsam dışıdır.

## 2026-10-16 — Figma açılış animasyonu

- Onaylı Figma bulut/logo katmanlarıyla yaklaşık 3 saniyelik açılış eklendi.
- Mevcut ana ekrana otomatik geçer; oyun, bölüm, ekonomi ve kayıt davranışı değişmedi.
- iOS Hareketi Azalt ayarı desteklenir. Açılış dokunmaları engeller, tamamlanınca engeli kaldırır.
- Tam EditMode paketi: 193 geçti, 4 atlandı, 0 hata. Son opaklık düzeltmesinden sonra 2 açılış testi yeniden geçti.
- Unity UI önizlemeleri 390×844, 375×667 ve 393×852 boyutlarında kontrol edildi.
- Tasarım kaynağı, zamanlama ve varlık açıklamaları: `Docs/IOS_OPENING_SPLASH.md`.

## 2026-10-16 — iOS depo temizliği

- README iOS kurulum, test ve Xcode akışı için yeniden yazıldı.
- Diğer platformun derleme betiği, Editor araçları, build profili, mağaza yapılandırması ve yayın belgeleri kaldırıldı.
- Yalnızca kaldırılan manifest aracına ait test çıkarıldı; oyun testleri korundu.
- Kullanılmayan JNI modülü kaldırıldı. Ortak oyun kodu, sahneler ve içerikler değişmedi.
- Önceki ayrıntılı geliştirme kayıtları Git geçmişinde korunur.

## 2026-10-16 — iPhone doğrulaması

- Unity export, yerel imzalı Xcode derlemesi, iPhone kurulumu ve başlatma başarılı.
- Kullanıcı ilk kısa test adımlarını yaptığını bildirdi; isteğiyle yedek alınarak cihaz kaydı 80. bölüme getirildi.
- Tam cihaz kabul testi henüz tamamlanmadı.

## 2026-10-16 — mevcut oyunun iOS aktarımı

- Kaynak sürüm d7c9416 temel alınarak oyun davranışı korundu.
- Aktarımda 192 EditMode testi geçti, 4 test atlandı, hata yoktu. İlk 100 bölümün salt okunur kalibrasyonu tamamlandı.
