# Cloudora — güncel ekip projesi

**Gökyüzünü düzelt. Dünyayı yeniden canlandır.** Unity6000.5.9f1 ile dikey, tek parmakla oynanan hava sıralama bulmacası.

Yiğit'in24 Kasım2026 kararıyla **güncel taban Bahadır'ın v4.1 dalıdır**: `cloudora-bahadir@5a838a1`. Ekip başlangıç dalı artık **bu reponun main dalı**. Önceki iOS1ee4334/cloudora-yigit kaynak teslimi tarihsel referanstır; bu sürüme eski kuralları dayatmaz.

**Yeni ekip üyesi ve Claude Code önce [CLAUDE.md](CLAUDE.md)'yi okusun.**

- [Güncel ürün ve teknik durum](Docs/GUNCEL_DURUM.md)
- [Aşamalı öğrenme ve ekibe açıklama yönergesi](Docs/EKIP_ONBOARDING.md)
- [Bütün kod, test ve belge okuma haritası](Docs/DOSYA_OKUMA_HARITASI.md)
- [v4.1 bölüm tasarımı](Docs/V41_TASARIM.md) / [100 bölümün görsel haritası](Docs/V41_BOLUM_HARITASI.html)
- [Sonraki işler ve karar bekleyen fikirler](Docs/SONRAKI_ADIMLAR.md)
- [Önceki iOS tabanıyla farklar](Docs/DAL_KARSILASTIRMASI_2026-11-24.md)

## İndir ve aç

Git/Git LFS ve Python3.9+ kur:

```sh
git clone --branch main --single-branch https://github.com/yigityvz/cloudora-android.git cloudora
cd cloudora
git lfs install --local
git lfs pull
git lfs fsck
python3 Tools/verify_reference_snapshot.py
```

Windows Python komutu `py -3` olabilir. Unity Hub'da6000.5.9f1 ile proje kökünü aç; paketleri yükseltme. Başlangıç sahnesi `Assets/_Cloudora/Scenes/02_Gameplay.unity`. Assets, Packages, ProjectSettings ve.meta birlikte kalır. Unity Play Mode ve EditMode Test Runner ile başla.

## Bugünkü oyun

100 bölüm: tanıtım1–14, Yeşil Vadi15–30, Rainy Coast31–60, Frost Peaks61–100. Ay61'de beşinci tür, Karıştır15'te; kapasite1–15'te4,16–80'de5,81–100'de6. Dolu tamamlanan hazne kilitlenir. v4.1 kayıp yalnız legal hamle kalmayınca; çözüm yolu kalmayan ama hamle yapılabilen durumlarda oyuncu yardımcı kullanır.

Bölümler Editor/V41LevelBuilder ile seçilmiş sabit tablodur;2/3 hamle ileri bakan sanal oyuncu ve tuzak ölçüleri vardır. Ödül100/200/300 coin, devam500; başlangıç0coin/5can. Sabit cam, elemente özel üstten aktarım, birleşik sıvı ve7 bitiriş efekti korunur. Detay [güncel sözleşmede](Docs/GUNCEL_DURUM.md).

## Platform ve test kanıtı

Android Build Support/SDK/NDK/OpenJDK gerekir. Dahili QA: `Cloudora/Build/Android Device QA APK`; Windows wrapper `Build-Android.ps1 -Mode qa`. ASCII/boşluksuz yol ve QA/üretim imzası ayrımı için [ANDROID_BUILD](Docs/ANDROID_BUILD.md), [Google Play süreci](Docs/GOOGLE_PLAY_SUBMISSION.md) ve kodu oku.

iOS araçları dalda kalır; [IOS_BUILD](Docs/IOS_BUILD.md) önceki iOS hattının rehberidir. Bu aktarım iOS reposunu veya iPhone kurulumunu değiştirmez.

V4.1 tarihsel test kaydı197 toplam/194 geçti/3 atlama/0 hata; Editor ve Android QA oynanış notları V41_TASARIM/PROJE_HAFIZASI içindedir. Bu main aktarımında kaynak/LFS/belge eşliği kontrol edilir; Unity testleri veya cihaz testleri bugün yeniden yapılmış gibi sunulmaz. FakeAd gerçek reklam, dahili imza mağaza yayını değildir.

## Fikirler ve paylaşım

[Tema Atlası](Docs/TEMA_ATLASI.html) ve [Coin Ekonomisi](Docs/COIN_EKONOMISI.html) geleceğe yönelik seçim sayfalarıdır;101–500/market/IAP mevcut oyuna eklenmiş değildir. Oyuncu kayıtları, Builds/Logs/Library, imza/sertifika ve gizli anahtarlar Git dışındadır. Yeni sürüm etiketi veya mağaza yayını bu teslimde oluşturulmaz.

Katkıda bulunanlar: [@yigityvz](https://github.com/yigityvz), [@bahadir-aktas](https://github.com/bahadir-aktas). Geçmiş kararlar Git ve tarihli belgelerde korunur.
