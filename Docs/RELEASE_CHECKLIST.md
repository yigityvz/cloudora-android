# Release-candidate checklist

Version candidate: `0.1.0` / Android version code `1`; package `com.yigityvz.cloudora`.

## Automated evidence

- [x] EditMode regression suite: fresh Phase 15 XML passed 26/26 on 2026-09-21 after package cleanup.
- [x] 10,000 generated level seeds: invariant and known-solution validation passed.
- [x] Final-source EditMode tests pass after QA fixes.
- [ ] PlayMode scene/input smoke tests pass after QA fixes.
- [x] Android development APK built with IL2CPP and ARM64 on 2026-09-21 (45,253,539 bytes; Unity BuildPlayer success, zero build errors).
- [x] Internal AAB built on 2026-09-21 (34,669,647 bytes; zero build errors) and inspected: package `com.yigityvz.cloudora`, version `0.1.0` / code 1, min/target API 26/36, ARM64-only native libraries, and Adaptive icon resources.

## Device acceptance

- [ ] Fresh install, first puzzle, restart, undo, completion, and Continue on an Android device.
- [ ] Portrait layout on narrow/tall screens, safe area/cutout, one-thumb targets and readable weather patterns.
- [ ] Pause/resume during transfer, no-moves state, life regeneration, save migration and corrupt-save recovery.
- [ ] Levels 1, 15, 16, 100, and 400: progression, modifiers, world/restoration feedback and solvability smoke.
- [ ] Ten repeated sessions with no blocking logcat exceptions, significant memory leak, or unacceptable frame pacing.
- [ ] Genuine Android screenshots captured and approved for store listing.

## Owner-controlled release gates

- [ ] Release signing key and secure signing process supplied outside Git; `jarsigner` confirms the current AAB uses the Android Debug certificate and is not publishable.
- [ ] Firebase SDK/project configuration and analytics consent/data-declaration decisions reviewed. Current build has a credential-free fallback logger.
- [ ] Privacy policy URL, support contact, legal review, and store data-safety answers supplied/approved.
- [ ] Store copy, icon, feature graphic, screenshots, content rating and target audience approved in the console.
- [ ] Explicit owner approval before enabling real AdMob or publishing any build.

No checklist item is implicitly cleared by producing a local artifact. If the owner has not approved the release and privacy decisions, the candidate remains internal-only.
