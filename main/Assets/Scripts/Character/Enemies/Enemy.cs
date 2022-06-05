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

    // Start is called before the first frame update
    void Start() {
        aiController = GetComponent<AIController>();
        state = 0; //Set state to move;
        aiController.InitPath(targetType);
        if (currentWeapon != null) currentWeapon.canFire = false;
        movementEnabled = true;
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        if (!aiController.reachedEndOfPath && movementEnabled) {
            aiController.RunPath();
        }

        if (aiController.reachedEndOfPath) {
            aiController.InitPath(targetType);
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

    public void Attack() {
        currentWeapon.canFire = true;
    }


}
