# Eksiksiz dosya okuma haritası — Bahadır v4.1 main

Kaynak `5a838a104cdce93e342c353c930288e08099ceab`. [EKIP_ONBOARDING](EKIP_ONBOARDING.md) okuma sırasını, [GUNCEL_DURUM](GUNCEL_DURUM.md) güncel sözleşmeyi belirler. Bu liste dosyanın okunduğunun/testin çalıştığının kanıtı değildir; gerçek içeriğin tamamını oku.

**101 C# dosyası**: 65 runtime, 13 Editor, 23 test/probe. Manifesto **482** oyun/varlık/ayar/derleme/mağaza dosyasını korur. Her dosya için okundu/kalan, sorumluluk, giriş/çıkış, yan etki, bağımlılık, risk ve ilgili test notu tut. Uzun dosya/truncate edilmiş çıktı atlanmaz.

## Scripts/Core

- [ ] [Assets/_Cloudora/Scripts/Core/CloudState.cs](../Assets/_Cloudora/Scripts/Core/CloudState.cs) — 60 satır; `CloudState`.
- [ ] [Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs](../Assets/_Cloudora/Scripts/Core/MobileRuntimeBootstrap.cs) — 44 satır; `MobileRuntimeBootstrap`.
- [ ] [Assets/_Cloudora/Scripts/Core/Move.cs](../Assets/_Cloudora/Scripts/Core/Move.cs) — 22 satır; `Move`.
- [ ] [Assets/_Cloudora/Scripts/Core/PuzzleRules.cs](../Assets/_Cloudora/Scripts/Core/PuzzleRules.cs) — 50 satır; `PuzzleRules`.
- [ ] [Assets/_Cloudora/Scripts/Core/PuzzleState.cs](../Assets/_Cloudora/Scripts/Core/PuzzleState.cs) — 18 satır; `PuzzleState`.

## Scripts/Modifiers

- [ ] [Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs](../Assets/_Cloudora/Scripts/Modifiers/CloudModifierRules.cs) — 55 satır; `PassiveModifierRule`, `FrozenModifierRule`, `LockedModifierRule`, `FogModifierRule`, `RainbowModifierRule`, `WindModifierRule`, `NightModifierRule`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs](../Assets/_Cloudora/Scripts/Modifiers/ICloudModifierRule.cs) — 18 satır; `ICloudModifierRule`, `IMoveCountModifier`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierData.cs) — 15 satır; `ModifierData`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierRuntime.cs) — 103 satır; `ModifierRuntime`.
- [ ] [Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs](../Assets/_Cloudora/Scripts/Modifiers/ModifierType.cs) — 13 satır; `ModifierType`.

## Scripts/Level

- [ ] [Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs](../Assets/_Cloudora/Scripts/Level/AuthoredLevelCatalog.cs) — 69 satır; `AuthoredLevelCatalog`.
- [ ] [Assets/_Cloudora/Scripts/Level/BalanceProfile.cs](../Assets/_Cloudora/Scripts/Level/BalanceProfile.cs) — 41 satır; `DifficultyBand`, `BalanceProfile`.
- [ ] [Assets/_Cloudora/Scripts/Level/BoardFrame.cs](../Assets/_Cloudora/Scripts/Level/BoardFrame.cs) — 37 satır; `BoardFrame`.
- [ ] [Assets/_Cloudora/Scripts/Level/CloudShuffler.cs](../Assets/_Cloudora/Scripts/Level/CloudShuffler.cs) — 114 satır; `CloudShuffleRejection`, `CloudShuffleResult`, `CloudShuffler`, `FailRecovery`.
- [ ] [Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs](../Assets/_Cloudora/Scripts/Level/DeterministicRandom.cs) — 35 satır; `DeterministicRandom`.
- [ ] [Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs](../Assets/_Cloudora/Scripts/Level/DifficultyCalculator.cs) — 27 satır; `DifficultyCalculator`.
- [ ] [Assets/_Cloudora/Scripts/Level/LevelDefinition.cs](../Assets/_Cloudora/Scripts/Level/LevelDefinition.cs) — 54 satır; `LevelDefinition`, `CloudDefinition`.
- [ ] [Assets/_Cloudora/Scripts/Level/LevelGenerator.cs](../Assets/_Cloudora/Scripts/Level/LevelGenerator.cs) — 145 satır; `LevelGenerator`.
- [ ] [Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs](../Assets/_Cloudora/Scripts/Level/PuzzleSolver.cs) — 628 satır; `SolveStatus`, `NaivePlayerStyle`, `DifficultyMetrics`, `PuzzleSolveResult`, `PuzzleSolver`, `ReachabilitySearch`, `StateKey`. Kilitli ölçüm,2/3 ileri sanal oyuncular, wander, güvenli durum araması; ReachabilitySearch burada kalsa da runtime fail tetikleyicisi değil.
- [ ] [Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs](../Assets/_Cloudora/Scripts/Level/PuzzleValidator.cs) — 74 satır; `PuzzleValidator`.
- [ ] [Assets/_Cloudora/Scripts/Level/V3BoardText.cs](../Assets/_Cloudora/Scripts/Level/V3BoardText.cs) — 41 satır; `V3BoardText`.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.Generated.cs) — 120 satır; `V3LevelData`. Yayımlanan100 kayıt; bütün satırlar okunur, elle yamanmaz.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelData.cs](../Assets/_Cloudora/Scripts/Level/V3LevelData.cs) — 40 satır; `V3LevelEntry`, `V3LevelData`.
- [ ] [Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs](../Assets/_Cloudora/Scripts/Level/V3LevelPlan.cs) — 48 satır; `V3Part`, `V3LevelPlan`. Shuffle15, bant/part/frame kuralları; güncel palet/kapasite için tablo ve V41Builder birlikte.

