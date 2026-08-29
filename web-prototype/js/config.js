/* ============================================================
   IRONFRONT 1917 — config.js
   ------------------------------------------------------------
   ALL GAME DATA LIVES HERE. This is the file you edit to add
   content without touching engine code:
     • CONFIG      – tunable gameplay/graphics numbers
     • WEAPONS     – weapon stats (models built in weapons.js)
     • ENEMY_TYPES – enemy stats (models built in characters.js)
     • MISSIONS    – missions, story pages, wave scripts.
                     Add a new object to this array and it will
                     appear in the mission select screen.
   ============================================================ */
'use strict';

/* ---------------- tiny math helpers (used everywhere) ---------------- */
window.U = {
  clamp: (v, a, b) => v < a ? a : (v > b ? b : v),
  lerp:  (a, b, t) => a + (b - a) * t,
  rand:  (a, b) => a + Math.random() * (b - a),
  randi: (a, b) => Math.floor(a + Math.random() * (b - a + 1)),
  pick:  (arr) => arr[Math.floor(Math.random() * arr.length)],
  dist2: (ax, az, bx, bz) => { const dx = ax - bx, dz = az - bz; return dx * dx + dz * dz; },
  smooth: (t) => t * t * (3 - 2 * t),
  dampCur: (cur, target, speed, dt) => U.lerp(cur, target, 1 - Math.exp(-speed * dt)),
};

/* ---------------- global tuning ---------------- */
window.CONFIG = {
  mapRadius: 150,            // playable radius (soft border)
  player: {
    walkSpeed: 5.0, sprintSpeed: 7.9, accel: 42, friction: 11,
    jumpVel: 7.2, gravity: 21, eyeHeight: 1.62,
    maxHp: 100, regenDelay: 6, regenRate: 14,
    maxGrenades: 3,
  },
  fov: { hip: 75, sprint: 82, adsRifle: 50, adsPistol: 55, adsSmg: 58 },
  enemyCap: { low: 12, high: 18 },        // max simultaneous enemies
  particleScale: { low: 0.55, high: 1.0 },// particle budgets per quality
  fogFar: { low: 150, high: 230 },
  grenadeFuse: 2.3,
  intermissionTime: 11,       // seconds between waves
  headshotMult: 2.2,
};

/* ---------------- weapons ----------------
   model: key into Weapons.viewmodels (see weapons.js)
   auto : hold to keep firing?
   Kick into CONFIG.fov for ADS.                                 */
window.WEAPONS = {
  rifle: {
    id: 'rifle', name: 'G-17 BOLT RIFLE', slot: 1, model: 'rifle',
    dmg: 46, mag: 5, reserve: 45, rate: 1.05, reload: 2.7,
    spread: 0.0035, adsSpread: 0.0004, adsFov: 'adsRifle',
    auto: false, kick: 0.055, shake: 0.12, sound: 'rifle', unlockedAtStart: true,
    desc: 'Heavy bolt-action rifle. Devastating at range.',
  },
  pistol: {
    id: 'pistol', name: 'M1912 PISTOL', slot: 2, model: 'pistol',
    dmg: 21, mag: 8, reserve: 64, rate: 0.17, reload: 1.7,
    spread: 0.014, adsSpread: 0.005, adsFov: 'adsPistol',
    auto: false, kick: 0.02, shake: 0.05, sound: 'pistol', unlockedAtStart: true,
    desc: 'Reliable sidearm. Fast handling, close work.',
  },
  smg: {
    id: 'smg', name: 'HELLRIEGEL SMG', slot: 3, model: 'smg',
    dmg: 15, mag: 32, reserve: 128, rate: 0.092, reload: 2.4,
    spread: 0.028, adsSpread: 0.012, adsFov: 'adsSmg',
    auto: true, kick: 0.014, shake: 0.04, sound: 'smg', unlockedAtStart: false,
    unlock: 'm1',  // unlocks when mission 'm1' is completed
    desc: 'Experimental drum-fed submachine gun. Shreds at close range.',
  },
};

/* ---------------- enemy types ----------------
   model: key into Characters.build (characters.js)             */
