# Cloudora — Claude Code giriş yönergesi

Bu dosya yeni ekip üyesinin ve Claude Code'un ilk okuyacağı dosyadır. Önce **oyunu öğren**, sonra verilen geliştirme görevini uygula. Bu dosyanın okunması kendi başına kod değiştirme, bölüm üretme, paket güncelleme, cihaz kaydı değiştirme veya yayınlama görevi değildir.

## Yetki ve güncel taban

- Proje sahibi Yiğit'tir. Ürün tabanı telefona kurulan ve `yigityvz/cloudora-ios/main` üzerinde bulunan **`1ee4334c4591ddf969c14a549f394833af4ed2ef`** oyun sürümüdür.
- Bu teslimin hedefi **`yigityvz/cloudora-android` → `cloudora-yigit`**. Bu dal aynı Unity oyun kaynaklarını ve ekip belgelerini taşır. `cloudora-bahadir` v4.1 ve Efe'nin v4.5.0 APK'sı ayrı geliştirme çizgileridir; sürüm numaraları daha büyük diye taban yerine geçirilmez.
- Android reposunda bulunmak, bu teslimde Android APK derlenip fiziksel cihazda doğrulandığı anlamına gelmez. iOS referansı ve Android kaynak aynalamasını ayır.
- Kullanıcının güncel açık talimatı önceliklidir. Bir geliştirme görevi belirli bir değişikliği zaten yetkilendiriyorsa sırf bu belgede korumalı alan sayıldığı için tekrar izin isteme. Kapsam dışındaki başka davranışları kendiliğinden değiştirme.

## Zorunlu okuma sırası

1. [Güncel ürün ve teknik durum](Docs/GUNCEL_DURUM.md): bugünkü kurallar, kapsam, istisnalar, eski belgelerle çelişkiler.
2. [Ekip onboarding ve öğrenme rotası](Docs/EKIP_ONBOARDING.md): aşama aşama dosya sırası; her aşamada geliştiriciye ne anlatılacağı; güvenli çalışma biçimi.
3. [Eksiksiz dosya okuma haritası](Docs/DOSYA_OKUMA_HARITASI.md): **bütün runtime, Editor ve test C# dosyaları**, sahne/prefab/ayar/kaynak ve doküman envanteri. Sadece aşağıdaki özetle yetinme.
4. Haritadaki sırayla gerçek kodu ve ilgili testleri oku. Uzun dosyaları parçalara böl; atlanan/truncate edilen bölümü okundu sayma. `.meta` GUID'lerinin ve görsel/font kaynaklarının bağlarını da incele.
5. [Dal karşılaştırması](Docs/DAL_KARSILASTIRMASI_2026-11-24.md): diğer dalın faydalı düzeltmeleri ile ürün davranışını değiştiren kararlarını ayır.
6. [Sonraki işler](Docs/SONRAKI_ADIMLAR.md): kabul edilmiş iş, öneri ve ayrı deneyleri karıştırma.

Bunları okuduktan sonra ekip üyesine Türkçe, anlaşılır bir açıklama yap: oyun döngüsü; başlangıç ve sahne; bölüm oluşturma/yayımlama; ekonomi/kayıt; çıkmaz/Undo/devam; UI/animasyon; test/derleme; korunan alanlar; açık konular. Dosya adlarını göster. Sonunda kendi kendini denetleme sorularını cevapla. Bilmediğin veya okuyamadığın yeri açıkça yaz; "her şeyi öğrendim" demek yerine okuma kapsamını göster.

## İlk güvenli komutlar

```sh
git status --short
git branch --show-current
git log -1 --oneline
git remote -v
git lfs version
git lfs pull
python3 Tools/verify_reference_snapshot.py
```

Python 3.9+ kullan. Windows'ta Python komutu `py -3` olabilir. LFS yoksa önce kur; pointer metnini görsel sanma. `verify_reference_snapshot.py`, bu teslimin oyun/ayar/varlık dosyalarının referansla eşliğini kontrol eder; dosya değiştirmez. Meşru yeni geliştirmede fark çıkması, onaylı işi geri almak veya manifesto özetlerini körlemesine yenilemek için gerekçe değildir.

