using UnityEngine;
using UnityEngine.UI;

public class MapWindow : MonoBehaviour
{
    public GameObject mapComponents;

    public Image background;
    public RawImage details;

    private void OnEnable()
    {
        //mapComponents.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //mapComponents.gameObject.SetActive(false);
    }

    public void SetBackgroundMaterial (Material material)
    {
        background.material = material;
    }

    public void SetDetailsTexture(RenderTexture renderTexture)
    {
        details.texture = renderTexture;
    }
}
