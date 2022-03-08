using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/**
 * A type of card that contains information specfic to buildings. 
 * 
 */
[CreateAssetMenu(fileName = "New Building Card", menuName = "Card/Building Card")]
public class BuildingCard : Card
{
    public int buildPrice; //Price to build. 
    public int maxHealth; //The amount of hp the object has. 

    public override string infoText() {
        StringBuilder sb = new StringBuilder();

        sb.Append("Cost to Purchase: " + unlockPrice + "\n");
        sb.Append("Build Cost: " + buildPrice + "\n");
        sb.Append("Max Health: " + maxHealth + "\n");

        return sb.ToString();
    }
}
