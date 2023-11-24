using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hazard Card", menuName = "Card/Hazard Card")]
public class HazardCard : Card
{
    public int minDamage;
    public int maxDamage;
    public float minSize;
    public float maxSize;
    public float duration;
    public bool isEnemy;

    public override string infoText() {
        return "(Hazard): " + name + "; Damage: " + maxDamage + "; Min. Size: " + minSize + "; Max. Size: " + maxSize + "; Duration: " + duration;
    }
}