## Scripts/Progression

- [ ] [Assets/_Cloudora/Scripts/Progression/BoosterManager.cs](../Assets/_Cloudora/Scripts/Progression/BoosterManager.cs) — 41 satır; `BoosterManager`.
- [ ] [Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs](../Assets/_Cloudora/Scripts/Progression/CoinEconomy.cs) — 47 satır; `CoinEconomy`, `CoinWallet`.
- [ ] [Assets/_Cloudora/Scripts/Progression/LifeManager.cs](../Assets/_Cloudora/Scripts/Progression/LifeManager.cs) — 54 satır; `LifeManager`.
- [ ] [Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs](../Assets/_Cloudora/Scripts/Progression/ProgressionManager.cs) — 29 satır; `ProgressionManager`.
- [ ] [Assets/_Cloudora/Scripts/Progression/RestorationManager.cs](../Assets/_Cloudora/Scripts/Progression/RestorationManager.cs) — 17 satır; `RestorationManager`.
- [ ] [Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs](../Assets/_Cloudora/Scripts/Progression/WorldCatalog.cs) — 27 satır; `WorldCatalog`.
- [ ] [Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs](../Assets/_Cloudora/Scripts/Progression/WorldDefinition.cs) — 34 satır; `WorldDefinition`.

## Scripts/Services

- [ ] [Assets/_Cloudora/Scripts/Services/AdMobAdService.cs](../Assets/_Cloudora/Scripts/Services/AdMobAdService.cs) — 17 satır; `AdMobAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs](../Assets/_Cloudora/Scripts/Services/AdPlacementPolicy.cs) — 27 satır; `AdPlacementPolicy`.
- [ ] [Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs](../Assets/_Cloudora/Scripts/Services/AnalyticsEvents.cs) — 23 satır; `AnalyticsEvents`.
- [ ] [Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/DebugAnalyticsService.cs) — 20 satır; `DebugAnalyticsService`.
- [ ] [Assets/_Cloudora/Scripts/Services/FailRecord.cs](../Assets/_Cloudora/Scripts/Services/FailRecord.cs) — 84 satır; `FailedSnapshot`, `FailRecord`.
- [ ] [Assets/_Cloudora/Scripts/Services/FakeAdService.cs](../Assets/_Cloudora/Scripts/Services/FakeAdService.cs) — 28 satır; `FakeAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/FirebaseAnalyticsService.cs) — 42 satır; `FirebaseAnalyticsService`, `AnalyticsPrivacy`.
- [ ] [Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs](../Assets/_Cloudora/Scripts/Services/GameFeedbackService.cs) — 55 satır; `GameFeedbackService`.
- [ ] [Assets/_Cloudora/Scripts/Services/IAdService.cs](../Assets/_Cloudora/Scripts/Services/IAdService.cs) — 13 satır; `RewardedPlacement`, `IAdService`.
- [ ] [Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs](../Assets/_Cloudora/Scripts/Services/IAnalyticsService.cs) — 9 satır; `IAnalyticsService`.
- [ ] [Assets/_Cloudora/Scripts/Services/ISaveService.cs](../Assets/_Cloudora/Scripts/Services/ISaveService.cs) — 8 satır; `ISaveService`.
- [ ] [Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs](../Assets/_Cloudora/Scripts/Services/LocalJsonSaveService.cs) — 72 satır; `LocalJsonSaveService`. Kayıt/yedek/temp/migration; oyuncu verisini koru.
- [ ] [Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs](../Assets/_Cloudora/Scripts/Services/ReleaseConfiguration.cs) — 32 satır; `ReleaseConfigurationData`, `ReleaseConfiguration`.
- [ ] [Assets/_Cloudora/Scripts/Services/SaveData.cs](../Assets/_Cloudora/Scripts/Services/SaveData.cs) — 26 satır; `SaveData`.

## Scripts/Puzzle

- [ ] [Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs](../Assets/_Cloudora/Scripts/Puzzle/CloudContainerView.cs) — 484 satır; `CloudContainerView`, `EssenceRegion`.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs](../Assets/_Cloudora/Scripts/Puzzle/MoveAnimator.cs) — 213 satır; `MoveAnimator`, `WeatherFlowGraphic`.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs](../Assets/_Cloudora/Scripts/Puzzle/PuzzleController.cs) — 781 satır; `PuzzleController`, `PuzzleSnapshot`. Ana akışın tamamı: giriş, hamle, HasLegalMove, fail/recovery, ekonomi, kayıt ve ekranlar. Eski BeginSolvabilityCheck geri getirilmez.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherMotionLibrary.cs) — 62 satır; `WeatherMotionStyle`, `WeatherMotionProfile`, `WeatherMotionLibrary`. Elemente özel üstten uçuş/iz; cam sabit, iç sıvı ayrı.
- [ ] [Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs](../Assets/_Cloudora/Scripts/Puzzle/WeatherType.cs) — 17 satır; `WeatherType`.

## Scripts/UI

