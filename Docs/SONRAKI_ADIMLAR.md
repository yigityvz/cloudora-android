# Sonraki adımlar — karar durumu ayrı tutulur

24 Kasım2026. Bu liste otomatik uygulama emri değildir. Güncel sözleşme `GUNCEL_DURUM.md`; öğrenme girişi `../CLAUDE.md`.

## Kullanıcının istediği bu teslim

- Telefona kurulan iOS `1ee4334` oyun tabanını Android reposunun `cloudora-yigit` dalında erişilebilir yapmak.
- Yeni ekip üyesi/Claude Code için sıralı, tüm projeyi kapsayan okuma yönergesi ve dosya haritası.
- Diğer Android dalıyla farkları ortaya koymak; `main`e birleştirme kararı bundan sonra kullanıcıya ait.
- Oynanış, animasyon, ekonomi, veri, ayar ve kayıt davranışını bu kaynak tesliminde değiştirmemek.

## İncelemeye hazır fakat bu teslimde taşınmayan adaylar

| Aday | Neden yararlı olabilir? | Neden topluca taşınmaz? |
|---|---|---|
| v4.1 öğretici üst alan rezervi | Çok satır/sıralı tahtada metin çakışmasını azaltabilir | Aynı controller diff'i çözümsüzlük kontrolünü de kaldırıyor; yalnız dar UI düzeltmesi ayrı test edilmeli |
| v4.1 milestone can sayacı yenilemesi | Kurtuluş ekranındaki kaynak bilgisini güncel tutar | Cihaz/ekran kanıtı ve dar regresyonla alınmalı |
| Android build aracı/manifest/profil | Android ekibinin aynı oyunu paketlemesi | Gameplay verisi/algoritma değişikliğinden ayrılmalı; izinler/servis/QA imzası incelenmeli |
|2–3 adım ileri bakan sanal oyuncu ölçümleri | Tasarım değerlendirmesine ek veri sağlayabilir | Puan ağırlığı, builder ve100 tabloyu birlikte değiştirme gerekçesi değildir; salt ölçüm modu düşünülmeli |

## Onay verilmemiş ürün değişiklikleri

- v4.1'deki yalnız legal hamle bittiğinde kaybetme kuralı, Ay61, Shuffle15, kapasite6 ve yeni100 tahta bu tabana alınmış değildir.
- v4.5.0 Efe APK'sının on bölge/Ay13/11 element/160 deneme,500 başlangıç coini, farklı booster fiyatları ve farklı kayıt/arayüz mimarisi bu ürünün güncel gereksinimi değildir.
- Yeni bölüm/element mekanikleri,100 sonrası içerik ve kalıcı test laboratuvarı yalnız ayrı ürün kararıyla eklenir. Kod genişletilebilir tasarlanmış olsa da gelecek içerik otomatik açık değildir.
- Gerçek reklam/analytics SDK'sı, mağaza yayını, IAP/backend/cloud save, iPad desteği veya paket yükseltmesi ayrı kapsam gerektirir.

## Açık kalite ve tutarlılık işleri

- Tanıtım14/öğretici15 sınırında metin ve UI etiketlerini kullanıcı kararıyla tutarlı hale getirmek.
-500 coin devam akışının işlevi/metni ile eski "ekstra hazne" beklentisini ayrı karara bağlamak.
- Tarihsel kalibrasyon ve runtime kilitli kuralların ölçüm farkını görünür tutmak; zorluk değerlendirmesini kullanıcı oynanışıyla desteklemek.
- Gerçek cihaz matrisinde kare süreleri, ısınma, bellek ve pil ölçmek. Görüntü kalitesini düşürmek otomatik çözüm değildir.
- Ses kliplerinin ve haptiklerin gerçek cihazda mevcut bağlanma/kabul durumunu kontrol etmek; source fallback'i tam ses tasarımı sanmamak.
- Asset/LFS/font import, Retina netlik, ekran güvenli alanı, dokunma ve hareket azaltma kabulünü Android/iOS'ta ayrı doğrulamak.

## Yayın öncesi

`RELEASE_CHECKLIST`, `PRIVACY_RELEASE_REVIEW` ve platform derleme belgelerine git. Paket imzası, doğru uygulama kimliği, gizlilik/destek bağlantısı, mağaza metinleri, son ikon/görseller ve gerçek cihaz kabulü tamamlanmadan "yayına hazır" yazma. Güncel test ve build çıktısını ilgili sürüme bağla; eski Logs referansının klonda bulunmadığını dikkate al.
