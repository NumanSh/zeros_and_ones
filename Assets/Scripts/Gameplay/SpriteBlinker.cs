using UnityEngine;
using UnityEngine.UI;

namespace ZerosAndOnes.Gameplay
{
    /// <summary>
    /// Makes a sprite (or UI Image) appear and disappear on a fixed interval.
    /// Attach to the object you want to blink, e.g. the lightning strike image.
    /// </summary>
    public class SpriteBlinker : MonoBehaviour
    {
        [Header("Blink Settings")]
        [Tooltip("Seconds the image stays visible / hidden before toggling.")]
        [SerializeField] private float interval = 3f;
        [Tooltip("Whether the image starts visible.")]
        [SerializeField] private bool startVisible = true;

        [Header("Sound Settings")]
        [Tooltip("Sound played each time the image appears (e.g. thunder).")]
        [SerializeField] private AudioClip appearSound;
        [Range(0f, 1f)]
        [SerializeField] private float soundVolume = 1f;

        private SpriteRenderer _spriteRenderer;
        private Image _uiImage;
        private AudioSource _audioSource;
        private float _timer;
        private bool _visible;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _uiImage = GetComponent<Image>();

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            _audioSource.playOnAwake = false;

            if (_spriteRenderer == null && _uiImage == null)
            {
                Debug.LogWarning("[SpriteBlinker] No SpriteRenderer or Image found on " + gameObject.name);
                enabled = false;
                return;
            }

            _visible = startVisible;
            ApplyVisibility();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= interval)
            {
                _timer = 0f;
                _visible = !_visible;
                ApplyVisibility();

                if (_visible && appearSound != null)
                {
                    _audioSource.PlayOneShot(appearSound, soundVolume);
                }
            }
        }

        private void ApplyVisibility()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = _visible;
            }

            if (_uiImage != null)
            {
                _uiImage.enabled = _visible;
            }
        }
    }
}
