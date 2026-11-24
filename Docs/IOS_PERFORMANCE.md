# Akıcılık ve kalite — 17 Ekim 2026

## Uygulanan optimizasyonlar

- Hedef kare hızı 60 olarak korundu. Ekran çözünürlüğü, render ölçeği, doku boyutu/sıkıştırması, tipografi ve efekt parçacık sayısı azaltılmadı.
- Her hamlede camı ve tüm dolum/simge nesnelerini yok edip oluşturmak kaldırıldı. Cam kalıcıdır; renk bölgeleri ve gelen içerik hazne başına sınırlı bir havuzdan tekrar kullanılır. Undo aynı görsel ağacı günceller.
- Aktarım efekti ve kendi Canvas'ı hamleler arasında tekrar kullanılır. Görsel önizleme iptalde temizlenir, havuzdaki nesne görünmez kalır.
- Çizimde kullanılan daire noktaları, yıldız/şimşek şekilleri ve yol örnekleri tekrar kullanılır. Gökkuşağının altı bandı aynı yol örneklerini paylaşır. Geçici listeler, arayüz üzerinden kutulanan enumerator ve üç parametreli Mathf.Min'in geçici dizisi kaldırıldı.
- Kaynak seviye önizlemesi, en üst bölgeyi her karede isim aramasıyla bulmak yerine doğrudan bölge referansını kullanır.
- Sabit sayaçlar ve kayıp ekranı her karede tekrar yazılmaz. Can/jeton/ilerleme/sayaç saniyesi değiştiğinde güncellenir; kayıt ve ekonomi işlemleri anında yenileme ister. Zaman, can dolumu ve oyun kuralları değişmedi.
- Yerleşim ve arka plan kırpması yalnızca boyut veya içerik değişiminde hesaplanır. Seçim/panel/buton geçişi bitince aynı özellikler tekrar tekrar yazılmaz.
- Dekoratif atmosfer ayrı, dokunma testi yapmayan bir alt Canvas'tadır. Hareketli arka planın sabit arayüzü tekrar gruplatması azaltıldı. Azaltılmış harekette sabit parçacıklar sürekli yeniden çizilmez; tercih ve dünya değişimi yine yenilenir.

Canvas ayırma ve nesne tekrar kullanımı [Unity'nin UI optimizasyon rehberine](https://unity.com/how-to/unity-ui-optimization-tips) göre uygulandı. Cam ve arayüzün görsel ayrıntısı korundu.

## Ölçümler ve sınırlar

Aynı Unity 6000.5.9f1 editöründe, ön hazırlık sonrası her çizim için 200 tekrar ve hazne güncellemesi için 40 tekrar ölçüldü. Bu CPU çizim ölçümüdür; cihaz FPS ölçümü değildir. İlk .NET tahsis sayacı bu editörde desteklenmediğinden o veriler -1 olarak işaretlendi; son doğrulama pozitif kontrolle çalışan Unity GC.Alloc kaydedicisini kullanır.

- Hamle/Undo tekrar testinde cam kimliği ve toplam görsel nesne sayısı sabit kalır; renk bölgesi havuzu kapasite + bir gelen bölge ile sınırlıdır.
- Yedi element için ön hazırlık sonrası 100 tekrarlı çizim testinde geçici managed tahsis sıfırdır. Bu, bütün oyunun her durumda sıfır tahsis yaptığı anlamına gelmez; hamle geçmişi gibi kalıcı veriler hâlâ oluşturulur.
- Kontrol edilen çizimlerin vertex sayıları önceki sürümle aynıdır.
- İlk karşılaştırmada beş ekran birebir aynıydı. Dünya değişimini sabit atmosferde de anında yenileyen son düzeltmeden sonra dört/on hazneli oynanış ve duraklatma hâlâ birebir aynıdır. Ayarlarda 250.125 pikselin 22’si (%0,0088), ana sayfada 1.000.500 pikselin 237’si (%0,0237) değişir: editör önizlemesindeki genel nokta atmosferi doğru dünya yapraklarına yenilenir. Yerleşim, dokular, tipografi ve ayrıntı azaltılmadı.
- CPU süreleri işlem ve makine yüküne göre değişebilir. Tek masaüstü ölçümünden tüm telefonlarda 60 FPS garantisi çıkarılmaz. iOS projesi için hedef 60 FPS'tir; farklı fiziksel modellerde termal/GPU profili ayrıca ölçülmelidir.

## Kanıtlar

- `Logs/PerformanceReview/before/metrics.json`, `after/metrics.json`: işlem süreleri, vertex sayıları ve son GC.Alloc ölçümü.
- `Logs/PerformanceReview/pixel-comparison.json`: beş ekranın piksel karşılaştırması.
- `Logs/performance-regression.xml`: genel set, 225 test; 220 geçti, 5 atlandı, hata yok.
- `Logs/performance-final.xml`: grafik açık hedefli 40 testin tamamı geçti; yedi elementte tahsis, dokunma, çizim, sabit şişeler, iptal, havuz tekrar kullanımı, Undo ve kaynak/arayüz akışları.
- `Logs/ElementMotionReview`: güncel gerçek Unity animasyon kareleri.

- `Logs/performance-static-world.xml`: azaltılmış harekette dünya değişimi için ek regresyon geçti. Son ölçümde yedi efekt, dört atmosfer, cam, panel ve tekrarlanan hazne güncellemesinin tümünde warmed GC.Alloc sayısı sıfır; son piksel karşılaştırmasının ayrıntıları yukarıdadır; üç ekran birebir, diğer ikisinde yalnızca küçük atmosfer farkı vardır.

## Cihaz çıktısı

Unity iOS export ve Xcode Release derlemesi başarılı. İmzalı `/private/tmp/cloudora-ios-performance-20261017/Cloudora.app` bağlı iPhone SE’ye mevcut uygulamanın üzerine kuruldu. Önce/sonra kayıt yedekleri `Logs/device-backup-performance-20261017` ve `Logs/device-backup-after-performance-20261017` içindedir. Telefon kilitli olduğu için otomatik açılış `FBSOpenApplicationErrorDomain 7 / Locked` ile reddedildi. Bu turda fiziksel cihazda animasyon FPS/termal ölçümü yapılmış sayılmaz. Kanıtlar: `Logs/performance-xcode.log`, `performance-device-install.log`, `performance-device-launch.log`, `performance-save-check.log`.
