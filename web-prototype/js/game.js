/* ============================================================
   IRONFRONT 1917 — game.js
   Core orchestrator: renderer, main loop, combat resolution,
   particles/tracers/explosions, mission phase machine,
   waves, HUD, menus, save/progress.
   ============================================================ */
'use strict';

window.Game = {
  canvas: null, renderer: null, scene: null, camera: null,
  dirLight: null,
  state: 'menu',           // menu | story | playing | paused | dead | complete
  time: 0, frames: 0, fpsT: 0, fps: 0,
  settings: { quality: 'auto', sens: 1.0, invertY: false, sound: true, showFps: false },
  effQuality: 'high',
  save: null,
  mission: null, phaseIdx: 0,
  waveIdx: 0, wavePending: [], waveSpawnT: 0, intermission: 0,
  defendT: 0, trickleT: 0,
  grenadeCount: 3, grenades: [],   // grenades[] = live grenade entities
  stats: null,
  shake: 0, outOfBounds: false,
  lockBlocked: false,      // pointer lock unavailable (e.g. embedded iframe) → on-screen controls
  bannerT: 0,
  bombardT: 6,
  beacon: null,
  hitSndCooldown: 0,

  /* ================================================================
     BOOT
     ================================================================ */
  boot() {
    this.canvas = document.getElementById('game');
    this.renderer = new THREE.WebGLRenderer({ canvas: this.canvas, antialias: true, powerPreference: 'high-performance' });
    this.renderer.outputEncoding = THREE.sRGBEncoding;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;

    this.scene = new THREE.Scene();
    this.scene.background = new THREE.Color(0x9ba184);
    this.scene.fog = new THREE.Fog(0x9ba184, 18, 210);

    this.camera = new THREE.PerspectiveCamera(CONFIG.fov.hip, innerWidth / innerHeight, 0.08, 700);
    this.scene.add(this.camera);

    const hemi = new THREE.HemisphereLight(0xd6dcc0, 0x42392a, 0.85);
    this.scene.add(hemi);
    this.dirLight = new THREE.DirectionalLight(0xffe9c4, 0.95);
    this.dirLight.position.set(-70, 90, -50);
    this.dirLight.castShadow = true;
    this.dirLight.shadow.mapSize.set(1024, 1024);
    const sc = this.dirLight.shadow.camera;
    sc.left = -90; sc.right = 90; sc.top = 90; sc.bottom = -90; sc.near = 10; sc.far = 300;
    this.dirLight.shadow.bias = -0.0006;
    this.scene.add(this.dirLight);

    this.save = SaveData.load();
    Object.assign(this.settings, this.save.settings || {});
    this.applyQuality();

    World.build(this.scene, this.effQuality);
    this._initFX();

    Player.bindInput();
    UI.bind(this);
    this.showScreen('menu-main');
    this.buildMissionList();
    this.syncSettingsUI();

    addEventListener('resize', () => this.onResize());
    document.addEventListener('pointerlockchange', () => {
      if (document.pointerLockElement !== this.canvas && this.state === 'playing' && !this.lockBlocked) this.pause();
    });
    document.addEventListener('pointerlockerror', () => {
      this.lockBlocked = true;
      if (this.state === 'playing') UI.showTouchControls();
    });
    this.canvas.addEventListener('click', () => {
      if (this.state === 'playing' && !Player.isTouch && !this.lockBlocked) this.canvas.requestPointerLock();
    });

    this.clock = new THREE.Clock();
    this.loop();
  },

  onResize() {
    this.camera.aspect = innerWidth / innerHeight;
    this.camera.updateProjectionMatrix();
    this.renderer.setSize(innerWidth, innerHeight);
  },

  applyQuality() {
    const s = this.settings;
    if (s.quality === 'auto') {
      const lowMem = navigator.deviceMemory && navigator.deviceMemory <= 4;
      this.effQuality = (Player.isTouch || lowMem || screen.width < 820) ? 'low' : 'high';
    } else this.effQuality = s.quality;
    const high = this.effQuality === 'high';
    this.renderer.setPixelRatio(high ? Math.min(devicePixelRatio || 1, 2) : 1);
    this.renderer.setSize(innerWidth, innerHeight);
    this.renderer.shadowMap.enabled = high;
    this.dirLight.castShadow = high;
    this.scene.fog.far = CONFIG.fogFar[this.effQuality];
    if (this.scene) this.scene.traverse(o => { if (o.material) o.material.needsUpdate = true; });
    SFX.setEnabled(s.sound);
  },

  /* ================================================================
     FX SYSTEMS — pooled particles, tracers, decals
     ================================================================ */
  _initFX() {
    this.particles = { pools: {}, active: [] };
    const defs = {
      blood: { color: 0x8a1610, n: 70, geo: [0.16, 0.16], blend: THREE.NormalBlending, grav: 13, drag: 0.92 },
      dirt:  { color: 0x5c4832, n: 55, geo: [0.18, 0.18], blend: THREE.NormalBlending, grav: 15, drag: 0.93 },
      spark: { color: 0xffc06a, n: 30, geo: [0.14, 0.14], blend: THREE.AdditiveBlending, grav: 3, drag: 0.95 },
      smoke: { color: 0x84847a, n: 26, geo: [1, 1], blend: THREE.NormalBlending, grav: -0.5, drag: 0.96 },
      flash: { color: 0xffd9a0, n: 12, geo: [1, 1], blend: THREE.AdditiveBlending, grav: 0, drag: 0.9 },
    };
    const geoCache = {};
    for (const kind in defs) {
      const d = defs[kind];
      const key = d.geo[0] + 'x' + d.geo[1];
      if (!geoCache[key]) geoCache[key] = new THREE.PlaneGeometry(d.geo[0], d.geo[1]);
      const pool = [];
      for (let i = 0; i < d.n; i++) {
        const m = new THREE.Mesh(geoCache[key], new THREE.MeshBasicMaterial({
          color: d.color, transparent: true, depthWrite: false, blending: d.blend,
        }));
        m.visible = false;
        this.scene.add(m);
        pool.push({ mesh: m, on: false, life: 0, maxLife: 1, vel: new THREE.Vector3(), grav: d.grav, drag: d.drag, grow: 0 });
      }
      this.particles.pools[kind] = pool;
    }
    this.particles.defs = defs;

    // tracers
    this.tracers = [];
    const tGeo = new THREE.BoxGeometry(0.022, 0.022, 1);
    for (let i = 0; i < 26; i++) {
      const m = new THREE.Mesh(tGeo, new THREE.MeshBasicMaterial({
        color: 0xffd27f, transparent: true, depthWrite: false, blending: THREE.AdditiveBlending,
      }));
      m.visible = false;
      this.scene.add(m);
      this.tracers.push({ mesh: m, on: false, life: 0, maxLife: 0.08 });
    }

    // blood pool decals
    this.decals = [];
    const dGeo = new THREE.CircleGeometry(0.55, 7);
    for (let i = 0; i < 22; i++) {
      const m = new THREE.Mesh(dGeo, new THREE.MeshBasicMaterial({ color: 0x560f0b, transparent: true, depthWrite: false }));
      m.rotation.x = -Math.PI / 2;
      m.visible = false;
      this.scene.add(m);
      this.decals.push({ mesh: m, on: false, life: 0 });
    }

    this.grenades = [];
    this.grenadeGeo = new THREE.IcosahedronGeometry(0.13, 0);
    this.grenadeMat = new THREE.MeshLambertMaterial({ color: 0x3a4432, flatShading: true });
  },

  spawnParticles(kind, x, y, z, n, power, upBias) {
    const pool = this.particles.pools[kind];
    n = Math.max(1, Math.round(n * CONFIG.particleScale[this.effQuality]));
    for (let i = 0; i < n; i++) {
      const p = pool.find(q => !q.on);
      if (!p) return;
      p.on = true;
      p.mesh.visible = true;
      p.mesh.position.set(x, y, z);
      p.vel.set(U.rand(-1, 1), U.rand(-1, 1) + (upBias || 0), U.rand(-1, 1)).normalize()
        .multiplyScalar(U.rand(0.3, 1) * power);
      p.maxLife = p.life = kind === 'smoke' ? U.rand(0.9, 1.8) : kind === 'flash' ? 0.09 : U.rand(0.3, 0.65);
      p.grow = kind === 'smoke' ? U.rand(1.2, 2.2) : kind === 'flash' ? U.rand(3, 5) : 0;
      const s = kind === 'smoke' ? U.rand(0.8, 1.6) : kind === 'flash' ? U.rand(0.5, 1) : U.rand(0.7, 1.6);
      p.mesh.scale.set(s, s, s);
      p.baseScale = s;
      this.particles.active.push(p);
    }
  },
  spawnBlood(point, dir, n) {
    this.spawnParticles('blood', point.x, point.y, point.z, n, 4.5, 0.7);
  },
  spawnDirt(x, y, z, n, power) { this.spawnParticles('dirt', x, y, z, n, power || 2.5, 0.8); },
  spawnMuzzleFlashAt(pos) { this.spawnParticles('flash', pos.x, pos.y, pos.z, 1, 0.1, 0); },

  spawnTracer(from, to, color, life) {
    const t = this.tracers.find(q => !q.on);
    if (!t) return;
    t.on = true;
    t.maxLife = t.life = life || 0.07;
    const m = t.mesh;
    m.visible = true;
    m.material.color.setHex(color || 0xffd27f);
    m.material.opacity = 1;
    const mid = new THREE.Vector3().addVectors(from, to).multiplyScalar(0.5);
    m.position.copy(mid);
    m.lookAt(to);
    m.scale.z = from.distanceTo(to);
  },

  addBloodPool(x, z) {
    const d = this.decals.find(q => !q.on) || this.decals[0];
    d.on = true; d.life = 20;
    const m = d.mesh;
    m.visible = true;
    m.position.set(x, World.heightAt(x, z) + 0.04, z);
    m.rotation.z = U.rand(0, 6.2);
    const s = U.rand(0.9, 2.1);
    m.scale.set(s, s, 1);
    m.material.opacity = 0.85;
  },

  updateFX(dt) {
    // particles
    const act = this.particles.active;
    for (let i = act.length - 1; i >= 0; i--) {
      const p = act[i];
      p.life -= dt;
      if (p.life <= 0) { p.on = false; p.mesh.visible = false; act.splice(i, 1); continue; }
      p.vel.y -= p.grav * dt;
      p.vel.multiplyScalar(Math.pow(p.drag, dt * 60));
      p.mesh.position.addScaledVector(p.vel, dt);
      p.mesh.quaternion.copy(this.camera.quaternion);
      const k = p.life / p.maxLife;
      p.mesh.material.opacity = k * 0.95;
      if (p.grow) { const s = p.baseScale * (1 + (1 - k) * p.grow); p.mesh.scale.set(s, s, s); }
    }
    // tracers
    for (const t of this.tracers) {
      if (!t.on) continue;
      t.life -= dt;
      if (t.life <= 0) { t.on = false; t.mesh.visible = false; continue; }
      t.mesh.material.opacity = t.life / t.maxLife;
    }
    // decals
    for (const d of this.decals) {
      if (!d.on) continue;
      d.life -= dt;
      if (d.life <= 0) { d.on = false; d.mesh.visible = false; continue; }
      if (d.life < 4) d.mesh.material.opacity = 0.85 * d.life / 4;
    }
    // grenades
    for (let i = this.grenades.length - 1; i >= 0; i--) {
      const g = this.grenades[i];
      g.t -= dt;
      g.vel.y -= 20 * dt;
      g.mesh.position.addScaledVector(g.vel, dt);
      g.mesh.rotation.x += dt * 7; g.mesh.rotation.z += dt * 5;
      const gy = World.heightAt(g.mesh.position.x, g.mesh.position.z) + 0.13;
      if (g.mesh.position.y < gy) {
        g.mesh.position.y = gy;
        if (Math.abs(g.vel.y) > 1.5) { SFX.footstep(); }
        g.vel.y = -g.vel.y * 0.34;
        g.vel.x *= 0.6; g.vel.z *= 0.6;
      }
      if (g.t <= 0) {
        this.explode(g.mesh.position.clone());
        this.scene.remove(g.mesh);
        this.grenades.splice(i, 1);
      }
    }
    if (this.shake > 0) this.shake = Math.max(0, this.shake - dt * 2.2);
  },

  /* ---------------- explosions & bombardment ---------------- */
  explode(pos) {
    SFX.explosion(false);
    this.shake = Math.min(1, this.shake + 0.65);
    this.spawnParticles('flash', pos.x, pos.y + 0.3, pos.z, 3, 0.1, 0);
    this.spawnParticles('dirt', pos.x, pos.y, pos.z, 22, 9, 1.4);
    this.spawnParticles('smoke', pos.x, pos.y + 0.5, pos.z, 7, 2.5, 1);
    this.spawnParticles('spark', pos.x, pos.y + 0.3, pos.z, 10, 8, 1);
    Enemies.explodeAt(pos.x, pos.z, 6.5, 115);
    const pd = Math.sqrt(U.dist2(pos.x, pos.z, Player.pos.x, Player.pos.z));
    if (pd < 5.5) Player.damage(Math.round(40 * (1 - pd / 5.5)) + 5, pos);
  },

  throwGrenade() {
    if (this.state !== 'playing' || this.grenadeCount <= 0) return;
    this.grenadeCount--;
    UI.refreshGrenades();
    SFX.grenadeThrow();
    if (this.stats) this.stats.grenades++;
    const mesh = new THREE.Mesh(this.grenadeGeo, this.grenadeMat);
    const dir = new THREE.Vector3();
    this.camera.getWorldDirection(dir);
    mesh.position.copy(this.camera.position).addScaledVector(dir, 0.5);
    mesh.position.y -= 0.15;
    this.scene.add(mesh);
    this.grenades.push({
      mesh,
      vel: dir.multiplyScalar(14).add(new THREE.Vector3(Player.vel.x * 0.35, 3.2, Player.vel.z * 0.35)),
      t: CONFIG.grenadeFuse,
    });
  },

  _bombardment(dt) {
    this.bombardT -= dt;
    if (this.bombardT > 0) return;
    this.bombardT = U.rand(7, 17);
    const a = U.rand(0, Math.PI * 2), r = U.rand(165, 205);
    const x = Math.cos(a) * r, z = Math.sin(a) * r;
    const y = World.heightAt(x * 0.8, z * 0.8) + U.rand(1, 6);
    this.spawnParticles('flash', x, y, z, 2, 0.1, 0);
    this.spawnParticles('smoke', x, y + 3, z, 3, 1.5, 1.2);
    this.shake = Math.min(1, this.shake + 0.05);
    setTimeout(() => SFX.distantBoom(), U.rand(700, 1400));
  },

  /* ================================================================
     COMBAT — player firing resolution
     ================================================================ */
  playerShoot(def) {
    SFX.shot(def.sound);
    this.shake = Math.min(1, this.shake + def.shake * 0.3);
    Player.pitchKick += def.kick * (Weapons.ads > 0.5 ? 0.45 : 1) * 0.9;
    UI.crosshairSpread();

    // spread
    const spread = U.lerp(def.spread, def.adsSpread, Weapons.ads) * (Player.sprintT > 0.3 ? 3 : 1);
    const dir = new THREE.Vector3();
    this.camera.getWorldDirection(dir);
    dir.x += U.rand(-1, 1) * spread;
    dir.y += U.rand(-1, 1) * spread;
    dir.z += U.rand(-1, 1) * spread;
    dir.normalize();

    const origin = this.camera.position.clone();
    const ray = new THREE.Raycaster(origin, dir, 0.1, 200);
    const targets = Enemies.hitMeshes.concat([World.terrain]);
    const hits = ray.intersectObjects(targets, false);

    const muzzle = new THREE.Vector3();
    Weapons.getMuzzleWorld(muzzle);

    if (hits.length) {
      const h = hits[0];
      const enemy = h.object.userData.enemy;
      if (enemy && enemy.alive) {
        const part = h.object.userData.part || 'body';
        enemy.lastHitPart = part;
        if (this.stats) { this.stats.hits++; if (part === 'head') this.stats.headHit = true; }
        SFX.hitmark(part === 'head');
        Enemies.damage(enemy, def.dmg, part, h.point, dir);
      } else {
        this.spawnDirt(h.point.x, h.point.y, h.point.z, 4, 2.2);
        if (this.stats) this.stats.headHit = false;
      }
      this.spawnTracer(muzzle, h.point, 0xffd27f);
    } else {
      this.spawnTracer(muzzle, origin.clone().addScaledVector(dir, 160), 0xffd27f);
    }
  },

  hitmarker(kill) { UI.hitmarker(kill); },

  onEnemyKilled(e) {
    if (!this.stats) return;
    const hs = e.lastHitPart === 'head';
    this.stats.kills++;
    if (hs) this.stats.headshots++;
    const score = e.cfg.score + (hs ? 5 : 0);
    this.stats.score += score;
    UI.killfeed(`${e.cfg.name}${hs ? ' — HEADSHOT' : ''} +${score}`);
  },

  damageFlash() {
    const el = document.getElementById('damage-flash');
    el.classList.add('hit');
    clearTimeout(this._dfT);
    this._dfT = setTimeout(() => el.classList.remove('hit'), 60);
  },

  showDamageDir(fromPos) {
    const dx = fromPos.x - Player.pos.x, dz = fromPos.z - Player.pos.z;
    const fx = -Math.sin(Player.yaw), fz = -Math.cos(Player.yaw);
    const rx = Math.cos(Player.yaw), rz = -Math.sin(Player.yaw);
    const rel = Math.atan2(dx * rx + dz * rz, dx * fx + dz * fz);
    const el = document.getElementById('dir-indicator');
    el.style.transform = `rotate(${rel * 180 / Math.PI}deg)`;
    const arc = el.querySelector('.dir-arc');
    arc.classList.add('show');
    clearTimeout(this._ddT);
    this._ddT = setTimeout(() => arc.classList.remove('show'), 90);
  },

  onPlayerDeath() {
    if (this.state !== 'playing') return;
    this.state = 'dead';
    document.exitPointerLock && document.exitPointerLock();
    SFX.enemyDie();
    setTimeout(() => {
      UI.fillStats('death-stats');
      this.showScreen('death-screen');
    }, 900);
  },

  /* ================================================================
     MISSIONS — phase machine
     ================================================================ */
  startMission(id) {
    const m = MISSIONS.find(x => x.id === id);
    if (!m) return;
    this.mission = m;
    this.showScreen(null);
    SFX.init(); SFX.ui();
    const fade = document.getElementById('fade-layer');
    fade.classList.add('black');
    setTimeout(() => {
      fade.classList.remove('black');
      UI.showStory(m.story, () => this._deploy(m));
    }, 700);
  },

  _deploy(m) {
    // keep scene hidden while we reset, reveal when the player deploys
    document.getElementById('fade-layer').classList.add('black');
    // title card
    const tc = document.getElementById('title-card');
    document.getElementById('title-card-text').textContent = m.name;
    tc.classList.remove('hidden');
    requestAnimationFrame(() => tc.classList.add('show'));
    setTimeout(() => {
      tc.classList.remove('show');
      setTimeout(() => tc.classList.add('hidden'), 1200);
    }, 2300);

    this._resetRun(m);
    setTimeout(() => {
      document.getElementById('fade-layer').classList.remove('black');
      this.state = 'playing';
      UI.enterGame();
      if (!Player.isTouch && !this.lockBlocked) this.canvas.requestPointerLock();
      this._initPhase();
    }, 2600);
  },

  _resetRun(m) {
    this.time = 0;
    this.phaseIdx = 0;
    this.grenadeCount = CONFIG.player.maxGrenades;
    this.stats = { kills: 0, headshots: 0, shots: 0, hits: 0, headHit: false, score: 0, grenades: 0, startWall: performance.now() };
    Enemies.clearAll();
    for (const g of this.grenades) this.scene.remove(g.mesh);
    this.grenades = [];
    Player.reset(m.startX, m.startZ, m.startYaw);
    const unlocked = Object.keys(WEAPONS).filter(i => WEAPONS[i].unlockedAtStart ||
      (WEAPONS[i].unlock && this.save.completed.includes(WEAPONS[i].unlock)));
    Weapons.init(this.camera, unlocked);
    UI.refreshGrenades();
    this.bombardT = U.rand(4, 9);
    this.beacon = null;
  },

  _initPhase() {
    const ph = this.mission.phases[this.phaseIdx];
    if (!ph) return this.completeMission();
    if (ph.type === 'waves') {
      this.waveIdx = 0;
      this.intermission = 0;
      this._startWave();
    } else if (ph.type === 'reach') {
      UI.setObjective(ph.label);
      this._placeBeacon(ph.target);
      this.trickleT = 2;
      SFX.waveHorn();
    } else if (ph.type === 'defend') {
      this.defendT = ph.time;
      this.trickleT = 1;
      UI.setObjective(ph.label || 'HOLD POSITION');
      UI.showBanner(ph.label || 'HOLD POSITION', 2.6);
      SFX.waveHorn();
    }
  },

  _placeBeacon(t) {
    const c = document.createElement('canvas'); c.width = c.height = 64;
    const g = c.getContext('2d');
    g.fillStyle = '#ffd24a';
    g.beginPath(); g.moveTo(32, 4); g.lineTo(56, 32); g.lineTo(32, 60); g.lineTo(8, 32); g.closePath(); g.fill();
    g.fillStyle = '#7a5c10';
    g.beginPath(); g.moveTo(32, 16); g.lineTo(44, 32); g.lineTo(32, 48); g.lineTo(20, 32); g.closePath(); g.fill();
    const spr = new THREE.Sprite(new THREE.SpriteMaterial({ map: new THREE.CanvasTexture(c), transparent: true, depthWrite: false }));
    spr.position.set(t.x, World.heightAt(t.x, t.z) + 16, t.z);
    spr.scale.set(5.5, 5.5, 1);
    this.scene.add(spr);
    this.beacon = spr;
  },

  _startWave() {
    const ph = this.mission.phases[this.phaseIdx];
    const wave = ph.waves[this.waveIdx];
    this.wavePending = [];
    for (const [type, n] of wave.list) for (let i = 0; i < n; i++) this.wavePending.push(type);
    // shuffle a little so types intermix
    for (let i = this.wavePending.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [this.wavePending[i], this.wavePending[j]] = [this.wavePending[j], this.wavePending[i]];
    }
    this.waveSpawnT = 1.5;
    UI.showBanner(`WAVE ${this.waveIdx + 1}`, 2.2);
    UI.setObjective(`${this.mission.objectiveShort} — WAVE ${this.waveIdx + 1} / ${ph.waves.length}`);
    SFX.waveHorn();
  },

  _spawnFromZone(type) {
    const ph = this.mission.phases[this.phaseIdx];
    const zoneName = ph.spawnZone || 'cratersFront';
    let zone = World.spawnZones[zoneName] || World.spawnZones.cratersFront;
    if (type === 'soldier') zone = World.spawnZones.ridge;
    const p = U.pick(zone);
    const jx = p.x + U.rand(-2, 2), jz = p.z + U.rand(-2, 2);
    Enemies.spawn(type, jx, jz);
  },

  _updateMission(dt) {
    const ph = this.mission.phases[this.phaseIdx];
    if (!ph) return;

    if (ph.type === 'waves') {
      if (this.intermission > 0) {
        this.intermission -= dt;
        UI.setObjective(`NEXT WAVE IN ${Math.ceil(this.intermission)}…`);
        if (this.intermission <= 0) { this.waveIdx++; this._startWave(); }
        return;
      }
      // spawn pending
      if (this.wavePending.length) {
        this.waveSpawnT -= dt;
        if (this.waveSpawnT <= 0 && Enemies.aliveCount() < CONFIG.enemyCap[this.effQuality]) {
          this._spawnFromZone(this.wavePending.shift());
          this.waveSpawnT = ph.waves[this.waveIdx].interval * U.rand(0.7, 1.3);
        }
      }
      const remaining = this.wavePending.length + Enemies.aliveCount();
      UI.setWave(this.waveIdx + 1, ph.waves.length, remaining);
      if (!this.wavePending.length && Enemies.aliveCount() === 0) {
        if (this.waveIdx >= ph.waves.length - 1) { this._phaseDone(); }
        else {
          this.intermission = CONFIG.intermissionTime;
          UI.showBanner('WAVE CLEARED — RESUPPLIED', 2.6);
          this._resupply();
          SFX.pickup();
        }
      }
    } else if (ph.type === 'reach') {
      // trickle spawns chasing the player
      this.trickleT -= dt;
      if (this.trickleT <= 0 && Enemies.aliveCount() < CONFIG.enemyCap[this.effQuality]) {
        this.trickleT = U.rand(2.4, 4.2);
        const type = Math.random() < 0.7 ? (Math.random() < 0.7 ? 'zombie' : 'runner') : 'soldier';
        // spawn behind/around the player (kept inside map bounds)
        const a = U.rand(0, Math.PI * 2), r = U.rand(30, 48);
        let x = Player.pos.x + Math.cos(a) * r, z = Player.pos.z + Math.sin(a) * r;
        const cr = Math.hypot(x, z), maxR = CONFIG.mapRadius - 8;
        if (cr > maxR) { x *= maxR / cr; z *= maxR / cr; }
        Enemies.spawn(type, x, z);
      }
      const d = Math.sqrt(U.dist2(Player.pos.x, Player.pos.z, ph.target.x, ph.target.z));
      UI.setObjective(`${ph.label} — ${Math.max(0, Math.round(d))}m`);
      if (this.beacon) {
        const s = 5.5 + Math.sin(this.time * 3) * 1;
        this.beacon.scale.set(s, s, 1);
      }
      if (d < ph.target.r) {
        if (this.beacon) { this.scene.remove(this.beacon); this.beacon = null; }
        UI.showBanner('POSITION REACHED', 2.4);
        this._phaseDone();
      }
    } else if (ph.type === 'defend') {
      this.defendT -= dt;
      this.trickleT -= dt;
      if (this.trickleT <= 0 && Enemies.aliveCount() < CONFIG.enemyCap[this.effQuality]) {
        this.trickleT = ph.spawnEvery * U.rand(0.7, 1.3);
        const type = U.pick(ph.list);
        const zone = Math.random() < 0.6 ? World.spawnZones.church : World.spawnZones.cratersAll;
        const p = U.pick(zone);
        Enemies.spawn(type, p.x + U.rand(-2, 2), p.z + U.rand(-2, 2));
      }
      UI.setObjective(`${ph.label} — ${Math.max(0, Math.ceil(this.defendT))}s`);
      UI.setWave(0, 0, Enemies.aliveCount());
      if (this.defendT <= 0) this._phaseDone();
    }
  },

  _phaseDone() {
    this.phaseIdx++;
    if (this.phaseIdx >= this.mission.phases.length) this.completeMission();
    else {
      this._resupply();
      this._initPhase();
    }
  },

  _resupply() {
    for (const id in Weapons.state) {
      const st = Weapons.state[id], def = WEAPONS[id];
      st.reserve = def.reserve;
    }
    this.grenadeCount = Math.min(CONFIG.player.maxGrenades, this.grenadeCount + 1);
    UI.refreshGrenades();
  },

  completeMission() {
    this.state = 'complete';
    document.exitPointerLock && document.exitPointerLock();
    SFX.missionComplete();
    // progress
    if (!this.save.completed.includes(this.mission.id)) this.save.completed.push(this.mission.id);
    SaveData.store(this.save);
    this.buildMissionList();
    setTimeout(() => {
      UI.fillStats('complete-stats');
      document.getElementById('unlock-note').textContent = this.mission.unlocksText || '';
      this.showScreen('complete-screen');
    }, 800);
  },

  /* ================================================================
     STATE CONTROL
     ================================================================ */
  pause() {
    if (this.state !== 'playing') return;
    this.state = 'paused';
    document.exitPointerLock && document.exitPointerLock();
    this.showScreen('pause-menu');
  },
  resume() {
    if (this.state !== 'paused') return;
    this.state = 'playing';
    this.showScreen(null);
    if (!Player.isTouch && !this.lockBlocked) this.canvas.requestPointerLock();
  },
  restartMission() {
    this.showScreen(null);
    this._resetRun(this.mission);
    this.state = 'playing';
    UI.enterGame();
    if (!Player.isTouch && !this.lockBlocked) this.canvas.requestPointerLock();
    this._initPhase();
  },
  quitToMenu() {
    this.state = 'menu';
    document.exitPointerLock && document.exitPointerLock();
    Enemies.clearAll();
    UI.exitGame();
    this.buildMissionList();
    this.showScreen('menu-main');
  },

  showScreen(id) {
    document.querySelectorAll('.screen').forEach(s => s.classList.add('hidden'));
    if (id) document.getElementById(id).classList.remove('hidden');
  },

  buildMissionList() {
    const list = document.getElementById('mission-list');
    list.innerHTML = '';
    for (const m of MISSIONS) {
      const locked = m.requires && !this.save.completed.includes(m.requires);
      const done = this.save.completed.includes(m.id);
      const card = document.createElement('div');
      card.className = 'mission-card' + (locked ? ' locked' : '');
      card.innerHTML = `<div><div class="mc-name">${m.name}</div>
        <div class="mc-desc">${m.chapter}<br>${m.desc} &nbsp;·&nbsp; ${m.difficulty}</div></div>
        <div class="mc-status">${locked ? 'LOCKED' : done ? 'CLEARED ✓' : 'AVAILABLE'}</div>`;
      if (!locked) card.addEventListener('click', () => { SFX.init(); SFX.ui(); this.startMission(m.id); });
      list.appendChild(card);
    }
  },

  syncSettingsUI() {
    UI.syncSeg('seg-quality', this.settings.quality);
    UI.syncSeg('seg-invert', this.settings.invertY ? 'on' : 'off');
    UI.syncSeg('seg-sound', this.settings.sound ? 'on' : 'off');
    UI.syncSeg('seg-fps', this.settings.showFps ? 'on' : 'off');
    document.getElementById('set-sens').value = this.settings.sens;
    document.getElementById('sens-val').textContent = this.settings.sens.toFixed(1);
  },
  saveSettings() {
    this.save.settings = this.settings;
    SaveData.store(this.save);
    this.applyQuality();
  },

  /* ================================================================
     MAIN LOOP
     ================================================================ */
  loop() {
    requestAnimationFrame(() => this.loop());
    let dt = this.clock.getDelta();
    dt = Math.min(dt, 0.05);

    // fps counter
    this.frames++; this.fpsT += dt;
    if (this.fpsT >= 0.5) {
      this.fps = Math.round(this.frames / this.fpsT);
      this.frames = 0; this.fpsT = 0;
      if (this.settings.showFps) UI.setFps(this.fps, this.renderer.info.render.calls);
    }

    if (this.state === 'menu') {
      // slow cinematic orbit over the battlefield
      const t = performance.now() * 0.001;
      const r = 46;
      this.camera.position.set(Math.sin(t * 0.06) * r, 13 + Math.sin(t * 0.13) * 2, 40 + Math.cos(t * 0.06) * r);
      this.camera.lookAt(0, 1, 12);
      if (this.camera.fov !== 60) { this.camera.fov = 60; this.camera.updateProjectionMatrix(); }
      World.update(dt);
    } else if (this.state === 'playing') {
      this.time += dt;
      Player.update(dt);
      Weapons.update(dt, Math.sqrt(Player.vel.x ** 2 + Player.vel.z ** 2), Player.bobPhase, Player.adsTarget);
      Enemies.update(dt);
      this._updateMission(dt);
      this._bombardment(dt);
      World.update(dt);
      this.updateFX(dt);
      UI.updateHUD();
    } else if (this.state === 'dead' || this.state === 'complete') {
      World.update(dt);
      this.updateFX(dt);
    }

    this.renderer.render(this.scene, this.camera);
  },
};

