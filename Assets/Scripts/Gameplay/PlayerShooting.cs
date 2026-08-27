using UnityEngine;
using UnityEngine.InputSystem;

namespace ZerosAndOnes.Gameplay
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.4f;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private float _fireCooldownTimer;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        private void Update()
        {
            // No shooting while the pause menu is open.
            if (UI.PauseMenuController.IsPaused) return;

            // Update cooldown timer
            if (_fireCooldownTimer > 0f)
            {
                _fireCooldownTimer -= Time.deltaTime;
            }

            // Check input for shooting (Space key or Left Mouse Click)
            bool shootInputPressed = false;
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                shootInputPressed = true;
            }
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                shootInputPressed = true;
            }

            // Fire projectile if inputs are pressed and off cooldown
            if (shootInputPressed && _fireCooldownTimer <= 0f)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (projectilePrefab == null)
            {
                Debug.LogWarning("[PlayerShooting] Projectile Prefab is missing, cannot shoot!");
                return;
            }

            // Reset cooldown
            _fireCooldownTimer = fireRate;

            // Determine shoot direction based on SpriteRenderer flip state
            Vector2 shootDirection = Vector2.right;
            if (spriteRenderer != null && spriteRenderer.flipX)
            {
                shootDirection = Vector2.left;
            }

            // Determine spawn position
            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + (Vector3)(shootDirection * 0.5f);

            // Instantiate and launch the projectile
            GameObject projectileObj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            PlayerProjectile projectile = projectileObj.GetComponent<PlayerProjectile>();
            
            if (projectile != null)
            {
                projectile.Launch(shootDirection);
            }
            else
            {
                Debug.LogError("[PlayerShooting] Spawned prefab is missing PlayerProjectile script component.");
            }
        }
    }
}
