package com.ironfront.core

import com.badlogic.gdx.ApplicationAdapter
import com.badlogic.gdx.Gdx
import com.badlogic.gdx.Input
import com.badlogic.gdx.InputProcessor
import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.GL20
import com.badlogic.gdx.graphics.PerspectiveCamera
import com.badlogic.gdx.graphics.g3d.Environment
import com.badlogic.gdx.graphics.g3d.Model
import com.badlogic.gdx.graphics.g3d.ModelBatch
import com.badlogic.gdx.graphics.g3d.ModelInstance
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.environment.DirectionalLight
import com.badlogic.gdx.graphics.g3d.utils.ModelBuilder
import com.badlogic.gdx.math.MathUtils
import com.badlogic.gdx.math.Quaternion
import com.badlogic.gdx.math.Vector3
import kotlin.math.sqrt
import kotlin.random.Random

/* ============================================================
   IRONFRONT 1917 — full playable loop (S3–S7 port).
   MENU → PLAY (3 waves) → WIN/DEAD. Touch + desktop controls,
   shooting, grenades, enemies, HUD, synthesized SFX.
   ============================================================ */
class IronFrontGame : ApplicationAdapter(), InputProcessor {

    enum class State { MENU, PLAY, DEAD, WIN }

    private lateinit var camera: PerspectiveCamera
    private lateinit var batch3d: ModelBatch
    private lateinit var env: Environment
    private val world = WorldBuilder()

    private var state = State.MENU
    private var touchMode = false
    private val rnd = Random(99)

    // player
    private val pos = Vector3(0f, 0f, 46f)
    private val vel = Vector3()
    private var yaw = 0f; private var pitch = 0f
    private var hp = Config.MAX_HP; private var lastHitT = -99f
    private var time = 0f
    private var bob = 0f; private var kick = 0f
    private var flash = 0f

    // weapons
    private var weaponId = "rifle"
    private val mags = mutableMapOf<String, Int>()
    private val reserves = mutableMapOf<String, Int>()
    private var cooldown = 0f; private var reloadT = 0f
    private var grenadeCount = Config.MAX_GRENADES

    // waves / stats
    private var waveIdx = 0
    private val pending = ArrayDeque<String>()
    private var spawnT = 0f; private var intermission = 0f
    private var kills = 0; private var shots = 0; private var hitsN = 0
    private var banner = ""; private var bannerT = 0f

    // input
    private val keys = mutableSetOf<Int>()
    private var fireHeld = false
    private var stickId = -1; private var stickX = 0f; private var stickY = 0f
    private var stickBX = 0f; private var stickBY = 0f; private var stickOn = false
    private var lookId = -1; private var lookLX = 0f; private var lookLY = 0f
    private var spreadVis = 0f

    // viewmodel
    private lateinit var gun: ModelInstance
    private val gunM4 = com.badlogic.gdx.math.Matrix4()

    /** test hook: jump straight into gameplay (used by headless checks) */
    var autoStart = false
    private var testFired = false

    override fun create() {
        touchMode = Gdx.input.isPeripheralAvailable(Input.Peripheral.MultitouchScreen)
        camera = PerspectiveCamera(75f, Gdx.graphics.width.toFloat(), Gdx.graphics.height.toFloat())
        camera.near = 0.1f; camera.far = 600f
        env = Environment()
        env.set(ColorAttribute.createAmbientLight(0.6f, 0.62f, 0.54f, 1f))
        env.add(DirectionalLight().set(Color(1f, 0.95f, 0.82f, 1f), Vector3(-0.4f, -0.8f, -0.45f)))
        world.build()
        batch3d = ModelBatch()
        Sfx.init(); Fx.init(); Tracers.init(); Grenades.init(); Hud.init()

        val mb = ModelBuilder()
        mb.begin()
        val p = mb.part("gun", GL20.GL_TRIANGLES, 3L,
            com.badlogic.gdx.graphics.g3d.Material(ColorAttribute.createDiffuse(Color(0.42f, 0.3f, 0.17f, 1f))))
        p.box(0f, 0f, 0.1f, 0.07f, 0.11f, 0.5f)
        p.box(0f, 0.02f, -0.35f, 0.05f, 0.07f, 0.6f)
        p.box(0f, 0.06f, -0.62f, 0.02f, 0.04f, 0.02f)
        gun = ModelInstance(mb.end())
        Gdx.input.inputProcessor = this
        if (autoStart) startGame()
    }

