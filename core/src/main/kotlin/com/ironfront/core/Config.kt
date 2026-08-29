package com.ironfront.core

/* ============================================================
   IRONFRONT 1917 — game data (Kotlin port of the web prototype).
   ALL content lives here: weapons, enemies, missions, story.
   Add missions/weapons/enemies without touching engine code.
   ============================================================ */

data class WeaponDef(
    val id: String, val name: String, val slot: Int,
    val dmg: Float, val mag: Int, val reserve: Int,
    val rate: Float, val reload: Float,
    val spread: Float, val adsSpread: Float,
    val auto: Boolean,
    val unlockedAtStart: Boolean,
    val unlockAfterMission: String? = null
)

data class EnemyDef(
    val id: String, val name: String,
    val hp: Float, val speedMin: Float, val speedMax: Float,
    val dmg: Float, val attackRange: Float, val attackRate: Float,
    val score: Int, val melee: Boolean, val riseFromGround: Boolean
)

data class StoryPage(val chapter: String, val text: String)
data class WaveDef(val list: List<Pair<String, Int>>, val interval: Float)

sealed class MissionPhase {
    data class Waves(val spawnZone: String, val waves: List<WaveDef>) : MissionPhase()
    data class Reach(val x: Float, val z: Float, val r: Float, val label: String) : MissionPhase()
    data class Defend(val time: Float, val spawnEvery: Float, val list: List<String>, val label: String) : MissionPhase()
}

data class MissionDef(
    val id: String, val name: String, val chapter: String,
    val desc: String, val difficulty: String,
    val requires: String?,
    val startX: Float, val startZ: Float,
    val objectiveShort: String,
    val story: List<StoryPage>,
    val phases: List<MissionPhase>,
    val completeText: String, val unlocksText: String
)

object Config {
    const val MAP_RADIUS = 150f
    const val MAX_HP = 100f
    const val REGEN_DELAY = 6f
    const val REGEN_RATE = 14f
    const val MAX_GRENADES = 3
    const val HEADSHOT_MULT = 2.2f
    const val INTERMISSION = 11f

    val weapons: Map<String, WeaponDef> = listOf(
        WeaponDef("rifle", "G-17 BOLT RIFLE", 1, 46f, 5, 45, 1.05f, 2.7f, 0.0035f, 0.0004f, false, true),
        WeaponDef("pistol", "M1912 PISTOL", 2, 21f, 8, 64, 0.17f, 1.7f, 0.014f, 0.005f, false, true),
        WeaponDef("smg", "HELLRIEGEL SMG", 3, 15f, 32, 128, 0.092f, 2.4f, 0.028f, 0.012f, true, false, "m1"),
    ).associateBy { it.id }

    val enemies: Map<String, EnemyDef> = listOf(
        EnemyDef("zombie", "RISEN INFANTRY", 42f, 2.1f, 3.1f, 16f, 1.8f, 1.15f, 10, true, true),
        EnemyDef("runner", "RISEN RUNNER", 26f, 4.3f, 5.2f, 12f, 1.7f, 0.85f, 15, true, true),
        EnemyDef("soldier", "GREY SOLDIER", 75f, 2.0f, 2.6f, 9f, 26f, 1.9f, 25, false, false),
    ).associateBy { it.id }

    val missions: List<MissionDef> = listOf(
        MissionDef(
            id = "m1", name = "DEVIL'S DAWN", chapter = "MISSION I — THE SOMME, 1917",
            desc = "Hold the forward trench against the rising dead.", difficulty = "★★☆",
            requires = null, startX = 0f, startZ = 46f,
            objectiveShort = "HOLD THE TRENCH LINE",
            story = listOf(
                StoryPage("PROLOGUE — OCTOBER, 1917",
                    "Three days the guns have not stopped.\nThree days of mud, and rain, and wire.\n\nThen, at dawn, the shelling ceased —\nand something worse began."),
                StoryPage("PROLOGUE",
                    "A green fog crawls across No Man's Land.\nThe wires hiss. The craters stir.\n\nThe men we lost yesterday are standing up."),
                StoryPage("ORDERS",
                    "You are the last rifleman of 3rd Squad.\n\nHOLD THE TRENCH. SURVIVE THE WAVES.\nWhatever used to wear our uniforms —\nit is no longer one of us."),
            ),
            phases = listOf(
                MissionPhase.Waves("cratersFront", listOf(
                    WaveDef(listOf("zombie" to 7), 1.1f),
                    WaveDef(listOf("zombie" to 8, "runner" to 3), 0.95f),
                    WaveDef(listOf("zombie" to 9, "runner" to 4, "soldier" to 2), 0.85f),
                ))
            ),
            completeText = "THE TRENCH HELD. FOR NOW.",
            unlocksText = "HELLRIEGEL SMG UNLOCKED — NEXT OPERATION AVAILABLE",
        ),
        MissionDef(
            id = "m2", name = "NO MAN'S LAND", chapter = "MISSION II — THE GREY CHURCH",
            desc = "Cross the craters to the ruined church. Survive what follows.", difficulty = "★★★",
            requires = "m1", startX = 0f, startZ = 46f,
            objectiveShort = "REACH THE RUINED CHURCH",
            story = listOf(
                StoryPage("MISSION II",
                    "The dead do not hold ground.\nThey walk. Endlessly, they walk.\n\nTheir trail leads to the ruined church\nat the far edge of No Man's Land."),
                StoryPage("ORDERS",
                    "Cross the open ground. Reach the church.\nFind what is calling them — and hold it\nuntil the light fails.\n\nMove fast. The land itself will claw at you."),
            ),
            phases = listOf(
                MissionPhase.Reach(-85f, -85f, 9f, "REACH THE RUINED CHURCH"),
                MissionPhase.Defend(50f, 2.4f, listOf("zombie", "zombie", "runner", "soldier"), "HOLD THE CHURCH UNTIL DUSK"),
            ),
            completeText = "THE GUNS FELL SILENT AT DUSK.",
            unlocksText = "THE FRONT IS QUIET… FOR NOW. MORE OPERATIONS SOON.",
        ),
    )
}
