using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameManager.player.transform.position + Vector3.up * 10.0f;
    }
}
