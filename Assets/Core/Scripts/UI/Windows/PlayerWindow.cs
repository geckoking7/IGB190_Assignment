using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the display of player details, including health, resource values, abilities,
/// level, experience, gold, and shop data.
/// </summary>
public class PlayerWindow : UIWindow
{
    [SerializeField] private RectTransform abilityContainer;
    [SerializeField] private TextMeshProUGUI playerLevelText;  
    [SerializeField] private TextMeshProUGUI playerHealthText; 
    [SerializeField] private TextMeshProUGUI playerResourceText; 
    [SerializeField] private TextMeshProUGUI playerResourceNameText; 
    [SerializeField] private TextMeshProUGUI playerGoldText;   
    [SerializeField] private Image xpBar;                      
    [SerializeField] private Material healthGlobeMaterial;     
    [SerializeField] private Material resourceGlobeMaterial;   
    [SerializeField] private Image healthBar;                  
    [SerializeField] private Image resourceBar;
    [SerializeField] private BuffIconUI[] buffs;
    [SerializeField] private BuffIconUI[] debuffs;

    // Cache values for the UI so that they only update when they change.
    int displayedLevel = 1;
    int displayedGold = 0;

    /// <summary>
    /// Performs the initial setup of the player window.
    /// </summary>
    private void Start()
    {
        SetupAbilitySlots();
        InitializePlayerStats();
        GameManager.events.OnGoldRemoved.AddListener(x => ImmediatelyUpdateGoldDisplay());
    }

    public void RedrawChacterHUD ()
    {
        for (int i = abilityContainer.transform.childCount - 1; i > 0; i--)
        {
            Destroy(abilityContainer.transform.GetChild(i).gameObject);
        }
        GameObject template = abilityContainer.GetChild(0).gameObject;
        foreach (var ability in GameManager.player.abilities)
        {
            AbilitySlotUI slot = Instantiate(template, abilityContainer).GetComponent<AbilitySlotUI>();
            slot.Setup(ability, GameManager.settings.abilityKeybinds[GameManager.player.abilities.IndexOf(ability)].ToString());
            slot.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Initializes ability slots in the player window.
    /// </summary>
    private void SetupAbilitySlots()
    {
        GameObject template = abilityContainer.GetChild(0).gameObject;
        foreach (var ability in GameManager.player.abilities)
        {
            AbilitySlotUI slot = Instantiate(template, abilityContainer).GetComponent<AbilitySlotUI>();
            slot.Setup(ability, GameManager.settings.abilityKeybinds[GameManager.player.abilities.IndexOf(ability)].ToString());
            slot.gameObject.SetActive(true);
        }
        Update();
    }

    /// <summary>
    /// Initializes player stats and resource-related elements in the UI.
    /// </summary>
    private void InitializePlayerStats()
    {
        playerResourceNameText.text = GameManager.player.resourceName;
        playerGoldText.text = ((int)GameManager.player.currentGold).ToString();
        healthGlobeMaterial = new Material(healthGlobeMaterial);
        resourceGlobeMaterial = new Material(GameManager.player.resourceMaterial);
        healthBar.material = healthGlobeMaterial;
        resourceBar.material = resourceGlobeMaterial;
    }

    /// <summary>
    /// Updates the player HUD each frame, including health, resources, XP, and gold.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        UpdatePlayerStats();
        UpdateGoldDisplay();
        UpdateBuffDisplay();
    }

    private void UpdateBuffDisplay ()
    {
        if (GameManager.player == null) return;

        foreach (BuffIconUI buffUIItem in buffs)
            if (buffUIItem.gameObject.activeSelf) buffUIItem.gameObject.SetActive(false);
        foreach (BuffIconUI buffUIItem in debuffs)
            if (buffUIItem.gameObject.activeSelf) buffUIItem.gameObject.SetActive(false);

        int buffCount = 0;
        int debuffCount = 0;

        foreach (Buff buff in GameManager.player.buffs.buffs)
        {
            if (buff.buffVisibleInUI)
            {
                if (buff.buffType == Buff.BuffType.Buff)
                {
                    buffs[buffCount].Redraw(buff);
                    buffCount++;
                }
                else if (buff.buffType == Buff.BuffType.Debuff)
                {
                    debuffs[debuffCount].Redraw(buff);
                    debuffCount++;
                }
            }
        }
    }

    /// <summary>
    /// Updates player stats display in the HUD.
    /// </summary>
    private void UpdatePlayerStats()
    {
        if (displayedLevel != GameManager.player.currentLevel)
        {
            playerLevelText.SetText("{0}", GameManager.player.currentLevel);
            displayedLevel = GameManager.player.currentLevel;
        }
        if (playerHealthText.gameObject.activeInHierarchy)
        {
            playerHealthText.SetText("{0} / {1}",
                Mathf.RoundToInt(GameManager.player.health),
                Mathf.RoundToInt(GameManager.player.stats.GetValue(Stat.MaxHealth)));
        }
        if (playerResourceText.gameObject.activeInHierarchy)
        {
            playerResourceText.SetText("{0} / {1}",
                Mathf.RoundToInt(GameManager.player.resource),
                Mathf.RoundToInt(GameManager.player.stats.GetValue(Stat.MaxResource)));
        }
        xpBar.fillAmount = GameManager.player.currentExperience / GameManager.player.experienceToNextLevel;
        SetGlobePercentage(healthGlobeMaterial, GameManager.player.health / 
            GameManager.player.stats.GetValue(Stat.MaxHealth));
        SetGlobePercentage(resourceGlobeMaterial, GameManager.player.resource / 
            GameManager.player.stats.GetValue(Stat.MaxResource));
    }

    private void ImmediatelyUpdateGoldDisplay()
    {
        playerGoldText.text = $"{GameManager.player.currentGold}";
        displayedGold = (int)GameManager.player.currentGold;
    }

    /// <summary>
    /// Updates the displayed gold amount smoothly.
    /// </summary>
    private void UpdateGoldDisplay()
    {
        int currentDisplayedGold = int.Parse(playerGoldText.text);
        int goldDifference = Mathf.RoundToInt(GameManager.player.currentGold - currentDisplayedGold);
        goldDifference = Mathf.RoundToInt(Mathf.Min(goldDifference, 100 * Time.unscaledDeltaTime));

        int newDisplay = currentDisplayedGold + goldDifference;
        if (currentDisplayedGold > GameManager.player.currentGold)
            newDisplay = (int)GameManager.player.currentGold;

        if (displayedGold != newDisplay)
        {
            playerGoldText.SetText("{0}", newDisplay);
            displayedGold = newDisplay;
        }
    }

    /// <summary>
    /// Sets the fill percentage for the player stat globes (e.g., health and resource globes).
    /// </summary>
    private void SetGlobePercentage(Material globeMaterial, float percent)
    {
        globeMaterial.SetFloat("_Fill", percent);
    }
}