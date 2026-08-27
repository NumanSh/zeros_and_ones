# Project Evaluation & Grading Audit Report

**Project:** Zeros & Ones (`student-choice-project-f`)  
**Assessment:** Unity Course Project – Student Choice  
**Evaluation Date:** August 16, 2026  

---

## 📊 Estimated Grade Overview

| Category | Current Score | Max Score | Status |
| :--- | :---: | :---: | :--- |
| **Functionality** | **26** | 30 | ⚠️ Missing Pause Menu (`Time.timeScale = 0`) |
| **Gameplay Mechanics** | **20** | 20 | ✅ 5 distinct, complex mechanics implemented |
| **Code Quality & Readability** | **14** | 15 | ✅ Well-organized, clean namespaces & singletons |
| **Documentation** | **6** | 15 | 🛑 **CRITICAL: Root `README.md` is missing!** |
| **Conceptual Understanding** | **9** | 10 | ✅ Solid usage of physics, UI events, saving |
| **Project Management** | **8** | 10 | ⚠️ Needs team member role definitions in README |
| **BASE SUBTOTAL** | **83 / 100** | **100** | **Grade: B+ / A-** |
| **Bonus Points (AI / Cheat / Framework)** | **+8** | +35 | ✅ Enemy Patrol AI implemented |
| **CURRENT TOTAL ESTIMATED GRADE** | **91 / 100** | **100** | **Grade: A** |
| **POTENTIAL GRADE WITH FIXES** | **105+ / 100** | **135** | **Grade: A+ (With Bonus)** |

---

## 🔍 Detailed Rubric Breakdown & Code Audit

### 1. Gameplay Mechanics (20 / 20 pts - EXCELLENT)
The project successfully satisfies the requirement of **at least 5 distinct, moderately complex gameplay mechanics**:
1. **Interactive Logic Gate & Cable System** (`LogicGateWorkspace.cs`, `CableWire.cs`, `LogicGates_Type.cs`): Real-time circuit building and signal evaluation (AND, OR, XOR, MUX, DMUX, Adders).
2. **Persistence & Save/Load System** (`SaveManager.cs`): Custom JSON serialization saving placed items and puzzle state across scenes.
3. **Player Combat & Shooting System** (`PlayerShooting.cs`, `Projectile.cs`, `PlayerHealth.cs`, `HeartsUI.cs`): Shooting projectiles, cooldowns, and player health tracking.
4. **Enemy AI & Damage System** (`PatrolEnemy.cs`, `EnemyDamage.cs`, `EnemyHealth.cs`): Waypoint navigation AI, damage dealing, and destruction logic.
5. **Stage Progression & Portal System** (`PortalController.cs`, `DoorController.cs`): Conditional portal unlocking leading from `firstMap` to `SecondMap`.

---

### 2. Functionality (26 / 30 pts - GOOD)
* **What works well:** Scene transitions, multi-level progression (`firstMap` -> `SecondMap`), logic gate puzzle interaction, UI HUD.
* **What is missing (Required Baseline Game Flow):**
  * **Pause Menu:** Page 2 of instructions explicitly requires a functional Pause Menu (`Time.timeScale = 0`) with *Resume*, *Restart*, *Settings*, and *Exit to Main Menu*. Currently, `GameState.cs` has a `Paused` state, but there is no Pause UI controller script bound to `ESC` or `P`.

---

### 3. Documentation (6 / 15 pts - CRITICAL ACTION REQUIRED)
* **Missing Root `README.md`:** Page 7 of the PDF states that submitting without overwriting `README.md` using the course template will penalize the documentation score.
* **Required README Contents:**
  * Project overview & team member role definitions.
  * Controls list (Keyboard/Mouse instructions).
  * Asset list & credits.
  * **5–10 minute YouTube narrated video link** showcasing gameplay & code architecture.
  * Screenshots highlighting key game features.

---

### 4. Project Management & Code Quality (22 / 25 pts - VERY GOOD)
* Organized namespace structure (`ZerosAndOnes.Managers`, `ZerosAndOnes.UI`, `ZerosAndOnes.Gameplay`).
* Proper singleton pattern in `GameManager.cs` and `AudioManager.cs` using `DontDestroyOnLoad`.

---

## 🚀 Bonus Opportunities (+35 Max Available)

1. **Cheat Manager (Bonus +5 pts) - MISSING:**
   * Page 4 specifies a Cheat Manager for instructor debugging wrapped in `#if UNITY_EDITOR || DEVELOPMENT_BUILD`.
   * *Action:* Create `CheatManager.cs` with hotkeys (`F1` for Invincibility, `F2` to Skip Level/Unlock Portals, `F3` for Speed Boost).

2. **AI Implementation (Bonus +6 to +10 pts) - IMPLEMENTED:**
   * `PatrolEnemy.cs` features waypoint patrol AI (+6 to +8 pts).

3. **External Framework / API Integration (Bonus up to +10 pts):**
   * Can be claimed if you document `SaveManager.cs` JSON architecture or integrate a lightweight external dialogue/narrative package (e.g. Ink).

---

## 🛠️ Step-by-Step Action Plan to Reach 100+ / 100 (A+)

1. **Create `README.md` at root:** Create the full project `README.md` template with team roles, controls, screenshots, and YouTube video link placeholder.
2. **Implement `PauseMenu.cs`:** Add a Pause Canvas overlay handling `ESC` key press, setting `Time.timeScale = 0`, and wiring Resume, Restart, Settings, and Main Menu buttons.
3. **Implement `CheatManager.cs` (+5 Bonus):** Add a cheat controller script wrapped in `#if UNITY_EDITOR || DEVELOPMENT_BUILD`.
4. **Add Credits button in Main Menu:** Ensure `MainMenuController.cs` has a Credits popup or scene link as specified on page 2.
