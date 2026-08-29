# Assets

Folder map for game content (wired via `assets.srcDirs = ['../assets']` on Android,
and loaded from `assets/` on desktop):

```
assets/
├── models/     .g3dj / .glb  — low-poly props & characters (S3+)
├── textures/   .png/.ktx     — diffuse/atlas maps (S9 pipeline)
├── audio/      .ogg          — sfx & music (S6)
├── ui/         .png + skin   — Scene2D atlas (S7)
└── data/       .json         — optional data overrides for Config.kt
```

## Asset policy (non-negotiable)

1. Only **original** assets, assets **you created**, or assets under a license that
   explicitly permits modification + redistribution (CC0, CC-BY, purchased with
   redistribution rights, or a written agreement from the rights holder).
2. A third-party pack accompanied only by a *claim* of permission is **not**
   uploaded here. Written proof (license file from the official distribution,
   contract, or an official public statement) must be reviewed first.
3. Ripped/recolor-modified assets from commercial games are never accepted —
   "slight changes" do not remove copyright and publishing them creates legal
   liability for this repository.

Until cleared assets exist, the game renders its built-in procedural
low-poly graphics (see `core/.../WorldBuilder.kt`), so the project always builds
and plays with zero external content.
