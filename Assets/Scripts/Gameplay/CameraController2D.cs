using UnityEngine;

namespace ZerosAndOnes.Gameplay
{
    public class CameraController2D : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f);

        [Header("Boundaries")]
        [SerializeField] private bool useBounds = true;
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minY = -5f;
        [SerializeField] private float maxY = 15f;

        private void LateUpdate()
        {
            if (target == null) return;

            // Desired camera position
            Vector3 desiredPosition = target.position + offset;

            // Apply bounds restriction
            if (useBounds)
            {
                float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
                float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);
                desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
            }

            // Smoothly move the camera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        // Helper method to set the target programmatically
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        // Draw boundaries in Editor for design convenience
        private void OnDrawGizmosSelected()
        {
            if (!useBounds) return;

            Gizmos.color = Color.red;
            
            // Draw a rectangle representing the camera movement limits
            Vector3 topLeft = new Vector3(minX, maxY, 0f);
            Vector3 topRight = new Vector3(maxX, maxY, 0f);
            Vector3 bottomLeft = new Vector3(minX, minY, 0f);
            Vector3 bottomRight = new Vector3(maxX, minY, 0f);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}
