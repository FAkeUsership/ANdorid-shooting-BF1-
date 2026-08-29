package com.ironfront.core

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.Pixmap
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.BitmapFont
import com.badlogic.gdx.graphics.g2d.SpriteBatch

/* ============================================================
   S7 — SpriteBatch HUD: menus, combat HUD, touch buttons.
   Touch layout rects shared with input handling in the game.
   ============================================================ */
object Hud {
    lateinit var batch: SpriteBatch
    lateinit var font: BitmapFont
    lateinit var big: BitmapFont
    lateinit var white: Texture

    val BEIGE = Color(0.91f, 0.89f, 0.81f, 1f)
    val DIM = Color(0.72f, 0.68f, 0.56f, 1f)
    val GOLD = Color(0.85f, 0.7f, 0.3f, 1f)
    val RED = Color(0.8f, 0.15f, 0.1f, 1f)
    val PANEL = Color(0.08f, 0.09f, 0.05f, 0.55f)

    fun init() {
        batch = SpriteBatch()
        font = BitmapFont().apply { data.setScale(1.1f) }
        big = BitmapFont().apply { data.setScale(3f) }
        val pm = Pixmap(4, 4, Pixmap.Format.RGBA8888)
        pm.setColor(Color.WHITE); pm.fill()
        white = Texture(pm)
    }

    fun rect(x: Float, y: Float, w: Float, h: Float, c: Color) {
        batch.color = c; batch.draw(white, x, y, w, h); batch.color = Color.WHITE
    }
    fun text(s: String, x: Float, y: Float, c: Color = BEIGE) {
        font.color = c; font.draw(batch, s, x, y)
    }

    val W: Int get() = Gdx.graphics.width
    val H: Int get() = Gdx.graphics.height

    // touch button rects (y-up screen space)
    fun fireRect() = floatArrayOf(W - 150f, 40f, 120f, 120f)
    fun grenadeRect() = floatArrayOf(W - 160f, 180f, 62f, 62f)
    fun reloadRect() = floatArrayOf(W - 70f, 190f, 60f, 60f)
    fun swapRect() = floatArrayOf(W - 240f, 60f, 62f, 62f)
    fun pauseRect() = floatArrayOf(W - 70f, H - 70f, 60f, 60f)

    fun inRect(r: FloatArray, x: Float, y: Float) = x >= r[0] && x <= r[0] + r[2] && y >= r[1] && y <= r[1] + r[3]

    fun drawButtons(grenades: Int) {
        val fr = fireRect(); rect(fr[0], fr[1], fr[2], fr[3], Color(0.2f, 0.22f, 0.12f, 0.6f))
        text("FIRE", fr[0] + 38f, fr[1] + 68f, BEIGE)
        val gr = grenadeRect(); rect(gr[0], gr[1], gr[2], gr[3], Color(0.2f, 0.22f, 0.12f, 0.6f))
        text("G x$grenades", gr[0] + 8f, gr[1] + 36f, BEIGE)
        val rr = reloadRect(); rect(rr[0], rr[1], rr[2], rr[3], Color(0.2f, 0.22f, 0.12f, 0.6f))
        text("RLD", rr[0] + 12f, rr[1] + 36f, BEIGE)
        val sr = swapRect(); rect(sr[0], sr[1], sr[2], sr[3], Color(0.2f, 0.22f, 0.12f, 0.6f))
        text("SWAP", sr[0] + 8f, sr[1] + 36f, BEIGE)
    }

    fun drawStick(bx: Float, by: Float, kx: Float, ky: Float) {
        rect(bx - 55f, by - 55f, 110f, 110f, Color(0.3f, 0.32f, 0.2f, 0.3f))
        rect(bx + kx - 26f, by + ky - 26f, 52f, 52f, Color(0.8f, 0.78f, 0.6f, 0.5f))
    }

    fun crosshair(spread: Float) {
        val cx = W / 2f; val cy = H / 2f; val o = 8f + spread * 40f
        rect(cx - 1.5f, cy + o, 3f, 10f, BEIGE); rect(cx - 1.5f, cy - o - 10f, 3f, 10f, BEIGE)
        rect(cx - o - 10f, cy - 1.5f, 10f, 3f, BEIGE); rect(cx + o, cy - 1.5f, 10f, 3f, BEIGE)
    }
}