    /* ---------------- flow ---------------- */
    private fun startGame() {
        state = State.PLAY
        pos.set(0f, Terrain.heightAt(0f, 46f), 46f); vel.setZero()
        yaw = 0f; pitch = 0f; hp = Config.MAX_HP; lastHitT = -99f; time = 0f
        weaponId = "rifle"
        for ((id, w) in Config.weapons) { mags[id] = w.mag; reserves[id] = w.reserve }
        grenadeCount = Config.MAX_GRENADES
        kills = 0; shots = 0; hitsN = 0
        Enemies.clear()
        waveIdx = 0; intermission = 0f
        startWave()
        if (!touchMode) Gdx.input.isCursorCatched = true
    }

    private fun startWave() {
        val ph = Config.missions[0].phases[0] as MissionPhase.Waves
        val w = ph.waves[waveIdx]
        pending.clear()
        for ((t, n) in w.list) repeat(n) { pending.addLast(t) }
        spawnT = 1.5f
        banner("WAVE ${waveIdx + 1}")
        Sfx.play("moan", 0.8f)
    }

    private fun banner(s: String) { banner = s; bannerT = 2.4f }

    private fun endGame(win: Boolean) {
        state = if (win) State.WIN else State.DEAD
        if (!touchMode) Gdx.input.isCursorCatched = false
    }

    /* ---------------- update ---------------- */
    override fun render() {
        val dt = Gdx.graphics.deltaTime.coerceAtMost(0.05f)
        update(dt)

        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT or GL20.GL_DEPTH_BUFFER_BIT)
        Gdx.gl.glClearColor(0.61f, 0.63f, 0.52f, 1f)
        batch3d.begin(camera)
        batch3d.render(world.instances, env)
        batch3d.render(Enemies.instances, env)
        batch3d.render(Tracers.instances, env)
        batch3d.render(Grenades.instances, env)
        if (state == State.PLAY) batch3d.render(listOf(gun), env)
        batch3d.render(listOf(Fx.instance), env)
        batch3d.end()

