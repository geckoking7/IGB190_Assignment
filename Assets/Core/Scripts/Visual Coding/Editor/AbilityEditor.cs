using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using Codice.CM.SEIDInfo;
using System;
using System.Runtime.Remoting.Messaging;

public class AbilityEditor : BaseEditor
{
    protected override string ListItemFolder => VisualCodeLabels.Folders.ABILITIES;

    protected override IVisualCodeHandler[] ListData => Resources.LoadAll<Ability>(ListItemFolder);

    protected Ability ability => selectedItem == null ? null : (Ability)selectedItem.GetData();

    protected override Type ManagedType => typeof(Ability);

    protected override string GetListHeaderText => "Abilities";


    protected override void DrawItemInspector(Rect panel, IVisualCodeHandler item)
    {
        base.DrawItemInspector(panel, item);

        GUIStyle richTextBold = new GUIStyle(EditorStyles.boldLabel);
        richTextBold.richText = true;

        float posX = panel.x;
        float posY = panel.y;
        float width = panel.width;

        // Draw the title
        EditorGUI.DrawRect(new Rect(posX, posY, width, headerHeight), headerColor);
        EditorGUI.LabelField(new Rect(posX + 8, posY, width, headerHeight),
            "[Settings - " + ability.name + "]", LogicEngineEditor.windowStyle_HeaderText);
        posY += headerHeight;


        posX += 10;
        posY += 10;
        width -= 20;

        // Ability Icon Option
        Rect iconRect = new Rect(posX, posY, iconSize, iconSize);
        ability.abilityIcon = (Sprite)EditorGUI.ObjectField(iconRect, ability.abilityIcon, typeof(Sprite), false);

        // Ability Type Option
        GUI.Label(new Rect(posX + 80, posY - 5, 100, labelHeight), VisualCodeLabels.Editor.ABILITY_TARGETS_CONTENT, richTextBold);
        ability.targetMode = (Ability.TargetMode)EditorGUI.EnumPopup(new Rect(posX + 80, posY + 14, 130, 22), ability.targetMode);

        // Cast Animation Option
        GUI.Label(new Rect(posX + 80, posY + 34, 100, labelHeight), VisualCodeLabels.Editor.ABILITY_ANIMATION_CONTENT, richTextBold);
        int id = 0;
        for (int i = 0; i < Unit.animations.Length; i++)
            if (Unit.animations[i] == ability.abilityAnimation)
                id = i;
        ability.abilityAnimation = Unit.animations[EditorGUI.Popup(new Rect(posX + 80, posY + 52, 130, toggleHeight), id, Unit.animations)];
        posY += 74;

        // Ability Description Input
        GUI.Label(new Rect(posX, posY, width, labelHeight), VisualCodeLabels.Editor.ABILITY_DESCRIPTION_CONTENT, EditorStyles.boldLabel);
        posY += labelHeight;
        ability.abilityDescription = GUI.TextArea(new Rect(posX, posY, width, 70), ability.abilityDescription);
        posY += 73;

        // Ability Flavour Text Input
        GUI.Label(new Rect(posX, posY, width, labelHeight), VisualCodeLabels.Editor.ABILITY_FLAVOUR_CONTENT, EditorStyles.boldLabel);
        posY += 20;
        ability.abilityFlavourText = GUI.TextArea(new Rect(posX, posY, width, 40), ability.abilityFlavourText);

        posY += 44;

        // Tag Input
        GUI.Label(new Rect(posX, posY, width, labelHeight), VisualCodeLabels.Editor.ABILITY_TAG_CONTENT, EditorStyles.boldLabel);
        ability.abilityTag = GUI.TextField(new Rect(posX + 30, posY, width - 30, toggleHeight), ability.abilityTag);
        posY += toggleHeight + boxPadding;

        // Toggle box with a number of options.
        EditorGUI.DrawRect(new Rect(posX, posY, width, 130), boxColor);
        posY += boxPadding;
        ability.canMoveWhileCasting = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.canMoveWhileCasting, VisualCodeLabels.Editor.ABILITY_CAST_WHILE_MOVING_CONTENT);
        posY += labelHeight;
        ability.hasSpecificCastTime = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.hasSpecificCastTime, VisualCodeLabels.Editor.ABILITY_HAS_SPECIFIC_CAST_TIME_CONTENT);
        posY += labelHeight;
        ability.requiresLineOfSight = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.requiresLineOfSight, VisualCodeLabels.Editor.ABILITY_REQUIRES_LINE_OF_SIGHT_CONTENT);
        posY += labelHeight;
        ability.canUpdateTargetWhileCasting = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.canUpdateTargetWhileCasting, VisualCodeLabels.Editor.ABILITY_UPDATE_TARGET_WHILE_CASTING_CONTENT);
        posY += labelHeight;
        ability.cooldownIsAtackSpeed = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.cooldownIsAtackSpeed, VisualCodeLabels.Editor.ABILITY_COOLDOWN_IS_ATTACK_SPEED_CONTENT);
        posY += labelHeight;
        ability.abilityGeneratesResource = GUI.Toggle(new Rect(posX + boxPadding, posY, width, toggleHeight), ability.abilityGeneratesResource, VisualCodeLabels.Editor.ABILITY_GENERATES_RESOURCES_CONTENT);
        posY += labelHeight;
        posY += boxPadding;
        posY += boxPadding;
        
        // Draw a box for the subsequent inputs.
        EditorGUI.DrawRect(new Rect(posX, posY, width, 95), boxColor);
        posY += smallPadding;

        // Range Input
        GUI.Label(new Rect(posX + boxPadding, posY, 100, labelHeight), VisualCodeLabels.Editor.ABILITY_RANGE_CONTENT, EditorStyles.boldLabel);
        if (ability.targetMode == Ability.TargetMode.PointInMelee || ability.targetMode == Ability.TargetMode.UnitInMelee)
        {
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(posX + boxPadding, posY + labelHeight, 100, labelHeight), "Melee");
            GUI.enabled = true;
        }
        else if (ability.targetMode == Ability.TargetMode.None)
        {
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(posX + boxPadding, posY + labelHeight, 100, labelHeight), "N/A");
            GUI.enabled = true;
        }
        else
        {
            ability.range = EditorGUI.FloatField(new Rect(posX + boxPadding, posY + labelHeight, 100, labelHeight), ability.range);
        }

        // Resources Input
        if (ability.abilityGeneratesResource)
            GUI.Label(new Rect(posX + 110, posY, 95, labelHeight), VisualCodeLabels.Editor.ABILITY_RESOURCE_GAIN_CONTENT, EditorStyles.boldLabel);
        else
            GUI.Label(new Rect(posX + 110, posY, 95, labelHeight), VisualCodeLabels.Editor.ABILITY_RESOURCE_COST_CONTENT, EditorStyles.boldLabel);
        ability.abilityCost = EditorGUI.FloatField(new Rect(posX + 110, posY + labelHeight, 95, labelHeight), ability.abilityCost);
        ability.abilityCost = Mathf.Max(0, ability.abilityCost);

        // Go to second row.
        posY += labelHeight + toggleHeight;

        // Cast Time Input
        GUI.Label(new Rect(posX + boxPadding, posY, 100, labelHeight), VisualCodeLabels.Editor.ABILITY_CAST_TIME_CONTENT, EditorStyles.boldLabel);
        if (ability.hasSpecificCastTime)
        {
            ability.castTime = EditorGUI.FloatField(new Rect(posX + boxPadding, posY + labelHeight, 100, labelHeight), ability.castTime);
        }
        else
        {
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(posX + boxPadding, posY + labelHeight, 100, labelHeight), "Auto");
            GUI.enabled = true;
        }

        // Cooldown Input
        GUI.Label(new Rect(posX + 110, posY, 95, labelHeight), VisualCodeLabels.Editor.ABILITY_COOLDOWN_CONTENT, EditorStyles.boldLabel);
        if (!ability.cooldownIsAtackSpeed)
        {
            ability.abilityCooldown = EditorGUI.FloatField(new Rect(posX + 110, posY + labelHeight, 95, labelHeight), ability.abilityCooldown);
        }
        else
        {
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(posX + 110, posY + labelHeight, 95, labelHeight), "Attack Speed");
            GUI.enabled = true;
        }

        // Finish the box and add some further padding.
        posY += labelHeight + toggleHeight + smallPadding + 2 * boxPadding;

        // Ability Sound Effect Input
        GUI.Label(new Rect(posX, posY, width, labelHeight), VisualCodeLabels.Editor.ABILITY_SOUND_EFFECT_CONTENT, EditorStyles.boldLabel);
        posY += labelHeight;
        ability.castCompleteSound = (AudioClip)EditorGUI.ObjectField(new Rect(posX, posY, width, toggleHeight), ability.castCompleteSound, typeof(AudioClip), false);
        posY += toggleHeight;

        // Volume Slider
        GUI.Label(new Rect(posX, posY, 50, labelHeight), VisualCodeLabels.Editor.ABILITY_SOUND_EFFECT_VOLUME_CONTENT);
        posY += 3;
        ability.castCompleteSoundVolume = EditorGUI.Slider(new Rect(posX + 55, posY, width - 55, labelHeight), ability.castCompleteSoundVolume, 0.0f, 1.0f);
        posY += labelHeight;

        // Animation Trigger Point Slider
        GUI.Label(new Rect(posX, posY, width, labelHeight), VisualCodeLabels.Editor.ABILITY_TRIGGER_POINT_CONTENT, EditorStyles.boldLabel);
        posY += labelHeight;
        ability.animationActivationPosition = EditorGUI.Slider(new Rect(posX, posY, width, labelHeight), ability.animationActivationPosition, 0.0f, 1.0f);
        posY += labelHeight;
    }
}
