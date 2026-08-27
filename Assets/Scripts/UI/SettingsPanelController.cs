using UnityEngine;
using UnityEngine.UI;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Attach to the SettingsPanel GameObject. Wires Master/Music/SFX sliders to the
    /// SettingsManager and handles opening/closing the panel.
    /// </summary>
    public class SettingsPanelController : MonoBehaviour
    {
        [Header("Volume Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Buttons")]
        [Tooltip("Button on the main menu that opens this panel.")]
        [SerializeField] private Button openButton;

        [Tooltip("Button inside this panel that closes it.")]
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (openButton != null)
                openButton.onClick.AddListener(Open);

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            InitializeSliders();
        }


        private void InitializeSliders()
        {
            var settings = SettingsManager.Instance;
            if (settings == null) return;

            SetupSlider(masterVolumeSlider, settings.MasterVolume, settings.SetMasterVolume);
            SetupSlider(musicVolumeSlider, settings.MusicVolume, settings.SetMusicVolume);
            SetupSlider(sfxVolumeSlider, settings.SFXVolume, settings.SetSFXVolume);
        }

        private static void SetupSlider(Slider slider, float currentValue, UnityEngine.Events.UnityAction<float> onChanged)
        {
            if (slider == null) return;

            slider.onValueChanged.RemoveAllListeners();
            slider.SetValueWithoutNotify(currentValue);
            slider.onValueChanged.AddListener(onChanged);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
