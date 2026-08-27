using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Spawns a <see cref="PauseMenuController"/> into every gameplay scene automatically.
    ///
    /// This runs instead of placing a pause menu prefab in each scene by hand, which matters
    /// because the project has 21 scenes (firstMap, SecondMap and 18 logic gate scenes).
    /// </summary>
    public static class PauseMenuBootstrap
    {
        private const string PauseMenuObjectName = "PauseMenu";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            // Unsubscribe first: with "Enter Play Mode Options" (no domain reload) statics survive
            // between play sessions, which would otherwise stack duplicate handlers.
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            // AfterSceneLoad fires once the first scene is already loaded, so handle it directly.
            SetUpScene(SceneManager.GetActiveScene());
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Additive) return;
            SetUpScene(scene);
        }

        private static void SetUpScene(Scene scene)
        {
            if (!scene.IsValid()) return;

            // Clear any pause left over from the scene we came from.
            Time.timeScale = 1f;

            // The main menu has nothing to pause.
            if (scene.name == PauseMenuController.MainMenuSceneName) return;

            if (Object.FindObjectOfType<PauseMenuController>() != null) return;

            EnsureEventSystem();

            // Not DontDestroyOnLoad: it belongs to this scene and dies with it, which keeps the
            // lifetime trivial and avoids duplicates across scene loads.
            var go = new GameObject(PauseMenuObjectName);
            go.AddComponent<PauseMenuController>();
        }

        /// <summary>
        /// Every scene currently ships an EventSystem, but a missing one would silently break all
        /// pause menu clicks, so create a matching StandaloneInputModule if it is ever absent.
        /// </summary>
        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            if (Object.FindObjectOfType<EventSystem>() != null) return;

            var go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
    }
}
