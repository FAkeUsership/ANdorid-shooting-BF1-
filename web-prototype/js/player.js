/* ============================================================
   IRONFRONT 1917 — player.js
   First-person controller: WASD + mouselook (pointer lock),
   full touch controls (virtual joystick + look area + buttons),
   sprint, jump, head-bob, damage & regen.
   ============================================================ */
'use strict';

window.Player = {
  pos: new THREE.Vector3(0, 0, 46),
  vel: new THREE.Vector3(),
  yaw: Math.PI, pitch: 0, pitchKick: 0,
  hp: 100, lastHitT: -99,
  grounded: true, jumpQueued: false,
  sprintT: 0, bobPhase: 0, stepT: 0,
  adsTarget: 0,
  isTouch: ('ontouchstart' in window) || (typeof navigator !== 'undefined' && navigator.maxTouchPoints > 0),

  /* input state */
  keys: {},
  fire: { held: false, fresh: false },
  adsHeld: false, adsToggle: false,
  moveInput: { x: 0, y: 0 },          // from keyboard or touch stick
  lookDelta: { x: 0, y: 0 },          // accumulated mouse/touch look

  reset(x, z, yaw) {
    this.pos.set(x, World.heightAt(x, z), z);
    this.vel.set(0, 0, 0);
    this.yaw = yaw; this.pitch = 0; this.pitchKick = 0;
    this.hp = CONFIG.player.maxHp; this.lastHitT = -99;
    this.grounded = true; this.sprintT = 0;
    this.fire.held = false; this.fire.fresh = false;
    this.adsHeld = false; this.adsToggle = false;
    this.moveInput.x = 0; this.moveInput.y = 0;
  },

  /* ================= INPUT BINDINGS ================= */
  bindInput() {
    addEventListener('keydown', e => {
      if (e.repeat) return;
      this.keys[e.code] = true;
      if (Game.state !== 'playing') return;
      if (e.code === 'KeyR') Weapons.reload();
      if (e.code === 'KeyG') Game.throwGrenade();
      if (e.code === 'Digit1') Weapons.switchWeapon('rifle');
      if (e.code === 'Digit2') Weapons.switchWeapon('pistol');
      if (e.code === 'Digit3') Weapons.switchWeapon('smg');
      if (e.code === 'KeyQ') Weapons.nextWeapon();
      if (e.code === 'Space') { this.jumpQueued = true; e.preventDefault(); }
    });
    addEventListener('keyup', e => { this.keys[e.code] = false; });

    addEventListener('mousemove', e => {
      if (document.pointerLockElement === Game.canvas) {
        this.lookDelta.x += e.movementX;
        this.lookDelta.y += e.movementY;
      }
    });
    addEventListener('mousedown', e => {
      if (Game.state !== 'playing' || document.pointerLockElement !== Game.canvas) return;
      if (e.button === 0) { this.fire.held = true; this.fire.fresh = true; }
      if (e.button === 2) this.adsHeld = true;
    });
    addEventListener('mouseup', e => {
      if (e.button === 0) this.fire.held = false;
      if (e.button === 2) this.adsHeld = false;
    });
    Game.canvas.addEventListener('contextmenu', e => e.preventDefault());

    // Unified pointer controls (touch AND mouse-drag fallback for when
    // pointer lock is unavailable, e.g. inside embedded iframes).
    this._bindPointerControls();
  },

  /* ---------------- unified pointer controls ---------------- */
  _bindPointerControls() {
    const stick = { id: null, bx: 0, by: 0 };
    const look = { id: null, lx: 0, ly: 0 };

    const moveZone = document.getElementById('joy-move-zone');
    const lookZone = document.getElementById('look-zone');
    const base = document.getElementById('joy-move-base');
    const knob = base.querySelector('.joy-knob');

    moveZone.addEventListener('pointerdown', e => {
      e.preventDefault();
      if (stick.id !== null || (e.pointerType === 'mouse' && e.button !== 0)) return;
      stick.id = e.pointerId; stick.bx = e.clientX; stick.by = e.clientY;
      base.style.display = 'block';
      base.style.left = e.clientX + 'px'; base.style.top = e.clientY + 'px';
      try { moveZone.setPointerCapture(e.pointerId); } catch (err) {}
    });
    moveZone.addEventListener('pointermove', e => {
      if (e.pointerId !== stick.id) return;
      let dx = e.clientX - stick.bx, dy = e.clientY - stick.by;
      const L = Math.sqrt(dx * dx + dy * dy), max = 52;
      if (L > max) { dx = dx / L * max; dy = dy / L * max; }
      knob.style.transform = `translate(calc(-50% + ${dx}px), calc(-50% + ${dy}px))`;
      this.moveInput.x = dx / max;
      this.moveInput.y = -dy / max;
    });
    const stickEnd = e => {
      if (e.pointerId !== stick.id) return;
      stick.id = null; this.moveInput.x = 0; this.moveInput.y = 0;
      base.style.display = 'none'; knob.style.transform = 'translate(-50%,-50%)';
    };
    moveZone.addEventListener('pointerup', stickEnd);
    moveZone.addEventListener('pointercancel', stickEnd);

    lookZone.addEventListener('pointerdown', e => {
      e.preventDefault();
      if (look.id !== null || (e.pointerType === 'mouse' && e.button !== 0)) return;
      look.id = e.pointerId; look.lx = e.clientX; look.ly = e.clientY;
      if (Game.state === 'playing') { this.fire.fresh = true; }   // tap/click = one shot
      try { lookZone.setPointerCapture(e.pointerId); } catch (err) {}
    });
    lookZone.addEventListener('pointermove', e => {
      if (e.pointerId !== look.id) return;
      this.lookDelta.x += (e.clientX - look.lx) * 1.6;
      this.lookDelta.y += (e.clientY - look.ly) * 1.6;
      look.lx = e.clientX; look.ly = e.clientY;
      this.fire.held = true;                                      // dragging = keep firing (auto weapons)
    });
    const lookEnd = e => {
      if (e.pointerId !== look.id) return;
      look.id = null; this.fire.held = false;
    };
    lookZone.addEventListener('pointerup', lookEnd);
    lookZone.addEventListener('pointercancel', lookEnd);

    const bind = (id, down, up) => {
      const el = document.getElementById(id);
      el.addEventListener('pointerdown', e => { e.preventDefault(); down(); el.classList.add('active'); });
      el.addEventListener('pointerup', e => { e.preventDefault(); if (up) up(); el.classList.remove('active'); });
      el.addEventListener('pointerleave', () => el.classList.remove('active'));
    };
    bind('btn-fire', () => { this.fire.held = true; this.fire.fresh = true; }, () => { this.fire.held = false; });
    bind('btn-ads', () => { this.adsToggle = !this.adsToggle; });
    bind('btn-reload', () => Weapons.reload());
    bind('btn-grenade', () => Game.throwGrenade());
    bind('btn-swap', () => Weapons.nextWeapon());
    bind('btn-jump', () => { this.jumpQueued = true; });
    const pb = document.getElementById('pause-btn');
    pb.addEventListener('pointerdown', e => { e.preventDefault(); Game.pause(); });
  },

  /* ================= DAMAGE / REGEN ================= */
  damage(amount, fromPos) {
    if (Game.state !== 'playing') return;
    this.hp -= amount;
    this.lastHitT = Game.time;
    SFX.hurt();
    Game.damageFlash();
    if (fromPos) Game.showDamageDir(fromPos);
    if (this.hp <= 0) { this.hp = 0; Game.onPlayerDeath(); }
  },

  /* ================= PER-FRAME ================= */
  update(dt) {
    const cfg = CONFIG.player;

    /* ---- look ---- */
    const sens = Game.settings.sens * (this.isTouch ? 0.0042 : 0.0021);
    this.yaw -= this.lookDelta.x * sens;
    const inv = Game.settings.invertY ? -1 : 1;
    this.pitch -= this.lookDelta.y * sens * inv;
    this.pitch = U.clamp(this.pitch, -1.5, 1.5);
    this.lookDelta.x = 0; this.lookDelta.y = 0;
    // recoil recovery
    this.pitchKick = U.dampCur(this.pitchKick, 0, 8, dt);

    /* ---- move input ---- */
    let ix = this.moveInput.x, iy = this.moveInput.y;
    if (this.keys.KeyW || this.keys.ArrowUp) iy += 1;
    if (this.keys.KeyS || this.keys.ArrowDown) iy -= 1;
    if (this.keys.KeyA || this.keys.ArrowLeft) ix -= 1;
    if (this.keys.KeyD || this.keys.ArrowRight) ix += 1;
    const iL = Math.sqrt(ix * ix + iy * iy);
    if (iL > 1) { ix /= iL; iy /= iL; }

    const ads = Weapons.ads;
    // desktop: hold SHIFT to sprint · touch stick: full tilt sprints automatically
    const shiftHeld = this.keys.ShiftLeft || this.keys.ShiftRight;
    const stickFull = this.isTouch && iL > 0.97;
    const wantSprint = (shiftHeld || stickFull) && iy > 0.2 && ads < 0.3;
    this.sprintT = U.dampCur(this.sprintT, wantSprint && iL > 0.1 ? 1 : 0, wantSprint ? 6 : 9, dt);
    const maxSpeed = U.lerp(cfg.walkSpeed, cfg.sprintSpeed, this.sprintT) * (1 - ads * 0.45);

    // world-space wish direction
    const fx = -Math.sin(this.yaw), fz = -Math.cos(this.yaw);
    const rx = Math.cos(this.yaw), rz = -Math.sin(this.yaw);
    let wx = fx * iy + rx * ix, wz = fz * iy + rz * ix;

    // accel / friction
    if (iL > 0.05) {
      this.vel.x += wx * cfg.accel * dt;
      this.vel.z += wz * cfg.accel * dt;
    } else {
      this.vel.x -= this.vel.x * Math.min(1, cfg.friction * dt);
      this.vel.z -= this.vel.z * Math.min(1, cfg.friction * dt);
    }
    const hv = Math.sqrt(this.vel.x * this.vel.x + this.vel.z * this.vel.z);
    if (hv > maxSpeed) { this.vel.x *= maxSpeed / hv; this.vel.z *= maxSpeed / hv; }

    /* ---- vertical ---- */
    if (this.jumpQueued && this.grounded && Game.state === 'playing') { this.vel.y = cfg.jumpVel; this.grounded = false; }
    this.jumpQueued = false;
    if (!this.grounded) this.vel.y -= cfg.gravity * dt;

    this.pos.x += this.vel.x * dt;
    this.pos.z += this.vel.z * dt;
    this.pos.y += this.vel.y * dt;

    const groundY = World.heightAt(this.pos.x, this.pos.z);
    if (this.pos.y <= groundY) {
      if (!this.grounded && this.vel.y < -6) SFX.footstep();
      this.pos.y = groundY; this.vel.y = 0; this.grounded = true;
    }

    /* ---- collisions: prop cylinders + map border ---- */
    for (const c of World.colliders) {
      const dx = this.pos.x - c.x, dz = this.pos.z - c.z;
      const d2 = dx * dx + dz * dz, rr = c.r + 0.45;
      if (d2 < rr * rr && d2 > 0.0001) {
        const d = Math.sqrt(d2);
        this.pos.x = c.x + dx / d * rr;
        this.pos.z = c.z + dz / d * rr;
      }
    }
    const cr = Math.sqrt(this.pos.x * this.pos.x + this.pos.z * this.pos.z);
    Game.outOfBounds = cr > CONFIG.mapRadius - 8;
    if (cr > CONFIG.mapRadius) {
      this.pos.x *= CONFIG.mapRadius / cr;
      this.pos.z *= CONFIG.mapRadius / cr;
    }

    /* ---- head bob + footsteps ---- */
    const speedNow = Math.sqrt(this.vel.x * this.vel.x + this.vel.z * this.vel.z);
    if (this.grounded && speedNow > 0.6) {
      this.bobPhase += speedNow * dt * 1.55;
      this.stepT -= speedNow * dt;
      if (this.stepT <= 0) { SFX.footstep(); this.stepT = 2.6; }
    }
    const bobY = Math.sin(this.bobPhase * 2) * 0.032 * (speedNow / cfg.walkSpeed) * (this.grounded ? 1 : 0);
    const bobX = Math.cos(this.bobPhase) * 0.02 * (speedNow / cfg.walkSpeed) * (this.grounded ? 1 : 0);

    /* ---- regen ---- */
    if (Game.time - this.lastHitT > cfg.regenDelay && this.hp < cfg.maxHp && this.hp > 0) {
      this.hp = Math.min(cfg.maxHp, this.hp + cfg.regenRate * dt);
    }

    /* ---- weapon ADS / fire ---- */
    this.adsTarget = (this.adsHeld || this.adsToggle) && this.sprintT < 0.4 ? 1 : 0;
    if (Game.state === 'playing') {
      const fired = Weapons.tryFire(this.fire);
      if (fired) Game.playerShoot(fired);
      this.fire.fresh = false;
    }

    /* ---- apply to camera ---- */
    const cam = Game.camera;
    cam.rotation.order = 'YXZ';
    cam.rotation.y = this.yaw;
    cam.rotation.x = this.pitch + this.pitchKick;
    cam.rotation.z = 0;
    cam.position.set(this.pos.x + bobX * Math.cos(this.yaw), this.pos.y + cfg.eyeHeight + bobY, this.pos.z - bobX * Math.sin(this.yaw));
    // screen shake
    if (Game.shake > 0.001) {
      cam.position.x += U.rand(-1, 1) * Game.shake * 0.12;
      cam.position.y += U.rand(-1, 1) * Game.shake * 0.12;
      cam.rotation.z += U.rand(-1, 1) * Game.shake * 0.01;
    }

    /* ---- FOV ---- */
    const def = Weapons.def();
    const adsFov = CONFIG.fov[def.adsFov] || 55;
    const targetFov = U.lerp(U.lerp(CONFIG.fov.hip, CONFIG.fov.sprint, this.sprintT), adsFov, Weapons.ads);
    if (Math.abs(cam.fov - targetFov) > 0.05) {
      cam.fov = U.dampCur(cam.fov, targetFov, 10, dt);
      cam.updateProjectionMatrix();
    }
  },
};
