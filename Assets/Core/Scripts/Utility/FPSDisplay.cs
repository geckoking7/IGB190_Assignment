using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;
    private float fps;
    void Update()
    {
        // Calculate delta time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Only update the FPS text once per second
        if (Time.time % 2.0f <= Time.unscaledDeltaTime)
        {
            float msec = deltaTime * 1000.0f;
            fps = 1f / deltaTime;
            string text = string.Format("FPS: {1:0.}", msec, fps);
            fpsText.text = text;
        }
#if UNITY_EDITOR
        fpsText.text = $"FPS: {(int)(fps)}\n" +
            $"Batches: {UnityStats.batches}\n" +
            $"Draw Calls: {UnityStats.drawCalls}\n" +
            $"Tris: {(int)(UnityStats.triangles / 1000)}k\n" +
            $"Verts: {(int)(UnityStats.vertices / 1000)}k";
        //$"Used VRAM: {UnityStats.v / 1024f:F1} MB";
#endif
    }
}