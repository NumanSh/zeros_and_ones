using UnityEngine;

namespace ZerosAndOnes.Gameplay
{
    /// <summary>
    /// Reacts to PlayerHealth.OnDied by freezing the character and rotating it
    /// to fake a "lying down" pose, since no dedicated death sprite exists.
    /// Attach to the same GameObject as PlayerHealth.
    /// </summary>
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        [Tooltip("Z rotation (degrees) applied to fake the character lying on its side.")]
        [SerializeField] private float lieDownRotationZ = 90f;

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb;

        private void Awake()
        {
            if (playerController == null) playerController = GetComponent<PlayerController>();
            if (animator == null) animator = GetComponent<Animator>();
            if (rb == null) rb = GetComponent<Rigidbody2D>();

            GetComponent<PlayerHealth>().OnDied += HandleDeath;
        }

        private void HandleDeath()
        {
            if (playerController != null) playerController.enabled = false;
            if (animator != null) animator.enabled = false;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.rotation = lieDownRotationZ;
            }
        }
    }
}
