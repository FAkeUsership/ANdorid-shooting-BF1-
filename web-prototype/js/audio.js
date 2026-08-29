/* ============================================================
   IRONFRONT 1917 — audio.js
   Fully procedural WebAudio sound engine. No audio files.
   Everything is synthesized: gunshots, reloads, zombie moans,
   explosions, wind ambience, distant artillery.
   ============================================================ */
'use strict';

window.SFX = {
  ctx: null, master: null, noiseBuf: null, enabled: true,
  _lastGroan: 0, _windStarted: false,

  /* must be called from a user gesture */
  init() {
    if (this.ctx) { if (this.ctx.state === 'suspended') this.ctx.resume(); return; }
    const AC = window.AudioContext || window.webkitAudioContext;
    if (!AC) return;
    this.ctx = new AC();
    this.master = this.ctx.createGain();
    this.master.gain.value = 0.75;
    this.master.connect(this.ctx.destination);
    // cached white noise buffer
    const len = this.ctx.sampleRate * 2;
    this.noiseBuf = this.ctx.createBuffer(1, len, this.ctx.sampleRate);
    const d = this.noiseBuf.getChannelData(0);
    for (let i = 0; i < len; i++) d[i] = Math.random() * 2 - 1;
    this.startWind();
  },
  setEnabled(v) { this.enabled = v; if (this.master) this.master.gain.value = v ? 0.75 : 0; },
  ok() { return this.enabled && this.ctx; },

  _noise(dur) {
    const s = this.ctx.createBufferSource();
    s.buffer = this.noiseBuf; s.loop = true;
    s.start(); s.stop(this.ctx.currentTime + dur + 0.05);
    return s;
  },
  _env(peak, dur, attack) {
    const g = this.ctx.createGain();
    const t = this.ctx.currentTime;
    g.gain.setValueAtTime(0.0001, t);
    g.gain.linearRampToValueAtTime(peak, t + (attack || 0.004));
    g.gain.exponentialRampToValueAtTime(0.0001, t + dur);
    return g;
  },
  _filter(type, f, q) {
    const fl = this.ctx.createBiquadFilter();
    fl.type = type; fl.frequency.value = f; fl.Q.value = q || 0.8;
    return fl;
  },
  _osc(type, f) { const o = this.ctx.createOscillator(); o.type = type; o.frequency.value = f; return o; },

  /* ---------------- ambience ---------------- */
  startWind() {
    if (!this.ctx || this._windStarted) return;
    this._windStarted = true;
    const n = this._noise(999999);
    const lp = this._filter('lowpass', 320, 0.4);
    const g = this.ctx.createGain(); g.gain.value = 0.045;
    // slow LFO on gain for gusting wind
    const lfo = this._osc('sine', 0.08), lg = this.ctx.createGain(); lg.gain.value = 0.022;
    lfo.connect(lg); lg.connect(g.gain); lfo.start();
    n.connect(lp); lp.connect(g); g.connect(this.master);
  },

  /* ---------------- weapons ---------------- */
  shot(kind) {
    if (!this.ok()) return;
    const t = this.ctx.currentTime;
    if (kind === 'rifle') {
      const n = this._noise(0.22), bp = this._filter('bandpass', 1700, 0.55), g = this._env(0.9, 0.22);
      n.connect(bp); bp.connect(g); g.connect(this.master);
      const o = this._osc('sine', 95), og = this._env(0.5, 0.13);
      o.frequency.exponentialRampToValueAtTime(38, t + 0.13);
      o.connect(og); og.connect(this.master); o.start(); o.stop(t + 0.16);
      this._click(2400, 0.02, 0.3);
    } else if (kind === 'pistol') {
      const n = this._noise(0.1), bp = this._filter('bandpass', 2300, 0.7), g = this._env(0.55, 0.1);
      n.connect(bp); bp.connect(g); g.connect(this.master);
      this._click(2800, 0.015, 0.22);
    } else { // smg
      const n = this._noise(0.075), bp = this._filter('bandpass', U.rand(1500, 2100), 0.8), g = this._env(0.42, 0.075);
      n.connect(bp); bp.connect(g); g.connect(this.master);
    }
  },
  _click(freq, dur, vol) {
    const o = this._osc('square', freq), g = this._env(vol || 0.2, dur);
    o.connect(g); g.connect(this.master); o.start(); o.stop(this.ctx.currentTime + dur + 0.02);
  },
  dryFire() { if (!this.ok()) return; this._click(900, 0.04, 0.16); },
  reload(kind) {
    if (!this.ok()) return;
    const seq = kind === 'rifle' ? [[0.0, 1400], [0.12, 2100], [0.55, 900], [1.9, 1600], [2.05, 2400]]
              : kind === 'pistol' ? [[0.0, 1100], [0.5, 800], [1.25, 1900]]
              : [[0.0, 1200], [0.18, 1700], [1.6, 1500], [1.75, 2200]];
    seq.forEach(([dt, f]) => setTimeout(() => this._click(f, 0.035, 0.3), dt * 1000));
  },
  weaponSwap() { if (!this.ok()) return; this._click(700, 0.05, 0.2); setTimeout(() => this._click(1300, 0.04, 0.2), 120); },
  grenadeThrow() { if (!this.ok()) return; const n = this._noise(0.18), bp = this._filter('bandpass', 700, 1.2), g = this._env(0.2, 0.18, 0.05); n.connect(bp); bp.connect(g); g.connect(this.master); },

  /* ---------------- combat feedback ---------------- */
  hitmark(head) { if (!this.ok()) return; this._click(head ? 1950 : 1350, 0.03, 0.2); },
  hurt() {
    if (!this.ok()) return;
    const o = this._osc('sine', 130), g = this._env(0.4, 0.22);
    o.frequency.exponentialRampToValueAtTime(55, this.ctx.currentTime + 0.22);
    o.connect(g); g.connect(this.master); o.start(); o.stop(this.ctx.currentTime + 0.25);
    const n = this._noise(0.12), lp = this._filter('lowpass', 500), ng = this._env(0.3, 0.12);
    n.connect(lp); lp.connect(ng); ng.connect(this.master);
  },
  explosion(distant) {
    if (!this.ok()) return;
    const v = distant ? 0.22 : 1.0, f = distant ? 110 : 400;
    const n = this._noise(distant ? 1.6 : 1.1), lp = this._filter('lowpass', f, 0.5), g = this._env(v, distant ? 1.6 : 1.0, 0.01);
    n.connect(lp); lp.connect(g); g.connect(this.master);
    if (!distant) {
      const o = this._osc('sine', 52), og = this._env(0.7, 0.7);
      o.frequency.exponentialRampToValueAtTime(28, this.ctx.currentTime + 0.7);
      o.connect(og); og.connect(this.master); o.start(); o.stop(this.ctx.currentTime + 0.75);
      const c = this._noise(0.14), hp = this._filter('highpass', 1800), cg = this._env(0.3, 0.14);
      c.connect(hp); hp.connect(cg); cg.connect(this.master);
    }
  },
  distantBoom() {
    if (!this.ok()) return;
    this.explosion(true);
  },

  /* ---------------- enemies ---------------- */
  moan() {
    if (!this.ok()) return;
    const now = performance.now();
    if (now - this._lastGroan < 1400) return;
    this._lastGroan = now;
    const t = this.ctx.currentTime;
    const f0 = U.rand(62, 96);
    const o = this._osc('sawtooth', f0);
    o.frequency.setValueAtTime(f0, t);
    o.frequency.linearRampToValueAtTime(f0 * U.rand(0.6, 0.8), t + 1.3);
    const vib = this._osc('sine', U.rand(4, 7)), vg = this.ctx.createGain(); vg.gain.value = 6;
    vib.connect(vg); vg.connect(o.frequency); vib.start(); vib.stop(t + 1.5);
    const lp = this._filter('lowpass', 260, 0.6), g = this.ctx.createGain();
    g.gain.setValueAtTime(0.0001, t);
    g.gain.linearRampToValueAtTime(0.16, t + 0.25);
    g.gain.exponentialRampToValueAtTime(0.0001, t + 1.45);
    o.connect(lp); lp.connect(g); g.connect(this.master);
    o.start(); o.stop(t + 1.5);
  },
  zombieAttack() {
    if (!this.ok()) return;
    const n = this._noise(0.22), bp = this._filter('bandpass', 500, 1.4), g = this._env(0.3, 0.22, 0.06);
    bp.frequency.exponentialRampToValueAtTime(180, this.ctx.currentTime + 0.22);
    n.connect(bp); bp.connect(g); g.connect(this.master);
  },
  enemyDie() {
    if (!this.ok()) return;
    const t = this.ctx.currentTime, f0 = U.rand(90, 140);
    const o = this._osc('sawtooth', f0);
    o.frequency.exponentialRampToValueAtTime(f0 * 0.35, t + 0.5);
    const lp = this._filter('lowpass', 400), g = this._env(0.24, 0.5, 0.02);
    o.connect(lp); lp.connect(g); g.connect(this.master); o.start(); o.stop(t + 0.55);
    const n = this._noise(0.25), lp2 = this._filter('lowpass', 700), ng = this._env(0.2, 0.25);
    n.connect(lp2); lp2.connect(ng); ng.connect(this.master);
  },
  soldierShot() {
    if (!this.ok()) return;
    const n = this._noise(0.14), bp = this._filter('bandpass', 1200, 0.6), g = this._env(0.3, 0.14);
    n.connect(bp); bp.connect(g); g.connect(this.master);
  },

  /* ---------------- ui / misc ---------------- */
  footstep() {
    if (!this.ok()) return;
    const n = this._noise(0.05), lp = this._filter('lowpass', U.rand(300, 480)), g = this._env(0.09, 0.05);
    n.connect(lp); lp.connect(g); g.connect(this.master);
  },
  ui() { if (!this.ok()) return; this._click(1000, 0.04, 0.14); },
  waveHorn() {
    if (!this.ok()) return;
    const t = this.ctx.currentTime;
    [64, 96.5].forEach((f, i) => {
      const o = this._osc('sawtooth', f);
      const lp = this._filter('lowpass', 480, 0.7);
      const g = this.ctx.createGain();
      g.gain.setValueAtTime(0.0001, t + i * 0.06);
      g.gain.linearRampToValueAtTime(0.14, t + 0.3 + i * 0.06);
      g.gain.exponentialRampToValueAtTime(0.0001, t + 1.6);
      o.connect(lp); lp.connect(g); g.connect(this.master);
      o.start(t + i * 0.06); o.stop(t + 1.7);
    });
  },
  pickup() { if (!this.ok()) return; this._click(1500, 0.05, 0.18); setTimeout(() => this._click(2000, 0.06, 0.18), 70); },
  missionComplete() {
    if (!this.ok()) return;
    [0, 0.18, 0.36].forEach((dt, i) => setTimeout(() => this._click(700 + i * 300, 0.1, 0.2), dt * 1000));
  },
};
