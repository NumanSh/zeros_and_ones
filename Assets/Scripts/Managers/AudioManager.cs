using UnityEngine;
using UnityEngine.Audio;

namespace ZerosAndOnes.Managers
{
    /// <summary>
    /// Persistent singleton for playing music and one-shot SFX. Routes audio through
    /// the AudioMixer's Music/SFX groups so SettingsManager's volume sliders affect it.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Mixer Routing")]
        [Tooltip("Assign the mixer's Music group here.")]
        [SerializeField] private AudioMixerGroup musicMixerGroup;

        [Tooltip("Assign the mixer's SFX group here.")]
        [SerializeField] private AudioMixerGroup sfxMixerGroup;

        [Header("Music")]
        [SerializeField] private AudioClip defaultMusicClip;
        [SerializeField] private bool playDefaultMusicOnStart = false;

        [Header("SFX")]
        [Tooltip("Number of simultaneous one-shot SFX sources.")]
        [SerializeField] private int sfxSourcePoolSize = 4;

        private AudioSource musicSource;
        private AudioSource[] sfxSources;
        private int nextSfxSourceIndex;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SetupAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (playDefaultMusicOnStart && defaultMusicClip != null)
            {
                PlayMusic(defaultMusicClip);
            }
        }

        private void SetupAudioSources()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.outputAudioMixerGroup = musicMixerGroup;

            sfxSourcePoolSize = Mathf.Max(1, sfxSourcePoolSize);
            sfxSources = new AudioSource[sfxSourcePoolSize];
            for (int i = 0; i < sfxSourcePoolSize; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.outputAudioMixerGroup = sfxMixerGroup;
                sfxSources[i] = source;
            }
        }

        /// <summary>Plays looping background music. Restarts only if the clip differs from the one currently playing.</summary>
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null || musicSource == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource == null) return;
            musicSource.Stop();
        }

        public void PauseMusic()
        {
            if (musicSource == null) return;
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            if (musicSource == null) return;
            musicSource.UnPause();
        }

        /// <summary>Plays a one-shot sound effect from the pool, cycling through sources round-robin.</summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null || sfxSources == null || sfxSources.Length == 0) return;

            var source = sfxSources[nextSfxSourceIndex];
            nextSfxSourceIndex = (nextSfxSourceIndex + 1) % sfxSources.Length;
            source.PlayOneShot(clip, volumeScale);
        }
    }
}
