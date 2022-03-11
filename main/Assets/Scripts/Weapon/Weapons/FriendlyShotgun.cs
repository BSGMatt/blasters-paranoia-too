using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShotgun : Weapon
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

    protected override void Fire() {

        float currentAngle = angle - card.maxSpread;

        //Fire pelletCount pellets at once, with each pellet traveling at different angles. 
        for (int i = 0; i < card.pelletCount; i++) {

            CreatePellet(card, currentAngle);

            currentAngle += (card.maxSpread * 2 / card.pelletCount);
        }

        ammo--;
    }
}
