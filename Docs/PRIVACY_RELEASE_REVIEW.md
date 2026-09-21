# Privacy and legal release review (not legal advice)

## Current implementation inventory

- Local save: progress, highest level, lives and UTC regeneration time, tutorial flags, sound/haptic settings, booster balances, and generated seed metadata in `Application.persistentDataPath`. The game does not implement accounts or cloud save.
- Analytics interface: gameplay event names and bounded gameplay parameters are routed to a local debug sink. Firebase discovery is optional and no Firebase project file or credential is included. The privacy guard rejects obvious personal-data parameter keys; this is a code safeguard, not a full compliance guarantee.
- Advertising: development builds use `FakeAdService`; non-development builds bind the unavailable `AdMobAdService` stub. No live ad unit or production request is configured, so release builds cannot grant unlimited fake rewards.
- Network permission: release manifests remove `android.permission.INTERNET` while network services are disabled. Re-enabling it requires the explicit `CLOUDORA_ENABLE_NETWORK` build flag and a fresh privacy/data-safety review.
- Unused Unity Analytics, IAP, and Multiplayer Center template packages were removed from direct dependencies. Engine modules and any remaining transitive services still require a final binary/runtime network audit; the current code-level fake-service statement alone is insufficient for a legal privacy claim.

## Required decisions and evidence before publication

1. Audit the final built binary and runtime network traffic for Unity/Firebase/Google SDK collection, including device identifiers, diagnostics, consent, child-directed flags, and regional obligations.
2. Decide the legal publisher identity, support email, retention/deletion contact, and hosted privacy-policy URL. Do not publish a placeholder policy.
3. Fill the actual store data-safety and content-rating forms from the final binary and approved policy, not from this draft.
4. If Firebase is enabled later, verify initialization, consent strategy, parameter transmission, retention, and event dictionary against the final SDK configuration.
5. If real ads are ever approved, separately review consent, age/target-audience settings, rewarded-ad disclosures, and Google Play policy obligations before switching the service binding.
6. Keep signing keys, Firebase configuration, ad-unit IDs, and account credentials out of Git.

No public privacy promise or app publication is authorized by this document. Owner/legal review and explicit publication approval remain required.
