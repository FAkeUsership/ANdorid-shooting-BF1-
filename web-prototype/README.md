# IRONFRONT 1917 — "The Great War · The Dead Walk"

A Battlefield-1-flavoured FPS rendered in Walking-Zombie-2-style low-poly graphics.
WWI trench warfare against the risen dead. Built with Three.js (vendored locally —
**zero external requests**, works offline). Personal test project.

## Run it

Any static server works:

```
cd ironfront
python3 -m http.server 8000
# open http://localhost:8000
```

Desktop: **WASD** move · **SHIFT** sprint · **SPACE** jump · **LMB** fire · **RMB** aim ·
**R** reload · **G** grenade · **1/2/3** or **Q** weapons · **ESC** pause.
Mobile: left stick move, right side drag = aim & fire, on-screen buttons for the rest.

Settings (quality / sensitivity / invert / sound / FPS counter) and mission progress
are saved in `localStorage`.

## What's inside

```
index.html          UI shell: menus, HUD, touch controls, story overlays
css/ui.css          all styling
js/vendor/three.min.js   Three.js r128 (local, no CDN)
js/config.js        ★ ALL GAME DATA — missions, story, weapons, enemy stats
js/audio.js         100% synthesized WebAudio SFX (shots, moans, artillery, wind)
js/world.js         terrain/trenches/craters/church/sky (instanced props)
js/characters.js    low-poly humanoid builder + painted faces
js/weapons.js       viewmodels, ammo, reload, ADS, recoil
js/enemies.js       AI state machines (rise → chase → lunge / shoot), pooling
js/player.js        FPS controller, touch controls, damage/regen
js/game.js          loop, combat resolution, particles, phases, HUD, menus
```

## Extending it

### 1. Add a mission + story
Open `js/config.js` → `MISSIONS` array and push a new object:

```js
{
  id: 'm3', name: 'THE GAS CELLS', chapter: 'MISSION III — ...',
  desc: 'Short line for the mission select screen.',
  difficulty: '★★★', startX: 0, startZ: 46, startYaw: 0,
  objectiveShort: 'DESTROY THE GAS CELLS',
  requires: 'm2',                      // optional: unlocks after completing m2
  story: [                             // typewriter pages, as many as you like
    { chapter: 'CHAPTER NAME', text: 'Line one.\nLine two.' },
  ],
  phases: [
    { type: 'reach',  target: { x: -30, z: -38, r: 8 }, label: 'REACH THE FARMHOUSE' },
    { type: 'defend', time: 40, spawnEvery: 2.5, list: ['zombie','runner'], label: 'HOLD' },
    { type: 'waves',  spawnZone: 'cratersFront',
      waves: [ { list: [['zombie',8],['soldier',2]], interval: 0.9 } ] },
  ],
  completeText: '...', unlocksText: '...',
}
```

Phase types already implemented: `waves`, `reach` (floating diamond beacon appears),
`defend`. They chain in order, so you can script multi-stage operations.

### 2. Add a weapon
`WEAPONS` in `config.js`: copy an entry, tweak `dmg/mag/rate/spread/kick`, give it a
`slot`, then build its viewmodel in `js/weapons.js` (`_build_<model>()` — boxes and
cylinders only, keep it chunky). `unlock: 'm1'` gates it behind a mission.

### 3. Add an enemy
`ENEMY_TYPES` in `config.js`: set hp / speed / dmg / `melee:false` for ranged.
Models live in `js/characters.js` (`Characters.build(type)`) — reuse an existing
model key or write a new builder (return `{group, mats, parts, muzzle}`).

### 4. Script story events during gameplay
The loop exposes everything on `window.IRONFRONT` — e.g. from the browser console:

```js
IRONFRONT.Enemies.spawn('runner', 10, 20);   // spawn mid-game
IRONFRONT.addMission({...});                  // hot-add a mission
IRONFRONT.Game.explode(Player.pos.clone());   // boom
```

## Performance notes (why it runs smooth on Android)

- No textures, no external assets: flat-shaded vertex-colored geometry everywhere.
- All repeated props (sandbags, posts, rocks, planks, grass) are `InstancedMesh`
  → the whole trench is a handful of draw calls.
- Particle pools, tracer pools, blood-decal pools and per-type enemy model pools:
  zero GC churn during combat.
- `AUTO` quality drops pixel ratio, shadows, particle counts and fog range on
  phones (settings → quality to force it).
- Enemy cap + spawn intervals keep the AI budget flat.

## License note

Personal test project; Three.js is MIT (see js/vendor/three.min.js header).
