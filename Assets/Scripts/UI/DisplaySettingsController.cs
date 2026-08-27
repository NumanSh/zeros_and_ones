using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Attach to the Settings panel alongside SettingsPanelController. Wires a
    /// fullscreen toggle and a resolution dropdown, and persists the choices via PlayerPrefs.
    /// </summary>
    public class DisplaySettingsController : MonoBehaviour
    {
        private const string FullscreenKey = "Settings.Fullscreen";
        private const string ResolutionWidthKey = "Settings.ResolutionWidth";
        private const string ResolutionHeightKey = "Settings.ResolutionHeight";

        [Header("UI References")]
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private List<Resolution> availableResolutions;

        private void OnEnable()
        {
            InitializeResolutions();
            InitializeFullscreenToggle();
        }

        private void InitializeResolutions()
        {
            if (resolutionDropdown == null) return;

            availableResolutions = new List<Resolution>();
            var seen = new HashSet<(int, int)>();

            foreach (var res in Screen.resolutions)
            {
                var key = (res.width, res.height);
                if (seen.Add(key))
                {
                    availableResolutions.Add(res);
                }
            }

            int savedWidth = PlayerPrefs.GetInt(ResolutionWidthKey, Screen.currentResolution.width);
            int savedHeight = PlayerPrefs.GetInt(ResolutionHeightKey, Screen.currentResolution.height);

            var options = new List<string>();
            int selectedIndex = 0;
            for (int i = 0; i < availableResolutions.Count; i++)
            {
                var res = availableResolutions[i];
                options.Add($"{res.width} x {res.height}");
                if (res.width == savedWidth && res.height == savedHeight)
                {
                    selectedIndex = i;
                }
            }

            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.SetValueWithoutNotify(selectedIndex);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

            ApplyResolution(availableResolutions[selectedIndex], Screen.fullScreen);
        }

        private void InitializeFullscreenToggle()
        {
            if (fullscreenToggle == null) return;

            bool savedFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;

            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen);
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);

            Screen.fullScreen = savedFullscreen;
        }

        private void OnResolutionChanged(int index)
        {
            if (availableResolutions == null || index < 0 || index >= availableResolutions.Count) return;
            ApplyResolution(availableResolutions[index], Screen.fullScreen);
        }

        private void OnFullscreenChanged(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        }

        private void ApplyResolution(Resolution resolution, bool fullscreen)
        {
            Screen.SetResolution(resolution.width, resolution.height, fullscreen);
            PlayerPrefs.SetInt(ResolutionWidthKey, resolution.width);
            PlayerPrefs.SetInt(ResolutionHeightKey, resolution.height);
        }
    }
}