- [ ] [Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs](../Assets/_Cloudora/Scripts/UI/AdaptiveBoardLayout.cs) — 89 satır; `AdaptiveBoardLayout`, `LayoutMetrics`. Responsive tahta, ReservedTop/TutorialClearance, yoğun tahtalar.
- [ ] [Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs](../Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs) — 140 satır; `CloudoraVisuals`, `SkyGradientGraphic`.
- [ ] [Assets/_Cloudora/Scripts/UI/DesignUI.cs](../Assets/_Cloudora/Scripts/UI/DesignUI.cs) — 156 satır; `DesignUI`, `DesignScreenFit`, `DesignPanel`, `EssenceSymbolFit`.
- [ ] [Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs](../Assets/_Cloudora/Scripts/UI/GameplayOverlay.cs) — 170 satır; `FailOptions`, `GameplayOverlay`, `DesignModalFit`.
- [ ] [Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs](../Assets/_Cloudora/Scripts/UI/LiquidFillGraphic.cs) — 53 satır; `LiquidFillGraphic`. Birleşik sıvı mesh/dalga/yükselme; ikinci kutu/logo yok.
- [ ] [Assets/_Cloudora/Scripts/UI/MainMenuController.cs](../Assets/_Cloudora/Scripts/UI/MainMenuController.cs) — 139 satır; `MainMenuController`.
- [ ] [Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs](../Assets/_Cloudora/Scripts/UI/MilestoneScreen.cs) — 79 satır; `MilestoneScreen`. Dünya kurtuluş ekranı ve can/kaynak yenilemesi.
- [ ] [Assets/_Cloudora/Scripts/UI/OpeningSplash.cs](../Assets/_Cloudora/Scripts/UI/OpeningSplash.cs) — 215 satır; `OpeningSplash`.
- [ ] [Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs](../Assets/_Cloudora/Scripts/UI/SafeAreaFitter.cs) — 48 satır; `SafeAreaFitter`.
- [ ] [Assets/_Cloudora/Scripts/UI/VectorShapes.cs](../Assets/_Cloudora/Scripts/UI/VectorShapes.cs) — 134 satır; `UIShapes`, `GameIcon`, `ButtonMotion`, `PanelPresence`, `GlassVesselGraphic`, `VesselFillMask`.
- [ ] [Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs](../Assets/_Cloudora/Scripts/UI/VesselCompletionFlourish.cs) — 446 satır; `VesselCompletionFlourish`. Yedi bitiriş, son Ay sahnesi, havuz ve azaltılmış hareket.
- [ ] [Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs](../Assets/_Cloudora/Scripts/UI/WeatherGlyph.cs) — 117 satır; `WeatherGlyph`.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs](../Assets/_Cloudora/Scripts/UI/WorldAtmosphere.cs) — 57 satır; `WorldAtmosphere`, `GameplayBackdrop`.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs](../Assets/_Cloudora/Scripts/UI/WorldRescueReplay.cs) — 39 satır; `WorldRescueReplay`.
- [ ] [Assets/_Cloudora/Scripts/UI/WorldScreenController.cs](../Assets/_Cloudora/Scripts/UI/WorldScreenController.cs) — 90 satır; `WorldScreenController`.

## Editor

- [ ] [Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs](../Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs) — 52 satır; `CloudoraAndroidManifestPostprocessor`. Manifest izin/uyumluluk denetimi; gerçek servis ekleme görevi değildir.
- [ ] [Assets/_Cloudora/Editor/CloudoraBuildTools.cs](../Assets/_Cloudora/Editor/CloudoraBuildTools.cs) — 191 satır; `CloudoraBuildTools`, `SigningState`. Android dev/QA/release/Play, kimlik override, signing restore ve SDK/ikon ayarları.
- [ ] [Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs](../Assets/_Cloudora/Editor/CloudoraIosBuildTools.cs) — 78 satır; `CloudoraIosBuildTools`.
- [ ] [Assets/_Cloudora/Editor/ContentValidationTool.cs](../Assets/_Cloudora/Editor/ContentValidationTool.cs) — 38 satır; `ContentValidationTool`.
- [ ] [Assets/_Cloudora/Editor/DesignPreview.cs](../Assets/_Cloudora/Editor/DesignPreview.cs) — 70 satır; `DesignPreview`.
- [ ] [Assets/_Cloudora/Editor/DesignQuality.cs](../Assets/_Cloudora/Editor/DesignQuality.cs) — 26 satır; `DesignQuality`.
- [ ] [Assets/_Cloudora/Editor/LevelCandidateFactory.cs](../Assets/_Cloudora/Editor/LevelCandidateFactory.cs) — 96 satır; `LevelCandidateFactory`.
- [ ] [Assets/_Cloudora/Editor/LevelCandidateSelector.cs](../Assets/_Cloudora/Editor/LevelCandidateSelector.cs) — 122 satır; `ScoreWindow`, `LevelCandidate`, `CandidateSelection`, `LevelCandidateSelector`.
- [ ] [Assets/_Cloudora/Editor/OpeningSplashPreview.cs](../Assets/_Cloudora/Editor/OpeningSplashPreview.cs) — 72 satır; `OpeningSplashPreview`.
- [ ] [Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs](../Assets/_Cloudora/Editor/RenderingPerformanceBenchmark.cs) — 65 satır; `RenderingPerformanceBenchmark`, `Row`, `Report`.
- [ ] [Assets/_Cloudora/Editor/V3LevelTool.cs](../Assets/_Cloudora/Editor/V3LevelTool.cs) — 295 satır; `V3LevelTool`, `V3LevelData`. Güncel V41 builder girişleri; salt rapor ve üretim komutlarını ayır.
- [ ] [Assets/_Cloudora/Editor/V41LevelBuilder.cs](../Assets/_Cloudora/Editor/V41LevelBuilder.cs) — 445 satır; `V3Novelty`, `TrapClass`, `V3LevelResult`, `V41LevelBuilder`, `Curve`, `SequenceComparer`, `BuildState`. Güncel tek kampanya kurucusu; bütün seçim/plan/gevşetme/pool/deterministik üretimi oku.
- [ ] [Assets/_Cloudora/Editor/WeatherSymbolPreview.cs](../Assets/_Cloudora/Editor/WeatherSymbolPreview.cs) — 39 satır; `WeatherSymbolPreview`.

## Tests/Editor

