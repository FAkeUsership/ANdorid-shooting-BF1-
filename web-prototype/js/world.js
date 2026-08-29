/* ============================================================
   IRONFRONT 1917 — world.js
   Builds the WWI battlefield in Walking-Zombie-2 style:
   low-poly, flat-shaded, vertex-colored, no textures.
   Everything static uses InstancedMesh where possible
   (sandbags, posts, rocks, planks, grass) so phones stay fast.
   ============================================================ */
'use strict';

window.World = {
  group: null, terrain: null,
  colliders: [],        // {x,z,r} cylinders the player/enemies push against
  craters: [],          // {x,z,r,d}
  trenches: [],         // {x1,z1,x2,z2,w,d}
  spawnZones: {},       // filled in build()
  clouds: [], wisps: [], sunSprite: null,

  /* ---------- analytic terrain height (shared by everything) ---------- */
  baseNoise(x, z) {
    return Math.sin(x * 0.045) * Math.cos(z * 0.038) * 1.5
         + Math.sin(x * 0.11 + z * 0.07) * 0.45
         + Math.cos(x * 0.021 - z * 0.05) * 0.9;
  },
  heightAt(x, z) {
    let y = this.baseNoise(x, z);
    // craters
    for (let i = 0; i < this.craters.length; i++) {
      const c = this.craters[i];
      const dx = x - c.x, dz = z - c.z;
      const dist = Math.sqrt(dx * dx + dz * dz);
      if (dist < c.r) { const t = 1 - dist / c.r; y -= c.d * U.smooth(t); }
      else if (dist < c.r * 1.4) { const t = 1 - (dist - c.r) / (c.r * 0.4); y += c.d * 0.16 * U.smooth(t); }
    }
    // trenches (carved lines)
    for (let i = 0; i < this.trenches.length; i++) {
      const tr = this.trenches[i];
      const d = this._segDist(x, z, tr.x1, tr.z1, tr.x2, tr.z2);
      if (d < tr.w) y -= tr.d * U.smooth(1 - d / tr.w);
      else if (d < tr.w * 1.5) y += tr.d * 0.2 * U.smooth(1 - (d - tr.w) / (tr.w * 0.5));
    }
    // flatten friendly start area a little
    const ds = Math.sqrt(U.dist2(x, z, 0, 46));
    if (ds < 14) { const f = U.smooth(U.clamp(ds / 14, 0, 1)); y = U.lerp(this.baseNoise(0, 46) * 0.4, y, f); }
    return y;
  },
  _segDist(px, pz, x1, z1, x2, z2) {
    const dx = x2 - x1, dz = z2 - z1;
    const L2 = dx * dx + dz * dz;
    let t = L2 ? ((px - x1) * dx + (pz - z1) * dz) / L2 : 0;
    t = U.clamp(t, 0, 1);
    return Math.sqrt(U.dist2(px, pz, x1 + dx * t, z1 + dz * t));
  },

  /* ================= BUILD ================= */
  build(scene, quality) {
    this.group = new THREE.Group();
    scene.add(this.group);
    this.colliders = []; this.craters = []; this.trenches = [];

    this._makeCraterLayout();
    this._makeTrenchLayout();
    this._buildTerrain();
    this._buildSandbags();
    this._buildDuckboards();
    this._buildWire();
    this._buildTrees();
    this._buildRocksAndDebris(quality);
    this._buildGrass();
    this._buildHouses();
    this._buildChurch();
    this._buildSky(scene, quality);
    this._makeSpawnZones();
    return this.group;
  },

  /* ---------- layouts ---------- */
  _makeCraterLayout() {
    const spots = [];
    for (let i = 0; i < 26; i++) {
      const x = U.rand(-80, 80), z = U.rand(-75, 30);
      if (U.dist2(x, z, 0, 46) < 400) continue;             // keep start clear
      if (U.dist2(x, z, -85, -85) < 324) continue;           // keep church clear
      let ok = true;
      for (const s of spots) if (U.dist2(x, z, s.x, s.z) < 130) { ok = false; break; }
      if (!ok) continue;
      spots.push({ x, z, r: U.rand(2.6, 5.2), d: U.rand(1.1, 2.0) });
    }
    this.craters = spots;
  },
  _makeTrenchLayout() {
    // main friendly line + short returns (zig-zag feel)
    this.trenches = [
      { x1: -34, z1: 42, x2: 34, z2: 42, w: 1.6, d: 1.7 },
      { x1: -34, z1: 42, x2: -40, z2: 50, w: 1.4, d: 1.6 },
      { x1: 34, z1: 42, x2: 41, z2: 49, w: 1.4, d: 1.6 },
      { x1: 0, z1: 42, x2: 4, z2: 52, w: 1.4, d: 1.6 },
      // enemy-side broken trench fragment
      { x1: -58, z1: -48, x2: -18, z2: -52, w: 1.5, d: 1.4 },
    ];
  },

  /* ---------- terrain mesh ---------- */
  _buildTerrain() {
    const size = 380, seg = 104;
    const geo = new THREE.PlaneGeometry(size, size, seg, seg);
    geo.rotateX(-Math.PI / 2);
    const pos = geo.attributes.position;
    const colors = new Float32Array(pos.count * 3);
    const cMudA = new THREE.Color(0x58452f), cMudB = new THREE.Color(0x6d5338);
    const cWet = new THREE.Color(0x362a20), cGrass = new THREE.Color(0x56683a);
    const tmp = new THREE.Color();
    for (let i = 0; i < pos.count; i++) {
      const x = pos.getX(i), z = pos.getZ(i);
      const y = this.heightAt(x, z);
      pos.setY(i, y);
      // colour: mud blend + grass patches + dark wet craters
      const n = Math.sin(x * 0.08 + 3.1) * Math.cos(z * 0.075 - 1.2);
      tmp.copy(cMudA).lerp(cMudB, U.clamp(n * 0.5 + 0.5 + U.rand(-0.06, 0.06), 0, 1));
      const gp = Math.sin(x * 0.05 + 1.7) * Math.sin(z * 0.043 - 0.6);
      if (gp > 0.45) tmp.lerp(cGrass, U.clamp((gp - 0.45) * 1.8, 0, 0.7));
      let inCrater = false;
      for (const c of this.craters) if (U.dist2(x, z, c.x, c.z) < c.r * c.r) { inCrater = true; break; }
      if (inCrater) tmp.lerp(cWet, 0.65);
      colors[i * 3] = tmp.r; colors[i * 3 + 1] = tmp.g; colors[i * 3 + 2] = tmp.b;
    }
    geo.setAttribute('color', new THREE.Float32BufferAttribute(colors, 3));
    geo.computeVertexNormals();
    const mat = new THREE.MeshLambertMaterial({ vertexColors: true, flatShading: true });
    this.terrain = new THREE.Mesh(geo, mat);
    this.terrain.receiveShadow = true;
    this.group.add(this.terrain);
  },

  /* ---------- instanced sandbags ---------- */
  _buildSandbags() {
    const bagGeo = new THREE.BoxGeometry(0.92, 0.34, 0.5);
    // pinch the top slightly so bags look stuffed, not like bricks
    const p = bagGeo.attributes.position;
    for (let i = 0; i < p.count; i++) if (p.getY(i) > 0) p.setX(i, p.getX(i) * 0.82);
    bagGeo.computeVertexNormals();
    const bagMat = new THREE.MeshLambertMaterial({ color: 0xffffff, flatShading: true });

    const mats = [];
    const m4 = new THREE.Matrix4(), q = new THREE.Quaternion(), e = new THREE.Euler(), s = new THREE.Vector3();
    const addBag = (x, y, z, ry) => {
      e.set(0, ry + U.rand(-0.09, 0.09), U.rand(-0.05, 0.05));
      q.setFromEuler(e); s.set(U.rand(0.9, 1.12), U.rand(0.9, 1.15), U.rand(0.9, 1.1));
      m4.compose(new THREE.Vector3(x, y, z), q, s);
      mats.push(m4.clone());
    };
    // parapets along trench segments
    for (const tr of this.trenches) {
      const dx = tr.x2 - tr.x1, dz = tr.z2 - tr.z1;
      const len = Math.sqrt(dx * dx + dz * dz);
      const nx = -dz / len, nz = dx / len;         // normal
      const ang = Math.atan2(dx, dz) + Math.PI / 2;
      const n = Math.floor(len / 0.95);
      for (let side = -1; side <= 1; side += 2) {
        for (let i = 0; i <= n; i++) {
          const t = i / n;
          const bx = tr.x1 + dx * t + nx * (tr.w + 0.42) * side;
          const bz = tr.z1 + dz * t + nz * (tr.w + 0.42) * side;
          const by = this.heightAt(bx, bz);
          addBag(bx, by + 0.18, bz, ang);
          if (Math.random() < 0.62) addBag(bx + U.rand(-0.06, 0.06), by + 0.52, bz + U.rand(-0.06, 0.06), ang + U.rand(-0.15, 0.15));
          if (side === -1 && Math.random() < 0.3) addBag(bx, by + 0.86, bz, ang + U.rand(-0.2, 0.2));
        }
      }
    }
    // a few surface strongpoints / cover piles in no man's land
    const piles = [[14, 8], [-20, -6], [30, -25], [-8, -30], [-45, -20], [48, -5]];
    for (const [cx, cz] of piles) {
      const rings = U.randi(5, 8);
      for (let i = 0; i < rings; i++) {
        const a = U.rand(0, Math.PI * 2);
        addBag(cx + Math.cos(a) * U.rand(0.2, 1.5), this.heightAt(cx, cz) + 0.18, cz + Math.sin(a) * U.rand(0.2, 1.5), a);
      }
      this.colliders.push({ x: cx, z: cz, r: 1.9 });
    }
    const inst = new THREE.InstancedMesh(bagGeo, bagMat, mats.length);
    const col = new THREE.Color();
    mats.forEach((m, i) => {
      inst.setMatrixAt(i, m);
      col.setHSL(0.085, U.rand(0.3, 0.46), U.rand(0.3, 0.46));
      inst.setColorAt(i, col);
    });
    inst.instanceMatrix.needsUpdate = true;
    if (inst.instanceColor) inst.instanceColor.needsUpdate = true;
    inst.castShadow = true; inst.receiveShadow = true;
    this.group.add(inst);
  },

  /* ---------- trench floor boards ---------- */
  _buildDuckboards() {
    const geo = new THREE.BoxGeometry(1.7, 0.07, 0.5);
    const mat = new THREE.MeshLambertMaterial({ color: 0x6b5233, flatShading: true });
    const mats = []; const m4 = new THREE.Matrix4(), q = new THREE.Quaternion(), e = new THREE.Euler();
    for (const tr of this.trenches) {
      const dx = tr.x2 - tr.x1, dz = tr.z2 - tr.z1;
      const len = Math.sqrt(dx * dx + dz * dz);
      const ang = Math.atan2(dx, dz) + Math.PI / 2;
      const n = Math.floor(len / 0.62);
      for (let i = 0; i <= n; i++) {
        const t = i / n;
        const x = tr.x1 + dx * t + U.rand(-0.15, 0.15), z = tr.z1 + dz * t + U.rand(-0.15, 0.15);
        e.set(0, ang + U.rand(-0.12, 0.12), 0); q.setFromEuler(e);
        m4.compose(new THREE.Vector3(x, this.heightAt(x, z) + 0.06, z), q, new THREE.Vector3(1, 1, U.rand(0.8, 1.1)));
        mats.push(m4.clone());
      }
    }
    const inst = new THREE.InstancedMesh(geo, mat, mats.length);
    mats.forEach((m, i) => inst.setMatrixAt(i, m));
    inst.receiveShadow = true;
    this.group.add(inst);
  },

  /* ---------- barbed wire ---------- */
  _buildWire() {
    const rows = [
      { z: 16, x1: -75, x2: 75 }, { z: -18, x1: -80, x2: 80 },
      { z: -58, x1: -105, x2: -40 },
    ];
    const postGeo = new THREE.BoxGeometry(0.12, 1.25, 0.12);
    const postMat = new THREE.MeshLambertMaterial({ color: 0x4a3b28, flatShading: true });
    const pts = [];
    const postMats = [];
    const m4 = new THREE.Matrix4(), q = new THREE.Quaternion(), e = new THREE.Euler();
    for (const row of rows) {
      const n = Math.floor((row.x2 - row.x1) / 4.2);
      let prev = null;
      for (let i = 0; i <= n; i++) {
        const x = row.x1 + i * 4.2 + U.rand(-0.5, 0.5);
        const z = row.z + U.rand(-1.2, 1.2);
        const y = this.heightAt(x, z);
        e.set(U.rand(-0.14, 0.14), U.rand(0, Math.PI), U.rand(-0.14, 0.14)); q.setFromEuler(e);
        m4.compose(new THREE.Vector3(x, y + 0.6, z), q, new THREE.Vector3(1, 1, 1));
        postMats.push(m4.clone());
        // 3 wire strands to previous post
        if (prev) {
          for (let s = 0; s < 3; s++) {
            const h = 0.3 + s * 0.34;
            pts.push(prev.x, prev.y + h, prev.z, x, y + h + U.rand(-0.08, 0.08), z);
            pts.push(prev.x, prev.y + h, prev.z, x, y + h * 0.7, z); // sag cross
          }
        }
        prev = { x, y, z };
      }
    }
    const postInst = new THREE.InstancedMesh(postGeo, postMat, postMats.length);
    postMats.forEach((m, i) => postInst.setMatrixAt(i, m));
    this.group.add(postInst);
    const wGeo = new THREE.BufferGeometry();
    wGeo.setAttribute('position', new THREE.Float32BufferAttribute(pts, 3));
    const wire = new THREE.LineSegments(wGeo, new THREE.LineBasicMaterial({ color: 0x55534e, transparent: true, opacity: 0.75 }));
    this.group.add(wire);
  },

  /* ---------- dead trees ---------- */
  _buildTrees() {
    const trunkMat = new THREE.MeshLambertMaterial({ color: 0x3d3428, flatShading: true });
    for (let i = 0; i < 13; i++) {
      const x = U.rand(-110, 110), z = U.rand(-95, 60);
      if (U.dist2(x, z, 0, 46) < 300 || U.dist2(x, z, -85, -85) < 500) continue;
      const g = new THREE.Group();
      const h = U.rand(4.5, 8);
      const trunk = new THREE.Mesh(new THREE.CylinderGeometry(0.14, U.rand(0.3, 0.44), h, 5), trunkMat);
      trunk.position.y = h / 2;
      g.add(trunk);
      const nb = U.randi(2, 5);
      for (let b = 0; b < nb; b++) {
        const bl = U.rand(1.2, 2.8);
        const branch = new THREE.Mesh(new THREE.CylinderGeometry(0.05, 0.11, bl, 4), trunkMat);
        branch.position.y = U.rand(h * 0.45, h * 0.85);
        branch.rotation.z = U.rand(0.7, 1.5) * (Math.random() < 0.5 ? 1 : -1);
        branch.position.x = Math.sin(branch.rotation.z) * bl * 0.4;
        branch.position.y += Math.cos(branch.rotation.z) * bl * 0.3;
        g.add(branch);
      }
      g.position.set(x, this.heightAt(x, z) - 0.1, z);
      g.rotation.y = U.rand(0, Math.PI * 2);
      g.rotation.z = U.rand(-0.06, 0.06);
      this.group.add(g);
      this.colliders.push({ x, z, r: 0.5 });
    }
  },

  /* ---------- rocks & broken planks (instanced) ---------- */
  _buildRocksAndDebris(quality) {
    const n = quality === 'high' ? 90 : 55;
    const rockGeo = new THREE.IcosahedronGeometry(0.5, 0);
    const rockMat = new THREE.MeshLambertMaterial({ color: 0xffffff, flatShading: true });
    const rocks = new THREE.InstancedMesh(rockGeo, rockMat, n);
    const m4 = new THREE.Matrix4(), q = new THREE.Quaternion(), e = new THREE.Euler(), col = new THREE.Color();
    for (let i = 0; i < n; i++) {
      const x = U.rand(-115, 115), z = U.rand(-100, 65);
      const s = U.rand(0.25, 1.4);
      e.set(U.rand(0, 3), U.rand(0, 3), U.rand(0, 3)); q.setFromEuler(e);
      m4.compose(new THREE.Vector3(x, this.heightAt(x, z) + s * 0.2, z), q, new THREE.Vector3(s, s * U.rand(0.5, 0.8), s));
      rocks.setMatrixAt(i, m4);
      col.setHSL(0.09, U.rand(0.05, 0.14), U.rand(0.28, 0.44));
      rocks.setColorAt(i, col);
    }
    rocks.instanceColor.needsUpdate = true;
    rocks.castShadow = quality === 'high';
    this.group.add(rocks);

    // broken planks
    const pn = quality === 'high' ? 34 : 20;
    const plankGeo = new THREE.BoxGeometry(1.6, 0.06, 0.3);
    const plankMat = new THREE.MeshLambertMaterial({ color: 0x5d472c, flatShading: true });
    const planks = new THREE.InstancedMesh(plankGeo, plankMat, pn);
    for (let i = 0; i < pn; i++) {
      const x = U.rand(-100, 100), z = U.rand(-90, 55);
      e.set(U.rand(-0.2, 0.2), U.rand(0, Math.PI), U.rand(-0.25, 0.25)); q.setFromEuler(e);
      m4.compose(new THREE.Vector3(x, this.heightAt(x, z) + 0.08, z), q, new THREE.Vector3(U.rand(0.6, 1.3), 1, 1));
      planks.setMatrixAt(i, m4);
    }
    this.group.add(planks);
  },

  /* ---------- grass tufts (instanced cones) ---------- */
  _buildGrass() {
    const n = 170;
    const geo = new THREE.ConeGeometry(0.16, 0.5, 4);
    const mat = new THREE.MeshLambertMaterial({ color: 0x62713f, flatShading: true });
    const inst = new THREE.InstancedMesh(geo, mat, n);
    const m4 = new THREE.Matrix4(), q = new THREE.Quaternion(), e = new THREE.Euler();
    let placed = 0, guard = 0;
    while (placed < n && guard++ < 900) {
      const x = U.rand(-110, 110), z = U.rand(-95, 70);
      const gp = Math.sin(x * 0.05 + 1.7) * Math.sin(z * 0.043 - 0.6);
      if (gp < 0.3) continue;
      const s = U.rand(0.7, 1.7);
      e.set(U.rand(-0.2, 0.2), U.rand(0, 6.2), U.rand(-0.2, 0.2)); q.setFromEuler(e);
      m4.compose(new THREE.Vector3(x, this.heightAt(x, z) + 0.2 * s, z), q, new THREE.Vector3(s, s, s));
      inst.setMatrixAt(placed++, m4);
    }
    this.group.add(inst);
  },

  /* ---------- ruined farmhouses ---------- */
  _buildHouses() {
    const wallMat = new THREE.MeshLambertMaterial({ color: 0x8a7a62, flatShading: true });
    const roofMat = new THREE.MeshLambertMaterial({ color: 0x4f3a2a, flatShading: true });
    const woodMat = new THREE.MeshLambertMaterial({ color: 0x54402a, flatShading: true });
    const spots = [[26, -12, 0.4], [-30, -38, -0.8], [55, -55, 1.9]];
    for (const [hx, hz, rot] of spots) {
      const g = new THREE.Group();
      const gy = this.heightAt(hx, hz);
      const W = 7, D = 5.5, H = 3.4;
      const wall = (w, h, x, y, z, ry) => {
        const m = new THREE.Mesh(new THREE.BoxGeometry(w, h, 0.42), wallMat);
        m.position.set(x, y, z); m.rotation.y = ry || 0;
        m.castShadow = true; m.receiveShadow = true;
        g.add(m); return m;
      };
      // broken walls (jagged tops approximated with stacked blocks)
      wall(W, H * 0.8, 0, H * 0.4, -D / 2, 0);
      wall(W * 0.55, H * 0.62, -W * 0.22, H * 0.31, D / 2, 0);
      wall(D, H * 0.9, -W / 2, H * 0.45, 0, Math.PI / 2);
      wall(D, H * 0.5, W / 2, H * 0.25, 0, Math.PI / 2);
      const top = new THREE.Mesh(new THREE.BoxGeometry(W * 0.4, 0.8, 0.42), wallMat);
      top.position.set(W * 0.28, H * 0.65, -D / 2); top.rotation.z = 0.12; g.add(top);
      // collapsed roof beams
      for (let i = 0; i < 3; i++) {
        const beam = new THREE.Mesh(new THREE.BoxGeometry(0.22, 0.22, D * 1.15), roofMat);
        beam.position.set(U.rand(-W / 2.6, W / 2.6), H * U.rand(0.35, 0.7), U.rand(-0.5, 0.5));
        beam.rotation.x = U.rand(-0.7, -0.2); beam.rotation.y = U.rand(-0.2, 0.2);
        g.add(beam);
      }
      // rubble
      for (let i = 0; i < 6; i++) {
        const r = new THREE.Mesh(new THREE.IcosahedronGeometry(U.rand(0.3, 0.75), 0), woodMat);
        r.position.set(U.rand(-W / 2 - 1, W / 2 + 1), U.rand(0.1, 0.4), U.rand(-D / 2 - 1.5, D / 2 + 1.5));
        g.add(r);
      }
      g.position.set(hx, gy, hz); g.rotation.y = rot;
      this.group.add(g);
      this.colliders.push({ x: hx, z: hz, r: 3.4 });
    }
  },

  /* ---------- ruined church (mission 2 objective) ---------- */
  _buildChurch() {
    const g = new THREE.Group();
    const stone = new THREE.MeshLambertMaterial({ color: 0x7d7466, flatShading: true });
    const dark = new THREE.MeshLambertMaterial({ color: 0x5c554a, flatShading: true });
    const W = 9, D = 12, H = 7;
    const wall = (w, h, x, y, z, ry) => {
      const m = new THREE.Mesh(new THREE.BoxGeometry(w, h, 0.6), stone);
      m.position.set(x, y, z); m.rotation.y = ry || 0;
      m.castShadow = true; m.receiveShadow = true; g.add(m);
    };
    wall(W, H * 0.75, 0, H * 0.37, -D / 2, 0);                       // back wall
    wall(W * 0.42, H * 0.55, -W * 0.29, H * 0.27, D / 2, 0);         // front broken
    wall(W * 0.3, H * 0.4, W * 0.33, H * 0.2, D / 2, 0);
    wall(D, H * 0.66, -W / 2, H * 0.33, 0, Math.PI / 2);
    wall(D * 0.6, H * 0.5, W / 2, H * 0.25, -D * 0.15, Math.PI / 2);
    // tower
    wall(3.4, H * 1.5, 0, H * 0.75, -D / 2 - 2.4, 0);
    const spire = new THREE.Mesh(new THREE.ConeGeometry(2.2, 3.2, 4), dark);
    spire.position.set(0, H * 1.5 + 1.4, -D / 2 - 2.4); spire.rotation.y = Math.PI / 4;
    g.add(spire);
    // cross
    const cross = new THREE.Group();
    const c1 = new THREE.Mesh(new THREE.BoxGeometry(0.22, 1.7, 0.22), dark);
    const c2 = new THREE.Mesh(new THREE.BoxGeometry(0.95, 0.22, 0.22), dark);
    c2.position.y = 0.35; cross.add(c1, c2);
    cross.position.set(0, H * 1.5 + 3.6, -D / 2 - 2.4); cross.rotation.z = 0.08;
    g.add(cross);
    // rubble & fallen bell
    for (let i = 0; i < 8; i++) {
      const r = new THREE.Mesh(new THREE.IcosahedronGeometry(U.rand(0.35, 0.9), 0), stone);
      r.position.set(U.rand(-W, W), U.rand(0.1, 0.5), U.rand(-D, D * 0.8));
      g.add(r);
    }
    const bell = new THREE.Mesh(new THREE.CylinderGeometry(0.55, 0.75, 0.9, 7), new THREE.MeshLambertMaterial({ color: 0x6f6238, flatShading: true }));
    bell.position.set(3.2, 0.55, 4.5); bell.rotation.z = 1.2; g.add(bell);

    g.position.set(-85, this.heightAt(-85, -85), -85);
    g.rotation.y = 0.5;
    this.group.add(g);
    this.colliders.push({ x: -85, z: -85, r: 7.5 });
  },

  /* ---------- sky, clouds, hills, smoke ---------- */
  _buildSky(scene, quality) {
    // pale overcast sun
    const c = document.createElement('canvas'); c.width = c.height = 64;
    const g2 = c.getContext('2d');
    const grd = g2.createRadialGradient(32, 32, 2, 32, 32, 30);
    grd.addColorStop(0, 'rgba(255,246,220,0.95)'); grd.addColorStop(0.35, 'rgba(255,240,200,0.35)'); grd.addColorStop(1, 'rgba(255,240,200,0)');
    g2.fillStyle = grd; g2.fillRect(0, 0, 64, 64);
    const sunTex = new THREE.CanvasTexture(c);
    this.sunSprite = new THREE.Sprite(new THREE.SpriteMaterial({ map: sunTex, transparent: true, depthWrite: false, fog: false }));
    this.sunSprite.position.set(-160, 75, -150); this.sunSprite.scale.set(90, 90, 1);
    scene.add(this.sunSprite);

    // low-poly clouds
    const cloudMat = new THREE.MeshLambertMaterial({ color: 0xdfe0d4, flatShading: true, transparent: true, opacity: 0.92, fog: false });
    const nClouds = quality === 'high' ? 9 : 5;
    for (let i = 0; i < nClouds; i++) {
      const cg = new THREE.Group();
      const nb = U.randi(3, 5);
      for (let b = 0; b < nb; b++) {
        const s = U.rand(3, 7);
        const m = new THREE.Mesh(new THREE.IcosahedronGeometry(s, 0), cloudMat);
        m.position.set(U.rand(-8, 8), U.rand(-1.5, 1.5), U.rand(-4, 4));
        m.scale.y = 0.55;
        cg.add(m);
      }
      cg.position.set(U.rand(-220, 220), U.rand(55, 95), U.rand(-240, 60));
      scene.add(cg);
      this.clouds.push({ g: cg, v: U.rand(0.4, 1.1) });
    }

    // distant hill silhouettes
    const hillMat = new THREE.MeshLambertMaterial({ color: 0x4a4638, flatShading: true });
    for (let i = 0; i < 12; i++) {
      const a = (i / 12) * Math.PI * 2 + U.rand(-0.2, 0.2);
      const r = U.rand(200, 235);
      const h = U.rand(14, 34);
      const hill = new THREE.Mesh(new THREE.ConeGeometry(U.rand(34, 66), h, 5), hillMat);
      hill.position.set(Math.cos(a) * r, h * 0.28, Math.sin(a) * r);
      hill.rotation.y = U.rand(0, 3);
      scene.add(hill);
    }

    // drifting ground mist wisps
    const wispMat = new THREE.MeshBasicMaterial({ color: 0x9aa48e, transparent: true, opacity: 0.1, depthWrite: false, side: THREE.DoubleSide });
    for (let i = 0; i < 7; i++) {
      const w = new THREE.Mesh(new THREE.PlaneGeometry(U.rand(9, 17), U.rand(2.5, 4.5)), wispMat.clone());
      const x = U.rand(-90, 90), z = U.rand(-80, 60);
      w.position.set(x, this.heightAt(x, z) + U.rand(0.8, 1.6), z);
      w.material.opacity = U.rand(0.06, 0.13);
      scene.add(w);
      this.wisps.push({ m: w, v: U.rand(0.25, 0.7), phase: U.rand(0, 6.3) });
    }
  },

  /* ---------- spawn zones for enemies ---------- */
  _makeSpawnZones() {
    const front = this.craters.filter(c => c.z > -25).map(c => ({ x: c.x, z: c.z }));
    if (front.length < 4) front.push({ x: -20, z: 5 }, { x: 20, z: 0 }, { x: 0, z: -10 }, { x: -40, z: 10 });
    this.spawnZones.cratersFront = front;
    const all = this.craters.map(c => ({ x: c.x, z: c.z }));
    this.spawnZones.cratersAll = all.length ? all : front;
    const ridge = [];
    for (let i = 0; i < 8; i++) {
      const a = Math.PI + U.rand(-1.25, 1.25); // enemy side (negative z)
      const r = U.rand(62, 88);
      ridge.push({ x: Math.sin(a) * r, z: Math.cos(a) * r });
    }
    this.spawnZones.ridge = ridge;
    const church = [];
    for (let i = 0; i < 10; i++) {
      const a = U.rand(0, Math.PI * 2), r = U.rand(22, 48);
      let x = -85 + Math.cos(a) * r, z = -85 + Math.sin(a) * r;
      const cr = Math.hypot(x, z), maxR = CONFIG.mapRadius - 8;
      if (cr > maxR) { x *= maxR / cr; z *= maxR / cr; }
      church.push({ x, z });
    }
    this.spawnZones.church = church;
  },

  /* ---------- per-frame ambience updates ---------- */
  update(dt) {
    for (const c of this.clouds) {
      c.g.position.x += c.v * dt;
      if (c.g.position.x > 240) c.g.position.x = -240;
    }
    const t = performance.now() * 0.001;
    for (const w of this.wisps) {
      w.m.position.x += w.v * dt;
      w.m.position.y += Math.sin(t * 0.5 + w.phase) * 0.002;
      if (w.m.position.x > 110) w.m.position.x = -110;
      w.m.quaternion.copy(Game.camera.quaternion);
      w.m.material.opacity = 0.07 + Math.sin(t * 0.3 + w.phase) * 0.03;
    }
  },
};
