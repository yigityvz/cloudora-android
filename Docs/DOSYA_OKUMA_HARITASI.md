# Eksiksiz dosya okuma haritası

Bu envanter, `1ee4334` kaynak tabanı ve bu teslimin ekip belgeleri için hazırlanmıştır. [Öğrenme rotası](EKIP_ONBOARDING.md) aşama sırasını belirler. Buradaki sınıf/metot adları **arama ipucudur**, dosyanın okunduğunun veya testin çalıştığının kanıtı değildir. Kodun tamamını, serileştirilmiş bağları ve ilgili testleri incele.

## Kapsam ve okuma kaydı

Her dosya için kendi inceleme notunda `okundu / kalan bölüm`, sorumluluk, çağıran/çağrılan, state yan etkisi, risk ve test karşılığını tut. Uzun `PuzzleController`, çizim kodları ve generated tablo parçalara ayrılmalı; araç çıktısı kesildiyse kalan bölüm okunmalıdır. Binary dosyalar metin gibi okunmaz: kullanım yeri, çözünürlük, importer, LFS ve lisans kontrol edilir.

Bu tabanda **106 C# dosyası**: 65 runtime, 14 Editor araç, 27 test/probe. [Referans manifestosu](REFERANS_TABAN.json) 478 oyun/varlık/ayar dosyasını byte/hash düzeyinde kapsar.

## Scripts/Core

5 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Core/CloudState.cs](../Assets/_Cloudora/Scripts/Core/CloudState.cs) — 60 satır. Tanımlar: `CloudState`. Tek haznenin veri modeli; üst grup, doluluk, tamamlanma ve uyumluluk.
- [ ] [Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs](../Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs) — 44 satır. Tanımlar: `MobileRuntimeBootstrap`. Mobil başlangıç, kare hedefi ve Canvas korumaları.
- [ ] [Assets/_Cloudora/Scripts/Core/Move.cs](../Assets/_Cloudora/Scripts/Core/Move.cs) — 22 satır. Tanımlar: `Move`. Hamlenin kaynak/hedef/tür/miktar verisi.
- [ ] [Assets/_Cloudora/Scripts/Core/PuzzleRules.cs](../Assets/_Cloudora/Scripts/Core/PuzzleRules.cs) — 50 satır. Tanımlar: `PuzzleRules`. Saf hamle/kazanma sözleşmesi; kaynak/kapasite/üst tür ve tamamlanmış hazne kilidi.
- [ ] [Assets/_Cloudora/Scripts/Core/PuzzleState.cs](../Assets/_Cloudora/Scripts/Core/PuzzleState.cs) — 18 satır. Tanımlar: `PuzzleState`. Tahtanın saf veri modeli ve durum kopyaları.

## Scripts/Modifiers

5 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs](../Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs) — 55 satır. Tanımlar: `PassiveModifierRule`, `FrozenModifierRule`, `LockedModifierRule`, `FogModifierRule`, `RainbowModifierRule`, `WindModifierRule`, `NightModifierRule`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs](../Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs) — 18 satır. Tanımlar: `ICloudModifierRule`, `IMoveCountModifier`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs) — 15 satır. Tanımlar: `ModifierData`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs) — 103 satır. Tanımlar: `ModifierRuntime`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs) — 13 satır. Tanımlar: `ModifierType`.

## Scripts/Level

14 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs](../Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs) — 69 satır. Tanımlar: `AuthoredLevelCatalog`. İlk15 kayıt ve öğretici işaretleri; ürün intro1–14 sınırıyla ayrımı oku.
- [ ] [Assets/_Cloudora/Scripts/Level/BalanceProfile.cs](../Assets/_Cloudora/Scripts/Level/BalanceProfile.cs) — 41 satır. Tanımlar: `DifficultyBand`, `BalanceProfile`.
- [ ] [Assets/_Cloudora/Scripts/Level/BoardFrame.cs](../Assets/_Cloudora/Scripts/Level/BoardFrame.cs) — 37 satır. Tanımlar: `BoardFrame`.
- [ ] [Assets/_Cloudora/Scripts/Level/CloudShuffler.cs](../Assets/_Cloudora/Scripts/Level/CloudShuffler.cs) — 114 satır. Tanımlar: `CloudShuffleRejection`, `CloudShuffleResult`, `CloudShuffler`, `FailRecovery`. Tek hazne güvenli karıştırma ve aday değerlendirme; tamamlanan hazne ve başarısız ücret davranışı.
- [ ] [Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs](../Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs) — 35 satır. Tanımlar: `DeterministicRandom`.
- [ ] [Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs](../Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs) — 27 satır. Tanımlar: `DifficultyCalculator`.
- [ ] [Assets/_Cloudora/Scripts/Level/LevelDefinition.cs](../Assets/_Cloudora/Scripts/Level/LevelDefinition.cs) — 54 satır. Tanımlar: `LevelDefinition`, `CloudDefinition`.
- [ ] [Assets/_Cloudora/Scripts/Level/LevelGenerator.cs](../Assets/_Cloudora/Scripts/Level/LevelGenerator.cs) — 145 satır. Tanımlar: `LevelGenerator`.
- [ ] [Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs](../Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs) — 563 satır. Tanımlar: `SolveStatus`, `NaivePlayerStyle`, `DifficultyMetrics`, `PuzzleSolveResult`, `PuzzleSolver`, `ReachabilitySearch`, `StateKey`. Arama, kanonik durum, tamamlanmış hazne kilidi, resumable reachability ve hata kurtarma. Limit ile çözümsüzlük kanıtını ayır.
- [ ] [Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs](../Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs) — 74 satır. Tanımlar: `PuzzleValidator`.
- [ ] [Assets/_Cloudora/Scripts/Level/V3BoardText.cs](../Assets/_Cloudora/Scripts/Level/V3BoardText.cs) — 41 satır. Tanımlar: `V3BoardText`.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs) — 120 satır. Tanımlar: `V3LevelData`. Yayımlanmış 100 kayıt. Tamamını oku, elle değiştirme ve okuma sırasında üretme.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelData.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.cs) — 40 satır. Tanımlar: `V3LevelEntry`, `V3LevelData`. Yayımlanmış girdilerin erişim/model katmanı.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs](../Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs) — 69 satır. Tanımlar: `V3Part`, `V3LevelPlan`. Bölüm planı, bant/part/frame ve palet kuralları; yayımlanan tabloyla birlikte değerlendir.

## Scripts/Progression

7 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Progression/BoosterManager.cs](../Assets/_Cloudora/Scripts/Progression/BoosterManager.cs) — 41 satır. Tanımlar: `BoosterManager`. Undo/Ek Bulut/Karıştır hakları ve harcama.
- [ ] [Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs](../Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs) — 47 satır. Tanımlar: `CoinEconomy`, `CoinWallet`. 100/200/300 ödül ve500 devam ücretinin merkezi kuralları.
- [ ] [Assets/_Cloudora/Scripts/Progression/LifeManager.cs](../Assets/_Cloudora/Scripts/Progression/LifeManager.cs) — 54 satır. Tanımlar: `LifeManager`. 5 can, UTC30dk yenilenme ve güvenli düşürme/sınır.
- [ ] [Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs](../Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs) — 29 satır. Tanımlar: `ProgressionManager`. Bölüm ilerlemesi, açılma ve100 sonu.
- [ ] [Assets/_Cloudora/Scripts/Progression/RestorationManager.cs](../Assets/_Cloudora/Scripts/Progression/RestorationManager.cs) — 17 satır. Tanımlar: `RestorationManager`. Dünya yüzdesini tamamlanan bölümlerden hesaplar.
- [ ] [Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs](../Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs) — 27 satır. Tanımlar: `WorldCatalog`. 100 sınırı ve dünya aralıkları; eski geniş katalogla karıştırma.
- [ ] [Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs](../Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs) — 34 satır. Tanımlar: `WorldDefinition`. Dünya verisi; eski Capacity metadata alanını gerçek bölüm kapasitesi sanma.

## Scripts/Services

