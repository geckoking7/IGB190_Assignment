using UnityEngine;

public class GameSettings
{
    public bool ItemsCanDrop { get; set; } = true;
    public bool HealthPickupsCanDrop { get; set; } = true;
    public bool GoldPickupsCanDrop { get; set; } = true;
    public float PlayerRespawnTime { get; set; } = 5;

    public bool CanAccessShop { get; set; } = true;
    public Vector3 playerCheckpoint;
    public bool checkpointSet = false;
}