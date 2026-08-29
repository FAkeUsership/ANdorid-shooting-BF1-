package com.ironfront.core

import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.GL20
import com.badlogic.gdx.graphics.g3d.Material
import com.badlogic.gdx.graphics.g3d.Model
import com.badlogic.gdx.graphics.g3d.ModelInstance
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.utils.ModelBuilder
import com.badlogic.gdx.math.MathUtils
import com.badlogic.gdx.math.Quaternion
import com.badlogic.gdx.math.Vector3
import kotlin.math.atan2
import kotlin.math.sqrt
import kotlin.random.Random

/* ============================================================
   S3+S5 — enemy rigs (procedural low-poly humanoids) and AI.
   Shared part-models per type; per-enemy ModelInstances.
   states: 0 rising · 1 chase · 2 attack-windup/strike · 3 dead
   ============================================================ */
class Enemy(val type: String) {
    var x = 0f; var z = 0f; var y = 0f; var yaw = 0f
    var hp = 0f; var speed = 0f; var dmg = 0f
    var attackRange = 0f; var attackRate = 0f; var score = 0
    var melee = true
    var state = 0
    var stateT = 0f; var attackCd = 0f; var animT = 0f
    var struck = false; var moanT = 2f; var riseDur = 1.4f
    var deathFlip = 1f; var deathDir = 0f
    var lastHitHead = false
    val parts = mutableListOf<ModelInstance>()   // legL legR torso head armL armR [rifle]
}

object Enemies {
    val list = mutableListOf<Enemy>()
    val instances = mutableListOf<ModelInstance>()
    private val partModels = mutableMapOf<String, List<Model>>()
    private val rnd = Random(13)

    // part indices
    const val LEG_L = 0; const val LEG_R = 1; const val TORSO = 2
    const val HEAD = 3; const val ARM_L = 4; const val ARM_R = 5; const val RIFLE = 6

    private val tmpQ = Quaternion(); private val tmpQ2 = Quaternion()
    private val tmpV = Vector3(); private val tmpV2 = Vector3()

    private fun partModel(type: String): List<Model> = partModels.getOrPut(type) {
        val def = Config.enemies[type]!!
        val soldier = type == "soldier"
        val skinC = if (soldier) Color(0.54f, 0.51f, 0.45f, 1f) else Color(0.49f, 0.55f, 0.40f, 1f)
        val clothC = if (soldier) Color(0.29f, 0.31f, 0.29f, 1f) else Color(0.34f, 0.36f, 0.27f, 1f)
        val skin = Material(ColorAttribute.createDiffuse(skinC))
        val cloth = Material(ColorAttribute.createDiffuse(clothC))
        val dark = Material(ColorAttribute.createDiffuse(Color(0.18f, 0.17f, 0.14f, 1f)))
        fun offsetBox(sx: Float, sy: Float, sz: Float, oy: Float, m: Material): Model {
            val mb = ModelBuilder()
            mb.begin()
            mb.part("p", GL20.GL_TRIANGLES, 3L, m).box(0f, oy, 0f, sx, sy, sz)
            return mb.end()
        }
        val leg = offsetBox(0.24f, 0.9f, 0.24f, -0.45f, cloth)
        val torso = offsetBox(if (soldier) 0.66f else 0.62f, 0.74f, 0.36f, 0f, cloth)
        val head = offsetBox(0.4f, 0.42f, 0.38f, 0f, skin)
        val arm = offsetBox(0.19f, 0.66f, 0.19f, -0.3f, cloth)
        val models = mutableListOf(leg, leg, torso, head, arm, arm)
        if (soldier) {
            val mb = ModelBuilder()
            mb.begin()
            val p = mb.part("r", GL20.GL_TRIANGLES, 3L, dark)
            p.box(0f, 0f, -0.3f, 0.06f, 0.08f, 0.9f)
            p.box(0f, -0.05f, 0.2f, 0.07f, 0.1f, 0.4f)
            models += mb.end()
        }
        models
    }

    fun spawn(type: String, x: Float, z: Float): Enemy {
        val def = Config.enemies[type]!!
        val e = Enemy(type)
        e.x = x; e.z = z; e.y = Terrain.heightAt(x, z)
        e.yaw = rnd.nextFloat() * 6.28f
        e.hp = def.hp; e.speed = rnd.nextFloat() * (def.speedMax - def.speedMin) + def.speedMin
        e.dmg = def.dmg; e.attackRange = def.attackRange; e.attackRate = def.attackRate
        e.score = def.score; e.melee = def.melee
        e.state = if (def.riseFromGround) 0 else 1
        e.attackCd = rnd.nextFloat()
        for (m in partModel(type)) {
            val mi = ModelInstance(m)
            e.parts += mi
            instances += mi
        }
        list += e
        return e
    }

    fun aliveCount() = list.count { it.state != 3 }

    fun clear() {
        list.clear()
        instances.clear()
    }

    private fun remove(e: Enemy) {
        for (p in e.parts) instances.remove(p)
        list.remove(e)
    }

