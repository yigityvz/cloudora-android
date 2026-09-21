# Google Play submission runbook

Cloudora is technically prepared for a Play-signed AAB, but publication remains owner-controlled. The build does not enable real AdMob, upload an artifact, or create a Play Console release.

## Build profiles

- `./Build-Android.ps1 -Mode dev`: development APK with fake rewards for local QA.
- `./Build-Android.ps1 -Mode release`: internal AAB, network services disabled, local debug signing; never upload this artifact.
- `./Build-Android.ps1 -Mode play`: production AAB. This profile fails closed until the public release metadata and upload signing variables below are valid.

Before a Play build, set `privacyPolicyUrl` and `supportEmail` in `Assets/Resources/CloudoraReleaseConfig.json`. Keep the keystore and secrets outside Git, then provide these process-scoped variables:

```powershell
$env:CLOUDORA_KEYSTORE_PATH = 'C:\secure\cloudora-upload.keystore'
$env:CLOUDORA_KEYSTORE_PASS = '<secret>'
$env:CLOUDORA_KEY_ALIAS = 'cloudora-upload'
$env:CLOUDORA_KEY_ALIAS_PASS = '<secret>'
$env:CLOUDORA_VERSION_CODE = '2'
./Build-Android.ps1 -Mode play -ProjectPath C:\Dev\Cloudora-codex
```

The build validates the HTTPS policy URL, support address, keystore path, credentials, and positive version code before producing `Builds/Android/Cloudora-0.1.0-play.aab`. Never paste secret values into logs, issues, commits, or Play listing notes.

## Current binary policy

- Package: `com.yigityvz.cloudora`; version name `0.1.0`; min API 26; target API 36; ARM64; IL2CPP.
- Network and live advertising are disabled. The Android postprocessor removes `android.permission.INTERNET` unless `CLOUDORA_ENABLE_NETWORK=1` is deliberately supplied for a separately reviewed build.
- Public native symbols are requested for AAB builds. Upload the generated symbols archive in Play Console when available.
- 16 KB native page alignment must be rechecked for the final Play-signed AAB with Bundletool.
- Increment `CLOUDORA_VERSION_CODE` for every Play Console upload.

## Console work that cannot be automated here

1. Enrol the upload key in Play App Signing and upload only the `-play.aab` artifact.
2. Complete App content: privacy policy, ads declaration (`No` for this build), app access (no login), target audience, content rating, and Data safety from the final binary and approved policy.
3. Add the approved support contact, store copy, icon, feature graphic, and genuine Android gameplay screenshots.
4. Run internal testing and the physical-device matrix in `Docs/RELEASE_CHECKLIST.md`. If the developer account is a newly created personal account, satisfy any closed-test eligibility requirement shown by Play Console.
5. Review pre-launch reports, Android vitals, warnings, country/pricing choices, and release notes.
6. Obtain explicit owner approval before production rollout or enabling real AdMob.

Official references: [target API requirements](https://developer.android.com/google/play/requirements/target-sdk), [16 KB page-size support](https://developer.android.com/guide/practices/page-sizes), [Play App Signing](https://support.google.com/googleplay/android-developer/answer/9842756), and [app review declarations](https://support.google.com/googleplay/android-developer/answer/9859455).
