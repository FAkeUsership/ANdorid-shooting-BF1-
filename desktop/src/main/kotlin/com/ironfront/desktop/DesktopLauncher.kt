package com.ironfront.desktop

import com.badlogic.gdx.backends.lwjgl3.Lwjgl3Application
import com.badlogic.gdx.backends.lwjgl3.Lwjgl3ApplicationConfiguration
import com.ironfront.core.IronFrontGame

/* Fast iteration target: run the same game on PC.
   gradle :desktop:run  */
fun main() {
    val cfg = Lwjgl3ApplicationConfiguration()
    cfg.setTitle("IRONFRONT 1917")
    cfg.setWindowedMode(1280, 800)
    cfg.useVsync(true)
    Lwjgl3Application(IronFrontGame(), cfg)
}
