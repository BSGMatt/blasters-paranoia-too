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
        CommonStart();
    }

    // Update is called once per frame
    void Update()
    {
        CommonUpdate();
    }

    public void FixedUpdate() {
        CommonFixedUpdate();
    }

    //Launches a pellet. 
    protected override void Fire() {
        CreatePellet(card, angle);
        ammo--;
    }
}
