# Hardware Sandbox: Zeros & Ones

> Step into the hidden digital battlefield of the silicon world. Play as a software engineer trapped inside the fractured circuitry of a broken computer. Purge the logic boards of critical system glitches, bypass dangerous security subroutines, and fix corrupted hardware blocks. As you navigate through treacherous platformer maps and evade hostile enemies, you will master the art of computer engineering — learning to construct a fully functional computer from simple logic gates up to the core ALU, saving the system from complete digital shutdown.

**Showcase:** [ Gameplay Showcase](https://drive.google.com/file/d/1ztGmuia7WsatX4CLqymFIH1AGd1E85ka/view?usp=drive_link) 

---

## Table of Contents

1. [Game Overview](#1-game-overview)
2. [Game Flow](#2-game-flow)
3. [How to Play](#3-how-to-play)
4. [Controls](#4-controls)
5. [Objectives & Goals](#5-objectives--goals)
6. [Game Mechanics](#6-game-mechanics)
7. [Logic Gates Reference](#7-logic-gates-reference)
8. [Tips and Tricks](#8-tips-and-tricks)
9. [Credits](#9-credits)
10. [Asset Links](#10-asset-links)
11. [Project Management](#11-project-management)

---

## 1. Game Overview

| Field | Details |
|---|---|
| **Title** | Hardware Sandbox: Zeros & Ones |
| **Genre** | 2D Side-Scroller / Action / Puzzle / Simulation |
| **Platform** | Windows, Linux |
| **Engine** | Unity 2022.x+ (C#) |
| **Repository** | GitHub — feature-branch workflow |

---

## 2. Game Flow

The game is structured as a connected sequence of scenes:

```
Main Menu
    │
    ▼
First Map  ──────────────────────────────────────────────────────────────
│  Exploration platformer world. Navigate to gate terminals to enter      │
│  challenge rooms. Thunder hazards patrol certain areas.                 │
│                                                                         │
│  Gates: NAND · NOT · AND · OR · XOR · HalfAdder · FullAdder            │
│         AND16 · NOT16 · OR16 · Add16 · Inc16                           │
└──────────────────────────────────────────────────────────────────────
    │  (complete first map gates to unlock)
    ▼
Second Map ───────────────────────────────────────────────────────────────
│  More complex multi-level world with a tech/circuit-board theme.        │
│  Thunder hazards are more frequent and cover greater map area.          │
│                                                                         │
│  Gates: Mux · Mux16 · Mux16_4Way · Mux16_8Way                         │
│         Dmux · Dmux4Way · Dmux8Way · Or4Way                            │
└──────────────────────────────────────────────────────────────────────
    │
    ▼
Challenge Scene (one per gate)
│  Isolated puzzle workspace. A countdown timer starts immediately.       │
│  The player must wire the correct logic circuit before time expires.    │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  Win / Pass / Failed / Game Over outcome panel shown on result  │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│  Exit button returns the player to the map at the correct spawn point. │
└──────────────────────────────────────────────────────────────────────
```

---

## 3. How to Play

The game blends platformer exploration with hardware logic puzzle-solving:

1. **Explore the map** — run, jump, and crouch through a 2D side-scrolling world. Avoid or bypass thunder hazards and enemies that drain your hearts.
2. **Find a gate terminal** — walk into a door labelled with a logic gate name. The game transitions to that gate's dedicated challenge scene.
3. **Solve the circuit** — drag the required gate components from your hotbar onto the workspace grid and wire them correctly to match the target truth table or specification.
4. **Beat the timer** — every challenge has a countdown. Submit your solution before it reaches zero to earn a Win or Pass result.
5. **Return to the map** — press the Exit button at any time to return to the exploration map. Your position is restored to the spawn point in front of the door you entered.
6. **Unlock and progress** — solving gates unlocks them permanently in your hotbar and marks them in the solved-components progress bar on the map.

---

## 4. Controls

### Map Exploration Mode (Keyboard)

| Key | Action |
|---|---|
| `A` / `D` or `←` / `→` | Move Left / Right |
| `W` / `Space` / `↑` | Jump (release early for a shorter jump) |
| `S` / `↓` | Crouch / Duck (also slows movement to 50%) |

### Circuit Simulation Mode (Mouse)

| Input | Action |
|---|---|
| **Left Click** | Click-and-drag terminal nodes to spawn or connect wires; select UI buttons; drag gates from the hotbar onto the grid |
| **Right Click + Drag** | Pan the camera viewport across the workspace canvas |
| **Scroll Wheel** | Zoom in and out within strict canvas boundary limits |

---

## 5. Objectives & Goals

- **Explore & Survive** — Navigate through two dangerous platformer maps while managing your 3-heart health bar against thunder hazards and enemies.
- **Unlock the Arsenal** — Complete logic gate challenges to permanently unlock advanced components (`Mux16_8Way`, `Add16`, `Or4Way`, etc.) into your hotbar.
- **Master Computer Engineering** — Cascade adders, selectors, and utility buses through both maps until you successfully build and configure a complete **Hack Computer 16-Bit ALU**.
- **Beat the Clock** — Complete each circuit challenge before the countdown timer expires. Faster solutions earn better outcome ratings.

---

## 6. Game Mechanics

### Player Health & Damage
The player has **3 hearts (6 half-heart units)** of health.
- Taking damage from a thunder hazard or an enemy removes half a heart and triggers a brief **invincibility window** — you cannot take further damage until the blink animation ends.
- Hearts are displayed in the **HeartPanel** UI in the top-left corner, showing full, half, and empty heart sprites in real time.
- When all hearts are depleted, the **Game Over** panel appears — you can **Play Again** (reload the current map) or return to the **Main Menu**.

### Challenge Outcome Panels
At the end of a challenge scene, one of four panels is shown:

| Panel | Trigger |
|---|---|
| **Win** | Correct solution submitted with time remaining |
| **Pass** | Challenge completed under an alternate pass condition |
| **Failed** | Timer reached 00:00 before a correct solution was submitted |
| **Game Over** | Player's health was fully depleted mid-challenge |

### Thunder Hazards
`SpriteBlinker` sprites appear and disappear on a timed interval with sound. A `ThunderDamage` trigger is active **only while the sprite is visible** — walking under a hidden lightning strike is safe.

### Doors & Portals
- **Doors** (`DoorController`) are placed at gate terminals throughout the maps. Walking into one loads the corresponding challenge scene and records the door's scene name for the return spawn.
- **Portals** (`PortalController`) are used for optional zone transitions. Stand on a portal and press `W` / `↑` to enter.

### Spawn System
When returning from a challenge or a portal, `PlayerSpawner` reads the last scene name and teleports the player to the matching named spawn-point Transform in the current map. This ensures the player always returns to the correct door.

### Advanced Cabling Framework
- **Multi-Bit Width Support** — the wire engine processes logic signals across 1-bit, 2-bit, 3-bit, 8-bit, and 16-bit structures.
- **Dynamic Source-Color Matching** — newly spawned wires automatically inherit the UI color of their source port, making complex parallel bus layouts highly readable.
- **9-Cable Fan-out** — every terminal node supports up to 9 outgoing wire connections from a single port.

### Port & Node Interface Rules
- **1-Bit ports** enforce a strict 1-to-1 rule — no secondary connection is allowed.
- **Multi-bit ports** (2, 3, 8, 16-bit) accept either one wide-bus cable or a cluster of matching 1-bit wires.
- **Bit-by-Bit Mode** — multi-bit cables include a UI toggle to select and manipulate an individual bit channel or route the entire bus as a single entity.

### Dynamic Shared Canvas Framework
- A **single reusable workspace prefab** is shared across all challenge scenes — no scene duplication.
- When a challenge initialises, it injects its specific constraints, gate descriptions, and countdown timer into the shared canvas at runtime.

### Locked Progression Tray & Drag-and-Drop Dock
- The bottom hotbar loads only the gates the player has already unlocked in previous maps.
- Built with Unity pointer events (`IBeginDragHandler`, `IDragHandler`, `IEndDragHandler`), letting players drag gate templates from the tray and drop them onto the active simulation grid's **green placement zone**.

### Hint Book (`!` Button)
Pressing the `!` button opens a paginated image reference book. Each page shows a diagram or truth table for a logic gate. Only gates the player has already solved are accessible.

### Solved Components Progress Bar
A paginated bar on the exploration map (3 items per page, with Next / Prev buttons) shows every gate the player has successfully solved. Completing a challenge marks it permanently.

### Local JSON Save System
- Uses `JsonUtility` to serialize save data to `Application.persistentDataPath`.
- Captures gate positions (X/Y world coordinates), GUID instance references, and toggled component flags.
- On load, rebuilds all `LineRenderer` wire networks by remapping stored endpoints to their matching named ports.

### Enemy AI & Combat System
- **Patrol & Proximity Chase AI** (`PatrolEnemy`): Enemies cycle through predefined waypoints in patrol mode. When the player enters the detection radius, the enemy switches to chase mode at accelerated speed.
- **Combat & Shooting** (`PlayerShooting`, `PlayerProjectile`): The player can fire projectiles (`Space` or Left Click) to engage and neutralize enemies from a distance.
- **Damage & Destruction** (`EnemyDamage`, `EnemyHealth`): Contact with enemies deals damage to the player's hearts, while enemy health depletes upon projectile impacts.

### Cheat & Debugging Manager *(Instructor Bonus Feature)*
- A dedicated in-game debugging panel and hotkey suite is available in the Unity Editor and Development Builds (press **`F1`** to toggle).
- Includes god mode (`F2`), full heal (`F3`), no-clip flight (`F4`), kill all enemies (`F5`), auto-solve puzzle (`F6`), unlock all gates (`F7`), scene navigation (`F8`/`F9`), and reload (`F10`).
- Fully isolated via `#if UNITY_EDITOR || DEVELOPMENT_BUILD` (see [CHEATS.md](file:///Assets/Scripts/Debugging/CHEATS.md) for full details).

---

## 7. Logic Gates Reference

### First Map Gates

| Gate | Description |
|---|---|
| `NAND` | Universal base gate — NOT of AND |
| `NOT` | Bitwise inverter |
| `AND` | 1-bit AND |
| `OR` | 1-bit OR |
| `XOR` | 1-bit XOR |
| `HalfAdder` | Adds two bits; outputs Sum and Carry |
| `FullAdder` | Adds two bits + Carry-in; outputs Sum and Carry-out |
| `AND16` | 16-bit bitwise AND with Single Bit / All Bits mode |
| `NOT16` | 16-bit bitwise NOT |
| `OR16` | 16-bit bitwise OR |
| `Add16` | 16-bit binary adder |
| `Inc16` | 16-bit incrementer (input + 1) |

### Second Map Gates

| Gate | Description |
|---|---|
| `Mux` | 1-bit 2-to-1 multiplexer |
| `Mux16` | 16-bit 2-to-1 multiplexer |
| `Mux16_4Way` | 16-bit 4-to-1 multiplexer |
| `Mux16_8Way` | 16-bit 8-to-1 multiplexer |
| `Dmux` | 1-bit 1-to-2 demultiplexer |
| `Dmux4Way` | 1-bit 1-to-4 demultiplexer |
| `Dmux8Way` | 1-bit 1-to-8 demultiplexer |
| `Or4Way` | 4-input OR — true if any input is 1 |

---

## 8. Tips and Tricks

- **Use the Hint Book** — if you forget how a component works, press `!` to open the reference manual. It shows diagrams and truth tables for every gate you have already solved.
- **Watch the wire colors** — wires always match the color of their source port. In complex 16-bit layouts, color-tracking is the fastest way to trace a broken connection.
- **Check port labels** — hover over a port to see its identifier. On multi-output chips like `FullAdder`, this prevents mixing up the `Sum` and `Carry` outputs.
- **Crouch under hazards** — thunder strikes hit a standing player but may miss a crouched one depending on collider placement.
- **Manage your invincibility window** — after taking a hit you are briefly immune. Use that window to reposition away from chained hazards before the blink ends.
- **Exit and re-enter freely** — pressing Exit in a challenge scene saves nothing but also costs nothing. Re-entering the door restarts the timer from full.

---

## 9. Credits

| Role | Responsibilities |
|---|---|
| **Mohammad Edkidek — Logic & Simulation Core** | Dynamic cable system, generic shared canvas pipeline, all logic gate scripts, Hack ALU architecture, JSON save/load serialization |
| **Numan Alsharabati — Maps & Adventuring Systems** | Platformer map layouts, player controller, health & damage system, enemy behavior, portal & door transitions, challenge status panels |

---

## 10. Asset Links

| Asset | Source |
|---|---|
| UI Textures & TextMeshPro Materials | *Unity Package* |
| Dungeon Floor & Wall Tiles | `[Assets/Assits_imported/DungeonFloorsAndWallsSamples/](https://assetstore.unity.com/packages/2d/textures-materials/stone/dungeon-floors-and-walls-20-free-samples-280741)` |
| Fantasy Wooden GUI (Free) | `[Assets/Assits_imported/Fantasy Wooden GUI Free/](https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811)` |
| Sprites & Animation Packs | *(link pending)* |
| SFX & Sound Tracking | *(link pending)* |

---

## 11. Project Management

- **Repository:** GitHub
- **Task Tracking:** GitHub Issues & Project Board checklists
- **Branch Strategy:** Feature-based branches with structured names
  - `main` — stable, reviewed code only
  - `1-main-menu` — main menu scene work
  - `2-map-player-controller` — map and player system work
  - `feature/<name>` — individual feature branches
