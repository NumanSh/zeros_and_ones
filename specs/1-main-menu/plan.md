# Implementation Plan: Main Menu Scene

## Feature Summary
Build the first scene of "Zeros and Ones" — a Main Menu with four options (New Game, Load Game, Settings, Exit) using Unity 6 and UI Toolkit. The menu serves as the player's entry point to the game.

## Technical Context
- **Engine**: Unity 6 (6000.4.1f1 LTS)
- **UI System**: UI Toolkit (UXML + USS)
- **Language**: C# (.NET)
- **Platform Target**: PC (Standalone)
- **Scene Management**: `UnityEngine.SceneManagement.SceneManager`

## Project Structure

```
Assets/_Project/
├── Scripts/
│   ├── MainMenu/
│   │   └── MainMenuController.cs    ← Button logic
│   └── Core/
│       └── GameManager.cs           ← Singleton scene manager
├── Scenes/
│   └── MainMenu.unity               ← The main menu scene
└── UI/
    ├── MainMenu.uxml                ← UI layout (buttons, structure)
    └── MainMenu.uss                 ← Styling (dark theme, hover effects)
```

## Data Model

### Entities
- **MenuButton**: A UI element with `label` (string), `action` (enum: NewGame, LoadGame, Settings, Exit), and `enabled` (bool).
- **GameState**: Enum tracking the current application state (MainMenu, InGame, Paused).

### State Transitions
```
App Launch → MainMenu
MainMenu → "New Game" → InGame (loads gameplay scene)
MainMenu → "Load Game" → [Placeholder: shows "Coming Soon" label]
MainMenu → "Settings" → [Placeholder: shows "Coming Soon" label]
MainMenu → "Exit" → Application.Quit()
```

## Skill Evaluation Gates

The following 12 skills from `.agent/skills/` will be applied as quality gates during and after implementation:

| # | Skill | When Applied | What It Checks |
|---|-------|-------------|----------------|
| 1 | **game-development** | Architecture Design | Game loop patterns, State Machine for menu states, Input Abstraction |
| 2 | **clean-code** | During Coding | Meaningful names, small functions, single responsibility, no magic numbers |
| 3 | **plan-writing** | This Document | Small focused tasks (2-5 min), clear verification, logical ordering |
| 4 | **architect-review** | Post-Implementation | Separation of concerns (UI vs Logic), proper module boundaries, SOLID principles |
| 5 | **code-review-excellence** | Post-Implementation | Correctness, maintainability, constructive feedback format |
| 6 | **security-review** | Post-Implementation | No hardcoded secrets, no exposed internal state, safe Application.Quit |
| 7 | **performance-optimizer** | Post-Implementation | No unnecessary allocations in Update(), efficient UI rendering |
| 8 | **find-bugs** | Post-Implementation | Null reference checks, missing scene references, edge cases |
| 9 | **production-code-audit** | Pre-Push | Line-by-line scan for code quality, dead code, naming conventions |
| 10 | **codebase-audit-pre-push** | Pre-Push | Junk files removed, .gitignore verified, architecture check |
| 11 | **context-driven-development** | Throughout | Context artifacts maintained, tech-stack documented, session continuity |
| 12 | **comprehensive-review** | Final Review | Multi-dimensional review: quality + architecture + testing + documentation |
| 13 | **error-debugging** | If Issues Arise | Root cause analysis, structured error investigation |
| 14 | **project-development** | Architecture | Pipeline architecture, minimal architecture principles, iteration planning |

## Verification Plan

### Automated
- Open the `MainMenu` scene in Unity Editor.
- Enter Play Mode → verify all 4 buttons render and are centered.
- Click "New Game" → verify scene transition (or log message if gameplay scene doesn't exist yet).
- Click "Exit" → verify `Application.Quit()` is called (log in Editor since Quit doesn't work in Play Mode).
- Hover over buttons → verify visual hover effect.

### Manual
- Build standalone (PC) → Launch → Verify "Exit" closes the app.
- Verify "Load Game" and "Settings" show placeholder feedback.
- Verify the menu looks clean on 1920×1080 and 1280×720 resolutions.

## Implementation Phases

### Phase 1: Setup (Project Foundation)
- Create the folder structure under `Assets/_Project/`
- Create the `MainMenu` scene
- Set `MainMenu` as the default scene in Build Settings

### Phase 2: UI Layout & Styling
- Create `MainMenu.uxml` with 4 button elements
- Create `MainMenu.uss` with dark theme styling and hover effects
- Attach the UI Document to the scene

### Phase 3: Button Logic
- Create `MainMenuController.cs` with button click handlers
- Implement New Game (scene load), Exit (quit), and placeholder actions
- Create `GameManager.cs` for scene management utility

### Phase 4: Polish & Testing
- Add hover animations via USS transitions
- Test all button actions
- Test resolution responsiveness
- Run all skill evaluation gates

### Phase 5: Skill Evaluation & Pre-Push Audit
- Apply all 12+ skills as quality gates
- Fix any issues found
- Commit and push to feature branch
