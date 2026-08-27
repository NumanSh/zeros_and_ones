using UnityEngine;

namespace ZerosAndOnes.Enemies
{
    public class SwingingObstacle : MonoBehaviour
    {
        [Header("Swinging Settings")]
        [Tooltip("How fast the obstacle swings back and forth.")]
        [SerializeField] private float speed = 2f;

        [Tooltip("The maximum angle of the swing in degrees (e.g. 45 degrees left and right).")]
        [SerializeField] private float maxAngle = 45f;

        [Tooltip("Use this offset (in seconds) to desynchronize multiple swinging obstacles.")]
        [SerializeField] private float startOffset = 0f;

        private void Update()
        {
            // Calculate Z-rotation angle using a sine wave
            float angle = Mathf.Sin((Time.time * speed) + startOffset) * maxAngle;
            
            // Apply rotation around the Z-axis
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