    /* returns true if this call killed it */
    fun damage(e: Enemy, dmg: Float, head: Boolean, px: Float, py: Float, pz: Float): Boolean {
        if (e.state == 3) return false
        e.hp -= if (head) dmg * Config.HEADSHOT_MULT else dmg
        e.lastHitHead = head
        Fx.blood(px, py, pz, if (head) 10 else 6)
        Sfx.play("hit", 0.5f)
        if (e.hp <= 0) {
            e.state = 3; e.stateT = 0f
            e.deathDir = e.yaw
            e.deathFlip = if (rnd.nextFloat() < 0.5f) 1f else -1f
            Fx.blood(e.x, e.y + 1f, e.z, 10)
            Sfx.play("moan", 0.4f)
            return true
        }
        return false
    }

    fun explodeAt(x: Float, z: Float, radius: Float, dmg: Float) {
        for (e in list.toList()) {
            if (e.state == 3) continue
            val d = sqrt(Terrain.dist2(e.x, e.z, x, z))
            if (d < radius) damage(e, dmg * (1f - d / radius * 0.7f), false, e.x, e.y + 1f, e.z)
        }
    }

    fun update(dt: Float, px: Float, pz: Float, onPlayerHit: (Float, Float, Float) -> Unit) {
        for (e in list.toList()) {
            when (e.state) {
                3 -> { // dead: topple + sink + remove
                    e.stateT += dt
                    applyRig(e, topple = MathUtils.clamp(e.stateT / 0.45f, 0f, 1f))
                    if (e.stateT > 5f) {
                        for (p in e.parts) p.transform.translate(0f, -dt * 0.8f, 0f)
                        if (e.stateT > 6.5f) remove(e)
                    }
                    continue
                }
                0 -> { // rising
                    e.stateT += dt
                    val t = MathUtils.clamp(e.stateT / e.riseDur, 0f, 1f)
                    applyRig(e, sink = 1.7f * (1f - t * t * (3 - 2 * t)))
                    if (t >= 1f) e.state = 1
                    continue
                }
            }

            val dx = px - e.x; val dz = pz - e.z
            val dist = sqrt(dx * dx + dz * dz) + 0.001f

            if (e.melee) {
                if (e.state == 1) {
                    steer(e, dx / dist, dz / dist, dt)
                    e.y = Terrain.heightAt(e.x, e.z)
                    val ty = atan2(dx, dz)
                    e.yaw += angDelta(e.yaw, ty) * MathUtils.clamp(dt * 8f, 0f, 1f)
                    e.animT += dt * if (e.type == "runner") 11f else 6.5f
                    e.moanT -= dt
                    if (e.moanT <= 0f) { e.moanT = 2.5f + rnd.nextFloat() * 3f; if (dist < 42f) Sfx.play("moan", 0.5f) }
                    if (dist < e.attackRange) { e.state = 2; e.stateT = 0f; e.struck = false }
                    else applyRig(e, walk = e.animT)
                } else if (e.state == 2) {
                    e.stateT += dt
                    if (e.stateT in 0.32f..0.42f && !e.struck) {
                        e.struck = true
                        if (dist < e.attackRange + 0.7f) onPlayerHit(e.dmg, e.x, e.z)
                    }
                    if (e.stateT > 0.5f) {
                        e.attackCd -= dt
                        if (dist > e.attackRange + 0.4f) { e.state = 1 }
                        else if (e.attackCd <= 0f) { e.stateT = 0f; e.struck = false; e.attackCd = e.attackRate * (0.8f + rnd.nextFloat() * 0.4f) }
                    }
                    applyRig(e, attack = e.stateT)
                }
            } else {
                // soldier
                e.attackCd -= dt
                val moving = dist > e.attackRange || dist < 8f
                if (moving) {
                    val s = if (dist > e.attackRange) 1f else -0.7f
                    steer(e, dx / dist * s, dz / dist * s, dt)
                    e.animT += dt * 6f
                } else if (e.attackCd <= 0f) {
                    e.attackCd = e.attackRate * (0.75f + rnd.nextFloat() * 0.5f)
                    soldierFire(e, dist, px, pz, onPlayerHit)
                }
                e.y = Terrain.heightAt(e.x, e.z)
                val ty = atan2(dx, dz)
                e.yaw += angDelta(e.yaw, ty) * MathUtils.clamp(dt * 6f, 0f, 1f)
                applyRig(e, walk = if (moving) e.animT else null, aim = !moving)
            }
        }
    }

    private fun angDelta(cur: Float, target: Float): Float {
        var d = target - cur
        while (d > Math.PI.toFloat()) d -= 2f * Math.PI.toFloat()
        while (d < -Math.PI.toFloat()) d += 2f * Math.PI.toFloat()
        return d
    }

