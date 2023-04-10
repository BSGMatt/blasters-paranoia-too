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

    /// <summary>
    /// Used for enemies; more important 
    /// buildings will be targeted over less important ones. 
    /// </summary>
    public int importance;

    /// <summary>
    /// The value this building will set a character's property to,
    /// if applicable. 
    /// </summary>
    /// <returns></returns>
    public float modifierValue;

    /// <summary>
    /// Represents the amount this building increase or decrease a resource,
    /// such as cash, xp, etc.
    /// </summary>
    public int resourceAmount;

    /// <summary>
    /// The type of resource this building modifies the amount of.
    /// </summary>
    public string resource;

    /// <summary>
    /// The maximum distance that characters will be detected by this building. 
    /// </summary>
    public float range;

    /// <summary>
    /// The weapon it will create when deployed, if applicable. 
    /// </summary>
    public WeaponCard weaponCard;

    public override string infoText() {
        StringBuilder sb = new StringBuilder();

        sb.Append("Cost to Purchase: " + unlockPrice + "\n");
        sb.Append("Build Cost: " + buildPrice + "\n");
        sb.Append("Max Health: " + maxHealth + "\n");

        if (!resource.Equals(""))
            sb.Append("Increases " + resource + "by " + resourceAmount + "per second.\n");

        //If this building has a weapon, show its stats. 
        if (weaponCard != null)
            sb.Append(weaponCard.weaponStatsOnly() + "\n");

        return sb.ToString();
    }
}
