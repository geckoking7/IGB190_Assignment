using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class CustomInteractable : Interactable
{
    public string interactableLabel;
    public bool allowInteractions = true;
    [Space(10)]

    public UnityEvent OnInteractionStarted = new UnityEvent();
    public UnityEvent OnInteractionCompleted = new UnityEvent();

    [Header("Keep at 0 for Instant Interactions")]
    public float timeToCompleteInteraction = 0;
    public float interactionDistance = 2.5f;

    [HideInInspector] public bool interactionInProgress;
    private float interactionFinishedAt;

    [HideInInspector] public int uniqueID;
    private static int nextUniqueID;

    private void Start()
    {
        uniqueID = nextUniqueID++;
        SetOutline(Color.yellow, 2);
    }

    protected override void Update()
    {
        base.Update();
        if (interactionInProgress && Time.time > interactionFinishedAt)
        {
            InteractionCompleted();
        }
    }

    /// <summary>
    /// Handles the item pickup interaction, adding the item to the player's inventory and triggering feedback.
    /// </summary>
    public override void OnInteraction()
    {
        if (interactionInProgress) return;
        interactionInProgress = true;
        interactionFinishedAt = Time.time + timeToCompleteInteraction;

        GameManager.player.StopMoving();
        GameManager.player.canMoveAt = interactionFinishedAt;
        GameManager.player.canCastAt = interactionFinishedAt;
        GameManager.events.OnInteractionStarted.Invoke(this);
        OnInteractionStarted.Invoke();
    }

    public void ForceInteract ()
    {
        if (interactionInProgress) return;
        interactionInProgress = true;
        interactionFinishedAt = Time.time + timeToCompleteInteraction;
        GameManager.events.OnInteractionStarted.Invoke(this);
        OnInteractionStarted.Invoke();
    }

    public void ForceTriggerInteract ()
    {
        if (interactionInProgress || !allowInteractions) return;
        GameManager.selectedInteractable = this;
        GameManager.selectedInteractableAt = Time.time;
    }

    private void InteractionCompleted ()
    {
        GameManager.selectedInteractable = null;
        interactionInProgress = false;
        OnInteractionCompleted.Invoke();
        GameManager.events.OnInteractionFinished.Invoke(this);
        DeselectItem();
    }

    /// <summary>
    /// Marks the item as selected when the player hovers over it.
    /// </summary>
    public void SelectItem()
    {
        GameManager.hoveredInteractable = this;
        SetOutline(Color.yellow, allowInteractions ? 4 : 0);
    }

    /// <summary>
    /// Unmarks the item as selected when the player stops hovering over it.
    /// </summary>
    public void DeselectItem()
    {
        if (GameManager.hoveredInteractable == this)
        {
            GameManager.hoveredInteractable = null;
        }
        SetOutline(Color.yellow, allowInteractions ? 2 : 0);
        
    }

    protected virtual void OnMouseDown()
    {
        ForceTriggerInteract();
        //if (interactionInProgress || !allowInteractions) return;
        //GameManager.selectedInteractable = this;
        //GameManager.selectedInteractableAt = Time.time;


        //TryToInteract();
    } 

    protected void OnMouseEnter()
    {
        if (allowInteractions)
            SelectItem();
    }

    private void OnMouseExit()
    {
        DeselectItem();
    }

    public void EnableInteractions ()
    {
        allowInteractions = true;
        SetOutline(Color.yellow, 2);
    }

    public void DisableInteractions ()
    {
        allowInteractions = false;
        SetOutline(Color.yellow, 0);
    }

    public override float GetInteractionDistance()
    {
        return interactionDistance;
    }
}
