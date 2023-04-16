using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BB : Character
{
    public WeaponLoader wLoader;
    public float def_dashForce = 5f;
    public float dashDuration = 1f;

    private Coroutine dash = null;
    private float dashForce = 0;

    // Start is called before the first frame update
    private Vector2 movingDirection;

    void Start()
    {
        //hp = maxHP; //I have this set for testing purposes. should be hp = maxHP;
        isEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetermineDirection();

        if (wLoader.isInit) WeaponInventory();

        CheckToUseAPowerUp();
    }

    private void DetermineDirection() {
        //Get the X and Y components of the BB's movement from the player's input. 
        float moveX = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));

        float moveY = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        movingDirection = new Vector2(moveX, moveY);

        //If player wants to dash
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (dash == null && stamina > 0) dash = StartCoroutine(Dash());
        }
    }

    private void WeaponInventory() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            wLoader.PreviousWeapon();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            wLoader.NextWeapon();
        }
    }

    /// <summary>
    /// Check if the player pressed on of the hotbar buttons to use an item. 
    /// </summary>
    private void CheckToUseAPowerUp() {
        int pcSlot = FindObjectOfType<InventoryManager>().HotBarButtonPressed();
        if (pcSlot != -1) {
            statsManager.ApplyPWEffect(FindObjectOfType<InventoryManager>().powerups.Pop(pcSlot));
        }
    }

    private IEnumerator Dash() {

        float dashTime = 0;

        //The mult and i make it so that the dash accelerates to a maxium and then
        //decreases smoothly, making a smoother looking dash. 
        float mult = Mathf.PI / (dashDuration / 0.1f);
        int i = 0;

        SpriteRenderer sp = GetComponent<SpriteRenderer>();

        stamina -= dashCost;

        invincible = true;

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0.5f);
        while (dashTime < dashDuration) {
            dashForce = def_dashForce * Mathf.Sin(mult * i);

            dashTime += 0.1f;
            i++;

            yield return new WaitForSeconds(0.1f);
        }

        invincible = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);

        if (stamina < 0) stamina = 0;

        dash = null;

        yield return 0;
    }

    private void FixedUpdate() {
        PlayerMovement();
    }

    //Method for reading input and making the player move. 
    private void PlayerMovement() {
        if (movingDirection != Vector2.zero) startedMoving.Invoke();

        if (movementEnabled) c2d.Move(movingDirection, maxSpeed * currentSpeedModValue);

        if (dash != null) {
            rb.AddForce(movingDirection * dashForce, ForceMode2D.Force);
        }
    }

    public override void Die() {
        dead = true;
        Debug.Log("BB is dead.");
    }

    public override void DisableMovement() {
        c2d.StopMoving();
        SetMovementEnabled(false);
    }

    public override void EnableMovement() {
        SetMovementEnabled(true);
    }

}
