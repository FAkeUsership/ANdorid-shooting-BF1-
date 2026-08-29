/* ============================================================
   IRONFRONT 1917 — characters.js
   Low-poly, flat-shaded humanoid builder — Walking-Zombie-2
   style proportions (stocky torso, slightly big head, simple
   painted-on face). Every enemy gets its own cloned materials
   so we can flash them red when hit without affecting others.
   Returns a rig the AI (enemies.js) animates procedurally.
   ============================================================ */
'use strict';

window.Characters = {
  faceCache: {},

  /* -------- painted face texture (canvas → texture) -------- */
  faceTexture(type) {
    if (this.faceCache[type]) return this.faceCache[type];
    const c = document.createElement('canvas'); c.width = 64; c.height = 64;
    const g = c.getContext('2d');
    g.clearRect(0, 0, 64, 64);
    if (type === 'soldier') {
      g.fillStyle = '#14140f';
      g.fillRect(16, 24, 9, 7); g.fillRect(39, 24, 9, 7);         // hollow eyes
      g.fillStyle = '#2a2a20'; g.fillRect(24, 44, 16, 5);        // grim mouth
      g.fillStyle = 'rgba(20,20,14,0.35)'; g.fillRect(12, 18, 40, 4); // brow shadow
    } else {
      const eye = type === 'runner' ? '#ffb03a' : '#e8e055';
      g.fillStyle = eye;
      g.fillRect(15, 23, 10, 8); g.fillRect(39, 23, 10, 8);      // glowing eyes
      g.fillStyle = '#0c0d08';
      g.fillRect(18, 26, 4, 3); g.fillRect(42, 26, 4, 3);        // pupils
      g.fillStyle = '#1a1008';
      g.fillRect(22, 42, 20, 9);                                  // gaping mouth
      g.fillStyle = '#3a2a1a';
      g.fillRect(24, 44, 3, 5); g.fillRect(31, 43, 3, 6); g.fillRect(38, 44, 3, 5); // teeth gaps
      if (type === 'runner') { g.fillStyle = 'rgba(120,30,20,0.6)'; g.fillRect(10, 14, 44, 5); }
    }
    const tex = new THREE.CanvasTexture(c);
    tex.magFilter = THREE.NearestFilter;
    this.faceCache[type] = tex;
    return tex;
  },

  /* ================= MAIN BUILDER =================
     type: 'zombie' | 'runner' | 'soldier'
     returns { group, mats, parts, muzzle? }               */
  build(type) {
    const cfg = ENEMY_TYPES[type];
    const runner = type === 'runner', soldier = type === 'soldier';

    // palette — zombies in decayed field-grey, soldiers in dark slate
    const skinC  = soldier ? 0x8a8372 : (runner ? 0x77835c : 0x7d8b66);
    const clothC = soldier ? 0x4a4f4a : (runner ? 0x5c5a40 : 0x565b46);
    const darkC  = 0x2e2c24;

    const skinM  = new THREE.MeshLambertMaterial({ color: skinC, flatShading: true });
    const clothM = new THREE.MeshLambertMaterial({ color: clothC, flatShading: true });
    const darkM  = new THREE.MeshLambertMaterial({ color: darkC, flatShading: true });
    const mats = { skin: skinM, cloth: clothM, dark: darkM };
    const parts = {};

    const root = new THREE.Group();

    /* ---- legs (pivot at hip, y=0.9) ---- */
    const legGeo = new THREE.BoxGeometry(0.24, 0.9, 0.24);
    legGeo.translate(0, -0.45, 0);
    const mkLeg = (side) => {
      const leg = new THREE.Mesh(legGeo, clothM);
      leg.position.set(0.17 * side, 0.9, 0);
      const boot = new THREE.Mesh(new THREE.BoxGeometry(0.26, 0.16, 0.34), darkM);
      boot.position.set(0, -0.85, 0.04);
      leg.add(boot);
      root.add(leg);
      return leg;
    };
    parts.legL = mkLeg(-1); parts.legR = mkLeg(1);

    /* ---- spine (everything above hips; lets us hunch zombies) ---- */
    const spine = new THREE.Group();
    spine.position.y = 0.9;
    root.add(spine);
    parts.spine = spine;

    /* ---- torso ---- */
    const torso = new THREE.Mesh(new THREE.BoxGeometry(runner ? 0.5 : 0.66, 0.74, 0.36), clothM);
    torso.position.y = 0.42;
    spine.add(torso);
    parts.torso = torso;
    // belt + chest straps = instant WWI silhouette
    const belt = new THREE.Mesh(new THREE.BoxGeometry(runner ? 0.54 : 0.7, 0.1, 0.4), darkM);
    belt.position.y = 0.12; spine.add(belt);
    const strap = new THREE.Mesh(new THREE.BoxGeometry(0.12, 0.6, 0.4), darkM);
    strap.position.set(-0.16, 0.45, 0); strap.rotation.z = 0.18; spine.add(strap);

    /* ---- arms (pivot at shoulder) ---- */
    const armGeo = new THREE.BoxGeometry(0.19, 0.66, 0.19);
    armGeo.translate(0, -0.3, 0);
    const mkArm = (side) => {
      const arm = new THREE.Mesh(armGeo, clothM);
      arm.position.set((runner ? 0.33 : 0.42) * side, 0.72, 0);
      const hand = new THREE.Mesh(new THREE.BoxGeometry(0.17, 0.17, 0.17), skinM);
      hand.position.set(0, -0.68, 0);
      arm.add(hand);
      // rolled sleeve cuff
      const cuff = new THREE.Mesh(new THREE.BoxGeometry(0.22, 0.1, 0.22), darkM);
      cuff.position.set(0, -0.5, 0); arm.add(cuff);
      spine.add(arm);
      return arm;
    };
    parts.armL = mkArm(-1); parts.armR = mkArm(1);

    /* ---- head group ---- */
    const headG = new THREE.Group();
    headG.position.y = 0.94;
    spine.add(headG);
    parts.head = headG;
    const head = new THREE.Mesh(new THREE.BoxGeometry(0.4, 0.42, 0.38), skinM);
    head.position.y = 0.16;
    headG.add(head);
    // face
    const face = new THREE.Mesh(new THREE.PlaneGeometry(0.34, 0.36),
      new THREE.MeshBasicMaterial({ map: this.faceTexture(type), transparent: true }));
    face.position.set(0, 0.16, 0.195);
    headG.add(face);

    if (soldier) {
      // stahlhelm
      const helm = new THREE.Mesh(new THREE.CylinderGeometry(0.26, 0.3, 0.2, 7), darkM);
      helm.position.y = 0.36; headG.add(helm);
      const brim = new THREE.Mesh(new THREE.CylinderGeometry(0.33, 0.35, 0.06, 7), darkM);
      brim.position.y = 0.28; headG.add(brim);
    } else {
      // messy hair / torn cap
      const hair = new THREE.Mesh(new THREE.BoxGeometry(0.42, 0.12, 0.4), darkM);
      hair.position.set(U.rand(-0.03, 0.03), 0.38, U.rand(-0.03, 0.03));
      hair.rotation.z = U.rand(-0.12, 0.12);
      headG.add(hair);
    }

    /* ---- soldier rifle + muzzle point ---- */
    let muzzle = null;
    if (soldier) {
      const rifle = new THREE.Group();
      const wood = new THREE.MeshLambertMaterial({ color: 0x5c4126, flatShading: true });
      const metal = new THREE.MeshLambertMaterial({ color: 0x2b2b2e, flatShading: true });
      const stock = new THREE.Mesh(new THREE.BoxGeometry(0.09, 0.12, 0.55), wood); stock.position.z = 0.16;
      const barrel = new THREE.Mesh(new THREE.BoxGeometry(0.05, 0.05, 0.85), metal); barrel.position.z = -0.38;
      const band = new THREE.Mesh(new THREE.BoxGeometry(0.08, 0.1, 0.1), wood); band.position.z = -0.12;
      rifle.add(stock, barrel, band);
      rifle.position.set(0.22, 0.5, -0.15);
      spine.add(rifle);
      muzzle = new THREE.Object3D();
      muzzle.position.set(0, 0, -0.82);
      rifle.add(muzzle);
    }

    /* ---- zombie damage detail: exposed wound blocks ---- */
    if (!soldier) {
      const wound = new THREE.Mesh(new THREE.BoxGeometry(0.16, 0.2, 0.06),
        new THREE.MeshLambertMaterial({ color: 0x6e2418, flatShading: true }));
      wound.position.set(U.rand(-0.18, 0.18), U.rand(0.3, 0.6), 0.19);
      spine.add(wound);
    }

    root.traverse(o => { if (o.isMesh) o.castShadow = true; });

    return { group: root, mats, parts, muzzle, type };
  },
};
