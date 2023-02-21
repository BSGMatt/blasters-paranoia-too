using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Character
{

    public BuildingCard card;
    public int age;

    public GameObject healthBar;

    private Coroutine aging;

    //Whether the aging Coroutine should be active. 
    public bool shouldAge;

    /// <summary>
    /// initalizes the building. 
    /// </summary>
    public void BaseInit() {
        maxHP = card.maxHealth;
        hp = maxHP;

        shouldAge = true;
        aging = StartCoroutine(AgeBuilding());
    }

    /// <summary>
    /// do something every frame while this building is active. 
    /// </summary>
    public abstract void Passive();

    /// <summary>
    /// Iterate through every building delpoyed 
    /// and increase their age every clock tick. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator AgeBuilding() {
        while (shouldAge) {
            //increase the age of the buildings every 6 seconds. (age 10/min) 
            yield return new WaitForSeconds(Character.tickRate * 48);

            age++;
        }

        aging = null;

        yield return 0;
    }
}
