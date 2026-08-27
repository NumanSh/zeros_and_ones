using UnityEngine;
using UnityEngine.InputSystem;

namespace ZerosAndOnes.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpForce = 11f;

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
        [SerializeField] private LayerMask groundLayer;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private Animator _animator;

        private BoxCollider2D _boxCollider;
        private Vector2 _originalColliderSize;
        private Vector2 _originalColliderOffset;
        private Vector3 _originalVisualScale;
        private bool _isCrouching;

        private float _horizontalInput;
        private bool _isGrounded;

        // Animation Parameter Hashes
        private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
        private static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("verticalVelocity");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            _boxCollider = GetComponent<BoxCollider2D>();
            if (_boxCollider != null)
            {
                _originalColliderSize = _boxCollider.size;
                _originalColliderOffset = _boxCollider.offset;
            }

            if (spriteRenderer != null)
            {
                _originalVisualScale = spriteRenderer.transform.localScale;
            }
        }

        private void Update()
        {
            // Update() still runs at Time.timeScale = 0, so without this the pause menu would keep
            // reading input: a jump queued here sets _rb.velocity and fires the moment we unpause.
            if (UI.PauseMenuController.IsPaused) return;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // The cheat panel owns the keyboard while it is open, otherwise WASD and the text typed
            // into its fields would drive the character at the same time.
            if (Debugging.CheatManager.CapturesGameplayInput)
            {
                _horizontalInput = 0f;
                return;
            }
#endif

            // Gather inputs using New Input System
            float horizontal = 0f;
            bool jumpPressed = false;
            bool jumpReleased = false;
            bool crouchHeld = false;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                {
                    crouchHeld = true;
                }

                // Jump on W or Up Arrow
                if (Keyboard.current.wKey.wasPressedThisFrame ||
                    Keyboard.current.upArrowKey.wasPressedThisFrame)
                {
                    jumpPressed = true;
                }
                if (Keyboard.current.wKey.wasReleasedThisFrame ||
                    Keyboard.current.upArrowKey.wasReleasedThisFrame)
                {
                    jumpReleased = true;
                }
            }

            _horizontalInput = horizontal;

            // Handle sprite flipping
            if (_horizontalInput > 0.1f)
            {
                if (spriteRenderer != null) spriteRenderer.flipX = false;
            }
            else if (_horizontalInput < -0.1f)
            {
                if (spriteRenderer != null) spriteRenderer.flipX = true;
            }

            // Ground checking
            CheckGround();

            // Crouch handling
            if (crouchHeld && _isGrounded)
            {
                if (!_isCrouching) Crouch(true);
            }
            else
            {
                if (_isCrouching && CanStandUp())
                {
                    Crouch(false);
                }
            }

            // Jump handling
            if (jumpPressed && _isGrounded && !_isCrouching)
            {
                Jump();
            }

            // Variable jump height cut-off
            if (jumpReleased && _rb.velocity.y > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            }

            // Update animations
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            // Standard horizontal running/crouch physics
            float currentSpeed = _isCrouching ? moveSpeed * 0.5f : moveSpeed;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            currentSpeed *= Debugging.CheatManager.MoveSpeedMultiplier;
