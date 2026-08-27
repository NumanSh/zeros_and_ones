using UnityEngine;
using ZerosAndOnes.Gameplay;

namespace ZerosAndOnes.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PatrolEnemy : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float patrolSpeed = 3f;
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointTolerance = 0.2f;

        [Header("Detection Settings")]
        [SerializeField] private bool detectPlayer = true;
        [SerializeField] private float detectionRadius = 5f;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        private Rigidbody2D _rb;
        private int _currentWaypointIndex = 0;
        private Transform _playerTransform;
        private bool _isChasing = false;

        private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        private void Start()
        {
            // Locate player transform via PlayerHealth component in the scene
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }

        private void Update()
        {
            CheckPlayerDetection();
        }

        private void FixedUpdate()
        {
            MoveEnemy();
        }

        private void CheckPlayerDetection()
        {
            if (!detectPlayer || _playerTransform == null)
            {
                _isChasing = false;
                return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                _isChasing = true;
            }
            else
            {
                _isChasing = false;
            }
        }

        private void MoveEnemy()
        {
            float speed = _isChasing ? chaseSpeed : patrolSpeed;
            Vector2 targetPos;

            if (_isChasing && _playerTransform != null)
            {
                targetPos = _playerTransform.position;
            }
            else
            {
                if (waypoints == null || waypoints.Length == 0)
                {
                    // No waypoints defined, remain stationary
                    _rb.velocity = new Vector2(0f, _rb.velocity.y);
                    if (animator != null) animator.SetBool(IsWalkingHash, false);
                    return;
                }
                targetPos = waypoints[_currentWaypointIndex].position;

                // Check if waypoint reached
                float distanceToWaypoint = Mathf.Abs(transform.position.x - targetPos.x);
                if (distanceToWaypoint <= waypointTolerance)
                {
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
                    targetPos = waypoints[_currentWaypointIndex].position;
                }
            }

            // Calculate direction and move horizontally
            float direction = Mathf.Sign(targetPos.x - transform.position.x);
            _rb.velocity = new Vector2(direction * speed, _rb.velocity.y);

            // Sprite flipping to face movement direction
            if (direction > 0.1f)
            {
                if (spriteRenderer != null) spriteRenderer.flipX = false;
            }
            else if (direction < -0.1f)
            {
                if (spriteRenderer != null) spriteRenderer.flipX = true;
            }

            // Update walking animation state
            if (animator != null)
            {
                animator.SetBool(IsWalkingHash, true);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw detection range in editor
            if (detectPlayer)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
                Gizmos.DrawWireSphere(transform.position, detectionRadius);
            }

            // Draw line between waypoints
            if (waypoints != null && waypoints.Length > 1)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    if (waypoints[i] != null)
                    {
                        Transform next = waypoints[(i + 1) % waypoints.Length];
                        if (next != null)
                        {
                            Gizmos.DrawLine(waypoints[i].position, next.position);
                        }
                    }
                }
            }
        }
    }
}
