package com.ironfront.core

import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.GL20
import com.badlogic.gdx.graphics.Mesh
import com.badlogic.gdx.graphics.VertexAttribute
import com.badlogic.gdx.graphics.VertexAttributes
import com.badlogic.gdx.graphics.g3d.Material
import com.badlogic.gdx.graphics.g3d.Model
import com.badlogic.gdx.graphics.g3d.ModelInstance
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.attributes.IntAttribute
import com.badlogic.gdx.graphics.g3d.utils.ModelBuilder
import kotlin.math.atan2
import kotlin.math.cos
import kotlin.math.sin
import kotlin.math.sqrt
import kotlin.random.Random

/* ============================================================
   S2 — procedural battlefield. Original low-poly geometry in
   the approved style direction (flat-shaded, vertex-colored).
   Merged meshes → ~3 draw calls for the whole static world.
   ============================================================ */
class WorldBuilder {
    val instances = mutableListOf<ModelInstance>()
    private var model: Model? = null

    fun build() {
        dispose()
        Terrain.generate()
        val rnd = Random(42)

        val kit = MeshKit(11)
        buildSandbags(kit, rnd)
        buildWire(kit, rnd)
        buildDebris(kit, rnd)
        buildGrass(kit, rnd)
        buildTrees(kit, rnd)
        buildHouses(kit, rnd)
        buildChurch(kit)
        buildHills(kit, rnd)

        val clouds = MeshKit(5)
        buildClouds(clouds, rnd)

        val mb = ModelBuilder()
        mb.begin()
        val mat = Material(
            ColorAttribute.createDiffuse(Color.WHITE),
            IntAttribute.createCullFace(GL20.GL_NONE))
        mb.part("terrain", terrainMesh(rnd), GL20.GL_TRIANGLES, mat)
        mb.part("props", kit.mesh(), GL20.GL_TRIANGLES, mat)
        mb.part("clouds", clouds.mesh(), GL20.GL_TRIANGLES, mat)
        model = mb.end()
        instances += ModelInstance(model)
    }

    /* ---------------- terrain ---------------- */
    private fun terrainMesh(rnd: Random): Mesh {
        val n = 96
        val size = 380f
        val vc = (n + 1) * (n + 1)
        val verts = FloatArray(vc * 9)
        val idx = ShortArray(n * n * 6)
        val mudAr = 0.345f; val mudAg = 0.271f; val mudAb = 0.184f
        val mudBr = 0.427f; val mudBg = 0.325f; val mudBb = 0.220f
        val wetR = 0.212f;  val wetG = 0.165f;  val wetB = 0.125f
        val grR = 0.337f;   val grG = 0.408f;   val grB = 0.227f

        var p = 0
        for (iz in 0..n) for (ix in 0..n) {
            val x = -size / 2f + size * ix / n
            val z = -size / 2f + size * iz / n
            val y = Terrain.heightAt(x, z)
            val hl = Terrain.heightAt(x - 2f, z); val hr = Terrain.heightAt(x + 2f, z)
            val hd = Terrain.heightAt(x, z - 2f); val hu = Terrain.heightAt(x, z + 2f)
            var nxv = hl - hr; var nyv = 4f; var nzv = hd - hu
            val l = sqrt(nxv * nxv + nyv * nyv + nzv * nzv)
            nxv /= l; nyv /= l; nzv /= l
            // color
            val nn = sin(x * 0.08f + 3.1f) * cos(z * 0.075f - 1.2f)
            var t = (nn * 0.5f + 0.5f + (rnd.nextFloat() * 0.12f - 0.06f)).coerceIn(0f, 1f)
            var r = mudAr + (mudBr - mudAr) * t
            var g = mudAg + (mudBg - mudAg) * t
            var b = mudAb + (mudBb - mudAb) * t
            val gp = sin(x * 0.05f + 1.7f) * sin(z * 0.043f - 0.6f)
            if (gp > 0.45f) {
                val gt = ((gp - 0.45f) * 1.8f).coerceIn(0f, 0.7f)
                r += (grR - r) * gt; g += (grG - g) * gt; b += (grB - b) * gt
            }
            if (Terrain.craters.any { Terrain.dist2(x, z, it.x, it.z) < it.r * it.r }) {
                r += (wetR - r) * 0.65f; g += (wetG - g) * 0.65f; b += (wetB - b) * 0.65f
            }
            verts[p++] = x; verts[p++] = y; verts[p++] = z
            verts[p++] = nxv; verts[p++] = nyv; verts[p++] = nzv
            verts[p++] = r; verts[p++] = g; verts[p++] = b
        }
        var q = 0
        for (iz in 0 until n) for (ix in 0 until n) {
            val a = (iz * (n + 1) + ix).toShort()
            val b0 = (a + 1).toShort()
            val c = (a + n + 1).toShort()
            val d = (c + 1).toShort()
            idx[q++] = a; idx[q++] = c; idx[q++] = b0
            idx[q++] = b0; idx[q++] = c; idx[q++] = d
        }
        val mesh = Mesh(true, false, vc, idx.size,
            VertexAttributes(
                VertexAttribute(VertexAttributes.Usage.Position, 3, "a_position"),
                VertexAttribute(VertexAttributes.Usage.Normal, 3, "a_normal"),
                VertexAttribute(VertexAttributes.Usage.ColorUnpacked, 3, "a_color")))
        mesh.setVertices(verts)
        mesh.setIndices(idx)
        return mesh
    }

