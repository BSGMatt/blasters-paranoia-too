using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character {

    public void Start() {
        isEnemy = true;
        hp = maxHP;
    }

    public override void Die() {
        CommonDieMethod();
    }

    public override void DisableMovement() {
        throw new System.NotImplementedException();
    }

    public override void EnableMovement() {
        throw new System.NotImplementedException();
    }
}
