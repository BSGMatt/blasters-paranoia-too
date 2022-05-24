using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IComparable<Enemy>
{
    public AIController aiController;
    public Healthbar hbar;
    public string state;
    public int difficulty;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int CompareTo(Enemy other) {
        int diff = difficulty - other.difficulty;
        return diff != 0 ? (int) Mathf.Sign(diff) : 0;
    }

    public void SetState(string val) {

    }

    public override void Die() {
        CommonDieMethod();
    }

    public override void DisableMovement() {
        currentWeapon.canFire = false;
    }

    public override void EnableMovement() {
        throw new NotImplementedException();
    }

    public override void Init() {
        throw new NotImplementedException();
    }


}