/* ============================================================
   UI helpers (DOM plumbing, kept close to game state)
   ============================================================ */
window.UI = {
  els: {},
  bind(game) {
    const $ = id => document.getElementById(id);
    this.els = {
      hud: $('hud'), crosshair: $('crosshair'), hitmarker: $('hitmarker'),
      hpBar: $('health-bar'), hpNum: $('health-num'),
      ammoMag: $('ammo-mag'), ammoRes: $('ammo-res'), weaponName: $('weapon-name'), ammoBox: $('ammo-box'),
      gPips: $('grenade-pips'), waveTitle: $('wave-title'), waveSub: $('wave-sub'),
      objective: $('objective-text'), banner: $('banner'), killfeed: $('killfeed'),
      oob: $('oob-warn'), fps: $('fps-counter'), pauseBtn: $('pause-btn'),
      touch: $('touch-ui'), compass: $('compass'),
    };

    $('btn-deploy').onclick = () => { SFX.init(); SFX.ui(); game.showScreen('menu-missions'); };
    $('btn-settings').onclick = () => { SFX.init(); SFX.ui(); game.showScreen('menu-settings'); };
    $('btn-help').onclick = () => { SFX.init(); SFX.ui(); game.showScreen('menu-help'); };
    $('btn-back-1').onclick = $('btn-back-2').onclick = $('btn-back-3').onclick =
      () => { SFX.ui(); game.showScreen('menu-main'); };
    $('btn-resume').onclick = () => { SFX.ui(); game.resume(); };
    $('btn-restart').onclick = () => { SFX.ui(); game.restartMission(); };
    $('btn-quit').onclick = () => { SFX.ui(); game.quitToMenu(); };
    $('btn-retry').onclick = () => { SFX.ui(); game.restartMission(); };
    $('btn-death-quit').onclick = () => { SFX.ui(); game.quitToMenu(); };
    $('btn-next').onclick = () => { SFX.ui(); game.quitToMenu(); };
    if (!Player.isTouch) $('pause-btn').onclick = () => game.pause();

    // settings widgets
    const segBind = (id, fn) => {
      const seg = $(id);
      seg.querySelectorAll('button').forEach(b => b.onclick = () => {
        SFX.init(); SFX.ui();
        this.syncSeg(id, b.dataset.v);
        fn(b.dataset.v);
      });
    };
    segBind('seg-quality', v => { game.settings.quality = v; game.saveSettings(); });
    segBind('seg-invert', v => { game.settings.invertY = v === 'on'; game.saveSettings(); });
    segBind('seg-sound', v => { game.settings.sound = v === 'on'; game.saveSettings(); });
    segBind('seg-fps', v => { game.settings.showFps = v === 'on'; game.saveSettings(); });
    $('set-sens').oninput = e => {
      game.settings.sens = parseFloat(e.target.value);
      $('sens-val').textContent = game.settings.sens.toFixed(1);
      game.saveSettings();
    };

    // story overlay advance
    $('story-overlay').addEventListener('click', () => this._storyAdvance());
  },

  syncSeg(id, v) {
    document.getElementById(id).querySelectorAll('button').forEach(b =>
      b.classList.toggle('sel', b.dataset.v === String(v)));
  },

  /* -------- screens -------- */
  enterGame() {
    this.els.hud.classList.remove('hidden');
    this.setWave(0, 0, 0);
    this.setObjective(Game.mission ? Game.mission.objectiveShort : 'HOLD THE LINE');
    this.refreshGrenades();
    if (Player.isTouch || Game.lockBlocked) this.showTouchControls();
    else this.els.pauseBtn.style.display = 'none';
  },
  showTouchControls() {
    this.els.touch.classList.remove('hidden');
    this.els.pauseBtn.style.display = 'block';
  },
  exitGame() {
    this.els.hud.classList.add('hidden');
    this.els.touch.classList.add('hidden');
  },

  /* -------- story typewriter -------- */
  showStory(pages, onDone) {
    this._story = { pages, i: 0, onDone, typing: true, shown: 0 };
    const ov = document.getElementById('story-overlay');
    ov.classList.remove('hidden');
    Game.state = 'story';
    this._storyPage();
  },
  _storyPage() {
    const s = this._story;
    const p = s.pages[s.i];
    document.getElementById('story-chapter').textContent = p.chapter;
    const el = document.getElementById('story-text');
    el.textContent = '';
    s.typing = true; s.shown = 0;
    clearInterval(this._storyTimer);
    this._storyTimer = setInterval(() => {
      s.shown++;
      el.textContent = p.text.slice(0, s.shown);
      if (s.shown >= p.text.length) { s.typing = false; clearInterval(this._storyTimer); }
    }, 16);
  },
  _storyAdvance() {
    const s = this._story;
    if (!s) return;
    if (s.typing) {
      clearInterval(this._storyTimer);
      s.typing = false;
      document.getElementById('story-text').textContent = s.pages[s.i].text;
      return;
    }
    s.i++;
    if (s.i >= s.pages.length) {
      document.getElementById('story-overlay').classList.add('hidden');
      const done = s.onDone;
      this._story = null;
      done();
    } else this._storyPage();
  },

  /* -------- HUD -------- */
  setObjective(t) { this.els.objective.textContent = t; },
  setWave(i, n, remaining) {
    if (n > 0) {
      this.els.waveTitle.textContent = `WAVE ${i} / ${n}`;
      this.els.waveSub.textContent = `HOSTILES REMAINING: ${remaining}`;
    } else if (remaining > 0) {
      this.els.waveTitle.textContent = 'HOSTILES';
      this.els.waveSub.textContent = `ACTIVE: ${remaining}`;
    } else {
      this.els.waveTitle.textContent = 'STANDBY';
      this.els.waveSub.textContent = '';
    }
  },
  showBanner(text, dur) {
    this.els.banner.textContent = text;
    this.els.banner.classList.add('show');
    clearTimeout(this._bannerT);
    this._bannerT = setTimeout(() => this.els.banner.classList.remove('show'), (dur || 2.2) * 1000);
  },
  hitmarker(kill) {
    const el = this.els.hitmarker;
    el.classList.toggle('kill', !!kill);
    el.classList.add('show');
    clearTimeout(this._hmT);
    this._hmT = setTimeout(() => el.classList.remove('show'), 100);
  },
  crosshairSpread() {
    this.els.crosshair.classList.add('spread');
    clearTimeout(this._chT);
    this._chT = setTimeout(() => this.els.crosshair.classList.remove('spread'), 110);
  },
  killfeed(text) {
    const el = document.createElement('div');
    el.className = 'kf-entry';
    el.textContent = text;
    this.els.killfeed.prepend(el);
    while (this.els.killfeed.children.length > 5) this.els.killfeed.lastChild.remove();
    setTimeout(() => { el.classList.add('fade'); setTimeout(() => el.remove(), 700); }, 2600);
  },
  refreshGrenades() {
    this.els.gPips.textContent = '✤ '.repeat(Math.max(0, Game.grenadeCount)).trim() || '—';
  },
  setFps(fps, calls) {
    this.els.fps.textContent = `${fps} FPS · ${calls} DRAWS`;
  },

  updateHUD() {
    const hp = Math.ceil(Player.hp);
    this.els.hpBar.style.width = hp + '%';
    this.els.hpBar.classList.toggle('low', hp < 35);
    this.els.hpNum.textContent = hp;
    document.getElementById('lowhp-pulse').style.opacity = hp < 35 ? 0.4 + Math.sin(Game.time * 4) * 0.25 : 0;

    const a = Weapons.hudAmmo();
    this.els.ammoMag.textContent = a.mag;
    this.els.ammoRes.textContent = ' / ' + a.reserve;
    this.els.weaponName.textContent = a.name + (a.reloading ? ' · RELOADING' : '');
    this.els.ammoBox.classList.toggle('reloading', a.reloading);

    this.els.crosshair.classList.toggle('ads', Weapons.ads > 0.6);
    this.els.oob.style.display = Game.outOfBounds ? 'block' : 'none';
    if (Game.settings.showFps === false && this.els.fps.textContent) this.els.fps.textContent = '';

    this._drawCompass();
  },

  _drawCompass() {
    const cv = this.els.compass;
    const ctx = cv.getContext('2d');
    const w = cv.width, h = cv.height;
    ctx.clearRect(0, 0, w, h);
    const heading = ((-Player.yaw * 180 / Math.PI) % 360 + 360) % 360;
    const labels = { 0: 'N', 45: 'NE', 90: 'E', 135: 'SE', 180: 'S', 225: 'SW', 270: 'W', 315: 'NW' };
    ctx.textAlign = 'center'; ctx.textBaseline = 'middle';
    for (let d = 0; d < 360; d += 15) {
      let delta = ((d - heading + 540) % 360) - 180;
      if (delta < -60 || delta > 60) continue;
      const x = w / 2 + delta * (w / 120);
      const edge = 1 - Math.abs(delta) / 60;
      if (labels[d] !== undefined) {
        ctx.font = d % 90 === 0 ? 'bold 13px Arial' : '10px Arial';
        ctx.fillStyle = `rgba(232,226,207,${0.35 + edge * 0.6})`;
        ctx.fillText(labels[d], x, h / 2);
      } else {
        ctx.fillStyle = `rgba(200,190,160,${0.25 + edge * 0.4})`;
        ctx.fillRect(x - 1, h - 8, 2, 5);
      }
    }
    ctx.fillStyle = '#c8a84a';
    ctx.fillRect(w / 2 - 1, 2, 2, 6);
  },

  fillStats(elId) {
    const s = Game.stats || { kills: 0, headshots: 0, shots: 0, hits: 0, score: 0, grenades: 0, startWall: performance.now() };
    const secs = Math.round((performance.now() - s.startWall) / 1000);
    const acc = s.shots ? Math.round(s.hits / s.shots * 100) : 0;
    document.getElementById(elId).innerHTML =
      `KILLS <b>${s.kills}</b><br>HEADSHOTS <b>${s.headshots}</b><br>` +
      `ACCURACY <b>${acc}%</b><br>GRENADES THROWN <b>${s.grenades}</b><br>` +
      `TIME <b>${Math.floor(secs / 60)}:${String(secs % 60).padStart(2, '0')}</b><br>` +
      `SCORE <b>${s.score}</b>`;
  },
};
