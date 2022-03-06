using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class used containing all of the methods and references shared between Character types
/// i.e, The Player and it's enemies. 
/// </summary>
public abstract class Character : MonoBehaviour
{

    [SerializeField] protected CharacterController2D c2d;
    [SerializeField] protected Rigidbody2D rb;
    

    //List all of the Character properties and their default values;
    #region DEFAULT_PROPERTY_VALUES
    protected const float def_maxSpeed = 10f;
    protected const int def_maxHP = 200;
    protected const float def_dmg_taken = 1f;
    #endregion

    /// <summary>
    /// <para>The maximum speed the character will move without any modifiers applied.
    /// With the use of modifiers (Like block modifiers) 
    /// the character speed can be modified to go beyond (or below) 
    /// this value. </para>
    /// 
    /// </summary>
    [SerializeField] protected float maxSpeed = def_maxSpeed;

    /// <summary>
    /// The maximum amount of damage the character can take. 
    /// </summary>
    [SerializeField] protected int maxHP = def_maxHP;

    /// <summary>
    /// <para>A percente determining how much of the incoming damage that the
    /// character will take. </para>
    /// </summary>
    [SerializeField] protected float dmgTaken = def_dmg_taken;

    /// <summary>
    /// The amount of hit points the character has. 
    /// </summary>
    protected int hp;

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

    ///

    protected bool movementEnabled = true;

    /// <summary>
    /// <para>Variables for storing the modifier values. Did this to avoid having to make a reference to
    /// the old speed/dmgtaken/hp values and check again when the effect is over to set back to the old values, etc. </para>
    /// </summary>
    protected float currentSpeedModValue = 1f;
    protected float currentDmgTakenModValue = 1f;
    protected float currentMaxHPModValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {

        int value = Mathf.RoundToInt(damage * dmgTaken);

        hp -= value;


        //Debug.Log("Current HP: " + hp);
        if (hp <= 0) {
            Die();
        }
    }


    public abstract void Die();
    public abstract void Init();

    public void SetHP(int hp) {
        this.hp = hp;
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



    public abstract void DisableMovement();
    public abstract void EnableMovement();


}
