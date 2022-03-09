using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyPistol : Weapon
{


    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        isEnemy = false;
        ammo = card.maxAmmo;
        cam = FindObjectOfType<CameraMan>().GetCamera();
        host.currentWeapon = this;
    }

    // Update is called once per frame
    void Update()
    {
        rb.position = host.GetRigidBody().position;

        AimWithMouse();

        //Check if the player pressed the reload key. 
        if (Input.GetKeyDown(KeyCode.R)) {
            if (reloading != null) {
                reloading = StartCoroutine(Reload());
            }
        }

        //Fire a pellet if player left-clicks. 
        if (Input.GetMouseButtonDown(0)) {
            if (canFire && ammo > 0) Fire();
        }
    }

    //Launches a pellet. 
    protected override void Fire() {
        CreatePellet(card, angle);
        ammo--;
    }
}
