using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Buff buff;
    public Image icon;
    public TextMeshProUGUI stacks;
    public Image fill;

    public void Redraw (Buff newBuff)
    {
        this.buff = newBuff;
        gameObject.SetActive(true);
        icon.sprite = buff.buffIcon;

        if (buff.buffHasDuration)
        {
            fill.fillAmount = 1.0f - buff.buffCurrentDuration / buff.buffMaxDuration;
        }
        else
            fill.fillAmount = 0;
        
        if (buff.buffMaximumStacks > 1 && buff.buffCurrentStacks > 1)
        {
            stacks.text = buff.buffCurrentStacks.ToString();
        }
        else
        {
            stacks.text = "";
        }
    }

    /// <summary>
    /// Shows the tooltip for the ability when the pointer enters the slot.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buff != null)
        {
            GameManager.ui.TooltipWindow.Show(buff);
        }
    }

    /// <summary>
    /// Hides the tooltip when the pointer exits the slot.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.ui.TooltipWindow.Hide();
    }
}
