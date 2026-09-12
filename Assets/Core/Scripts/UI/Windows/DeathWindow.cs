using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathWindow : UIWindow, IPausing
{
    [SerializeField] private TextMeshProUGUI causeOfDeathText;
    [SerializeField] private TextMeshProUGUI respawnTimeRemaining;
    [SerializeField] private Button respawnButton;
    private float windowShownAt;

    public override void Setup()
    {
        base.Setup();
        GameManager.events.OnPlayerKilled.AddListener(OnPlayerKilled);
    }

    private void OnEnable()
    {
        windowShownAt = Time.unscaledTime;
    }

    protected override void Update()
    {
        base.Update();
        int secondsRemaining = Mathf.CeilToInt(GameManager.gameSettings.PlayerRespawnTime - (Time.unscaledTime - windowShownAt));
        if (secondsRemaining > 0)
        {
            respawnTimeRemaining.text = $"Waiting to Respawn ({secondsRemaining})";
            respawnButton.interactable = false;
        }
        else
        {
            respawnTimeRemaining.text = $"Respawn";
            respawnButton.interactable = true;
        }
    }

    /// <summary>
    /// Handles the player being killed by updating the death message.
    /// </summary>
    private void OnPlayerKilled(GameEvents.OnPlayerKilledInfo info)
    {
        // Update the cause of death text based on whether a unit caused the death
        string[] deathCauses = new []{ "eviscerated", "disemboweled", "destroyed", "brutally slain", "pwned", "taken apart", "annihilated", "obliterated" };
        var deathCause = deathCauses[Random.Range(0, deathCauses.Length - 1)];
        causeOfDeathText.text = info.killingUnit != null ? $"You were {deathCause} by a {info.killingUnit.unitName}" : $"You were {deathCause}";

        // Show the death window after a delay
        Invoke(nameof(Show), 2.0f);
    }

    /// <summary>
    /// Revives the player and hides the death window.
    /// </summary>
    public void RevivePlayer()
    {
        GameManager.player.Revive();
        Hide();
    }
}