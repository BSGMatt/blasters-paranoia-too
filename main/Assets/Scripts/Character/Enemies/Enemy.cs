using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Enemy : Character, IComparable<Enemy>
{
    public Healthbar hbar;
    public int state;
    public int difficulty;
    public Approach approach;
    public WeaponCard weaponCard;
    public EnemyCard enemyCard;

    // Start is called before the first frame update
    protected void Start() {

        LoadValuesFromCard();

        isEnemy = true;
        isLevelable = false;
        onlyHasHealth = true;

        aiController = GetComponent<AIController>();
        state = 0; //Set state to move;
        aiController.InitPath(targetType);

        //initialize the enemy's weapon. 
        currentWeapon = GameObject.Instantiate(weaponCard.prefab, transform).GetComponent<Weapon>();
        currentWeapon.host = this;
        currentWeapon.card = weaponCard;

        if (currentWeapon != null) currentWeapon.canFire = false;
        movementEnabled = true;

        //set initial state based on its approach
        switch(approach)
        {
            default:
                state = Character.s_move;
                currentWeapon.canFire = false;
                break;
            case Approach.AGGRO:
                state = Character.s_both;
                currentWeapon.canFire = true;
                break;
        }
    }
    
    private void LoadValuesFromCard() {
        maxHP = enemyCard.maxHP;
        hp = maxHP;
        maxSpeed = enemyCard.speed;
        weaponCard = enemyCard.weapon;
        approach = enemyCard.approach;
        targetType = enemyCard.targetType;
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = enemyCard.color;
        if (enemyCard.sprite != null) {
            spr.sprite = enemyCard.sprite;
        }
    }

    // Update is called once per frame
    protected void Update() {
        //If there is no target, start a new path. 
        if (aiController.target == null) MoveToNextTarget();

        //Check if the current target is a character that is dead. 
        Character targetChar = aiController.target.gameObject.GetComponent<Character>();
        if (targetChar != null && targetChar.dead) {
            MoveToNextTarget();
        }

        if (hp <= 0) Die();
    }

    //Helper method for changing the target and creating a new path. 
    private void MoveToNextTarget() {

        if (approach == Approach.PASSIVE) {
            if (currentWeapon != null) currentWeapon.canFire = false;
        }

        aiController.ResetPath();
    }

    protected void FixedUpdate() {

        //Keep following path until the end has been reached. 
        if (!aiController.reachedEndOfPath && movementEnabled) {
            aiController.RunPath();        
        }

        if (aiController.reachedEndOfPath)
        {
            currentWeapon.canFire = true;

            //Reload if neccessary
            if (currentWeapon.ammo <= 0)
                currentWeapon.ammo = currentWeapon.card.maxAmmo;


            state = Character.s_attack;

            //Check if the target is moving away from us, if so, start moving. 
            if (Vector2.Distance(transform.position, aiController.target.position) >= aiController.maxDistance)
            {
                aiController.InitPath(targetType);
                state = Character.s_both;

                if (approach == Approach.PASSIVE)
                {
                    currentWeapon.canFire = false;
                    state = Character.s_move;
                }
            }
        }
    }

    public int CompareTo(Enemy other) {
        int diff = difficulty - other.difficulty;
        return diff != 0 ? (int) Mathf.Sign(diff) : 0;
    }

    public void SetState(int val) {
        if (val < 0 || val > 2) return;
        state = val;
    }

    public override void Die() {
        FindObjectOfType<Minimap>().DeleteMinimapIcon(minimapIcon);
        CommonDieMethod();
    }

    public override void DisableMovement() {
        aiController.StopPath();
        if (currentWeapon != null) currentWeapon.canFire = false;
        movementEnabled = false;
    }

    public override void EnableMovement() {
        if (aiController.reachedEndOfPath) aiController.InitPath(targetType);

        //Check if the enemy was allowed to fire their weapon before stopping
        if (currentWeapon != null && state > 0) currentWeapon.canFire = true;
        movementEnabled = true;
    }

}
