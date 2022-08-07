using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : Weapon
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
        if (canFire && firing == null) {
            firing = StartCoroutine(FireCoroutine());
        }

        if (!canFire) {
            StopCoroutine(firing);
            firing = null;
        }
    }

    private void FixedUpdate() {
        rb.position = host.GetRigidBody().position;
        AimAtTarget();
    }

    protected override void Fire() {
        CreatePellet(card, angle + RandomSpreadValue());
        ammo--;
    }
}