## Açıkça korunacak davranışlar

- Kampanya **100** bölüm; tanıtım **1–14**; Yeşil Vadi **15–30**, Rainy Coast **31–60**, Frost Peaks **61–100**. 101+ oynanabilir değildir.
- Ay **60'ta beşinci element** olarak gelir. Mevcut yayımlanmış tabloları yeniden üretme; `V3LevelData.Generated.cs` elle düzenlenmez.
- Tamamlanmış hazne normal aktarma ve Karıştır için kilitlidir. Geri Al'ın önceki hamleyi geri çevirmesi ayrı davranıştır.
- **Legal hamle olması kazanma yolu olduğu anlamına gelmez.** Başarısızlık, güncel iOS'taki çözümsüzlük aramasına dayanır. Bunu sadece `HasLegalMove` kontrolüne düşürme.
- Ödüller **100 / 200 / 300**; ücretli devam **500**; başlangıç coin **0**; Karıştır **16**'da. Can **5**, yenilenme **30 dakika**.
- Bitmemiş bölümden ana sayfaya çıkış **1 can** harcar, bölümü başa alır; kazanılmış bölümden dönüş ücretsizdir. Başarısızlıkta yeniden deneme için 1–5'in can muafiyeti farklı bir kuraldır.
- Şişeler aktarma sırasında sabittir. Element, kendi simge/iz/parçacıklarıyla üstten girer. Hazne içindeki aynı element tek sıvı gövdesi olarak yükselir; ikinci blok/logo veya kesintisiz sıvı borusu eklenmez.
- Yedi bitiriş efekti korunur; Ay'ın son tasarımı gölgeden çıkan kraterli dolunaydır. Küçük vektör simgeler, bulutsuz hazne tabanı, Retina netlik ve Hareketi Azalt desteği korunur.
- Kayıt şeması, isimleri, yedek kurtarma ve hata geçmişi korunur. Cihaz ilerlemesini, coin/can/hakları veya tercihleri bir test kolaylığı için izinsiz sıfırlama.

## Değişiklik, test ve teslim protokolü

- Önce ilgili alanın kodunu **ve testini** oku; değişiklikten etkilenen durumları anlat. Mevcut değişiklikleri koru. `main`e otomatik birleştirme veya force push yapma.
- Başka dalı komple merge/cherry-pick etmek yerine hangi davranışın taşındığını incele. Özellikle `PuzzleSolver`, `PuzzleController`, bölüm verisi ve `V3LevelPlan` birlikte değerlendirilir.
- Sırf doküman güncelliyorsan runtime testlerini yeniden çalıştırılmış gibi gösterme. Kod değişikliğinde anlamlı hedef testleri, gerekli regresyon ve platform doğrulamasını çalıştır; atlamaları açık yaz.
- Gameplay dosyaları ve varlıklar için [korunan taban manifestosu](Docs/REFERANS_TABAN.json) ve doğrulama aracını kullan. Manifesto bugünkü referanstır; gelecek onaylı sürümde değişikliği ayrı kayıt ve test kanıtıyla güncelle.
- Yeni kaynak dosyasında Unity `.meta` dosyasını koru/ekle. Görsel/fontlar Git LFS ile gider. GUID, paket sürümleri, proje kimliği, imza ve mağaza hedefleri gelişigüzel değiştirilmez.
- SDK/reklam/analytics etkinleştirme, izin ekleme, sertifika/Team ID ve mağaza yayını ayrı kapsamdır. `CLOUDORA_DEVICE_QA` test servisidir, gerçek reklam geliri değildir.
- `Logs`, `Library`, `Builds`, cihaz kayıtları, sertifikalar, gizli anahtarlar ve yerel IDE dosyaları commit edilmez.
- Görev sonunda ne değiştiğini, neyin test edildiğini/edilmediğini ve kalan riskleri ekip üyesine anlat. `Docs/GUNCEL_DURUM.md`, `Docs/PROJE_HAFIZASI.md` ve `ClaudoraRelease.md` ilgiliyse güncellenir; tarihsel kaydı bugünkü kural gibi yeniden yazma.
