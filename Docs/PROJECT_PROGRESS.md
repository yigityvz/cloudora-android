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
