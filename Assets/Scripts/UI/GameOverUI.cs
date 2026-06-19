using UnityEngine;
using UnityEngine.SceneManagement;
using ZerosAndOnes.Gameplay;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Shows the Game Over panel when the player dies, and handles
    /// the Play Again / Exit buttons. Attach anywhere under the gameplay Canvas.
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private GameObject gameOverPanel;

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDied += ShowGameOver;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDied -= ShowGameOver;
            }
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        public void PlayAgain()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
