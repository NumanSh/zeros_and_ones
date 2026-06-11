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
        [SerializeField] private float climbSpeed = 5f;

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
        private float _verticalInput;
        private bool _isGrounded;
        private bool _isNearLadder;
        private bool _isClimbing;
        private float _originalGravityScale;

        // Animation Parameter Hashes
        private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
        private static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
        private static readonly int IsClimbingHash = Animator.StringToHash("isClimbing");
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

            _originalGravityScale = _rb.gravityScale;
        }

        private void Update()
        {
            // Gather inputs using New Input System
            float horizontal = 0f;
            float vertical = 0f;
            bool jumpPressed = false;
            bool jumpReleased = false;
            bool crouchHeld = false;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                {
                    vertical -= 1f;
                    crouchHeld = true;
                }
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                {
                    vertical += 1f;
                }

                // Jump on Space, W, or Up Arrow
                if (Keyboard.current.spaceKey.wasPressedThisFrame ||
                    Keyboard.current.wKey.wasPressedThisFrame ||
                    Keyboard.current.upArrowKey.wasPressedThisFrame)
                {
                    jumpPressed = true;
                }
                if (Keyboard.current.spaceKey.wasReleasedThisFrame ||
                    Keyboard.current.wKey.wasReleasedThisFrame ||
                    Keyboard.current.upArrowKey.wasReleasedThisFrame)
                {
                    jumpReleased = true;
                }
            }

            _horizontalInput = horizontal;
            _verticalInput = vertical;

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

            // Ladder transition logic
            if (_isNearLadder && Mathf.Abs(_verticalInput) > 0.1f)
            {
                _isClimbing = true;
            }

            if (!_isNearLadder)
            {
                _isClimbing = false;
            }

            // Crouch handling
            if (crouchHeld && _isGrounded && !_isClimbing)
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
            if (jumpPressed && _isGrounded && !_isClimbing && !_isCrouching)
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
            if (_isClimbing)
            {
                // Disables gravity during climbing
                _rb.gravityScale = 0f;
                // Move vertically and horizontally (climbing movement)
                _rb.velocity = new Vector2(_horizontalInput * moveSpeed * 0.75f, _verticalInput * climbSpeed);
            }
            else
            {
                // Re-enable gravity scale
                _rb.gravityScale = _originalGravityScale;
                // Standard horizontal running/crouch physics
                float currentSpeed = _isCrouching ? moveSpeed * 0.5f : moveSpeed;
                _rb.velocity = new Vector2(_horizontalInput * currentSpeed, _rb.velocity.y);
            }
        }

        private void CheckGround()
        {
            if (groundCheckPoint != null)
            {
                _isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);
            }
            else
            {
                // Fallback: check slightly below player collider bounds
                Vector2 boxCenter = new Vector2(_collider.bounds.center.x, _collider.bounds.min.y);
                _isGrounded = Physics2D.OverlapBox(boxCenter, groundCheckSize, 0f, groundLayer);
            }
        }

        private void Jump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
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
            _animator.SetBool(IsClimbingHash, _isClimbing);
            _animator.SetFloat(VerticalVelocityHash, _rb.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ladder"))
            {
                _isNearLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ladder"))
            {
                _isNearLadder = false;
                _isClimbing = false;
            }
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
