using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Character
{
    public const float tickRate = 0.125f;

    public BuildingCard card;
    public int age;

    private bool shouldAge = true;

    public abstract void Passive();

}
