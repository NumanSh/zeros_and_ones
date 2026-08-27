using UnityEngine;
using UnityEngine.InputSystem;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class PortalController : MonoBehaviour
    {
        [Header("Portal Settings")]
        [SerializeField] private string portalID;
        [SerializeField] private string targetSceneName;
        [SerializeField] private bool isLocked = false;

        private Collider2D _collider;
        private bool _isPlayerInside = false;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            // Ensure collider is configured as a trigger
            _collider.isTrigger = true;
        }

        private void Update()
        {
            // Otherwise W/Up would trigger a scene load while the pause menu is open.
            if (UI.PauseMenuController.IsPaused) return;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Same reason for the cheat panel: a "w" typed into one of its fields must not load a
            // scene just because the player happens to be standing in a portal.
            if (Debugging.CheatManager.CapturesGameplayInput) return;
#endif

            if (_isPlayerInside && !isLocked)
            {
                // Require pressing Up Arrow or W to enter the portal
                if (Keyboard.current != null && 
                    (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame))
                {
                    EnterPortal();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Verify if colliding object has PlayerController
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                _isPlayerInside = true;
                Debug.Log($"[PortalController] Player is standing at portal {portalID}. Press Up Arrow or W to enter.");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                _isPlayerInside = false;
            }
        }

        private void EnterPortal()
        {
            Debug.Log($"[PortalController] Player entered portal {portalID}. Transitioning to scene: {targetSceneName}");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogWarning("[PortalController] GameManager instance not found, unable to transition scene.");
            }
        }
    }
}
