using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// Handles the display of tooltips for abilities and items.
/// </summary>
public class TooltipWindow : UIWindow
{
    [SerializeField] private RectTransform tooltipContainer;
    [SerializeField] private Image tooltipIcon;            
    [SerializeField] public TextMeshProUGUI tooltipTitle;   
    [SerializeField] private TextMeshProUGUI tooltipSubheading; 
    [SerializeField] private TextMeshProUGUI tooltipDescription;
    [SerializeField] private TextMeshProUGUI tooltipFlavourText;

    IVisualCodeHandler tooltipSource = null;

    protected override void Update()
    {
        base.Update();
        Resize();
        if (tooltipSource != null && tooltipSource is Buff buff)
        {
            if (!GameManager.player.buffs.HasBuff(buff))
            {
                Hide();
            }
        }
    }

    public string GetTitle ()
    {
        return tooltipTitle.text;
    }

    /// <summary>
    /// Shows the tooltip for a given ability.
    /// </summary>
    /// <param name="ability">The ability to display in the tooltip.</param>
    public void Show(Ability ability)
    {
        tooltipSource = ability;
        tooltipIcon.sprite = ability.abilityIcon;
        tooltipTitle.text = ability.name;
        tooltipTitle.color = new Color(0.8f, 0.6f, 0.5f); // Custom color for ability titles
        tooltipSubheading.text = $"<color=yellow>{ability.GetTotalCooldown(GameManager.player):N1}s Cooldown</color>";

        tooltipDescription.text = ability.GetTooltip(GameManager.player)
            .Replace("Resource", GameManager.player.resourceName);

        tooltipFlavourText.gameObject.SetActive(ability.abilityFlavourText.Length > 0);
        tooltipFlavourText.text = ability.abilityFlavourText;

        gameObject.SetActive(true);
        Resize();
    }

    /// <summary>
    /// Shows the tooltip for a given item.
    /// </summary>
    /// <param name="item">The item to display in the tooltip.</param>
    public void Show(Item item)
    {
        tooltipSource = item;
        tooltipIcon.sprite = item.itemIcon;
        tooltipTitle.text = item.name;
        tooltipTitle.color = item.GetItemColor();
        tooltipSubheading.text = item.GetTypeDescription();
        tooltipDescription.text = $"<color=yellow>{item.GetDescription().Replace("Resource", GameManager.player.resourceName)}</color>";

        tooltipFlavourText.gameObject.SetActive(item.itemFlavourText.Length > 0);
        tooltipFlavourText.text = item.itemFlavourText;

        gameObject.SetActive(true);
        Resize();
    }

    public void Show (Buff buff)
    {
        tooltipSource = buff;
        tooltipIcon.sprite = buff.buffIcon;
        tooltipTitle.text = buff.name;

        string subtitle = "";
        if (buff.buffHasDuration)
        {
            subtitle += "Timed ";
        }
        if (buff.buffType == Buff.BuffType.Buff)
        {
            subtitle += "Buff";
        }
        else
        {
            subtitle += "Debuff";
        }

        tooltipSubheading.text = $"<color=yellow>{subtitle}</color>";

        string desc = Regex.Replace(buff.buffDescription, @"\d+(?:\.\d+)?%?", "<color=yellow>$&</color>");
        tooltipDescription.text = $"{desc.Replace("Resource", GameManager.player.resourceName)}";




        tooltipFlavourText.gameObject.SetActive(buff.buffFlavourText.Length > 0);
        tooltipFlavourText.text = buff.buffFlavourText;

        gameObject.SetActive(true);
        Resize();
    }

    private void Resize ()
    {
        float offset = 150;
        offset += tooltipDescription.GetComponent<RectTransform>().sizeDelta.y;
        if (tooltipFlavourText.gameObject.activeSelf)
            offset += tooltipFlavourText.GetComponent<RectTransform>().sizeDelta.y + 30;

        Vector2 size = tooltipContainer.sizeDelta;
        size.y = offset;
        tooltipContainer.sizeDelta = size;
    }
}
