# IRONFRONT 1917 — Build Sections (batch plan)

Work is delivered in sections so each batch stays reviewable.
Status: ✅ done · 🚧 skeleton started · ⬜ not started

| # | Section | Contents | Status |
|---|---------|----------|--------|
| S0 | Repo & build skeleton | Gradle multi-module (core / desktop / android), docs, license, asset-policy, web prototype reference | ✅ |
| S1 | Core engine | State machine, FPS camera, mouse/touch look, WASD move, ModelBatch rendering | 🚧 skeleton in |
| S2 | World generation | Displaced vertex-colored terrain, trench carving, craters, merged sandbags/posts/rocks/grass, ruined houses + church, clouds, hills (≈3 draw calls; distance fog deferred to S9) | ✅ |
| S3 | Characters | low-poly zombie/runner/soldier rigs, procedural walk/lunge/death anim | ✅ |
| S4 | Weapons & combat | hitscan + headshots, tracers, grenades/explosions, pooled particle FX, viewmodel gun | ✅ |
| S5 | Enemy AI & waves | rise→chase→lunge + ranged soldiers, steering/separation, 3-wave scheduler w/ intermissions | ✅ |
| S6 | Audio | runtime-synthesized WAV SFX (shots/hit/moan/boom/hurt/reload), zero shipped audio | ✅ |
| S7 | UI & story | menu/death/victory screens, HP/ammo/wave HUD, touch stick + FIRE/G/RLD/SWAP buttons, banners | ✅ |
| S8 | Save & progression | SharedPreferences progress/settings, unlocks | ⬜ |
| S9 | Perf & release | Texture/atlas pipeline for licensed assets, pooling audit, release signing, store listing | ⬜ |

The `web-prototype/` folder contains the finished HTML/Three.js version of this
design — it is the gameplay/visual reference the Kotlin port follows (same Config
values already mirrored in `core/.../Config.kt`).
