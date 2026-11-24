# Cloudora — Yiğit'in güncel oyun tabanı

**Gökyüzünü düzelt. Dünyayı yeniden canlandır.** Dikey ekranda kaynak ve hedef hazneye dokunarak hava türlerini ayırdığımız Unity bulmacası.

Bu dal, telefona kurulan **`cloudora-ios/main@1ee4334`** oyun sürümünün `cloudora-android/cloudora-yigit` kaynak teslimidir. Oyun, bölüm verileri, testler, görseller ve Unity ayarları referansla aynıdır. Daha yüksek numaralı başka APK veya `cloudora-bahadir` dalı bu tabanın yerine geçirilmemiştir.

**Yeni ekip üyesi / Claude Code: önce [CLAUDE.md](CLAUDE.md) dosyasını oku.** İçindeki yönerge seni oyun, kod, test, tasarım ve tarihsel belgelerin tamamına doğru sırayla götürür.

- [Güncel durum ve korunacak kararlar](Docs/GUNCEL_DURUM.md)
- [Adım adım öğrenme rotası](Docs/EKIP_ONBOARDING.md)
- [Tüm kod ve belge okuma haritası](Docs/DOSYA_OKUMA_HARITASI.md)
- [Diğer dallarla farklar](Docs/DAL_KARSILASTIRMASI_2026-11-24.md)
- [Sonraki adımlar ve karar bekleyen konular](Docs/SONRAKI_ADIMLAR.md)

## İndir ve aç

Git ve **Git LFS** kullan. Unity **6000.5.9f1** sürümünü kur; paket sürümlerini otomatik yükseltme.

```sh
git clone --branch cloudora-yigit --single-branch https://github.com/yigityvz/cloudora-android.git cloudora-yigit
cd cloudora-yigit
git lfs install --local
git lfs pull
git lfs fsck
python3 Tools/verify_reference_snapshot.py
```

Windows'ta Python komutu `py -3` olabilir. Unity Hub'a bu kök klasörü ekle; sahne `Assets/_Cloudora/Scenes/02_Gameplay.unity`. Kaynaklar, `.meta`, `Packages` ve `ProjectSettings` birlikte tutulur. Play Mode ve EditMode Test Runner ile başla.

## Güncel kapsam

100 bölüm; tanıtım1–14, Yeşil Vadi15–30, Rainy Coast31–60, Frost Peaks61–100. Ay60'ta beşinci elementtir. Bitmiş hazneler kilitli; çıkmaz algısı kazanma yolunun kalmamasına dayanır. Ödüller100/200/300, ücretli devam500, Karıştır16'da. Sabit cam, elemente özel üstten giriş, sıvı iç dolum ve yedi bitiriş efekti korunur. Ayrıntı [güncel sözleşmede](Docs/GUNCEL_DURUM.md).

## Platformlar ve test kanıtı

**iOS referansı:** macOS/Xcode, iOS Build Support; iPhone, iOS15+, IL2CPP/ARM64/Metal, `com.yigityvz.cloudora`. Derleme [IOS_BUILD.md](Docs/IOS_BUILD.md) ve `Build-iOS.sh` ile yapılır. Unity kapalıyken `./Build-iOS.sh test`; kapsam gerekiyorsa `./Build-iOS.sh build`.

**Android kaynak teslimi:** aynı Unity oyun kodunu Android ekibi çekebilir. Android Build Support/SDK/NDK/JDK kurulup Build Profiles'tan Android'e geçilir. Bu eşlemede Android build aracı/profili eklenmedi, APK üretilmedi ve Android cihaz testi yapılmadı. Platform uyarlaması, oyun algoritmasını başka daldan taşımakla aynı iş değildir; [öğrenme rotasındaki Android bölümünü](Docs/EKIP_ONBOARDING.md) oku.

Son kayıtlı iOS test sonucu19 Ekim:241 toplam,235 geçti,6 atlama,0 hata; iOS export, imzalı Xcode Release ve iPhone kurulum/açılış kanıtı var. Bu teslimde oyun değişmediği için eski sonuç **bugün yeniden çalıştırılmış** gibi sunulmaz. Kaynak eşliği manifesto aracıyla doğrulanır. [Durum ve kanıt sınırları](Docs/GUNCEL_DURUM.md).

## Kayıt ve paylaşım

İlerleme `cloudora-save.json` ile yereldir; yedek kurtarma mevcut. Derlemeler, cihaz kayıtları, Library/Logs, imza/sertifika ve gizli bilgiler Git dışında kalır. `CLOUDORA_DEVICE_QA` test ödül servisini açar; gerçek reklam veya mağaza yayını değildir. `main`e birleştirme ve üretim yayını ayrı kullanıcı kararıdır.

Katkıda bulunanlar: [@yigityvz](https://github.com/yigityvz), [@bahadir-aktas](https://github.com/bahadir-aktas). Geçmiş geliştirme kararları Git geçmişinde ve tarihli belgelerde korunur.
