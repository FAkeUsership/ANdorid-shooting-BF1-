package com.ironfront.core

import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.GL20
import com.badlogic.gdx.graphics.Mesh
import com.badlogic.gdx.graphics.VertexAttribute
import com.badlogic.gdx.graphics.VertexAttributes
import com.badlogic.gdx.graphics.g3d.Material
import com.badlogic.gdx.graphics.g3d.Model
import com.badlogic.gdx.graphics.g3d.ModelInstance
import com.badlogic.gdx.graphics.g3d.attributes.BlendingAttribute
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.model.MeshPart
import com.badlogic.gdx.graphics.g3d.model.Node
import com.badlogic.gdx.graphics.g3d.model.NodePart
import com.badlogic.gdx.graphics.g3d.utils.ModelBuilder
import com.badlogic.gdx.math.MathUtils
import com.badlogic.gdx.math.Quaternion
import com.badlogic.gdx.math.Vector3
import kotlin.math.acos
import kotlin.random.Random

/* ============================================================
   S4 — particles (one dynamic mesh), tracers, grenades,
   hitscan resolution. Pooled, zero GC in the loop.
   ============================================================ */
object Fx {
    const val MAX = 280
    private val px = FloatArray(MAX * 3); private val pv = FloatArray(MAX * 3)
    private val plife = FloatArray(MAX); private val pmax = FloatArray(MAX)
    private val psize = FloatArray(MAX)
    private val pcol = FloatArray(MAX * 3)
    private val pon = BooleanArray(MAX)
    private lateinit var mesh: Mesh
    lateinit var instance: ModelInstance
    private val rnd = Random(3)
    private val verts = FloatArray(MAX * 4 * 7)
    private val idx = ShortArray(MAX * 6)
    private val right = Vector3(); private val up = Vector3()

    fun init() {
        for (q in 0 until MAX) {
            val b = (q * 4).toShort()
            idx[q * 6] = b; idx[q * 6 + 1] = (b + 1).toShort(); idx[q * 6 + 2] = (b + 2).toShort()
            idx[q * 6 + 3] = b; idx[q * 6 + 4] = (b + 2).toShort(); idx[q * 6 + 5] = (b + 3).toShort()
        }
        mesh = Mesh(false, false, MAX * 4, MAX * 6,
            VertexAttributes(
                VertexAttribute(VertexAttributes.Usage.Position, 3, "a_position"),
                VertexAttribute(VertexAttributes.Usage.ColorUnpacked, 4, "a_color")))
        mesh.setIndices(idx)
        val model = Model()
        val part = MeshPart()
        part.id = "fx"; part.mesh = mesh; part.primitiveType = GL20.GL_TRIANGLES
        part.offset = 0; part.size = MAX * 6
        part.center.set(0f, 0f, 0f)
        part.halfExtents.set(4000f, 4000f, 4000f)   // never frustum-cull the dynamic pool
        val node = Node()
        val np = NodePart()
        np.meshPart = part
        np.material = Material(BlendingAttribute(true, 1f))
        node.parts.add(np)
        model.nodes.add(node)
        model.meshes.add(mesh)
        model.materials.add(np.material)
        instance = ModelInstance(model)
    }

    private fun spawn(x: Float, y: Float, z: Float, n: Int, power: Float, upB: Float,
                      r: Float, g: Float, b: Float, life: Float) {
        var done = 0
        for (i in 0 until MAX) {
            if (pon[i]) continue
            pon[i] = true
            px[i * 3] = x; px[i * 3 + 1] = y; px[i * 3 + 2] = z
            val a = rnd.nextFloat() * 6.283f; val u = rnd.nextFloat() * 2f - 1f + upB
            val s = power * (0.3f + rnd.nextFloat() * 0.7f)
            pv[i * 3] = MathUtils.cos(a) * s; pv[i * 3 + 1] = u * s; pv[i * 3 + 2] = MathUtils.sin(a) * s
            plife[i] = life * (0.6f + rnd.nextFloat() * 0.4f); pmax[i] = plife[i]
            psize[i] = 0.09f + rnd.nextFloat() * 0.1f
            pcol[i * 3] = r; pcol[i * 3 + 1] = g; pcol[i * 3 + 2] = b
            if (++done >= n) break
        }
    }

    fun blood(x: Float, y: Float, z: Float, n: Int) = spawn(x, y, z, n, 4.5f, 0.7f, 0.55f, 0.08f, 0.06f, 0.5f)
    fun dirt(x: Float, y: Float, z: Float, n: Int) = spawn(x, y, z, n, 3f, 0.9f, 0.36f, 0.28f, 0.19f, 0.6f)
    fun flash(x: Float, y: Float, z: Float) = spawn(x, y, z, 3, 0.5f, 0.3f, 1f, 0.85f, 0.5f, 0.1f)
    fun spark(x: Float, y: Float, z: Float, n: Int) = spawn(x, y, z, n, 8f, 1f, 1f, 0.7f, 0.3f, 0.5f)

