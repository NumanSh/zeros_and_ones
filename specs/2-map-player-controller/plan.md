# Implementation Plan: Vertical Map & Player Controller

## Feature Summary
Design a vertical scrolling world map and a 2D player character controller (with running, jumping, and climbing physics). The player traverses platforms, climbs ladders to ascend levels, and enters logic portals that load corresponding level solver scenes.

---

## Technical Context
- **Engine**: Unity 6 (6000.4.1f1 LTS)
- **Physics**: Unity 2D Physics (`Rigidbody2D`, `BoxCollider2D`, triggers)
- **Tile System**: 2D Tilemap System (`Tilemap`, `TilemapRenderer`, `TilemapCollider2D`)
- **Assets**: 
  - Tiles: `Assets/BayatGames/Free Platform Game Assets/Tiles`
  - Player Sprites: `Assets/BayatGames/Free Platform Game Assets/Character`
  - Backgrounds: `Assets/1 Backgrounds`
- **Camera**: Custom Lerp script (`CameraController2D`) with vertical bounding boundaries.

---

## Project Structure
We will write the scripts and files into the designated paths we created during the folder structure reorganization:

```
Assets/
├── Scenes/
│   ├── MainMenu.unity               
│   └── ExplorationMap.unity          ← [NEW] Exploration hub scene
├── Scripts/
│   ├── Managers/
│   │   ├── GameManager.cs            ← [MODIFY] Add transition from main menu to ExplorationMap
│   │   └── GameState.cs              ← [MODIFY] Add ExplorationMap enum state
│   ├── Gameplay/
│   │   ├── PlayerController.cs       ← [NEW] Physics movement, jump, and ladder logic
│   │   ├── CameraController2D.cs     ← [NEW] Smooth horizontal & vertical player tracking
│   │   └── PortalController.cs       ← [NEW] Handles collision trigger scene transition
│   └── UI/
│       └── (existing UI Controller scripts)
└── UI/
    └── (existing UI Toolkit assets)
```

---

## Skill Evaluation Gates

The following skills from `.agent/skills/` will be applied as quality gates during design and implementation:

| # | Skill | When Applied | What It Checks |
|---|-------|-------------|----------------|
| 1 | **game-development** | Design & Code | Proper 2D platforming physics, frame-rate independent updates (`FixedUpdate`), clean state triggers. |
| 2 | **clean-code** | Coding | Meaningful naming conventions, single responsibility controllers, small functions, no magic numbers. |
| 3 | **plan-writing** | This Document | Structured tasks, logical dependencies, clear verification steps. |
| 4 | **architect-review** | Post-Implementation | Separation of player input, state logic, and scene-level camera controllers. |
| 5 | **security-review** | Post-Implementation | Defensive programming to prevent out-of-bounds null exceptions when loading scenes. |
| 6 | **performance-optimizer** | Coding | Avoiding `GameObject.Find` or allocations in `Update()`, caching components. |
| 7 | **find-bugs** | Post-Implementation | Visual boundary limits, ground checks robustness, collider seam fixes. |

---

## Verification Plan

### Automated/Editor Tests
- Open `ExplorationMap` scene:
  - Enter Play Mode.
  - Verify player character falls due to gravity and lands correctly on the ground tilemap.
  - Use Left/Right keys to walk, Space to jump. Verify jump physics feel responsive.
  - Climb ladders: verify gravity turns off, vertical keys ascend/descend, and gravity restores when stepping off the ladder.
  - Verify camera follows player vertically and horizontally with smooth interpolation.
  - Verify entering a portal loads the expected target scene or a debug placeholder scene.

### Manual Verification
- Verify the player cannot glitch/clip through vertical walls or ceiling tiles.
- Verify boundaries: Ensure the camera halts and does not scroll into black out-of-bounds space below the floor or above the ceiling.

---

## Implementation Phases

### Phase 1: Scene & Asset Setup [NEW]
- Create `ExplorationMap` scene.
- Set up a standard 2D Grid with base Tilemaps (Platforms, Ladders, Backgrounds).
- Slice character and tile sprites inside `Assets/BayatGames/Free Platform Game Assets`.
- Build the vertical layout map using platforms and ladders.

### Phase 2: 2D Player Controller [NEW]
- Create `PlayerController.cs` under `Assets/Scripts/Gameplay/`.
- Configure player `Rigidbody2D` with constraints (Freeze Rotation Z).
- Implement ground checking (boxcast/overlapcircle) on ground layer.
- Implement ladder climbing physics (switch to trigger collision, ignore gravity, vertical inputs).
- Set up sprite animations or orientation flips.

### Phase 3: Smooth Camera Controller [NEW]
- Create `CameraController2D.cs` under `Assets/Scripts/Gameplay/`.
- Implement camera position lerp targeting the player's position.
- Add boundary clamps (`minX, maxX, minY, maxY`) to stop camera movement at level boundaries.

### Phase 4: Portal Gateway System [NEW]
- Create `PortalController.cs` under `Assets/Scripts/Gameplay/`.
- Attach `BoxCollider2D` (isTrigger = true) to portals.
- Implement portal scene transition triggering `GameManager.Instance`.
- Add a visual text indicator above the portal door (e.g. "AND", "OR", "ALU").

### Phase 5: Integration & Scene Connection [MODIFY]
- Update `GameManager.cs` to change states to `ExplorationMap` when loading the scene.
- Update `MainMenuController.cs` so clicking "New Game" successfully loads the `ExplorationMap` scene instead of a placeholder debug scene.
