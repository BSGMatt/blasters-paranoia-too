using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Character
{

    public BuildingCard card;
    public int age;

    public abstract void Passive();

}
