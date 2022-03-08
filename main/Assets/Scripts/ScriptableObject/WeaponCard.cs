using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Weapon Card", menuName = "Card/Weapon Card")]
public class WeaponCard : Card
{
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
        sb.Append("Max Ammo: " + maxAmmo + "\n");
        sb.Append("Damage/Pellet: " + dmgPerPellet + "\n");
        sb.Append("Pellet Count: " + pelletCount + "\n");
        
        if (minSpread == maxSpread) {
            sb.Append("Spread: " + minSpread);
        }
        else {
            sb.Append("Spread Range: " + minSpread + " - " + maxSpread);
        }

        sb.Append("Pellet Speed: " + pelletSpeed + "\n");
        sb.Append("Fire Rate: " + fireRate + "\n");
        sb.Append("Reload Speed: " + reloadSpeed + "\n");

        return sb.ToString();
    }
}
