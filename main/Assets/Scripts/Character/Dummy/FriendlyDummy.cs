using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FriendlyDummy : Character {

    public void Start() {
        isEnemy = false;
    }

    public override void Die() {
        Debug.Log("Die");
        CommonDieMethod();
    }

    public override void DisableMovement() {
        movementEnabled = false;
    }

    public override void EnableMovement() {
        movementEnabled = true;
    }

    //Method for reading input and making the player move. 
    private void PlayerMovement() {
        //Get the X and Y components of the BB's movement from the player's input. 
        float moveX = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));

        float moveY = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        Vector2 movingDirection = new Vector2(moveX, moveY);
        
        c2d.Move(movingDirection, maxSpeed * currentSpeedModValue);
    }
}
