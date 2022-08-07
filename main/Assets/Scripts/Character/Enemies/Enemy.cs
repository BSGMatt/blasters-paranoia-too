using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IComparable<Enemy>
{
    public AIController aiController;
    public Healthbar hbar;
    public int state;
    public int difficulty;
    public int targetType; //0 - Player, 1 - Building
    public GameObject weaponObj;
    public Approach approach;

    // Start is called before the first frame update
    void Start() {
        aiController = GetComponent<AIController>();
        state = 0; //Set state to move;
        aiController.InitPath(targetType);

        //initialize the enemy's weapon. 
        currentWeapon = GameObject.Instantiate(weaponObj, transform.position, Quaternion.identity).GetComponent<Weapon>();
        currentWeapon.host = this;

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

    // Update is called once per frame
    void Update() {
        if (aiController.target == null) aiController.ResetPath();
    }

    void FixedUpdate() {

        //Keep following path until the end has been reached. 
        if (!aiController.reachedEndOfPath && movementEnabled) {
            aiController.RunPath();        
        }

        if (aiController.reachedEndOfPath)
        {
            currentWeapon.canFire = true;
            state = Character.s_attack;

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
        CommonDieMethod();

        FindObjectOfType<SpawnManager>().enemiesLeft--;
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

    public override void Init() {
        Debug.LogWarning("This init function doesn't do anything. ");
    }

}
