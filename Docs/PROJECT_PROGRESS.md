# Cloudora Project Progress

## Baseline

- Phase 0: complete
- Phase 1: complete (`39338b3`)

## Phase 2 - Puzzle Feel & Core Tools

Implemented restart, multi-step undo snapshots, invalid destination shake/haptic hooks, a fast arcing transfer animation with input locking, one-shot solved-cloud feedback, runtime gameplay controls, a visible level-complete panel, and settings-ready audio hooks.

Manual smoke test:

1. Open `Assets/_Cloudora/Scenes/02_Gameplay.unity` and enter Play Mode.
2. Verify source selection, cancellation, legal grouped movement, and invalid shake.
3. Make several moves, undo them in order, then restart and compare with the initial board.
4. Solve the level and confirm each completed cloud celebrates once and the complete panel blocks further moves.

Automated/build verification is recorded in the commit that closes this phase.

## Phase 3 - Level Data & Tutorial Foundation

The prototype constant was replaced by serializable `LevelDefinition`/`CloudDefinition` data and a dedicated authored catalog. Fifteen deterministic onboarding levels now carry level, world, capacity, seed, and minimal visual cue metadata. The controller supports level loading, restart from the exact authored state, continue flow, and an Inspector debug start level/context action.

Manual smoke test: set `Debug Start Level` to 1, 6, 10, and 15; enter Play Mode; verify the correct board, header, cue, restart state, and continue transition.

## Phase 4 - Procedural Generator

Added pure C# `CloudState`, `PuzzleState`, `Move`, and `PuzzleRules` domain logic. Level 16+ now uses a deterministic generator that begins solved, applies controlled reversible moves, stores a known legal solution, rejects invalid/already-solved boards, and assigns a reproducible difficulty score. Seed override and debug start-level loading support broken-level reproduction.

Batch verification generates hundreds of boards, checks determinism/invariants, and replays every known solution to solved state.

## Phase 5 - Capacity & Adaptive Layout

Cloud state/view capacity is data-driven across 4, 5, 6, and 7 slots. `AdaptiveBoardLayout` calculates column count, cell aspect, and spacing for 5-12 clouds, anchors the board inside portrait safe regions, and recalculates after resolution changes. Automated cases cover 720x1280, 1080x1920, 1080x2400, and 1440x3200 portrait spaces without overlap.

Physical Android readability remains part of the Phase 14 device checklist and cannot be truthfully marked complete without a connected device.

## Phase 6 - World, Restoration & Progression

Thirteen data-driven worlds now define level ranges, capacity, new-rule cadence, and theme colors through level 400. Pure progression and restoration services calculate unlocks, four roughly five-level milestones, 100% completion, and world transitions. Gameplay exposes a themed world panel with restoration state and upcoming rule.

## Phase 7 - Modifier System

An extensible `ICloudModifierRule` registry drives modifier behavior independently of views. Frozen blocks a reserved cloud for two successful moves; Locked depends on a key cloud becoming solved; Fog reveals after interaction; Rainbow is a true wildcard in receive and solved rules; Night hides lower tokens; Wind is registered for later-level composition. Generated boards reserve blocked clouds from the known solution path and validate wildcard deficits.

## Phase 8 - Lives & Boosters

Added a five-life, 30-minute UTC regeneration model with tutorial grace and retry cost only after meaningful progress. Undo now spends charges; Extra Cloud expands the live board and layout; Safe Shuffle replaces generated boards only with a newly validated deterministic board. HUD counters, regeneration countdown, out-of-lives, no-moves, and depleted-booster panels are wired to gameplay. Reward acquisition remains an abstraction hook until fake ads in Phase 11.

## Phase 9 - Persistence

Implemented schema-versioned local JSON persistence for current/highest level, derived world restoration, lives and UTC timer, tutorial completion flags, sound/haptics, booster inventory, and generated seed metadata. Writes use a temporary file plus previous-save backup; load falls back to backup or safe defaults after corruption, and migration clamps invalid legacy values.

## Phase 10 - Analytics

