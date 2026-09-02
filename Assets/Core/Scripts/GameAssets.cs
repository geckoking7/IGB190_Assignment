using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameAssets class handles the storage and retrieval of key game assets.
/// </summary>
[System.Serializable]
public class GameAssets
{
    public GoldPickup goldPickup;
    public ItemPickup itemPickup;
    public HealthPickup healthPickup;
    public UnitUI unitUI;
    public StatusMessageUI statusMessageUI;
    

    public CircleEffectGuide circleEffectGuide;
    public LineEffectGuide lineEffectGuide;
    public ArcEffectGuide arcEffectGuide;

    public GameFeedback notificationReceivedFeedback;
    public GameFeedback questReceivedFeedback;
    public GameFeedback questCompletedFeedback;

    public Region squareRegion;
    public Region sphericalRegion;
    public GameObject playerMapMarker;
    public GameObject enemyMapMarker;
    public GameObject allyMapMarker;

    public GameObject empoweredEffect;
    public GameObject stunEffect;

    

    public GameObject fog;
    public Material dissolveMaterial;

    public LayerMask floorMask;
    public LayerMask wallMask;
    public LayerMask monsterMask;

    public AnimationCurve smoothInOutCurve;

    

}
