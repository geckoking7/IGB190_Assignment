using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays all relevant current stats to the player.
/// </summary>
public class StatsWindow : UIWindow
{
    [SerializeField] private RectTransform attributesContainer;  // Container for stat UI elements
    private bool isSetup = false;  // Tracks whether the setup has already been performed

    [SerializeField] private GameObject closeButton;

    private HashSet<Stat> statsAdded = new HashSet<Stat>();

    /// <summary>
    /// Performs initial setup, adding relevant stats to the window when enabled.
    /// </summary>
    private void OnEnable()
    {
        if (isSetup) return;
        isSetup = true;

        // Add relevant stats to the window with appropriate formatting
        AddAllStats();
    }

    protected override void Update()
    {
        base.Update();
        closeButton.SetActive(!GameManager.ui.ShopWindow.isActiveAndEnabled);
    }

    /// <summary>
    /// Adds all the relevant stats to the window with their respective formats.
    /// </summary>
    private void AddAllStats()
    {
        AddStat(Stat.Damage, "N0");
        AddStat(Stat.AttacksPerSecond, "N1");
        AddStat(Stat.Armor, "N0");
        AddStat(Stat.MaxHealth, "N0");
        AddStat(Stat.MaxResource, "N0");
        AddStat(Stat.MovementSpeed, "P0");
        AddStat(Stat.ResourceCostReduction, "P0", -1.0f);
        AddStat(Stat.ResourceGeneration, "P0");
        AddStat(Stat.CooldownReduction, "P0");
        AddStat(Stat.CriticalStrikeChance, "P0");
        AddStat(Stat.CriticalStrikeDamage, "P0");

        // This hides the stat from being shown.
        statsAdded.Add(Stat.DamageTaken);

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (!statsAdded.Contains(stat))
            {
                AddStat(stat, "N0");
            }
        } 
    }

    /// <summary>
    /// Adds a specific stat to the window, displaying it in the specified format.
    /// </summary>
    private void AddStat(Stat stat, string format, float adjustment = 0)
    {
        if (statsAdded.Contains(stat))
        {
            Debug.LogError("Cannot add the same stat to the stat window multiple times.");
            return;
        }

        statsAdded.Add(stat);
        GameObject template = attributesContainer.GetChild(0).gameObject;
        GameObject statItem = Instantiate(template, attributesContainer);
        statItem.SetActive(true);
        statItem.GetComponent<AttributesItem>().Setup(stat, format, adjustment);
    }
}