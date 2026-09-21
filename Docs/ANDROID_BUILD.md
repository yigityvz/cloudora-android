# Android Build and QA

## Reproducible builds

Use Unity `6000.5.9f1` with Android Build Support installed.

Unity's Android tools reject non-ASCII project paths. If the checkout path contains characters such as `ğ`, open the same worktree through an ASCII-only junction (for example `C:\Dev\Cloudora-codex`) before building. The APK/AAB output still lands in the worktree's ignored `Builds/Android` directory.

- Development APK: `Cloudora > Build > Android Development APK`
- Release-candidate AAB: `Cloudora > Build > Android Release AAB`
- Play-signed AAB: `Cloudora > Build > Android Play AAB`
- Reproducible Windows batch: `./Build-Android.ps1 -Mode dev -ProjectPath C:\Dev\Cloudora-codex` (or `-Mode release` / `-Mode play`)

On this Windows host, the wrapper scopes `TEMP` and `TMP` to the ASCII project path so Java 17 can create Gradle/NIO sockets reliably under a Unicode user profile. The original environment is restored afterward. It does not alter signing or publish anything.

The build tool enforces portrait, `com.yigityvz.cloudora`, version `0.1.0`, min API 26, target API 36, IL2CPP, ARM64, and the real gameplay scene. Internal release output uses local debug signing. The Play profile requires an external upload keystore, an incrementable `CLOUDORA_VERSION_CODE`, and valid public release contacts; see `GOOGLE_PLAY_SUBMISSION.md`.

Latest internal artifacts (2026-09-21): the post-hardening `Cloudora-0.1.0.aab` is 34,686,463 bytes and its public-symbol archive is 20,726,505 bytes. Unity BuildPlayer completed with zero errors and zero warnings. Bundletool confirmed package/version metadata, API 26/36, automatic install location, no INTERNET permission, and 16 KB native-library alignment; `jarsigner` identifies the AAB certificate as Android Debug, so it must not be uploaded as a production release.

On this Windows host, the Java 17 Gradle client cannot open its NIO pipe in the Unicode user-profile temporary path. The checked-in PowerShell wrapper scopes `TEMP` and `TMP` to the ASCII project path before launching Unity. The Unity editor menu can still be used when the editor itself inherited an ASCII temporary path. Do not change machine-wide Java properties or the bundled JDK.

## Device QA matrix

- 16:9, 18:9, 19.5:9, 20:9 portrait; camera cutout and gesture navigation
- Fresh install, upgrade over previous save, corrupted save recovery
- 30-minute life regeneration across pause, process kill, timezone change, and offline use
- Background/foreground during a move, level complete, fake reward, and save
- One-thumb touch targets, sound/haptic toggles, color plus pattern recognition
- Ten repeated sessions; monitor Console/logcat, memory, load time, and stable 60 FPS target

No physical Android device was connected during the automated Phase 14 run; device-only checks remain explicitly open in the release checklist.
