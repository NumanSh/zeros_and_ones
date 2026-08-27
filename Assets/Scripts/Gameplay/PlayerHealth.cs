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
        private bool _isInitialized = false;

        public int MaxHealth
        {
            get
            {
                InitializeHealth();
                return _maxHealth;
            }
        }

        public int CurrentHealth
        {
            get
            {
                InitializeHealth();
                return _currentHealth;
            }
        }

        private void InitializeHealth()
        {
            if (_isInitialized) return;
            _maxHealth = maxHearts * 2;
            _currentHealth = _maxHealth;
            _isInitialized = true;
        }

        private void Awake()
        {
            InitializeHealth();
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
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Cheat hook. The whole cheat system is compiled out of release builds, so this call
            // does not exist there either.
            if (Debugging.CheatManager.GodMode) return;
#endif

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

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Cheat menu only: forces health to an exact value in half-hearts, ignoring god mode and
        /// the invincibility window, so death and damage can be demonstrated on demand without
        /// having to switch other cheats off first.
        /// </summary>
        public void CheatSetHealth(int halfHearts)
        {
            InitializeHealth();

            _currentHealth = Mathf.Clamp(halfHearts, 0, _maxHealth);
            _invincibleTimer = 0f;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
#endif
    }
}
