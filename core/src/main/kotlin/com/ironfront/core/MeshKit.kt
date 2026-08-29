package com.ironfront.core

import com.badlogic.gdx.graphics.Mesh
import com.badlogic.gdx.graphics.VertexAttribute
import com.badlogic.gdx.graphics.VertexAttributes
import kotlin.math.cos
import kotlin.math.sin
import kotlin.random.Random

/* ============================================================
   S2 — vertex sink for merged low-poly static geometry.
   Everything appended here becomes ONE mesh → ONE draw call.
   Vertex format: position(3) + normal(3) + color(3).
   Winding is irrelevant: materials render double-sided.
   ============================================================ */
class MeshKit(seed: Int = 7) {
    private val v = ArrayList<Float>(16384)
    private val i = ArrayList<Int>(32768)
    private var vc = 0
    private val rnd = Random(seed)

    fun vert(x: Float, y: Float, z: Float, nx: Float, ny: Float, nz: Float, r: Float, g: Float, b: Float) {
        v.add(x); v.add(y); v.add(z)
        v.add(nx); v.add(ny); v.add(nz)
        v.add(r); v.add(g); v.add(b); v.add(1f)
        vc++
    }

    fun quad(ax: Float, ay: Float, az: Float,
             bx: Float, by: Float, bz: Float,
             cx: Float, cy: Float, cz: Float,
             dx: Float, dy: Float, dz: Float,
             nx: Float, ny: Float, nz: Float,
             r: Float, g: Float, b: Float) {
        val b0 = vc
        vert(ax, ay, az, nx, ny, nz, r, g, b)
        vert(bx, by, bz, nx, ny, nz, r, g, b)
        vert(cx, cy, cz, nx, ny, nz, r, g, b)
        vert(dx, dy, dz, nx, ny, nz, r, g, b)
        i.add(b0); i.add(b0 + 1); i.add(b0 + 2)
        i.add(b0); i.add(b0 + 2); i.add(b0 + 3)
    }

    /** axis box rotated around Y, centered (cx,cy,cz) */
    fun box(cx: Float, cy: Float, cz: Float, sx: Float, sy: Float, sz: Float, ry: Float,
            r: Float, g: Float, b: Float, vary: Float = 0.08f) {
        val m = 1f + (rnd.nextFloat() * 2f - 1f) * vary
        val cr = (r * m).coerceIn(0f, 1f); val cg = (g * m).coerceIn(0f, 1f); val cb = (b * m).coerceIn(0f, 1f)
        val hx = sx / 2f; val hy = sy / 2f; val hz = sz / 2f
        val c = cos(ry); val s = sin(ry)
        fun tx(x: Float, z: Float) = cx + x * c + z * s
        fun tz(x: Float, z: Float) = cz - x * s + z * c
        fun nx(x: Float, z: Float) = x * c + z * s
        fun nz(x: Float, z: Float) = -x * s + z * c

        quad(tx(-hx, -hz), cy + hy, tz(-hx, -hz), tx(hx, -hz), cy + hy, tz(hx, -hz),
             tx(hx, hz), cy + hy, tz(hx, hz), tx(-hx, hz), cy + hy, tz(-hx, hz), 0f, 1f, 0f, cr, cg, cb)
        quad(tx(-hx, -hz), cy - hy, tz(-hx, -hz), tx(-hx, hz), cy - hy, tz(-hx, hz),
             tx(hx, hz), cy - hy, tz(hx, hz), tx(hx, -hz), cy - hy, tz(hx, -hz), 0f, -1f, 0f, cr, cg, cb)
        var n = Pair(nx(1f, 0f), nz(1f, 0f))
        quad(tx(hx, -hz), cy - hy, tz(hx, -hz), tx(hx, hz), cy - hy, tz(hx, hz),
             tx(hx, hz), cy + hy, tz(hx, hz), tx(hx, -hz), cy + hy, tz(hx, -hz), n.first, 0f, n.second, cr, cg, cb)
        n = Pair(nx(-1f, 0f), nz(-1f, 0f))
        quad(tx(-hx, -hz), cy - hy, tz(-hx, -hz), tx(-hx, -hz), cy + hy, tz(-hx, -hz),
             tx(-hx, hz), cy + hy, tz(-hx, hz), tx(-hx, hz), cy - hy, tz(-hx, hz), n.first, 0f, n.second, cr, cg, cb)
        n = Pair(nx(0f, 1f), nz(0f, 1f))
        quad(tx(-hx, hz), cy - hy, tz(-hx, hz), tx(-hx, hz), cy + hy, tz(-hx, hz),
             tx(hx, hz), cy + hy, tz(hx, hz), tx(hx, hz), cy - hy, tz(hx, hz), n.first, 0f, n.second, cr, cg, cb)
        n = Pair(nx(0f, -1f), nz(0f, -1f))
        quad(tx(-hx, -hz), cy - hy, tz(-hx, -hz), tx(hx, -hz), cy - hy, tz(hx, -hz),
             tx(hx, -hz), cy + hy, tz(hx, -hz), tx(-hx, -hz), cy + hy, tz(-hx, -hz), n.first, 0f, n.second, cr, cg, cb)
    }

    /** 4-sided spike (grass tufts, hills, spires) */
    fun spike(cx: Float, cy: Float, cz: Float, s: Float, h: Float,
              r: Float, g: Float, b: Float, vary: Float = 0.1f) {
        val m = 1f + (rnd.nextFloat() * 2f - 1f) * vary
        val cr = (r * m).coerceIn(0f, 1f); val cg = (g * m).coerceIn(0f, 1f); val cb = (b * m).coerceIn(0f, 1f)
        val hs = s / 2f
        val base = floatArrayOf(
            cx - hs, cy, cz - hs,  cx + hs, cy, cz - hs,  cx + hs, cy, cz + hs,  cx - hs, cy, cz + hs)
        for (k in 0 until 4) {
            val a = k * 3; val b0 = ((k + 1) % 4) * 3
            val s0 = vc
            vert(base[a], base[a + 1], base[a + 2], 0f, 0.5f, 0f, cr, cg, cb)
            vert(base[b0], base[b0 + 1], base[b0 + 2], 0f, 0.5f, 0f, cr, cg, cb)
            vert(cx, cy + h, cz, 0f, 1f, 0f, cr, cg, cb)
            i.add(s0); i.add(s0 + 1); i.add(s0 + 2)
        }
        quad(base[0], base[1], base[2], base[9], base[10], base[11],
             base[6], base[7], base[8], base[3], base[4], base[5], 0f, -1f, 0f, cr, cg, cb)
    }

    val vertexCount: Int get() = vc

    fun mesh(): Mesh {
        val mesh = Mesh(
            true, false, vc, i.size,
            VertexAttributes(
                VertexAttribute(VertexAttributes.Usage.Position, 3, "a_position"),
                VertexAttribute(VertexAttributes.Usage.Normal, 3, "a_normal"),
                VertexAttribute(VertexAttributes.Usage.ColorUnpacked, 4, "a_color")))
        mesh.setVertices(v.toFloatArray())
        mesh.setIndices(i.map { it.toShort() }.toShortArray())
        return mesh
    }
}
