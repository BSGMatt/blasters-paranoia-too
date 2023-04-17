using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A base class used containing all of the methods and references shared between Character types
/// i.e, The Player and it's enemies. 
/// </summary>
public abstract class Character : MonoBehaviour
{
    
    public const float timeToStartPassiveHeal = 2f;
    public const int passiveHealPerSecond = 5;
    public const float tickRate = 0.125f;
    public bool dead = false;

    public const int s_move = 0;
    public const int s_attack = 1;
    public const int s_both = 2;

    public const int dashCost = 10; //The amount of stamina used to perform a dash.

    public CharacterController2D c2d;
    public Rigidbody2D rb;
    public BoxCollider2D surfaceCollider;
    public CircleCollider2D nonSurfaceCollider;
    public StatsManager statsManager;
    

    //These values are only used by BB, the player character, but since weapons have references
    //to only Character objects, not BB objects, I decided to put them here. 
    public int xp;
    public int stamina;
    public int level;



    public UnityEvent takenDamage;
    public UnityEvent startedMoving;

    public MinimapIcon minimapIcon;
    public AIController aiController;
    public Weapon currentWeapon;
    public TargetType targetType; //0 - Player, 1 - Building, 2 - Crystal

    /// <summary>
    /// <para>The maximum speed the character will move without any modifiers applied.
    /// With the use of modifiers (Like block modifiers) 
    /// the character speed can be modified to go beyond (or below) 
    /// this value. </para>
    /// 
    /// </summary>
    [SerializeField] protected float maxSpeed = 10f;

    /// <summary>
    /// The amount of stamina the character has. Stamina affects the players ability to move. 
    /// </summary>
    public int maxStamina = 100;

    /// <summary>
    /// The maximum amount of damage the character can take. 
    /// </summary>
    public int maxHP = 200;

    /// <summary>
    /// The amount of sheild points the character has. 
    /// </summary>
    public int sheilds;
    
    /// <summary>
    /// The amount of hit points the character has. 
    /// </summary>
    public int hp;

    /// <summary>
    /// Is this Character an enemy?
    /// </summary>
    protected bool isEnemy;

    /// <summary>
    /// The preferred way the character will try to approach its target. 
    /// 0 = Straight Line
    /// 1 = Follow 
    /// 2 = Get In Front
    /// </summary>
    protected int prefferedApproach;

    public bool invincible = false;

    protected bool movementEnabled = true;

    /// <summary>
    /// Can this Character level up?
    /// </summary>
    public bool isLevelable = true;

    /// <summary>
    /// Is health the only stat that this Character has?
    /// </summary>
    public bool onlyHasHealth = false;

    /// <summary>
    /// <para>Variables for storing the modifier values. Did this to avoid having to make a reference to
    /// the old speed/dmgtaken/hp values and check again when the effect is over to set back to the old values, etc. </para>
    /// </summary>
    public float currentSpeedModValue = 1f;
    public float currentDmgDealtModValue = 1f;

    public void Awake() {
        takenDamage = new UnityEvent();
        startedMoving = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {

        takenDamage.Invoke();

        Debug.Log("Damage Dealt: " + damage);

        if (sheilds > 0) {
            sheilds -= damage;
            if (sheilds < 0) sheilds = 0;
        }
        else {
            hp -= damage;
        }


        //Debug.Log("Current HP: " + hp);
        if (hp <= 0) {
            Die();
        }
    }


    public abstract void Die();

    /// <summary>
    /// A method to be used alongside Die(). Contains the operations that most characters will use when they die. 
    /// </summary>
    public void CommonDieMethod() {
        if (currentWeapon != null) Destroy(currentWeapon.gameObject);
        if (minimapIcon != null) Destroy(minimapIcon.gameObject);
        Destroy(gameObject);
    }

    public Rigidbody2D GetRigidBody() {
        return rb;
    }

    //Returns the Character's CharacterController2D reference. 
    public CharacterController2D GetController() {
        return c2d;
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }

    public float GetCurrentSpeed() {
        return (maxSpeed * currentSpeedModValue);
    }

    public int GetPrefferedApproach() {
        return prefferedApproach;
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void SetMovementEnabled(bool val) {
        movementEnabled = val;
    }

    public void ReplenishHPAndStamina() {
        hp = maxHP;
        stamina = maxStamina;
    }



    public abstract void DisableMovement();
    public abstract void EnableMovement();


}
