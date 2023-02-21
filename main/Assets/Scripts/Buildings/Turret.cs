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
    }

    // Update is called once per frame
    void Update() {
        
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
