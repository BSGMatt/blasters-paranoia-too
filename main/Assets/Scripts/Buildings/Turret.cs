using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for buildings with weapons. 
/// </summary>
public class Turret : Building
{

    public DetectionTrigger rangeDetectionTrigger;

    // Start is called before the first frame update
    void Start() {
        BaseInit();

        rangeDetectionTrigger.trigger.radius = card.range;
        rangeDetectionTrigger.listUpdateEvent.AddListener(UpdateWeaponTarget);

        //Spawn in the turret's weapon.
        currentWeapon = GameObject.Instantiate(card.weaponCard.prefab, transform).GetComponent<Weapon>();
        currentWeapon.host = this;
        currentWeapon.card = card.weaponCard;
        isEnemy = false;
    }

    // Update is called once per frame
    void Update() {

        if (currentWeapon.target == null) currentWeapon.canFire = false;
    }

    public override void Die() {

        rangeDetectionTrigger.listUpdateEvent.RemoveListener(UpdateWeaponTarget);
        CommonDieMethod();
    }

    /// <summary>
    /// Update the turret weapon's target. 
    /// </summary>
    private void UpdateWeaponTarget() {
        currentWeapon.GetClosestTargetInList(rangeDetectionTrigger.characterList);

        if (currentWeapon.target != null) currentWeapon.canFire = true;
        Debug.Log("Turret's new target: " + currentWeapon.target);
    }

    public override void DisableMovement() {
        return;
    }

    public override void EnableMovement() {
        return;
    }

    public override void Passive() {
        return;
    }



}
