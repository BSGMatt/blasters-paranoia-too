using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BB : Character
{
    public WeaponLoader wLoader;

    // Start is called before the first frame update
    Vector2 movingDirection;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        DetermineDirection();

        WeaponInventory();
    }

    private void DetermineDirection() {
        //Get the X and Y components of the BB's movement from the player's input. 
        float moveX = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));

        float moveY = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        movingDirection = new Vector2(moveX, moveY);
    }

    private void WeaponInventory() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            wLoader.PreviousWeapon();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            wLoader.NextWeapon();
        }
    }

    private void FixedUpdate() {
        PlayerMovement();
    }

    //Method for reading input and making the player move. 
    private void PlayerMovement() {
        if (movementEnabled) c2d.Move(movingDirection, maxSpeed);
    }

    public override void Die() {
        Debug.Log("BB is dead.");
    }

    public override void Init() {
        SetHP(maxHP);
        isEnemy = false;
    }

    public override void DisableMovement() {
        c2d.StopMoving();
        SetMovementEnabled(false);
    }

    public override void EnableMovement() {
        SetMovementEnabled(true);
    }




}
