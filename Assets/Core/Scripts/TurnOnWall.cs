using System.Collections;
using UnityEngine;

public class TurnOnWall : MonoBehaviour
{
    public GameObject wall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        wall.SetActive(true);
    }
}
