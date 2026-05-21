# Feature Specification: Vertical Map & Player Controller

## Summary
Implement a vertical exploration map and a 2D player controller for "Zeros & Ones." This feature serves as the primary navigation hub where the player moves their character vertically through platforms, climbs ladders or rides elevators, jumps across gaps, and enters left/right portal doors that trigger logic gate puzzle challenges.

---

## Actors
- **Player Character (Chibi Robot)**: The avatar controlled by the player to navigate the vertical layout.
- **Portal Gateways**: Interactive portals positioned on the sides of platforms that lead to individual logic level solvers.

---

## User Scenarios

### 1. Navigating the Map
- **Actor**: Player
- **Precondition**: Player has loaded the main exploration map.
- **Flow**:
  1. The player uses movement controls (WASD/Arrow keys) to walk left and right on platforms.
  2. The player presses the jump key to hop over gaps or climb onto higher platforms.
  3. The player climbs vertical ladders or utilizes elevators/gravity-lifts to ascend/descend levels.
  4. The camera follows the player to keep them centered.

### 2. Entering a Challenge Portal
- **Actor**: Player
- **Precondition**: Player is standing in front of an active portal.
- **Flow**:
  1. The player approaches a portal doorway (labeled with the logic gate they must build, e.g., "AND GATE").
  2. The player triggers the portal.
  3. The game transitions from the exploration map scene to the corresponding Logic Solver scene.

---

## Functional Requirements

### 1. 2D Player Controller
- **Movement**: Left and right horizontal movement with acceleration and deceleration.
- **Jumping**: A physics-based jump with variable jump height (holding jump key longer results in a slightly higher jump).
- **Vertical Navigation**: Ability to climb ladders (vertical movement disabled from standard gravity when on a ladder) or step onto elevators.
- **Animation States**: Support for idle, running, jumping, and climbing states (using the sprites in `Assets/BayatGames/Free Platform Game Assets/Character`).

### 2. Vertical Map Layout
- **Visual Design**: Built using tiling assets from `Assets/BayatGames/Free Platform Game Assets/Tiles` with backgrounds from `Assets/1 Backgrounds`.
- **Verticality**: Multiple floors/platforms stacked vertically, encouraging the player to climb up to reach advanced modules.
- **Colliders**: Correctly configured 2D colliders (platforms, walls, and ground) to prevent the player from falling out of bounds.

### 3. Portal Gates
- **Interactive Doorways**: Portal sprites placed on the left/right boundaries of platforms.
- **Visual Indicators**: Active portals glow or pulse, indicating they are open. Completed portals show a success indicator. Locked portals appear deactivated.
- **Scene Transition**: Triggers a load of the logic solver level.

---

## Non-Functional / Quality Attributes
- **Controls Responsiveness**: Input latency for jumping and movement should feel instantaneous (under 50ms).
- **Camera Smoothing**: Camera vertical/horizontal tracking must have interpolation (lerp) to prevent motion sickness.
- **Performance**: Maintaining stable 60 FPS on standard desktop machines.

---

## Success Criteria
- [ ] Player character spawns at the starting point on the map.
- [ ] Player can move left/right, jump, and climb ladders smoothly using keyboard controls.
- [ ] Camera successfully follows the player as they move vertically and horizontally.
- [ ] Approaching a portal door triggers the scene transition to a placeholder logic level.
- [ ] Colliders prevent the player from falling through the floor or passing through solid walls.

---

## Assumptions
- Standard input actions (keyboard-based: WASD/Arrows, Space for jump, E for interact) will be handled using the Unity Input System.
- Platforming is non-lethal (no death pits or hazards), focusing purely on exploration and puzzle selection.

---

## Clarifications

### Session 2026-05-20
- **Q**: Is the vertical map freely explorable from the start, or are portals gated strictly by progression?
  - **A**: Strict Progression (Option A). Note: For the initial layout phase, portals will be laid out visually without enforcing active progress locks.
- **Q**: Should the camera follow the player smoothly as they climb vertically, or utilize screen-by-screen room flip transitions?
  - **A**: Smooth Vertical Scrolling (Option A). Camera smoothly follows player in X and Y axes.
- **Q**: How does the player enter a portal gate? Should collision alone trigger the portal automatically, or should it require a button press?
  - **A**: Automatic Collision Trigger (Option B). Simply touching the doorway's collider transitions the player.
