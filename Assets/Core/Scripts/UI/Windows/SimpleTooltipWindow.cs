using TMPro;
using UnityEngine;


public class SimpleTooltipWindow : UIWindow
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void Show (string title, string description)
    {
        base.Show();
        this.title.text = title;
        this.description.text = description;
    }
}