- [ ] [Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs](../Assets/_Cloudora/Tests/Editor/AbandonAttemptTests.cs) — 51 satır; `AbandonAttemptTests`, `MemorySave`.
  İnceleme/test girişleri: `Save`, `HomeFromActiveAttemptSpendsOneLifeAndRestoresOriginalBoardButWinIsFree`.
- [ ] [Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs](../Assets/_Cloudora/Tests/Editor/CampaignReleaseTests.cs) — 59 satır; `CampaignReleaseTests`.
  İnceleme/test girişleri: `ExactlyThreeWorldsCoverAllPostIntroLevelsThrough100`, `EveryWorldProgressUsesOnlyItsOwnCompletedLevels`, `WorldBoundariesMatchTheApprovedCampaign`, `FinishedCampaignNeverLoadsOrSavesLevel101`, `WinHomeButtonCallsHomeWithoutAwardingOrContinuing`, `TransferPreviewDoesNotMutateBoardAndCanBeCancelled`.
- [ ] [Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionEconomyTests.cs) — 110 satır; `CompletionEconomyTests`, `MemorySave`.
  İnceleme/test girişleri: `EveryLevelUsesTheRequestedRewardCycleAndContinueFee`, `CompletedSourcesAreImmutableInCoreViewAndShuffleIncludingWildcards`, `Save`, `ControllerCreditsOnlyOnceAndCharges500OnlyForSuccessfulContinue`, `RealControllerMoveTriggersEveryElementFinishAfterTopEntry`.
- [ ] [Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs](../Assets/_Cloudora/Tests/Editor/CompletionFlourishTests.cs) — 115 satır; `CompletionFlourishTests`.
  İnceleme/test girişleri: `RichCompletionMeshesStayBoundedAndAllocateNoGarbageAfterWarmup`, `FinishCrownsFitTheHudAndDenseBoardEdges`, `AllSevenFinishesKeepTheGlassFixedAndCleanUpOnUndo`.
- [ ] [Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs](../Assets/_Cloudora/Tests/Editor/DesignNavigationTests.cs) — 29 satır; `DesignNavigationTests`.
  İnceleme/test girişleri: `MilestoneDoesNotAdvanceUntilPlayIsPressed`.
- [ ] [Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs](../Assets/_Cloudora/Tests/Editor/ElementMotionTests.cs) — 91 satır; `ElementMotionTests`.
  İnceleme/test girişleri: `EveryElementHasItsOwnStyleAndUnknownElementsHaveSafeFallback`, `FutureElementCanUseAuthoredProfileWithoutChangingAnimator`, `AllElementsKeepBothVesselsFixedAndCancellationRestoresPreviews`.
- [ ] [Assets/_Cloudora/Tests/Editor/LevelToolTests.cs](../Assets/_Cloudora/Tests/Editor/LevelToolTests.cs) — 569 satır; `LevelToolTests`.
  İnceleme/test girişleri: `SolvedBoardNeedsZeroMoves`, `OneMoveBoardIsSolvedInOneMove`, `BoardWithoutLegalMovesIsUnsolvable`, `SolverRejectsMalformedBoards`, `SolverDoesNotMutateInputBoard`, `SolutionsReplayWithGameRulesAndHaveOptimalLength`, `OptimalMovesMatchIndependentReferenceSearch`, `CloudOrderDoesNotChangeMetrics`, `SolverIsDeterministic`, `StateLimitIsRespectedAndReported`, `MetricRatiosStayInRange`, `ScoreRisesWithLengthTrapsMisdirectionAndNaivePlay`, `NaivePlayerSolvesObviousBoardWithoutExtraMoves`, `NaivePlayerMetricsStayInRange`, `NaivePlayerFailsMoreWhereTrapsExist`, `TwoEmptyThreeColorBoardsNeverTrapAnyPlayer`, `ThinkingPlayerFallsIntoFewerTrapsThanNaivePlayer`, `ThinkingPlayerSeesAClosedDeadEndOneMoveAhead`, `LookingFurtherAheadNeverFallsIntoMoreTraps`, `NaiveFailureWeighsFifteenInTheScore`, `MeasurementUsesTheLockedCompletionRuleByDefault`, `NaivePlayerAgreesWithDeviceTestData`, `SensiblePlayerBeatsRandomPlayer`, `DealMatchesFrameExactly`, `DealIsDeterministicAndSeedSensitive`, `RandomSequenceIsFixedAcrossPlatforms`, `FrameRejectsInvalidValues`, `SelectionPicksInWindowCandidatesWithDistinctBackups`, `SelectionPrefersScoreClosestToWindowCenter`, `SelectionFailsLoudlyWhenWindowIsUnreachable`, `SelectionFailsWhenBackupsAreMissing`, `SelectionSkipsCandidatesOverTheStateLimit`, `SelectedCandidateBecomesValidLevelDefinition`, `V3PatternRepeatsEveryFiveLevels`, `V3PartsCoverOneToHundred`.
- [ ] [Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs](../Assets/_Cloudora/Tests/Editor/LiquidMotionTests.cs) — 70 satır; `LiquidMotionTests`.
  İnceleme/test girişleri: `TopEntryConservesVolumeAndNeverFillsBeforeArrival`, `SameElementGrowsOneFluidBodyAndCancellationRestoresTheExactView`, `LiquidSurfaceIsBoundedAndItsWarmedMeshAllocatesNoGarbage`.
- [ ] [Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs](../Assets/_Cloudora/Tests/Editor/MainMenuLifeTests.cs) — 26 satır; `MainMenuLifeTests`.
  İnceleme/test girişleri: `ZeroLivesOffersARefillAndRegenerationRestoresThePlayAction`.
- [ ] [Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs](../Assets/_Cloudora/Tests/Editor/MoveFlowTests.cs) — 39 satır; `MoveFlowTests`.
  İnceleme/test girişleri: `FlowCompletesWithTimeScaleZeroAndLeavesCommitToController`.
