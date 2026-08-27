using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// The in-game pause menu. Pressing ESC freezes the game (Time.timeScale = 0) and shows an
    /// overlay with Resume, Restart, Settings and Exit to Main Menu.
    ///
    /// The whole UI is built in code by <see cref="PauseMenuUIFactory"/> and spawned into every
    /// gameplay scene by <see cref="PauseMenuBootstrap"/>, so no prefab or per-scene setup is needed.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        public const string MainMenuSceneName = "MainMenuMap";

        // PlayerPrefs keys, shared with SettingsManager and DisplaySettingsController so the pause
        // menu and the main menu's own settings panel always agree.
        private const string MasterVolumeKey = "Settings.MasterVolume";
        private const string MusicVolumeKey = "Settings.MusicVolume";
        private const string SFXVolumeKey = "Settings.SFXVolume";
        private const string FullscreenKey = "Settings.Fullscreen";
        private const string ResolutionWidthKey = "Settings.ResolutionWidth";
        private const string ResolutionHeightKey = "Settings.ResolutionHeight";

        /// <summary>
        /// True while the game is paused. Gameplay scripts check this to skip their Update logic,
        /// because Update() keeps running at Time.timeScale = 0.
        /// </summary>
        public static bool IsPaused { get; private set; }

        private GameObject _overlayRoot;
        private RectTransform _rootPanel;
        private RectTransform _settingsPanel;

        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _sfxSlider;
        private Toggle _fullscreenToggle;
        private TMP_Dropdown _resolutionDropdown;
        private List<Resolution> _availableResolutions;

        private GameState _stateBeforePause = GameState.ExplorationMap;

        private void Awake()
        {
            IsPaused = false;
            BuildUI();
            _overlayRoot.SetActive(false);
        }

        private void OnDestroy()
        {
            // A scene change while paused must not leave the game frozen or the flag stuck on.
            if (IsPaused)
            {
                IsPaused = false;
                Time.timeScale = 1f;
            }
        }

        private void Update()
        {
            if (Keyboard.current == null) return;
            if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;

            if (!IsPaused)
            {
                Pause();
            }
            else if (_settingsPanel.gameObject.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                Resume();
            }
        }

        // ---------------------------------------------------------------- actions

        public void Pause()
        {
            if (IsPaused) return;

            IsPaused = true;
            Time.timeScale = 0f;

            if (GameManager.Instance != null)
            {
                _stateBeforePause = GameManager.Instance.CurrentState;
                GameManager.Instance.SetState(GameState.Paused);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PauseMusic();
            }

            _overlayRoot.SetActive(true);
            ShowRootPanel();
        }

        public void Resume()
        {
            if (!IsPaused) return;

            IsPaused = false;
            Time.timeScale = 1f;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(_stateBeforePause);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ResumeMusic();
            }

            _overlayRoot.SetActive(false);
        }

        public void Restart()
        {
            // Restore time before loading, otherwise the new scene starts frozen.
            ReleasePause();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitToMainMenu()
        {
            ReleasePause();

            // Clear the door re-entry guard so doors behave normally on the next run.
            ExitButton.LastSceneName = string.Empty;

            if (Application.CanStreamedLevelBeLoaded(MainMenuSceneName))
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
            else
            {
                Debug.LogError($"[PauseMenuController] Scene '{MainMenuSceneName}' is not in Build Settings.");
            }
        }

        public void OpenSettings()
        {
            RefreshSettingsUI();
            _rootPanel.gameObject.SetActive(false);
            _settingsPanel.gameObject.SetActive(true);
        }

        public void CloseSettings()
        {
            ShowRootPanel();
        }

        private void ShowRootPanel()
        {
            _settingsPanel.gameObject.SetActive(false);
            _rootPanel.gameObject.SetActive(true);
        }

        private void ReleasePause()
        {
            IsPaused = false;
            Time.timeScale = 1f;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ResumeMusic();
            }
        }

        // ---------------------------------------------------------------- UI construction

        private void BuildUI()
        {
            // Far above every existing scene canvas (the highest in the project is 100), but below
            // 30000: that is the sorting order TMP_Dropdown gives its popup list, which has to be
            // able to draw on top of this panel.
            var canvas = PauseMenuUIFactory.CreateCanvas("PauseMenuCanvas", 29000);
            _overlayRoot = canvas.gameObject;
            _overlayRoot.transform.SetParent(transform, false);

            // Dim backdrop; raycastTarget blocks clicks on anything underneath.
            PauseMenuUIFactory.CreateFullScreenImage(_overlayRoot.transform, "Dim", PauseMenuUIFactory.DimColor);

            BuildRootPanel(_overlayRoot.transform);
            BuildSettingsPanel(_overlayRoot.transform);

            _settingsPanel.gameObject.SetActive(false);
        }

        private void BuildRootPanel(Transform parent)
        {
            // Padding keeps the content clear of the board art's carved frame. Height covers the
            // 83px title, four 62px buttons, four 16px gaps and 86 of padding.
            _rootPanel = PauseMenuUIFactory.CreatePanel(parent, "PausePanel", new Vector2(560f, 500f), 16f,
                new RectOffset(52, 52, 40, 46));

            PauseMenuUIFactory.CreateLabel(_rootPanel, "Title", "PAUSED", 52f, FontStyles.Bold);

            PauseMenuUIFactory.CreateButton(_rootPanel, "ResumeButton", "Resume", 62f, Resume);
            PauseMenuUIFactory.CreateButton(_rootPanel, "RestartButton", "Restart", 62f, Restart);
            PauseMenuUIFactory.CreateButton(_rootPanel, "SettingsButton", "Settings", 62f, OpenSettings);
            PauseMenuUIFactory.CreateButton(_rootPanel, "ExitButton", "Exit to Main Menu", 62f, ExitToMainMenu);
        }

        private void BuildSettingsPanel(Transform parent)
        {
            // Sized for its content: title 70 + header 45 + three 46px sliders + header 45 +
            // toggle 46 + dropdown 46 + back 58, plus 8 gaps of 14 and 84 of padding.
            _settingsPanel = PauseMenuUIFactory.CreatePanel(parent, "PauseSettingsPanel", new Vector2(700f, 680f), 14f,
                new RectOffset(56, 56, 38, 46));

            PauseMenuUIFactory.CreateLabel(_settingsPanel, "Title", "SETTINGS", 44f, FontStyles.Bold);

            PauseMenuUIFactory.CreateLabel(_settingsPanel, "AudioHeader", "Audio", 28f, FontStyles.Bold,
                TextAlignmentOptions.MidlineLeft);

            _masterSlider = PauseMenuUIFactory.CreateSliderRow(_settingsPanel, "MasterRow", "Master Volume");
            _musicSlider = PauseMenuUIFactory.CreateSliderRow(_settingsPanel, "MusicRow", "Music Volume");
            _sfxSlider = PauseMenuUIFactory.CreateSliderRow(_settingsPanel, "SfxRow", "SFX Volume");

            PauseMenuUIFactory.CreateLabel(_settingsPanel, "DisplayHeader", "Display", 28f, FontStyles.Bold,
                TextAlignmentOptions.MidlineLeft);

            _fullscreenToggle = PauseMenuUIFactory.CreateToggleRow(_settingsPanel, "FullscreenRow", "Fullscreen");
            _resolutionDropdown = PauseMenuUIFactory.CreateDropdownRow(_settingsPanel, "ResolutionRow", "Resolution");

            PauseMenuUIFactory.CreateButton(_settingsPanel, "BackButton", "Back", 58f, CloseSettings);

            _masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            _fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
            _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        // ---------------------------------------------------------------- settings

        /// <summary>
        /// Pushes current values into the widgets without firing their listeners, mirroring
        /// SettingsPanelController.SetupSlider's SetValueWithoutNotify pattern.
        /// </summary>
        private void RefreshSettingsUI()
        {
            var settings = SettingsManager.Instance;

            _masterSlider.SetValueWithoutNotify(settings != null
                ? settings.MasterVolume
                : PlayerPrefs.GetFloat(MasterVolumeKey, 1f));

            _musicSlider.SetValueWithoutNotify(settings != null
                ? settings.MusicVolume
                : PlayerPrefs.GetFloat(MusicVolumeKey, 1f));

            _sfxSlider.SetValueWithoutNotify(settings != null
                ? settings.SFXVolume
                : PlayerPrefs.GetFloat(SFXVolumeKey, 1f));

            bool fullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
            _fullscreenToggle.SetIsOnWithoutNotify(fullscreen);

            RefreshResolutionOptions();
        }

        private void RefreshResolutionOptions()
        {
            _availableResolutions = new List<Resolution>();
            var seen = new HashSet<(int, int)>();

            foreach (var res in Screen.resolutions)
            {
                if (seen.Add((res.width, res.height)))
                {
                    _availableResolutions.Add(res);
                }
            }

            int savedWidth = PlayerPrefs.GetInt(ResolutionWidthKey, Screen.width);
            int savedHeight = PlayerPrefs.GetInt(ResolutionHeightKey, Screen.height);

            var options = new List<string>();
            int selectedIndex = 0;
            for (int i = 0; i < _availableResolutions.Count; i++)
            {
                var res = _availableResolutions[i];
                options.Add($"{res.width} x {res.height}");
                if (res.width == savedWidth && res.height == savedHeight)
                {
                    selectedIndex = i;
                }
            }

            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.SetValueWithoutNotify(selectedIndex);
            _resolutionDropdown.RefreshShownValue();

            // Unlike DisplaySettingsController, the resolution is NOT re-applied here.
            // Opening the pause settings should not change the window.
        }

        private void OnMasterVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetMasterVolume(value);
                return;
            }

            // SettingsManager only exists in MainMenuMap, so a game launched straight into a
            // gameplay scene has none. Fall back to the same PlayerPrefs key plus a direct
            // listener volume so the slider still does something.
            PlayerPrefs.SetFloat(MasterVolumeKey, value);
            AudioListener.volume = value;
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetMusicVolume(value);
                return;
            }

            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetSFXVolume(value);
                return;
            }

            PlayerPrefs.SetFloat(SFXVolumeKey, value);
        }

        private void OnFullscreenChanged(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        }

        private void OnResolutionChanged(int index)
        {
            if (_availableResolutions == null || index < 0 || index >= _availableResolutions.Count) return;

            var resolution = _availableResolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt(ResolutionWidthKey, resolution.width);
            PlayerPrefs.SetInt(ResolutionHeightKey, resolution.height);
        }
    }
}
