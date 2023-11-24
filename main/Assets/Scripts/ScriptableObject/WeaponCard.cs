using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Weapon Card", menuName = "Card/Weapon Card")]
public class WeaponCard : Card
{
    public GameObject pelletType;

    public int maxAmmo;
    public int dmgPerPellet;
    public int pelletCount;
    public float minSpread;
    public float maxSpread;
    public float pelletSpeed;
    public float fireRate;
    public float reloadSpeed;

    public override string infoText() {
        StringBuilder sb = new StringBuilder();

        sb.Append("Cost to Purchase: " + unlockPrice + "\n");
        
        sb.Append(weaponStatsOnly());

        return sb.ToString();
    }

    public string weaponStatsOnly() {

        StringBuilder sb = new StringBuilder();

        sb.Append("Max Ammo: " + maxAmmo + " pellets\n");
        sb.Append("Damage per Pellet: " + dmgPerPellet + "\n");
        sb.Append("Pellet Count: " + pelletCount + "\n");

        sb.Append(string.Format("Fire Rate: {0:f2} pellets per second\n", (1/fireRate)));

        if (minSpread == maxSpread) {
            sb.Append("Spread: " + minSpread + "°\n");
        }
        else {
            sb.Append("Spread Range: " + minSpread + "° - " + maxSpread + "°\n");
        }

        sb.Append("Pellet Speed: " + pelletSpeed + " units per second\n");
        sb.Append("Reload Speed: " + reloadSpeed + " units per second\n");

        return sb.ToString();
    }
}
