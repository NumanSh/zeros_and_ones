# Research: Vertical Map & Player Controller

## Decision 1: 2D Physics-Based Player Controller
- **Decision**: Use a custom `Rigidbody2D` physics-based movement script.
- **Rationale**: Rigidbody-based movement guarantees correct physical interactions with platforms, slopes, and colliders. Ladder climbing is achieved by temporarily setting the Rigidbody gravity scale to `0` and directly modifying vertical velocity while inside a ladder trigger zone.
- **Alternatives Considered**:
  - Transform-based movement (non-physics): Can cause jitter or clip through colliders when moving fast.
  - CharacterController (3D): Not ideal for standard 2D physics and Tilemaps.

## Decision 2: Camera System
- **Decision**: Implement a custom `CameraController2D` script that interpolates (lerps) towards the player's position.
- **Rationale**: Keeps dependencies low and is easy to customize. A custom lerp camera with boundary bounds is lightweight, fast to implement, and handles vertical scroll limits elegantly.
- **Alternatives Considered**:
  - Unity Cinemachine: Excellent but introduces dependencies on packages. For a lightweight puzzle platformer, a custom script is cleaner and easier to inspect.

## Decision 3: Map Design & Construction
- **Decision**: Use Unity's 2D Tilemap System with standard 2D Box/Composite Colliders.
- **Rationale**: Tilemaps allow painting grid-based platform maps efficiently using the tiles in `Assets/BayatGames/Free Platform Game Assets/Tiles`. Composite Colliders combine adjacent tile colliders to prevent the player from getting stuck on tile seams.
- **Alternatives Considered**:
  - Placing individual prefab sprites: Hard to manage, poor performance, and prone to collider seam bugs.

## Decision 4: Portal Trigger and Transitions
- **Decision**: Use a `BoxCollider2D` with `isTrigger` set to true, calling `GameManager.Instance.LoadScene()` on `OnTriggerEnter2D`.
- **Rationale**: Simpler flow requested by user (Q3: Option B - Automatic Collision Trigger). No complex interaction key prompt is needed for this phase.
- **Alternatives Considered**:
  - Interaction Key Prompt: Rejected by user choice in favor of immediate collision-based entry.

## Decision 5: Character Sprites and Animation
- **Decision**: Use Unity's native `Animator` component with a 2D Sprite-based animation system.
- **Rationale**: The assets in `Assets/BayatGames/Free Platform Game Assets/Character` provide frames for Idle, Run, Jump, and Climb. A simple state machine controller is the standard approach to swap sprites based on character state.
