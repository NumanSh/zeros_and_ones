// The whole cheat system is compiled out of release builds: this guard wraps the entire file, and
// every hook into gameplay code repeats the same directive. Nothing here ships to players.
#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using ZerosAndOnes.Enemies;
using ZerosAndOnes.Gameplay;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.Debugging
{
    /// <summary>
    /// Backend of the in-game cheat menu: it holds the cheat state and performs the actions.
    /// <see cref="CheatConsole"/> is the on-screen interface and <see cref="CheatBootstrap"/> spawns
    /// both into every scene automatically, so there is no prefab and no per-scene setup.
    ///
    /// Gameplay code reads the static flags below through hooks wrapped in the same
    /// UNITY_EDITOR || DEVELOPMENT_BUILD guard as this file:
    ///   PlayerHealth.TakeDamage    -> <see cref="GodMode"/>
    ///   PlayerController.Update    -> <see cref="CapturesGameplayInput"/>
    ///   PlayerController.FixedUpdate/Jump -> <see cref="MoveSpeedMultiplier"/>, <see cref="JumpForceMultiplier"/>
    /// </summary>
    public class CheatManager : MonoBehaviour
    {
        public const string ObjectName = "CheatManager";

        public static CheatManager Instance { get; private set; }

        // ---------------------------------------------------------------- cheat state

        /// <summary>The player takes no damage while this is on.</summary>
        public static bool GodMode { get; private set; }

        /// <summary>Free-fly: physics off, the player is moved directly by WASD through walls.</summary>
        public static bool NoClip { get; private set; }

        public static bool EnemiesDisabled { get; private set; }

        public static float MoveSpeedMultiplier { get; private set; } = 1f;
        public static float JumpForceMultiplier { get; private set; } = 1f;
        public static float NoClipSpeed { get; set; } = 14f;

        /// <summary>
        /// True while the cheat panel is open. PlayerController skips its input reading, so keys
        /// typed into the panel (WASD, the scene filter) do not also drive the character.
        /// </summary>
        public static bool CapturesGameplayInput { get; set; }

        /// <summary>Result of the last cheat, shown at the bottom of the panel.</summary>
        public static string LastMessage { get; private set; } = "Cheat menu ready. F1 opens and closes it.";
        public static float LastMessageTime { get; private set; }

        // ---------------------------------------------------------------- cached lookups

        private PlayerController _player;
        private PlayerHealth _playerHealth;
        private RigidbodyType2D _noClipPreviousBodyType = RigidbodyType2D.Dynamic;
        private Vector3? _savedPosition;
        private string _savedPositionScene;
        private static string[] _buildScenes;

        public PlayerController Player
        {
            get
            {
                if (_player == null) _player = FindObjectOfType<PlayerController>();
                return _player;
            }
        }

        public PlayerHealth Health
        {
            get
            {
                if (_playerHealth == null) _playerHealth = FindObjectOfType<PlayerHealth>();
                return _playerHealth;
            }
        }

        public Transform PlayerTransform => Player != null ? Player.transform : null;

        /// <summary>Every scene in Build Settings, by name, in build order.</summary>
        public static IReadOnlyList<string> BuildScenes
        {
            get
            {
                if (_buildScenes == null)
                {
                    int count = SceneManager.sceneCountInBuildSettings;
                    _buildScenes = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        _buildScenes[i] = SceneNameFromBuildIndex(i);
                    }
                }

                return _buildScenes;
            }
        }

        // ---------------------------------------------------------------- lifecycle

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (Instance == this) Instance = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // The cached player belongs to the scene we just left.
            _player = null;
            _playerHealth = null;

            // Cheats that act on scene objects have to be re-applied to the new scene's objects.
            if (NoClip) ApplyNoClipToPlayer(true);
            if (EnemiesDisabled) ApplyEnemiesDisabled();
        }

        private void Update()
        {
            if (NoClip) UpdateNoClipMovement();
        }

        // ---------------------------------------------------------------- player cheats

        public void ToggleGodMode() => SetGodMode(!GodMode);

        public void SetGodMode(bool enabled)
        {
            GodMode = enabled;
            Log(enabled ? "God mode ON - the player takes no damage." : "God mode OFF.");
        }

        public void HealToFull()
        {
            PlayerHealth health = Health;
            if (health == null)
            {
                Log("No player in this scene to heal.");
                return;
            }

            health.CheatSetHealth(health.MaxHealth);
            Log("Player healed to full.");
        }

        public void ChangeHealth(int halfHearts)
        {
            PlayerHealth health = Health;
            if (health == null)
            {
                Log("No player in this scene.");
                return;
            }

            health.CheatSetHealth(health.CurrentHealth + halfHearts);
            Log($"Health set to {health.CurrentHealth}/{health.MaxHealth} half-hearts.");
        }

        /// <summary>Kills the player outright, to demonstrate the death and game-over flow.</summary>
        public void KillPlayer()
        {
            PlayerHealth health = Health;
            if (health == null)
            {
                Log("No player in this scene to kill.");
                return;
            }

            // CheatSetHealth deliberately ignores god mode and the invincibility window, so the
            // death flow can be demonstrated without turning other cheats off first.
            health.CheatSetHealth(0);
            Log("Player killed.");
        }

        public void SetMoveSpeedMultiplier(float multiplier)
        {
            MoveSpeedMultiplier = Mathf.Clamp(multiplier, 0.1f, 10f);
        }

        public void SetJumpForceMultiplier(float multiplier)
        {
            JumpForceMultiplier = Mathf.Clamp(multiplier, 0.1f, 5f);
        }

        public void ResetMovementCheats()
        {
            MoveSpeedMultiplier = 1f;
            JumpForceMultiplier = 1f;
            Log("Movement multipliers reset to 1x.");
        }

        // ---------------------------------------------------------------- no-clip

        public void ToggleNoClip() => SetNoClip(!NoClip);

        public void SetNoClip(bool enabled)
        {
            NoClip = enabled;
            ApplyNoClipToPlayer(enabled);
            Log(enabled
                ? "No-clip ON - WASD flies through walls, gravity is off."
                : "No-clip OFF.");
        }

        /// <summary>
        /// Swaps the player between normal physics and free flight. The original body type is kept
        /// so turning no-clip off restores exactly what the scene had.
        /// </summary>
        private void ApplyNoClipToPlayer(bool enabled)
        {
            PlayerController player = Player;
            if (player == null) return;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (enabled)
            {
                if (rb != null)
                {
                    _noClipPreviousBodyType = rb.bodyType;
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                // Disabled so its FixedUpdate does not fight the direct transform movement.
                player.enabled = false;
            }
            else
            {
                if (rb != null)
                {
                    rb.bodyType = _noClipPreviousBodyType;
                    rb.velocity = Vector2.zero;
                }

                player.enabled = true;
            }
        }

        private void UpdateNoClipMovement()
        {
            // Typing into the panel's text fields must not fly the player around.
            if (CheatConsole.IsTyping) return;

            Transform player = PlayerTransform;
            if (player == null) return;

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            Vector2 direction = Vector2.zero;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) direction.x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) direction.x += 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) direction.y += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) direction.y -= 1f;

            if (direction.sqrMagnitude < 0.0001f) return;

            // Unscaled time so no-clip still works while the game is paused or slowed down.
            float speed = NoClipSpeed * (keyboard.leftShiftKey.isPressed ? 3f : 1f);
            player.position += (Vector3)(direction.normalized * speed * Time.unscaledDeltaTime);
        }

        // ---------------------------------------------------------------- teleport

        public void TeleportPlayer(Vector3 worldPosition, string label = null)
        {
            Transform player = PlayerTransform;
            if (player == null)
            {
                Log("No player in this scene to teleport.");
                return;
            }

            // Keep the player's own Z so 2D sorting order is unaffected by the teleport.
            worldPosition.z = player.position.z;
            player.position = worldPosition;

            // Without this the fall speed built up before the teleport carries over and the player
            // immediately drops through the floor they were placed on.
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            Log(string.IsNullOrEmpty(label)
                ? $"Teleported to ({worldPosition.x:F1}, {worldPosition.y:F1})."
                : $"Teleported to {label}.");
        }

        public void SavePosition()
        {
            Transform player = PlayerTransform;
            if (player == null)
            {
                Log("No player in this scene.");
                return;
            }

            _savedPosition = player.position;
            _savedPositionScene = SceneManager.GetActiveScene().name;
            Log($"Position bookmarked ({player.position.x:F1}, {player.position.y:F1}).");
        }

        public bool HasSavedPosition => _savedPosition.HasValue;

        public void RestorePosition()
        {
            if (!_savedPosition.HasValue)
            {
                Log("No bookmarked position yet.");
                return;
            }

            if (_savedPositionScene != SceneManager.GetActiveScene().name)
            {
                Log($"The bookmark belongs to scene '{_savedPositionScene}'.");
                return;
            }

            TeleportPlayer(_savedPosition.Value, "the bookmark");
        }

        /// <summary>
        /// Every door, portal and named spawn point in the scene, as teleport destinations.
        /// Rebuilt on demand because scene contents change constantly while testing.
        /// </summary>
        public List<(string Label, Vector3 Position)> CollectTeleportTargets()
        {
            var targets = new List<(string, Vector3)>();

            foreach (DoorController door in FindObjectsOfType<DoorController>())
            {
                targets.Add(($"Door: {door.gateType}", door.transform.position));
            }

            foreach (PortalController portal in FindObjectsOfType<PortalController>())
            {
                targets.Add(($"Portal: {portal.gameObject.name}", portal.transform.position));
            }

            foreach (PlayerSpawner spawner in FindObjectsOfType<PlayerSpawner>())
            {
                targets.Add(($"Spawn: {spawner.gameObject.name}", spawner.transform.position));
            }

            return targets;
        }

        // ---------------------------------------------------------------- level skipping

        public void ReloadScene() => LoadSceneByName(SceneManager.GetActiveScene().name);

        public void LoadMainMenu() => LoadSceneByName(UI.PauseMenuController.MainMenuSceneName);

        /// <summary>Jumps forward or backward through the Build Settings scene list.</summary>
        public void StepScene(int direction)
        {
            int count = SceneManager.sceneCountInBuildSettings;
            if (count == 0) return;

            int index = SceneManager.GetActiveScene().buildIndex;
            if (index < 0)
            {
                Log("The current scene is not in Build Settings, cannot step from it.");
                return;
            }

            index = ((index + direction) % count + count) % count;
            LoadSceneByName(SceneNameFromBuildIndex(index));
        }

        public void LoadSceneByName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Log($"Scene '{sceneName}' is not in Build Settings.");
                return;
            }

            PrepareForSceneJump();

            // Routed through GameManager so the GameState stays correct, exactly like a door does.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }

            Log($"Jumped to scene '{sceneName}'.");
        }

        /// <summary>
        /// A cheat jump is not a door transition, so the door bookkeeping has to be cleared:
        /// otherwise PlayerSpawner would drop the player at an unrelated door and DoorController
        /// would suppress the door they land next to.
        /// </summary>
        private void PrepareForSceneJump()
        {
            string current = SceneManager.GetActiveScene().name;
            if (current == "firstMap" || current == "SecondMap")
            {
                ExitButton.LastExplorationMap = current;
            }

            ExitButton.LastSceneName = string.Empty;

            // Jumping while paused or slowed must not leave the next scene frozen.
            Time.timeScale = 1f;
        }

        private static string SceneNameFromBuildIndex(int buildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        // ---------------------------------------------------------------- puzzle progress

        /// <summary>The logic gate whose puzzle scene is currently loaded, or null on a map scene.</summary>
        public static LogicGates_Type? GateForCurrentScene()
        {
            string scene = SceneManager.GetActiveScene().name;

            foreach (LogicGates_Type gate in Enum.GetValues(typeof(LogicGates_Type)))
            {
                // Case-insensitive: the mapping says "Xor_Scene" while the file is "Xor_scene".
                if (string.Equals(DoorController.GetSceneNameForGateType(gate), scene,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return gate;
                }
            }

            return null;
        }

        public static bool IsGateSolved(LogicGates_Type gate)
        {
            return ComponentBarManager.SolvedComponents.TryGetValue(gate, out bool solved) && solved;
        }

        public void SolveCurrentGate()
        {
            LogicGates_Type? gate = GateForCurrentScene();
            if (!gate.HasValue)
            {
                Log("This scene is not a logic gate puzzle.");
                return;
            }

            SetGateSolved(gate.Value, true);
        }

        public void SetGateSolved(LogicGates_Type gate, bool solved)
        {
            EnsureAllGateKeys();
            ComponentBarManager.SolvedComponents[gate] = solved;
            SaveManager.SaveLogicGates(ComponentBarManager.SolvedComponents);
            RefreshComponentBar();
            Log($"{gate} marked as {(solved ? "SOLVED" : "unsolved")}.");
        }

        public void SetAllGatesSolved(bool solved)
        {
            EnsureAllGateKeys();

            foreach (LogicGates_Type gate in Enum.GetValues(typeof(LogicGates_Type)))
            {
                ComponentBarManager.SolvedComponents[gate] = solved;
            }

            SaveManager.SaveLogicGates(ComponentBarManager.SolvedComponents);
            RefreshComponentBar();
            Log(solved
                ? "All logic gates unlocked. Reload the map to see the doors open."
                : "All logic gates locked again.");
        }

        public void ClearSaveFile()
        {
            SaveManager.ClearSave();
            ComponentBarManager.SolvedComponents.Clear();
            EnsureAllGateKeys();
            RefreshComponentBar();
            Log("Save file deleted and progress reset.");
        }

        /// <summary>
        /// Removes the physical door barriers in the current scene without touching the save file,
        /// so a map can be walked through end to end while testing.
        /// </summary>
        public void OpenDoorsInScene()
        {
            int opened = 0;

            foreach (DoorController controller in FindObjectsOfType<DoorController>())
            {
                if (controller.door != null && controller.door.activeSelf)
                {
                    controller.door.SetActive(false);
                    opened++;
                }
            }

            foreach (Remove_doors remover in FindObjectsOfType<Remove_doors>())
            {
                if (remover.door != null && remover.door.activeSelf)
                {
                    remover.door.SetActive(false);
                    opened++;
                }
            }

            Log($"Opened {opened} door(s) in this scene.");
        }

        /// <summary>Puts time back on a puzzle clock so a gate can be demonstrated unhurried.</summary>
        public void RefillPuzzleTimer(float seconds = 600f)
        {
            Timer[] timers = FindObjectsOfType<Timer>(true);
            foreach (Timer timer in timers)
            {
                timer.CheatRefill(seconds);
            }

            Log(timers.Length > 0
                ? $"Refilled {timers.Length} puzzle timer(s) to {seconds / 60f:0.#} minutes."
                : "No puzzle timer in this scene.");
        }

        private static void EnsureAllGateKeys()
        {
            foreach (LogicGates_Type gate in Enum.GetValues(typeof(LogicGates_Type)))
            {
                if (!ComponentBarManager.SolvedComponents.ContainsKey(gate))
                {
                    ComponentBarManager.SolvedComponents[gate] = false;
                }
            }
        }

        private static void RefreshComponentBar()
        {
            ComponentBarManager bar = FindObjectOfType<ComponentBarManager>();
            if (bar != null) bar.UpdateBarPages();
        }

        // ---------------------------------------------------------------- world cheats

        public void KillAllEnemies()
        {
            EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
            foreach (EnemyHealth enemy in enemies)
            {
                enemy.TakeDamage(enemy.MaxHealth);
            }

            Log($"Killed {enemies.Length} enem{(enemies.Length == 1 ? "y" : "ies")}.");
        }

        public void ToggleEnemiesDisabled() => SetEnemiesDisabled(!EnemiesDisabled);

        public void SetEnemiesDisabled(bool disabled)
        {
            EnemiesDisabled = disabled;
            ApplyEnemiesDisabled();
            Log(disabled
                ? "Hazards disabled - enemies stop moving and nothing deals contact damage."
                : "Hazards re-enabled.");
        }

        private void ApplyEnemiesDisabled()
        {
            bool enabled = !EnemiesDisabled;

            foreach (EnemyDamage damage in FindObjectsOfType<EnemyDamage>()) damage.enabled = enabled;
            foreach (PatrolEnemy patrol in FindObjectsOfType<PatrolEnemy>()) patrol.enabled = enabled;
            foreach (SwingingObstacle swing in FindObjectsOfType<SwingingObstacle>()) swing.enabled = enabled;
            foreach (ThunderDamage thunder in FindObjectsOfType<ThunderDamage>()) thunder.enabled = enabled;
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = Mathf.Clamp(scale, 0f, 8f);
            Log($"Time scale set to {Time.timeScale:0.##}x.");
        }

        // ---------------------------------------------------------------- messages

        public static void Log(string message)
        {
            LastMessage = message;
            LastMessageTime = Time.unscaledTime;
            UnityEngine.Debug.Log($"[Cheats] {message}");
        }
    }
}

#endif