- [ ] [Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs](../Assets/_Cloudora/Tests/Editor/OpeningSplashTests.cs) — 51 satır; `OpeningSplashTests`.
  İnceleme/test girişleri: `SplashWaitsForReadinessAndReleasesInputWithTimeScaleZero`, `ReducedMotionUsesStaticCloudsAndNoGlint`.
- [ ] [Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs](../Assets/_Cloudora/Tests/Editor/PointerMotionVisualTests.cs) — 91 satır; `PointerMotionVisualTests`.
  İnceleme/test girişleri: `TransparentVesselReceivesTouchesAndElementTransfersRenderToCompletion`.
- [ ] [Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs](../Assets/_Cloudora/Tests/Editor/PuzzleFeelTests.cs) — 419 satır; `PuzzleFeelTests`.
  İnceleme/test girişleri: `GroupMoveAndSnapshotRestoreRemainConsistent`, `InvalidDestinationDoesNotMutateState`, `AuthoredTutorialLevelsHaveValidElementCounts`, `GeneratedLevelsAreDeterministicAndKnownSolvable`, `PureRulesApplyGroupedMoveAndDetectSolved`, `PureRulesRejectPartialGroupMoveThatPlayerCannotChoose`, `WindMovesOneElementFromModifiedCloud`, `SafeAreaAnchorsRespectCutoutInsets`, `StartupRepairsInvisibleRootCanvas`, `ManifestFilterRemovesOnlyInternetPermission`, `AdaptiveLayoutFitsPortraitBounds`, `TutorialCueKeepsTheBoardBelowTheText`, `WorldProgressionCrossesBoundaryAndRestoresInStages`, `CompletedLevelResumesAtNextLevelWithoutAdvancingActiveScreen`, `RainbowIsWildcardWithoutAmbiguousSolvedState`, `GeneratorIntroducesExpectedModifier`, `LivesRegenerateOfflineAndRespectCap`, `TutorialRetryIsFreeAndBoostersCannotUnderflow`, `ZeroLivesBlocksNewNormalAttemptButNotTutorialOrPaidCurrentAttempt`, `ModifierSnapshotAndExtraCloudPreserveDynamicCounters`, `SaveRoundTripsAndCorruptPrimaryFallsBackToBackup`, `ValidSchemaSaveStillNormalizesUnsafeFields`, `AnalyticsRejectsPersonalDataKeys`, `AdPolicyProtectsEarlyLevelsAndFrequencyCaps`, `DifficultyWaveHasFinaleAndEasyWorldReset`.
- [ ] [Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs](../Assets/_Cloudora/Tests/Editor/ReachabilityRegressionTests.cs) — 60 satır; `ReachabilityRegressionTests`.
  İnceleme/test girişleri: `ScreenshotYellowCycleIsALossDespiteLegalMoves`, `ExtraVesselRescuesTheScreenshotAndCannotReuseADeadCacheEntry`, `ExhaustedFrameBudgetNeverMeansLost`, `All100StartingBoardsHaveSolutionsUnderRuntimeRules`, `IncrementalSearchAgreesWithIndependentFullGraphAndPermutation`.
- [ ] [Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs](../Assets/_Cloudora/Tests/Editor/RenderingReuseTests.cs) — 114 satır; `RenderingReuseTests`.
  İnceleme/test girişleri: `WarmedElementMeshesGenerateNoManagedGarbage`, `RepeatedMovesReuseGlassAndBoundedRegionsAndKeepUndoAccurate`, `WorldChangeInvalidatesAtmosphereEvenWhenMotionIsStatic`, `RepeatedFailedStateDoesNotRebuildItsText`.
- [ ] [Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs](../Assets/_Cloudora/Tests/Editor/V31GameplayTests.cs) — 317 satır; `V31GameplayTests`.
  İnceleme/test girişleri: `ShuffleRejectsCloudsThatCannotChange`, `ShuffleNeverTouchesProtectedClouds`, `ShuffleChangesOnlyTheChosenCloudAndKeepsItsContents`, `ShuffleAlwaysLeavesASolvableBoard`, `ShuffleReportsWhenNoSafeArrangementExists`, `ShuffleIsDeterministicPerSeedAndVariesAcrossSeeds`, `ContinueFindsNewestSolvableState`, `CoinRewardsFollowDifficultyAndContinueIsMeaningful`, `CoinWalletNeverGoesNegative`, `CoinsSurviveSaveAndAreNormalized`, `FailRecordRoundTripsThroughSave`, `FailRecordIsIgnoredWhenItDoesNotFit`, `OldSavesLoadWithoutFailRecord`, `FigmaHelpersHaveDistinctTouchTargetsAndNoRestart`, `FailPanelOffersLifeAdAndCoinsAndBlocksBoosters`, `LifeAdRemainsAvailableFromPauseMenu`, `ShuffleUnlocksAtFifteenByLevelNumber`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32InputTests.cs](../Assets/_Cloudora/Tests/Editor/V32InputTests.cs) — 55 satır; `V32InputTests`.
  İnceleme/test girişleri: `AnalyticsLinesCarryFrameAndTime`, `InfoPopupInterceptsInputAndDismissesWithoutUsingHelper`, `BottomButtonClicksAreLoggedWithFrame`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs) — 89 satır; `V32LayoutTests`.
  İnceleme/test girişleri: `DefaultLayoutIsTheClassicPackedBoard`, `ScatteredLayoutKeepsPiecesAndFreeSpace`, `ScatteredLayoutIsDeterministicAndVaried`, `ScatteredBoardsAreSolvedWithTheGameRules`, `InvalidOpenCloudCountsAreRejected`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs) — 116 satır; `V32NoMovesTests`.
  İnceleme/test girişleri: `Level24BoardCanFailOnTheFirstMove`, `ThreeColorsWithTwoEmptiesNeverRunOutOfMoves`, `NoMovesDepthMatchesIndependentSearch`, `NoMovesDoesNotChangeTheScore`, `ScoreAgreesWithEditorTestOnSingleEmptyBoards`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs) — 45 satır; `V32ShuffleLoadProbe`.
  İnceleme/test girişleri: `MeasureShuffleOnLargeBoards`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs) — 92 satır; `V32SolverLoadTests`.
  İnceleme/test girişleri: `QuickCheckAgreesWithTheFullSolver`, `QuickCheckReportsUnsolvableAndLimit`, `ShuffleWithExtraCloudsIsFastAndNeverFalselyRejected`, `FailRecoveryWithExtraCloudsIsFast`.
