using UnityEngine;

namespace ZerosAndOnes.Gameplay
{
    /// <summary>
    /// Damages the player while they overlap the thunder strike's trigger area
    /// and the strike sprite is currently visible.
    /// Attach to the lightning/thunder GameObject alongside a trigger Collider2D.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ThunderDamage : MonoBehaviour
    {
        [Tooltip("Only deal damage while this blinker's sprite is visible. Leave empty to always damage on overlap.")]
        [SerializeField] private SpriteBlinker spriteBlinker;
        [Tooltip("Damage dealt per hit, in half-heart units (1 = half a heart, 2 = a full heart).")]
        [SerializeField] private int damageHalfHearts = 1;
        [Tooltip("Seconds before the same player can be damaged again.")]
        [SerializeField] private float damageCooldown = 2f;

        private float _cooldownTimer;
        private PlayerHealth _playerInside;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
            }

            if (_playerInside == null) return;
            if (spriteBlinker != null && !spriteBlinker.IsVisible) return;
            if (_cooldownTimer > 0f) return;

            _playerInside.TakeDamage(damageHalfHearts);
            _cooldownTimer = damageCooldown;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                _playerInside = health;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && health == _playerInside)
            {
                _playerInside = null;
            }
        }
    }
}