window.ENEMY_TYPES = {
  zombie: {
    id: 'zombie', name: 'RISEN INFANTRY', model: 'zombie',
    hp: 42, speedMin: 2.1, speedMax: 3.1, dmg: 16, attackRange: 1.8,
    attackRate: 1.15, score: 10, melee: true, riseFromGround: true,
  },
  runner: {
    id: 'runner', name: 'RISEN RUNNER', model: 'runner',
    hp: 26, speedMin: 4.3, speedMax: 5.2, dmg: 12, attackRange: 1.7,
    attackRate: 0.85, score: 15, melee: true, riseFromGround: true,
  },
  soldier: {
    id: 'soldier', name: 'GREY SOLDIER', model: 'soldier',
    hp: 75, speedMin: 2.0, speedMax: 2.6, dmg: 9, attackRange: 26,
    attackRate: 1.9, score: 25, melee: false, riseFromGround: false,
    accuracy: 0.5, bulletSpread: 0.075,
  },
};

/* ============================================================
   MISSIONS — ADD NEW MISSIONS HERE.
   ------------------------------------------------------------
   phases run in order. Supported phase types:
     {type:'waves',  waves:[ {list:[['zombie',6],['soldier',2]], interval:1.0}, ... ]}
     {type:'reach',  target:{x,z,r}, label:'REACH THE ...'}
     {type:'defend', time:45, spawnEvery:2.6, list:[...types to mix...]}
   story: array of pages shown before deployment (typewriter).
   spawnZones: 'craters' | 'ridge' — where enemies appear.
   ============================================================ */
window.MISSIONS = [
  {
    id: 'm1', name: "DEVIL'S DAWN", chapter: 'MISSION I — THE SOMME, 1917',
    desc: 'Hold the forward trench against the rising dead.',
    difficulty: '★★☆', startX: 0, startZ: 46, startYaw: 0,
    objectiveShort: 'HOLD THE TRENCH LINE',
    story: [
      { chapter: 'PROLOGUE — OCTOBER, 1917', text:
'Three days the guns have not stopped.\n' +
'Three days of mud, and rain, and wire.\n\n' +
'Then, at dawn, the shelling ceased —\n' +
'and something worse began.' },
      { chapter: 'PROLOGUE', text:
'A green fog crawls across No Man\'s Land.\n' +
'The wires hiss. The craters stir.\n\n' +
'The men we lost yesterday are standing up.' },
      { chapter: 'ORDERS', text:
'You are the last rifleman of 3rd Squad.\n\n' +
'HOLD THE TRENCH. SURVIVE THE WAVES.\n' +
'Whatever used to wear our uniforms —\n' +
'it is no longer one of us.' },
    ],
    phases: [
      { type: 'waves', spawnZone: 'cratersFront', waves: [
        { list: [['zombie', 7]], interval: 1.1 },
        { list: [['zombie', 8], ['runner', 3]], interval: 0.95 },
        { list: [['zombie', 9], ['runner', 4], ['soldier', 2]], interval: 0.85 },
      ]},
    ],
    completeText: 'THE TRENCH HELD. FOR NOW.',
    unlocksText: 'HELLRIEGEL SMG UNLOCKED — NEXT OPERATION AVAILABLE',
  },
  {
    id: 'm2', name: "NO MAN'S LAND", chapter: 'MISSION II — THE GREY CHURCH',
    desc: 'Cross the craters to the ruined church. Survive what follows.',
    difficulty: '★★★', startX: 0, startZ: 46, startYaw: 0,
    objectiveShort: 'REACH THE RUINED CHURCH',
    requires: 'm1',
    story: [
      { chapter: 'MISSION II', text:
'The dead do not hold ground.\nThey walk. Endlessly, they walk.\n\n' +
'Their trail leads to the ruined church\n' +
'at the far edge of No Man\'s Land.' },
      { chapter: 'ORDERS', text:
'Cross the open ground. Reach the church.\n' +
'Find what is calling them — and hold it\n' +
'until the light fails.\n\n' +
'Move fast. The land itself will claw at you.' },
    ],
    phases: [
      { type: 'reach', target: { x: -85, z: -85, r: 9 }, label: 'REACH THE RUINED CHURCH' },
      { type: 'defend', time: 50, spawnEvery: 2.4, list: ['zombie', 'zombie', 'runner', 'soldier'], label: 'HOLD THE CHURCH UNTIL DUSK' },
    ],
    completeText: 'THE GUNS FELL SILENT AT DUSK.',
    unlocksText: 'THE FRONT IS QUIET… FOR NOW. MORE OPERATIONS SOON.',
  },
];

/* ---------------- save helpers ---------------- */
window.SaveData = {
  key: 'if17_save',
  load() {
    try { return Object.assign({ completed: [], settings: {} }, JSON.parse(localStorage.getItem(this.key) || '{}')); }
    catch (e) { return { completed: [], settings: {} }; }
  },
  store(data) { try { localStorage.setItem(this.key, JSON.stringify(data)); } catch (e) {} },
};
