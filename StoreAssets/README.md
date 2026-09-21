# Store visual pack

`Source/` holds the original generated Cloudora icon and feature illustration. `Play/` contains deterministic, center-cropped PNG exports at 512×512 and 1024×500, plus a padded transparent foreground and solid sky-blue background for Android adaptive icons. Re-export with `Export-PlayAssets.ps1` after changing a source image. Do not overwrite an existing asset casually; review the generated output at actual store size.

The graphics are original Cloudora-branded art. The icon and banner were generated with the built-in image-generation tool using the Cloudora sky palette, then resized/cropped by the checked-in export script. The two final prompts are recorded below for reproducibility. No third-party licensed art was introduced.

- Icon prompt: original centered cloud-and-weather emblem for a cozy puzzle, integrating white cloud, sun, raindrop and snowflake; soft vector-like finish; Cloudora sky palette; no text, watermark, hard outline, or mockup.
- Feature prompt: wide calm sky-restoration illustration with clouds, sun, rain, snow and wind; Cloudora palette, rounded soft shapes, generous left negative space; no text, watermark, UI, or mockup.

`Play/` is a store-art candidate, not evidence of gameplay. Authentic screenshots must be captured from a verified Android build during device QA. Do not submit mock gameplay screenshots or treat the visual pack alone as release approval.
