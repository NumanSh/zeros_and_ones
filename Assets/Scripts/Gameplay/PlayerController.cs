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
        [SerializeField] private float jumpForce = 14f;
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

            _originalGravityScale = _rb.gravityScale;
        }

        private void Update()
        {
            // Gather inputs using New Input System
            float horizontal = 0f;
            float vertical = 0f;
            bool jumpPressed = false;
            bool jumpReleased = false;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;

                if (Keyboard.current.spaceKey.wasPressedThisFrame) jumpPressed = true;
                if (Keyboard.current.spaceKey.wasReleasedThisFrame) jumpReleased = true;
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

            // Jump handling
            if (jumpPressed && _isGrounded && !_isClimbing)
            {
                Jump();
            }

            // Variable jump height cut-off
            if (jumpReleased && _rb.linearVelocity.y > 0f)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
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
                _rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed * 0.75f, _verticalInput * climbSpeed);
            }
            else
            {
                // Re-enable gravity scale
                _rb.gravityScale = _originalGravityScale;
                // Standard horizontal running physics
                _rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed, _rb.linearVelocity.y);
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
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }

        private void UpdateAnimations()
        {
            if (_animator == null) return;

            _animator.SetBool(IsRunningHash, Mathf.Abs(_rb.linearVelocity.x) > 0.1f);
            _animator.SetBool(IsGroundedHash, _isGrounded);
            _animator.SetBool(IsClimbingHash, _isClimbing);
            _animator.SetFloat(VerticalVelocityHash, _rb.linearVelocity.y);
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