    fun update(dt: Float, camDir: Vector3) {
        right.set(camDir).crs(Vector3.Y).nor()
        up.set(camDir).scl(-1f).crs(right).nor()
        var v = 0
        for (i in 0 until MAX) {
            if (!pon[i]) continue
            plife[i] -= dt
            if (plife[i] <= 0f) { pon[i] = false; continue }
            pv[i * 3 + 1] -= 12f * dt
            px[i * 3] += pv[i * 3] * dt; px[i * 3 + 1] += pv[i * 3 + 1] * dt; px[i * 3 + 2] += pv[i * 3 + 2] * dt
            val a = plife[i] / pmax[i]
            val s = psize[i]
            val x = px[i * 3]; val y = px[i * 3 + 1]; val z = px[i * 3 + 2]
            val r = pcol[i * 3] * a; val g = pcol[i * 3 + 1] * a; val b = pcol[i * 3 + 2] * a
            putVert(x - right.x * s - up.x * s, y - right.y * s - up.y * s, z - right.z * s - up.z * s, r, g, b, a, v++ * 7)
            putVert(x + right.x * s - up.x * s, y + right.y * s - up.y * s, z + right.z * s - up.z * s, r, g, b, a, v++ * 7)
            putVert(x + right.x * s + up.x * s, y + right.y * s + up.y * s, z + right.z * s + up.z * s, r, g, b, a, v++ * 7)
            putVert(x - right.x * s + up.x * s, y - right.y * s + up.y * s, z - right.z * s + up.z * s, r, g, b, a, v++ * 7)
        }
        if (v == 0) {
            for (k in 0 until 4) putVert(0f, -999f, 0f, 0f, 0f, 0f, 0f, k * 7)
            v = 4
        }
        mesh.setVertices(verts, 0, v)
    }

    private fun putVert(x: Float, y: Float, z: Float, r: Float, g: Float, b: Float, a: Float, o: Int) {
        verts[o] = x; verts[o + 1] = y; verts[o + 2] = z
        verts[o + 3] = r; verts[o + 4] = g; verts[o + 5] = b; verts[o + 6] = a
    }
}

/** rotation aligning +Z with dir (axis-angle between Z and dir) */
fun qFromZ(dir: Vector3, out: Quaternion): Quaternion {
    val d = Vector3(dir).nor()
    val dot = d.z.coerceIn(-1f, 1f)
    if (dot > 0.9999f) { out.idt(); return out }
    if (dot < -0.9999f) { out.setFromAxis(1f, 0f, 0f, 180f); return out }
    val ax = Vector3(0f, 0f, 1f).crs(d).nor()
    out.setFromAxis(ax, (acos(dot.toDouble()).toFloat() * 57.29578f))
    return out
}

object Tracers {
    private class T {
        lateinit var mi: ModelInstance
        var life = 0f
    }
    private val pool = mutableListOf<T>()

    fun init() {
        val mb = ModelBuilder()
        mb.begin()
        mb.part("t", GL20.GL_TRIANGLES, 3L,
            Material(ColorAttribute.createDiffuse(Color(1f, 0.8f, 0.4f, 1f)), BlendingAttribute(true, 1f)))
            .box(0f, 0f, 0f, 0.03f, 0.03f, 1f)
        val model = mb.end()
        repeat(24) {
            val t = T()
            t.mi = ModelInstance(model)
            t.mi.transform.setToScaling(0f, 0f, 0f)
            pool += t
        }
    }

    val instances: List<ModelInstance> get() = pool.map { it.mi }

    private val tmpDir = Vector3(); private val tmpScale = Vector3()
    private val tmpMid = Vector3(); private val tmpQ = Quaternion()

    fun spawn(from: Vector3, to: Vector3, r: Float, g: Float, b: Float) {
        val t = pool.firstOrNull { it.life <= 0f } ?: return
        t.life = 0.08f
        tmpMid.set(from).add(to).scl(0.5f)
        val len = from.dst(to)
        tmpDir.set(to).sub(from)
        qFromZ(tmpDir, tmpQ)
        t.mi.transform.set(tmpMid, tmpQ, tmpScale.set(1f, 1f, len))
    }

    fun update(dt: Float) {
        for (t in pool) {
            if (t.life > 0f) {
                t.life -= dt
                if (t.life <= 0f) t.mi.transform.setToScaling(0f, 0f, 0f)
            }
        }
    }
}

object Grenades {
    private class G {
        lateinit var mi: ModelInstance
        val vel = Vector3()
        var t = 0f; var on = false
    }
    private val pool = mutableListOf<G>()

    fun init() {
        val mb = ModelBuilder()
        mb.begin()
        mb.part("g", GL20.GL_TRIANGLES, 3L, Material(ColorAttribute.createDiffuse(Color(0.2f, 0.26f, 0.18f, 1f))))
            .box(0f, 0f, 0f, 0.22f, 0.26f, 0.22f)
        val model = mb.end()
        repeat(4) {
            val g = G()
            g.mi = ModelInstance(model)
            g.mi.transform.setToScaling(0f, 0f, 0f)
            pool += g
        }
    }

    val instances: List<ModelInstance> get() = pool.filter { it.on }.map { it.mi }

    fun throwG(from: Vector3, dir: Vector3, playerVel: Vector3) {
        val g = pool.firstOrNull { !it.on } ?: return
        g.on = true; g.t = 2.3f
        g.mi.transform.setToTranslation(from.x, from.y, from.z)
        g.vel.set(dir).scl(14f).add(playerVel.x * 0.35f, 3.2f, playerVel.z * 0.35f)
    }

    fun update(dt: Float, onExplode: (Vector3) -> Unit) {
        val p = Vector3()
        for (g in pool) {
            if (!g.on) continue
            g.t -= dt
            g.vel.y -= 20f * dt
            g.mi.transform.getTranslation(p)
            p.add(g.vel.x * dt, g.vel.y * dt, g.vel.z * dt)
            val gy = Terrain.heightAt(p.x, p.z) + 0.13f
            if (p.y < gy) { p.y = gy; g.vel.y = -g.vel.y * 0.34f; g.vel.x *= 0.6f; g.vel.z *= 0.6f }
            g.mi.transform.setTranslation(p)
            if (g.t <= 0f) {
                g.on = false
                g.mi.transform.setToScaling(0f, 0f, 0f)
                onExplode(p)
            }
        }
    }
}