#endif

            _rb.velocity = new Vector2(_horizontalInput * currentSpeed, _rb.velocity.y);
        }

        private void CheckGround()
        {
            _isGrounded = false;
            Vector2 checkPosition;
            Vector2 checkSize;

            if (groundCheckPoint != null)
            {
                checkPosition = groundCheckPoint.position;
                checkSize = groundCheckSize;
            }
            else
            {
                checkPosition = new Vector2(_collider.bounds.center.x, _collider.bounds.min.y);
                checkSize = new Vector2(groundCheckSize.x, 0.2f);
            }

            Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPosition, checkSize, 0f, groundLayer);
            
            System.Text.StringBuilder debugInfo = new System.Text.StringBuilder();
            
            foreach (var col in colliders)
            {
                debugInfo.Append($"{col.gameObject.name} [Layer: {LayerMask.LayerToName(col.gameObject.layer)}, Trigger: {col.isTrigger}], ");
                
                // Ignore the player's own colliders and trigger colliders (doors, portals, items)
                if (col.gameObject != gameObject && !col.transform.IsChildOf(transform) && !col.isTrigger)
                {
                    _isGrounded = true;
                }
            }

            // Print deep coordinates debug statement in console
            string colliderBoundsInfo = _collider != null ? $"Collider MinY: {_collider.bounds.min.y:F3}, CenterY: {_collider.bounds.center.y:F3}" : "No Collider";
            
            // if (colliders.Length > 0)
            // {
            //     Debug.Log($"[CheckGround Debug] Grounded: {_isGrounded}. {colliderBoundsInfo}. CheckPos: {checkPosition.x:F3},{checkPosition.y:F3}. CheckSize: {checkSize.x:F3},{checkSize.y:F3}. Detected: {debugInfo}");
            // }
            // else
            // {
            //     Debug.Log($"[CheckGround Debug] Grounded: {_isGrounded}. {colliderBoundsInfo}. CheckPos: {checkPosition.x:F3},{checkPosition.y:F3}. CheckSize: {checkSize.x:F3},{checkSize.y:F3}. No colliders detected at feet.");
            // }
        }

        private void Jump()
        {
            float force = jumpForce;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            force *= Debugging.CheatManager.JumpForceMultiplier;
#endif

            _rb.velocity = new Vector2(_rb.velocity.x, force);
        }

        private bool CanStandUp()
        {
            if (_boxCollider == null) return true;

            // Calculate the ceiling check area above the crouched player
            float checkHeight = _originalColliderSize.y - _boxCollider.size.y;
            Vector2 checkSize = new Vector2(_boxCollider.size.x * 0.9f, 0.05f);
            Vector2 checkCenter = new Vector2(
                transform.position.x + _boxCollider.offset.x,
                transform.position.y + _boxCollider.offset.y + (_boxCollider.size.y / 2f) + (checkHeight / 2f) + 0.05f
            );

            // Check if there is anything on the Ground layer blocking us
            return !Physics2D.OverlapBox(checkCenter, checkSize, 0f, groundLayer);
        }

        private void Crouch(bool crouch)
        {
            _isCrouching = crouch;
            bool isRootSprite = (spriteRenderer != null && spriteRenderer.transform == transform);

            if (crouch)
            {
                if (isRootSprite)
                {
                    // If the sprite is on the root, we scale the root transform itself.
                    // This automatically scales the collider, so we do not resize the collider size manually to avoid double-scaling.
                    transform.localScale = new Vector3(_originalVisualScale.x, _originalVisualScale.y * 0.6f, _originalVisualScale.z);
                    
                    // We must NOT change transform.localPosition of the root to avoid teleporting the character.
                    if (_boxCollider != null)
                    {
                        _boxCollider.size = _originalColliderSize;
                        _boxCollider.offset = _originalColliderOffset;
                    }
                }
                else
                {
                    // If the sprite is on a child GameObject, we shrink the collider on the root and squish the child visual.
                    if (_boxCollider != null)
                    {
                        float newHeight = _originalColliderSize.y * 0.6f;
                        _boxCollider.size = new Vector2(_originalColliderSize.x, newHeight);
                        float originalBottom = _originalColliderOffset.y - (_originalColliderSize.y / 2f);
                        float newBottom = originalBottom + 0.05f;
                        _boxCollider.offset = new Vector2(_originalColliderOffset.x, newBottom + (newHeight / 2f));
                    }
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.transform.localScale = new Vector3(_originalVisualScale.x, _originalVisualScale.y * 0.6f, _originalVisualScale.z);
                        spriteRenderer.transform.localPosition = new Vector3(0f, -(_originalColliderSize.y * 0.2f), 0f);
                    }
                }
            }
            else
            {
                if (isRootSprite)
                {
                    transform.localScale = _originalVisualScale;
                }
                else
                {
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.transform.localScale = _originalVisualScale;
                        spriteRenderer.transform.localPosition = Vector3.zero;
                    }
                }

                if (_boxCollider != null)
                {
                    _boxCollider.size = _originalColliderSize;
                    _boxCollider.offset = _originalColliderOffset;
                }
            }
        }

        private void UpdateAnimations()
        {
            if (_animator == null) return;

            _animator.SetBool(IsRunningHash, Mathf.Abs(_rb.velocity.x) > 0.1f);
            _animator.SetBool(IsGroundedHash, _isGrounded);
            _animator.SetFloat(VerticalVelocityHash, _rb.velocity.y);
        }

        // Draw ground check helper in Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (groundCheckPoint != null)
            {
                Gizmos.DrawWireCube(groundCheckPoint.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
            }
            else if (_collider != null)
            {
                Vector3 boxCenter = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, 0f);
                Gizmos.DrawWireCube(boxCenter, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
            }
        }
    }
}
