using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : Weapon
{

    // Start is called before the first frame update
    void Start()
    {
        isEnemy = true;
        firing = null;
        GrabHostTarget();
        CommonStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire && firing == null)
        {
            firing = StartCoroutine(FireCoroutine());
        }

        if (!canFire)
        {
            if (firing != null) StopCoroutine(firing);
            firing = null;
        }
    }

    void FixedUpdate()
    {
        rb.position = host.GetRigidBody().position;
        //Debug.Log("Enemy Shotgun angle: " + AimAtTarget());
        AimAtTarget();
    }

    protected override void Fire()
    {

        float currentAngle = angle - card.maxSpread;

        //Fire pelletCount pellets at once, with each pellet traveling at different angles. 
        for (int i = 0; i < card.pelletCount; i++) {

            CreatePellet(card, currentAngle);

            currentAngle += (card.maxSpread * 2 / card.pelletCount);
        }

        ammo--;
    }
}
