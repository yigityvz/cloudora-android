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
