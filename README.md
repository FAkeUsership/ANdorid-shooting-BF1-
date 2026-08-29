# IRONFRONT 1917 — Android WWI FPS (open source)

Battlefield-1-flavoured wave-combat FPS in Walking-Zombie-2-style low-poly graphics.
Kotlin + libGDX. Android primary target, desktop module for fast PC iteration.

```
core/     platform-free game code (Kotlin)      ← everything gameplay lives here
desktop/  LWJGL3 launcher   (gradle :desktop:run)
android/  Android launcher  (open in Android Studio; needs SDK)
assets/   content pipeline  (see assets/README.md — asset policy!)
web-prototype/  finished HTML/Three.js reference build of the same design
docs/     STRUCTURE.md — the section-by-section build plan
```

## Build

Desktop (no Android SDK needed):
```
gradle :desktop:run
```
Android: open the folder in Android Studio (it creates `local.properties` with your
SDK path, which activates the `:android` module), then Run `AndroidLauncher`.

## Status

Section **S0/S1** delivered: multi-module Gradle build, data-driven config
(missions / story / weapons / enemies already ported from the web prototype),
FPS camera + look/move input, placeholder trench world. See `docs/STRUCTURE.md`
for the roadmap (S2 world → S9 release).

## Legal / asset policy

- Code: MIT (see LICENSE).
- Assets: original or properly licensed content only. Ripped commercial assets are
  never committed — see `assets/README.md`.
- Never commit credentials (tokens, keystores, signing configs).
