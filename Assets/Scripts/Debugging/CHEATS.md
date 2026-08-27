# Cheat Manager

A debugging/testing tool for instructors and developers. Press **F1** in the running game to open
the cheat panel.

It exists **only in the Unity Editor and in Development Builds**. Every file and every hook is
wrapped in `#if UNITY_EDITOR || DEVELOPMENT_BUILD`, so a normal (non-development) player build
contains none of this code — verified by compiling `Assembly-CSharp` without those symbols: the
cheat types are absent from the resulting assembly.

## Setup

None. `CheatBootstrap` creates the menu from code when the game starts, in whichever scene it
starts in, and it survives scene loads. Nothing has to be added to any of the 40 scenes.

## Hotkeys

| Key | Cheat |
| --- | --- |
| `F1` (or `` ` ``) | Open / close the cheat panel |
| `F2` | God mode on / off |
| `F3` | Heal the player to full |
| `F4` | No-clip on / off (fly with WASD, hold Shift to sprint) |
| `F5` | Kill every enemy in the scene |
| `F6` | Mark the current gate puzzle as solved |
| `F7` | Unlock all logic gates |
| `F8` / `F9` | Previous / next scene in Build Settings |
| `F10` | Reload the current scene |

Hotkeys are ignored while a text field in the panel has focus.

## Panel tabs

**Player** — god mode, no-clip, heal to full, add/remove half a heart, kill the player (to
demonstrate the death and game-over flow), move-speed multiplier, jump-force multiplier, no-clip
speed.

**Teleport** — click-to-teleport (left click anywhere in the world), a position bookmark to jump
back to, teleport to typed X/Y coordinates, and a one-click list of every door, portal and spawn
point in the current scene.

**Scenes** — level skipping: reload, previous/next in build order, direct buttons for the main
menu and both exploration maps, and a filterable list of all scenes in Build Settings. Jumps go
through `GameManager` so the game state stays correct, and clear the door bookkeeping that
`PlayerSpawner` and `DoorController` use, so the player lands normally instead of at an unrelated
door.

**Puzzles** — solve the gate for the current scene, unlock or lock all gates, delete the save file,
remove the door barriers in the current scene without touching the save, refill the puzzle timer,
and a per-gate list showing what is solved (click a gate to toggle it). Gate changes are written to
`game_save.json` and the component bar refreshes immediately.

**World** — kill all enemies, disable every hazard (enemy movement, contact damage, thunder), time
scale from freeze to 4x, and the on-screen `F1` reminder toggle.

## How it is wired into gameplay

The panel itself is IMGUI (`OnGUI`): no Canvas, no EventSystem, no prefab, it draws on top of every
scene's UI and keeps working at `Time.timeScale = 0`.

Four gameplay files read the cheat state, each hook wrapped in the same conditional-compilation
guard:

| File | Hook |
| --- | --- |
| `Gameplay/PlayerHealth.cs` | `TakeDamage` returns early under god mode; `CheatSetHealth` sets health exactly |
| `Gameplay/PlayerController.cs` | ignores input while the panel is open; speed and jump multipliers |
| `Gameplay/PortalController.cs` | ignores W/Up while the panel is open |
| `UI/Timer.cs` | `CheatRefill` puts time back on a puzzle clock |
