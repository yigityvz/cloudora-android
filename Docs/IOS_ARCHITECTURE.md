# Cloudora iOS mimarisi

> **Android main için okuma notu (24 Kasım2026):** Bu belge iOS/v4 geçmişidir. Güncel taban Bahadır v4.1; Ay61, Shuffle15 ve yalnız No Moves başarısızlığı için [GUNCEL_DURUM](GUNCEL_DURUM.md) ve [V41_TASARIM](V41_TASARIM.md) esas alınır. Görsel/teknik kayıtları yeni kural sanarak eski davranışı geri getirme.

- Oyun: `Assets/_Cloudora/Scripts/` altında saf C# bulmaca kuralları, bölüm verileri, ilerleme, kayıt ve Unity arayüzü.
- Ana sahne: `Assets/_Cloudora/Scenes/02_Gameplay.unity`.
- Editor: iOS derleme aracı, bölüm üretim/kalibrasyon araçları ve içerik doğrulaması.
- Derleme: `Build-iOS.sh` → Unity IL2CPP ARM64/Metal export → Xcode → iPhone.
- Kayıt: `Application.persistentDataPath` altında `cloudora-save.json`; yedek kurtarma mevcut servis tarafından yönetilir.
- Platform hedefi: iPhone, iOS 15+, dikey ekran. İmzalama yereldir; üretilen Xcode projesi Git dışındadır.

Platform temizliğinde oyun kodu, sahneler, bölüm içerikleri ve görseller değiştirilmedi. Unity'nin ortak ProjectSettings ve texture importer şemalarında diğer platformlara ait alanlar bulunabilir; bunlar iOS derleme hedefini değiştirmez. Yeniden üretilecek şema alanlarını elle silmek yerine Unity biçimi korunur.
