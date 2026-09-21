# Cloudora

**Fix the sky. Restore the world.** Cloudora is an Android-first, portrait, cozy weather-sorting puzzle built in Unity 6000.5.9f1 (Universal 2D). Tap a source cloud and then a compatible destination; matching top weather moves as a group. No level countdown or drag gesture is required.

![Cloudora feature art](StoreAssets/Play/cloudora-feature-1024x500.png)

## Product and architecture

- Fifteen authored onboarding levels lead into seed-reproducible generated puzzles. The generator reverse-shuffles solved boards and records a legal solution, then validates invariants before accepting a level.
- Cloud capacity progresses from four to seven slots. Thirteen world definitions span levels 1–400, with restoration milestones, new weather, modifiers, and difficulty waves.
- Rule logic lives in pure C# state and move classes; Unity cloud views, animations, responsive board layout, HUD, world screen, and settings sit around that core. Modifier rules are registered independently of the views.
- Local JSON save data includes progress, life regeneration, tutorial flags, settings and boosters, with schema migration and backup recovery. No account, backend, or cloud-save service is required for the puzzle loop.
- Analytics and advertising are behind interfaces. This repository binds fake ads and a credential-free analytics fallback. Real AdMob is not enabled; no public release is authorized.

## Run and test

1. Open the project in Unity 6000.5.9f1 and load `Assets/_Cloudora/Scenes/02_Gameplay.unity`.
2. Enter Play Mode to inspect the initial menu, one-finger puzzle loop, world and settings surfaces.
3. Run EditMode tests in Unity Test Runner. `Cloudora/Validate 10,000 Levels` runs the deterministic content stress check.
4. Use `Cloudora/Build/Android Development APK` for an internal build. See [Android build and device QA](Docs/ANDROID_BUILD.md) for the ASCII-only path requirement and open physical-device checks.

The latest fresh EditMode result passed 26/26 tests after package cleanup; a separate content stress check passed 10,000/10,000 generated-level validations. These checks do not substitute for Android PlayMode/device smoke tests or prove real-world difficulty quality. See [phase progress](Docs/PROJECT_PROGRESS.md) and the [release checklist](Docs/RELEASE_CHECKLIST.md) for exact status and blockers.

## Store/portfolio package

The [store-art pack](StoreAssets/README.md) contains an original icon and feature illustration, plus exact-size PNG candidates; [English and Turkish store copy](StoreAssets/STORE_COPY.md) is a draft. These are not gameplay screenshots. Authentic Android screenshots, release signing, privacy-policy approval, and publication remain separate owner-controlled gates. See the [privacy release review](Docs/PRIVACY_RELEASE_REVIEW.md).
