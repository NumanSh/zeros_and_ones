// Compiled only in the Unity Editor and in Development Builds - see CheatManager.cs.
#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using ZerosAndOnes.Gameplay;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.Debugging
{
    /// <summary>
    /// The on-screen interface for <see cref="CheatManager"/>: a draggable panel with tabs, plus
    /// function-key shortcuts for the cheats used most often while testing.
    ///
    /// Drawn with IMGUI (OnGUI) on purpose. IMGUI needs no Canvas, no EventSystem and no prefab,
    /// it renders on top of every scene's UI, and it keeps working at Time.timeScale = 0 - so the
    /// panel is available in all 40 scenes without touching any of them, and cannot be broken by
    /// the game's own UI.
    /// </summary>
    [RequireComponent(typeof(CheatManager))]
    public class CheatConsole : MonoBehaviour
    {
        private enum Tab
        {
            Player,
            Teleport,
            Scenes,
            Puzzles,
            World
        }

        private const int WindowId = 91731;
        private static readonly string[] TabNames = { "Player", "Teleport", "Scenes", "Puzzles", "World" };

        /// <summary>True while an IMGUI text field has focus, so hotkeys are not typed into it.</summary>
        public static bool IsTyping { get; private set; }

        /// <summary>Shows the "F1 - Cheat menu" reminder in the corner while the panel is closed.</summary>
        public static bool ShowHint { get; set; } = true;

        private CheatManager _cheats;
        private bool _isOpen;
        private Tab _tab = Tab.Player;
        private Rect _windowRect = new Rect(20f, 20f, 470f, 430f);

        private Vector2 _targetScroll;
        private Vector2 _sceneScroll;
        private Vector2 _gateScroll;

        private string _sceneFilter = string.Empty;
        private string _teleportX = "0";
        private string _teleportY = "0";
        private bool _teleportOnClick;

        private float _fps;

        /// <summary>
        /// The whole panel is scaled up on tall displays: the default IMGUI font is unreadable at
        /// 1440p and above. Scaling GUI.matrix scales the text with the widgets.
        /// </summary>
        private float Scale => Mathf.Clamp(Screen.height / 900f, 1f, 2.5f);

        private Rect ScreenSpaceWindowRect => new Rect(
            _windowRect.x * Scale, _windowRect.y * Scale,
            _windowRect.width * Scale, _windowRect.height * Scale);

        private void Awake()
        {
            _cheats = GetComponent<CheatManager>();
        }

        private void OnDisable()
        {
            // Never leave the player's input locked out if this component goes away.
            CheatManager.CapturesGameplayInput = false;
            IsTyping = false;
        }

        private void Update()
        {
            _fps = Mathf.Lerp(_fps, 1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f), 0.1f);

            HandleHotkeys();
            HandleTeleportClick();
        }

        // ---------------------------------------------------------------- input

        private void HandleHotkeys()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.f1Key.wasPressedThisFrame ||
                (keyboard.backquoteKey.wasPressedThisFrame && !IsTyping))
            {
                SetOpen(!_isOpen);
            }

            // Everything below is a single key, so it must not fire while a text field has focus.
            if (IsTyping) return;

            if (keyboard.f2Key.wasPressedThisFrame) _cheats.ToggleGodMode();
            if (keyboard.f3Key.wasPressedThisFrame) _cheats.HealToFull();
            if (keyboard.f4Key.wasPressedThisFrame) _cheats.ToggleNoClip();
            if (keyboard.f5Key.wasPressedThisFrame) _cheats.KillAllEnemies();
            if (keyboard.f6Key.wasPressedThisFrame) _cheats.SolveCurrentGate();
            if (keyboard.f7Key.wasPressedThisFrame) _cheats.SetAllGatesSolved(true);
            if (keyboard.f8Key.wasPressedThisFrame) _cheats.StepScene(-1);
            if (keyboard.f9Key.wasPressedThisFrame) _cheats.StepScene(1);
            if (keyboard.f10Key.wasPressedThisFrame) _cheats.ReloadScene();
        }

        private void SetOpen(bool open)
        {
            _isOpen = open;

            // Hand the keyboard to the panel so WASD typed into it does not also move the player.
            CheatManager.CapturesGameplayInput = open;

            if (!open)
            {
                IsTyping = false;
                GUIUtility.keyboardControl = 0;
            }
        }

        /// <summary>Click-to-teleport: any click outside the panel moves the player there.</summary>
        private void HandleTeleportClick()
        {
            if (!_teleportOnClick) return;

            Mouse mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;

            Vector2 screenPoint = mouse.position.ReadValue();

            // GUI coordinates start at the top-left, pointer coordinates at the bottom-left.
            if (_isOpen && ScreenSpaceWindowRect.Contains(new Vector2(screenPoint.x, Screen.height - screenPoint.y)))
            {
                return;
            }

            Camera camera = Camera.main;
            if (camera == null)
            {
                CheatManager.Log("No camera tagged MainCamera in this scene.");
                return;
            }

            // Z is the distance from the camera to the gameplay plane at z = 0.
            Vector3 world = camera.ScreenToWorldPoint(
                new Vector3(screenPoint.x, screenPoint.y, Mathf.Abs(camera.transform.position.z)));

            _cheats.TeleportPlayer(world);
        }

        // ---------------------------------------------------------------- drawing

        private void OnGUI()
        {
            Matrix4x4 previousMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Scale, Scale, 1f));

            if (_isOpen)
            {
                ClampWindowToScreen();
                _windowRect = GUILayout.Window(WindowId, _windowRect, DrawWindow, "CHEAT MENU  -  development build only");
                IsTyping = GUIUtility.keyboardControl != 0;
            }
            else if (ShowHint)
            {
                GUI.Box(new Rect(8f, 8f, 150f, 24f), "F1  Cheat menu");
            }

            GUI.matrix = previousMatrix;
        }

        private void ClampWindowToScreen()
        {
            float maxX = Screen.width / Scale - 60f;
            float maxY = Screen.height / Scale - 40f;
            _windowRect.x = Mathf.Clamp(_windowRect.x, -_windowRect.width + 60f, maxX);
            _windowRect.y = Mathf.Clamp(_windowRect.y, 0f, maxY);
        }

        private void DrawWindow(int id)
        {
            DrawStatus();

            _tab = (Tab)GUILayout.Toolbar((int)_tab, TabNames);
            GUILayout.Space(4f);

            switch (_tab)
            {
                case Tab.Player: DrawPlayerTab(); break;
                case Tab.Teleport: DrawTeleportTab(); break;
                case Tab.Scenes: DrawScenesTab(); break;
                case Tab.Puzzles: DrawPuzzlesTab(); break;
                case Tab.World: DrawWorldTab(); break;
            }

            GUILayout.FlexibleSpace();
            DrawFooter();

            GUI.DragWindow(new Rect(0f, 0f, 100000f, 20f));
        }

        private void DrawStatus()
        {
            Transform player = _cheats.PlayerTransform;
            PlayerHealth health = _cheats.Health;

            string state = GameManager.Instance != null ? GameManager.Instance.CurrentState.ToString() : "no GameManager";
            string position = player != null ? $"{player.position.x:F1}, {player.position.y:F1}" : "no player";
            string hearts = health != null ? $"{health.CurrentHealth}/{health.MaxHealth}" : "-";

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"Scene: {SceneManager.GetActiveScene().name}    State: {state}");
            GUILayout.Label($"Player: {position}    Health: {hearts} half-hearts    {_fps:F0} fps");
            GUILayout.Label($"God: {OnOff(CheatManager.GodMode)}   No-clip: {OnOff(CheatManager.NoClip)}   " +
                            $"Hazards: {(CheatManager.EnemiesDisabled ? "OFF" : "on")}   Time: {Time.timeScale:0.##}x");
            GUILayout.EndVertical();
        }

        private void DrawFooter()
        {
            GUILayout.Box(CheatManager.LastMessage);
            GUILayout.Label("F1 menu  F2 god  F3 heal  F4 no-clip  F5 kill enemies  " +
                            "F6 solve gate  F7 unlock all  F8/F9 prev/next scene  F10 reload");
        }

        // ---------------------------------------------------------------- tabs

        private void DrawPlayerTab()
        {
            if (GUILayout.Toggle(CheatManager.GodMode, "  God mode - no damage taken  (F2)") != CheatManager.GodMode)
            {
                _cheats.ToggleGodMode();
            }

            if (GUILayout.Toggle(CheatManager.NoClip, "  No-clip - fly through walls with WASD, hold Shift to sprint  (F4)")
                != CheatManager.NoClip)
            {
                _cheats.ToggleNoClip();
            }

            GUILayout.BeginHorizontal();
            if (Button("Heal to full (F3)")) _cheats.HealToFull();
            if (Button("+1 half heart")) _cheats.ChangeHealth(1);
            if (Button("-1 half heart")) _cheats.ChangeHealth(-1);
            if (Button("Kill player")) _cheats.KillPlayer();
            GUILayout.EndHorizontal();

            GUILayout.Space(6f);

            GUILayout.Label($"Move speed: {CheatManager.MoveSpeedMultiplier:0.##}x");
            _cheats.SetMoveSpeedMultiplier(GUILayout.HorizontalSlider(CheatManager.MoveSpeedMultiplier, 0.25f, 5f));

            GUILayout.Label($"Jump force: {CheatManager.JumpForceMultiplier:0.##}x");
            _cheats.SetJumpForceMultiplier(GUILayout.HorizontalSlider(CheatManager.JumpForceMultiplier, 0.5f, 3f));

            GUILayout.Label($"No-clip speed: {CheatManager.NoClipSpeed:0.#}");
            CheatManager.NoClipSpeed = GUILayout.HorizontalSlider(CheatManager.NoClipSpeed, 2f, 40f);

            if (Button("Reset movement multipliers")) _cheats.ResetMovementCheats();
        }

        private void DrawTeleportTab()
        {
            _teleportOnClick = GUILayout.Toggle(_teleportOnClick,
                "  Click to teleport - left click anywhere in the world");

            GUILayout.BeginHorizontal();
            if (Button("Bookmark position")) _cheats.SavePosition();
            GUI.enabled = _cheats.HasSavedPosition;
            if (Button("Return to bookmark")) _cheats.RestorePosition();
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("X", GUILayout.Width(14f));
            _teleportX = GUILayout.TextField(_teleportX, GUILayout.Width(70f));
            GUILayout.Label("Y", GUILayout.Width(14f));
            _teleportY = GUILayout.TextField(_teleportY, GUILayout.Width(70f));
            if (Button("Teleport to coordinates")) TeleportToTypedCoordinates();
            GUILayout.EndHorizontal();

            GUILayout.Label("Destinations in this scene:");
            _targetScroll = GUILayout.BeginScrollView(_targetScroll, GUI.skin.box, GUILayout.Height(150f));

            List<(string Label, Vector3 Position)> targets = _cheats.CollectTeleportTargets();
            if (targets.Count == 0)
            {
                GUILayout.Label("No doors, portals or spawn points here.");
            }

            foreach ((string label, Vector3 position) in targets)
            {
                if (GUILayout.Button($"{label}   ({position.x:F0}, {position.y:F0})"))
                {
                    _cheats.TeleportPlayer(position, label);
                }
            }

            GUILayout.EndScrollView();
        }

        private void TeleportToTypedCoordinates()
        {
            if (float.TryParse(_teleportX, out float x) && float.TryParse(_teleportY, out float y))
            {
                _cheats.TeleportPlayer(new Vector3(x, y, 0f));
            }
            else
            {
                CheatManager.Log("Those coordinates are not numbers.");
            }
        }

        private void DrawScenesTab()
        {
            GUILayout.BeginHorizontal();
            if (Button("Reload (F10)")) _cheats.ReloadScene();
            if (Button("Previous (F8)")) _cheats.StepScene(-1);
            if (Button("Next (F9)")) _cheats.StepScene(1);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (Button("Main menu")) _cheats.LoadMainMenu();
            if (Button("firstMap")) _cheats.LoadSceneByName("firstMap");
            if (Button("SecondMap")) _cheats.LoadSceneByName("SecondMap");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter", GUILayout.Width(42f));
            _sceneFilter = GUILayout.TextField(_sceneFilter);
            if (GUILayout.Button("x", GUILayout.Width(24f))) _sceneFilter = string.Empty;
            GUILayout.EndHorizontal();

            _sceneScroll = GUILayout.BeginScrollView(_sceneScroll, GUI.skin.box, GUILayout.Height(160f));

            string activeScene = SceneManager.GetActiveScene().name;
            foreach (string scene in CheatManager.BuildScenes)
            {
                if (!string.IsNullOrEmpty(_sceneFilter) &&
                    scene.IndexOf(_sceneFilter, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (GUILayout.Button(scene == activeScene ? $"> {scene}" : scene))
                {
                    _cheats.LoadSceneByName(scene);
                }
            }

            GUILayout.EndScrollView();
        }

        private void DrawPuzzlesTab()
        {
            LogicGates_Type? currentGate = CheatManager.GateForCurrentScene();

            GUILayout.Label(currentGate.HasValue
                ? $"This scene is the {currentGate.Value} puzzle."
                : "This scene is not a logic gate puzzle.");

            GUILayout.BeginHorizontal();
            GUI.enabled = currentGate.HasValue;
            if (Button("Solve this gate (F6)")) _cheats.SolveCurrentGate();
            GUI.enabled = true;
            if (Button("Unlock all gates (F7)")) _cheats.SetAllGatesSolved(true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (Button("Lock all gates")) _cheats.SetAllGatesSolved(false);
            if (Button("Delete save file")) _cheats.ClearSaveFile();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (Button("Open doors in this scene")) _cheats.OpenDoorsInScene();
            if (Button("Refill puzzle timer")) _cheats.RefillPuzzleTimer();
            GUILayout.EndHorizontal();

            GUILayout.Label("Progress - click a gate to toggle it:");
            _gateScroll = GUILayout.BeginScrollView(_gateScroll, GUI.skin.box, GUILayout.Height(140f));

            foreach (LogicGates_Type gate in Enum.GetValues(typeof(LogicGates_Type)))
            {
                bool solved = CheatManager.IsGateSolved(gate);
                if (GUILayout.Button($"{(solved ? "[x]" : "[ ]")}  {gate}"))
                {
                    _cheats.SetGateSolved(gate, !solved);
                }
            }

            GUILayout.EndScrollView();
        }

        private void DrawWorldTab()
        {
            GUILayout.BeginHorizontal();
            if (Button("Kill all enemies (F5)")) _cheats.KillAllEnemies();
            if (GUILayout.Toggle(CheatManager.EnemiesDisabled, "  Disable all hazards")
                != CheatManager.EnemiesDisabled)
            {
                _cheats.ToggleEnemiesDisabled();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(6f);
            GUILayout.Label($"Time scale: {Time.timeScale:0.##}x");

            GUILayout.BeginHorizontal();
            if (Button("Freeze")) _cheats.SetTimeScale(0f);
            if (Button("0.25x")) _cheats.SetTimeScale(0.25f);
            if (Button("0.5x")) _cheats.SetTimeScale(0.5f);
            if (Button("1x")) _cheats.SetTimeScale(1f);
            if (Button("2x")) _cheats.SetTimeScale(2f);
            if (Button("4x")) _cheats.SetTimeScale(4f);
            GUILayout.EndHorizontal();

            GUILayout.Space(6f);
            ShowHint = GUILayout.Toggle(ShowHint, "  Show the F1 reminder while the menu is closed");

            GUILayout.Space(6f);
            GUILayout.Label($"Save file: {Application.persistentDataPath}/game_save.json");

            if (Button("Close menu (F1)")) SetOpen(false);
        }

        // ---------------------------------------------------------------- helpers

        private static bool Button(string label) => GUILayout.Button(label, GUILayout.Height(24f));

        private static string OnOff(bool value) => value ? "ON" : "off";
    }
}

#endif
