using UnityEngine;
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

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            // Ensure collider is configured as a trigger
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Verify if colliding object has PlayerController
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (isLocked)
                {
                    Debug.Log($"[PortalController] Portal {portalID} is locked.");
                    return;
                }

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
}
