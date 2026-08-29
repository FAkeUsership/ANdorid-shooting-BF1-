package com.ironfront.core

import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.VertexAttributes
import com.badlogic.gdx.graphics.g3d.Material
import com.badlogic.gdx.graphics.g3d.Model
import com.badlogic.gdx.graphics.g3d.ModelInstance
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.utils.ModelBuilder
import com.badlogic.gdx.math.MathUtils

/* ============================================================
   S1 skeleton world: flat-shaded low-poly placeholders.
   Section S2 replaces this with the displaced terrain, trench
   carving, instanced props and ruined church from the design.
   ============================================================ */
class WorldBuilder {
    val instances = mutableListOf<ModelInstance>()
    private val models = mutableListOf<Model>()

    private val attrs: Long = (VertexAttributes.Usage.Position or
            VertexAttributes.Usage.Normal or
            VertexAttributes.Usage.ColorUnpacked).toLong()

    private fun mat(r: Float, g: Float, b: Float): Material =
        Material(ColorAttribute.createDiffuse(Color(r, g, b, 1f)))

    private fun box(w: Float, h: Float, d: Float, m: Material, x: Float, y: Float, z: Float, ry: Float = 0f): ModelInstance {
        val model = ModelBuilder().createBox(w, h, d, m, attrs)
        models += model
        val inst = ModelInstance(model)
        inst.transform.setToTranslation(x, y, z).rotate(0f, 1f, 0f, ry * MathUtils.radiansToDegrees)
        instances += inst
        return inst
    }

    fun build() {
        instances.clear(); models.clear()

        // ground slab (mud)
        box(340f, 2f, 340f, mat(0.36f, 0.28f, 0.19f), 0f, -1f, 0f)

        // trench parapet placeholder (sandbag line)
        for (x in -32..32 step 2) {
            box(1.9f, 0.7f, 0.9f, mat(0.55f, 0.44f, 0.28f), x.toFloat(), 0.35f, 42f, MathUtils.random(-4f, 4f))
        }
        // barbed-wire posts placeholder
        for (x in -70..70 step 6) {
            box(0.14f, 1.3f, 0.14f, mat(0.28f, 0.22f, 0.15f), x.toFloat() + MathUtils.random(-0.5f, 0.5f), 0.65f, 16f + MathUtils.random(-1f, 1f))
        }
        // a few crates / cover
        repeat(6) {
            box(1.6f, 1.2f, 1.6f, mat(0.42f, 0.32f, 0.18f),
                MathUtils.random(-40f, 40f), 0.6f, MathUtils.random(-20f, 10f), MathUtils.random(0f, 90f))
        }
    }

    fun dispose() {
        models.forEach { it.dispose() }
        models.clear(); instances.clear()
    }
}