- [ ] [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs) — 64 satır; `V32VisualTests`.
  İnceleme/test girişleri: `WeatherColorsMatchChosenPalette`, `WeatherColorsAreClearlyDistinct`, `WeatherUsesDistinctModernSymbolsWithoutInterceptingInput`.
- [ ] [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs) — 337 satır; `V3LevelBuildTests`.
  İnceleme/test girişleri: `BuildCoversOneToHundredWithTheFivePattern`, `FramesFollowThePlan`, `ScatteredLayoutsOnlyWhereTheyLetThePlayerBreathe`, `BoardsAreDistinct`, `HarderBandsScoreHigherWithinEachCycle`, `SameBandBarelyGetsEasierWithinAPart`, `MostLevelsHitTheirTargetScore`, `EveryCycleFromTwentyOneHasTwoHighTwoMediumAndOneZero`, `TrapRatesFollowTheirClassAndThinkingPlayersPass`, `NoMovesNeverInTheFirstMovesAndEverySingleEmptyLevelCanReachIt`, `NoMovesComesNoEarlierThanTheFourthMove`, `TrapsFromThirtyOneNeedRealThought`, `LastLevelIsTheHardestExamAndMakesCarefulPlayersThink`, `FinalesAreExamsAndNotShort`, `TutorialCuesAreDemonstratedByTheBoard`, `BuildIsDeterministic`, `BuiltLevelsAreValidLevelDefinitions`, `GeneratedDataMatchesBuilder`, `CatalogUsesV3BoardsAndKeepsTutorialFrame`, `LevelsAfterTheCampaignStayOnOldSystem`, `BuiltLevelsLoadFromV3Data`, `BoardTextRoundTrips`.

## Sahne, prefab, platform ve varlık okuması

[Assets/_Cloudora/Scenes/02_Gameplay.unity](../Assets/_Cloudora/Scenes/02_Gameplay.unity) → [Assets/_Cloudora/Prefabs/CloudContainer.prefab](../Assets/_Cloudora/Prefabs/CloudContainer.prefab) → script.meta GUID bağları → runtime kurulumu. Serialize alan ile programatik UI ayrımını açıkla. `Assets/Settings/Build Profiles` Android profilini, `Assets/Plugins/iOS` koşullu native kodu taşır.

Görseller/fontlar için Resources yükleme adı, piksel çözünürlük, maxSize/compression/filter/platform override, LFS ve lisansı incele. `.meta` GUID kaybı referansı koparır. DesignSources tasarım referansıdır; otomatik yeniden export yapma. `StoreAssets/Play` yayın adayıdır, üretim onayı değildir. Nunito-OFL korunur.

Önce ProjectVersion/manifest/lock, sonra EditorBuildSettings/Player/Quality/Graphics/input ayarlarını oku. Mevcut Unity açılış farklarını otomatik commit etme. Android wrapper, manifest postprocessor ve Google Play sürecini birlikte oku. iOS derleme aracı Android aracı değildir.

## Belgelerin tamamı ve öncelik

Güncel giriş/rota → V41 tasarım/harita → gerçek kod/test → konu belgeleri → tarihçe/öneri. COIN_EKONOMISI/TEMA_ATLASI gelecek fikirleridir. Eski iOS1ee4334 veya v3.2 metnini güncel main kararı yapma. HTML sayfalarını tarayıcıda da incele.

