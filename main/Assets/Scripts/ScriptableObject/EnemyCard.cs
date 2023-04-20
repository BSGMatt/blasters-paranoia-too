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
    public Color color = Color.white;

    public float minDistanceFromTarget = 4;

    public float maxDistanceFromTarget = 6;

    public override string infoText() {
        return "Enemy Name: " + name + ", " + difficulty;
    }
}
