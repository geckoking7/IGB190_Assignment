using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.ui.SimpleTooltipWindow.Show(title, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.ui.SimpleTooltipWindow.Hide();
    }
}
