using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZerosAndOnes.Core
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
            SetState(GameState.InGame);
            // Assuming the next scene in build index is the gameplay scene, or load by name
            // For now, if the scene doesn't exist, we just log it to avoid runtime crashes during development.
            if (Application.CanStreamedLevelBeLoaded("GameplayScene"))
            {
                SceneManager.LoadScene("GameplayScene");
            }
            else
            {
                Debug.LogWarning("[GameManager] 'GameplayScene' not found in Build Settings! Placeholder action taken.");
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
