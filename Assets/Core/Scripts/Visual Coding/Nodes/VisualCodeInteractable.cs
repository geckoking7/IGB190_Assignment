using UnityEngine;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Interactable with Label",
        dynamicDescription = "Interactable with Label $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Label")]
    public CustomInteractable InteractableWithLabel(string label)
    {
        CustomInteractable[] interactables = GameObject.FindObjectsByType<CustomInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            if (interactable.interactableLabel == label)
            {
                return interactable;
            }
        }
        return null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactable with ID", 
        dynamicDescription = "Interactable with ID $")]
    [NumberArg(argType = ArgType.Temp, tempLabel = "ID")]
    public CustomInteractable InteractableWithID(float id)
    {
        CustomInteractable[] interactables = GameObject.FindObjectsByType<CustomInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            if (interactable.uniqueID == id)
            {
                return interactable;
            }
        }
        return null;
    } 
}