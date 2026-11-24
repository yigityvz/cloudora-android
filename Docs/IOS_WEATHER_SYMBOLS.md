# Şişe içi element simgeleri — 18 Ekim 2026

Yedi element, tek bir 24 birimlik vektör ailesiyle yeniden çizildi. Eski dolu bitmapler ve gökkuşağının elmas metin karakteri yerine ince, tutarlı çizgiler kullanılır:

- Güneş: halkalı merkez ve kısa yuvarlak ışınlar.
- Yağmur: konturlu damla ve küçük iç yansıma.
- Kar: altı kollu, açık dallı kristal.
- Rüzgâr: iki kıvrımlı ve bir düz esinti çizgisi.
- Ay: ince hilal ve küçük yıldız.
- Şimşek: konturlu, yuvarlak birleşimli zikzak.
- Gökkuşağı: üç iç içe yay.

Optik ağırlık ortaktır. Güneşte sıcak koyu ton, karda koyu teal, diğerlerinde hafif kırık beyaz kullanılır. Kapalı içerik simge göstermez. Bilinmeyen elementler konturlu elmasla güvenli şekilde gösterilir.

Simge genişliği dolgu bölgesinin %52'sinden %34'üne düşürüldü; yükseklik sınırı %75'ten %50'ye indirildi. Tam dolu şişelerde de maksimum 18 tasarım noktası kullanılır. Büyük grupla birlikte simge büyümez. Gelen içerik aynı simge ailesini kullanır.

Eğriler kesintisiz kenar yumuşatmalı mesh şeritleridir. Vektörler farklı çözünürlüklerde keskin kalır; geçici çizim dizisi üretilmez. Hazne havuzu, aktarım animasyonu, renkler ve oyun kuralları korunur.

Doğrulama: `Logs/weather-symbol-tests.xml` içinde 17/17 hedefli test geçti. Gerçek dokunma, tüm element aktarımı, Undo/tekrar kullanım, iptal ve görsel simge testleri kapsandı. Karma/tam dolu yedi element ve küçük ekran önizlemeleri `Logs/WeatherSymbols` içinde; `WeatherSymbolPreview.RunBatch` ile tekrar üretilebilir.

Unity export ve imzalı Xcode Release derlemesi başarılı. `/private/tmp/cloudora-ios-weather-symbols-20261018/Cloudora.app` iPhone SE’ye kuruldu ve devicectl ile açıldı. Kayıt yedekleri `Logs/device-backup-weather-symbols-20261018` ve `Logs/device-backup-after-weather-symbols-20261018` içinde; kurulum/açılış/kayıt kanıtları `Logs/weather-symbol-device-install.log`, `weather-symbol-device-launch.log`, `weather-symbol-save-check.log`.
