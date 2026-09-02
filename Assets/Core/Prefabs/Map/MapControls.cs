using UnityEngine;
using UnityEngine.UI;

public class MapControls : MonoBehaviour
{
    private RenderTexture mapRenderTextureNavMesh;
    private RenderTexture mapRenderTextureDetails;

    [SerializeField] private Camera navMeshCamera;
    [SerializeField] private Camera detailsCamera;
    [SerializeField] private Material mapMaterial;
    [SerializeField] private GameObject mapComponents;

    [SerializeField] private Material minimapMaterial;
    [SerializeField] private RawImage minimapTexture;
    [SerializeField] private Image minimapBackground;

    // Anything above this will not be rendered.
    private float ADDED_HEIGHT = 10;

    private void Start()
    {
        mapRenderTextureNavMesh = new RenderTexture(Screen.width, Screen.height, 24);
        mapRenderTextureDetails = new RenderTexture(Screen.width, Screen.height, 24);
        navMeshCamera.targetTexture = mapRenderTextureNavMesh;
        detailsCamera.targetTexture = mapRenderTextureDetails;
        GameManager.ui.MapWindow.SetDetailsTexture(mapRenderTextureDetails);
        mapMaterial = new Material(mapMaterial);
        mapMaterial.SetTexture("_MaskTex", mapRenderTextureNavMesh);
        minimapBackground.material = mapMaterial;
        minimapTexture.texture = mapRenderTextureDetails;
        GameManager.ui.MapWindow.SetBackgroundMaterial(mapMaterial);
        mapComponents.SetActive(true);

    }

    private void Update()
    {
        navMeshCamera.transform.position = GameManager.player.transform.position + Vector3.up * ADDED_HEIGHT;
        detailsCamera.transform.position = GameManager.player.transform.position + Vector3.up * ADDED_HEIGHT;
        GameManager.ui.MapWindow.gameObject.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
