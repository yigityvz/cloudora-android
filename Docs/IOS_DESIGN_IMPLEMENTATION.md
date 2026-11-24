# Cloudora iOS tasarım aktarımı — 17 Ekim 2026

Bu belge ilk aktarımı kaydeder. Sonraki kullanıcı isteğiyle kapsam 100 bölüm / 3 dünya olarak güncellendi; güncel kalite ve kampanya davranışı için `IOS_QUALITY_CAMPAIGN.md` esas alınır.

Figma kaynağı: `PfPK72jwbGPBJ8Ah0RlwWx`. Native Unity uGUI kullanılır; React/Tailwind veya web görünümü eklenmedi. Ham Figma görselleri `Docs/DesignSources`, yerel çalışma zamanı kaynakları `Assets/_Cloudora/Resources/Design` altındadır. Nunito ExtraBold lisansı kaynak klasöründedir.

## Uygulanan ekranlar

- 3 saniyelik mevcut açılış ve azaltılmış hareket desteği.
- İlk 15 bölüm için bulut arka planlı ana sayfa; sonraki bölümler için dünya kartı, gerçek ilerleme, kaynak sayaçları.
- Yeşil Vadi, Rainy Coast ve Frost Peaks dünya resimleri; 13 dünyalık mevcut katalog korunur. Tasarımı bulunmayan diğer dünyalar ortak bulut arka planını kullanır.
- Dünyalar listesi, kilitli/aktif/kurtarılmış durumlar, ziyaret ekranı ve ilk üç dünya için kurtuluş görsel tekrarı.
- Ayarlar: mevcut ses ve titreşim ayarları; azaltılmış hareket tercihi. Destek/gizlilik bağlantıları yalnızca ReleaseConfiguration içinde tanımlıysa gösterilir.
- Oynanış üst kaynak çubuğu, dinamik cam hazneler, hava sembolleri, taşmayan yardımcı butonları ve sayaçları. Haznelerin içerikleri/capacity gerçek oyun verilerinden gelir.
- Duraklatma, bölüm tamamlandı, başarısızlık ve bilgilendirme panelleri; can/jeton/reklam uygunluğu mevcut oyun kodundan alınır.
- 15. bölüm sonunda tanıtım tamamlandı → Yeşil Vadi tanıtımı → bölüm 16; dünya sınırlarında kurtuluş → sonraki dünya tanıtımı. Bu ekranlar ödül vermez veya bölümü kendileri ilerletmez; mevcut devam callback'ini çağırır.

## Korunan işleyiş ve tasarım farkları

- Bölüm verileri, çözücü, hamle/Undo/Shuffle/ek bulut kuralları, ekonomi ve kayıt formatı değiştirilmedi.
- İlk 15 bölümün dünyasız görünümü sunum katmanındadır. WorldCatalog'un 1–20 / 21–40 / 41–60 sınırları ve gerçek restorasyon hesabı aynıdır.
- Figma'nın örnek 500 jeton/ek hazne önerisi uygulanmadı. Mevcut başarısızlık kurtarması 40 jetonla son çözülebilir duruma dönüş olarak doğru adlandırılır. 1–5 ücretsiz tekrar ve mevcut can/reklam seçenekleri korunur.
- Figma'da ilk üç dünyanın kurtuluşu oyunun sonu sayılırken gerçek katalog 13 dünyadır; üçüncü dünyadan sonra oyun sona erdirilmez.
- Kaynak Figma kurtuluş keyframe grubunda export edilebilir motion track yok (`nodes: []`). Tekrar gösterimi üç yerel resim aşaması arasında kısa opaklık geçişi kullanır; azaltılmış harekette fade uygulanmaz.

## Doğrulama

- EditMode: 200 toplam; 196 geçti, 4 mevcut atlama; 0 başarısız (`Logs/ios-editmode.xml`).
- Üç ek geçiş testi: bölüm 15, 20, 40 için keşif ekranının ilerlemeyi değiştirmediği ve yalnızca Oyna düğmesinin mevcut devam eylemini çağırdığı doğrulandı.
- 390×844 ve 375×667 UI render'ları `Logs/DesignPreview`; ana sayfa, ayarlar, liste, oynanış, sonuç panelleri ve milestone ekranları incelendi.
- Unity iOS export: başarılı, 0 hata, 5 mevcut ikon uyarısı (`Logs/ios-build.log`).
- İmzalı cihaz derlemesi ve fiziksel kurulum sonucu `IOS_STATUS.md` içinde izlenir.

## Devam eden görsel kapsam

Bu çalışma ilk ortak ekranları ve geçişleri taşır. Özel mekaniklerin Figma'ya özgü tanıtım varyantları, ayrıntılı zorluk rozetleri, bölüm sonu dünya ilerleme animasyonu ve tüm dünyalar tamamlandı sunumunun 13 dünyaya uyarlanması henüz tamamlanmadı. Bunlar için mevcut ortak paneller kullanılmaktadır. Gerçek cihaz görsel/dokunma kabulü ayrıca yapılmalıdır; render testleri fiziksel oynanış testinin yerine geçmez.