    /* ---------------- props ---------------- */
    private val bagR = 0.55f; private val bagG = 0.44f; private val bagB = 0.30f

    private fun buildSandbags(kit: MeshKit, rnd: Random) {
        for (tr in Terrain.trenches) {
            val dx = tr.x2 - tr.x1; val dz = tr.z2 - tr.z1
            val len = sqrt(dx * dx + dz * dz)
            val nx = -dz / len; val nz = dx / len
            val ang = atan2(dx, dz) + Math.PI.toFloat() / 2f
            val cnt = (len / 0.95f).toInt()
            for (side in intArrayOf(-1, 1)) for (i in 0..cnt) {
                val t = i.toFloat() / cnt
                val bx = tr.x1 + dx * t + nx * (tr.w + 0.42f) * side
                val bz = tr.z1 + dz * t + nz * (tr.w + 0.42f) * side
                val by = Terrain.heightAt(bx, bz)
                kit.box(bx, by + 0.18f, bz, 0.92f, 0.34f, 0.5f, ang + rnd.nextFloat() * 0.18f - 0.09f, bagR, bagG, bagB, 0.16f)
                if (rnd.nextFloat() < 0.62f)
                    kit.box(bx + rnd.nextFloat() * 0.12f - 0.06f, by + 0.52f, bz + rnd.nextFloat() * 0.12f - 0.06f,
                        0.92f, 0.34f, 0.5f, ang + rnd.nextFloat() * 0.3f - 0.15f, bagR, bagG, bagB, 0.16f)
                if (side == -1 && rnd.nextFloat() < 0.3f)
                    kit.box(bx, by + 0.86f, bz, 0.92f, 0.34f, 0.5f, ang + rnd.nextFloat() * 0.4f - 0.2f, bagR, bagG, bagB, 0.16f)
            }
        }
        // surface strongpoints = cover + colliders
        val piles = listOf(14f to 8f, -20f to -6f, 30f to -25f, -8f to -30f, -45f to -20f, 48f to -5f)
        for ((cx, cz) in piles) {
            val rings = 5 + rnd.nextInt(4)
            for (k in 0 until rings) {
                val a = rnd.nextFloat() * 6.283f
                kit.box(cx + cos(a) * rnd.nextFloat() * 1.4f, Terrain.heightAt(cx, cz) + 0.18f,
                    cz + sin(a) * rnd.nextFloat() * 1.4f, 0.92f, 0.34f, 0.5f, a, bagR, bagG, bagB, 0.16f)
            }
            Terrain.colliders += Terrain.Collider(cx, cz, 1.9f)
        }
    }

