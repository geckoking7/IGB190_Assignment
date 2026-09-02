using UnityEngine;

public class MinimapWindow : MonoBehaviour
{
    public RectTransform container;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        container.gameObject.SetActive(!GameManager.ui.MapWindow.gameObject.activeInHierarchy);    
    }

    public void OnMenuButtonPressed ()
    {
        GameManager.ui.MainMenuWindow.Show();
    }

    public void OnSettingsButtonPressed ()
    {
        GameManager.ui.OptionsWindow.Show();
    }
}
