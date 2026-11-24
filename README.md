# Cloudora iOS

**Gökyüzünü düzelt. Dünyayı yeniden canlandır.**

Cloudora, iPhone için Unity ile geliştirilmiş, dikey ekranda oynanan bir hava olaylarını sıralama bulmacasıdır. Bu depo iOS uygulamasının kaynak kodunu, oyun içeriklerini ve Xcode derleme araçlarını içerir.

## Gereksinimler

- macOS ve Xcode
- Unity **6000.5.9f1**, **iOS Build Support** modülüyle
- Git LFS
- Cihazda çalıştırmak için yerel Apple geliştirme imzası ve Geliştirici Modu açık bir iPhone

Hedef: **iOS 15.0+**, iPhone, dikey ekran, **IL2CPP / ARM64 / Metal**. Bundle ID: `com.yigityvz.cloudora`.

## Kurulum

```sh
git clone https://github.com/yigityvz/cloudora-ios.git
cd cloudora-ios
git lfs install --local
git lfs pull
```

Projeyi Unity Hub üzerinden açın. Ana sahne: `Assets/_Cloudora/Scenes/02_Gameplay.unity`.

## Test ve derleme

Aynı projeyi açan Unity oturumunu kapatıp depo kökünde çalıştırın:

```sh
./Build-iOS.sh test
./Build-iOS.sh build
```

Oluşan `Builds/iOS/Cloudora/Unity-iPhone.xcodeproj` dosyasını Xcode ile açın. **Unity-iPhone → Signing & Capabilities** bölümünden kendi Team'inizi seçin, iPhone'u bağlayıp **Run** ile çalıştırın. Mevcut export klasörü doluysa yeniden üretmeden önce başka bir konuma taşıyın.

Ayrıntılar: [iOS derleme rehberi](Docs/IOS_BUILD.md) · [Doğrulama durumu](Docs/IOS_STATUS.md) · [Cihaz testleri](Docs/IOS_DEVICE_QA.md).

## Açılış ekranı

Figma tasarımından alınan bulut ve logo katmanlarıyla animasyonlu açılış, mevcut ana ekrana otomatik geçer. iOS “Hareketi Azalt” ayarı desteklenir. [Uygulama notları](Docs/IOS_OPENING_SPLASH.md).

## Oyun ve proje yapısı

- Oynanabilir kapsam 100 bölüm ve 3 dünyadır: tanıtım 1–14, Yeşil Vadi 15–30, Rainy Coast 31–60, Frost Peaks 61–100. Bölümler mevcut sabit tahta verilerini kullanır; 101 ve sonrası kapalıdır.
- Ay, 60. bölümden itibaren beşinci elementtir. Zorluk ve üretim doğrulaması: [Ay geçişi](Docs/IOS_MOON_FROM_60.md).
- Bulutlar arasında uyumlu hava olayları dokunarak taşınır. Bölümler, zorluk, can, jeton ve yardımcı mekanikleri mevcut oyun sürümüyle korunur.
- İlerleme ve ayarlar yerel JSON kaydında tutulur; yedek kurtarma desteklenir.
- `Assets/_Cloudora/Scripts`: oyun kuralları, arayüz ve servisler.
- `Assets/_Cloudora/Editor`: iOS derleme ve bölüm araçları.
- `Assets/_Cloudora/Tests`: EditMode testleri.
- `Docs`: iOS rehberleri ve bölüm tasarım belgeleri.

Derleme çıktıları, cihaz kayıtları ve imzalama bilgileri Git'e eklenmez. Yerel cihaz derlemesi `CLOUDORA_DEVICE_QA` tanımıyla test reklam/ödül servisini korur; Development Build filigranı içermez. Bu bir App Store dağıtımı değildir; mağaza derlemesi QA tanımı olmadan ayrıca hazırlanmalıdır.

## Katkıda bulunanlar

- [@yigityvz](https://github.com/yigityvz)
- [@bahadir-aktas](https://github.com/bahadir-aktas)