14 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Services/AdMobAdService.cs](../Assets/_Cloudora/Scripts/Services/AdMobAdService.cs) — 17 satır. Tanımlar: `AdMobAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs](../Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs) — 27 satır. Tanımlar: `AdPlacementPolicy`.
- [ ] [Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs](../Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs) — 23 satır. Tanımlar: `AnalyticsEvents`.
- [ ] [Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs) — 20 satır. Tanımlar: `DebugAnalyticsService`.
- [ ] [Assets/_Cloudora/Scripts/Services/FailRecord.cs](../Assets/_Cloudora/Scripts/Services/FailRecord.cs) — 84 satır. Tanımlar: `FailedSnapshot`, `FailRecord`. Başarısız denemenin tahta/hamle/Undo/modifier geçmişi.
- [ ] [Assets/_Cloudora/Scripts/Services/FakeAdService.cs](../Assets/_Cloudora/Scripts/Services/FakeAdService.cs) — 28 satır. Tanımlar: `FakeAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs) — 42 satır. Tanımlar: `FirebaseAnalyticsService`, `AnalyticsPrivacy`.
- [ ] [Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs](../Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs) — 55 satır. Tanımlar: `GameFeedbackService`. Titreşim/ses tercihi ve platform geri bildirimi.
- [ ] [Assets/_Cloudora/Scripts/Services/IAdService.cs](../Assets/_Cloudora/Scripts/Services/IAdService.cs) — 13 satır. Tanımlar: `RewardedPlacement`, `IAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs) — 9 satır. Tanımlar: `IAnalyticsService`.
- [ ] [Assets/_Cloudora/Scripts/Services/ISaveService.cs](../Assets/_Cloudora/Scripts/Services/ISaveService.cs) — 8 satır. Tanımlar: `ISaveService`.
- [ ] [Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs](../Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs) — 72 satır. Tanımlar: `LocalJsonSaveService`. JSON yazma/okuma, geçici dosya ve yedekten kurtarma; oyuncu kaydını koru.
- [ ] [Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs](../Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs) — 32 satır. Tanımlar: `ReleaseConfigurationData`, `ReleaseConfiguration`.
- [ ] [Assets/_Cloudora/Scripts/Services/SaveData.cs](../Assets/_Cloudora/Scripts/Services/SaveData.cs) — 26 satır. Tanımlar: `SaveData`. Persist edilen alanlar ve şema2; alan adı/tür değişikliği göç gerektirir.

## Scripts/Puzzle

5 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs](../Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs) — 484 satır. Tanımlar: `CloudContainerView`, `EssenceRegion`. Hazne input/seçim, içerik ve görünüm; cam sabit kalır.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs](../Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs) — 213 satır. Tanımlar: `MoveAnimator`, `WeatherFlowGraphic`. Kaynak/hedef animasyon yaşam döngüsü ve modelle senkronizasyon.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs](../Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs) — 809 satır. Tanımlar: `PuzzleController`, `PuzzleSnapshot`. Ana orkestrasyon; başlangıç, input kilidi, hamle/Undo, çözümsüzlük, ekonomi, kayıt ve ekran geçişleri. Bütün metotları parça parça oku.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs) — 62 satır. Tanımlar: `WeatherMotionStyle`, `WeatherMotionProfile`, `WeatherMotionLibrary`. Elemente özel uçuş/iz/parçacık dili; iç sıvı hareketinden ayrıdır.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs) — 17 satır. Tanımlar: `WeatherType`. Enum sırası, 7 tanımlı tür; yayımlanan 100 bölümdeki5 türden ayrıdır.

## Scripts/UI

15 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs](../Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs) — 76 satır. Tanımlar: `AdaptiveBoardLayout`, `LayoutMetrics`.
- [ ] [Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs](../Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs) — 140 satır. Tanımlar: `CloudoraVisuals`, `SkyGradientGraphic`.
- [ ] [Assets/_Cloudora/Scripts/UI/DesignUI.cs](../Assets/_Cloudora/Scripts/UI/DesignUI.cs) — 156 satır. Tanımlar: `DesignUI`, `DesignScreenFit`, `DesignPanel`, `EssenceSymbolFit`. Programatik arayüz, font/ölçek/stil ve yeniden kullanım.
- [ ] [Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs](../Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs) — 169 satır. Tanımlar: `FailOptions`, `GameplayOverlay`, `DesignModalFit`.
- [ ] [Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs](../Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs) — 53 satır. Tanımlar: `LiquidFillGraphic`. Birleşik sıvı yüzeyi, yükselme/dalga ve mesh üretimi.
- [ ] [Assets/_Cloudora/Scripts/UI/MainMenuController.cs](../Assets/_Cloudora/Scripts/UI/MainMenuController.cs) — 139 satır. Tanımlar: `MainMenuController`.
- [ ] [Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs](../Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs) — 79 satır. Tanımlar: `MilestoneScreen`.
- [ ] [Assets/_Cloudora/Scripts/UI/OpeningSplash.cs](../Assets/_Cloudora/Scripts/UI/OpeningSplash.cs) — 215 satır. Tanımlar: `OpeningSplash`. Yaklaşık3sn açılış ve geçiş/hareket tercihi.
- [ ] [Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs](../Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs) — 48 satır. Tanımlar: `SafeAreaFitter`.
- [ ] [Assets/_Cloudora/Scripts/UI/VectorShapes.cs](../Assets/_Cloudora/Scripts/UI/VectorShapes.cs) — 134 satır. Tanımlar: `UIShapes`, `GameIcon`, `ButtonMotion`, `PanelPresence`, `GlassVesselGraphic`, `VesselFillMask`.
- [ ] [Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs](../Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs) — 446 satır. Tanımlar: `VesselCompletionFlourish`. Yedi elementin tamamlanma sahneleri, havuz ve azaltılmış hareket.
- [ ] [Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs](../Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs) — 117 satır. Tanımlar: `WeatherGlyph`. Küçük monoline element ikonları ve boyutlandırma.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs](../Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs) — 57 satır. Tanımlar: `WorldAtmosphere`, `GameplayBackdrop`.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs](../Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs) — 39 satır. Tanımlar: `WorldRescueReplay`.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldScreenController.cs](../Assets/_Cloudora/Scripts/UI/WorldScreenController.cs) — 90 satır. Tanımlar: `WorldScreenController`.

## Editor

14 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Editor/CampaignMoonRollout.cs](../Assets/_Cloudora/Editor/CampaignMoonRollout.cs) — 119 satır. Tanımlar: `CampaignMoonRollout`. Ay60 beşinci tür yayımlama katmanı; temel üreticiyi tek başına yeniden çalıştırmak bunu kaybettirebilir.
- [ ] [Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs](../Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs) — 78 satır. Tanımlar: `CloudoraIosBuildTools`. iOS yapılandırma/derleme/test Editor girişleri; Android aracı değildir.
- [ ] [Assets/_Cloudora/Editor/ContentValidationTool.cs](../Assets/_Cloudora/Editor/ContentValidationTool.cs) — 38 satır. Tanımlar: `ContentValidationTool`. İçerik doğrulama aracı; legacy kapsamını güncel100/UI kabulünden ayır.
- [ ] [Assets/_Cloudora/Editor/DesignPreview.cs](../Assets/_Cloudora/Editor/DesignPreview.cs) — 70 satır. Tanımlar: `DesignPreview`.
- [ ] [Assets/_Cloudora/Editor/DesignQuality.cs](../Assets/_Cloudora/Editor/DesignQuality.cs) — 24 satır. Tanımlar: `DesignQuality`.
- [ ] [Assets/_Cloudora/Editor/LevelCandidateFactory.cs](../Assets/_Cloudora/Editor/LevelCandidateFactory.cs) — 96 satır. Tanımlar: `LevelCandidateFactory`.
- [ ] [Assets/_Cloudora/Editor/LevelCandidateSelector.cs](../Assets/_Cloudora/Editor/LevelCandidateSelector.cs) — 122 satır. Tanımlar: `ScoreWindow`, `LevelCandidate`, `CandidateSelection`, `LevelCandidateSelector`.
- [ ] [Assets/_Cloudora/Editor/OpeningSplashPreview.cs](../Assets/_Cloudora/Editor/OpeningSplashPreview.cs) — 72 satır. Tanımlar: `OpeningSplashPreview`.
- [ ] [Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs](../Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs) — 65 satır. Tanımlar: `RenderingPerformanceBenchmark`, `Row`, `Report`.
- [ ] [Assets/_Cloudora/Editor/V3LadderBuilder.cs](../Assets/_Cloudora/Editor/V3LadderBuilder.cs) — 324 satır. Tanımlar: `V3Novelty`, `V3LadderBuilder`, `PartConfig`.
- [ ] [Assets/_Cloudora/Editor/V3LevelBuilder.cs](../Assets/_Cloudora/Editor/V3LevelBuilder.cs) — 149 satır. Tanımlar: `V3LevelResult`, `V3LevelBuilder`.
- [ ] [Assets/_Cloudora/Editor/V3LevelTool.cs](../Assets/_Cloudora/Editor/V3LevelTool.cs) — 299 satır. Tanımlar: `V3LevelTool`, `V3LevelData`. Editor üretim/kalibrasyon/yayımlama menüleri; öğrenirken üretim komutu çalıştırma.
- [ ] [Assets/_Cloudora/Editor/V3ThemeBuilder.cs](../Assets/_Cloudora/Editor/V3ThemeBuilder.cs) — 365 satır. Tanımlar: `V3ThemeBuilder`, `LayoutStep`, `Segment`, `ThemeConfig`.
- [ ] [Assets/_Cloudora/Editor/WeatherSymbolPreview.cs](../Assets/_Cloudora/Editor/WeatherSymbolPreview.cs) — 39 satır. Tanımlar: `WeatherSymbolPreview`.

## Tests/Editor

27 dosya. Dosya yolları tamdır; sıra için öğrenme rotasına dön.

- [ ] [Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs](../Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs) — 51 satır. Tanımlar: `AbandonAttemptTests`, `MemorySave`.
  İnceleme/test girişleri: `Save`, `HomeFromActiveAttemptSpendsOneLifeAndRestoresOriginalBoardButWinIsFree`.
- [ ] [Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs](../Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs) — 59 satır. Tanımlar: `CampaignReleaseTests`.
  İnceleme/test girişleri: `ExactlyThreeWorldsCoverAllPostIntroLevelsThrough100`, `EveryWorldProgressUsesOnlyItsOwnCompletedLevels`, `WorldBoundariesMatchTheApprovedCampaign`, `FinishedCampaignNeverLoadsOrSavesLevel101`, `WinHomeButtonCallsHomeWithoutAwardingOrContinuing`, `TransferPreviewDoesNotMutateBoardAndCanBeCancelled`.
- [ ] [Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs) — 110 satır. Tanımlar: `CompletionEconomyTests`, `MemorySave`.
  İnceleme/test girişleri: `EveryLevelUsesTheRequestedRewardCycleAndContinueFee`, `CompletedSourcesAreImmutableInCoreViewAndShuffleIncludingWildcards`, `Save`, `ControllerCreditsOnlyOnceAndCharges500OnlyForSuccessfulContinue`, `RealControllerMoveTriggersEveryElementFinishAfterTopEntry`.
- [ ] [Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs) — 115 satır. Tanımlar: `CompletionFlourishTests`.
  İnceleme/test girişleri: `RichCompletionMeshesStayBoundedAndAllocateNoGarbageAfterWarmup`, `FinishCrownsFitTheHudAndDenseBoardEdges`, `AllSevenFinishesKeepTheGlassFixedAndCleanUpOnUndo`.
- [ ] [Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs](../Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs) — 29 satır. Tanımlar: `DesignNavigationTests`.
  İnceleme/test girişleri: `MilestoneDoesNotAdvanceUntilPlayIsPressed`.
- [ ] [Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs](../Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs) — 91 satır. Tanımlar: `ElementMotionTests`.
  İnceleme/test girişleri: `EveryElementHasItsOwnStyleAndUnknownElementsHaveSafeFallback`, `FutureElementCanUseAuthoredProfileWithoutChangingAnimator`, `AllElementsKeepBothVesselsFixedAndCancellationRestoresPreviews`.
- [ ] [Assets/_Cloudora/Tests/Editor/LevelToolTests.cs](../Assets/_Cloudora/Tests/Editor/LevelToolTests.cs) — 476 satır. Tanımlar: `LevelToolTests`.
  İnceleme/test girişleri: `SolvedBoardNeedsZeroMoves`, `OneMoveBoardIsSolvedInOneMove`, `BoardWithoutLegalMovesIsUnsolvable`, `SolverRejectsMalformedBoards`, `SolverDoesNotMutateInputBoard`, `SolutionsReplayWithGameRulesAndHaveOptimalLength`, `OptimalMovesMatchIndependentReferenceSearch`, `CloudOrderDoesNotChangeMetrics`, `SolverIsDeterministic`, `StateLimitIsRespectedAndReported`, `MetricRatiosStayInRange`, `ScoreRisesWithLengthTrapsMisdirectionAndNaivePlay`, `NaivePlayerSolvesObviousBoardWithoutExtraMoves`, `NaivePlayerMetricsStayInRange`, `NaivePlayerFailsMoreWhereTrapsExist`, `NaivePlayerAgreesWithDeviceTestData`, `SensiblePlayerBeatsRandomPlayer`, `DealMatchesFrameExactly`, `DealIsDeterministicAndSeedSensitive`, `RandomSequenceIsFixedAcrossPlatforms`, `FrameRejectsInvalidValues`, `SelectionPicksInWindowCandidatesWithDistinctBackups`, `SelectionPrefersScoreClosestToWindowCenter`, `SelectionFailsLoudlyWhenWindowIsUnreachable`, `SelectionFailsWhenBackupsAreMissing`, `SelectionSkipsCandidatesOverTheStateLimit`, `SelectedCandidateBecomesValidLevelDefinition`, `V3PatternRepeatsEveryFiveLevels`, `V3PartsCoverOneToHundred`.
- [ ] [Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs](../Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs) — 70 satır. Tanımlar: `LiquidMotionTests`.
  İnceleme/test girişleri: `TopEntryConservesVolumeAndNeverFillsBeforeArrival`, `SameElementGrowsOneFluidBodyAndCancellationRestoresTheExactView`, `LiquidSurfaceIsBoundedAndItsWarmedMeshAllocatesNoGarbage`.
- [ ] [Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs](../Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs) — 26 satır. Tanımlar: `MainMenuLifeTests`.
  İnceleme/test girişleri: `ZeroLivesOffersARefillAndRegenerationRestoresThePlayAction`.
- [ ] [Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs](../Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs) — 74 satır. Tanımlar: `MoonRolloutTests`.
  İnceleme/test girişleri: `MoonIsTheFifthElementFrom60AndNoExistingElementDisappears`, `Only60Through80ChangeAndDifficultyFramesAndTrapSlotsStayProtected`, `AllNewBoardsAreSolvableWithCompletedVesselsLockedAndLoadTheirFiveElements`, `PublishedCyclesKeepMediumBelowHardBelowVeryHard`.
- [ ] [Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs](../Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs) — 39 satır. Tanımlar: `MoveFlowTests`.
  İnceleme/test girişleri: `FlowCompletesWithTimeScaleZeroAndLeavesCommitToController`.
- [ ] [Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs](../Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs) — 51 satır. Tanımlar: `OpeningSplashTests`.
  İnceleme/test girişleri: `SplashWaitsForReadinessAndReleasesInputWithTimeScaleZero`, `ReducedMotionUsesStaticCloudsAndNoGlint`.
- [ ] [Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs](../Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs) — 91 satır. Tanımlar: `PointerMotionVisualTests`.
  İnceleme/test girişleri: `TransparentVesselReceivesTouchesAndElementTransfersRenderToCompletion`.
- [ ] [Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs](../Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs) — 398 satır. Tanımlar: `PuzzleFeelTests`.
  İnceleme/test girişleri: `GroupMoveAndSnapshotRestoreRemainConsistent`, `InvalidDestinationDoesNotMutateState`, `AuthoredTutorialLevelsHaveValidElementCounts`, `GeneratedLevelsAreDeterministicAndKnownSolvable`, `PureRulesApplyGroupedMoveAndDetectSolved`, `PureRulesRejectPartialGroupMoveThatPlayerCannotChoose`, `WindMovesOneElementFromModifiedCloud`, `SafeAreaAnchorsRespectCutoutInsets`, `StartupRepairsInvisibleRootCanvas`, `AdaptiveLayoutFitsPortraitBounds`, `WorldProgressionCrossesBoundaryAndRestoresInStages`, `CompletedLevelResumesAtNextLevelWithoutAdvancingActiveScreen`, `RainbowIsWildcardWithoutAmbiguousSolvedState`, `GeneratorIntroducesExpectedModifier`, `LivesRegenerateOfflineAndRespectCap`, `TutorialRetryIsFreeAndBoostersCannotUnderflow`, `ZeroLivesBlocksNewNormalAttemptButNotTutorialOrPaidCurrentAttempt`, `ModifierSnapshotAndExtraCloudPreserveDynamicCounters`, `SaveRoundTripsAndCorruptPrimaryFallsBackToBackup`, `ValidSchemaSaveStillNormalizesUnsafeFields`, `AnalyticsRejectsPersonalDataKeys`, `AdPolicyProtectsEarlyLevelsAndFrequencyCaps`, `DifficultyWaveHasFinaleAndEasyWorldReset`.
- [ ] [Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs](../Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs) — 60 satır. Tanımlar: `ReachabilityRegressionTests`.
  İnceleme/test girişleri: `ScreenshotYellowCycleIsALossDespiteLegalMoves`, `ExtraVesselRescuesTheScreenshotAndCannotReuseADeadCacheEntry`, `ExhaustedFrameBudgetNeverMeansLost`, `All100StartingBoardsHaveSolutionsUnderRuntimeRules`, `IncrementalSearchAgreesWithIndependentFullGraphAndPermutation`.
- [ ] [Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs](../Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs) — 109 satır. Tanımlar: `RenderingReuseTests`.
  İnceleme/test girişleri: `WarmedElementMeshesGenerateNoManagedGarbage`, `RepeatedMovesReuseGlassAndBoundedRegionsAndKeepUndoAccurate`, `WorldChangeInvalidatesAtmosphereEvenWhenMotionIsStatic`, `RepeatedFailedStateDoesNotRebuildItsText`.
- [ ] [Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs](../Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs) — 316 satır. Tanımlar: `V31GameplayTests`.
  İnceleme/test girişleri: `ShuffleRejectsCloudsThatCannotChange`, `ShuffleNeverTouchesProtectedClouds`, `ShuffleChangesOnlyTheChosenCloudAndKeepsItsContents`, `ShuffleAlwaysLeavesASolvableBoard`, `ShuffleReportsWhenNoSafeArrangementExists`, `ShuffleIsDeterministicPerSeedAndVariesAcrossSeeds`, `ContinueFindsNewestSolvableState`, `CoinRewardsFollowDifficultyAndContinueIsMeaningful`, `CoinWalletNeverGoesNegative`, `CoinsSurviveSaveAndAreNormalized`, `FailRecordRoundTripsThroughSave`, `FailRecordIsIgnoredWhenItDoesNotFit`, `OldSavesLoadWithoutFailRecord`, `FigmaHelpersHaveDistinctTouchTargetsAndNoRestart`, `FailPanelOffersLifeAdAndCoinsAndBlocksBoosters`, `LifeAdRemainsAvailableFromPauseMenu`, `ShuffleUnlocksAtSixteenByLevelNumber`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32InputTests.cs](../Assets/_Cloudora/Tests/Editor/V32InputTests.cs) — 55 satır. Tanımlar: `V32InputTests`.
  İnceleme/test girişleri: `AnalyticsLinesCarryFrameAndTime`, `InfoPopupInterceptsInputAndDismissesWithoutUsingHelper`, `BottomButtonClicksAreLoggedWithFrame`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32LadderTests.cs](../Assets/_Cloudora/Tests/Editor/V32LadderTests.cs) — 266 satır. Tanımlar: `V32LadderTests`.
  İnceleme/test girişleri: `TargetsContinueSmoothlyFromAdaptation`, `TargetsRiseWithinEachBandAndKeepCycleOrder`, `VeryHardTargetsGrowFasterThanOtherBands`, `SolutionsNeverReachAbsurdLengths`, `NoMovesAppearsSometimesAndAcrossBands`, `OnlyThePartFinaleMayLockThePlayer`, `VeryHardPrefersSingleEmptyButNotAlways`, `MisleadingFilterKeepsObviousBoardsOutOfHardBands`, `TransitionFollowsPatternAndCycleOrder`, `SameBandNeverGetsEasierAndStartIsSmooth`, `ScoresStayCloseToTargets`, `CapacityAndColorsNeverFallBack`, `PartReachesCapacityFiveAndSingleEmptyCloud`, `EachNoveltyComesAloneOnAMediumLevel`, `ScatteredLayoutArrivesOnceThenMixesWithClassic`, `SingleEmptyCloudBecomesMoreFrequent`, `BoardsAreDistinctFromOneToThirty`, `OutlierBoardsAreNeverChosen`, `TransitionIsDeterministicAndValid`, `LaterThemesUseTheThemeBuilder`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs) — 89 satır. Tanımlar: `V32LayoutTests`.
  İnceleme/test girişleri: `DefaultLayoutIsTheClassicPackedBoard`, `ScatteredLayoutKeepsPiecesAndFreeSpace`, `ScatteredLayoutIsDeterministicAndVaried`, `ScatteredBoardsAreSolvedWithTheGameRules`, `InvalidOpenCloudCountsAreRejected`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs) — 116 satır. Tanımlar: `V32NoMovesTests`.
  İnceleme/test girişleri: `Level24BoardCanFailOnTheFirstMove`, `ThreeColorsWithTwoEmptiesNeverRunOutOfMoves`, `NoMovesDepthMatchesIndependentSearch`, `NoMovesDoesNotChangeTheScore`, `ScoreAgreesWithEditorTestOnSingleEmptyBoards`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs) — 45 satır. Tanımlar: `V32ShuffleLoadProbe`.
  İnceleme/test girişleri: `MeasureShuffleOnLargeBoards`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs) — 92 satır. Tanımlar: `V32SolverLoadTests`.
  İnceleme/test girişleri: `QuickCheckAgreesWithTheFullSolver`, `QuickCheckReportsUnsolvableAndLimit`, `ShuffleWithExtraCloudsIsFastAndNeverFalselyRejected`, `FailRecoveryWithExtraCloudsIsFast`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs](../Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs) — 150 satır. Tanımlar: `V32Theme3Tests`.
  İnceleme/test girişleri: `TargetsFollowTheTemplateCurve`, `ColorsCapacityAndNovelties`, `LayoutBreathesWithTheNewThemeAndTheNewColor`, `TrapLevelsBecomeMoreFrequentAndBreatheWithTheNewColor`, `SomeHardAndVeryHardLevelsAreTraps`, `CycleOrderSameBandAndTargets`, `FinaleIsALongEnoughTrapAndOnlyItLocks`, `NoMovesIsOccasionalAcrossBands`, `BoardsAreUniqueValidAndDeterministic`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs](../Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs) — 194 satır. Tanımlar: `V32ThemeTests`.
  İnceleme/test girişleri: `TargetsFollowTheTemplateCurve`, `CurveContinuesFromTransitionWithoutJump`, `Theme2UsesFourColorsCapacityFiveAndTwoEmpties`, `NewColorAndLayoutStepsComeAloneOnMediumLevels`, `LayoutStartsSoftAndHardensGraduallyWithMixing`, `TrapAndPlanningLevelsMixAfterTheEmptyCloudBreathes`, `PatternCycleOrderAndSameBandNeverEasier`, `ScoresStayCloseToTargetsAndLengthsAreSane`, `NoMovesIsOccasionalAcrossBandsAndOnlyTheFinaleLocks`, `FinaleIsTheHardestTrapOfTheTheme`, `BoardsAreUniqueValidAndDeterministic`, `TransitionPlanIsUntouched`, `TestedPlanChangesOnlyAtLevel59`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs) — 64 satır. Tanımlar: `V32VisualTests`.
  İnceleme/test girişleri: `WeatherColorsMatchChosenPalette`, `WeatherColorsAreClearlyDistinct`, `WeatherUsesDistinctModernSymbolsWithoutInterceptingInput`.
- [ ] [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs) — 263 satır. Tanımlar: `V3LevelBuildTests`.
  İnceleme/test girişleri: `IntroUsesFrameAndPattern`, `AdaptationUsesFrameAndPattern`, `BoardsAreDistinctAcrossAllParts`, `HarderBandsScoreHigherWithinEachCycle`, `SameBandLevelsDoNotGetEasierWithinPart`, `BandWindowsDoNotOverlap`, `PartTransitionsAreCumulativeWithoutJumps`, `TutorialCuesAreDemonstratedByTheBoard`, `BuildIsDeterministic`, `BuiltLevelsAreValidLevelDefinitions`, `UndesignedPartsAreRejected`, `GeneratedDataMatchesBuilder`, `CatalogUsesV3BoardsAndKeepsTutorialFrame`, `LevelsNotYetRebuiltStayOnOldSystem`, `TransitionLevelsLoadFromV3Data`, `BoardTextRoundTrips`.

## Sahne, prefab, assembly ve platform bağları

- Başlangıç sahnesi [Assets/_Cloudora/Scenes/02_Gameplay.unity](../Assets/_Cloudora/Scenes/02_Gameplay.unity); prefab [Assets/_Cloudora/Prefabs/CloudContainer.prefab](../Assets/_Cloudora/Prefabs/CloudContainer.prefab). YAML içindeki her GUID için ilgili `.meta` dosyasını bul; Inspector bağlantıları ve programatik UI kurulumu birlikte okunur.
- Assembly tanımları runtime/Editor/test sınırını belirler; test derlemelerini release koduna taşıma. Aşağıdaki tüm `.asmdef` ve platform kaynaklarını incele:

- [Assets/Plugins/iOS/CloudoraAccessibility.mm](../Assets/Plugins/iOS/CloudoraAccessibility.mm)

## Paketler ve ayarlar

Önce Unity sürümü ve manifest/lock, sonra build sahnesi/Player/kalite/grafik/input ayarları. Bu teslimde hepsi referansla aynı; açılan Editor değişiklik yaptı diye otomatik kabul edilmez. İmza/Team/SDK güncellemesi ayrı görevdir.

- [.gitattributes](../.gitattributes)
- [.gitignore](../.gitignore)
- [Build-iOS.sh](../Build-iOS.sh)
- [Packages/manifest.json](../Packages/manifest.json)
- [Packages/packages-lock.json](../Packages/packages-lock.json)
- [ProjectSettings/AudioManager.asset](../ProjectSettings/AudioManager.asset)
- [ProjectSettings/ClusterInputManager.asset](../ProjectSettings/ClusterInputManager.asset)
- [ProjectSettings/DynamicsManager.asset](../ProjectSettings/DynamicsManager.asset)
- [ProjectSettings/EditorBuildSettings.asset](../ProjectSettings/EditorBuildSettings.asset)
- [ProjectSettings/EditorSettings.asset](../ProjectSettings/EditorSettings.asset)
- [ProjectSettings/GraphicsSettings.asset](../ProjectSettings/GraphicsSettings.asset)
- [ProjectSettings/InputManager.asset](../ProjectSettings/InputManager.asset)
- [ProjectSettings/MemorySettings.asset](../ProjectSettings/MemorySettings.asset)
- [ProjectSettings/MultiplayerManager.asset](../ProjectSettings/MultiplayerManager.asset)
- [ProjectSettings/NavMeshAreas.asset](../ProjectSettings/NavMeshAreas.asset)
- [ProjectSettings/NetworkManager.asset](../ProjectSettings/NetworkManager.asset)
- [ProjectSettings/PackageManagerSettings.asset](../ProjectSettings/PackageManagerSettings.asset)
- [ProjectSettings/Physics2DSettings.asset](../ProjectSettings/Physics2DSettings.asset)
- [ProjectSettings/PhysicsCoreProjectSettings2D.asset](../ProjectSettings/PhysicsCoreProjectSettings2D.asset)
- [ProjectSettings/PresetManager.asset](../ProjectSettings/PresetManager.asset)
- [ProjectSettings/ProjectAuditorSettings.asset](../ProjectSettings/ProjectAuditorSettings.asset)
- [ProjectSettings/ProjectSettings.asset](../ProjectSettings/ProjectSettings.asset)
- [ProjectSettings/ProjectVersion.txt](../ProjectSettings/ProjectVersion.txt)
- [ProjectSettings/QualitySettings.asset](../ProjectSettings/QualitySettings.asset)
- [ProjectSettings/SceneTemplateSettings.json](../ProjectSettings/SceneTemplateSettings.json)
- [ProjectSettings/ShaderGraphSettings.asset](../ProjectSettings/ShaderGraphSettings.asset)
- [ProjectSettings/TagManager.asset](../ProjectSettings/TagManager.asset)
- [ProjectSettings/TimeManager.asset](../ProjectSettings/TimeManager.asset)
- [ProjectSettings/URPProjectSettings.asset](../ProjectSettings/URPProjectSettings.asset)
- [ProjectSettings/UnityConnectSettings.asset](../ProjectSettings/UnityConnectSettings.asset)
- [ProjectSettings/VFXManager.asset](../ProjectSettings/VFXManager.asset)
- [ProjectSettings/VersionControlSettings.asset](../ProjectSettings/VersionControlSettings.asset)
- [ProjectSettings/XRSettings.asset](../ProjectSettings/XRSettings.asset)

## Belgelerin tamamı — aynı yetkide değiller

Güncel talimat → GUNCEL_DURUM → kod/test → son konu belgesi → tarihsel kayıt. Aşağıdaki belgelerin **içeriğinin tamamını** oku; önerileri uygulanmış özellik veya eski kararı geri dönüş talimatı sayma. HTML raporlarını tarayıcıda da incele.

- [CLAUDE.md](../CLAUDE.md) — Güncel giriş/rota/karar ayrımı.
- [CONTRIBUTORS.md](../CONTRIBUTORS.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [ClaudoraRelease.md](../ClaudoraRelease.md) — Kronolojik tarihsel kayıt; son kararlarla birlikte.
- [Docs/DAL_KARSILASTIRMASI_2026-11-24.md](../Docs/DAL_KARSILASTIRMASI_2026-11-24.md) — Güncel giriş/rota/karar ayrımı.
- [Docs/DOSYA_OKUMA_HARITASI.md](../Docs/DOSYA_OKUMA_HARITASI.md) — Güncel giriş/rota/karar ayrımı.
- [Docs/DesignSources/Nunito-OFL.txt](../Docs/DesignSources/Nunito-OFL.txt) — Font lisansı; korunmalı.
- [Docs/EKIP_ONBOARDING.md](../Docs/EKIP_ONBOARDING.md) — Güncel giriş/rota/karar ayrımı.
- [Docs/GELISTIRME_ONERILERI.md](../Docs/GELISTIRME_ONERILERI.md) — Öneri arşivi; onaylı iş listesi değil.
- [Docs/GUNCEL_DURUM.md](../Docs/GUNCEL_DURUM.md) — Güncel giriş/rota/karar ayrımı.
- [Docs/IOS_ARCHITECTURE.md](../Docs/IOS_ARCHITECTURE.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_BUILD.md](../Docs/IOS_BUILD.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_COMPLETION_EFFECTS.md](../Docs/IOS_COMPLETION_EFFECTS.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_DESIGN_IMPLEMENTATION.md](../Docs/IOS_DESIGN_IMPLEMENTATION.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_DEVICE_QA.md](../Docs/IOS_DEVICE_QA.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_ELEMENT_MOTION.md](../Docs/IOS_ELEMENT_MOTION.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_ITEM_TRANSFER_RESTORE.md](../Docs/IOS_ITEM_TRANSFER_RESTORE.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_LIQUID_MOTION.md](../Docs/IOS_LIQUID_MOTION.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_MOON_FROM_60.md](../Docs/IOS_MOON_FROM_60.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_OPENING_SPLASH.md](../Docs/IOS_OPENING_SPLASH.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_PERFORMANCE.md](../Docs/IOS_PERFORMANCE.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_QUALITY_CAMPAIGN.md](../Docs/IOS_QUALITY_CAMPAIGN.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_STATUS.md](../Docs/IOS_STATUS.md) — Kronolojik tarihsel kayıt; son kararlarla birlikte.
- [Docs/IOS_UX_REVIEW.md](../Docs/IOS_UX_REVIEW.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/IOS_WEATHER_SYMBOLS.md](../Docs/IOS_WEATHER_SYMBOLS.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/PRIVACY_RELEASE_REVIEW.md](../Docs/PRIVACY_RELEASE_REVIEW.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/PROJECT_PROGRESS.md](../Docs/PROJECT_PROGRESS.md) — Kronolojik tarihsel kayıt; son kararlarla birlikte.
- [Docs/PROJE_HAFIZASI.md](../Docs/PROJE_HAFIZASI.md) — Kronolojik tarihsel kayıt; son kararlarla birlikte.
- [Docs/RELEASE_CHECKLIST.md](../Docs/RELEASE_CHECKLIST.md) — Konu belgesi/kanıt; GUNCEL_DURUM ile birlikte.
- [Docs/SONRAKI_ADIMLAR.md](../Docs/SONRAKI_ADIMLAR.md) — Güncel giriş/rota/karar ayrımı.
- [Docs/V3_BOLUM_HARITASI.html](../Docs/V3_BOLUM_HARITASI.html) — Tarihsel bölüm/kalibrasyon belgesi.
- [Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md](../Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md) — Tarihsel bölüm/kalibrasyon belgesi.
- [Docs/V3_CERCEVE_ENVANTERI.md](../Docs/V3_CERCEVE_ENVANTERI.md) — Tarihsel bölüm/kalibrasyon belgesi.
- [Docs/V3_SABLON.md](../Docs/V3_SABLON.md) — Tarihsel bölüm/kalibrasyon belgesi.
- [README.md](../README.md) — Güncel giriş/rota/karar ayrımı.
- [StoreAssets/README.md](../StoreAssets/README.md) — Mağaza taslağı; yayın onayı değil.
- [StoreAssets/STORE_COPY.md](../StoreAssets/STORE_COPY.md) — Mağaza taslağı; yayın onayı değil.
- [crVersion.md](../crVersion.md) — Kronolojik tarihsel kayıt; son kararlarla birlikte.

## Görseller, fontlar ve tasarım kaynakları

`Assets/Resources/Design` oyun ekranları, ikon/font ve dünya görsellerini; `Assets/Resources/Splash` açılış kaynaklarını; `Docs/DesignSources` tasarım referanslarını içerir. Sahne/UI kodunun Resources yükleme adları dosya adlarına bağlıdır. PNG/font gibi LFS nesneleri gerçek byte olarak indirilmeli. `.meta` GUID ve platform importer ayarları silinmemeli. Nunito lisansı ve mağaza kaynaklarının yayın durumu korunmalıdır.

Her varlık için: nerede yükleniyor, piksel boyutu/netlik, compression/maxSize/filter/Android-iOS override, runtime tekrar kullanım, lisans. Tasarım kaynakları üretim kaynağıyla aynı amaçta değildir; otomatik yeniden export etme.

## Korunacak alanlar ve değişiklik sınırı

| Alan | Neyi koru? | Değişiklikte beraber incelenecekler |
|---|---|---|
| Çekirdek/solver | Hamle, tamamlanan hazne kilidi ve çözümsüzlük kanıtı | PuzzleRules, PuzzleSolver, PuzzleController, Reachability/MoveFlow testleri |
| Bölüm tabloları/üretim | 100 kayıt, deterministik seed, palet/zorluk, Ay60 | Generated veri + plan + Editor pipeline + Moon/Campaign testleri |
| Ekonomi/kayıt | Coin/can/hak, fail history, şema2, yedekler | Progression, Services, recovery/abandon/economy testleri |
| Görsel/animasyon | Sabit cam, üstten uçuş, birleşik sıvı ve7 bitiriş | View, Animator, MotionLibrary, LiquidFill, Flourish ve görsel test |
| Unity varlık/ayar | GUID, LFS nesnesi, font lisansı, sürüm/scene bağı | .meta, prefab/scene, Packages/ProjectSettings |
| Yayın/platform | İmza, servis ve mağaza hedefi | Build aracı, release config, privacy/release checklist |

Bunlar mutlak olarak hiç değiştirilemez dosyalar değil, **görev kapsamı olmadan dokunulmayacak sözleşmelerdir**. Kullanıcının sonraki açık görevi ilgili alanı değiştirebilir; kapsam dışı davranışı beraber değiştirme. Generated veri elle yamanmaz; yayın pipeline ve doğrulama kullanılır. Gizli anahtar/sertifika/cihaz kaydı Git'e konmaz.

## Tam dosya envanteri

Aşağıdaki liste teslimin sürüm kontrollü tüm dosyalarını kapsar; `.meta` ve ikili dosyalar da dahil. `Library/Logs/Builds` ve yerel cihaz kayıtları hariçtir. Kaynak/varlık byte doğrulaması için manifesto, dosya varlığı ve okuma kapsamı için bu liste kullanılır.

<details>
<summary>Assets — 445 dosya</summary>

- [Assets/Plugins.meta](../Assets/Plugins.meta)
- [Assets/Plugins/iOS.meta](../Assets/Plugins/iOS.meta)
- [Assets/Plugins/iOS/CloudoraAccessibility.mm](../Assets/Plugins/iOS/CloudoraAccessibility.mm)
- [Assets/Plugins/iOS/CloudoraAccessibility.mm.meta](../Assets/Plugins/iOS/CloudoraAccessibility.mm.meta)
- [Assets/Resources.meta](../Assets/Resources.meta)
- [Assets/Resources/CloudoraReleaseConfig.json](../Assets/Resources/CloudoraReleaseConfig.json)
- [Assets/Resources/CloudoraReleaseConfig.json.meta](../Assets/Resources/CloudoraReleaseConfig.json.meta)
- [Assets/Scenes.meta](../Assets/Scenes.meta)
- [Assets/Scenes/SampleScene.unity](../Assets/Scenes/SampleScene.unity)
- [Assets/Scenes/SampleScene.unity.meta](../Assets/Scenes/SampleScene.unity.meta)
- [Assets/Settings.meta](../Assets/Settings.meta)
- [Assets/Settings/Build Profiles.meta](../Assets/Settings/Build%20Profiles.meta)
- [Assets/Settings/DefaultVolumeProfile.asset](../Assets/Settings/DefaultVolumeProfile.asset)
- [Assets/Settings/DefaultVolumeProfile.asset.meta](../Assets/Settings/DefaultVolumeProfile.asset.meta)
- [Assets/Settings/InputSystem_Actions.inputactions](../Assets/Settings/InputSystem_Actions.inputactions)
- [Assets/Settings/InputSystem_Actions.inputactions.meta](../Assets/Settings/InputSystem_Actions.inputactions.meta)
- [Assets/Settings/Lit2DSceneTemplate.scenetemplate](../Assets/Settings/Lit2DSceneTemplate.scenetemplate)
- [Assets/Settings/Lit2DSceneTemplate.scenetemplate.meta](../Assets/Settings/Lit2DSceneTemplate.scenetemplate.meta)
- [Assets/Settings/Renderer2D.asset](../Assets/Settings/Renderer2D.asset)
- [Assets/Settings/Renderer2D.asset.meta](../Assets/Settings/Renderer2D.asset.meta)
- [Assets/Settings/Scenes.meta](../Assets/Settings/Scenes.meta)
- [Assets/Settings/Scenes/URP2DSceneTemplate.unity](../Assets/Settings/Scenes/URP2DSceneTemplate.unity)
- [Assets/Settings/Scenes/URP2DSceneTemplate.unity.meta](../Assets/Settings/Scenes/URP2DSceneTemplate.unity.meta)
- [Assets/Settings/UniversalRP.asset](../Assets/Settings/UniversalRP.asset)
- [Assets/Settings/UniversalRP.asset.meta](../Assets/Settings/UniversalRP.asset.meta)
- [Assets/Settings/UniversalRenderPipelineGlobalSettings.asset](../Assets/Settings/UniversalRenderPipelineGlobalSettings.asset)
- [Assets/Settings/UniversalRenderPipelineGlobalSettings.asset.meta](../Assets/Settings/UniversalRenderPipelineGlobalSettings.asset.meta)
- [Assets/_Cloudora.meta](../Assets/_Cloudora.meta)
- [Assets/_Cloudora/Art.meta](../Assets/_Cloudora/Art.meta)
- [Assets/_Cloudora/Art/cloudora-adaptive-background-512.png](../Assets/_Cloudora/Art/cloudora-adaptive-background-512.png)
- [Assets/_Cloudora/Art/cloudora-adaptive-background-512.png.meta](../Assets/_Cloudora/Art/cloudora-adaptive-background-512.png.meta)
- [Assets/_Cloudora/Art/cloudora-adaptive-foreground-512.png](../Assets/_Cloudora/Art/cloudora-adaptive-foreground-512.png)
- [Assets/_Cloudora/Art/cloudora-adaptive-foreground-512.png.meta](../Assets/_Cloudora/Art/cloudora-adaptive-foreground-512.png.meta)
- [Assets/_Cloudora/Art/cloudora-icon-512.png](../Assets/_Cloudora/Art/cloudora-icon-512.png)
- [Assets/_Cloudora/Art/cloudora-icon-512.png.meta](../Assets/_Cloudora/Art/cloudora-icon-512.png.meta)
- [Assets/_Cloudora/Audio.meta](../Assets/_Cloudora/Audio.meta)
- [Assets/_Cloudora/Editor.meta](../Assets/_Cloudora/Editor.meta)
- [Assets/_Cloudora/Editor/CampaignMoonRollout.cs](../Assets/_Cloudora/Editor/CampaignMoonRollout.cs)
- [Assets/_Cloudora/Editor/CampaignMoonRollout.cs.meta](../Assets/_Cloudora/Editor/CampaignMoonRollout.cs.meta)
- [Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs](../Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs)
- [Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs.meta](../Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs.meta)
- [Assets/_Cloudora/Editor/ContentValidationTool.cs](../Assets/_Cloudora/Editor/ContentValidationTool.cs)
- [Assets/_Cloudora/Editor/ContentValidationTool.cs.meta](../Assets/_Cloudora/Editor/ContentValidationTool.cs.meta)
- [Assets/_Cloudora/Editor/DesignPreview.cs](../Assets/_Cloudora/Editor/DesignPreview.cs)
- [Assets/_Cloudora/Editor/DesignPreview.cs.meta](../Assets/_Cloudora/Editor/DesignPreview.cs.meta)
- [Assets/_Cloudora/Editor/DesignQuality.cs](../Assets/_Cloudora/Editor/DesignQuality.cs)
- [Assets/_Cloudora/Editor/DesignQuality.cs.meta](../Assets/_Cloudora/Editor/DesignQuality.cs.meta)
- [Assets/_Cloudora/Editor/LevelCandidateFactory.cs](../Assets/_Cloudora/Editor/LevelCandidateFactory.cs)
- [Assets/_Cloudora/Editor/LevelCandidateFactory.cs.meta](../Assets/_Cloudora/Editor/LevelCandidateFactory.cs.meta)
- [Assets/_Cloudora/Editor/LevelCandidateSelector.cs](../Assets/_Cloudora/Editor/LevelCandidateSelector.cs)
- [Assets/_Cloudora/Editor/LevelCandidateSelector.cs.meta](../Assets/_Cloudora/Editor/LevelCandidateSelector.cs.meta)
- [Assets/_Cloudora/Editor/OpeningSplashPreview.cs](../Assets/_Cloudora/Editor/OpeningSplashPreview.cs)
- [Assets/_Cloudora/Editor/OpeningSplashPreview.cs.meta](../Assets/_Cloudora/Editor/OpeningSplashPreview.cs.meta)
- [Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs](../Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs)
- [Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs.meta](../Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs.meta)
- [Assets/_Cloudora/Editor/V3LadderBuilder.cs](../Assets/_Cloudora/Editor/V3LadderBuilder.cs)
- [Assets/_Cloudora/Editor/V3LadderBuilder.cs.meta](../Assets/_Cloudora/Editor/V3LadderBuilder.cs.meta)
- [Assets/_Cloudora/Editor/V3LevelBuilder.cs](../Assets/_Cloudora/Editor/V3LevelBuilder.cs)
- [Assets/_Cloudora/Editor/V3LevelBuilder.cs.meta](../Assets/_Cloudora/Editor/V3LevelBuilder.cs.meta)
- [Assets/_Cloudora/Editor/V3LevelTool.cs](../Assets/_Cloudora/Editor/V3LevelTool.cs)
- [Assets/_Cloudora/Editor/V3LevelTool.cs.meta](../Assets/_Cloudora/Editor/V3LevelTool.cs.meta)
- [Assets/_Cloudora/Editor/V3ThemeBuilder.cs](../Assets/_Cloudora/Editor/V3ThemeBuilder.cs)
- [Assets/_Cloudora/Editor/V3ThemeBuilder.cs.meta](../Assets/_Cloudora/Editor/V3ThemeBuilder.cs.meta)
- [Assets/_Cloudora/Editor/WeatherSymbolPreview.cs](../Assets/_Cloudora/Editor/WeatherSymbolPreview.cs)
- [Assets/_Cloudora/Editor/WeatherSymbolPreview.cs.meta](../Assets/_Cloudora/Editor/WeatherSymbolPreview.cs.meta)
- [Assets/_Cloudora/Materials.meta](../Assets/_Cloudora/Materials.meta)
- [Assets/_Cloudora/Prefabs.meta](../Assets/_Cloudora/Prefabs.meta)
- [Assets/_Cloudora/Prefabs/CloudContainer.prefab](../Assets/_Cloudora/Prefabs/CloudContainer.prefab)
- [Assets/_Cloudora/Prefabs/CloudContainer.prefab.meta](../Assets/_Cloudora/Prefabs/CloudContainer.prefab.meta)
- [Assets/_Cloudora/Resources.meta](../Assets/_Cloudora/Resources.meta)
- [Assets/_Cloudora/Resources/Design.meta](../Assets/_Cloudora/Resources/Design.meta)
- [Assets/_Cloudora/Resources/Design/006df.png](../Assets/_Cloudora/Resources/Design/006df.png)
- [Assets/_Cloudora/Resources/Design/006df.png.meta](../Assets/_Cloudora/Resources/Design/006df.png.meta)
- [Assets/_Cloudora/Resources/Design/05725.png](../Assets/_Cloudora/Resources/Design/05725.png)
- [Assets/_Cloudora/Resources/Design/05725.png.meta](../Assets/_Cloudora/Resources/Design/05725.png.meta)
- [Assets/_Cloudora/Resources/Design/05e75.png](../Assets/_Cloudora/Resources/Design/05e75.png)
- [Assets/_Cloudora/Resources/Design/05e75.png.meta](../Assets/_Cloudora/Resources/Design/05e75.png.meta)
- [Assets/_Cloudora/Resources/Design/0e0cb.png](../Assets/_Cloudora/Resources/Design/0e0cb.png)
- [Assets/_Cloudora/Resources/Design/0e0cb.png.meta](../Assets/_Cloudora/Resources/Design/0e0cb.png.meta)
- [Assets/_Cloudora/Resources/Design/12cdb.png](../Assets/_Cloudora/Resources/Design/12cdb.png)
- [Assets/_Cloudora/Resources/Design/12cdb.png.meta](../Assets/_Cloudora/Resources/Design/12cdb.png.meta)
- [Assets/_Cloudora/Resources/Design/1603c.png](../Assets/_Cloudora/Resources/Design/1603c.png)
- [Assets/_Cloudora/Resources/Design/1603c.png.meta](../Assets/_Cloudora/Resources/Design/1603c.png.meta)
- [Assets/_Cloudora/Resources/Design/16c49.png](../Assets/_Cloudora/Resources/Design/16c49.png)
- [Assets/_Cloudora/Resources/Design/16c49.png.meta](../Assets/_Cloudora/Resources/Design/16c49.png.meta)
- [Assets/_Cloudora/Resources/Design/1a3ce.png](../Assets/_Cloudora/Resources/Design/1a3ce.png)
- [Assets/_Cloudora/Resources/Design/1a3ce.png.meta](../Assets/_Cloudora/Resources/Design/1a3ce.png.meta)
- [Assets/_Cloudora/Resources/Design/22aa2.png](../Assets/_Cloudora/Resources/Design/22aa2.png)
- [Assets/_Cloudora/Resources/Design/22aa2.png.meta](../Assets/_Cloudora/Resources/Design/22aa2.png.meta)
- [Assets/_Cloudora/Resources/Design/232d2.png](../Assets/_Cloudora/Resources/Design/232d2.png)
- [Assets/_Cloudora/Resources/Design/232d2.png.meta](../Assets/_Cloudora/Resources/Design/232d2.png.meta)
- [Assets/_Cloudora/Resources/Design/249a4.png](../Assets/_Cloudora/Resources/Design/249a4.png)
- [Assets/_Cloudora/Resources/Design/249a4.png.meta](../Assets/_Cloudora/Resources/Design/249a4.png.meta)
- [Assets/_Cloudora/Resources/Design/2bf57.png](../Assets/_Cloudora/Resources/Design/2bf57.png)
- [Assets/_Cloudora/Resources/Design/2bf57.png.meta](../Assets/_Cloudora/Resources/Design/2bf57.png.meta)
- [Assets/_Cloudora/Resources/Design/2ce2b.png](../Assets/_Cloudora/Resources/Design/2ce2b.png)
- [Assets/_Cloudora/Resources/Design/2ce2b.png.meta](../Assets/_Cloudora/Resources/Design/2ce2b.png.meta)
- [Assets/_Cloudora/Resources/Design/2ceb3.png](../Assets/_Cloudora/Resources/Design/2ceb3.png)
- [Assets/_Cloudora/Resources/Design/2ceb3.png.meta](../Assets/_Cloudora/Resources/Design/2ceb3.png.meta)
- [Assets/_Cloudora/Resources/Design/329e9.png](../Assets/_Cloudora/Resources/Design/329e9.png)
- [Assets/_Cloudora/Resources/Design/329e9.png.meta](../Assets/_Cloudora/Resources/Design/329e9.png.meta)
- [Assets/_Cloudora/Resources/Design/34a1f.png](../Assets/_Cloudora/Resources/Design/34a1f.png)
- [Assets/_Cloudora/Resources/Design/34a1f.png.meta](../Assets/_Cloudora/Resources/Design/34a1f.png.meta)
- [Assets/_Cloudora/Resources/Design/3ae59.png](../Assets/_Cloudora/Resources/Design/3ae59.png)
- [Assets/_Cloudora/Resources/Design/3ae59.png.meta](../Assets/_Cloudora/Resources/Design/3ae59.png.meta)
- [Assets/_Cloudora/Resources/Design/3c5bb.png](../Assets/_Cloudora/Resources/Design/3c5bb.png)
- [Assets/_Cloudora/Resources/Design/3c5bb.png.meta](../Assets/_Cloudora/Resources/Design/3c5bb.png.meta)
- [Assets/_Cloudora/Resources/Design/40038.png](../Assets/_Cloudora/Resources/Design/40038.png)
- [Assets/_Cloudora/Resources/Design/40038.png.meta](../Assets/_Cloudora/Resources/Design/40038.png.meta)
- [Assets/_Cloudora/Resources/Design/480b9.png](../Assets/_Cloudora/Resources/Design/480b9.png)
- [Assets/_Cloudora/Resources/Design/480b9.png.meta](../Assets/_Cloudora/Resources/Design/480b9.png.meta)
- [Assets/_Cloudora/Resources/Design/4b2fc.png](../Assets/_Cloudora/Resources/Design/4b2fc.png)
- [Assets/_Cloudora/Resources/Design/4b2fc.png.meta](../Assets/_Cloudora/Resources/Design/4b2fc.png.meta)
- [Assets/_Cloudora/Resources/Design/4c96c.png](../Assets/_Cloudora/Resources/Design/4c96c.png)
- [Assets/_Cloudora/Resources/Design/4c96c.png.meta](../Assets/_Cloudora/Resources/Design/4c96c.png.meta)
- [Assets/_Cloudora/Resources/Design/4e035.png](../Assets/_Cloudora/Resources/Design/4e035.png)
- [Assets/_Cloudora/Resources/Design/4e035.png.meta](../Assets/_Cloudora/Resources/Design/4e035.png.meta)
- [Assets/_Cloudora/Resources/Design/50b49.png](../Assets/_Cloudora/Resources/Design/50b49.png)
- [Assets/_Cloudora/Resources/Design/50b49.png.meta](../Assets/_Cloudora/Resources/Design/50b49.png.meta)
- [Assets/_Cloudora/Resources/Design/56480.png](../Assets/_Cloudora/Resources/Design/56480.png)
- [Assets/_Cloudora/Resources/Design/56480.png.meta](../Assets/_Cloudora/Resources/Design/56480.png.meta)
- [Assets/_Cloudora/Resources/Design/57814.png](../Assets/_Cloudora/Resources/Design/57814.png)
- [Assets/_Cloudora/Resources/Design/57814.png.meta](../Assets/_Cloudora/Resources/Design/57814.png.meta)
- [Assets/_Cloudora/Resources/Design/5941f.png](../Assets/_Cloudora/Resources/Design/5941f.png)
- [Assets/_Cloudora/Resources/Design/5941f.png.meta](../Assets/_Cloudora/Resources/Design/5941f.png.meta)
- [Assets/_Cloudora/Resources/Design/64132.png](../Assets/_Cloudora/Resources/Design/64132.png)
- [Assets/_Cloudora/Resources/Design/64132.png.meta](../Assets/_Cloudora/Resources/Design/64132.png.meta)
- [Assets/_Cloudora/Resources/Design/654dc.png](../Assets/_Cloudora/Resources/Design/654dc.png)
- [Assets/_Cloudora/Resources/Design/654dc.png.meta](../Assets/_Cloudora/Resources/Design/654dc.png.meta)
- [Assets/_Cloudora/Resources/Design/6565a.png](../Assets/_Cloudora/Resources/Design/6565a.png)
- [Assets/_Cloudora/Resources/Design/6565a.png.meta](../Assets/_Cloudora/Resources/Design/6565a.png.meta)
- [Assets/_Cloudora/Resources/Design/6621d.png](../Assets/_Cloudora/Resources/Design/6621d.png)
- [Assets/_Cloudora/Resources/Design/6621d.png.meta](../Assets/_Cloudora/Resources/Design/6621d.png.meta)
- [Assets/_Cloudora/Resources/Design/6ca6c.png](../Assets/_Cloudora/Resources/Design/6ca6c.png)
- [Assets/_Cloudora/Resources/Design/6ca6c.png.meta](../Assets/_Cloudora/Resources/Design/6ca6c.png.meta)
- [Assets/_Cloudora/Resources/Design/72720.png](../Assets/_Cloudora/Resources/Design/72720.png)
- [Assets/_Cloudora/Resources/Design/72720.png.meta](../Assets/_Cloudora/Resources/Design/72720.png.meta)
- [Assets/_Cloudora/Resources/Design/7465e.png](../Assets/_Cloudora/Resources/Design/7465e.png)
- [Assets/_Cloudora/Resources/Design/7465e.png.meta](../Assets/_Cloudora/Resources/Design/7465e.png.meta)
- [Assets/_Cloudora/Resources/Design/84576.png](../Assets/_Cloudora/Resources/Design/84576.png)
- [Assets/_Cloudora/Resources/Design/84576.png.meta](../Assets/_Cloudora/Resources/Design/84576.png.meta)
- [Assets/_Cloudora/Resources/Design/8758e.png](../Assets/_Cloudora/Resources/Design/8758e.png)
- [Assets/_Cloudora/Resources/Design/8758e.png.meta](../Assets/_Cloudora/Resources/Design/8758e.png.meta)
- [Assets/_Cloudora/Resources/Design/88c3d.png](../Assets/_Cloudora/Resources/Design/88c3d.png)
- [Assets/_Cloudora/Resources/Design/88c3d.png.meta](../Assets/_Cloudora/Resources/Design/88c3d.png.meta)
- [Assets/_Cloudora/Resources/Design/8c480.png](../Assets/_Cloudora/Resources/Design/8c480.png)
- [Assets/_Cloudora/Resources/Design/8c480.png.meta](../Assets/_Cloudora/Resources/Design/8c480.png.meta)
- [Assets/_Cloudora/Resources/Design/8da82.png](../Assets/_Cloudora/Resources/Design/8da82.png)
- [Assets/_Cloudora/Resources/Design/8da82.png.meta](../Assets/_Cloudora/Resources/Design/8da82.png.meta)
- [Assets/_Cloudora/Resources/Design/90fcc.png](../Assets/_Cloudora/Resources/Design/90fcc.png)
- [Assets/_Cloudora/Resources/Design/90fcc.png.meta](../Assets/_Cloudora/Resources/Design/90fcc.png.meta)
- [Assets/_Cloudora/Resources/Design/91521.png](../Assets/_Cloudora/Resources/Design/91521.png)
- [Assets/_Cloudora/Resources/Design/91521.png.meta](../Assets/_Cloudora/Resources/Design/91521.png.meta)
- [Assets/_Cloudora/Resources/Design/91f4f.png](../Assets/_Cloudora/Resources/Design/91f4f.png)
- [Assets/_Cloudora/Resources/Design/91f4f.png.meta](../Assets/_Cloudora/Resources/Design/91f4f.png.meta)
- [Assets/_Cloudora/Resources/Design/92bf0.png](../Assets/_Cloudora/Resources/Design/92bf0.png)
- [Assets/_Cloudora/Resources/Design/92bf0.png.meta](../Assets/_Cloudora/Resources/Design/92bf0.png.meta)
- [Assets/_Cloudora/Resources/Design/982c2.png](../Assets/_Cloudora/Resources/Design/982c2.png)
- [Assets/_Cloudora/Resources/Design/982c2.png.meta](../Assets/_Cloudora/Resources/Design/982c2.png.meta)
- [Assets/_Cloudora/Resources/Design/9864a.png](../Assets/_Cloudora/Resources/Design/9864a.png)
- [Assets/_Cloudora/Resources/Design/9864a.png.meta](../Assets/_Cloudora/Resources/Design/9864a.png.meta)
- [Assets/_Cloudora/Resources/Design/9c136.png](../Assets/_Cloudora/Resources/Design/9c136.png)
- [Assets/_Cloudora/Resources/Design/9c136.png.meta](../Assets/_Cloudora/Resources/Design/9c136.png.meta)
- [Assets/_Cloudora/Resources/Design/9e0d3.png](../Assets/_Cloudora/Resources/Design/9e0d3.png)
- [Assets/_Cloudora/Resources/Design/9e0d3.png.meta](../Assets/_Cloudora/Resources/Design/9e0d3.png.meta)
- [Assets/_Cloudora/Resources/Design/Nunito-ExtraBold.ttf](../Assets/_Cloudora/Resources/Design/Nunito-ExtraBold.ttf)
- [Assets/_Cloudora/Resources/Design/Nunito-ExtraBold.ttf.meta](../Assets/_Cloudora/Resources/Design/Nunito-ExtraBold.ttf.meta)
- [Assets/_Cloudora/Resources/Design/a5f3a.png](../Assets/_Cloudora/Resources/Design/a5f3a.png)
- [Assets/_Cloudora/Resources/Design/a5f3a.png.meta](../Assets/_Cloudora/Resources/Design/a5f3a.png.meta)
- [Assets/_Cloudora/Resources/Design/a7fd1.png](../Assets/_Cloudora/Resources/Design/a7fd1.png)
- [Assets/_Cloudora/Resources/Design/a7fd1.png.meta](../Assets/_Cloudora/Resources/Design/a7fd1.png.meta)
- [Assets/_Cloudora/Resources/Design/ab7f3.png](../Assets/_Cloudora/Resources/Design/ab7f3.png)
- [Assets/_Cloudora/Resources/Design/ab7f3.png.meta](../Assets/_Cloudora/Resources/Design/ab7f3.png.meta)
- [Assets/_Cloudora/Resources/Design/acd7e.png](../Assets/_Cloudora/Resources/Design/acd7e.png)
- [Assets/_Cloudora/Resources/Design/acd7e.png.meta](../Assets/_Cloudora/Resources/Design/acd7e.png.meta)
- [Assets/_Cloudora/Resources/Design/ad73d.png](../Assets/_Cloudora/Resources/Design/ad73d.png)
- [Assets/_Cloudora/Resources/Design/ad73d.png.meta](../Assets/_Cloudora/Resources/Design/ad73d.png.meta)
- [Assets/_Cloudora/Resources/Design/b3e5d.png](../Assets/_Cloudora/Resources/Design/b3e5d.png)
- [Assets/_Cloudora/Resources/Design/b3e5d.png.meta](../Assets/_Cloudora/Resources/Design/b3e5d.png.meta)
- [Assets/_Cloudora/Resources/Design/b44ef.png](../Assets/_Cloudora/Resources/Design/b44ef.png)
- [Assets/_Cloudora/Resources/Design/b44ef.png.meta](../Assets/_Cloudora/Resources/Design/b44ef.png.meta)
- [Assets/_Cloudora/Resources/Design/b6f1c.png](../Assets/_Cloudora/Resources/Design/b6f1c.png)
- [Assets/_Cloudora/Resources/Design/b6f1c.png.meta](../Assets/_Cloudora/Resources/Design/b6f1c.png.meta)
- [Assets/_Cloudora/Resources/Design/ba2f9.png](../Assets/_Cloudora/Resources/Design/ba2f9.png)
- [Assets/_Cloudora/Resources/Design/ba2f9.png.meta](../Assets/_Cloudora/Resources/Design/ba2f9.png.meta)
- [Assets/_Cloudora/Resources/Design/bef7c.png](../Assets/_Cloudora/Resources/Design/bef7c.png)
- [Assets/_Cloudora/Resources/Design/bef7c.png.meta](../Assets/_Cloudora/Resources/Design/bef7c.png.meta)
- [Assets/_Cloudora/Resources/Design/c0f0e.png](../Assets/_Cloudora/Resources/Design/c0f0e.png)
- [Assets/_Cloudora/Resources/Design/c0f0e.png.meta](../Assets/_Cloudora/Resources/Design/c0f0e.png.meta)
- [Assets/_Cloudora/Resources/Design/c190e.png](../Assets/_Cloudora/Resources/Design/c190e.png)
- [Assets/_Cloudora/Resources/Design/c190e.png.meta](../Assets/_Cloudora/Resources/Design/c190e.png.meta)
- [Assets/_Cloudora/Resources/Design/c51f5.png](../Assets/_Cloudora/Resources/Design/c51f5.png)
- [Assets/_Cloudora/Resources/Design/c51f5.png.meta](../Assets/_Cloudora/Resources/Design/c51f5.png.meta)
- [Assets/_Cloudora/Resources/Design/c6a3b.png](../Assets/_Cloudora/Resources/Design/c6a3b.png)
- [Assets/_Cloudora/Resources/Design/c6a3b.png.meta](../Assets/_Cloudora/Resources/Design/c6a3b.png.meta)
- [Assets/_Cloudora/Resources/Design/d0de5.png](../Assets/_Cloudora/Resources/Design/d0de5.png)
- [Assets/_Cloudora/Resources/Design/d0de5.png.meta](../Assets/_Cloudora/Resources/Design/d0de5.png.meta)
- [Assets/_Cloudora/Resources/Design/d4098.png](../Assets/_Cloudora/Resources/Design/d4098.png)
- [Assets/_Cloudora/Resources/Design/d4098.png.meta](../Assets/_Cloudora/Resources/Design/d4098.png.meta)
- [Assets/_Cloudora/Resources/Design/d58f1.png](../Assets/_Cloudora/Resources/Design/d58f1.png)
- [Assets/_Cloudora/Resources/Design/d58f1.png.meta](../Assets/_Cloudora/Resources/Design/d58f1.png.meta)
- [Assets/_Cloudora/Resources/Design/d703a.png](../Assets/_Cloudora/Resources/Design/d703a.png)
- [Assets/_Cloudora/Resources/Design/d703a.png.meta](../Assets/_Cloudora/Resources/Design/d703a.png.meta)
- [Assets/_Cloudora/Resources/Design/da4f7.png](../Assets/_Cloudora/Resources/Design/da4f7.png)
- [Assets/_Cloudora/Resources/Design/da4f7.png.meta](../Assets/_Cloudora/Resources/Design/da4f7.png.meta)
- [Assets/_Cloudora/Resources/Design/dbecc.png](../Assets/_Cloudora/Resources/Design/dbecc.png)
- [Assets/_Cloudora/Resources/Design/dbecc.png.meta](../Assets/_Cloudora/Resources/Design/dbecc.png.meta)
- [Assets/_Cloudora/Resources/Design/de635.png](../Assets/_Cloudora/Resources/Design/de635.png)
- [Assets/_Cloudora/Resources/Design/de635.png.meta](../Assets/_Cloudora/Resources/Design/de635.png.meta)
- [Assets/_Cloudora/Resources/Design/dff46.png](../Assets/_Cloudora/Resources/Design/dff46.png)
- [Assets/_Cloudora/Resources/Design/dff46.png.meta](../Assets/_Cloudora/Resources/Design/dff46.png.meta)
- [Assets/_Cloudora/Resources/Design/e8e2c.png](../Assets/_Cloudora/Resources/Design/e8e2c.png)
- [Assets/_Cloudora/Resources/Design/e8e2c.png.meta](../Assets/_Cloudora/Resources/Design/e8e2c.png.meta)
- [Assets/_Cloudora/Resources/Design/ee931.png](../Assets/_Cloudora/Resources/Design/ee931.png)
- [Assets/_Cloudora/Resources/Design/ee931.png.meta](../Assets/_Cloudora/Resources/Design/ee931.png.meta)
- [Assets/_Cloudora/Resources/Design/f0c41.png](../Assets/_Cloudora/Resources/Design/f0c41.png)
- [Assets/_Cloudora/Resources/Design/f0c41.png.meta](../Assets/_Cloudora/Resources/Design/f0c41.png.meta)
- [Assets/_Cloudora/Resources/Design/f3a6a.png](../Assets/_Cloudora/Resources/Design/f3a6a.png)
- [Assets/_Cloudora/Resources/Design/f3a6a.png.meta](../Assets/_Cloudora/Resources/Design/f3a6a.png.meta)
- [Assets/_Cloudora/Resources/Design/f745e.png](../Assets/_Cloudora/Resources/Design/f745e.png)
- [Assets/_Cloudora/Resources/Design/f745e.png.meta](../Assets/_Cloudora/Resources/Design/f745e.png.meta)
- [Assets/_Cloudora/Resources/Design/f787f.png](../Assets/_Cloudora/Resources/Design/f787f.png)
- [Assets/_Cloudora/Resources/Design/f787f.png.meta](../Assets/_Cloudora/Resources/Design/f787f.png.meta)
- [Assets/_Cloudora/Resources/Design/fa776.png](../Assets/_Cloudora/Resources/Design/fa776.png)
- [Assets/_Cloudora/Resources/Design/fa776.png.meta](../Assets/_Cloudora/Resources/Design/fa776.png.meta)
- [Assets/_Cloudora/Resources/Design/fac64.png](../Assets/_Cloudora/Resources/Design/fac64.png)
- [Assets/_Cloudora/Resources/Design/fac64.png.meta](../Assets/_Cloudora/Resources/Design/fac64.png.meta)
- [Assets/_Cloudora/Resources/Design/ffaa1.png](../Assets/_Cloudora/Resources/Design/ffaa1.png)
- [Assets/_Cloudora/Resources/Design/ffaa1.png.meta](../Assets/_Cloudora/Resources/Design/ffaa1.png.meta)
- [Assets/_Cloudora/Resources/Splash.meta](../Assets/_Cloudora/Resources/Splash.meta)
- [Assets/_Cloudora/Resources/Splash/Cloud_Back_Left.png](../Assets/_Cloudora/Resources/Splash/Cloud_Back_Left.png)
- [Assets/_Cloudora/Resources/Splash/Cloud_Back_Left.png.meta](../Assets/_Cloudora/Resources/Splash/Cloud_Back_Left.png.meta)
- [Assets/_Cloudora/Resources/Splash/Cloud_Back_Right.png](../Assets/_Cloudora/Resources/Splash/Cloud_Back_Right.png)
- [Assets/_Cloudora/Resources/Splash/Cloud_Back_Right.png.meta](../Assets/_Cloudora/Resources/Splash/Cloud_Back_Right.png.meta)
- [Assets/_Cloudora/Resources/Splash/Cloud_Front_Left.png](../Assets/_Cloudora/Resources/Splash/Cloud_Front_Left.png)
- [Assets/_Cloudora/Resources/Splash/Cloud_Front_Left.png.meta](../Assets/_Cloudora/Resources/Splash/Cloud_Front_Left.png.meta)
- [Assets/_Cloudora/Resources/Splash/Cloud_Front_Right.png](../Assets/_Cloudora/Resources/Splash/Cloud_Front_Right.png)
- [Assets/_Cloudora/Resources/Splash/Cloud_Front_Right.png.meta](../Assets/_Cloudora/Resources/Splash/Cloud_Front_Right.png.meta)
- [Assets/_Cloudora/Resources/Splash/Glint.png](../Assets/_Cloudora/Resources/Splash/Glint.png)
- [Assets/_Cloudora/Resources/Splash/Glint.png.meta](../Assets/_Cloudora/Resources/Splash/Glint.png.meta)
- [Assets/_Cloudora/Resources/Splash/Logo_CLOUDORA.png](../Assets/_Cloudora/Resources/Splash/Logo_CLOUDORA.png)
- [Assets/_Cloudora/Resources/Splash/Logo_CLOUDORA.png.meta](../Assets/_Cloudora/Resources/Splash/Logo_CLOUDORA.png.meta)
- [Assets/_Cloudora/Resources/Splash/Sun_Disc.png](../Assets/_Cloudora/Resources/Splash/Sun_Disc.png)
- [Assets/_Cloudora/Resources/Splash/Sun_Disc.png.meta](../Assets/_Cloudora/Resources/Splash/Sun_Disc.png.meta)
- [Assets/_Cloudora/Resources/Splash/Sun_Glow.png](../Assets/_Cloudora/Resources/Splash/Sun_Glow.png)
- [Assets/_Cloudora/Resources/Splash/Sun_Glow.png.meta](../Assets/_Cloudora/Resources/Splash/Sun_Glow.png.meta)
- [Assets/_Cloudora/Scenes.meta](../Assets/_Cloudora/Scenes.meta)
- [Assets/_Cloudora/Scenes/02_Gameplay.unity](../Assets/_Cloudora/Scenes/02_Gameplay.unity)
- [Assets/_Cloudora/Scenes/02_Gameplay.unity.meta](../Assets/_Cloudora/Scenes/02_Gameplay.unity.meta)
- [Assets/_Cloudora/ScriptableObjects.meta](../Assets/_Cloudora/ScriptableObjects.meta)
- [Assets/_Cloudora/Scripts.meta](../Assets/_Cloudora/Scripts.meta)
- [Assets/_Cloudora/Scripts/Core.meta](../Assets/_Cloudora/Scripts/Core.meta)
- [Assets/_Cloudora/Scripts/Core/CloudState.cs](../Assets/_Cloudora/Scripts/Core/CloudState.cs)
- [Assets/_Cloudora/Scripts/Core/CloudState.cs.meta](../Assets/_Cloudora/Scripts/Core/CloudState.cs.meta)
- [Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs](../Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs)
- [Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs.meta](../Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs.meta)
- [Assets/_Cloudora/Scripts/Core/Move.cs](../Assets/_Cloudora/Scripts/Core/Move.cs)
- [Assets/_Cloudora/Scripts/Core/Move.cs.meta](../Assets/_Cloudora/Scripts/Core/Move.cs.meta)
- [Assets/_Cloudora/Scripts/Core/PuzzleRules.cs](../Assets/_Cloudora/Scripts/Core/PuzzleRules.cs)
- [Assets/_Cloudora/Scripts/Core/PuzzleRules.cs.meta](../Assets/_Cloudora/Scripts/Core/PuzzleRules.cs.meta)
- [Assets/_Cloudora/Scripts/Core/PuzzleState.cs](../Assets/_Cloudora/Scripts/Core/PuzzleState.cs)
- [Assets/_Cloudora/Scripts/Core/PuzzleState.cs.meta](../Assets/_Cloudora/Scripts/Core/PuzzleState.cs.meta)
- [Assets/_Cloudora/Scripts/Level.meta](../Assets/_Cloudora/Scripts/Level.meta)
- [Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs](../Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs)
- [Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs.meta](../Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs.meta)
- [Assets/_Cloudora/Scripts/Level/BalanceProfile.cs](../Assets/_Cloudora/Scripts/Level/BalanceProfile.cs)
- [Assets/_Cloudora/Scripts/Level/BalanceProfile.cs.meta](../Assets/_Cloudora/Scripts/Level/BalanceProfile.cs.meta)
- [Assets/_Cloudora/Scripts/Level/BoardFrame.cs](../Assets/_Cloudora/Scripts/Level/BoardFrame.cs)
- [Assets/_Cloudora/Scripts/Level/BoardFrame.cs.meta](../Assets/_Cloudora/Scripts/Level/BoardFrame.cs.meta)
- [Assets/_Cloudora/Scripts/Level/CloudShuffler.cs](../Assets/_Cloudora/Scripts/Level/CloudShuffler.cs)
- [Assets/_Cloudora/Scripts/Level/CloudShuffler.cs.meta](../Assets/_Cloudora/Scripts/Level/CloudShuffler.cs.meta)
- [Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs](../Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs)
- [Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs.meta](../Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs.meta)
- [Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs](../Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs)
- [Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs.meta](../Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs.meta)
- [Assets/_Cloudora/Scripts/Level/LevelDefinition.cs](../Assets/_Cloudora/Scripts/Level/LevelDefinition.cs)
- [Assets/_Cloudora/Scripts/Level/LevelDefinition.cs.meta](../Assets/_Cloudora/Scripts/Level/LevelDefinition.cs.meta)
- [Assets/_Cloudora/Scripts/Level/LevelGenerator.cs](../Assets/_Cloudora/Scripts/Level/LevelGenerator.cs)
- [Assets/_Cloudora/Scripts/Level/LevelGenerator.cs.meta](../Assets/_Cloudora/Scripts/Level/LevelGenerator.cs.meta)
- [Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs](../Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs)
- [Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs.meta](../Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs.meta)
- [Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs](../Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs)
- [Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs.meta](../Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs.meta)
- [Assets/_Cloudora/Scripts/Level/V3BoardText.cs](../Assets/_Cloudora/Scripts/Level/V3BoardText.cs)
- [Assets/_Cloudora/Scripts/Level/V3BoardText.cs.meta](../Assets/_Cloudora/Scripts/Level/V3BoardText.cs.meta)
- [Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs)
- [Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs.meta](../Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs.meta)
- [Assets/_Cloudora/Scripts/Level/V3LevelData.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.cs)
- [Assets/_Cloudora/Scripts/Level/V3LevelData.cs.meta](../Assets/_Cloudora/Scripts/Level/V3LevelData.cs.meta)
- [Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs](../Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs)
- [Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs.meta](../Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs.meta)
- [Assets/_Cloudora/Scripts/Modifiers.meta](../Assets/_Cloudora/Scripts/Modifiers.meta)
- [Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs](../Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs)
- [Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs.meta](../Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs.meta)
- [Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs](../Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs)
- [Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs.meta](../Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs.meta)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs.meta](../Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs.meta)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs.meta](../Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs.meta)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs)
- [Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs.meta](../Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs.meta)
- [Assets/_Cloudora/Scripts/Progression.meta](../Assets/_Cloudora/Scripts/Progression.meta)
- [Assets/_Cloudora/Scripts/Progression/BoosterManager.cs](../Assets/_Cloudora/Scripts/Progression/BoosterManager.cs)
- [Assets/_Cloudora/Scripts/Progression/BoosterManager.cs.meta](../Assets/_Cloudora/Scripts/Progression/BoosterManager.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs](../Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs)
- [Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs.meta](../Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/LifeManager.cs](../Assets/_Cloudora/Scripts/Progression/LifeManager.cs)
- [Assets/_Cloudora/Scripts/Progression/LifeManager.cs.meta](../Assets/_Cloudora/Scripts/Progression/LifeManager.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs](../Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs)
- [Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs.meta](../Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/RestorationManager.cs](../Assets/_Cloudora/Scripts/Progression/RestorationManager.cs)
- [Assets/_Cloudora/Scripts/Progression/RestorationManager.cs.meta](../Assets/_Cloudora/Scripts/Progression/RestorationManager.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs](../Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs)
- [Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs.meta](../Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs.meta)
- [Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs](../Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs)
- [Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs.meta](../Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs.meta)
- [Assets/_Cloudora/Scripts/Puzzle.meta](../Assets/_Cloudora/Scripts/Puzzle.meta)
- [Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs](../Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs)
- [Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs.meta](../Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs.meta)
- [Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs](../Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs)
- [Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs.meta](../Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs.meta)
- [Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs](../Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs)
- [Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs.meta](../Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs.meta)
- [Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs)
- [Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs.meta](../Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs.meta)
- [Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs)
- [Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs.meta](../Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs.meta)
- [Assets/_Cloudora/Scripts/Services.meta](../Assets/_Cloudora/Scripts/Services.meta)
- [Assets/_Cloudora/Scripts/Services/AdMobAdService.cs](../Assets/_Cloudora/Scripts/Services/AdMobAdService.cs)
- [Assets/_Cloudora/Scripts/Services/AdMobAdService.cs.meta](../Assets/_Cloudora/Scripts/Services/AdMobAdService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs](../Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs)
- [Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs.meta](../Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs.meta)
- [Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs](../Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs)
- [Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs.meta](../Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs.meta)
- [Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs)
- [Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs.meta](../Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/FailRecord.cs](../Assets/_Cloudora/Scripts/Services/FailRecord.cs)
- [Assets/_Cloudora/Scripts/Services/FailRecord.cs.meta](../Assets/_Cloudora/Scripts/Services/FailRecord.cs.meta)
- [Assets/_Cloudora/Scripts/Services/FakeAdService.cs](../Assets/_Cloudora/Scripts/Services/FakeAdService.cs)
- [Assets/_Cloudora/Scripts/Services/FakeAdService.cs.meta](../Assets/_Cloudora/Scripts/Services/FakeAdService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs)
- [Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs.meta](../Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs](../Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs)
- [Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs.meta](../Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/IAdService.cs](../Assets/_Cloudora/Scripts/Services/IAdService.cs)
- [Assets/_Cloudora/Scripts/Services/IAdService.cs.meta](../Assets/_Cloudora/Scripts/Services/IAdService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs)
- [Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs.meta](../Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/ISaveService.cs](../Assets/_Cloudora/Scripts/Services/ISaveService.cs)
- [Assets/_Cloudora/Scripts/Services/ISaveService.cs.meta](../Assets/_Cloudora/Scripts/Services/ISaveService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs](../Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs)
- [Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs.meta](../Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs.meta)
- [Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs](../Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs)
- [Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs.meta](../Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs.meta)
- [Assets/_Cloudora/Scripts/Services/SaveData.cs](../Assets/_Cloudora/Scripts/Services/SaveData.cs)
- [Assets/_Cloudora/Scripts/Services/SaveData.cs.meta](../Assets/_Cloudora/Scripts/Services/SaveData.cs.meta)
- [Assets/_Cloudora/Scripts/UI.meta](../Assets/_Cloudora/Scripts/UI.meta)
- [Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs](../Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs)
- [Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs.meta](../Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs.meta)
- [Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs](../Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs)
- [Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs.meta](../Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs.meta)
- [Assets/_Cloudora/Scripts/UI/DesignUI.cs](../Assets/_Cloudora/Scripts/UI/DesignUI.cs)
- [Assets/_Cloudora/Scripts/UI/DesignUI.cs.meta](../Assets/_Cloudora/Scripts/UI/DesignUI.cs.meta)
- [Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs](../Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs)
- [Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs.meta](../Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs.meta)
- [Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs](../Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs)
- [Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs.meta](../Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs.meta)
- [Assets/_Cloudora/Scripts/UI/MainMenuController.cs](../Assets/_Cloudora/Scripts/UI/MainMenuController.cs)
- [Assets/_Cloudora/Scripts/UI/MainMenuController.cs.meta](../Assets/_Cloudora/Scripts/UI/MainMenuController.cs.meta)
- [Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs](../Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs)
- [Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs.meta](../Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs.meta)
- [Assets/_Cloudora/Scripts/UI/OpeningSplash.cs](../Assets/_Cloudora/Scripts/UI/OpeningSplash.cs)
- [Assets/_Cloudora/Scripts/UI/OpeningSplash.cs.meta](../Assets/_Cloudora/Scripts/UI/OpeningSplash.cs.meta)
- [Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs](../Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs)
- [Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs.meta](../Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs.meta)
- [Assets/_Cloudora/Scripts/UI/VectorShapes.cs](../Assets/_Cloudora/Scripts/UI/VectorShapes.cs)
- [Assets/_Cloudora/Scripts/UI/VectorShapes.cs.meta](../Assets/_Cloudora/Scripts/UI/VectorShapes.cs.meta)
- [Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs](../Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs)
- [Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs.meta](../Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs.meta)
- [Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs](../Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs)
- [Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs.meta](../Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs.meta)
- [Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs](../Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs)
- [Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs.meta](../Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs.meta)
- [Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs](../Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs)
- [Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs.meta](../Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs.meta)
- [Assets/_Cloudora/Scripts/UI/WorldScreenController.cs](../Assets/_Cloudora/Scripts/UI/WorldScreenController.cs)
- [Assets/_Cloudora/Scripts/UI/WorldScreenController.cs.meta](../Assets/_Cloudora/Scripts/UI/WorldScreenController.cs.meta)
- [Assets/_Cloudora/Tests.meta](../Assets/_Cloudora/Tests.meta)
- [Assets/_Cloudora/Tests/Editor.meta](../Assets/_Cloudora/Tests/Editor.meta)
- [Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs](../Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs)
- [Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs.meta](../Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs](../Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs)
- [Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs.meta](../Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs)
- [Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs.meta](../Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs)
- [Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs.meta](../Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs](../Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs)
- [Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs.meta](../Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs](../Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs)
- [Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs.meta](../Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/LevelToolTests.cs](../Assets/_Cloudora/Tests/Editor/LevelToolTests.cs)
- [Assets/_Cloudora/Tests/Editor/LevelToolTests.cs.meta](../Assets/_Cloudora/Tests/Editor/LevelToolTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs](../Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs)
- [Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs.meta](../Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs](../Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs)
- [Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs.meta](../Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs](../Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs)
- [Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs.meta](../Assets/_Cloudora/Tests/Editor/MoonRolloutTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs](../Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs)
- [Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs.meta](../Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs](../Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs)
- [Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs.meta](../Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs](../Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs)
- [Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs.meta](../Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs](../Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs)
- [Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs.meta](../Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs](../Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs)
- [Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs.meta](../Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs](../Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs)
- [Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs.meta](../Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs](../Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs)
- [Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32InputTests.cs](../Assets/_Cloudora/Tests/Editor/V32InputTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32InputTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32InputTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32LadderTests.cs](../Assets/_Cloudora/Tests/Editor/V32LadderTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32LadderTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32LadderTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs)
- [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs.meta](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs](../Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs)
- [Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32Theme3Tests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs](../Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32ThemeTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs)
- [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs.meta)
- [Assets/_Cloudora/UI.meta](../Assets/_Cloudora/UI.meta)

</details>

<details>
<summary>Docs — 104 dosya</summary>

- [Docs/DAL_KARSILASTIRMASI_2026-11-24.md](../Docs/DAL_KARSILASTIRMASI_2026-11-24.md)
- [Docs/DAL_KARSILASTIRMASI_VERI.json](../Docs/DAL_KARSILASTIRMASI_VERI.json)
- [Docs/DOSYA_OKUMA_HARITASI.md](../Docs/DOSYA_OKUMA_HARITASI.md)
- [Docs/DesignSources/006df.svg](../Docs/DesignSources/006df.svg)
- [Docs/DesignSources/05725.svg](../Docs/DesignSources/05725.svg)
- [Docs/DesignSources/05e75.svg](../Docs/DesignSources/05e75.svg)
- [Docs/DesignSources/0e0cb.svg](../Docs/DesignSources/0e0cb.svg)
- [Docs/DesignSources/1603c.svg](../Docs/DesignSources/1603c.svg)
- [Docs/DesignSources/16c49.svg](../Docs/DesignSources/16c49.svg)
- [Docs/DesignSources/1a3ce.svg](../Docs/DesignSources/1a3ce.svg)
- [Docs/DesignSources/22aa2.svg](../Docs/DesignSources/22aa2.svg)
- [Docs/DesignSources/249a4.svg](../Docs/DesignSources/249a4.svg)
- [Docs/DesignSources/2bf57.svg](../Docs/DesignSources/2bf57.svg)
- [Docs/DesignSources/2ceb3.svg](../Docs/DesignSources/2ceb3.svg)
- [Docs/DesignSources/329e9.svg](../Docs/DesignSources/329e9.svg)
- [Docs/DesignSources/34a1f.svg](../Docs/DesignSources/34a1f.svg)
- [Docs/DesignSources/3ae59.svg](../Docs/DesignSources/3ae59.svg)
- [Docs/DesignSources/3c5bb.svg](../Docs/DesignSources/3c5bb.svg)
- [Docs/DesignSources/40038.svg](../Docs/DesignSources/40038.svg)
- [Docs/DesignSources/480b9.svg](../Docs/DesignSources/480b9.svg)
- [Docs/DesignSources/4b2fc.svg](../Docs/DesignSources/4b2fc.svg)
- [Docs/DesignSources/4c96c.svg](../Docs/DesignSources/4c96c.svg)
- [Docs/DesignSources/4e035.svg](../Docs/DesignSources/4e035.svg)
- [Docs/DesignSources/50b49.svg](../Docs/DesignSources/50b49.svg)
- [Docs/DesignSources/56480.svg](../Docs/DesignSources/56480.svg)
- [Docs/DesignSources/57814.svg](../Docs/DesignSources/57814.svg)
- [Docs/DesignSources/64132.svg](../Docs/DesignSources/64132.svg)
- [Docs/DesignSources/654dc.svg](../Docs/DesignSources/654dc.svg)
- [Docs/DesignSources/6565a.svg](../Docs/DesignSources/6565a.svg)
- [Docs/DesignSources/6621d.svg](../Docs/DesignSources/6621d.svg)
- [Docs/DesignSources/6ca6c.svg](../Docs/DesignSources/6ca6c.svg)
- [Docs/DesignSources/72720.svg](../Docs/DesignSources/72720.svg)
- [Docs/DesignSources/7465e.svg](../Docs/DesignSources/7465e.svg)
- [Docs/DesignSources/8758e.svg](../Docs/DesignSources/8758e.svg)
- [Docs/DesignSources/88c3d.svg](../Docs/DesignSources/88c3d.svg)
- [Docs/DesignSources/8c480.svg](../Docs/DesignSources/8c480.svg)
- [Docs/DesignSources/8da82.svg](../Docs/DesignSources/8da82.svg)
- [Docs/DesignSources/90fcc.svg](../Docs/DesignSources/90fcc.svg)
- [Docs/DesignSources/91521.svg](../Docs/DesignSources/91521.svg)
- [Docs/DesignSources/91f4f.svg](../Docs/DesignSources/91f4f.svg)
- [Docs/DesignSources/92bf0.svg](../Docs/DesignSources/92bf0.svg)
- [Docs/DesignSources/982c2.svg](../Docs/DesignSources/982c2.svg)
- [Docs/DesignSources/9864a.svg](../Docs/DesignSources/9864a.svg)
- [Docs/DesignSources/9c136.svg](../Docs/DesignSources/9c136.svg)
- [Docs/DesignSources/9e0d3.svg](../Docs/DesignSources/9e0d3.svg)
- [Docs/DesignSources/Nunito-OFL.txt](../Docs/DesignSources/Nunito-OFL.txt)
- [Docs/DesignSources/a5f3a.svg](../Docs/DesignSources/a5f3a.svg)
- [Docs/DesignSources/a7fd1.svg](../Docs/DesignSources/a7fd1.svg)
- [Docs/DesignSources/ab7f3.svg](../Docs/DesignSources/ab7f3.svg)
- [Docs/DesignSources/acd7e.svg](../Docs/DesignSources/acd7e.svg)
- [Docs/DesignSources/ad73d.svg](../Docs/DesignSources/ad73d.svg)
- [Docs/DesignSources/b3e5d.svg](../Docs/DesignSources/b3e5d.svg)
- [Docs/DesignSources/b44ef.svg](../Docs/DesignSources/b44ef.svg)
- [Docs/DesignSources/b6f1c.svg](../Docs/DesignSources/b6f1c.svg)
- [Docs/DesignSources/ba2f9.svg](../Docs/DesignSources/ba2f9.svg)
- [Docs/DesignSources/bef7c.svg](../Docs/DesignSources/bef7c.svg)
- [Docs/DesignSources/c0f0e.svg](../Docs/DesignSources/c0f0e.svg)
- [Docs/DesignSources/c190e.svg](../Docs/DesignSources/c190e.svg)
- [Docs/DesignSources/c51f5.svg](../Docs/DesignSources/c51f5.svg)
- [Docs/DesignSources/c6a3b.svg](../Docs/DesignSources/c6a3b.svg)
- [Docs/DesignSources/d0de5.svg](../Docs/DesignSources/d0de5.svg)
- [Docs/DesignSources/d4098.svg](../Docs/DesignSources/d4098.svg)
- [Docs/DesignSources/d58f1.svg](../Docs/DesignSources/d58f1.svg)
- [Docs/DesignSources/d703a.svg](../Docs/DesignSources/d703a.svg)
- [Docs/DesignSources/da4f7.svg](../Docs/DesignSources/da4f7.svg)
- [Docs/DesignSources/dbecc.svg](../Docs/DesignSources/dbecc.svg)
- [Docs/DesignSources/de635.svg](../Docs/DesignSources/de635.svg)
- [Docs/DesignSources/dff46.svg](../Docs/DesignSources/dff46.svg)
- [Docs/DesignSources/e8e2c.svg](../Docs/DesignSources/e8e2c.svg)
- [Docs/DesignSources/f0c41.svg](../Docs/DesignSources/f0c41.svg)
- [Docs/DesignSources/f3a6a.svg](../Docs/DesignSources/f3a6a.svg)
- [Docs/DesignSources/f745e.svg](../Docs/DesignSources/f745e.svg)
- [Docs/DesignSources/f787f.svg](../Docs/DesignSources/f787f.svg)
- [Docs/DesignSources/fa776.svg](../Docs/DesignSources/fa776.svg)
- [Docs/DesignSources/fac64.svg](../Docs/DesignSources/fac64.svg)
- [Docs/DesignSources/ffaa1.svg](../Docs/DesignSources/ffaa1.svg)
- [Docs/EKIP_ONBOARDING.md](../Docs/EKIP_ONBOARDING.md)
- [Docs/GELISTIRME_ONERILERI.md](../Docs/GELISTIRME_ONERILERI.md)
- [Docs/GUNCEL_DURUM.md](../Docs/GUNCEL_DURUM.md)
- [Docs/IOS_ARCHITECTURE.md](../Docs/IOS_ARCHITECTURE.md)
- [Docs/IOS_BUILD.md](../Docs/IOS_BUILD.md)
- [Docs/IOS_COMPLETION_EFFECTS.md](../Docs/IOS_COMPLETION_EFFECTS.md)
- [Docs/IOS_DESIGN_IMPLEMENTATION.md](../Docs/IOS_DESIGN_IMPLEMENTATION.md)
- [Docs/IOS_DEVICE_QA.md](../Docs/IOS_DEVICE_QA.md)
- [Docs/IOS_ELEMENT_MOTION.md](../Docs/IOS_ELEMENT_MOTION.md)
- [Docs/IOS_ITEM_TRANSFER_RESTORE.md](../Docs/IOS_ITEM_TRANSFER_RESTORE.md)
- [Docs/IOS_LIQUID_MOTION.md](../Docs/IOS_LIQUID_MOTION.md)
- [Docs/IOS_MOON_FROM_60.md](../Docs/IOS_MOON_FROM_60.md)
- [Docs/IOS_OPENING_SPLASH.md](../Docs/IOS_OPENING_SPLASH.md)
- [Docs/IOS_PERFORMANCE.md](../Docs/IOS_PERFORMANCE.md)
- [Docs/IOS_QUALITY_CAMPAIGN.md](../Docs/IOS_QUALITY_CAMPAIGN.md)
- [Docs/IOS_STATUS.md](../Docs/IOS_STATUS.md)
- [Docs/IOS_UX_REVIEW.md](../Docs/IOS_UX_REVIEW.md)
- [Docs/IOS_WEATHER_SYMBOLS.md](../Docs/IOS_WEATHER_SYMBOLS.md)
- [Docs/PRIVACY_RELEASE_REVIEW.md](../Docs/PRIVACY_RELEASE_REVIEW.md)
- [Docs/PROJECT_PROGRESS.md](../Docs/PROJECT_PROGRESS.md)
- [Docs/PROJE_HAFIZASI.md](../Docs/PROJE_HAFIZASI.md)
- [Docs/REFERANS_TABAN.json](../Docs/REFERANS_TABAN.json)
- [Docs/RELEASE_CHECKLIST.md](../Docs/RELEASE_CHECKLIST.md)
- [Docs/SONRAKI_ADIMLAR.md](../Docs/SONRAKI_ADIMLAR.md)
- [Docs/V3_BOLUM_HARITASI.html](../Docs/V3_BOLUM_HARITASI.html)
- [Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md](../Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md)
- [Docs/V3_CERCEVE_ENVANTERI.md](../Docs/V3_CERCEVE_ENVANTERI.md)
- [Docs/V3_SABLON.md](../Docs/V3_SABLON.md)

</details>

<details>
<summary>Packages — 2 dosya</summary>

- [Packages/manifest.json](../Packages/manifest.json)
- [Packages/packages-lock.json](../Packages/packages-lock.json)

</details>

<details>
<summary>ProjectSettings — 28 dosya</summary>

- [ProjectSettings/AudioManager.asset](../ProjectSettings/AudioManager.asset)
- [ProjectSettings/ClusterInputManager.asset](../ProjectSettings/ClusterInputManager.asset)
- [ProjectSettings/DynamicsManager.asset](../ProjectSettings/DynamicsManager.asset)
- [ProjectSettings/EditorBuildSettings.asset](../ProjectSettings/EditorBuildSettings.asset)
- [ProjectSettings/EditorSettings.asset](../ProjectSettings/EditorSettings.asset)
- [ProjectSettings/GraphicsSettings.asset](../ProjectSettings/GraphicsSettings.asset)
- [ProjectSettings/InputManager.asset](../ProjectSettings/InputManager.asset)
- [ProjectSettings/MemorySettings.asset](../ProjectSettings/MemorySettings.asset)
- [ProjectSettings/MultiplayerManager.asset](../ProjectSettings/MultiplayerManager.asset)
- [ProjectSettings/NavMeshAreas.asset](../ProjectSettings/NavMeshAreas.asset)
- [ProjectSettings/NetworkManager.asset](../ProjectSettings/NetworkManager.asset)
- [ProjectSettings/PackageManagerSettings.asset](../ProjectSettings/PackageManagerSettings.asset)
- [ProjectSettings/Physics2DSettings.asset](../ProjectSettings/Physics2DSettings.asset)
- [ProjectSettings/PhysicsCoreProjectSettings2D.asset](../ProjectSettings/PhysicsCoreProjectSettings2D.asset)
- [ProjectSettings/PresetManager.asset](../ProjectSettings/PresetManager.asset)
- [ProjectSettings/ProjectAuditorSettings.asset](../ProjectSettings/ProjectAuditorSettings.asset)
- [ProjectSettings/ProjectSettings.asset](../ProjectSettings/ProjectSettings.asset)
- [ProjectSettings/ProjectVersion.txt](../ProjectSettings/ProjectVersion.txt)
- [ProjectSettings/QualitySettings.asset](../ProjectSettings/QualitySettings.asset)
- [ProjectSettings/SceneTemplateSettings.json](../ProjectSettings/SceneTemplateSettings.json)
- [ProjectSettings/ShaderGraphSettings.asset](../ProjectSettings/ShaderGraphSettings.asset)
- [ProjectSettings/TagManager.asset](../ProjectSettings/TagManager.asset)
- [ProjectSettings/TimeManager.asset](../ProjectSettings/TimeManager.asset)
- [ProjectSettings/URPProjectSettings.asset](../ProjectSettings/URPProjectSettings.asset)
- [ProjectSettings/UnityConnectSettings.asset](../ProjectSettings/UnityConnectSettings.asset)
- [ProjectSettings/VFXManager.asset](../ProjectSettings/VFXManager.asset)
- [ProjectSettings/VersionControlSettings.asset](../ProjectSettings/VersionControlSettings.asset)
- [ProjectSettings/XRSettings.asset](../ProjectSettings/XRSettings.asset)

</details>

<details>
<summary>StoreAssets — 6 dosya</summary>

- [StoreAssets/Artwork/cloudora-feature-1024x500.png](../StoreAssets/Artwork/cloudora-feature-1024x500.png)
- [StoreAssets/Artwork/cloudora-icon-512.png](../StoreAssets/Artwork/cloudora-icon-512.png)
- [StoreAssets/README.md](../StoreAssets/README.md)
- [StoreAssets/STORE_COPY.md](../StoreAssets/STORE_COPY.md)
- [StoreAssets/Source/cloudora-feature-master.png](../StoreAssets/Source/cloudora-feature-master.png)
- [StoreAssets/Source/cloudora-icon-master.png](../StoreAssets/Source/cloudora-icon-master.png)

</details>

<details>
<summary>Tools — 1 dosya</summary>

- [Tools/verify_reference_snapshot.py](../Tools/verify_reference_snapshot.py)

</details>

<details>
<summary>root — 9 dosya</summary>

- [.gitattributes](../.gitattributes)
- [.gitignore](../.gitignore)
- [.vsconfig](../.vsconfig)
- [Build-iOS.sh](../Build-iOS.sh)
- [CLAUDE.md](../CLAUDE.md)
- [CONTRIBUTORS.md](../CONTRIBUTORS.md)
- [ClaudoraRelease.md](../ClaudoraRelease.md)
- [README.md](../README.md)
- [crVersion.md](../crVersion.md)

</details>