    private fun buildWire(kit: MeshKit, rnd: Random) {
        val rows = listOf(Triple(16f, -75f, 75f), Triple(-18f, -80f, 80f), Triple(-58f, -105f, -40f))
        for ((z, x1, x2) in rows) {
            val cnt = ((x2 - x1) / 4.2f).toInt()
            var prev: Triple<Float, Float, Float>? = null
            for (i in 0..cnt) {
                val x = x1 + i * 4.2f + rnd.nextFloat() - 0.5f
                val zz = z + rnd.nextFloat() * 2.4f - 1.2f
                val y = Terrain.heightAt(x, zz)
                kit.box(x, y + 0.6f, zz, 0.12f, 1.25f, 0.12f, rnd.nextFloat() * 0.3f, 0.29f, 0.23f, 0.16f)
                val cur = Triple(x, y, zz)
                prev?.let { pv ->
                    val ddx = cur.first - pv.first; val ddz = cur.third - pv.third
                    val len = sqrt(ddx * ddx + ddz * ddz)
                    val ry = atan2(ddx, ddz)
                    val mx = (cur.first + pv.first) / 2f; val mz = (cur.third + pv.third) / 2f
                    for (h in floatArrayOf(0.35f, 0.62f, 0.9f)) {
                        kit.box(mx, (cur.second + pv.second) / 2f + h, mz, 0.035f, 0.035f, len, ry, 0.34f, 0.33f, 0.31f, 0.05f)
                    }
                }
                prev = cur
            }
        }
    }

    private fun buildDebris(kit: MeshKit, rnd: Random) {
        repeat(60) { // rocks
            val x = rnd.nextFloat() * 230f - 115f; val z = rnd.nextFloat() * 165f - 100f
            val s = 0.25f + rnd.nextFloat() * 1.1f
            kit.box(x, Terrain.heightAt(x, z) + s * 0.2f, z, s, s * (0.5f + rnd.nextFloat() * 0.3f), s,
                rnd.nextFloat() * 3f, 0.32f, 0.30f, 0.26f, 0.2f)
        }
        repeat(24) { // broken planks
            val x = rnd.nextFloat() * 200f - 100f; val z = rnd.nextFloat() * 145f - 90f
            kit.box(x, Terrain.heightAt(x, z) + 0.08f, z, 0.3f, 0.06f, 1.6f * (0.6f + rnd.nextFloat() * 0.7f),
                rnd.nextFloat() * 3f, 0.36f, 0.28f, 0.17f, 0.15f)
        }
    }

    private fun buildGrass(kit: MeshKit, rnd: Random) {
        var placed = 0; var guard = 0
        while (placed < 170 && guard++ < 900) {
            val x = rnd.nextFloat() * 220f - 110f; val z = rnd.nextFloat() * 165f - 95f
            val gp = sin(x * 0.05f + 1.7f) * sin(z * 0.043f - 0.6f)
            if (gp < 0.3f) continue
            val s = 0.7f + rnd.nextFloat()
            kit.spike(x, Terrain.heightAt(x, z), z, 0.32f * s, 0.5f * s, 0.38f, 0.44f, 0.25f, 0.2f)
            placed++
        }
    }

