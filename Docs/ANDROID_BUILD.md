# Android Build and QA

## Reproducible builds

Use Unity `6000.5.9f1` with Android Build Support installed.

Unity's Android tools reject non-ASCII project paths. If the checkout path contains characters such as `ğ`, open the same worktree through an ASCII-only junction (for example `C:\Dev\Cloudora-codex`) before building. The APK/AAB output still lands in the worktree's ignored `Builds/Android` directory.

- Development APK: `Cloudora > Build > Android Development APK`
- Release-candidate AAB: `Cloudora > Build > Android Release AAB`
- Reproducible Windows batch: `./Build-Android.ps1 -Mode dev -ProjectPath C:\Dev\Cloudora-codex` (or `-Mode release`)

The build tool enforces portrait, `com.yigityvz.cloudora`, version `0.1.0` / code 1, min API 26, target API 36, IL2CPP, ARM64, and the real gameplay scene. Release output uses the local debug signing path until the owner explicitly supplies private release signing outside Git.

On this Windows host, the Java 17 Gradle client cannot open its NIO pipe in the Unicode user-profile temporary path. The checked-in PowerShell wrapper sets an ASCII `JDK_JAVA_OPTIONS` pipe directory before launching Unity. The Unity editor menu can still be used when the editor itself was launched with that environment setting. Do not set machine-wide Java properties or change the bundled JDK.

## Device QA matrix

- 16:9, 18:9, 19.5:9, 20:9 portrait; camera cutout and gesture navigation
- Fresh install, upgrade over previous save, corrupted save recovery
- 30-minute life regeneration across pause, process kill, timezone change, and offline use
- Background/foreground during a move, level complete, fake reward, and save
- One-thumb touch targets, sound/haptic toggles, color plus pattern recognition
- Ten repeated sessions; monitor Console/logcat, memory, load time, and stable 60 FPS target

No physical Android device was connected during the automated Phase 14 run; device-only checks remain explicitly open in the release checklist.
