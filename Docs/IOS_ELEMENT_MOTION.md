# Element aktarımı — 17 Ekim 2026

Bu belge `IOS_UX_REVIEW.md` içindeki eski şişeyi kaldırıp dökme davranışının yerini alır.

- Seçim: altın cam çerçeve, kısa ışık geçişi ve küçük işaret. Şişe büyümez, konumu veya açısı değişmez.
- Aktarım: şişeler sabittir. Element, kaynak yüzeyinden çıkar; ağızların üstünden kavis çizerek hedefe iner. Aynı sütundaki farklı satırlarda yana açılan yol kullanılır.
- Kaynak seviye parçacık çıkışıyla azalır; hedef seviye parçacık varışıyla artar. Veri değişikliği animasyonun sonunda mevcut PuzzleController tarafından bir kez yapılır. Undo, ödül, can ve çözüm kuralları değişmez.
- Tamamlanan hazne yalnızca ışıkla kutlanır; transfer sonrasında da şişe ölçeklenmez.
- Zorluk rozeti 390 pt tasarımında 5 pt yukarı alındı. Bölüm zorluğu ve tahta verileri değiştirilmedi.

| Element | Hareket |
| --- | --- |
| Güneş | Dönen ışınlı küre, sıcak ışık parçacıkları |
| Yağmur | Hareket eden küçük bulut ve damlalar |
| Kar | Dönen altı kollu kristaller, hafif savrulma |
| Rüzgâr | Kıvrılan mint çizgiler ve yanal esinti |
| Ay | Hilal ve yıldız izi |
| Şimşek | Daha hızlı elektrik oku ve kırıklı iz; tekrarlayan flaş yok |
| Gökkuşağı | Altı renkli iz ve yıldız |

## Yeni bölümler ve elementler

Aktarım sistemi bölüm numarası, dünya veya hazne adedi kontrol etmez. Hazne geometrisi ve aktarılan grup büyüklüğünü kullanır; ilk 100 bölümle sınırlı değildir. Yayındaki bölüm sayısı yine 100'dür.

Varsayılan profiller `WeatherMotionProfile.Default` içinde bulunur. Özelleştirmek için Unity Project panelinde **Create → Cloudora → Weather Motion Library** oluşturup `Assets/_Cloudora/Resources/WeatherMotionLibrary.asset` olarak kaydet. Profiles listesinde element, görsel stil, uçuş süresi, yay yüksekliği, simge boyutu, savrulma ve isteğe bağlı renk seçilebilir. MoveAnimator bu dosyayı otomatik yükler; alternatif olarak kendi MotionLibrary alanına atanabilir.

Yeni WeatherType değeri eklenip profil henüz tanımlanmazsa güvenli Essence efekti kullanılır. Yeni profil var olan stillerden birini kullanabilir; farklı çizim gerekiyorsa WeatherMotionStyle ve WeatherFlowGraphic.Symbol genişletilir. Yeni seviyeler için ayrıca animasyon kodu yazılmaz.

## Erişilebilirlik ve temizlik

Azaltılmış hareket açıkken uçan parçacık yerine kısa seviye geçişi uygulanır. Animasyon unscaledDeltaTime kullanır; timeScale sıfırken tamamlanır. İptal veya animatör devre dışı olduğunda geçici görseller kaldırılır, kaynak önizlemesi geri alınır. Geçici Canvas veya şişe render sırası değişikliği yoktur.

## Doğrulama

- `Logs/element-motion-regression.xml`: 223 test, 218 geçti, 5 atlandı, hata yok. Atlamalardan biri grafik gerektiren testtir.
- `Logs/element-motion-final.xml`: grafik açık hedefli doğrulama; yedi element, gerçek dokunma hedefi, aynı sütunda satırlar arası yol, sabit şişe konum/açı/ölçek, iptal, azaltılmış hareket, timeScale=0 ve veri bütünlüğü.
- `Logs/ElementMotionReview`: gerçek Unity render kareleri ve zaman çizelgesi; referans videodaki aktarım kesitleri ayrıca 0,12 saniye aralıkla incelendi.

## iPhone çıktısı

Unity iOS export ve Xcode Release derlemesi başarılı. İmzalı çıktı: `/private/tmp/cloudora-ios-element-motion-20261017/Cloudora.app`. Kurulum mevcut uygulamanın üzerine yapıldı; öncesinde cihaz Documents kaydı `Logs/device-backup-element-motion-20261017` içine yedeklendi. Derleme/kurulum/açılış kanıtları `Logs/element-motion-xcode.log`, `Logs/element-motion-device-install.log`, `Logs/element-motion-device-launch.log`.

Cihaza kurulum başarılıdır. Otomatik açılış telefon kilidi nedeniyle reddedildi (`FBSOpenApplicationErrorDomain 7 / Locked`); cihaz üzerinde etkileşimli animasyon kontrolü kullanıcı tarafından kilit açıldıktan sonra yapılabilir. Grafik doğrulaması gerçek Unity render çıktısında tamamlandı.
