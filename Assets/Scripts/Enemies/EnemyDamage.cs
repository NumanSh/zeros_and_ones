using UnityEngine;
using ZerosAndOnes.Gameplay;

namespace ZerosAndOnes.Enemies
{
    public class EnemyDamage : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private int damageHalfHearts = 1;
        [SerializeField] private float damageCooldown = 0.5f;

        private float _cooldownTimer;

        private void Update()
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDealDamage(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDealDamage(other.gameObject);
        }

        private void TryDealDamage(GameObject target)
        {
            if (_cooldownTimer > 0f) return;

            // Check if the collided object has the PlayerHealth component
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageHalfHearts);
                _cooldownTimer = damageCooldown;
            }
        }
    }
}
