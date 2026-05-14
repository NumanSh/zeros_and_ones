# Research: Main Menu Scene

## Decision 1: Unity Version & UI System
- **Decision**: Use Unity 6 (6000.4.1f1) with UI Toolkit (USS/UXML) as the primary UI framework.
- **Rationale**: Unity 6 LTS is already configured in the project (`ProjectVersion.txt`). UI Toolkit is Unity's modern UI system replacing the legacy IMGUI and even Unity UI (uGUI). It supports USS (similar to CSS) for styling, making it future-proof and maintainable.
- **Alternatives Considered**:
  - uGUI (Canvas-based): More tutorials available, but legacy and being phased out.
  - IMGUI: Only suitable for editor tools, not runtime UI.

## Decision 2: Scene Management Strategy
- **Decision**: Use Unity's `SceneManager.LoadScene()` for scene transitions.
- **Rationale**: Simple, built-in, and sufficient for a linear scene flow. No need for additive scene loading at this stage.
- **Alternatives Considered**:
  - Addressable Assets: Overkill for the current scope.
  - Custom scene loader: Unnecessary complexity.

## Decision 3: Project Folder Structure
- **Decision**: Adopt a feature-based folder structure under `Assets/`.
- **Rationale**: Keeps related scripts, scenes, and assets together. Scales well as the game grows (Logic Engine, ALU, RAM modules, etc.).
- **Structure**:
  ```
  Assets/
  ├── _Project/
  │   ├── Scripts/
  │   │   ├── MainMenu/
  │   │   │   └── MainMenuController.cs
  │   │   └── Core/
  │   │       └── GameManager.cs
  │   ├── Scenes/
  │   │   └── MainMenu.unity
  │   ├── UI/
  │   │   ├── MainMenu.uxml
  │   │   └── MainMenu.uss
  │   └── Audio/
  │       └── (placeholder for future SFX)
  └── Scenes/
      └── SampleScene.unity (existing default)
  ```

## Decision 4: Visual Style for Menu
- **Decision**: Use placeholder Unity UI buttons with a clean dark theme (dark background, light text, subtle borders).
- **Rationale**: User chose placeholders (Q1: A). The "Zeros and Ones" brand identity suggests a dark, technical, terminal-like aesthetic. Green-on-dark or white-on-dark with monospaced fonts.
- **Alternatives Considered**:
  - Custom sprite-based buttons: Deferred to a future art pass.

## Decision 5: Audio
- **Decision**: No audio in this iteration.
- **Rationale**: The spec focuses on visual UI and button functionality. Audio can be added as a polish task later.
