# iPhone cihaz testleri

## 16 Ekim 2026 — yapılanlar

- iPhone SE üzerinde yerel imzalı uygulama kurulumu, açılış ve çalışan süreç doğrulandı.
- Mevcut kayıt yedeklendi, uygulama silinmeden güncellendi.
- Kullanıcı geçerli/geçersiz hamle, Undo ve 5→6 geçişini içeren kısa test isteğine “yaptım” yanıtını verdi. Okunan kayıt bölüm 10 / tamamlanan 9 idi.
- Kullanıcının isteğiyle yeni yedek alınıp cihaz test kaydı 80. bölüme getirildi; can ve jeton korundu. Bu değişiklik yalnızca cihaz kaydındadır.
- Tam test matrisi tamamlanmış değildir. Depo temizliği sonrası yeni Unity testleri `IOS_STATUS.md` içinde kaydedilir; cihazdaki oyun yeniden yüklenmedi.

## Kalan kontroller

- Küçük ekran, çentikli ve Dynamic Island iPhone'larda arayüz ve dokunma alanları.
- Tek bulut Shuffle (16. bölümden itibaren), Extra, Undo, başarısızlık ve Retry.
- 40 jeton/test reklamı ile son çözülebilir duruma dönüş; başarısızlık kaydının kapatıp açınca korunması.
- Arka plana alma, ekran kilidi, yeniden açılış, can yenilenmesi ve ayarların kalıcılığı.
- Titreşim, ses ayarı, uzun oturum performansı ve bellek kullanımı.
- İlk 100 bölümden sonraki mevcut modifier mekaniklerinin kontrolü.

Normal tamamlanmamış tahta yeniden açılışta baştan başlar; başarısızlık kaydı ayrı tutulur. Testte mevcut oyun davranışı esas alınır. Kullanıcı kayıtları ve cihaz kimlikleri Git'e eklenmez.

## Açılış sürümü — 16 Ekim

Yeni açılışla imzalı derleme, iPhone kurulumu, başlatma ve çalışan süreç doğrulandı. Mevcut kayıt kurulumdan önce yedeklendi. Cihazda bulut açılışı, logo, ana menüye geçiş ve iOS Hareketi Azalt görünümü için kullanıcı görsel kontrolü bekleniyor.

## 17 Ekim kalite / 100 bölüm sürümü

- Gerçek cihazda üst sayaçlar, panel başlıkları ve küçük buton yazılarının netliğini kontrol et; Development Build filigranı görünmemeli.
- Geçerli aktarımda kavisli renk akışı, hafif eğilme ve dolgu değişimi görülmeli. Hamle sonunda aynı parçalar hedefe taşınmalı; Undo sonucu eski tahtaya dönmeli.
- Ayarlar → Azaltılmış Hareket açıkken akış/eğilme yerine kısa geçiş olmalı.
- Bölüm kazanma panelinden Ana Sayfaya Dön, sonraki bölümü ve yalnızca bir kez verilen ödülü göstermeli.
- Dünya yüzde/sayacı: 16 öncesi 0/25; 16 bitince 1/25 ve %4; 40 bitince Yeşil Vadi %100; 70 bitince Rainy Coast %100; 100 bitince Frost Peaks %100.
- 100 sonrasında 101 açılmamalı; yeniden başlatınca tamamlandı ana sayfası ve üç kurtarılmış dünya görülmeli.
- Yerel QA ödülleri aynı biçimde çalışmalı; ödül mesajları Türkçe olmalı.

## Son UX sürümü — güncel kontrol

- Fotoğraftaki sarı döngüsü (`RRR/NNN/NRSSS/SRNS`, kapasite 5) legal hamle bulunmasına rağmen kayıp olmalı.
- Sonraki hamle/Undo/ek hazne eski analizi iptal etmeli; ek boş hazneli tahta eski kayıp önbelleğini kullanmamalı.
- Aralıklar: dünya 1 15–30; dünya 2 31–60; dünya 3 61–100. 14 bitince dünya tanıtımı açılmalı.
- Bitmemiş bölümde Ana Sayfa −1 can harcamalı; tekrar Oyna başlangıç tahtasını açmalı. Kazanma sonrası Ana Sayfa can harcamamalı. Sıfır canda can kazanma eylemi ana sayfada görünmeli.
- Haznelerde bulut ayağı olmamalı; akışta kaynak ekran içinde kalmalı ve yerine döndüğünde görünür olmalı.
- Her dünyada farklı atmosfer, tüm modallerde okunabilir metin, sabit yardımcı yerleşimi ve sürekli reklam açıklaması olmaması kontrol edilmeli.
