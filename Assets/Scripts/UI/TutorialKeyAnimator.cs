using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Animates a UI Image or SpriteRenderer representing a keyboard key.
    /// Cycles through unpressed, intermediate, and pressed states (3 frames).
    /// Captures physical keyboard inputs to reflect real-time presses on the overlay.
    /// </summary>
    public class TutorialKeyAnimator : MonoBehaviour
    {
        [Header("Sprite Configuration")]
        [Tooltip("Expects 3 sprites: 0 = Normal/Unpressed, 1 = Intermediate/Hover, 2 = Pressed.")]
        [SerializeField] private Sprite[] keyFrames;

        [Header("Animation Settings")]
        [SerializeField] private bool autoSimulateClick = true;
        [SerializeField] private float clickInterval = 1.5f;
        [SerializeField] private float pressDuration = 0.25f;

        [Header("Input Monitoring")]
        [Tooltip("The keyboard key to monitor. Options: a, d, w, s, space, left, right, up, down")]
        [SerializeField] private string keyboardKeyName;

        // References to UI Image or SpriteRenderer
        private Image _uiImage;
        private SpriteRenderer _spriteRenderer;

        private float _nextSimulatedClickTime;
        private bool _isSimulatingPress;

        private void Awake()
        {
            _uiImage = GetComponent<Image>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Set initial simulated click timer
            _nextSimulatedClickTime = Time.time + Random.Range(0f, clickInterval);
        }

        private void Update()
        {
            bool isPhysicallyPressed = IsKeyPressed();

            if (isPhysicallyPressed)
            {
                // Stop any simulated press if the player actually presses it
                _isSimulatingPress = false;
                SetSpriteFrame(2);
            }
            else if (_isSimulatingPress)
            {
                SetSpriteFrame(2);
            }
            else
            {
                // Idle / unpressed state
                SetSpriteFrame(0);

                // Handle auto-simulated click cycle
                if (autoSimulateClick && Time.time >= _nextSimulatedClickTime)
                {
                    StartCoroutine(SimulateClickRoutine());
                }
            }
        }

        private void SetSpriteFrame(int index)
        {
            if (keyFrames == null || keyFrames.Length == 0) return;
            
            // Clamp index to available frames
            int safeIndex = Mathf.Clamp(index, 0, keyFrames.Length - 1);
            Sprite targetSprite = keyFrames[safeIndex];

            if (_uiImage != null)
            {
                _uiImage.sprite = targetSprite;
            }
            else if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = targetSprite;
            }
        }

        private IEnumerator SimulateClickRoutine()
        {
            _nextSimulatedClickTime = Time.time + clickInterval + pressDuration;
            
            // Frame 1: intermediate hover
            SetSpriteFrame(1);
            yield return new WaitForSeconds(0.08f);

            // Frame 2: fully pressed
            _isSimulatingPress = true;
            yield return new WaitForSeconds(pressDuration);
            _isSimulatingPress = false;

            // Frame 1: intermediate release
            SetSpriteFrame(1);
            yield return new WaitForSeconds(0.08f);
            
            SetSpriteFrame(0);
        }

        private bool IsKeyPressed()
        {
            if (Keyboard.current == null || string.IsNullOrEmpty(keyboardKeyName)) return false;

            switch (keyboardKeyName.ToLower())
            {
                case "a": 
                    return Keyboard.current.aKey.isPressed;
                case "d": 
                    return Keyboard.current.dKey.isPressed;
                case "w": 
                    return Keyboard.current.wKey.isPressed;
                case "s": 
                    return Keyboard.current.sKey.isPressed;
                case "space": 
                    return Keyboard.current.spaceKey.isPressed;
                case "left": 
                    return Keyboard.current.leftArrowKey.isPressed;
                case "right": 
                    return Keyboard.current.rightArrowKey.isPressed;
                case "up": 
                    return Keyboard.current.upArrowKey.isPressed;
                case "down": 
                    return Keyboard.current.downArrowKey.isPressed;
                default: 
                    return false;
            }
        }
    }
}