    private fun buildTrees(kit: MeshKit, rnd: Random) {
        repeat(13) {
            val x = rnd.nextFloat() * 220f - 110f; val z = rnd.nextFloat() * 155f - 95f
            if (Terrain.dist2(x, z, 0f, 46f) < 300f || Terrain.dist2(x, z, -85f, -85f) < 500f) return@repeat
            val h = 4.5f + rnd.nextFloat() * 3.5f
            val y = Terrain.heightAt(x, z)
            kit.box(x, y + h / 2f - 0.1f, z, 0.42f, h, 0.42f, rnd.nextFloat() * 0.1f, 0.24f, 0.20f, 0.15f)
            val nb = 2 + rnd.nextInt(4)
            repeat(nb) {
                val bl = 1.2f + rnd.nextFloat() * 1.6f
                val dir = if (rnd.nextFloat() < 0.5f) 1f else -1f
                kit.box(x + dir * bl * 0.35f, y + h * (0.45f + rnd.nextFloat() * 0.4f), z,
                    bl, 0.14f, 0.14f, if (dir > 0) 1.2f else -1.2f, 0.24f, 0.20f, 0.15f)
            }
            Terrain.colliders += Terrain.Collider(x, z, 0.5f)
        }
    }

    private fun buildHouses(kit: MeshKit, rnd: Random) {
        val spots = listOf(Triple(26f, -12f, 0.4f), Triple(-30f, -38f, -0.8f), Triple(55f, -55f, 1.9f))
        val wall = 0.54f; val wallG = 0.48f; val wallB = 0.38f
        for ((hx, hz, rot) in spots) {
            val gy = Terrain.heightAt(hx, hz)
            val w = 7f; val d = 5.5f; val h = 3.4f
            val c = cos(rot); val s = sin(rot)
            fun px(lx: Float, lz: Float) = hx + lx * c + lz * s
            fun pz(lx: Float, lz: Float) = hz - lx * s + lz * c
            kit.box(px(0f, -d / 2f), gy + h * 0.4f, pz(0f, -d / 2f), w, h * 0.8f, 0.42f, rot, wall, wallG, wallB)
            kit.box(px(-w * 0.22f, d / 2f), gy + h * 0.31f, pz(-w * 0.22f, d / 2f), w * 0.55f, h * 0.62f, 0.42f, rot, wall, wallG, wallB)
            kit.box(px(-w / 2f, 0f), gy + h * 0.45f, pz(-w / 2f, 0f), 0.42f, h * 0.9f, d, rot, wall, wallG, wallB)
            kit.box(px(w / 2f, 0f), gy + h * 0.25f, pz(w / 2f, 0f), 0.42f, h * 0.5f, d, rot, wall, wallG, wallB)
            repeat(3) { // collapsed roof beams
                kit.box(px(rnd.nextFloat() * w / 1.3f - w / 2.6f, rnd.nextFloat() - 0.5f), gy + h * (0.35f + rnd.nextFloat() * 0.35f),
                    pz(0f, 0f), 0.22f, 0.22f, d * 1.15f, rot + rnd.nextFloat() * 0.4f - 0.7f, 0.31f, 0.23f, 0.16f)
            }
            repeat(6) { // rubble
                val lx = rnd.nextFloat() * (w + 2f) - w / 2f - 1f
                val lz = rnd.nextFloat() * (d + 3f) - d / 2f - 1.5f
                kit.box(px(lx, lz), gy + 0.25f, pz(lx, lz), 0.6f + rnd.nextFloat() * 0.5f, 0.5f, 0.6f, rnd.nextFloat() * 3f, wall, wallG, wallB, 0.2f)
            }
            Terrain.colliders += Terrain.Collider(hx, hz, 3.4f)
        }
    }

