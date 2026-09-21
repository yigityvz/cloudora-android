# Release-candidate checklist

Version candidate: `0.1.0` / Android version code `1`; package `com.yigityvz.cloudora`.

## Automated evidence

- [x] EditMode regression suite passed 31/31 on 2026-09-21 after Google Play readiness hardening (including 2,000 generated levels).
- [x] 10,000 generated level seeds: invariant and known-solution validation passed.
- [x] Final-source EditMode tests pass after QA fixes.
- [ ] PlayMode scene/input smoke tests pass after QA fixes.
- [x] Android development APK built with IL2CPP and ARM64 on 2026-09-21 (45,253,539 bytes; Unity BuildPlayer success, zero build errors).
- [x] Post-hardening internal AAB built on 2026-09-21 (34,686,463 bytes; zero errors/warnings) with a 20,726,505-byte public-symbol archive. Bundletool inspection confirmed package `com.yigityvz.cloudora`, version `0.1.0` / code 1, min/target API 26/36, no INTERNET permission, automatic install location, and 16 KB native-library alignment.
- [x] Play profile fails closed when public policy/contact metadata or external upload signing credentials are missing.

## Device acceptance

- [ ] Fresh install, first puzzle, restart, undo, completion, and Continue on an Android device.
- [ ] Portrait layout on narrow/tall screens, safe area/cutout, one-thumb targets and readable weather patterns.
- [ ] Pause/resume during transfer, no-moves state, life regeneration, save migration and corrupt-save recovery.
- [ ] Levels 1, 15, 16, 100, and 400: progression, modifiers, world/restoration feedback and solvability smoke.
- [ ] Ten repeated sessions with no blocking logcat exceptions, significant memory leak, or unacceptable frame pacing.
- [ ] Genuine Android screenshots captured and approved for store listing.

## Owner-controlled release gates

- [ ] Release upload key and secure signing variables supplied outside Git; the `play` profile rejects missing credentials. The current internal AAB uses the Android Debug certificate and is not publishable.
- [ ] Firebase SDK/project configuration and analytics consent/data-declaration decisions reviewed. Current build has a credential-free fallback logger.
- [ ] HTTPS privacy policy URL and support contact added to `Assets/Resources/CloudoraReleaseConfig.json`; legal review and store Data safety answers supplied/approved.
- [ ] Store copy, icon, feature graphic, screenshots, content rating and target audience approved in the console.
- [ ] Explicit owner approval before enabling real AdMob or publishing any build.

No checklist item is implicitly cleared by producing a local artifact. If the owner has not approved the release and privacy decisions, the candidate remains internal-only.
