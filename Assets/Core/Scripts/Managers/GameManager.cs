using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Player player;
    public static Unit hoveredUnit;
    public static Monster hoveredMonster;
    public static Interactable selectedInteractable;
    public static float selectedInteractableAt;
    public static Interactable hoveredInteractable;
    public static string characterToSpawn = "";

    public const string ABILITY_RESOURCES_FOLDER = "Abilities";
    public const string ITEM_RESOURCES_FOLDER = "Items";
    public const string BUFF_RESOURCES_FOLDER = "Buffs";
    public const string SCRIPTS_RESOURCES_FOLDER = "General Scripts";
    public const string TEMPLATES_RESOURCES_FOLDER = "Templates";



    // Singleton reference to the game manager.
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GameManager>();
            return _instance;
        }
    }

    // Reference to the game assets.
    private VisualCodeManager _logicEngine;
    public static VisualCodeManager logicEngine
    {
        get
        {
            if (instance._logicEngine == null)
                instance._logicEngine = FindFirstObjectByType<VisualCodeManager>();
            return instance._logicEngine;
        }
    }

    // Reference to the game assets.
    [SerializeField] private GameAssets _assets;
    public static GameAssets assets
    {
        get
        {
            return instance._assets;
        }
    }

    // Reference to the game settings.
    [SerializeField] private Settings _settings;
    public static Settings settings
    {
        get {

            if (instance == null)
                return new Settings();
            else if (instance._settings == null)
                instance._settings = new Settings();
            return instance._settings;

            /*
            if (instance == null)
            {
                if (File.Exists("Settings.json"))
                {
                    return JsonUtility.FromJson<Settings>(File.ReadAllText("Settings.json"));
                }
                else
                {
                    return new Settings();
                }
            }
            if (instance._settings == null)
            {
                if (File.Exists("Settings.json"))
                {
                    instance._settings = JsonUtility.FromJson<Settings>(File.ReadAllText("Settings.json"));
                }
                else
                {
                    instance._settings = new Settings();
                }
            }
            if (instance._settings == null) Debug.Log("Settings were unable to be loaded correctly.");
            return instance._settings;
            */
        }
    }

    // Reference to the UI windows.
    private UIManager _ui;
    public static UIManager ui
    {
        get
        {
            if (instance._ui == null)
                instance._ui = new UIManager();
            return instance._ui;
        }
    }

    // Reference to the game events.
    private GameEvents _events;
    public static GameEvents events
    {
        get
        {
            if (instance._events == null)
                instance._events = new GameEvents();
            return instance._events;
        }
    }

    // Reference to the quest manager.
    private QuestManager _quests;
    public static QuestManager quests
    {
        get
        {
            if (instance._quests == null)
                instance._quests = new QuestManager();
            return instance._quests;
        }
    }

    // Reference to the music manager.
    private MusicManager _music;
    public static MusicManager music
    {
        get
        {
            if (instance._music == null)
                instance._music = FindFirstObjectByType<MusicManager>();
            return instance._music;
        }
    }

    // Reference to the monster spawn manager.
    private static MonsterSpawnManager _spawner;
    public static MonsterSpawnManager spawner
    {
        get
        {
            if (_spawner == null)
                _spawner = FindFirstObjectByType<MonsterSpawnManager>();
            return _spawner;
        }
    }

    // Reference to the item manager.
    private static ItemManager _items;
    public static ItemManager items
    {
        get
        {
            if (_items == null)
                _items = new ItemManager();
            return _items;
        }
    }

    private static GameSettings _gameSettings;
    public static GameSettings gameSettings
    {
        get
        {
            if (_gameSettings == null)
                _gameSettings = new GameSettings();
            return _gameSettings;
        }
    }

    [Header("Game Settings")]

    [SerializeField] private MonsterValues _monsterValues;
    public static MonsterValues monsterValues => instance._monsterValues;
    [System.Serializable] public class MonsterValues
    {
        [Tooltip("The minimum amount of gold a 'regular' monster will drop. Specific monsters can drop a modified amount (e.g., 200% of the base amount, 10% of the base amount).")]
        public float baseGoldDropAmountMinimum = 10;
        [Tooltip("The maximum amount of gold a 'regular' monster will drop. Specific monsters can drop a modified amount (e.g., 200% of the base amount, 10% of the base amount).")]
        public float baseGoldDropAmountMaximum = 20;
        [Tooltip("The chance that a monster will drop gold (e.g., 0.4 is a 40% chance for each monster to drop gold).")]
        [Range(0.0f, 1.0f)] public float goldDropChance = 0.4f;
        [Tooltip("The chance that a monster will drop a common item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float unempoweredMonsterCommonDropChance = 0.02f;
        [Tooltip("The chance that a monster will drop a rare item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float unempoweredMonsterRareDropChance = 0.005f;
        [Tooltip("The chance that a monster will drop a legendary item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float unempoweredMonsterLegendaryDropChance = 0;
    }

    [SerializeField] private EmpoweredMonsterValues _empoweredMonsterValues;
    public static EmpoweredMonsterValues empoweredMonsterValues => instance._empoweredMonsterValues;
    [System.Serializable] public class EmpoweredMonsterValues
    {
        [Tooltip("The health modifier for empowered units. For example, a value of 4 means 400% of regular health.")]
        public float empoweredMonsterHealthModifier = 4.0f;
        [Tooltip("The damage modifier for empowered units. For example, a value of 1.5 means 150% of regular damage.")]
        public float empoweredMonsterDamageModifier = 1.5f;
        [Tooltip("The attack speed modifier for empowered units. For example, a value of 1.5 means 150% of regular attack speed.")]
        public float empoweredMonsterAttackSpeedModifier = 1.5f;
        [Tooltip("The experience modifier for empowered units. For example, a value of 5 means 500% of regular experience.")]
        public float empoweredMonsterXPModifier = 5.0f;
        [Tooltip("The gold drop modifier for empowered units. For example, a value of 3 means 300% of regular gold.")]
        public float empoweredMonsterGoldModifier = 3.0f;
        [Tooltip("The chance that an empowered monster will drop a common item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float empoweredMonsterCommonDropChance = 1f;
        [Tooltip("The chance that an empowered monster will drop a rare item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float empoweredMonsterRareDropChance = 0.2f;
        [Tooltip("The chance that an empowered monster will drop a legendary item (e.g., 0.02 = 2%).")]
        [Range(0.0f, 1.0f)] public float empoweredMonsterLegendaryDropChance = 0;
    }

    [SerializeField] private PlayerExperienceValues _playerExperienceValues;
    public static PlayerExperienceValues playerExperienceValues => instance._playerExperienceValues;
    [System.Serializable] public class PlayerExperienceValues
    {
        [Tooltip("The health modifier for empowered units. For example, a value of 4 means 400% of regular health.")]
        public float baseMonsterXP = 10;
        [Tooltip("The experience required for the player to complete their FIRST level up.")]
        public float startingXPPerLevel = 100;
        [Tooltip("The additional experience required for the player to complete each subsequent level up.")]
        public float additionalMaxXPPerLevel = 100;
    }

    [SerializeField] private MonsterScalingValues _monsterScalingValues;
    public static MonsterScalingValues monsterScalingValues => instance._monsterScalingValues;
    [System.Serializable] public class MonsterScalingValues
    {
        [Tooltip("The increased health monsters gain as the player levels up. 0.2 = 20% increased health per player level.")]
        [Range(0.0f, 1.0f)] public float increasedHealthPerPlayerLevel = 0.2f;
        [Tooltip("The increased damage monsters gain as the player levels up. 0.2 = 20% increased damage per player level.")]
        [Range(0.0f, 1.0f)] public float increasedDamagePerPlayerLevel = 0.2f;
    }

    [SerializeField] private HealthGlobeValues _healthGlobeValues;
    public static HealthGlobeValues healthGlobeValues => instance._healthGlobeValues;
    [System.Serializable] public class HealthGlobeValues
    {
        [Tooltip("The base chance that a monster will drop a health globe. For example, 0.5 = 50% chance for a health pickup.")]
        [Range(0.0f, 1.0f)] public float baseHealthGlobeChance = 0.5f;
        [Tooltip("The reduced chance that a monster will drop a health globe for each one that already exists. For example, 0.15 = 15%, so two health globes would reduced the chance by 30% in total.")]
        [Range(0.0f, 1.0f)] public float reducedChancePerExistingGlobe = 0.15f;
        [Tooltip("Determines how closely the health pickups can spawn to the dead enemy (e.g., 3 units means somewhere randomly within 3 units).")]
        public float spawnRadius = 3;
        [Tooltip("Determines how long the health pickup will last before it expires. A value of 5 would mean 5 seconds before it is destroyed.")]
        public float lifetime = 5;
        [Tooltip("Determines how much health the health globe will restore by default.")]
        public float healthGlobeHealthRestore = 100;
    }

    [SerializeField] private InventoryValues _inventoryValues;
    public static InventoryValues inventoryValues => instance._inventoryValues;
    [System.Serializable] public class InventoryValues
    {
        [Tooltip("Determines how much gold is returned when an item is sold. A value of 0.5 means 50% of purchase price.")]
        public float sellItemReturnRate = 0.5f;
    }

    [SerializeField] private ArmorValues _armorValues;
    public static ArmorValues armorValues => instance._armorValues;
    [System.Serializable]
    public class ArmorValues
    {
        public float maxArmor = 1000;
        public AnimationCurve armorDamageReductionCurve;
    }

    /// <summary>
    /// When the GameManager is destroyed, save the settings to a file for future use.
    /// </summary>
    private void OnDestroy()
    {
        if (Application.isEditor) return;
        File.WriteAllText("Settings.json", JsonUtility.ToJson(settings));
    }

    public void OnEnable()
    {
        if (CharacterSelectManager.selectedCharacter != "")
        {
            Player[] players = GameObject.FindObjectsByType<Player>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Player player in players)
            {
                player.gameObject.SetActive(player.unitName == CharacterSelectManager.selectedCharacter);
            }
        }
        player = GameObject.FindFirstObjectByType<Player>();
        if (player == null) 
            Debug.Log("No player character has been added. If you loaded from the character select screen, " +
                "make sure that the character name on the select screen matches the name of the character in the game scene.");
        ui.Setup();
    }


    private void Start()
    {
        Ability[] abilities = Resources.LoadAll<Ability>(ABILITY_RESOURCES_FOLDER);
        foreach (Ability ability in abilities)
        {
            ability.engine.Setup();
        }
    }

    private void Update()
    {
        HandleUserInput();
        hoveredMonster = GetHoveredMonster();
    }

    private Monster GetHoveredMonster ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, assets.monsterMask)) 
            return hit.collider.GetComponent<Monster>();
        return null;
    }

    public void WinGame ()
    {
        GameManager.events.OnGameWon.Invoke();
        StartCoroutine(GoToEpilogue());
    }

    private IEnumerator GoToEpilogue()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        op.allowSceneActivation = false;
        yield return new WaitForSeconds(7.0f);
        op.allowSceneActivation = true;
    }

    private void HandleUserInput ()
    {
        if (Input.GetKeyDown(settings.inventoryWindowKeybind) || Input.GetKeyDown(settings.characterWindowKeybind))
        {
            ui.CharacterWindow.Show();
            if (gameSettings.CanAccessShop)
                ui.ShopWindow.Show();
            else
                ui.ShopWindow.Hide();
        }
    }

    public static void SendEventMessage (string message)
    {
        events.OnEventMessageReceived.Invoke(new GameEvents.EventMessageInfo(message));
    }

    /// <summary>
    /// Sets the checkpoint for the player.
    /// </summary>
    public static void SetCheckpoint (Vector3 newCheckpoint)
    {
        gameSettings.checkpointSet = true;
        gameSettings.playerCheckpoint = newCheckpoint;
    }
}
