# Cloudora — Claude Code ve yeni ekip üyesi için giriş

**Önce öğren ve ekibe anlat; sonra açık geliştirme görevini uygula.** Bu dosya tek başına oyun değiştirme, bölüm üretme, oyuncu kaydı düzenleme veya yayınlama görevi değildir.

## Güncel taban ve yetki

Yiğit'in24 Kasım2026 son talimatı: **Bahadır'ın dalı son güncel sürüm**, ekip reposunun **main** dalı buna göre hazırlanacak. Kaynak `yigityvz/cloudora-android/cloudora-bahadir@5a838a104cdce93e342c353c930288e08099ceab` (v4.1). Önceki `cloudora-yigit@cc04e5e`/iOS`1ee4334` yönergesi Android main için geçerli ürün tabanı değildir. Daha yüksek numaralı APK'yı otomatik kaynak seçme.

Kullanıcının güncel açık talimatı önceliklidir. Bu teslimde main'e push açıkça yetkilidir; tarihsel hafızadaki başka remote/branch iş akışını buraya uygulama. Sonraki işte yalnız yetkilendirilen hedefe git; force push/yayın/paket yükseltme varsayma. Zaten verilmiş yetkiyi yeniden isteme.

## Okuma sırası — dosyaların tamamına ilerle

1. [GUNCEL_DURUM](Docs/GUNCEL_DURUM.md): bugünkü ürün, mimari ve kanıt sınırları.
2. [EKIP_ONBOARDING](Docs/EKIP_ONBOARDING.md): adım adım bütün oyunu öğrenme ve arkadaşına açıklama rotası; kopyalanabilir başlangıç mesajı.
3. [PROJE_HAFIZASI](Docs/PROJE_HAFIZASI.md) (yeni üst not + tarihsel kayıt), [V41_TASARIM](Docs/V41_TASARIM.md), [v4.1 harita](Docs/V41_BOLUM_HARITASI.html).
4. [DOSYA_OKUMA_HARITASI](Docs/DOSYA_OKUMA_HARITASI.md): bütün runtime/Editor/test C# dosyaları, belgeler, sahne/prefab/ayar/varlık envanteri. Gerçek dosyaları ilgili testleriyle oku; uzun dosyada kesilmiş çıktıyı okundu sayma.
5. [Önceki dalların farkları](Docs/DAL_KARSILASTIRMASI_2026-11-24.md), [Sonraki işler](Docs/SONRAKI_ADIMLAR.md): tarihsel karar, mevcut özellik ve öneriyi ayır.

Her aşamada ekibe sade Türkçeyle anlat: dosya/sorumluluk/bağlantı, oyun davranışı, yan etki, ilgili test ve risk. Sonunda mimari/oyun döngüsü, üretim, ekonomi/kayıt, UI/animasyon, platform ve korunacak alanlar için devir özeti ver. Okuyamadığın dosyayı açık yaz; yalnız sınıf isimlerinden tamlık iddiası kurma.

## İlk güvenli kurulum

```sh
git clone --branch main --single-branch https://github.com/yigityvz/cloudora-android.git cloudora
cd cloudora
git lfs install --local
git lfs pull
git lfs fsck
git status --short
python3 Tools/verify_reference_snapshot.py
```

Git LFS ve Python3.9+ gerekir (Windows `py -3`). Unity6000.5.9f1; Assets/Packages/ProjectSettings/.meta birlikte kalır. Hash aracı salt okunur, gerçek LFS nesnesini ve ek dosyaları kontrol eder. İleride onaylı değişiklik fark üretirse işi geri alma veya manifestoyu sadece kontrol yeşil olsun diye yenileme.

## Kapsam dışında korunacak sözleşmeler

- 100 bölüm; tanıtım1–14, Yeşil Vadi15–30, Rainy31–60, Frost61–100. Ay61 beşinci tür; Shuffle15; kapasite4→16'da5→81'de6.
- Tamamlanan hazne aktarma/Shuffle için kilitli; Undo ayrı davranış. **v4.1 otomatik kayıp yalnız legal hamle kalmayınca**; eski iOS çözümsüzlük tetikleyicisini geri getirme.
- V41LevelBuilder ve yayımlanmış100 tablo deterministiktir. Generated dosya elle yamanmaz; öğrenirken üretim menüsü çalıştırılmaz. Puan, seed, trap/NoMoves/2-3lookahead ölçümü görev olmadan değiştirilmez.
- Ödüller100/200/300, devam500, başlangıç0 coin/5can/3-1-1 hak,30dk yenilenme. Ana sayfaya bitmemiş bölümden çıkış1can; retry1–5 muafiyeti ayrıdır. Devam son çözülebilir duruma dönüş; ekstra hazne ayrı işlevdir.
- Sabit şişe, elemente özel üstten uçuş, birleşik sıvı, küçük simgeler ve7 bitiriş/Hareketi Azalt/yeniden kullanım korunur. Tutorial reserved top ve milestone can yenilemesini kaldırma.
- Save şema2, fail/Undo/modifier geçmişi, backup recovery ve PlayerPrefs korunur. Oyuncu kaydı/ilerlemesi/hakları öğrenme için sıfırlanmaz.
- GUID/.meta, paket kilidi, imza/SDK ve font lisansını koru. Yeni kaynakta.meta ekle; LFS varlıklarını pointer olarak bırakma. Logs/Library/Builds/cihaz kaydı/sertifika/gizli anahtar commit edilmez.

## Geliştirme ve teslim

Kullanıcının kapsamı → ilgili kod/test → dar değişiklik → anlamlı hedef test ve gereken regresyon/platform QA → diff → güncel durum/hafıza/ClaudoraRelease. Tarih-saat için gerçek sistem saatini kullan; kaynakta CRLF varsa koru. Sürüm etiketi/versiyon artışı kendiliğinden yapılmaz.

Doküman tesliminde eski Unity sonucunu bugün çalıştırılmış gibi sunma. QA fake reklamı gerçek SDK sanma. Android build/kurulum/oynanış ve iOS export/imza/kurulum ayrı kanıttır. `TEMA_ATLASI/COIN_EKONOMISI` seçim bekleyen fikirlerdir;101–500/market/IAP kendiliğinden eklenmez. Mevcut kullanıcı yetkisi kapsamındaki işi gereksiz onay döngüsüne sokmadan tamamla.