        drawHud()
    }

    private fun update(dt: Float) {
        bannerT -= dt
        flash = maxOf(0f, flash - dt * 1.6f)
        spreadVis = maxOf(0f, spreadVis - dt * 4f)
        if (state != State.PLAY) {
            // menu backdrop: slow drift over the battlefield
            val t = time
            time += dt * 0.2f
            camera.position.set(MathUtils.sin(t * 0.05f) * 40f, 12f, 44f + MathUtils.cos(t * 0.05f) * 30f)
            camera.lookAt(0f, 1f, 10f)
            camera.update()
            return
        }
        time += dt

        // look (desktop cursor-catch)
        if (!touchMode && Gdx.input.isCursorCatched) {
            yaw -= Gdx.input.deltaX * 0.0028f
            pitch = (pitch - Gdx.input.deltaY * 0.0028f).coerceIn(-1.45f, 1.45f)
        }

        // move input
        var ix = stickX; var iy = stickY
        if (keys.contains(Input.Keys.W)) iy += 1f
        if (keys.contains(Input.Keys.S)) iy -= 1f
        if (keys.contains(Input.Keys.A)) ix -= 1f
        if (keys.contains(Input.Keys.D)) ix += 1f
        val il = sqrt(ix * ix + iy * iy)
        if (il > 1f) { ix /= il; iy /= il }
        val sprint = (il > 0.97f && iy > 0.5f)
        val speed = if (sprint) 7.9f else 5f
        val fx = -MathUtils.sin(yaw); val fz = -MathUtils.cos(yaw)
        val rx = MathUtils.cos(yaw); val rz = -MathUtils.sin(yaw)
        vel.x = (fx * iy + rx * ix) * speed
        vel.z = (fz * iy + rz * ix) * speed
        pos.x += vel.x * dt; pos.z += vel.z * dt
        for (c in Terrain.colliders) {
            val dx = pos.x - c.x; val dz = pos.z - c.z
            val d2 = dx * dx + dz * dz; val rr = c.r + 0.45f
            if (d2 < rr * rr && d2 > 0.0001f) {
                val d = sqrt(d2); pos.x = c.x + dx / d * rr; pos.z = c.z + dz / d * rr
            }
        }
        val cr = sqrt(pos.x * pos.x + pos.z * pos.z)
        if (cr > Config.MAP_RADIUS) { pos.x *= Config.MAP_RADIUS / cr; pos.z *= Config.MAP_RADIUS / cr }
        pos.y = Terrain.heightAt(pos.x, pos.z)
        val spd = sqrt(vel.x * vel.x + vel.z * vel.z)
        if (spd > 0.6f) bob += spd * dt * 1.55f

        // regen
        if (time - lastHitT > Config.REGEN_DELAY && hp < Config.MAX_HP)
            hp = minOf(Config.MAX_HP, hp + Config.REGEN_RATE * dt)

        // weapons
        cooldown -= dt
        if (reloadT > 0f) {
            reloadT -= dt
            if (reloadT <= 0f) {
                val w = Config.weapons[weaponId]!!
                val take = minOf(w.mag - (mags[weaponId] ?: 0), reserves[weaponId] ?: 0)
                mags[weaponId] = (mags[weaponId] ?: 0) + take
                reserves[weaponId] = (reserves[weaponId] ?: 0) - take
            }
        }
        kick = MathUtils.lerp(kick, 0f, 1f - kotlin.math.exp(-9f * dt))
        if (fireHeld && cooldown <= 0f && reloadT <= 0f) tryFire()
        if (autoStart) {
            if (!testFired && time > 4f) { testFired = true; throwGrenade() }
            if (time in 4f..10f) {
                // aim at nearest enemy and fire — headless proof of combat
                var best: Enemy? = null; var bd = 1e9f
                for (e in Enemies.list) { if (e.state == 3) continue; val d = Terrain.dist2(e.x, e.z, pos.x, pos.z); if (d < bd) { bd = d; best = e } }
                best?.let { e ->
                    yaw = kotlin.math.atan2(-(e.x - pos.x), -(e.z - pos.z))
                    pitch = -0.03f
                    if (cooldown <= 0f && reloadT <= 0f) tryFire()
                }
            }
        }

        // enemies
        Enemies.update(dt, pos.x, pos.z) { dmg, ex, ez -> damagePlayer(dmg, ex, ez) }

        // waves
        if (intermission > 0f) {
            intermission -= dt
            if (intermission <= 0f) { waveIdx++; startWave() }
        } else {
            if (pending.isNotEmpty()) {
                spawnT -= dt
                if (spawnT <= 0f && Enemies.aliveCount() < 12) {
                    spawnEnemy(pending.removeFirst())
                    val ph = Config.missions[0].phases[0] as MissionPhase.Waves
                    spawnT = ph.waves[waveIdx].interval * (0.7f + rnd.nextFloat() * 0.6f)
                }
            } else if (Enemies.aliveCount() == 0) {
                val ph = Config.missions[0].phases[0] as MissionPhase.Waves
                if (waveIdx >= ph.waves.size - 1) { endGame(true); return }
                intermission = 8f
                banner("WAVE CLEARED — RESUPPLIED")
                for ((id, w) in Config.weapons) reserves[id] = w.reserve
                grenadeCount = minOf(Config.MAX_GRENADES, grenadeCount + 1)
            }
        }

        Tracers.update(dt)
        Fx.update(dt, camera.direction)
        Grenades.update(dt) { p -> explodeAt(p) }

        // camera
        val bobY = MathUtils.sin(bob * 2f) * 0.03f * minOf(1f, spd / 5f)
        camera.position.set(pos.x, pos.y + 1.62f + bobY, pos.z)
        camera.direction.set(
            -MathUtils.sin(yaw) * MathUtils.cos(pitch),
            MathUtils.sin(pitch),
            -MathUtils.cos(yaw) * MathUtils.cos(pitch))
        camera.up.set(0f, 1f, 0f)
        camera.update()

        // viewmodel
        val right = Vector3(camera.direction).crs(Vector3.Y).nor()
        val upv = Vector3().set(right).crs(camera.direction).nor().scl(-1f)
        val gp = Vector3(camera.position).add(right.cpy().scl(0.26f - kick * 0.1f))
            .add(upv.cpy().scl(-0.25f)).add(camera.direction.cpy().scl(0.45f - kick * 0.25f))
        // camera-aligned basis: X=right, Y=up, Z=-dir
        // camera-aligned basis written straight into the matrix (col-major)
        val zax = Vector3(camera.direction).scl(-1f)
        val xax = Vector3(camera.direction).crs(Vector3.Y).nor()
        val yax = Vector3(xax).crs(camera.direction).nor()
        val mm = gunM4.`val`
        mm[com.badlogic.gdx.math.Matrix4.M00] = xax.x; mm[com.badlogic.gdx.math.Matrix4.M10] = xax.y; mm[com.badlogic.gdx.math.Matrix4.M20] = xax.z; mm[com.badlogic.gdx.math.Matrix4.M30] = 0f
        mm[com.badlogic.gdx.math.Matrix4.M01] = yax.x; mm[com.badlogic.gdx.math.Matrix4.M11] = yax.y; mm[com.badlogic.gdx.math.Matrix4.M21] = yax.z; mm[com.badlogic.gdx.math.Matrix4.M31] = 0f
        mm[com.badlogic.gdx.math.Matrix4.M02] = zax.x; mm[com.badlogic.gdx.math.Matrix4.M12] = zax.y; mm[com.badlogic.gdx.math.Matrix4.M22] = zax.z; mm[com.badlogic.gdx.math.Matrix4.M32] = 0f
        mm[com.badlogic.gdx.math.Matrix4.M03] = gp.x;  mm[com.badlogic.gdx.math.Matrix4.M13] = gp.y;  mm[com.badlogic.gdx.math.Matrix4.M23] = gp.z;  mm[com.badlogic.gdx.math.Matrix4.M33] = 1f
        gun.transform.set(gunM4)
    }

    private fun spawnEnemy(type: String) {
        val ph = Config.missions[0].phases[0] as MissionPhase.Waves
        val zone = if (type == "soldier") "ridge" else ph.spawnZone
        val pts = Terrain.spawnZones[zone] ?: Terrain.spawnZones["cratersFront"]!!
        val p = pts[rnd.nextInt(pts.size)]
        Enemies.spawn(type, p.x + rnd.nextFloat() * 4f - 2f, p.z + rnd.nextFloat() * 4f - 2f)
    }

    private fun tryFire() {
        val w = Config.weapons[weaponId]!!
        if ((mags[weaponId] ?: 0) <= 0) { reload(); return }
        mags[weaponId] = (mags[weaponId] ?: 0) - 1
        cooldown = w.rate
        kick = 0.5f
        spreadVis = 1f
        shots++
        Sfx.play(if (weaponId == "pistol") "pistol" else "rifle", 0.8f)
        val dir = Vector3(camera.direction)
        val sp = w.spread
        dir.add(rnd.nextFloat() * sp * 2 - sp, rnd.nextFloat() * sp * 2 - sp, rnd.nextFloat() * sp * 2 - sp).nor()
        val origin = Vector3(camera.position)
        val muzzle = Vector3(origin).add(dir.cpy().scl(0.7f)).add(0f, -0.12f, 0f)
        Fx.flash(muzzle.x, muzzle.y, muzzle.z)

        // nearest enemy sphere hit
        var best: Enemy? = null; var bestT = 1e9f; var head = false
        for (e in Enemies.list) {
            if (e.state == 3) continue
            for (hs in booleanArrayOf(false, true)) {
                val c = Vector3(e.x, e.y + if (hs) 1.75f else 1.05f, e.z)
                val r = if (hs) 0.3f else 0.55f
                val t = sphereHit(origin, dir, c, r)
                if (t != null && t < bestT) { bestT = t; best = e; head = hs }
            }
        }
        if (best != null) {
            hitsN++
            val hp3 = Vector3(origin).add(dir.cpy().scl(bestT))
            val killed = Enemies.damage(best, w.dmg, head, hp3.x, hp3.y, hp3.z)
            if (killed) { kills++; best = null }
            Tracers.spawn(muzzle, hp3, 1f, 0.82f, 0.45f)
        } else {
            // ground march
            var hit: Vector3? = null
            var t = 3f
            while (t < 160f) {
                val p = Vector3(origin).add(dir.cpy().scl(t))
                if (p.y <= Terrain.heightAt(p.x, p.z)) { hit = p; break }
                t += 1.5f
            }
            val end = hit ?: Vector3(origin).add(dir.cpy().scl(160f))
            if (hit != null) Fx.dirt(hit.x, hit.y, hit.z, 4)
            Tracers.spawn(muzzle, end, 1f, 0.82f, 0.45f)
        }
    }

    private fun sphereHit(o: Vector3, d: Vector3, c: Vector3, r: Float): Float? {
        val ox = o.x - c.x; val oy = o.y - c.y; val oz = o.z - c.z
        val b = ox * d.x + oy * d.y + oz * d.z
        val cc = ox * ox + oy * oy + oz * oz - r * r
        val disc = b * b - cc
        if (disc < 0f) return null
        val t = -b - sqrt(disc)
        return if (t > 0f) t else null
    }

    private fun reload() {
        val w = Config.weapons[weaponId]!!
        if (reloadT > 0f) return
        if ((mags[weaponId] ?: 0) >= w.mag || (reserves[weaponId] ?: 0) <= 0) return
        reloadT = w.reload
        Sfx.play("reload", 0.7f)
    }

    private fun throwGrenade() {
        if (state != State.PLAY || grenadeCount <= 0) return
        grenadeCount--
        Grenades.throwG(Vector3(camera.position).add(camera.direction.cpy().scl(0.5f)), camera.direction, vel)
    }

    private fun explodeAt(p: Vector3) {
        Sfx.play("boom", 1f)
        Fx.flash(p.x, p.y + 0.4f, p.z)
        Fx.dirt(p.x, p.y, p.z, 20)
        Fx.spark(p.x, p.y + 0.3f, p.z, 10)
        var before = Enemies.aliveCount()
        Enemies.explodeAt(p.x, p.z, 6.5f, 115f)
        kills += before - Enemies.aliveCount()
        val pd = sqrt(Terrain.dist2(p.x, p.z, pos.x, pos.z))
        if (pd < 5.5f) damagePlayer(40f * (1f - pd / 5.5f) + 5f, p.x, p.z)
    }

    private fun damagePlayer(dmg: Float, ex: Float, ez: Float) {
        if (state != State.PLAY) return
        hp -= dmg
        lastHitT = time
        flash = 1f
        Sfx.play("hurt", 0.8f)
        if (hp <= 0f) { hp = 0f; endGame(false) }
    }

    /* ---------------- HUD ---------------- */
    private fun drawHud() {
        Hud.batch.begin()
        val W = Hud.W.toFloat(); val H = Hud.H.toFloat()
        when (state) {
            State.MENU -> {
                Hud.rect(0f, 0f, W, H, Color(0f, 0f, 0f, 0.45f))
                Hud.big.color = Hud.BEIGE
                Hud.big.draw(Hud.batch, "IRONFRONT", W / 2f - 130f, H * 0.68f)
                Hud.big.color = Hud.GOLD
                Hud.big.data.setScale(1.6f)
                Hud.big.draw(Hud.batch, "1 9 1 7", W / 2f - 60f, H * 0.6f)
                Hud.big.data.setScale(3f)
                Hud.text("THE DEAD WALK — HOLD THE TRENCH", W / 2f - 130f, H * 0.52f, Hud.DIM)
                Hud.rect(W / 2f - 120f, H * 0.36f, 240f, 56f, Color(0.5f, 0.42f, 0.2f, 0.7f))
                Hud.text("DEPLOY", W / 2f - 34f, H * 0.36f + 34f, Hud.BEIGE)
                if (touchMode) Hud.text("left: move stick · right: look · FIRE to shoot", W / 2f - 160f, H * 0.25f, Hud.DIM)
                else Hud.text("WASD move · mouse look · LMB fire · R reload · G grenade · 1/2 swap", W / 2f - 240f, H * 0.25f, Hud.DIM)
            }
            State.PLAY -> {
                // health
                Hud.rect(20f, 24f, 220f, 12f, Color(0.1f, 0.1f, 0.06f, 0.6f))
                Hud.rect(20f, 24f, 220f * hp / Config.MAX_HP, 12f, if (hp < 35) Hud.RED else Hud.BEIGE)
                Hud.text("${hp.toInt()}", 250f, 36f)
                // ammo
                val w = Config.weapons[weaponId]!!
                Hud.text("${w.name}${if (reloadT > 0f) " · RELOADING" else ""}", W - 260f, 90f, Hud.DIM)
                Hud.big.color = Hud.BEIGE; Hud.big.data.setScale(1.8f)
                Hud.big.draw(Hud.batch, "${mags[weaponId]} / ${reserves[weaponId]}", W - 160f, 60f)
                Hud.big.data.setScale(3f)
                // wave
                val ph = Config.missions[0].phases[0] as MissionPhase.Waves
                val remaining = pending.size + Enemies.aliveCount()
                Hud.text("WAVE ${waveIdx + 1} / ${ph.waves.size}", 20f, H - 40f)
                Hud.text(if (intermission > 0f) "NEXT WAVE IN ${intermission.toInt()}" else "HOSTILES: $remaining", 20f, H - 70f, Hud.DIM)
                Hud.text("KILLS $kills", 20f, H - 100f, Hud.DIM)
                Hud.crosshair(spreadVis)
                if (bannerT > 0f) { Hud.big.color = Hud.BEIGE; Hud.big.data.setScale(2.2f); Hud.big.draw(Hud.batch, banner, W / 2f - banner.length * 14f, H * 0.66f); Hud.big.data.setScale(3f) }
                if (flash > 0f) Hud.rect(0f, 0f, W, H, Color(0.7f, 0.05f, 0.02f, flash * 0.45f))
                if (touchMode) {
                    Hud.drawButtons(grenadeCount)
                    if (stickOn) Hud.drawStick(stickBX, stickBY, stickX * 52f, stickY * 52f)
                }
            }
            State.DEAD, State.WIN -> {
                Hud.rect(0f, 0f, W, H, Color(0f, 0f, 0f, 0.6f))
                Hud.big.color = if (state == State.WIN) Hud.GOLD else Hud.RED
                Hud.big.draw(Hud.batch, if (state == State.WIN) "THE TRENCH HELD" else "YOU FELL", W / 2f - 150f, H * 0.62f)
                Hud.text("KILLS $kills   SHOTS $shots   ACCURACY ${if (shots > 0) hitsN * 100 / shots else 0}%", W / 2f - 140f, H * 0.5f, Hud.DIM)
                Hud.text("TAP / CLICK — RETURN TO MENU", W / 2f - 120f, H * 0.4f, Hud.DIM)
            }
        }
        Hud.batch.end()
    }

    /* ---------------- input ---------------- */
    override fun keyDown(keycode: Int): Boolean {
        keys += keycode
        if (state == State.PLAY) {
            when (keycode) {
                Input.Keys.R -> reload()
                Input.Keys.G -> throwGrenade()
                Input.Keys.NUM_1 -> weaponId = "rifle"
                Input.Keys.NUM_2 -> weaponId = "pistol"
                Input.Keys.ESCAPE -> endGame(false)
            }
        }
        return true
    }
    override fun keyUp(keycode: Int): Boolean { keys -= keycode; return true }
    override fun keyTyped(character: Char) = false

    override fun touchDown(x: Int, y: Int, pointer: Int, button: Int): Boolean {
        val sy = (Hud.H - y).toFloat()
        when (state) {
            State.MENU -> { startGame(); return true }
            State.DEAD, State.WIN -> { state = State.MENU; return true }
            else -> {}
        }
        if (!touchMode && button == 0) { fireHeld = true; return true }
        if (Hud.inRect(Hud.fireRect(), x.toFloat(), sy)) { fireHeld = true; return true }
        if (Hud.inRect(Hud.reloadRect(), x.toFloat(), sy)) { reload(); return true }
        if (Hud.inRect(Hud.grenadeRect(), x.toFloat(), sy)) { throwGrenade(); return true }
        if (Hud.inRect(Hud.swapRect(), x.toFloat(), sy)) { weaponId = if (weaponId == "rifle") "pistol" else "rifle"; return true }
        if (Hud.inRect(Hud.pauseRect(), x.toFloat(), sy)) { endGame(false); return true }
        if (x < Hud.W * 0.45f) {
            stickId = pointer; stickBX = x.toFloat(); stickBY = sy; stickOn = true; stickX = 0f; stickY = 0f
        } else {
            lookId = pointer; lookLX = x.toFloat(); lookLY = y.toFloat()
        }
        return true
    }

    override fun touchDragged(x: Int, y: Int, pointer: Int): Boolean {
        if (pointer == stickId) {
            var dx = (x - stickBX) / 52f; var dy = (Hud.H - y - stickBY) / 52f
            val l = sqrt(dx * dx + dy * dy)
            if (l > 1f) { dx /= l; dy /= l }
            stickX = dx; stickY = dy
        } else if (pointer == lookId && state == State.PLAY) {
            yaw -= (x - lookLX) * 0.005f
            pitch = (pitch - (y - lookLY) * 0.005f).coerceIn(-1.45f, 1.45f)
            lookLX = x.toFloat(); lookLY = y.toFloat()
        }
        return true
    }

    override fun touchUp(x: Int, y: Int, pointer: Int, button: Int): Boolean {
        if (pointer == stickId) { stickId = -1; stickOn = false; stickX = 0f; stickY = 0f }
        if (pointer == lookId) lookId = -1
        if (button == 0 || pointer == lookId || !touchMode) fireHeld = false
        return true
    }

    override fun touchCancelled(x: Int, y: Int, pointer: Int, button: Int): Boolean = touchUp(x, y, pointer, button)
    override fun mouseMoved(x: Int, y: Int) = false
    override fun scrolled(amountX: Float, amountY: Float) = false

    override fun dispose() {
        batch3d.dispose(); world.dispose()
    }
}