    private fun steer(e: Enemy, mx: Float, mz: Float, dt: Float) {
        var ax = mx; var az = mz
        for (o in list) {
            if (o === e || o.state == 3) continue
            val d2 = Terrain.dist2(e.x, e.z, o.x, o.z)
            if (d2 < 1.7f) {
                val d = sqrt(d2) + 0.01f
                ax += (e.x - o.x) / d * 0.8f; az += (e.z - o.z) / d * 0.8f
            }
        }
        for (c in Terrain.colliders) {
            val d2 = Terrain.dist2(e.x, e.z, c.x, c.z)
            val rr = c.r + 0.55f
            if (d2 < rr * rr) {
                val d = sqrt(d2) + 0.01f
                val push = (rr - d) / rr
                ax += (e.x - c.x) / d * push * 1.6f
                az += (e.z - c.z) / d * push * 1.6f
            }
        }
        val l = sqrt(ax * ax + az * az) + 0.001f
        e.x += ax / l * e.speed * dt
        e.z += az / l * e.speed * dt
    }

    private fun soldierFire(e: Enemy, dist: Float, px: Float, pz: Float, onPlayerHit: (Float, Float, Float) -> Unit) {
        Sfx.play("rifle", 0.25f)
        val from = tmpV2.set(e.x, e.y + 1.35f, e.z)
        var chance = 0.5f
        if (dist > 20f) chance *= 0.75f
        val hit = rnd.nextFloat() < chance
        val to = Vector3(px, 1.5f, pz)
        if (!hit) to.add(rnd.nextFloat() * 2.8f - 1.4f, rnd.nextFloat() * 2f - 0.9f, rnd.nextFloat() * 2.8f - 1.4f)
        Tracers.spawn(from, to, 1f, 0.7f, 0.4f)
        if (hit) onPlayerHit(e.dmg, e.x, e.z)
    }

    /* ---------- rig transform writer ---------- */
    private val SCALE1 = Vector3(1f, 1f, 1f)
    private fun smooth(t: Float) = t * t * (3f - 2f * t)

    private fun applyRig(e: Enemy, walk: Float? = null, attack: Float = 0f, aim: Boolean = false,
                         topple: Float = 0f, sink: Float = 0f) {
        val yawQ = Quaternion().setFromAxis(0f, 1f, 0f, e.yaw * MathUtils.radiansToDegrees)
        val deathQ = if (topple > 0f)
            Quaternion().setFromAxis(1f, 0f, 0f, -90f * e.deathFlip * smooth(topple)) else null
        val rootQ = if (deathQ != null) yawQ.cpy().mul(deathQ) else yawQ
        val baseY = e.y - sink

        val rootM = com.badlogic.gdx.math.Matrix4().set(rootQ)
        fun place(mi: ModelInstance, ox: Float, oy: Float, oz: Float, localQ: Quaternion) {
            val wq = rootQ.cpy().mul(localQ)
            tmpV.set(ox, oy, oz).rot(rootM)
            mi.transform.set(tmpV2.set(e.x + tmpV.x, baseY + tmpV.y, e.z + tmpV.z), wq, SCALE1)
        }

        val swing = if (walk != null) MathUtils.sin(walk) * 0.55f else 0f
        val qLegL = Quaternion().setFromAxis(1f, 0f, 0f, swing * 57.3f)
        val qLegR = Quaternion().setFromAxis(1f, 0f, 0f, -swing * 57.3f)
        val hunch = if (e.melee) 14f else 4f
        val qTorso = Quaternion().setFromAxis(1f, 0f, 0f, hunch)
        val qHead = Quaternion().setFromAxis(1f, 0f, 0f, -hunch * 0.6f)
        val armBase = if (e.melee) -78f else if (aim) -83f else -20f
        val qArmL = Quaternion().setFromAxis(1f, 0f, 0f, armBase + swing * 20f)
        val qArmR = Quaternion().setFromAxis(1f, 0f, 0f, armBase - swing * 20f)
        val (qA, qT) = if (attack > 0f) {
            val wind = if (attack < 0.32f) -140f else if (attack < 0.45f) -35f else -78f
            Pair(Quaternion().setFromAxis(1f, 0f, 0f, wind),
                 Quaternion().setFromAxis(1f, 0f, 0f, if (attack < 0.32f) -8f else 28f))
        } else Pair(qArmL, qTorso)

        place(e.parts[LEG_L], -0.17f, 0.9f, 0f, qLegL)
        place(e.parts[LEG_R], 0.17f, 0.9f, 0f, qLegR)
        place(e.parts[TORSO], 0f, 1.28f, 0f, qT)
        place(e.parts[HEAD], 0f, 1.78f, if (e.melee) 0.1f else 0f, qHead)
        place(e.parts[ARM_L], -0.42f, 1.6f, 0f, if (attack > 0f) qA else qArmL)
        place(e.parts[ARM_R], 0.42f, 1.6f, 0f, if (attack > 0f) qA else qArmR)
        if (e.parts.size > RIFLE) place(e.parts[RIFLE], 0.22f, 1.45f, -0.15f, Quaternion().setFromAxis(1f, 0f, 0f, -83f))
    }
}
