using System;
using System.Collections;
using UnityEngine;

namespace ZerosAndOnes.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float invincibilityDuration = 0.2f;
        [SerializeField] private float destroyDelay = 0.5f;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color flashColor = Color.red;

        public event Action OnDied;
        public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)

        private int _currentHealth;
        private float _invincibleTimer;
        private Color _originalColor = Color.white;
        private Coroutine _flashCoroutine;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => maxHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (spriteRenderer != null)
            {
                _originalColor = spriteRenderer.color;
            }
        }

        private void Update()
        {
            if (_invincibleTimer > 0f)
            {
                _invincibleTimer -= Time.deltaTime;
            }
        }

        public void TakeDamage(int amount)
        {
            if (_invincibleTimer > 0f || _currentHealth <= 0) return;

            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            _invincibleTimer = invincibilityDuration;

            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            // Trigger Hit Flash Visual Feedback
            if (_currentHealth > 0)
            {
                TriggerHitFlash();
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void TriggerHitFlash()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            _flashCoroutine = StartCoroutine(HitFlashRoutine());
        }

        private IEnumerator HitFlashRoutine()
        {
            if (spriteRenderer == null) yield break;

            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = _originalColor;
        }

        private void Die()
        {
            // Fire the OnDied event
            OnDied?.Invoke();

            // Disable all colliders on this GameObject to prevent further hits/damage
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            // Stop movement physics
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }

            // Disable other scripts (AI patrol, attack behavior, damage dealing)
            MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>();
            foreach (var script in scripts)
            {
                if (script != this)
                {
                    script.enabled = false;
                }
            }

            // Destroy the enemy GameObject after the specified delay
            Destroy(gameObject, destroyDelay);
        }

        [ContextMenu("Debug: Kill Enemy")]
        private void DebugKill()
        {
            TakeDamage(maxHealth);
        }
    }
}
