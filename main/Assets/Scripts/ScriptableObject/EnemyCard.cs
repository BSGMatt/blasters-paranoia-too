using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Card", menuName = "Card/Enemy Card")]
public class EnemyCard : Card
{
    public float speed;
    public int maxHP;
    public float difficulty;
    public WeaponCard weapon;
    public Approach approach;
    public TargetType targetType;

    //The enemy to spawn upon death. 
    public EnemyCard child;

    //The delay between when an enemy dies and when the child is spawned. 
    public float spawnDelay;

    //The hazard that this enemy may spawn. 
    public HazardCard hazard;

    //The number of instances of the gameObject to spawn. 
    public int childCount;

    //The distance from the enemy's location where the children will spawn. 
    public float childDistance;

    public Color color = Color.white;

    public float minDistanceFromTarget = 4;

    public float maxDistanceFromTarget = 6;

    public override string infoText() {
        return "Enemy Name: " + name + ", " + difficulty;
    }
}
