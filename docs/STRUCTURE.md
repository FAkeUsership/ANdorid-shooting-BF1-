# IRONFRONT 1917 — Build Sections (batch plan)

Work is delivered in sections so each batch stays reviewable.
Status: ✅ done · 🚧 skeleton started · ⬜ not started

| # | Section | Contents | Status |
|---|---------|----------|--------|
| S0 | Repo & build skeleton | Gradle multi-module (core / desktop / android), docs, license, asset-policy, web prototype reference | ✅ |
| S1 | Core engine | State machine, FPS camera, mouse/touch look, WASD move, ModelBatch rendering | 🚧 skeleton in |
| S2 | World generation | Displaced vertex-colored terrain, trench carving, craters, merged sandbags/posts/rocks/grass, ruined houses + church, clouds, hills (≈3 draw calls; distance fog deferred to S9) | ✅ |
| S3 | Characters | Low-poly humanoid rigs (zombie/runner/soldier), procedural walk/attack/death animation | ⬜ |
| S4 | Weapons & combat | Hitscan raycasts, tracers, recoil/ADS/reload, grenades, explosions, pooled particles | ⬜ |
| S5 | Enemy AI & waves | rise→chase→lunge / ranged fire states, steering, wave/phase scheduler from Config | ⬜ |
| S6 | Audio | Synthesized SFX (or wired licensed audio once assets are cleared) | ⬜ |
| S7 | UI & story | Scene2D HUD (compass, health, ammo, killfeed), mission select, typewriter story pages | ⬜ |
| S8 | Save & progression | SharedPreferences progress/settings, unlocks | ⬜ |
| S9 | Perf & release | Texture/atlas pipeline for licensed assets, pooling audit, release signing, store listing | ⬜ |

The `web-prototype/` folder contains the finished HTML/Three.js version of this
design — it is the gameplay/visual reference the Kotlin port follows (same Config
values already mirrored in `core/.../Config.kt`).
