using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Canvas (uGUI) main menu controller. Attach to the Canvas in the MainMenuMap scene.
    /// Wires the New Game button to load the gameplay scene.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Tooltip("Optional: drag the New Game Button here. If left empty, it is found by the GameObject name below.")]
        [SerializeField] private Button newGameButton;

        [Tooltip("Name of the New Game button GameObject (used only when the button reference above is empty).")]
        [SerializeField] private string newGameButtonName = "NewGame";

        [Tooltip("Scene loaded when New Game is clicked. Must be in Build Settings.")]
        [SerializeField] private string gameplaySceneName = "firstMap";

        private void Start()
        {
            if (newGameButton == null)
            {
                var go = GameObject.Find(newGameButtonName);
                if (go != null) newGameButton = go.GetComponent<Button>();
            }

            if (newGameButton != null)
            {
                newGameButton.onClick.RemoveListener(StartNewGame);
                newGameButton.onClick.AddListener(StartNewGame);
            }
            else
            {
                Debug.LogWarning($"[MainMenuController] New Game button not found (looked for '{newGameButtonName}'). " +
                                 "Assign it in the Inspector or check the GameObject name.");
            }
        }

        /// <summary>Loads the gameplay scene. Also usable directly from a Button OnClick in the Inspector.</summary>
        public void StartNewGame()
        {
            Debug.Log("[MainMenuController] New Game clicked -> loading gameplay scene.");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadGameplayScene();
                return;
            }

            if (Application.CanStreamedLevelBeLoaded(gameplaySceneName))
            {
                SceneManager.LoadScene(gameplaySceneName);
            }
            else
            {
                Debug.LogError($"[MainMenuController] Scene '{gameplaySceneName}' is not in Build Settings. " +
                               "Add it via File > Build Settings (or it cannot be loaded).");
            }
        }
    }
}
