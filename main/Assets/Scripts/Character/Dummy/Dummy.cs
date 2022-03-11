using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character {

    public void Start() {
        Init();
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

    public override void Init() {
        isEnemy = true;
        hp = maxHP;
    }
}
