package com.ironfront.core

import kotlin.math.cos
import kotlin.math.sin
import kotlin.math.sqrt
import kotlin.random.Random

/* ============================================================
   S2 — analytic battlefield terrain + layout data.
   Craters, carved trench lines, flatten zone at spawn, plus
   colliders and enemy spawn zones. Pure math, deterministic.
   ============================================================ */
object Terrain {
    data class Crater(val x: Float, val z: Float, val r: Float, val d: Float)
    data class Trench(val x1: Float, val z1: Float, val x2: Float, val z2: Float, val w: Float, val d: Float)
    data class Collider(val x: Float, val z: Float, val r: Float)
    data class P2(val x: Float, val z: Float)

    val craters = mutableListOf<Crater>()
    val trenches = mutableListOf<Trench>()
    val colliders = mutableListOf<Collider>()
    val spawnZones = mutableMapOf<String, List<P2>>()

    fun baseNoise(x: Float, z: Float): Float =
        (sin(x * 0.045) * cos(z * 0.038) * 1.5 +
         sin(x * 0.11 + z * 0.07) * 0.45 +
         cos(x * 0.021 - z * 0.05) * 0.9).toFloat()

    private fun smooth(t: Float): Float { val c = t.coerceIn(0f, 1f); return c * c * (3 - 2 * c) }

    private fun segDist(px: Float, pz: Float, t: Trench): Float {
        val dx = t.x2 - t.x1; val dz = t.z2 - t.z1
        val l2 = dx * dx + dz * dz
        var u = if (l2 > 0f) ((px - t.x1) * dx + (pz - t.z1) * dz) / l2 else 0f
        u = u.coerceIn(0f, 1f)
        return sqrt(dist2(px, pz, t.x1 + dx * u, t.z1 + dz * u))
    }

    fun dist2(ax: Float, az: Float, bx: Float, bz: Float): Float {
        val dx = ax - bx; val dz = az - bz; return dx * dx + dz * dz
    }

    fun heightAt(x: Float, z: Float): Float {
        var y = baseNoise(x, z)
        for (c in craters) {
            val dist = sqrt(dist2(x, z, c.x, c.z))
            if (dist < c.r) y -= c.d * smooth(1 - dist / c.r)
            else if (dist < c.r * 1.4f) y += c.d * 0.16f * smooth(1 - (dist - c.r) / (c.r * 0.4f))
        }
        for (t in trenches) {
            val d = segDist(x, z, t)
            if (d < t.w) y -= t.d * smooth(1 - d / t.w)
            else if (d < t.w * 1.5f) y += t.d * 0.2f * smooth(1 - (d - t.w) / (t.w * 0.5f))
        }
        val ds = sqrt(dist2(x, z, 0f, 46f))
        if (ds < 14f) y = lerp(baseNoise(0f, 46f) * 0.4f, y, smooth(ds / 14f))
        return y
    }

    private fun lerp(a: Float, b: Float, t: Float) = a + (b - a) * t

    fun generate() {
        val rnd = Random(1917)
        craters.clear(); trenches.clear(); colliders.clear(); spawnZones.clear()

        // craters in no-man's-land
        var guard = 0
        while (craters.size < 26 && guard++ < 400) {
            val x = rnd.nextFloat() * 160f - 80f
            val z = rnd.nextFloat() * 105f - 75f
            if (dist2(x, z, 0f, 46f) < 400f) continue
            if (dist2(x, z, -85f, -85f) < 324f) continue
            if (craters.any { dist2(x, z, it.x, it.z) < 130f }) continue
            craters += Crater(x, z, 2.6f + rnd.nextFloat() * 2.6f, 1.1f + rnd.nextFloat() * 0.9f)
        }

        // trench network
        trenches += Trench(-34f, 42f, 34f, 42f, 1.6f, 1.7f)
        trenches += Trench(-34f, 42f, -40f, 50f, 1.4f, 1.6f)
        trenches += Trench(34f, 42f, 41f, 49f, 1.4f, 1.6f)
        trenches += Trench(0f, 42f, 4f, 52f, 1.4f, 1.6f)
        trenches += Trench(-58f, -48f, -18f, -52f, 1.5f, 1.4f)

        // spawn zones
        val front = craters.filter { it.z > -25f }.map { P2(it.x, it.z) }.toMutableList()
        if (front.size < 4) front += listOf(P2(-20f, 5f), P2(20f, 0f), P2(0f, -10f), P2(-40f, 10f))
        spawnZones["cratersFront"] = front
        spawnZones["cratersAll"] = craters.map { P2(it.x, it.z) }.ifEmpty { front }
        val ridge = mutableListOf<P2>()
        for (k in 0 until 8) {
            val a = Math.PI + (rnd.nextFloat() * 2.5 - 1.25)
            val r = 62f + rnd.nextFloat() * 26f
            ridge += P2((sin(a) * r).toFloat(), (cos(a) * r).toFloat())
        }
        spawnZones["ridge"] = ridge
        val church = mutableListOf<P2>()
        for (k in 0 until 10) {
            val a = rnd.nextFloat() * 6.283f; val r = 22f + rnd.nextFloat() * 26f
            var x = -85f + cos(a) * r; var z = -85f + sin(a) * r
            val cr = sqrt(x * x + z * z); val maxR = Config.MAP_RADIUS - 8f
            if (cr > maxR) { x *= maxR / cr; z *= maxR / cr }
            church += P2(x, z)
        }
        spawnZones["church"] = church
    }
}
