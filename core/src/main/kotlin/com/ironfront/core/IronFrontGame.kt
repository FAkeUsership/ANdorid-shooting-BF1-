package com.ironfront.core

import com.badlogic.gdx.ApplicationAdapter
import com.badlogic.gdx.Gdx
import com.badlogic.gdx.Input
import com.badlogic.gdx.InputProcessor
import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.PerspectiveCamera
import com.badlogic.gdx.graphics.g2d.BitmapFont
import com.badlogic.gdx.graphics.g3d.Environment
import com.badlogic.gdx.graphics.g3d.ModelBatch
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.environment.DirectionalLight
import com.badlogic.gdx.math.MathUtils
import com.badlogic.gdx.math.Vector3
import com.badlogic.gdx.scenes.scene2d.Stage
import com.badlogic.gdx.scenes.scene2d.ui.Label
import com.badlogic.gdx.utils.ScreenUtils

/* ============================================================
   IRONFRONT 1917 — libGDX core.
   S1 skeleton: state machine + FPS camera + touch/mouse look +
   WASD move + placeholder world. Later sections layer on:
   S2 world · S3 characters · S4 weapons · S5 AI · S6 audio ·
   S7 HUD/story UI · S8 saves · S9 perf/release.
   ============================================================ */
class IronFrontGame : ApplicationAdapter(), InputProcessor {

    enum class State { MENU, STORY, PLAYING, PAUSED, DEAD, COMPLETE }

    private lateinit var camera: PerspectiveCamera
    private lateinit var batch: ModelBatch
    private lateinit var env: Environment
    private lateinit var stage: Stage
    private lateinit var hudLabel: Label
    private val world = WorldBuilder()

    var state: State = State.MENU
        private set

    // FPS look / move
    private var yaw = 0f
    private var pitch = 0f
    private val pos = Vector3(0f, 1.62f, 46f)
    private val heldKeys = mutableSetOf<Int>()

    override fun create() {
        camera = PerspectiveCamera(75f, Gdx.graphics.width.toFloat(), Gdx.graphics.height.toFloat())
        camera.near = 0.1f
        camera.far = 400f

        env = Environment()
        env.set(ColorAttribute.createAmbientLight(0.58f, 0.6f, 0.52f, 1f))
        env.add(DirectionalLight().set(Color(1f, 0.95f, 0.82f, 1f), Vector3(-0.4f, -0.8f, -0.45f)))

        world.build()
        batch = ModelBatch()

        stage = Stage()
        hudLabel = Label("IRONFRONT 1917",
            Label.LabelStyle(BitmapFont(), Color(0.91f, 0.89f, 0.81f, 1f)))
        hudLabel.setFontScale(2f)
        hudLabel.setPosition(24f, Gdx.graphics.height - 70f)
        stage.addActor(hudLabel)

        Gdx.input.inputProcessor = this
    }

    override fun render() {
        val dt = Gdx.graphics.deltaTime.coerceAtMost(0.05f)
        update(dt)

        ScreenUtils.clear(0.61f, 0.63f, 0.52f, 1f)
        batch.begin(camera)
        batch.render(world.instances, env)
        batch.end()

        stage.act(dt)
        stage.draw()
    }

    private fun update(dt: Float) {
        if (state == State.PLAYING) {
            var fx = 0f; var fz = 0f
            if (Input.Keys.W in heldKeys || Input.Keys.UP in heldKeys)  { fx += -MathUtils.sin(yaw); fz += -MathUtils.cos(yaw) }
            if (Input.Keys.S in heldKeys || Input.Keys.DOWN in heldKeys){ fx +=  MathUtils.sin(yaw); fz +=  MathUtils.cos(yaw) }
            if (Input.Keys.A in heldKeys || Input.Keys.LEFT in heldKeys){ fx += -MathUtils.cos(yaw); fz +=  MathUtils.sin(yaw) }
            if (Input.Keys.D in heldKeys || Input.Keys.RIGHT in heldKeys){fx +=  MathUtils.cos(yaw); fz += -MathUtils.sin(yaw) }
            val l = kotlin.math.sqrt((fx * fx + fz * fz).toDouble()).toFloat()
            if (l > 0.01f) {
                val speed = if (Input.Keys.SHIFT_LEFT in heldKeys) 7.9f else 5f
                pos.x += fx / l * speed * dt
                pos.z += fz / l * speed * dt
                val r = kotlin.math.sqrt((pos.x * pos.x + pos.z * pos.z).toDouble()).toFloat()
                if (r > Config.MAP_RADIUS) { pos.x *= Config.MAP_RADIUS / r; pos.z *= Config.MAP_RADIUS / r }
            }
        }
        camera.position.set(pos)
        camera.direction.set(
            -MathUtils.sin(yaw) * MathUtils.cos(pitch),
            MathUtils.sin(pitch),
            -MathUtils.cos(yaw) * MathUtils.cos(pitch))
        camera.up.set(0f, 1f, 0f)
        camera.update()

        hudLabel.setText(when (state) {
            State.MENU -> "IRONFRONT 1917 — TAP TO DEPLOY"
            State.PLAYING -> "WAVE 1 / 3 — HOLD THE TRENCH"
            else -> state.name
        })
    }

    /* ---------------- input ---------------- */
    override fun keyDown(keycode: Int): Boolean {
        heldKeys += keycode
        if (keycode == Input.Keys.ESCAPE && state == State.PLAYING) state = State.PAUSED
        return true
    }
    override fun keyUp(keycode: Int): Boolean { heldKeys -= keycode; return true }
    override fun keyTyped(character: Char) = false

    override fun touchDown(x: Int, y: Int, pointer: Int, button: Int): Boolean {
        if (state == State.MENU) state = State.PLAYING
        else if (state == State.PAUSED) state = State.PLAYING
        return true
    }
    override fun touchUp(x: Int, y: Int, pointer: Int, button: Int) = true
    override fun touchCancelled(x: Int, y: Int, pointer: Int, button: Int) = true

    override fun touchDragged(x: Int, y: Int, pointer: Int): Boolean {
        if (state != State.PLAYING) return false
        yaw -= Gdx.input.deltaX * 0.004f
        pitch = (pitch - Gdx.input.deltaY * 0.004f).coerceIn(-1.45f, 1.45f)
        return true
    }

    override fun mouseMoved(x: Int, y: Int) = false
    override fun scrolled(amountX: Float, amountY: Float) = false

    override fun resize(width: Int, height: Int) {
        camera.viewportWidth = width.toFloat()
        camera.viewportHeight = height.toFloat()
        camera.update()
        stage.viewport.update(width, height, true)
    }

    override fun dispose() {
        batch.dispose()
        stage.dispose()
        world.dispose()
    }
}