    private fun buildChurch(kit: MeshKit) {
        val hx = -85f; val hz = -85f; val rot = 0.5f
        val gy = Terrain.heightAt(hx, hz)
        val st = 0.49f; val stG = 0.45f; val stB = 0.40f
        val w = 9f; val d = 12f; val h = 7f
        val c = cos(rot); val s = sin(rot)
        fun px(lx: Float, lz: Float) = hx + lx * c + lz * s
        fun pz(lx: Float, lz: Float) = hz - lx * s + lz * c
        kit.box(px(0f, -d / 2f), gy + h * 0.37f, pz(0f, -d / 2f), w, h * 0.75f, 0.6f, rot, st, stG, stB)
        kit.box(px(-w * 0.29f, d / 2f), gy + h * 0.27f, pz(-w * 0.29f, d / 2f), w * 0.42f, h * 0.55f, 0.6f, rot, st, stG, stB)
        kit.box(px(w * 0.33f, d / 2f), gy + h * 0.2f, pz(w * 0.33f, d / 2f), w * 0.3f, h * 0.4f, 0.6f, rot, st, stG, stB)
        kit.box(px(-w / 2f, 0f), gy + h * 0.33f, pz(-w / 2f, 0f), 0.6f, h * 0.66f, d, rot, st, stG, stB)
        kit.box(px(w / 2f, -d * 0.15f), gy + h * 0.25f, pz(w / 2f, -d * 0.15f), 0.6f, h * 0.5f, d * 0.6f, rot, st, stG, stB)
        // tower + spire + cross
        kit.box(px(0f, -d / 2f - 2.4f), gy + h * 0.75f, pz(0f, -d / 2f - 2.4f), 3.4f, h * 1.5f, 3.4f, rot, st, stG, stB)
        kit.spike(px(0f, -d / 2f - 2.4f), gy + h * 1.5f, pz(0f, -d / 2f - 2.4f), 4.4f, 3.2f, 0.31f, 0.23f, 0.16f, 0.05f)
        kit.box(px(0f, -d / 2f - 2.4f), gy + h * 1.5f + 3.6f, pz(0f, -d / 2f - 2.4f), 0.22f, 1.7f, 0.22f, rot, 0.31f, 0.23f, 0.16f)
        kit.box(px(0f, -d / 2f - 2.4f), gy + h * 1.5f + 3.95f, pz(0f, -d / 2f - 2.4f), 0.95f, 0.22f, 0.22f, rot, 0.31f, 0.23f, 0.16f)
        // fallen bell + rubble
        kit.box(px(3.2f, 4.5f), gy + 0.55f, pz(3.2f, 4.5f), 1.1f, 0.9f, 1.1f, 1.2f, 0.44f, 0.38f, 0.22f)
        repeat(8) {
            val lx = (Math.random() * 2 * w - w).toFloat(); val lz = (Math.random() * 1.8 * d - d).toFloat()
            kit.box(px(lx, lz), gy + 0.3f, pz(lx, lz), 0.7f, 0.6f, 0.7f, (Math.random() * 3).toFloat(), st, stG, stB, 0.2f)
        }
        Terrain.colliders += Terrain.Collider(hx, hz, 7.5f)
    }

    private fun buildHills(kit: MeshKit, rnd: Random) {
        repeat(12) {
            val a = (it.toFloat() / 12f) * 6.283f + rnd.nextFloat() * 0.4f - 0.2f
            val r = 200f + rnd.nextFloat() * 35f
            kit.spike(cos(a) * r, -2f, sin(a) * r, 60f + rnd.nextFloat() * 60f, 14f + rnd.nextFloat() * 20f,
                0.30f, 0.28f, 0.22f, 0.08f)
        }
    }

    private fun buildClouds(kit: MeshKit, rnd: Random) {
        repeat(9) {
            val cx = rnd.nextFloat() * 440f - 220f
            val cy = 55f + rnd.nextFloat() * 40f
            val cz = rnd.nextFloat() * 300f - 240f
            repeat(4) {
                val s = 3f + rnd.nextFloat() * 4f
                kit.box(cx + rnd.nextFloat() * 16f - 8f, cy + rnd.nextFloat() * 3f - 1.5f, cz + rnd.nextFloat() * 8f - 4f,
                    s * 2f, s * 0.8f, s, rnd.nextFloat() * 3f, 0.87f, 0.88f, 0.83f, 0.05f)
            }
        }
    }

    fun dispose() {
        model?.dispose()
        model = null
        instances.clear()
    }
}
