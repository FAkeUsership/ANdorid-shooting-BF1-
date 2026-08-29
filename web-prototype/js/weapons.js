/* ============================================================
   IRONFRONT 1917 — weapons.js
   First-person viewmodels (WZ2-style chunky hands + blocky
   guns) and weapon state: ammo, cooldowns, reload, ADS, recoil.
   Firing resolution (raycasts, damage) lives in game.js.
   ============================================================ */
'use strict';

window.Weapons = {
  camera: null, holder: null,        // holder = sway/bob group on camera
  models: {},                        // id -> {group, muzzlePoint, flash}
  state: {},                         // id -> {mag, reserve, owned}
  current: 'rifle',
  cooldown: 0, reloading: 0, reloadDur: 0,
  switchT: 0, switchTo: null,
  kick: 0, ads: 0, swayT: 0,

  /* ---------------- init ---------------- */
  init(camera, unlockedIds) {
    this.camera = camera;
    if (this.holder && this.holder.parent) this.holder.parent.remove(this.holder); // restart safety
    this.holder = new THREE.Group();
    camera.add(this.holder);
    this.models = {};
    const flashTex = this._flashTexture();
    for (const id of Object.keys(WEAPONS)) {
      const def = WEAPONS[id];
      const g = this['_build_' + def.model]();
      g.visible = false;
      this.holder.add(g);
      const muzzlePoint = new THREE.Object3D();
      muzzlePoint.position.copy(g.userData.muzzle || new THREE.Vector3(0, 0, -0.8));
      g.add(muzzlePoint);
      const flash = new THREE.Sprite(new THREE.SpriteMaterial({
        map: flashTex, transparent: true, depthWrite: false, depthTest: false,
        blending: THREE.AdditiveBlending, color: 0xffd9a0,
      }));
      flash.scale.set(0.34, 0.34, 1);
      flash.visible = false;
      muzzlePoint.add(flash);
      this.models[id] = { group: g, muzzlePoint, flash, flashT: 0 };
      this.state[id] = { mag: def.mag, reserve: def.reserve, owned: unlockedIds.includes(id) };
    }
    this.current = 'rifle';
    this.models.rifle.group.visible = true;
    this.cooldown = 0; this.reloading = 0; this.switchT = 0; this.kick = 0; this.ads = 0;
  },

  _flashTexture() {
    const c = document.createElement('canvas'); c.width = c.height = 64;
    const g = c.getContext('2d');
    const grd = g.createRadialGradient(32, 32, 1, 32, 32, 30);
    grd.addColorStop(0, 'rgba(255,255,230,1)');
    grd.addColorStop(0.25, 'rgba(255,200,90,0.9)');
    grd.addColorStop(0.6, 'rgba(255,120,20,0.35)');
    grd.addColorStop(1, 'rgba(255,80,0,0)');
    g.fillStyle = grd; g.fillRect(0, 0, 64, 64);
    // star spikes
    g.strokeStyle = 'rgba(255,230,150,0.85)'; g.lineWidth = 3;
    for (let i = 0; i < 4; i++) {
      const a = i * Math.PI / 2 + 0.4;
      g.beginPath();
      g.moveTo(32 + Math.cos(a) * 6, 32 + Math.sin(a) * 6);
      g.lineTo(32 + Math.cos(a) * 28, 32 + Math.sin(a) * 28);
      g.stroke();
    }
    return new THREE.CanvasTexture(c);
  },

  /* ---------------- shared bits for viewmodels ---------------- */
  _hand(x, y, z, rz) {
    const skin = new THREE.MeshLambertMaterial({ color: 0xc9976b, flatShading: true });
    const sleeve = new THREE.MeshLambertMaterial({ color: 0x4d5340, flatShading: true });
    const g = new THREE.Group();
    const h = new THREE.Mesh(new THREE.BoxGeometry(0.105, 0.13, 0.15), skin);
    const thumb = new THREE.Mesh(new THREE.BoxGeometry(0.045, 0.05, 0.09), skin);
    thumb.position.set(-0.055, 0.05, -0.01);
    const s = new THREE.Mesh(new THREE.BoxGeometry(0.125, 0.125, 0.16), sleeve);
    s.position.z = 0.14;
    g.add(h, thumb, s);
    g.position.set(x, y, z);
    if (rz) g.rotation.z = rz;
    return g;
  },

  /* ---------------- model builders ---------------- */
  _build_rifle() {
    const g = new THREE.Group();
    const wood = new THREE.MeshLambertMaterial({ color: 0x6b4a2a, flatShading: true });
    const metal = new THREE.MeshLambertMaterial({ color: 0x33353a, flatShading: true });
    const dark = new THREE.MeshLambertMaterial({ color: 0x232428, flatShading: true });
    const add = (geo, m, x, y, z, rx) => { const mm = new THREE.Mesh(geo, m); mm.position.set(x, y, z); if (rx) mm.rotation.x = rx; g.add(mm); return mm; };
    add(new THREE.BoxGeometry(0.075, 0.12, 0.46), wood, 0, -0.01, 0.26, 0.06);       // stock
    add(new THREE.BoxGeometry(0.06, 0.095, 0.34), metal, 0, 0.01, -0.02);             // receiver
    add(new THREE.BoxGeometry(0.055, 0.075, 0.34), wood, 0, -0.005, -0.36);           // handguard
    add(new THREE.BoxGeometry(0.032, 0.032, 0.52), metal, 0, 0.012, -0.62);           // barrel
    add(new THREE.BoxGeometry(0.02, 0.05, 0.02), dark, 0, 0.05, -0.86);               // front sight
    add(new THREE.BoxGeometry(0.05, 0.035, 0.02), dark, 0, 0.05, -0.12);              // rear sight
    const bolt = add(new THREE.BoxGeometry(0.09, 0.03, 0.03), metal, 0.05, 0.03, 0.04); // bolt handle
    bolt.rotation.z = -0.4;
    add(new THREE.BoxGeometry(0.03, 0.09, 0.05), metal, 0, -0.07, 0.06, 0.3);          // trigger guard-ish
    g.add(this._hand(0.015, -0.11, 0.2, 0.15));      // right hand on grip
    g.add(this._hand(-0.01, -0.09, -0.34, -0.1));    // left hand on handguard
    g.userData.muzzle = new THREE.Vector3(0, 0.012, -0.9);
    return g;
  },
  _build_pistol() {
    const g = new THREE.Group();
    const metal = new THREE.MeshLambertMaterial({ color: 0x3a3c42, flatShading: true });
    const wood = new THREE.MeshLambertMaterial({ color: 0x6b4a2a, flatShading: true });
    const add = (geo, m, x, y, z, rx) => { const mm = new THREE.Mesh(geo, m); mm.position.set(x, y, z); if (rx) mm.rotation.x = rx; g.add(mm); return mm; };
    add(new THREE.BoxGeometry(0.05, 0.065, 0.3), metal, 0, 0.03, -0.05);   // slide/barrel
    add(new THREE.BoxGeometry(0.045, 0.05, 0.2), metal, 0, -0.02, 0.0);    // frame
    add(new THREE.BoxGeometry(0.05, 0.15, 0.07), wood, 0, -0.1, 0.08, 0.25); // grip
    add(new THREE.BoxGeometry(0.018, 0.03, 0.018), metal, 0, 0.075, -0.18); // sight
    g.add(this._hand(0, -0.1, 0.09, 0));
    g.userData.muzzle = new THREE.Vector3(0, 0.03, -0.22);
    return g;
  },
  _build_smg() {
    const g = new THREE.Group();
    const metal = new THREE.MeshLambertMaterial({ color: 0x33343a, flatShading: true });
    const wood = new THREE.MeshLambertMaterial({ color: 0x6b4a2a, flatShading: true });
    const dark = new THREE.MeshLambertMaterial({ color: 0x232428, flatShading: true });
    const add = (geo, m, x, y, z, rx) => { const mm = new THREE.Mesh(geo, m); mm.position.set(x, y, z); if (rx) mm.rotation.x = rx; g.add(mm); return mm; };
    add(new THREE.BoxGeometry(0.08, 0.11, 0.4), metal, 0, 0, 0.05);                    // body
    add(new THREE.BoxGeometry(0.06, 0.08, 0.3), dark, 0, 0, -0.3);                    // barrel jacket
    add(new THREE.BoxGeometry(0.03, 0.03, 0.2), metal, 0, 0.01, -0.5);                // barrel
    add(new THREE.BoxGeometry(0.07, 0.1, 0.3), wood, 0, -0.01, 0.34, 0.05);           // stock
    const drum = add(new THREE.CylinderGeometry(0.085, 0.085, 0.12, 9), metal, 0.0, -0.13, -0.05); // drum mag
    drum.rotation.z = Math.PI / 2; drum.rotation.y = 0.3;
    add(new THREE.BoxGeometry(0.05, 0.1, 0.06), wood, 0, -0.08, 0.18, 0.3);           // grip
    add(new THREE.BoxGeometry(0.02, 0.04, 0.02), dark, 0, 0.06, -0.58);               // sight
    g.add(this._hand(0.01, -0.11, 0.2, 0.15));
    g.add(this._hand(-0.01, -0.1, -0.28, -0.12));
    g.userData.muzzle = new THREE.Vector3(0, 0.01, -0.62);
    return g;
  },

  /* ---------------- actions ---------------- */
  def() { return WEAPONS[this.current]; },
  st() { return this.state[this.current]; },

  canSwitch() { return this.reloading <= 0 && this.switchT <= 0; },

  switchWeapon(id) {
    if (!id || id === this.current || !this.state[id] || !this.state[id].owned) return false;
    if (this.switchT > 0) return false;
    this.switchTo = id;
    this.switchT = 0.001;      // will count up; phase 1 lowers, phase 2 raises
    this.reloading = 0;
    SFX.weaponSwap();
    return true;
  },
  nextWeapon() {
    const owned = Object.keys(WEAPONS).filter(i => this.state[i].owned);
    const idx = owned.indexOf(this.current);
    return this.switchWeapon(owned[(idx + 1) % owned.length]);
  },

  reload() {
    const def = this.def(), st = this.st();
    if (this.reloading > 0 || this.switchT > 0) return false;
    if (st.mag >= def.mag || st.reserve <= 0) return false;
    this.reloading = this.reloadDur = def.reload;
    SFX.reload(def.model);
    return true;
  },
  finishReload() {
    const def = this.def(), st = this.st();
    const need = def.mag - st.mag;
    const take = Math.min(need, st.reserve);
    st.mag += take; st.reserve -= take;
  },

  /* returns def if a shot was fired this call, else null */
  tryFire(triggerHeld) {
    const def = this.def(), st = this.st();
    if (this.cooldown > 0 || this.reloading > 0 || this.switchT > 0) return null;
    if (Player.sprintT > 0.3) return null;                       // can't shoot mid-sprint
    if (!def.auto && !triggerHeld.fresh) return null;            // semi needs fresh press
    if (st.mag <= 0) { SFX.dryFire(); this.reload(); return null; }
    st.mag--;
    this.cooldown = def.rate;
    this.kick = def.kick;
    this.models[this.current].flashT = 0.05;
    this.models[this.current].flash.material.rotation = U.rand(0, Math.PI * 2);
    if (st.mag === 0 && st.reserve > 0) setTimeout(() => { if (this.current === def.id && this.reloading <= 0) this.reload(); }, 220);
    return def;
  },

  /* ---------------- per-frame ---------------- */
  update(dt, moveSpeed, bobPhase, adsTarget) {
    this.cooldown = Math.max(0, this.cooldown - dt);
    this.swayT += dt;
    this.kick = U.dampCur(this.kick, 0, 9, dt);
    this.ads = U.dampCur(this.ads, adsTarget, 10, dt);

    // switch animation: 0.16s down, swap model, 0.2s up
    if (this.switchT > 0) {
      this.switchT += dt;
      if (this.switchT >= 0.16 && this.switchTo) {
        this.models[this.current].group.visible = false;
        this.current = this.switchTo;
        this.models[this.current].group.visible = true;
        this.switchTo = null;
      }
      if (this.switchT >= 0.36) this.switchT = 0;
    }

    // reload timing
    if (this.reloading > 0) {
      this.reloading -= dt;
      if (this.reloading <= 0) { this.reloading = 0; this.finishReload(); }
    }

    // muzzle flash decay
    for (const id in this.models) {
      const m = this.models[id];
      if (m.flashT > 0) {
        m.flashT -= dt;
        m.flash.visible = m.flashT > 0;
        const s = 0.3 + (0.05 - Math.max(0, m.flashT)) * 4;
        m.flash.scale.set(s, s, 1);
      }
    }

    /* ---- pose ---- */
    const def = this.def();
    const adsPos = { rifle: [0.0, -0.205, -0.44], pistol: [0.0, -0.235, -0.4], smg: [0.0, -0.235, -0.46] }[this.current];
    const hipPos = [0.3, -0.29, -0.56];
    const a = this.ads;
    let px = U.lerp(hipPos[0], adsPos[0], a);
    let py = U.lerp(hipPos[1], adsPos[1], a);
    let pz = U.lerp(hipPos[2], adsPos[2], a);

    // idle sway
    const sw = (1 - a) * 0.008 + 0.002;
    px += Math.sin(this.swayT * 1.3) * sw;
    py += Math.sin(this.swayT * 1.9) * sw * 0.7;

    // movement bob
    const bobAmp = U.clamp(moveSpeed / CONFIG.player.walkSpeed, 0, 1.15) * (1 - a * 0.85);
    py += Math.sin(bobPhase * 2) * 0.014 * bobAmp;
    px += Math.cos(bobPhase) * 0.011 * bobAmp;

    // sprint dip
    const sprint = Player.sprintT;
    py -= sprint * 0.09; pz -= sprint * 0.1;

    // recoil kick
    pz += this.kick * 1.6;
    py += this.kick * 0.35;

    // reload dip
    if (this.reloading > 0) {
      const t = 1 - this.reloading / this.reloadDur;
      const dip = Math.sin(Math.min(1, t * 1.15) * Math.PI);
      py -= dip * 0.16;
      this.holder.rotation.x = dip * 0.55;
    } else {
      this.holder.rotation.x = U.dampCur(this.holder.rotation.x, this.kick * 2.2, 12, dt);
    }

    // switch dip
    if (this.switchT > 0) {
      const t = this.switchT < 0.16 ? this.switchT / 0.16 : 1 - (this.switchT - 0.16) / 0.2;
      py -= t * 0.35;
    }

    this.holder.position.set(px, py, pz);
    this.holder.rotation.y = (1 - a) * -0.06 + Math.sin(this.swayT * 1.1) * 0.004;
    this.holder.rotation.z = Math.sin(bobPhase) * 0.012 * bobAmp + this.kick * 0.5;

    return { ads: this.ads };
  },

  getMuzzleWorld(target) {
    return this.models[this.current].muzzlePoint.getWorldPosition(target);
  },

  /* HUD ammo refresh */
  hudAmmo() {
    const st = this.st();
    return { mag: st.mag, reserve: st.reserve, name: this.def().name, reloading: this.reloading > 0 };
  },
};
