/* ============================================================
   IRONFRONT 1917 — enemies.js
   Enemy AI: procedural animation, steering (separation +
   obstacle avoidance), melee lunges, ranged fire, deaths.
   Models are pooled per type to keep GC pressure near zero.
   ============================================================ */
'use strict';

window.Enemies = {
  list: [],
  pools: {},          // type -> [rig, ...]
  hitMeshes: [],      // raycast targets for the player's bullets

  /* ---------------- spawn / recycle ---------------- */
  spawn(type, x, z) {
    const cfg = ENEMY_TYPES[type];
    let rig = (this.pools[type] || []).pop();
    if (!rig) rig = Characters.build(type);
    const e = {
      type, cfg, rig,
      x, z, y: World.heightAt(x, z),
      yaw: U.rand(0, Math.PI * 2),
      hp: cfg.hp, speed: U.rand(cfg.speedMin, cfg.speedMax),
      state: cfg.riseFromGround ? 'rising' : 'chase',
      stateT: 0, attackCd: U.rand(0.4, 1.2), animT: U.rand(0, 10),
      flashT: 0, alive: true, moanT: U.rand(1, 4),
      strafeDir: Math.random() < 0.5 ? 1 : -1,
      riseDur: cfg.riseFromGround ? U.rand(1.2, 1.7) : 0,
    };
    const g = rig.group;
    g.visible = true;
    g.position.set(x, cfg.riseFromGround ? e.y - 1.7 : e.y, z);
    g.rotation.set(0, e.yaw, 0);
    g.scale.set(1, 1, 1);
    rig.mats.skin.emissive.setRGB(0, 0, 0);
    rig.mats.cloth.emissive.setRGB(0, 0, 0);
    if (!g.parent) Game.scene.add(g);
    // register raycast meshes
    this._register(e);
    this.list.push(e);
    if (cfg.riseFromGround) { Game.spawnDirt(e.x, e.y + 0.1, e.z, 6, 0.6); SFX.moan(); }
    return e;
  },

  _register(e) {
    e.rig.group.traverse(o => {
      if (o.isMesh) {
        o.userData.enemy = e;
        o.userData.part = (o === e.rig.parts.head || o.parent === e.rig.parts.head || (o.parent && o.parent.parent === e.rig.parts.head)) ? 'head' : 'body';
        this.hitMeshes.push(o);
      }
    });
  },
  _unregister(e) {
    this.hitMeshes = this.hitMeshes.filter(m => m.userData.enemy !== e);
  },

  /* ---------------- damage ---------------- */
  damage(e, amount, part, point, dir) {
    if (!e.alive) return;
    const head = part === 'head';
    if (head) amount *= CONFIG.headshotMult;
    e.hp -= amount;
    e.flashT = 0.14;
    e.rig.mats.skin.emissive.setRGB(0.55, 0.06, 0.03);
    e.rig.mats.cloth.emissive.setRGB(0.3, 0.03, 0.02);
    if (point) Game.spawnBlood(point, dir, head ? 10 : 6);
    Game.hitmarker(head && e.hp <= 0);
    if (e.hp <= 0) this.kill(e, dir);
  },

  kill(e, dir) {
    e.alive = false;
    e.state = 'dead';
    e.stateT = 0;
    e.deathDir = dir ? Math.atan2(dir.x, dir.z) : e.yaw;
    this._unregister(e);
    SFX.enemyDie();
    Game.onEnemyKilled(e);
    Game.spawnBlood(new THREE.Vector3(e.x, e.y + 0.9, e.z), dir, 12);
    Game.addBloodPool(e.x, e.z);
  },

  explodeAt(x, z, radius, dmg) {
    for (const e of this.list) {
      if (!e.alive) continue;
      const d = Math.sqrt(U.dist2(e.x, e.z, x, z));
      if (d < radius) {
        const fall = 1 - (d / radius) * 0.75;
        this.damage(e, dmg * fall, 'body', new THREE.Vector3(e.x, e.y + 1, e.z),
          new THREE.Vector3(e.x - x, 0.3, e.z - z).normalize());
      }
    }
  },

  aliveCount() { let n = 0; for (const e of this.list) if (e.alive) n++; return n; },

  clearAll() {
    for (const e of this.list) this._recycle(e);
    this.list = [];
    this.hitMeshes = [];
  },

  _recycle(e) {
    this._unregister(e);
    e.rig.group.visible = false;
    (this.pools[e.type] = this.pools[e.type] || []).push(e.rig);
  },

  /* ---------------- steering helpers ---------------- */
  _steer(e, dt) {
    let dx = Player.pos.x - e.x, dz = Player.pos.z - e.z;
    const dist = Math.sqrt(dx * dx + dz * dz) || 0.001;
    let mx = dx / dist, mz = dz / dist;
    // separation from other enemies
    for (const o of this.list) {
      if (o === e || !o.alive) continue;
      const d2 = U.dist2(e.x, e.z, o.x, o.z);
      if (d2 < 1.7) {
        const d = Math.sqrt(d2) || 0.01;
        mx += (e.x - o.x) / d * 0.8; mz += (e.z - o.z) / d * 0.8;
      }
    }
    // obstacle avoidance (push tangent around colliders)
    for (const c of World.colliders) {
      const d2 = U.dist2(e.x, e.z, c.x, c.z);
      const rr = c.r + 0.55;
      if (d2 < rr * rr) {
        const d = Math.sqrt(d2) || 0.01;
        const push = (rr - d) / rr;
        mx += (e.x - c.x) / d * push * 1.6;
        mz += (e.z - c.z) / d * push * 1.6;
      }
    }
    const L = Math.sqrt(mx * mx + mz * mz) || 1;
    e.x += (mx / L) * e.speed * dt;
    e.z += (mz / L) * e.speed * dt;
    return dist;
  },

  /* ---------------- animation helpers ---------------- */
  _animZombie(e, dt, run) {
    const p = e.rig.parts;
    e.animT += dt * (run ? 11 : 6.5);
    const s = Math.sin(e.animT);
    p.legL.rotation.x = s * (run ? 0.9 : 0.55);
    p.legR.rotation.x = -s * (run ? 0.9 : 0.55);
    p.spine.rotation.x = run ? 0.34 : 0.24;
    p.spine.rotation.y = Math.sin(e.animT * 0.5) * 0.08;
    if (run) {
      p.armL.rotation.x = -1.1 + Math.sin(e.animT) * 0.5;
      p.armR.rotation.x = -1.1 - Math.sin(e.animT) * 0.5;
    } else {
      p.armL.rotation.x = -1.35 + Math.sin(e.animT * 0.7) * 0.12;
      p.armR.rotation.x = -1.35 + Math.cos(e.animT * 0.63) * 0.12;
      p.armL.rotation.z = 0.14; p.armR.rotation.z = -0.14;
    }
    p.head.rotation.z = Math.sin(e.animT * 0.4 + 1) * 0.16;
    p.head.rotation.x = 0.12;
  },
  _animSoldier(e, dt, moving) {
    const p = e.rig.parts;
    if (moving) {
      e.animT += dt * 6;
      const s = Math.sin(e.animT);
      p.legL.rotation.x = s * 0.6; p.legR.rotation.x = -s * 0.6;
      p.armL.rotation.x = -0.3 - s * 0.25;
      p.armR.rotation.x = -0.95;
      p.spine.rotation.x = 0.06;
    } else {
      // aiming stance
      p.legL.rotation.x *= 0.8; p.legR.rotation.x *= 0.8;
      p.armR.rotation.x = -1.45;
      p.armR.rotation.z = -0.12;
      p.armL.rotation.x = -1.15;
      p.armL.rotation.z = 0.35;
      p.spine.rotation.x = 0.04;
      p.head.rotation.x = -0.05;
    }
  },

  /* ---------------- main update ---------------- */
  update(dt) {
    const px = Player.pos.x, pz = Player.pos.z;
    for (let i = this.list.length - 1; i >= 0; i--) {
      const e = this.list[i];
      const g = e.rig.group;

      if (!e.alive) {
        /* ---- death: topple, sink, recycle ---- */
        e.stateT += dt;
        const t = Math.min(1, e.stateT / 0.45);
        g.rotation.x = -U.smooth(t) * Math.PI / 2 * (e.deathFlip || (e.deathFlip = Math.random() < 0.5 ? 1 : -1));
        g.rotation.y = e.deathDir;
        if (e.stateT > 4.5) {
          g.position.y -= dt * 0.8;
          if (e.stateT > 6.5) { this._recycle(e); this.list.splice(i, 1); continue; }
        }
        continue;
      }

      // hit-flash decay
      if (e.flashT > 0) {
        e.flashT -= dt;
        if (e.flashT <= 0) { e.rig.mats.skin.emissive.setRGB(0, 0, 0); e.rig.mats.cloth.emissive.setRGB(0, 0, 0); }
      }

      if (e.state === 'rising') {
        e.stateT += dt;
        const t = U.clamp(e.stateT / e.riseDur, 0, 1);
        g.position.y = e.y - 1.7 * (1 - U.smooth(t));
        if (Math.random() < 0.25) Game.spawnDirt(e.x + U.rand(-0.3, 0.3), g.position.y + 0.2, e.z + U.rand(-0.3, 0.3), 1, 0.4);
        if (t >= 1) { e.state = 'chase'; g.position.y = e.y; }
        continue;
      }

      const distToPlayer = Math.sqrt(U.dist2(e.x, e.z, px, pz));

      /* ---- melee (zombies / runners) ---- */
      if (e.cfg.melee) {
        if (e.state === 'chase') {
          const dist = this._steer(e, dt);
          e.y = World.heightAt(e.x, e.z);
          g.position.set(e.x, e.y, e.z);
          // face player
          const targetYaw = Math.atan2(px - e.x, pz - e.z);
          e.yaw += ((targetYaw - e.yaw + Math.PI * 3) % (Math.PI * 2) - Math.PI) * Math.min(1, dt * 8);
          g.rotation.y = e.yaw;
          this._animZombie(e, dt, e.type === 'runner');
          // moan
          e.moanT -= dt;
          if (e.moanT <= 0) { e.moanT = U.rand(2.5, 6); if (distToPlayer < 42) SFX.moan(); }
          if (dist < e.cfg.attackRange) { e.state = 'attack'; e.stateT = 0; }
        } else if (e.state === 'attack') {
          e.stateT += dt;
          const p = e.rig.parts;
          if (e.stateT < 0.32) {
            // wind-up: rear back
            p.spine.rotation.x = -0.15;
            p.armL.rotation.x = -2.4; p.armR.rotation.x = -2.4;
          } else if (e.stateT < 0.42) {
            // strike!
            p.spine.rotation.x = 0.5;
            p.armL.rotation.x = -0.6; p.armR.rotation.x = -0.6;
            if (!e.struck) {
              e.struck = true;
              SFX.zombieAttack();
              if (distToPlayer < e.cfg.attackRange + 0.7) {
                Player.damage(e.cfg.dmg, new THREE.Vector3(e.x, e.y + 1, e.z));
              }
            }
          } else {
            e.attackCd -= dt;
            if (distToPlayer > e.cfg.attackRange + 0.4) { e.state = 'chase'; e.struck = false; }
            else if (e.attackCd <= 0) { e.stateT = 0; e.struck = false; e.attackCd = e.cfg.attackRate * U.rand(0.8, 1.25); }
          }
          g.position.set(e.x, e.y, e.z);
        }
        continue;
      }

      /* ---- ranged (grey soldiers) ---- */
      e.attackCd -= dt;
      const wantRange = e.cfg.attackRange;
      if (distToPlayer > wantRange) {
        this._steer(e, dt);
        this._animSoldier(e, dt, true);
      } else if (distToPlayer < 8) {
        // back off
        let dx = (e.x - px), dz = (e.z - pz);
        const L = Math.sqrt(dx * dx + dz * dz) || 1;
        e.x += dx / L * e.speed * 0.7 * dt; e.z += dz / L * e.speed * 0.7 * dt;
        this._animSoldier(e, dt, true);
      } else {
        // hold + strafe a little + fire
        e.x += Math.cos(e.yaw) * e.strafeDir * 0.35 * dt;
        e.z -= Math.sin(e.yaw) * e.strafeDir * 0.35 * dt;
        this._animSoldier(e, dt, false);
        if (e.attackCd <= 0 && distToPlayer < wantRange + 6) {
          e.attackCd = e.cfg.attackRate * U.rand(0.75, 1.3);
          e.strafeDir = -e.strafeDir;
          this._soldierFire(e, distToPlayer);
        }
      }
      e.y = World.heightAt(e.x, e.z);
      g.position.set(e.x, e.y, e.z);
      const ty = Math.atan2(px - e.x, pz - e.z);
      e.yaw += ((ty - e.yaw + Math.PI * 3) % (Math.PI * 2) - Math.PI) * Math.min(1, dt * 6);
      g.rotation.y = e.yaw;
    }
  },

  _soldierFire(e, dist) {
    const from = new THREE.Vector3();
    if (e.rig.muzzle) e.rig.muzzle.getWorldPosition(from);
    else from.set(e.x, e.y + 1.3, e.z);
    SFX.soldierShot();
    Game.spawnMuzzleFlashAt(from);
    // recoil anim
    e.rig.parts.armR.rotation.x = -1.3;
    // accuracy: base chance, worse if player is sprinting / far
    let chance = e.cfg.accuracy;
    if (Player.sprintT > 0.3) chance *= 0.6;
    if (dist > 20) chance *= 0.75;
    const hit = Math.random() < chance;
    const target = new THREE.Vector3(Player.pos.x, Player.pos.y + CONFIG.player.eyeHeight * 0.9, Player.pos.z);
    if (!hit) {
      target.x += U.rand(-1.4, 1.4); target.y += U.rand(-0.9, 1.1); target.z += U.rand(-1.4, 1.4);
    }
    Game.spawnTracer(from, target, 0xffb36a, 0.06);
    if (hit) {
      setTimeout(() => { if (Game.state === 'playing') Player.damage(e.cfg.dmg, new THREE.Vector3(e.x, e.y + 1, e.z)); },
        U.clamp(dist / 60, 0.05, 0.3) * 1000);
    }
  },
};
