using UnityEngine;
using UnityEngine.Audio;

namespace ZerosAndOnes.Managers
{
    /// <summary>
    /// Persistent singleton that owns audio volume settings (Master/Music/SFX),
    /// applies them to the AudioMixer, and saves/loads them via PlayerPrefs.
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        private const string MasterVolumeKey = "Settings.MasterVolume";
        private const string MusicVolumeKey = "Settings.MusicVolume";
        private const string SFXVolumeKey = "Settings.SFXVolume";

        private const float DefaultVolume = 1f;
        private const float MinDb = -80f;

        [Header("Audio Mixer")]
        [Tooltip("Assign the game's AudioMixer asset. Must expose 'MasterVolume', 'MusicVolume', and 'SFXVolume' parameters.")]
        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private string masterParam = "MasterVolume";
        [SerializeField] private string musicParam = "MusicVolume";
        [SerializeField] private string sfxParam = "SFXVolume";

        public float MasterVolume { get; private set; } = DefaultVolume;
        public float MusicVolume { get; private set; } = DefaultVolume;
        public float SFXVolume { get; private set; } = DefaultVolume;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadSettings()
        {
            MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultVolume);
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultVolume);
            SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, DefaultVolume);

            ApplyMasterVolume();
            ApplyMusicVolume();
            ApplySFXVolume();
        }

        public void SetMasterVolume(float linearVolume)
        {
            MasterVolume = linearVolume;
            ApplyMasterVolume();
            PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        }

        public void SetMusicVolume(float linearVolume)
        {
            MusicVolume = linearVolume;
            ApplyMusicVolume();
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        }

        public void SetSFXVolume(float linearVolume)
        {
            SFXVolume = linearVolume;
            ApplySFXVolume();
            PlayerPrefs.SetFloat(SFXVolumeKey, SFXVolume);
        }

        private void ApplyMasterVolume() => ApplyToMixer(masterParam, MasterVolume);
        private void ApplyMusicVolume() => ApplyToMixer(musicParam, MusicVolume);
        private void ApplySFXVolume() => ApplyToMixer(sfxParam, SFXVolume);

        private void ApplyToMixer(string parameterName, float linearVolume)
        {
            if (audioMixer == null || string.IsNullOrEmpty(parameterName)) return;

            float db = linearVolume > 0.0001f ? Mathf.Log10(linearVolume) * 20f : MinDb;
            audioMixer.SetFloat(parameterName, db);
        }
    }
}
