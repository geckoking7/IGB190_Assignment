using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailRendererReset : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TrailRenderer>().Clear();
    }
}
