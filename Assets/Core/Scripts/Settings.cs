using UnityEngine;
/// <summary>
/// A basic class storing all of the settings in the game. This class should only
/// contain basic types (e.g. int, float, bool, string) or types which can be serialised.
/// 
/// The game settings will be serialised into a JSON file when the game is closed, and
/// loaded from the JSON file when it is opened.
/// </summary>
[System.Serializable]
public class Settings
{
    [Header("Default Quality Settings")]
    public int graphicsQuality = 5;

    [Header("Sound Settings")]
    public float masterVolume = 0.5f;
    public float musicVolume = 0.2f;
    public float effectsVolume = 0.5f;

    [Header("User Interface")]
    public bool showTutorialMessages = true;
    public bool showDamageNumbers = true;
    public bool showFullHealthBars = true;
    public bool showGoldPickupUI = true;
    public bool showTopHealthBarUI = true;
    public bool showGameUI = true;
    //public bool showShopWhenCharacterWindowOpens = true;
    public bool showUnitOutlines = false;
    public bool hideAllOutlines = false;
    public bool useWSADMovement = true;

    [Header("Keybinds")]
    public KeyCode forceMoveKeybind = KeyCode.LeftControl;
    public KeyCode forceHoldKeybind = KeyCode.LeftShift;
    public KeyCode characterWindowKeybind = KeyCode.C;
    public KeyCode inventoryWindowKeybind = KeyCode.I;
    public KeyCode interactKeybind = KeyCode.Space;
    public KeyCode toggleMapKeybind = KeyCode.M;
    public KeyCode holdMapKeybind = KeyCode.Tab;
    public KeyCode moveUpKeybind = KeyCode.W;
    public KeyCode moveDownKeybind = KeyCode.S;
    public KeyCode moveLeftKeybind = KeyCode.A;
    public KeyCode moveRightKeybind = KeyCode.D;
    public KeyCode[] abilityKeybinds = new KeyCode[6]
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
    };

    public void VerifySettings ()
    {
        if (GameManager.settings == null)
        {
            Debug.Log("Error loading settings.");
        }
    }
}