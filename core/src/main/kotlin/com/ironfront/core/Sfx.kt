package com.ironfront.core

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.audio.Sound
import java.io.ByteArrayOutputStream
import java.io.File
import kotlin.math.exp
import kotlin.math.sin
import kotlin.random.Random

/* ============================================================
   S6 — runtime-synthesized SFX. WAV bytes generated in code
   (no shipped audio assets), written to temp files, played via
   libGDX. Same idea as the web build's WebAudio synth.
   ============================================================ */
object Sfx {
    private val cache = mutableMapOf<String, Sound>()
    var enabled = true
    private const val SR = 22050
    private const val TAU = 6.2831853f

    private fun synth(name: String, seconds: Float, gen: (Float, Float) -> Float) {
        if (cache.containsKey(name)) return
        val n = (seconds * SR).toInt()
        val bytes = ByteArrayOutputStream()
        fun w16(v: Int) { bytes.write(v and 0xff); bytes.write((v shr 8) and 0xff) }
        fun w32(v: Int) { w16(v and 0xffff); w16((v ushr 16) and 0xffff) }
        val dataLen = n * 2
        bytes.write("RIFF".toByteArray()); w32(36 + dataLen); bytes.write("WAVE".toByteArray())
        bytes.write("fmt ".toByteArray()); w32(16); w16(1); w16(1); w32(SR); w32(SR * 2); w16(2); w16(16)
        bytes.write("data".toByteArray()); w32(dataLen)
        for (i in 0 until n) {
            val t = i.toFloat() / SR
            val s = gen(t, i.toFloat() / n).coerceIn(-1f, 1f)
            w16((s * 32767).toInt())
        }
        val f = File(File(System.getProperty("java.io.tmpdir")), "if17_$name.wav")
        f.writeBytes(bytes.toByteArray())
        f.deleteOnExit()
        cache[name] = Gdx.audio.newSound(Gdx.files.absolute(f.absolutePath))
    }

    private fun noise(r: Random) = r.nextFloat() * 2f - 1f

    fun init() {
        if (cache.isNotEmpty()) return
        val r = Random(7)
        synth("rifle", 0.25f) { t, _ ->
            noise(r) * exp(-t * 26f) * 0.9f + sin(TAU * 90f * t) * exp(-t * 40f) * 0.7f
        }
        synth("pistol", 0.14f) { t, _ -> noise(r) * exp(-t * 40f) * 0.7f }
        synth("hit", 0.06f) { t, _ -> (if (sin(TAU * 1300f * t) > 0f) 0.3f else -0.3f) * exp(-t * 30f) }
        synth("moan", 1.2f) { t, k ->
            val f = 85f - 30f * k
            sin(TAU * f * t + 4f * sin(6f * t)) * 0.22f * minOf(1f, t * 6f) * exp(-t * 1.4f)
        }
        synth("boom", 1.0f) { t, _ ->
            noise(r) * exp(-t * 5f) * 0.9f + sin(TAU * 45f * t) * exp(-t * 6f) * 0.8f
        }
        synth("hurt", 0.2f) { t, _ -> sin(TAU * (120f - 60f * t) * t) * exp(-t * 18f) * 0.6f }
        synth("reload", 0.5f) { t, _ ->
            val a = if (t in 0.00f..0.03f) 0.4f else 0f
            val b = if (t in 0.15f..0.18f) 0.4f else 0f
            val c = if (t in 0.38f..0.42f) 0.45f else 0f
            (a + b + c) * (noise(r) * 0.7f + 0.3f)
        }
    }

    fun play(name: String, vol: Float = 1f) {
        if (!enabled) return
        cache[name]?.play(vol)
    }
}
