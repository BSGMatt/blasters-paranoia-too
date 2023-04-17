using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAutomaticWeapon : Weapon
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Don't do anything if the player is trying to access the shop. 
        if (FindObjectOfType<GameManager>().shopEnabled || FindObjectOfType<GameManager>().builderEnabled) {
            return;
        }

        if (target != null) AimAtTarget();

        if (canFire && firing == null) {
            firing = StartCoroutine(FireCoroutine());
        }

        if (!canFire) {
            StopCoroutine(firing);
            firing = null;
        }
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
