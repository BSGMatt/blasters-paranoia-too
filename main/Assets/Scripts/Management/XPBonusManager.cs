using UnityEngine;

/// <summary>
/// Script that keeps track of everything needed to calculate XP bonuses.
/// </summary>
public class XPBonusManager : MonoBehaviour
{
    /// <summary>
    /// The amount of XP rewarded for killing an enemy.
    /// </summary>
    public int xpBonusForKilling = 100;

    /// <summary>
    /// The amount of XP rewarded for buying items at the shop.
    /// </summary>
    public int xpBonusForBuying = 10;

    /// <summary>
    /// The amount of XP rewarded for surviving a wave
    /// </summary>
    public int xpBonusForSurviving = 300;

    /// <summary>
    /// The amount of XP rewarded for buildings that stayed alive during the last wave. 
    /// </summary>
    public int xpBonusForBuildingsAlive = 10;

    /// <summary>
    /// The amount of XP rewarded for killing the boss. 
    /// </summary>
    public int xpBonusForBoss = 500;


    /// <summary>
    /// The character to give the xp bonuses to. 
    /// </summary>
    public Character player;

    private GameManager gm;
    private Builder builder;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        builder = gm.builder.GetComponent<Builder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Adds all of the xp bonuses to the player's total xp. 
    /// </summary>
    public void ApplyXPBonuses() {
        int totalXPBonuses = 0;

        //Add amount of enemies killed
        totalXPBonuses += gm.spawnManager.totalEnemiesInWave * xpBonusForKilling;
        totalXPBonuses += xpBonusForSurviving;
        foreach (Building b in builder.buildingsDeployed) {
            totalXPBonuses += xpBonusForBuildingsAlive * b.age;
        }

        if (gm.IsBossWave()) totalXPBonuses += xpBonusForBoss;

        player.xp += totalXPBonuses;
    }
}
