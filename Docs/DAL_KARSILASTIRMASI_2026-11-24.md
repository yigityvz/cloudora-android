# cloudora-yigit teslimi ve diğer dallar — 24 Kasım 2026

## Hangi sürüm esas?

Yiğit'in açık kararı: **telefona yüklenen iOS oyunu esas projedir**. Referans `yigityvz/cloudora-ios/main@1ee4334c4591ddf969c14a549f394833af4ed2ef`. Bu kaynak, `yigityvz/cloudora-android/cloudora-yigit` dalına oyun dosyaları değişmeden eşlenir; yeni ekip yönergesi/belgeler ve salt okunur taban kontrolü eklenir.

Git geçmişi korunur: hedef dalın önceki `73880522961d584e1fb76b13a15e8c419b7ebef0` commit'inin üstüne eşleme commit'i, ardından iOS kaynak atasını da içeren ekip teslim merge commit'i gelir. Eski dal ve iOS kaynağı erişilebilir kalır; `--force` kullanılmaz. `main`e birleştirme bu görevde yapılmaz.

## Karşılaştırılan uzak dallar

| Kaynak | Kontrol edilen commit | Durumu |
|---|---|---|
| cloudora-ios/main | `1ee4334c4591ddf969c14a549f394833af4ed2ef` | Yetkili gameplay tabanı; 19 Ekim iPhone sürümü |
| Android/cloudora-yigit (önce) | `73880522961d584e1fb76b13a15e8c419b7ebef0` | Eski prototip/katkıcı kaydı;14 Ekim |
| Android/cloudora-bahadir | `5a838a104cdce93e342c353c930288e08099ceab` |24 Ekim belge commit'i, v4.1 oyun çizgisi |
| Android/main | `39338b3262e482c7d5808068512a276ffb176e22` |27 Ağustos prototipi; yeni teslimle güncellenmez |

Efe'nin v4.5.0 ZIP'i yalnız APK+HTML içeriyordu. `efe/denemeler` dalı bu remote'da görünmedi. APK incelemesi, kaynak dal diff'i değildir; v4.1 sonuçlarıyla karıştırılmadı.

## iOS tabanı ↔ Bahadır v4.1: anlamlı farklar

| Konu | Bu teslim / iOS1ee4334 | Bahadır v4.1 |
|---|---|---|
| Başarısızlık | `BeginSolvabilityCheck/ReachabilitySearch`: kazanma yolu kalmadığı kanıtlanınca | Runtime arama kaldırılmış; sadece `HasLegalMove` false ise kayıp |
| Ay |60'ta beşinci tür, yayımlama wrapper'ı `CampaignMoonRollout` |61'de; wrapper kaldırılmış |
| Karıştır |16 |15 |
| Kapasite |1–18:4,19–100:5 |1–15:4,16–80:5,81–100:6 |
|100 tahta | V3 + Ay60 yayımlanmış katalog | v4.1 builder ile yeniden kurulmuş katalog |
| Aynı başlangıç metni | Yalnız3. bölüm iki dalda aynı board metnine sahip |99 board metni farklı;100 tam girdinin hepsinde en az bir alan farklı |
|100. bölüm | kapasite5, seed784, kayıtlı en kısa18, score34.0321655 | kapasite6, seed262028, kayıtlı en kısa23, score42.5042 |
|60. bölüm | Ay dahil5 tür, seed1962,14 hamle | Ay yok; seed11974,15 hamle |
| Puan/ölçüm | NaiveFailureWeight10; tarihsel kalibrasyon bazı çağrılarda kilitsiz, runtime kilitli | Ağırlık15;2/3 adım ileri bakan oyuncu, wander ayrımı ve kilitli ölçüm eklenmiş |
| Editör üreticileri |V3LevelBuilder/V3LadderBuilder/V3ThemeBuilder + CampaignMoonRollout |V41LevelBuilder; eski builder'lar kaldırılmış, V3LevelTool değiştirilmiş |
| Öğretici yerleşimi | Mevcut üst alan/etiket davranışı | ReservedTop/TutorialClearance, beyaz16→12 punto tek satır düzeni |
| Milestone HUD | Mevcut kaynak yenilemesi |SetLives sırasında milestone kaynak yenilemesi eklenmiş |
| Android platformu | iOS referans araçları ve aynı Unity kaynakları | Android build aracı, manifest postprocessor, profil, JNI paketi ve Android texture override'ları var |

**Ölçüm notu:** Puandaki artış tek başına "daha iyi" veya insan için tam o oranda daha zor demek değildir; formül, ölçüm oyuncusu ve tablolar birlikte değişmiştir. Bir dalın test sayısını diğerinin doğrulaması sayma.

## Ortak kalan önemli alanlar

Karşılaştırılan iki gameplay sürümünde temel `CloudState/PuzzleRules` ve tamamlanmış hazne kilidi,100 sınırı/üç dünya,100/200/300–500 ekonomi ve ana sayfaya çıkış can kuralı korunmuştur. Aktarım/bitiriş ana kodları ve resim içerikleri aynı tabandan gelir; importer `.meta` farkları ayrıca bulunur. Çok sayıda dosya farkının bir kısmı belge veya importer/boşluk farkıdır, yeni özellik sayısı değildir.

Bu nedenle Bahadır v4.1, Efe APK'sındaki yeni `App/Board/GameScreen`,11 element,26 bölge/260 bölüm, başlangıç500, Shuffle6 ve farklı kayıt sistemiyle aynı paket olarak anlatılamaz.

## Yararlı olabilecek dar taşıma adayları

1. Öğretici yazısı ile tahta arasında yer ayırma; özellikle iki sıralı tahtayı test ederek.
2. Dünya kurtuluş ekranında can sayacının anlık yenilenmesi.
3. Android build/manifest/texture uyarlaması; sadece platform alanları seçilerek, gerekli izinler kontrol edilerek.
4. İleriye bakan sanal oyuncu ölçümü; başlangıçta salt rapor modu olarak ve gameplay/formül/tabloyu değiştirmeden.

Bunlar **bu teslimde alınmadı**. Tüm controller/solver dosyasını merge etmek ilk iki düzeltmeyle birlikte başarısızlık kararını ve zorluğu da değiştirir. Sonraki görevde kullanıcı hangi farkı istiyorsa dar bir değişiklik ve test hazırlanmalıdır.

## main için karar kontrolü

- Güncel oyun tabanını kullanıcı onayladı mı? Bu görevde evet: iOS1ee4334. Başka dalın davranışlarını taşımaya toplu onay yok.
- Android paketleme ve fiziksel cihaz kabulü tamam mı? Bu eşlemede hayır; yeni Android build yapılmadı.
- Yeni ekip yönergesi ve tam kaynaklar LFS ile indirilebiliyor mu? Teslim kontrolü buna odaklanır.
- Ek UI düzeltmeleri seçildi mi? Henüz ayrı karar verilmedi.
- `main`e taşıma talimatı verildi mi? Hayır; kullanıcı karşılaştırma sonrası karar verecek.

Ham karşılaştırma ve100 girdinin alanları: [DAL_KARSILASTIRMASI_VERI.json](DAL_KARSILASTIRMASI_VERI.json). Bu JSON'daki dosya listesi **iOS1ee4334 → Bahadır5a838a1** yönündedir; yeni ekip belgelerinin eklenmesi gameplay diff'ine dahil edilmez.
