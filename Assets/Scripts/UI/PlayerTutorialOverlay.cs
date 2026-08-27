using System.Collections;
using UnityEngine;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Controls the lifecycle of the player onboarding tutorial overlay.
    /// Displays it for a set duration, then fades it out smoothly using CanvasGroup.
    /// </summary>
    public class PlayerTutorialOverlay : MonoBehaviour
    {
        // Tracks if the tutorial has already been shown in this play session
        private static bool _hasShownTutorial = false;

        [Header("Target UI to Fade & Destroy")]
        [Tooltip("The specific panel or CanvasGroup to fade and disable/destroy. If left empty, it will fall back to the CanvasGroup on this GameObject.")]
        [SerializeField] private CanvasGroup targetCanvasGroup;

        [Header("Lifetime Settings")]
        [SerializeField] private float displayDuration = 10f;
        [SerializeField] private float fadeDuration = 1f;

        private void Awake()
        {
            // If the tutorial was already shown, destroy this overlay instantly
            if (_hasShownTutorial)
            {
                Destroy(gameObject);
                return;
            }

            // Mark the tutorial as shown
            _hasShownTutorial = true;

            if (targetCanvasGroup == null)
            {
                targetCanvasGroup = GetComponent<CanvasGroup>();
            }
        }

        private void OnEnable()
        {
            // Listen for scene load events to detect returning to the Main Menu
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            // Reset the flag if the player returns to the Main Menu so they get the tutorial next play-through
            if (scene.name == "MainMenu")
            {
                _hasShownTutorial = false;
            }
        }

        private IEnumerator Start()
        {
            if (targetCanvasGroup == null)
            {
                Debug.LogWarning("[PlayerTutorialOverlay] No target CanvasGroup assigned, and none found on this GameObject!");
                yield break;
            }

            // Make sure canvas is fully visible on start
            targetCanvasGroup.alpha = 1f;
            
            // Wait for the specified duration (e.g. 10 seconds)
            yield return new WaitForSeconds(displayDuration);

            // Smoothly fade out the canvas group alpha
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                targetCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                yield return null;
            }

            targetCanvasGroup.alpha = 0f;
            
            // Clean up: deactivate/destroy only the target panel
            if (targetCanvasGroup.gameObject != gameObject)
            {
                targetCanvasGroup.gameObject.SetActive(false);
                Destroy(targetCanvasGroup.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
