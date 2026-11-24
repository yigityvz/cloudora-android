# Cloudora açılış animasyonu

Kaynak: Figma `PfPK72jwbGPBJ8Ah0RlwWx`, normal zaman çizelgesi `2028:1974`, bekleme `2028:2050`, azaltılmış hareket `2028:2382`, üretim planı `2028:2126`.

## Kapsam

Yalnızca açılış ekranı eklendi. `MainMenuController.Create` mevcut menüyü oluşturduktan sonra bağımsız `OpeningSplash` katmanını açar. Bulmaca kodu, kayıt, jeton, can, yardımcılar ve mevcut ana ekran değiştirilmedi. Figma'daki `Home_Target` örneği yerine gerçek mevcut menü açılır.

- 0–0,25 sn: bulut perdesi kapalı.
- 0,25–1,45 sn: ön bulutlar 205, arka bulutlar 130 tasarım birimi açılır.
- 0,55–1,3 sn: logo 0,97→1 ölçek ve opaklık/yükseliş geçişi.
- 0,8–1,5 sn: güneş ışığı; 1,4–1,8 sn: kısa parıltı.
- 2,55–3 sn: hazır ana ekrana geçiş. Figma önizleme döngüsü uygulamada bir kez oynatılır.
- İçerik hazır değilse açık bulut durumunda 4 saniyelik hafif bekleme döngüsü sürer.
- iOS Reduce Motion ayarı, yalnızca bu açılış için yerel UIKit sorgusuyla okunur. Hareket/ölçek/parıltı kapatılır; 250 ms opaklık geçişleri kullanılır.
- Oyun zamanı değiştirilmez; animasyon ilk çizilen kareden sonra başlayan gerçek zaman sayacını kullanır; soğuk açılış yükleme süresi üç saniyelik gösterimden düşülmez. Geçiş bitene kadar dokunmalar engellenir, ardından katman kaldırılır.

## Varlıklar

Orijinal bulut ve logo PNG dosyaları Figma tasarım çıktısından alınmıştır; görseller yeniden çizilmemiştir. Güneş/parıltı bağımsız şeffaf vektör katmanlarının PNG export'larıdır; ışık üretim varlığından alınır. Arka plan, tasarımdaki üç renkli gradyanla Unity içinde oluşturulur. Dosyalar `Assets/_Cloudora/Resources/Splash/` altındadır; geçici dış bağlantılar uygulamada kullanılmaz.

UI tasarım boyutu 390×844, genişliğe göre ölçekleme. Bulutlar ekran dışına taşar; logo merkezde kalır. `OpeningSplashPreview.RunBatch`, gerçek Unity UI ile 390×844, 375×667 ve 393×852 önizlemelerini Git dışındaki `Logs/SplashPreview/` altına üretir. Bu araç ayrıca PNG import ayarlarını (alpha, mipmap, trilinear, uncompressed) uygular.

## Doğrulama

Açılışın hazır içerik beklemesi, oyun zamanı durduğunda tamamlanması, dokunma engelinin kaldırılması ve azaltılmış hareket için regresyon testleri eklendi. Güncel test ve cihaz sonuçları `IOS_STATUS.md` içinde tutulur.
