# Üstten element ekleme ve hazne içi sıvı yükselmesi — 18 Ekim 2026

Kullanıcının son düzeltmesi: onaylanmış elemente özel aktarım korunacak; yalnızca hazne içindeki aynı element birleşmesi/yükselmesi sıvı gibi olacak. Uçuşu sıvı borusuna dönüştürmek kapsam dışıdır.

Önceki üstten giriş hareketi geri getirildi: güneş, yağmur, kar, rüzgâr, hilal, şimşek ve gökkuşağı kendi hareketli simgesi, izi ve küçük parçacıklarıyla kavis çizip hedefin ağzından girer. Şişeler yerinden oynamaz. Hareket profili/ilerideki element desteği korunur. Sıvı şeridi denemesi kaldırıldı.

LiquidFillGraphic ve aynı elementin tek renk bölgesini uzatan önizleme korunur. Böylece gelen ikinci element yeni bir blok/ikinci logo oluşturmaz; yüzey hafif dalgalanarak yükselir ve durulur. Dolu hazne kilidi, ekonomi ve bölüm verileri aynı kalır.

Altı bitiriş sahnesi ve son istenen yeni dolunay sahnesi değişmedi. VesselCompletionFlourish dosya özeti kontrol edildi. Telefonda Cloudora.ReducedMotion=1 bulundu; bu, bitirişleri kısa kenar parlamasına indiriyordu. Kullanıcının normal animasyonları geri getirme talebi doğrultusunda yalnızca bu oyun tercihi 0 yapılır; diğer tercihler ve sistem ayarları değiştirilmez.

Doğrulama: `Logs/item-restore.xml` ilk çalıştırmada 19 testten 17 geçti. İki yeni testteki sorun, beş kapasiteli gerçek bölümün testte dört kabul edilmesi ve sıfır karşılaştırmasındaki kayan nokta toleransıydı. Bunlar düzeltildikten sonra `Logs/item-restore-controller.xml` içinde 4/4 geçti. Gerçek PuzzleController hamlesi bütün yedi elementte hazneyi tamamlayıp ilgili bitirişi açar; bitmiş hazne aktarılamaz. Aynı renkte tek gövde, Geri Al/iptal, dalga sınırları, normal/azaltılmış hareket ve warmed GC.Alloc kontrolleri kapsanır.

Cihaz ilerlemesi kurulmadan önce yedeklendi: bölüm61, tamamlanan60, 3430 altın, 5 can. Kullanıcı bu turda tekrar 60'a alma istemediğinden kayıt aynen korunur. Önceki 100/100 tamamlanmış asıl kayıt ayrıca `Logs/device-backup-before-liquid-20261018` içindedir.

Cihaz sonucu: Unity iOS export 0 hata / 5 mevcut ikon uyarısı; imzalı Xcode Release başarılı (`Logs/item-restore-xcode.log`). `/private/tmp/cloudora-ios-item-restore-20261018/Cloudora.app` iPhone SE’ye silmeden kuruldu ve başlatıldı. Yalnızca oyunun Cloudora.ReducedMotion tercihi 1→0 yapıldı; açılış sonrası tekrar okundu ve 0 doğrulandı. Kayıt dosyasına yazılmadı. İş sırasında oyun kaydı bölüm61/highest60'tan bölüm62/highest61'e ilerlemiş, altın3430→3530 ve Undo3→2 olmuş; can5, diğer haklar aynı. Bu ilerleme geri alınmadı (`Logs/item-restore-save-check.json`). Fiziksel görsel kabul kullanıcı testine açıktır.