- [CLAUDE.md](../CLAUDE.md) — Güncel giriş/tasarım/okuma rotası.
- [CONTRIBUTORS.md](../CONTRIBUTORS.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [ClaudoraRelease.md](../ClaudoraRelease.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/ANDROID_BUILD.md](../Docs/ANDROID_BUILD.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/COIN_EKONOMISI.html](../Docs/COIN_EKONOMISI.html) — Seçim bekleyen öneri; uygulanmış özellik değil.
- [Docs/DAL_KARSILASTIRMASI_2026-11-24.md](../Docs/DAL_KARSILASTIRMASI_2026-11-24.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/DOSYA_OKUMA_HARITASI.md](../Docs/DOSYA_OKUMA_HARITASI.md) — Güncel giriş/tasarım/okuma rotası.
- [Docs/DesignSources/Nunito-OFL.txt](../Docs/DesignSources/Nunito-OFL.txt) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/EKIP_ONBOARDING.md](../Docs/EKIP_ONBOARDING.md) — Güncel giriş/tasarım/okuma rotası.
- [Docs/GELISTIRME_ONERILERI.md](../Docs/GELISTIRME_ONERILERI.md) — Seçim bekleyen öneri; uygulanmış özellik değil.
- [Docs/GOOGLE_PLAY_SUBMISSION.md](../Docs/GOOGLE_PLAY_SUBMISSION.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/GUNCEL_DURUM.md](../Docs/GUNCEL_DURUM.md) — Güncel giriş/tasarım/okuma rotası.
- [Docs/IOS_ARCHITECTURE.md](../Docs/IOS_ARCHITECTURE.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_BUILD.md](../Docs/IOS_BUILD.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_COMPLETION_EFFECTS.md](../Docs/IOS_COMPLETION_EFFECTS.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_DESIGN_IMPLEMENTATION.md](../Docs/IOS_DESIGN_IMPLEMENTATION.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_DEVICE_QA.md](../Docs/IOS_DEVICE_QA.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_ELEMENT_MOTION.md](../Docs/IOS_ELEMENT_MOTION.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_FARKLARI.html](../Docs/IOS_FARKLARI.html) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_FARKLARI.md](../Docs/IOS_FARKLARI.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_ITEM_TRANSFER_RESTORE.md](../Docs/IOS_ITEM_TRANSFER_RESTORE.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_LIQUID_MOTION.md](../Docs/IOS_LIQUID_MOTION.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_MOON_FROM_60.md](../Docs/IOS_MOON_FROM_60.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_OPENING_SPLASH.md](../Docs/IOS_OPENING_SPLASH.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_PERFORMANCE.md](../Docs/IOS_PERFORMANCE.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_QUALITY_CAMPAIGN.md](../Docs/IOS_QUALITY_CAMPAIGN.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_STATUS.md](../Docs/IOS_STATUS.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_UX_REVIEW.md](../Docs/IOS_UX_REVIEW.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/IOS_WEATHER_SYMBOLS.md](../Docs/IOS_WEATHER_SYMBOLS.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/PRIVACY_RELEASE_REVIEW.md](../Docs/PRIVACY_RELEASE_REVIEW.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/PROJECT_PROGRESS.md](../Docs/PROJECT_PROGRESS.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/PROJE_HAFIZASI.md](../Docs/PROJE_HAFIZASI.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/RELEASE_CHECKLIST.md](../Docs/RELEASE_CHECKLIST.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [Docs/SONRAKI_ADIMLAR.md](../Docs/SONRAKI_ADIMLAR.md) — Seçim bekleyen öneri; uygulanmış özellik değil.
- [Docs/TEMA_ATLASI.html](../Docs/TEMA_ATLASI.html) — Seçim bekleyen öneri; uygulanmış özellik değil.
- [Docs/V3_BOLUM_HARITASI.html](../Docs/V3_BOLUM_HARITASI.html) — Tarihsel kayıt/karşılaştırma.
- [Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md](../Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/V3_CERCEVE_ENVANTERI.md](../Docs/V3_CERCEVE_ENVANTERI.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/V3_SABLON.md](../Docs/V3_SABLON.md) — Tarihsel kayıt/karşılaştırma.
- [Docs/V41_BOLUM_HARITASI.html](../Docs/V41_BOLUM_HARITASI.html) — Güncel giriş/tasarım/okuma rotası.
- [Docs/V41_TASARIM.md](../Docs/V41_TASARIM.md) — Güncel giriş/tasarım/okuma rotası.
- [README.md](../README.md) — Güncel giriş/tasarım/okuma rotası.
- [StoreAssets/README.md](../StoreAssets/README.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [StoreAssets/STORE_COPY.md](../StoreAssets/STORE_COPY.md) — Konu/süreç/lisans; GUNCEL_DURUM ile birlikte.
- [crVersion.md](../crVersion.md) — Tarihsel kayıt/karşılaştırma.

## Görev olmadan değiştirilmeyecek alanlar

| Alan | Birlikte korunacak sözleşme |
|---|---|
| Kurallar/solver/controller | Tamamlanan kilidi, No Moves fail, Undo ve güvenli kurtarma |
| V41Builder/plan/tablo | Seed/puan/trap/2-3lookahead,100 kayıt, Ay61/Shuffle15/kapasite4-5-6 |
| Ekonomi/save |100-200-300/500, can ücretleri, şema2 ve fail/backup geçmişi |
| UI/motion | Sabit cam, üstten uçuş, birleşik sıvı,7 bitiriş, tutorial clearances, hareket tercihi/havuzlar |
| Varlık/paket/platform | GUID/LFS/font lisansı, paket kilidi, manifest/imza/yayın hedefi |

Bunlar mutlak dokunulmaz dosyalar değil; kapsam dışı değiştirilmeyecek alanlardır. Yeni açık görev ilgili alanı değiştirebilir. Generated veri elle yamanmaz, cihaz kaydı test kolaylığı için izinsiz sıfırlanmaz. İlk okuma görevinde kaynak değişikliği yapılmaz.

## Tam sürüm kontrollü envanter

Bütün dosyalar aşağıdadır; binary dosyayı metin gibi okumak yerine kullanım/import/hash/lisans denetimi yap. Library/Logs/Builds ve kişisel anahtar/kayıtlar Git dışında kalır.

<details>
<summary>Assets — 439 dosya</summary>

- [Assets/Plugins.meta](../Assets/Plugins.meta)
- [Assets/Plugins/iOS.meta](../Assets/Plugins/iOS.meta)
- [Assets/Plugins/iOS/CloudoraAccessibility.mm](../Assets/Plugins/iOS/CloudoraAccessibility.mm)
- [Assets/Plugins/iOS/CloudoraAccessibility.mm.meta](../Assets/Plugins/iOS/CloudoraAccessibility.mm.meta)
- [Assets/Resources.meta](../Assets/Resources.meta)
- [Assets/Resources/BillingMode.json](../Assets/Resources/BillingMode.json)
- [Assets/Resources/BillingMode.json.meta](../Assets/Resources/BillingMode.json.meta)
- [Assets/Resources/CloudoraReleaseConfig.json](../Assets/Resources/CloudoraReleaseConfig.json)
- [Assets/Resources/CloudoraReleaseConfig.json.meta](../Assets/Resources/CloudoraReleaseConfig.json.meta)
- [Assets/Scenes.meta](../Assets/Scenes.meta)
- [Assets/Scenes/SampleScene.unity](../Assets/Scenes/SampleScene.unity)
- [Assets/Scenes/SampleScene.unity.meta](../Assets/Scenes/SampleScene.unity.meta)
- [Assets/Settings.meta](../Assets/Settings.meta)
- [Assets/Settings/Build Profiles.meta](../Assets/Settings/Build%20Profiles.meta)
- [Assets/Settings/Build Profiles/Android™.asset](../Assets/Settings/Build%20Profiles/Android™.asset)
- [Assets/Settings/Build Profiles/Android™.asset.meta](../Assets/Settings/Build%20Profiles/Android™.asset.meta)
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
- [Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs](../Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs)
- [Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs.meta](../Assets/_Cloudora/Editor/CloudoraAndroidManifestPostprocessor.cs.meta)
- [Assets/_Cloudora/Editor/CloudoraBuildTools.cs](../Assets/_Cloudora/Editor/CloudoraBuildTools.cs)
- [Assets/_Cloudora/Editor/CloudoraBuildTools.cs.meta](../Assets/_Cloudora/Editor/CloudoraBuildTools.cs.meta)
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
- [Assets/_Cloudora/Editor/V3LevelTool.cs](../Assets/_Cloudora/Editor/V3LevelTool.cs)
- [Assets/_Cloudora/Editor/V3LevelTool.cs.meta](../Assets/_Cloudora/Editor/V3LevelTool.cs.meta)
- [Assets/_Cloudora/Editor/V41LevelBuilder.cs](../Assets/_Cloudora/Editor/V41LevelBuilder.cs)
- [Assets/_Cloudora/Editor/V41LevelBuilder.cs.meta](../Assets/_Cloudora/Editor/V41LevelBuilder.cs.meta)
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
- [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32LayoutTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32NoMovesTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs)
- [Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs.meta](../Assets/_Cloudora/Tests/Editor/V32ShuffleLoadProbe.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32SolverLoadTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs)
- [Assets/_Cloudora/Tests/Editor/V32VisualTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V32VisualTests.cs.meta)
- [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs)
- [Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs.meta](../Assets/_Cloudora/Tests/Editor/V3LevelBuildTests.cs.meta)
- [Assets/_Cloudora/UI.meta](../Assets/_Cloudora/UI.meta)

</details>

<details>
<summary>Docs — 112 dosya</summary>

- [Docs/ANDROID_BUILD.md](../Docs/ANDROID_BUILD.md)
- [Docs/COIN_EKONOMISI.html](../Docs/COIN_EKONOMISI.html)
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
- [Docs/GOOGLE_PLAY_SUBMISSION.md](../Docs/GOOGLE_PLAY_SUBMISSION.md)
- [Docs/GUNCEL_DURUM.md](../Docs/GUNCEL_DURUM.md)
- [Docs/IOS_ARCHITECTURE.md](../Docs/IOS_ARCHITECTURE.md)
- [Docs/IOS_BUILD.md](../Docs/IOS_BUILD.md)
- [Docs/IOS_COMPLETION_EFFECTS.md](../Docs/IOS_COMPLETION_EFFECTS.md)
- [Docs/IOS_DESIGN_IMPLEMENTATION.md](../Docs/IOS_DESIGN_IMPLEMENTATION.md)
- [Docs/IOS_DEVICE_QA.md](../Docs/IOS_DEVICE_QA.md)
- [Docs/IOS_ELEMENT_MOTION.md](../Docs/IOS_ELEMENT_MOTION.md)
- [Docs/IOS_FARKLARI.html](../Docs/IOS_FARKLARI.html)
- [Docs/IOS_FARKLARI.md](../Docs/IOS_FARKLARI.md)
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
- [Docs/TEMA_ATLASI.html](../Docs/TEMA_ATLASI.html)
- [Docs/V3_BOLUM_HARITASI.html](../Docs/V3_BOLUM_HARITASI.html)
- [Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md](../Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md)
- [Docs/V3_CERCEVE_ENVANTERI.md](../Docs/V3_CERCEVE_ENVANTERI.md)
- [Docs/V3_SABLON.md](../Docs/V3_SABLON.md)
- [Docs/V41_BOLUM_HARITASI.html](../Docs/V41_BOLUM_HARITASI.html)
- [Docs/V41_TASARIM.md](../Docs/V41_TASARIM.md)

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
<summary>StoreAssets — 9 dosya</summary>

- [StoreAssets/Export-PlayAssets.ps1](../StoreAssets/Export-PlayAssets.ps1)
- [StoreAssets/Play/cloudora-adaptive-background-512.png](../StoreAssets/Play/cloudora-adaptive-background-512.png)
- [StoreAssets/Play/cloudora-adaptive-foreground-512.png](../StoreAssets/Play/cloudora-adaptive-foreground-512.png)
- [StoreAssets/Play/cloudora-feature-1024x500.png](../StoreAssets/Play/cloudora-feature-1024x500.png)
- [StoreAssets/Play/cloudora-icon-512.png](../StoreAssets/Play/cloudora-icon-512.png)
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
<summary>root — 10 dosya</summary>

- [.gitattributes](../.gitattributes)
- [.gitignore](../.gitignore)
- [.vsconfig](../.vsconfig)
- [Build-Android.ps1](../Build-Android.ps1)
- [Build-iOS.sh](../Build-iOS.sh)
- [CLAUDE.md](../CLAUDE.md)
- [CONTRIBUTORS.md](../CONTRIBUTORS.md)
- [ClaudoraRelease.md](../ClaudoraRelease.md)
- [README.md](../README.md)
- [crVersion.md](../crVersion.md)

</details>
