/* ============================================================
   IRONFRONT 1917 — main.js
   Boot + console API for extending the game.
   ============================================================ */
'use strict';

window.addEventListener('DOMContentLoaded', () => {
  Game.boot();

  /* Handy console API while you extend the game:
     IRONFRONT.Game                 – engine/state
     IRONFRONT.addEnemyType(def)    – register a new enemy type at runtime
     IRONFRONT.addMission(mission)  – push a new mission into the menu    */
  window.IRONFRONT = {
    Game, Player, Enemies, Weapons, World, CONFIG, WEAPONS, ENEMY_TYPES, MISSIONS,
    addEnemyType(def) { ENEMY_TYPES[def.id] = def; console.log('enemy type added:', def.id); },
    addMission(mission) { MISSIONS.push(mission); Game.buildMissionList(); console.log('mission added:', mission.id); },
  };
});
