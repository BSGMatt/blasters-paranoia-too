using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyDummy : Character {

    public void Start() {
        isEnemy = false;
    }

    public override void Die() {
        Debug.Log("Die");
        CommonDieMethod();
    }

    public override void DisableMovement() {
        throw new System.NotImplementedException();
    }

    public override void EnableMovement() {
        throw new System.NotImplementedException();
    }
}
