using System;
using UnityEngine;

namespace ZerosAndOnes.Gameplay
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHearts = 3;
        [Tooltip("Seconds the player is immune to damage after being hit.")]
        [SerializeField] private float invincibilityDuration = 0.5f;

        public event Action<int, int> OnHealthChanged; // (currentHalfHearts, maxHalfHearts)
        public event Action OnDied;

        private int _maxHealth;
        private int _currentHealth;
        private float _invincibleTimer;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _maxHealth = maxHearts * 2;
            _currentHealth = _maxHealth;
        }

        private void Update()
        {
            if (_invincibleTimer > 0f)
            {
                _invincibleTimer -= Time.deltaTime;
            }
        }

        public void TakeDamage(int halfHearts)
        {
            if (_invincibleTimer > 0f || _currentHealth <= 0) return;

            _currentHealth = Mathf.Max(0, _currentHealth - halfHearts);
            _invincibleTimer = invincibilityDuration;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }

        public void Heal(int halfHearts)
        {
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + halfHearts);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        [ContextMenu("Debug: Refill Full Health")]
        private void DebugRefillFullHealth()
        {
            Heal(_maxHealth);
        }
    }
}
