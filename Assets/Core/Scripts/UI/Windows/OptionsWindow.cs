using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class OptionsWindow : UIWindow, IPausing, ICloseable
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown qualitySettingsDropdown;

    [Header("Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;

    [Header("Toggles")]
    [SerializeField] private Toggle showTutorialMessagesToggle;
    [SerializeField] private Toggle showDamageNumbersToggle;
    [SerializeField] private Toggle showFullHealthBarsToggle;
    [SerializeField] private Toggle showGoldPickupUIToggle;
    [SerializeField] private Toggle showTopHealthBarUIToggle;
    [SerializeField] private Toggle showGameUIToggle;
    [SerializeField] private Toggle showPerformanceMetricsToggle;

    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown monitorDropdown;

    [SerializeField] private GameObject hideableGameUI;

    public override void Setup()
    {
        base.Setup();
        InitializeDropdowns();
        InitializeSliders();
        InitializeToggles();

    }

    public void ResetKeybinds ()
    {
        GameManager.settings.abilityKeybinds = new KeyCode[8]
        {
            KeyCode.Mouse0,
            KeyCode.Mouse1,
            KeyCode.Q,
            KeyCode.E,
            KeyCode.R,
            KeyCode.T,
            KeyCode.Space,
            KeyCode.LeftShift
        };
        KeybindItem[] items = GameObject.FindObjectsByType<KeybindItem>(FindObjectsSortMode.None);
        foreach (KeybindItem item in items)
            item.UpdateKeybindDisplay();
        AbilitySlotUI[] slots = GameObject.FindObjectsByType<AbilitySlotUI>(FindObjectsSortMode.None);
        foreach (AbilitySlotUI slot in slots)
        {
            slot.RedrawHotkey();
        }

    }

    /// <summary>
    /// Initializes the graphics dropdowns.
    /// </summary>
    private void InitializeDropdowns()
    {
        // Initialise Quality Settings Dropdown
        qualitySettingsDropdown.ClearOptions();
        qualitySettingsDropdown.AddOptions(QualitySettings.names.ToList());

        int qualityIndex = Mathf.Min(QualitySettings.names.Length - 1, GameManager.settings.graphicsQuality);
        qualitySettingsDropdown.value = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);

        qualitySettingsDropdown.onValueChanged.AddListener(value =>
        {
            GameManager.settings.graphicsQuality = value;
            QualitySettings.SetQualityLevel(value);
        });

        displayModeDropdown.onValueChanged.AddListener(x => UpdateScreen());

        // Resolution Dropdown
        List<string> options = new List<string>();
        HashSet<string> seen = new HashSet<string>();
        foreach (Resolution resolution in Screen.resolutions)
        {
            string option = $"{resolution.width} x {resolution.height}";
            if (seen.Add(option))
            {
                options.Insert(0, option);
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(x => UpdateScreen());

        // Display Dropdown
        List<DisplayInfo> d = new List<DisplayInfo>();
        Screen.GetDisplayLayout(d);
      
        List<string> displays = new List<string>();
        for (int i = 0; i < d.Count; i++)
        {
            displays.Add($"Display {i+1}");
        }
        monitorDropdown.ClearOptions();
        monitorDropdown.AddOptions(displays);
        monitorDropdown.onValueChanged.AddListener(x => UpdateScreen());
    }

    /// <summary>
    /// Initializes the volume sliders.
    /// </summary>
    private void InitializeSliders()
    {
        masterVolumeSlider.value = GameManager.settings.masterVolume;
        AudioListener.volume = GameManager.settings.masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(value =>
        {
            AudioListener.volume = value;
            GameManager.settings.masterVolume = value;
        });

        musicVolumeSlider.value = GameManager.settings.musicVolume;
        GameManager.music.SetVolume(GameManager.settings.musicVolume);
        musicVolumeSlider.onValueChanged.AddListener(value =>
        {
            GameManager.music.SetVolume(value);
            GameManager.settings.musicVolume = value;
        });

        effectVolumeSlider.value = GameManager.settings.effectsVolume;
        effectVolumeSlider.onValueChanged.AddListener(value =>
        {
            GameManager.settings.effectsVolume = value;
        });
    }

    private void UpdateScreen ()
    {
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        Screen.MoveMainWindowTo(displays[monitorDropdown.value], Vector2Int.zero);

        string resolutionText = resolutionDropdown.options[resolutionDropdown.value].text;
        int width = int.Parse(resolutionText.Split('x')[0].Trim());
        int height = int.Parse(resolutionText.Split('x')[1].Trim());
        Screen.SetResolution(width, height, (FullScreenMode)displayModeDropdown.value);
    }

    /// <summary>
    /// Initializes the UI toggles.
    /// </summary>
    private void InitializeToggles()
    {
        SetupToggle(showTutorialMessagesToggle, GameManager.settings.showTutorialMessages, value =>
        {
            GameManager.settings.showTutorialMessages = value;
        });

        SetupToggle(showDamageNumbersToggle, GameManager.settings.showDamageNumbers, value =>
        {
            GameManager.settings.showDamageNumbers = value;
        });

        SetupToggle(showFullHealthBarsToggle, GameManager.settings.showFullHealthBars, value =>
        {
            GameManager.settings.showFullHealthBars = value;
        });

        SetupToggle(showGoldPickupUIToggle, GameManager.settings.showGoldPickupUI, value =>
        {
            GameManager.settings.showGoldPickupUI = value;
        });

        SetupToggle(showTopHealthBarUIToggle, GameManager.settings.showTopHealthBarUI, value =>
        {
            GameManager.settings.showTopHealthBarUI = value;
        });

        SetupToggle(showGameUIToggle, true, value =>
        {
            hideableGameUI.SetActive(value);
        });

        SetupToggle(showPerformanceMetricsToggle, false, value =>
        {
            GameObject go = GameObject.FindFirstObjectByType<FPSDisplay>(FindObjectsInactive.Include).gameObject;
            go.SetActive(value);
        });

        
    }

    /// <summary>
    /// Sets up a toggle with its initial value and listener.
    /// </summary>
    private void SetupToggle(Toggle toggle, bool initialValue, UnityEngine.Events.UnityAction<bool> onValueChanged)
    {
        toggle.isOn = initialValue;
        toggle.onValueChanged.AddListener(onValueChanged);
    }

    

    public void Confirm ()
    {
        Hide();
    }

    public void Cancel ()
    {
        Hide();
    }
}