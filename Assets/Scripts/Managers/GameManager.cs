using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZerosAndOnes.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
            Debug.Log($"[GameManager] State changed to: {newState}");
        }

        public void LoadGameplayScene()
        {
            LoadScene("ExplorationMap");
        }

        public void LoadScene(string sceneName)
        {
            if (sceneName == "ExplorationMap")
            {
                SetState(GameState.ExplorationMap);
            }
            else if (sceneName == "MainMenu")
            {
                SetState(GameState.MainMenu);
            }
            else
            {
                SetState(GameState.PuzzleActive);
            }

            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning($"[GameManager] Scene '{sceneName}' not found in Build Settings! Action logged.");
            }
        }

        public void QuitGame()
        {
            Debug.Log("[GameManager] Application Quit requested.");
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