Added an `IAnalyticsService` boundary, canonical no-PII event dictionary, privacy-key rejection, and instrumentation for game/level lifecycle, duration, moves, worlds, modifiers, failures, lives, and boosters. `FirebaseAnalyticsService` discovers the Firebase Unity SDK at runtime when present and otherwise mirrors events to a debug sink, keeping builds credential-free until Google configuration is supplied.

## Phase 11 - Ad Abstraction / Fake Monetization

Gameplay depends only on `IAdService`. Validation builds instantiate `FakeAdService`, log every rewarded/interstitial opportunity, and grant life/Undo/Extra Cloud/Safe Shuffle rewards deterministically. Interstitial policy blocks levels 1-5, requires four eligible completions, and applies a five-minute cap. `AdMobAdService` is an unavailable no-op stub; no SDK, ad unit, or real monetization is enabled.

## Phase 12 - Full UI/UX, Art & Accessibility

Added an original runtime-generated visual system: soft sky gradient, rounded frosted panels, cloud-lobed vessels, subtle shadows, selection lift, and color plus unique high-contrast weather patterns. Main Menu, Gameplay HUD, World, Settings, No Moves/Lives, and Level Complete surfaces now share the palette. Large touch targets, sound/haptic toggles, responsive layout, and pulsing minimal tutorial cues support one-thumb and non-color-only play without licensed assets.

## Phase 13 - Difficulty Balancing & Content Validation

Difficulty is config-driven through named bands and a repeating easy/medium/hard/medium/very-hard wave, with explicit world finales and easy resets. Shuffle depth follows the band instead of rising forever; duration targets are 30-60 seconds early, 1-3 minutes midgame, and 3-5 minutes for later challenge content. `Cloudora/Validate 10,000 Levels` provides reproducible batch QA across endless seeds, modifiers, capacity, and world cycles.

Validation result (2026-09-15, Unity 6000.5.9f1): 10,000/10,000 generated levels passed with zero invariant/known-solution failures; observed difficulty score range 29.9-108.3. The EditMode regression suite separately generated 2,000 levels and passed 23/23 tests.

## Phase 14 - Mobile Optimization, QA & Release Engineering

Android settings are enforced as portrait 1080x1920 reference, package `com.yigityvz.cloudora`, version 0.1.0, min API 26, target API 36, IL2CPP, and ARM64. Build settings now use the gameplay scene. Runtime targets 60 FPS and low-priority background loading; pause/save and offline life math are covered. The Windows batch wrapper handles a Java 17 NIO pipe failure in the Unicode user profile. A development APK built successfully with Unity BuildPlayer on 2026-09-16 (52,492,065 bytes; zero build errors), then the wrapper repeated the successful build. The 23/23 EditMode XML referenced during Phase 14 was from 2026-09-15, not a fresh Phase 14 run: the command's `-quit` flag exited before tests. A corrected Phase 15 command subsequently generated a new 25/25 passing XML. Physical touch/FPS/cutout testing remains open.

## Phase 15 - Store / Portfolio / Release Candidate

Original Cloudora-branded icon and feature art have exact-size 512×512 and 1024×500 PNG candidates with source images and a deterministic export script. The Adaptive icon is wired into Android PlayerSettings for the API 26+ build. English/Turkish store copy, a portfolio README, privacy inventory, and owner-controlled release checklist are prepared. Phase 15 QA fixed modifier-aware Undo, Extra Cloud state preservation, zero-life entry gating, completed-level resume, and in-game menu access. Unused Unity Analytics, IAP, and Multiplayer Center template packages were removed, and Unity Connect was disabled. A fresh 2026-09-21 EditMode XML passed 26/26 after this cleanup. Final-source Unity BuildPlayer produced a 45,253,539-byte development APK and a 34,669,647-byte internal AAB with zero build errors; package/version/API/ARM64 and Adaptive icon contents were inspected. The AAB is Android Debug-signed and is not publishable. Genuine Android screenshots, physical device testing, release signing, Firebase consent/configuration, policy URL, and explicit publication approval remain open. No real AdMob or publishing has been activated.
