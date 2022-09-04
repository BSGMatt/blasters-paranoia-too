using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Boss
{
    // Start is called before the first frame update

    private bool chargingInit = false;
    private bool releaseInit = false;

    private void Awake() {
        base.Awake();
    }

    void Start()
    {
        base.Start();
        aiController.InitPath(0);

        currentWeapon = Instantiate<GameObject>(weapons[0].prefab, transform.position, Quaternion.identity).GetComponent<Weapon>();
        currentWeapon.card = weapons[0];
        currentWeapon.host = this;
        currentWeapon.ammo = currentWeapon.card.maxAmmo;

        movementEnabled = true;
        currentWeapon.canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        if (hp <= 0) Die();
    }

    public override void CalmState() {

        if (!movementEnabled) EnableMovement();
        if (aiController.target == null) aiController.ResetPath();

        //Keep following path until the end has been reached. 
        if (!aiController.reachedEndOfPath && movementEnabled) {
            aiController.RunPath();
        }

        if (aiController.reachedEndOfPath) {

            if (Vector2.Distance(transform.position, aiController.target.position) >= aiController.maxDistance) {
                aiController.InitPath(targetType);
            }
        }

        //if we're returning to our calm state, reset anything we changed in the previous state. 
        if (prevState != -1) {
            Destroy(currentWeapon.gameObject);
            currentWeapon = Instantiate<GameObject>(weapons[0].prefab, transform.position, Quaternion.identity).GetComponent<Weapon>();
            currentWeapon.card = weapons[0];
            currentWeapon.host = this;

            /*Debug.Log(currentWeapon.host);
            Debug.Log(currentWeapon.card);*/

            currentWeapon.canFire = true;
            currentWeapon.ammo = currentWeapon.card.maxAmmo;
            Debug.Log(currentWeapon.target);
            prevState = -1;
        }
    }

    public override void ChargingState() {
        if (!chargingInit) {
            DisableMovement();
            Destroy(currentWeapon.gameObject);
            currentWeapon = Instantiate<GameObject>(weapons[1].prefab, transform.position, Quaternion.identity).GetComponent<Weapon>();
            currentWeapon.card = weapons[1];
            currentWeapon.host = this;

            Debug.Log(currentWeapon.host);
            Debug.Log(currentWeapon.card);

            currentWeapon.canFire = false;
            currentWeapon.ammo = currentWeapon.card.maxAmmo;
            chargingInit = true;
            return;
        }

    }

    public override void ReleaseState() {

        Debug.Log(currentWeapon.host);
        Debug.Log(currentWeapon.card);

        currentWeapon.canFire = true;
        chargingInit = false;
    }

    public override void Die() {
        bossDied.Invoke();
        CommonDieMethod();
    }

    public override void DisableMovement() {
        aiController.StopPath();
        if (currentWeapon != null) currentWeapon.canFire = false;
        movementEnabled = false;
    }

    public override void EnableMovement() {
        if (!aiController.reachedEndOfPath) {
            aiController.StopPath();
        }
        aiController.InitPath(0);

        //Check if the enemy was allowed to fire their weapon before stopping
        if (state != 1) currentWeapon.canFire = true;
        movementEnabled = true;
    }

    public override void Init() {
        
    }
  
}
