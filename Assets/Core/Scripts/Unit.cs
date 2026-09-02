using MyUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Unit : Interactable
{
    [Tooltip("The name of the unit. For player characters, this controls which character to load, what items can drop etc. For monsters and other units, it is the label shown to the player on hover.")]
    public string unitName;
    
    [Header("[Base Attributes]")]
    [Tooltip("The starting damage of the unit. Abilities will use this number to calculate how much damage they should do (e.g., an attack may do 200% of this number).")]
    [SerializeField] protected float baseDamage = 10;
    [Tooltip("The starting attacks per second of the unit (e.g., a value of 2 is two attacks per second). For this to affect abilities, they must be marked as 'Cooldown is Attack Speed'.")]
    [SerializeField] protected float baseAttacksPerSecond = 1.0f;
    [Tooltip("The starting maximum health of the unit (a value of 100 means the unit has 100 health). This is the base amount before modifiers.")]
    [SerializeField] protected float baseMaxHealth = 100;
    [Tooltip("The starting maximum resource of the unit (a value of 100 means the unit has 100 resource). This is the base amount before modifiers.")]
    [SerializeField] protected float baseMaxResource = 100;
    [Tooltip("The starting movement speed of the unit (a value of 3.0 means the unit moves 3 units per second). This is the base amount before modifiers.")]
    [SerializeField] protected float baseMovementSpeed = 3.0f;
    [Tooltip("The starting armor of the unit. This is the base amount before modifiers.")]
    [SerializeField] protected float baseArmor = 100;
    [Tooltip("The starting critical chance of the unit (a value of 0.2 would mean a 20% for a critical). This is the base amount before modifiers.")]
    [SerializeField] protected float baseCriticalStrikeChance = 0.2f;
    [Tooltip("The starting critical damage of the unit (a value of 2 would mean criticals deal 200% damage). This is the base amount before modifiers.")]
    [SerializeField] protected float baseCriticalStrikeDamage = 2.0f;
    [Tooltip("The starting health regeneration of the unit (a value of 2 would restore 2 health per second). This is the base amount before modifiers.")]
    [SerializeField] protected float baseHealthRegen = 0.0f;
    [Tooltip("The starting resource regeneration of the unit (a value of 2 would restore 2 resource per second). This is the base amount before modifiers.")]
    [SerializeField] protected float baseResourceRegen = 0.0f;
    public float baseAttackRange = 2.0f;

    [HideInInspector] public float health = 100;
    [HideInInspector] public float resource = 0;

    // Controls all of the unit stats. Use this to request the current value of a stat
    // or apply a modifier.
    public Stats stats = new Stats();

    [Header("[Castable Abilities]")]
    [Tooltip("All of the abilities the unit is able to cast.")]
    public List<Ability> abilities = new List<Ability>();
    public Dictionary<Ability, float> abilitiesLastCastAt = new Dictionary<Ability, float>();
    [HideInInspector] public Ability lastAbilityCast;
    private Dictionary<Ability, StatModifier> abilityDamageModifiers = new Dictionary<Ability, StatModifier>();
    private Dictionary<Ability, StatModifier> abilityCooldownModifiers = new Dictionary<Ability, StatModifier>();
    private Dictionary<Ability, StatModifier> abilityCostModifiers = new Dictionary<Ability, StatModifier>();

    [Header("[Unit Visuals]")]
    [Tooltip("Determines whether the unit can be force moved. This may be from knockbacks, or from force movement commands. This is useful for units which can't move (e.g., pylons).")]
    public bool unitCanBeForceMoved = true;
    public Transform handPoint;
    public GameFeedback onDeathFeedback;
    public GameFeedback onHitFeedback;

    [Header("Effect Attachment Points")]
    public Transform origin;
    public Transform castPoint;
    public Transform head;
    public Transform center;
    public Transform leftHand;
    public Transform rightHand;
    public Transform custom1;
    public Transform custom2;
    public Transform custom3;



    // Cached Values for Targeting, Attacking and Casting
    [HideInInspector] public Unit target;
    [NonSerialized] public Vector3 targetPosition;
    [NonSerialized] public Ability abilityBeingCast;
    [NonSerialized] public float finishCastAt;
    [NonSerialized] public float canMoveAt;
    [NonSerialized] public float canCastAt;
    [NonSerialized] public float stunnedUntil;
    protected Vector3 attackDirection;

    // Cache references to important components for easy access later.
    protected NavMeshAgent agentNavigation;
    [HideInInspector] public Animator animator;
    protected bool unitIsActive = false;
    protected bool hasAnimations = true;

    // Constants to prevent magic numbers in the code. Makes it easier to edit later.
    [HideInInspector] public bool isDead;
    protected const float FORCE_MOVE_DISTANCE_LEEWAY = 1.5f;
    protected const float MOVEMENT_DELAY_AFTER_ATTACKING = 0.0f;
    protected const float UNIT_TURNING_SPEED = 20.0f;
    protected const float TIME_BEFORE_CORPSE_DESTROYED = 5.0f;
    protected const float UNIT_DEACTIVATION_DISTANCE = 20;
    protected const string DEATH_TRIGGER = "Die";
    public static string[] animations = new string[] {
        "None", "One Hand Slash", "One Hand Stab", "Two Hand Slash", "Cheer",
        "Shout", "Pickup", "Magic Channel", "Magic Area Attack", "Punch",
        "Bow Shoot", "Jump", "Magic Front Attack", "Roll", "Custom1", "Custom2",
        "Custom3", "Custom4", "Custom5"
    };

    public enum Faction { Player, Enemy, Other };
    private Faction faction = Faction.Enemy;

    // Variables controlling how a unit is 'spun'.
    private float spinUntil, spinAngle, spinSpeed;

    public bool isTargetable { get; private set; }

    [HideInInspector] public BuffController buffs = null;

    [HideInInspector] public float forceMoveResetStoppingDistance;
    [HideInInspector] public bool isForceMoving;
    [HideInInspector] public Vector3 forceMoveLocation;

    /// <summary>
    /// Perform all initial setup.
    /// </summary>
    protected virtual void Start()
    {
        CacheComponents();
        SetupStats(); 
        SetupAbilities();
        SetupAnimations();
        SetupMapMarker();
    }

    /// <summary>
    /// Handles all the required setup for any map markers that this object should have.
    /// Any map marker objects should go on the 'Map' layer and be facing up towards the camera.
    /// </summary>
    protected virtual void SetupMapMarker()
    {

    }

    /// <summary>
    /// An untargetable unit cannot be the designated target for an ability, and cannot
    /// be hovered over or attacked. This can be used to make the player or other units
    /// unable to be attacked.
    /// </summary>
    public void SetTargetableStatus(bool isTargetable)
    {
        this.isTargetable = isTargetable;
    }

    /// <summary>
    /// Returns true if the unit is currently stunned, otherwise returns false.
    /// </summary>
    public virtual bool IsStunned()
    {
        return (Time.time < stunnedUntil);
    }

    /// <summary>
    /// Stuns the unit. A stunned unit cannot move or cast abilities.
    /// </summary>
    public virtual void Stun(float duration, Unit stunningUnit, IVisualCodeHandler stunningSource)
    {
        if (isDead) return;
        stunnedUntil = Time.time + duration;
        animator.CrossFadeInFixedTime("Stunned", 0.1f);
        GameManager.events.OnUnitStunned.Invoke(new GameEvents.OnUnitStunnedInfo(
                this,
                stunningUnit,
                stunningSource,
                duration
            ));

        SpawnOrRefreshEffect(GameManager.assets.stunEffect.GetComponent<CustomVisualEffect>(), duration, 1.0f, new Vector3(0, 0, 0));
    }

    public void SpawnVisualEffect(CustomVisualEffect effect, float duration, float scale, Vector3 offset)
    {
        GameObject obj = ObjectPooler.InstantiatePooled(effect.gameObject, transform.position + offset, transform.rotation);
        obj.name = effect.name;
        obj.transform.localScale = effect.transform.localScale * scale;
        if (duration > 0) obj.GetComponent<CustomVisualEffect>().DestroyAfter(duration);
        obj.transform.SetParent(transform);
    }

    public void SpawnOrRefreshEffect(CustomVisualEffect effect, float duration, float scale, Vector3 offset)
    {
        if (effect == null) return;
        Transform existing = transform.Find(effect.name);
        if (existing != null)
        {
            existing.GetComponent<CustomVisualEffect>().DestroyAfter(duration);
        }
        else
        {
            SpawnVisualEffect(effect, duration, scale, offset);
        }
    }

    public void RemoveVisualEffect(CustomVisualEffect effect)
    {
        if (effect == null) return;
        Transform existing = transform.Find(effect.name);
        if (existing != null && existing.GetComponent<CustomVisualEffect>() != null)
        {
            ObjectPooler.DestroyPooled(existing.gameObject);
        }
    }

    /// <summary>
    /// Apply the damage formula to this unit.
    /// </summary>
    protected virtual float ApplyDamageFormula(float amount, bool isCritical,
        Unit damagingUnit, IVisualCodeHandler damageSource)
    {
        // Apply damage modifiers (e.g. a -50% damage taken buff).
        amount *= GetBaseDamageTakenModifier();

        // Armor currently doesn't do anything? Add logic here.

        // Return the modified amount.
        return amount;
    }

    /// <summary>
    /// Perform all frame-by-frame unit updates.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (GameManager.player.isDead) return;
        if (!(unitIsActive = IsUnitActive())) return;
        ApplyStatBuffs();
        UpdateAnimations();
        ManageAbilityCasting();
        //FaceTowardsAttackTarget();
        ApplyHealthRegeneration(Time.deltaTime);
        ApplyResourceRegeneration(Time.deltaTime);
        buffs.Update();
    }

    /// <summary>
    /// Perform all initial stat setup for the unit.
    /// </summary>
    protected virtual void SetupStats()
    {
        stats.TrackStat(Stat.Damage, "Damage", baseDamage);
        stats.TrackStat(Stat.MaxHealth, "Max Health", baseMaxHealth);
        stats.TrackStat(Stat.MaxResource, "Max Resource", baseMaxResource);
        stats.TrackStat(Stat.MovementSpeed, "Movement Speed", 1);
        stats.TrackStat(Stat.Armor, "Armor", baseArmor);
        stats.TrackStat(Stat.AttacksPerSecond, "Attacks Per Second", baseAttacksPerSecond);
        stats.TrackStat(Stat.ResourceCostReduction, "Ability Cost Reduction", 1);
        stats.TrackStat(Stat.CooldownReduction, "Cooldown Reduction", 1);
        stats.TrackStat(Stat.CriticalStrikeChance, "Critical Strike Chance", baseCriticalStrikeChance);
        stats.TrackStat(Stat.CriticalStrikeDamage, "Critical Strike Damage", baseCriticalStrikeDamage);
        stats.TrackStat(Stat.ResourceGeneration, "Resource Generation", 1);
        stats.TrackStat(Stat.DamageTaken, "Damage Taken Modifier", 1);

        // Track any remaining stats, but use a default value of zero as none was given.
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            stats.TrackStat(stat, stat.Label(), 0);

        health = stats.GetValue(Stat.MaxHealth);
    }

    /// <summary>
    /// Cache components for easier access later.
    /// </summary>
    private void CacheComponents()
    {
        agentNavigation = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        buffs = new BuffController(this);
    }

    /// <summary>
    /// Performs all animation setup. Not all units will use the default animations,
    /// and if they don't, we don't want to try and play those animations (e.g. not
    /// all units may be able to walk).
    /// </summary>
    private void SetupAnimations()
    {
        hasAnimations = false;
        if (animator == null)
        {
            hasAnimations = false;
        }
        else
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
                if (param.name == "Speed") hasAnimations = true;
        }
    }

    /// <summary>
    /// Returns true if the unit can move, otherwise false.
    /// </summary>
    public virtual bool CanMove()
    {
        return (Time.time >= canMoveAt);
    }

    /// <summary>
    /// Returns true if the unit currently has a cast in progress, otherwise false.
    /// </summary>
    public bool CastInProgress()
    {
        return (abilityBeingCast != null);
    }

    /// <summary>
    /// Set the target of this unit to the specified unit.
    /// </summary>
    protected void SetTarget(Unit unit)
    {
        target = unit;
    }

    /// <summary>
    /// Returns true if the unit is currently moving, otherwise false.
    /// </summary>
    public bool IsMoving()
    {
        return (agentNavigation.velocity.magnitude > 0);
    }

    /// <summary>
    /// Force the unit to walk towards the given location. Once they reach the given
    /// location, they can perform actions as normal.
    /// </summary>
    public void ForceMove(Vector3 location)
    {
        if (!unitCanBeForceMoved) return;
        isForceMoving = true;
        forceMoveResetStoppingDistance = agentNavigation.stoppingDistance;
        agentNavigation.stoppingDistance = 0;
        forceMoveLocation = Utilities.GetValidNavMeshPosition(location);
    }

    public void StopForceMove()
    {
        isForceMoving = false;
        agentNavigation.stoppingDistance = forceMoveResetStoppingDistance;
        StopMoving();
    }

    /// <summary>
    /// Immediately cancel the force move command. This will allow the unit to move
    /// normally without reaching the target location.
    /// </summary>
    public void CancelForceMove()
    {
        isForceMoving = false;
    }

    /// <summary>
    /// Setup the abilities for this unit. This creates a unique copy so that
    /// it the unique instance on this unit can be modified without modifying the
    /// base copy.
    /// </summary>
    protected virtual void SetupAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i] = abilities[i].ShallowCopy();
            abilities[i].SetOwner(this);
            GameManager.logicEngine.AddEngine(abilities[i].engine);
        }
    }

    /// <summary>
    /// Stop the unit from moving.
    /// </summary>
    public void StopMoving()
    {
        if (agentNavigation.isOnNavMesh)
            agentNavigation.SetDestination(transform.position);
    }

    /// <summary>
    /// Get the "Cast Point" of this unit.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCastPoint()
    {
        if (castPoint != null)
            return castPoint.transform.position;
        else if (handPoint != null)
            return handPoint.transform.position;
        else
            return transform.position + new Vector3(0, 1, 0);
    }

    /// <summary>
    /// Play a specific animation on the unit.
    /// </summary>
    public void PlayAnimation(string animation, float totalAnimationTime = 1.0f)
    {
        if (!hasAnimations) return;
        totalAnimationTime = Mathf.Clamp(totalAnimationTime, 0.1f, 5.0f);

        if (animation != animations[0])
        {
            float speedModifier = Mathf.Max(1.0f, 1f / totalAnimationTime);
            animator.SetFloat("AbilityCastSpeed", speedModifier);
            //animator.CrossFadeInFixedTime(animation, 0.3f / speedModifier);
            animator.Play(animation);
        }
    }

    /// <summary>
    /// Smoothly face towards the attack target if the unit is attacking.
    /// </summary>
    protected void FaceTowardsAttackTarget()
    {
        // Smoothly face towards the last attack direction if stationary.
        if (Time.time < canMoveAt && attackDirection != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(attackDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                look, Time.deltaTime * UNIT_TURNING_SPEED);
        }
    }

    /// <summary>
    /// Apply any per-frame unit changes needed due to unit buffs.
    /// </summary>
    private void ApplyStatBuffs()
    {
        if (IsStunned())
        {
            agentNavigation.speed = 0;
        }
        else
        {
            agentNavigation.speed = baseMovementSpeed * stats.GetValue(Stat.MovementSpeed);
        }
    }

    /// <summary>
    /// Apply health regeneration to the unit for the specified time period.
    /// </summary>
    private void ApplyHealthRegeneration(float duration)
    {
        if (baseHealthRegen != 0) AddHealth(baseHealthRegen * duration);
    }

    /// <summary>
    /// Apply resource regeneration to the unit for the specified time period.
    /// </summary>
    private void ApplyResourceRegeneration(float duration)
    {
        if (baseResourceRegen != 0) AddResource(baseResourceRegen * duration);
    }

    /// <summary>
    /// Returns true if the unit is currently active, otherwise false.
    /// Units deactivate when they are far away from the player, to prevent
    /// unnecessary game logic from being run.
    /// </summary>
    private bool IsUnitActive()
    {
        return (Vector3.Distance(GameManager.player.transform.position,
            transform.position) < UNIT_DEACTIVATION_DISTANCE);
    }

    /// <summary>
    /// Do all required animation updates.
    /// </summary>
    private void UpdateAnimations()
    {
        if (!hasAnimations) return;
        animator.SetFloat("Speed", agentNavigation.velocity.magnitude / baseMovementSpeed);
        animator.SetBool("Stunned", IsStunned());
    }

    /// <summary>
    /// Manage the casting of abilities for this unit.
    /// </summary>
    private void ManageAbilityCasting()
    {
        if (IsStunned())
        {
            if (IsCasting()) abilityBeingCast.CancelCast(this);
            return;
        }
        if (IsCasting() && Time.time > finishCastAt)
            abilityBeingCast.FinishCast(this, target, targetPosition);
        else if (!IsCasting() && !isForceMoving)
            TryToCastAbilities();
    }

    /// <summary>
    /// Start a unit spinning.
    /// </summary>
    /// <param name="spinSpeed">The rotation speed per second (in degrees).</param>
    /// <param name="duration">The total time to spin for.</param>
    public void StartSpin(float spinSpeed, float duration)
    {
        this.spinUntil = Time.time + duration;
        this.spinSpeed = spinSpeed;
        this.spinAngle = animator.transform.rotation.eulerAngles.y;
    }

    /// <summary>
    /// Handle spinning in late update so that all other position and rotation
    /// changes have already been handled.
    /// </summary>
    private void LateUpdate()
    {
        Spin();
    }

    /// <summary>
    /// Handle all of the spinning logic for the unit.
    /// </summary>
    private void Spin()
    {
        if (Time.time < spinUntil)
        {
            spinAngle += spinSpeed * Time.deltaTime;
            animator.transform.rotation = Quaternion.Euler(0, spinAngle, 0);
        }
        else
        {
            animator.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary>
    /// This function is called every frame and should include logic for casting abilities.
    /// </summary>
    protected virtual void TryToCastAbilities()
    {

    }

    /// <summary>
    /// Return the attack point of the unit. The attack point is exactly half way
    /// between the unit and the maximum range of the unit. This allows you to do an effect
    /// directly circling the enemies within a set distance of the main attack point.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAttackPoint()
    {
        return transform.position + transform.forward * baseAttackRange / 2.0f;
    }

    /// <summary>
    /// Returns true if the unit can cast the given ability, otherwise false.
    /// </summary>
    private bool CanCastAbility(Ability ability, Unit target, Vector3 targetPosition)
    {
        if (IsCasting()) // Unit cannot already be casting.
            return false;
        if (Time.time < canCastAt) // Unit cannot be prevented from casting.
            return false;
        if (!ability.IsValidToCast(this, target, targetPosition)) // Ability requirements must be met.
            return false;
        return true;
    }

    public void ReduceAbilityCooldown(Ability ability, float amount)
    {
        foreach (var a in abilitiesLastCastAt.Keys)
        {
            if (ability.name == a.name)
            {
                ability = a;
                break;
            }
        }

        if (abilitiesLastCastAt.ContainsKey(ability))
        {
            abilitiesLastCastAt[ability] -= amount;
        }
        else
        {
            abilitiesLastCastAt.Add(ability, -999);
        }
    }

    public virtual bool CastAbility(Ability ability, Unit target, Vector3 targetPosition, bool applyCost = true, bool applyCastingRestrictions = true)
    {
        if (applyCastingRestrictions)
        {
            if (!CanCastAbility(ability, target, targetPosition)) return false;

            abilitiesLastCastAt[ability] = Time.time;
            if (!ability.canMoveWhileCasting) StopMoving();

            if (ability.requiresLineOfSight)
                targetPosition = Utilities.GetClosestPointInLOS(transform.position, targetPosition);
        }

        ability.StartCast(this, target, targetPosition, applyCost, applyCastingRestrictions);

        if (applyCastingRestrictions)
        {
            // Calculate the attack direction for this ability.
            attackDirection = (targetPosition - transform.position);
            attackDirection.y = 0;
            attackDirection.Normalize();
        }
        else
        {
            ability.FinishCast(this, target, targetPosition);
        }

        return true;
    }

    /// <summary>
    /// Cast the ability without checking any requirements. E.g. An Item may use this method
    /// to cast an ability automatically without the usual fanfare and checks.
    /// </summary>
    public virtual void CastAbilityWithoutCheckingRequirements(Ability ability,
        Unit target, Vector3 targetPosition, bool payCosts, bool hasCastTime)
    {
        ability.StartCast(this, target, targetPosition);



        if (!hasCastTime)
        {
            ability.FinishCast(this, target, targetPosition);
        }
    }



    /// <summary>
    /// Return the damage value, modified by unit stats (e.g. armor and DR).
    /// </summary>
    protected virtual float GetArmorDamageTakenModifier()
    {
        return 1.0f - (GameManager.armorValues.armorDamageReductionCurve.Evaluate(
            stats.GetValue(Stat.Armor) / GameManager.armorValues.maxArmor));
    }

    /// <summary>
    /// Apply the base damage taken modifier (e.g. an 'increase damage taken by 100%' debuff).
    /// </summary>
    protected virtual float GetBaseDamageTakenModifier()
    {
        return stats.GetValue(Stat.DamageTaken);
    }

    /// <summary>
    /// Remove the specified amount of health from the unit, killing it if needed.
    /// </summary>
    public virtual void TakeDamage(float amount, bool isCritical,
        Unit damagingUnit, IVisualCodeHandler damageSource)
    {
        if (isDead) return;

        amount = ApplyDamageFormula(amount, isCritical, damagingUnit, damageSource);
        if (amount < 0) return;

        // Remove the health.
        RemoveHealth(amount, damagingUnit, damageSource, isCritical);

        // Apply the "on hit" feedback for this unit.
        onHitFeedback?.ActivateFeedback(gameObject, null, transform.position);

        // Trigger OnUnitDamaged event.
        GameManager.events.OnUnitDamaged.Invoke(new GameEvents.OnUnitDamagedInfo(this, amount, damagingUnit, damageSource, isCritical));
    }

    /// <summary>
    /// Kill the unit, destroying the unit logic but keeping the model 
    /// around to play the death animation.
    /// </summary>
    public virtual void Kill(Unit killingUnit, IVisualCodeHandler killingSource, bool isCritical = false)
    {
        // Do not kill the unit if it is already dead.
        if (isDead) return;

        // If unit isn't dead, perform required death actions.
        isDead = true;
        GameManager.events.OnUnitKilled.Invoke(new GameEvents.OnUnitKilledInfo(this, killingUnit, killingSource, isCritical));
        onDeathFeedback?.ActivateFeedback(gameObject, null, gameObject.transform.position);
        if (hasAnimations)
        {
            animator.SetTrigger(DEATH_TRIGGER);
        }
        StopMoving();
    }

    /// <summary>
    /// // Add the specified amount of health to the unit.
    /// </summary>
    public virtual void AddHealth(float amount, Unit healingUnit = null,
        IVisualCodeHandler healingSource = null)
    {
        health = Mathf.Min(health + amount, stats.GetValue(Stat.MaxHealth));
        if (healingUnit != null && healingSource != null)
        {
            GameManager.events.OnUnitHealed.Invoke(new GameEvents.OnUnitHealedInfo(this, amount, healingUnit, healingSource));
        }
    }

    /// <summary>
    /// Remove the specified amount of health from the unit. 
    /// </summary>
    public virtual void RemoveHealth(float amount, Unit damagingUnit = null,
        IVisualCodeHandler damageSource = null, bool isCritical = false)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Kill(damagingUnit, damageSource, isCritical);
        }
    }

    /// <summary>
    /// Add the specified amount of resource to the unit.
    /// </summary>
    public virtual void AddResource(float amount)
    {
        amount *= stats.GetValue(Stat.ResourceGeneration);
        resource = Mathf.Clamp(resource + amount, 0, stats.GetValue(Stat.MaxResource));
    }

    /// <summary>
    /// Remove the specified amount of resource from the unit.
    /// </summary>
    public virtual void RemoveResource(float amount)
    {
        resource = Mathf.Clamp(resource - amount, 0, stats.GetValue(Stat.MaxResource));
    }

    /// <summary>
    /// Filter the given unit list to only return allies of this unit.
    /// </summary>
    public virtual List<Unit> GetAllies(List<Unit> units)
    {
        List<Unit> allies = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (GetFaction() == unit.GetFaction())
            {
                allies.Add(unit);
            }
        }
        return allies;
    }

    /// <summary>
    /// Filter the given unit list to only return enemies of this unit.
    /// </summary>
    public virtual List<Unit> GetEnemies(List<Unit> units)
    {
        List<Unit> enemies = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (GetFaction() != unit.GetFaction())
            {
                enemies.Add(unit);
            }
        }
        return enemies;
    }

    /// <summary>
    /// Return true if the specified unit is an ally of this unit, otherwise return false.
    /// </summary>
    public bool IsAlly(Unit unit)
    {
        return (GetFaction() == unit.GetFaction());
    }

    /// <summary>
    /// Return true if the specified unit is an enemy of this unit, otherwise return false.
    /// </summary>
    public bool IsEnemy(Unit unit)
    {
        return (GetFaction() != unit.GetFaction());
    }

    

    

    /// <summary> 
    /// Return the faction of the given unit.
    /// </summary>
    public virtual Faction GetFaction()
    {
        return faction;
    }

    public virtual void SetFaction(Faction faction)
    {
        this.faction = faction;
    }

    

    

    /// <summary>
    /// Return the name of this unit.
    /// </summary>
    public override string ToString()
    {
        return name;
    }

    public virtual void OnAbilitiesUpdated()
    {

    }

    

    public bool HasBuff(string buffName)
    {
        return stats.HasBuffWithLabel(buffName);
    }

    public float GetCurrentHealthPercent()
    {
        return health / stats.GetValue(Stat.MaxHealth);
    }

    public float GetCurrentResourcePercent()
    {
        return resource / stats.GetValue(Stat.MaxResource);
    }



    #region Combat

    /// <summary>
    /// Returns true if the point is in range of the unit, otherwise false.
    /// </summary>
    public override bool InRange(Vector3 point)
    {
        return Vector3.Distance(transform.position, point) < baseAttackRange;
    }

    /// <summary>
    /// Return true if the unit is currently casting, otherwise false.
    /// </summary>
    /// <returns></returns>
    public bool IsCasting()
    {
        return (abilityBeingCast != null);
    }

    /// <summary>
    /// Return true if the attack should be a critical, otherwise retuern false.
    /// </summary>
    private bool CheckForCritical()
    {
        return (Random.value < stats.GetValue(Stat.CriticalStrikeChance));
    }

    /// <summary>
    /// Have this unit damage another unit for the specified amount.
    /// This method will handle the possibility of attacker modifiers, critical
    /// strike chance etc.
    /// </summary>
    public void DamageOtherUnit(Unit unit, float weaponDamagePercent, IVisualCodeHandler source)
    {
        float amount = Mathf.Round(stats.GetValue(Stat.Damage) * weaponDamagePercent);
        if (source != null && source.GetData() is Ability ability)
            amount *= GetAbilityDamageModifier(ability);

        if (CheckForCritical())
        {
            amount *= stats.GetValue(Stat.CriticalStrikeDamage);
            unit.TakeDamage(amount, true, this, source);
        }
        else
        {
            unit.TakeDamage(amount, false, this, source);
        }
    }

    public void HealOtherUnit(Unit unit, float amount, IVisualCodeHandler source)
    {
        unit.AddHealth(amount, this, source);
    }

    /// <summary>
    /// Have this unit damage the given list of units for the specified amount.
    /// This method will handle the possibility of attacker modifiers, critical
    /// strike chance etc.
    /// </summary>
    public void DamageOtherUnits(List<Unit> units, float weaponDamagePercent, IVisualCodeHandler source)
    {
        float amount = Mathf.Round(stats.GetValue(Stat.Damage) * weaponDamagePercent);
        if (source != null && source.GetData() is Ability ability)
            amount *= GetAbilityDamageModifier(ability);

        bool isCrit = false;
        if (CheckForCritical())
        {
            amount *= stats.GetValue(Stat.CriticalStrikeDamage);
            isCrit = true;
        }
        foreach (Unit unit in units)
        {
            unit.TakeDamage(amount, isCrit, this, source);
        }
    }

    #endregion

    #region Unit Abilities/Casting

    // Should fix this up to reduce the code repetition.
    public void AddAbility(Ability ability)
    {
        Ability abilityToAdd = ability.ShallowCopy();
        abilityToAdd.SetOwner(this);
        GameManager.logicEngine.AddEngine(abilityToAdd.engine);
        abilities.Add(abilityToAdd);
        OnAbilitiesUpdated();
    }

    public void LockAbility(Ability ability)
    {
        // TODO: Add this logic.
    }

    public void UnlockAbility(Ability ability)
    {
        // TODO: Add this logic.
    }

    // Should fix this up to reduce the code repetition.
    public void RemoveAbility(Ability abilityTemplate)
    {
        foreach (var a in abilities)
        {
            if (abilityTemplate.name == a.name)
            {
                abilityTemplate = a;
                break;
            }
        }
        if (abilityTemplate != null)
        {
            GameManager.logicEngine.RemoveEngine(abilityTemplate.GetEngine());
            abilities.Remove(abilityTemplate);
            OnAbilitiesUpdated();
        }
    }

    // Should fix this up to reduce the code repetition.
    public void ReplaceAbility(Ability abilityToReplaceTemplate, Ability newAbility)
    {
        foreach (var a in abilities)
        {
            if (abilityToReplaceTemplate.name == a.name)
            {
                abilityToReplaceTemplate = a;
                break;
            }
        }
        if (abilityToReplaceTemplate != null)
        {
            int index = abilities.IndexOf(abilityToReplaceTemplate);
            if (index >= 0)
            {
                GameManager.logicEngine.RemoveEngine(abilityToReplaceTemplate.GetEngine());
                Ability abilityToAdd = newAbility.ShallowCopy();
                abilityToAdd.SetOwner(this);
                GameManager.logicEngine.AddEngine(abilityToAdd.engine);
                abilities[index] = abilityToAdd;
                OnAbilitiesUpdated();
            }
        }
    }

    /// <summary>
    /// Casts the given ability. This will only work in the specified ability is 
    /// castable. If the ability cannot be cast, nothing will happen.
    /// </summary>
    public void CastAbility(Ability ability)
    {

    }

    /// <summary>
    /// Casts the given ability without any of the requirements. This means that 
    /// the ability will not cost resources, go on cooldown, prevent movement, or any
    /// other usual checks.
    /// </summary>
    public void CastAbilityWithoutRequirements(Ability ability)
    {

    }

    public void AddTimedAbilityDamageModifier(Ability ability, float modifier, float duration, string buffName = "Buff", int maxStacks = 99)
    {
        bool isRefreshing = HasBuff(buffName);
        if (!abilityDamageModifiers.ContainsKey(ability.template))
        {
            abilityDamageModifiers[ability.template] = new StatModifier($"{ability.name}_DamageMod", 1.0f);
        }
        abilityDamageModifiers[ability.template].AddTimedPercentageModifier(modifier, duration, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public void AddAbilityDamageModifier(Ability ability, float modifier, string buffName = "Buff", int maxStacks = 99)
    {
        if (!abilityDamageModifiers.ContainsKey(ability.template))
        {
            abilityDamageModifiers[ability.template] = new StatModifier($"{ability.name}_DamageMod", 1.0f);
        }
        abilityDamageModifiers[ability.template].AddPercentageModifier(modifier, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public void AddTimedAbilityCooldownModifier(Ability ability, float modifier, float duration, string buffName = "Buff", int maxStacks = 99)
    {
        if (!abilityCooldownModifiers.ContainsKey(ability.template))
        {
            abilityCooldownModifiers[ability.template] = new StatModifier($"{ability.name}_CooldownMod", 1.0f);
        }
        abilityCooldownModifiers[ability.template].AddTimedPercentageModifier(modifier, duration, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public void AddAbilityCooldownModifier(Ability ability, float modifier, string buffName = "Buff", int maxStacks = 99)
    {
        if (!abilityCooldownModifiers.ContainsKey(ability.template))
        {
            abilityCooldownModifiers[ability.template] = new StatModifier($"{ability.name}_CooldownMod", 1.0f);
        }
        abilityCooldownModifiers[ability.template].AddPercentageModifier(modifier, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public void AddTimedAbilityCostModifier(Ability ability, float modifier, float duration, string buffName = "Buff", int maxStacks = 99)
    {
        if (!abilityCostModifiers.ContainsKey(ability.template))
        {
            abilityCostModifiers[ability.template] = new StatModifier($"{ability.name}_CostMod", 1.0f);
        }
        abilityCostModifiers[ability.template].AddTimedPercentageModifier(modifier, duration, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public void AddAbilityCostModifier(Ability ability, float modifier, string buffName = "Buff", int maxStacks = 99)
    {
        if (!abilityCostModifiers.ContainsKey(ability.template))
        {
            abilityCostModifiers[ability.template] = new StatModifier($"{ability.name}_CostMod", 1.0f);
        }
        abilityCostModifiers[ability.template].AddPercentageModifier(modifier, buffName, maxStacks);
        GameManager.events.OnUnitGainsBuff.Invoke(new GameEvents.OnUnitGainsBuffInfo(this, ability.GetOwner(), buffName));
    }

    public float GetAbilityDamageModifier(Ability ability)
    {
        if (abilityDamageModifiers.ContainsKey(ability.template))
        {
            return abilityDamageModifiers[ability.template].GetValue();
        }
        else
        {
            return 1.0f;
        }
    }

    public float GetAbilityCooldownModifier(Ability ability)
    {
        if (abilityCooldownModifiers.ContainsKey(ability.template))
        {
            return abilityCooldownModifiers[ability.template].GetValue();
        }
        else
        {
            return 1.0f;
        }
    }

    public float GetAbilityCostModifier(Ability ability)
    {
        if (abilityCostModifiers.ContainsKey(ability.template))
        {
            return 2.0f - abilityCostModifiers[ability.template].GetValue();
        }
        else
        {
            return 1.0f;
        }
    }

    public void RemoveAbilityBuffModifiers(Ability ability, string buffName)
    {
        if (abilityCooldownModifiers.ContainsKey(ability.template))
        {
            abilityCooldownModifiers[ability.template].RemoveModifiersWithLabel(buffName);
        }
        if (abilityCostModifiers.ContainsKey(ability.template))
        {
            abilityCostModifiers[ability.template].RemoveModifiersWithLabel(buffName);
        }
        if (abilityDamageModifiers.ContainsKey(ability.template))
        {
            abilityDamageModifiers[ability.template].RemoveModifiersWithLabel(buffName);
        }
    }

    #endregion


    #region Unit Visuals / Effects

    #endregion

    #region Movement

    /// <summary>
    /// Move this unit towards the target location over time.
    /// </summary>
    public void MoveOverTime(Vector3 targetLocation, float duration)
    {
        if (!unitCanBeForceMoved) return;
        StartCoroutine(MoveOverTimeCoroutine(targetLocation, duration));
    }

    /// <summary>
    /// Coroutine to hand moving the unit in a straight line over time.
    /// </summary>
    private IEnumerator MoveOverTimeCoroutine(Vector3 targetLocation, float duration)
    {
        Vector3 startLocation = transform.position;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            StopMoving();
            agentNavigation.speed = 0;
            transform.LookAt(targetPosition);
            float frac = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startLocation, targetLocation, frac);
            yield return null;
        }
        StopMoving();
        agentNavigation.SetDestination(transform.position +
            (targetLocation - startLocation).normalized * 0.01f);
    }

    /// <summary>
    /// Teleport the player to the given location.
    /// </summary>
    public void Teleport(Vector3 newPosition)
    {
        if (!unitCanBeForceMoved) return;
        agentNavigation.Warp(Utilities.GetValidNavMeshPosition(newPosition));
        StopMoving();
    }

    #endregion
}
