using UnityEngine;
using ZerosAndOnes.Enemies;

namespace ZerosAndOnes.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerProjectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float maxLifetime = 4f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            // Ensure gravity doesn't pull the projectile down
            _rb.gravityScale = 0f;
            
            // Auto destroy after max lifetime
            Destroy(gameObject, maxLifetime);
        }

        public void Launch(Vector2 direction)
        {
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            
            // Set velocity toward direction
            _rb.velocity = direction.normalized * speed;

            // Rotate projectile sprite to point toward direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Ignore other triggers (e.g. portals, doors, item pickups)
            if (other.isTrigger) return;

            // Ignore the player who shot the projectile
            if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null) return;

            // Check if we hit an enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            // Destroy projectile on impact with ground/walls/enemies
            Destroy(gameObject);
        }
    }
}
