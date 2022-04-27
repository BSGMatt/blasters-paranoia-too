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

    public void BaseInit() {
        maxHP = card.maxHealth;
        hp = maxHP;

        shouldAge = true;
        aging = StartCoroutine(AgeBuilding());
    }

    public abstract void Passive();

    /// <summary>
    /// Iterate through every building delpoyed 
    /// and increase their age every clock tick. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator AgeBuilding() {
        while (shouldAge) {
            yield return new WaitForSeconds(Character.tickRate);

            age++;
        }

        aging = null;

        yield return 0;
    }
}
