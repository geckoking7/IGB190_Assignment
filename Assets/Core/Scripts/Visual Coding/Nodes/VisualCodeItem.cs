using MyUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using Random = UnityEngine.Random;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
         dropdownDescription = "Random Item of Rarity",
         dynamicDescription = "Random $ Item")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Rarities, allowFunction = false, allowPreset = false)]
    public Item RandomItemOfRarity(string rarity)
    {
        return Item.GetRandomItemOfRarity((Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), rarity));
    }

    public Item ThisItem()
    {
        return (Item)LogicEngine.current.engineHandler;
    }

    [VisualScriptingFunction(
         dropdownDescription = "Player Equipment in Slot",
         dynamicDescription = "Player Equipment in $ Slot")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.ItemSlots, allowFunction = false, allowPreset = false)]
    public Item PlayerEquipmentInSlot (string slot)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        switch (slot)
        {
            case PresetStrings.Weapon:
                return GameManager.player.equipment.GetItemAtID(0);
            case PresetStrings.Amulet:
                return GameManager.player.equipment.GetItemAtID(1);
            case PresetStrings.Armor:
                return GameManager.player.equipment.GetItemAtID(2);
            case PresetStrings.Boots:
                return GameManager.player.equipment.GetItemAtID(3);
            case PresetStrings.Ring1:
                return GameManager.player.equipment.GetItemAtID(4);
            case PresetStrings.Ring2:
                return GameManager.player.equipment.GetItemAtID(5);
        }
        return null;
    }
}
