using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public const int MAX_LEVELS = 100;

    public int currentLevel;
    public int[] xpThresholds;
    private Character player;

    public LevelManager(Character c) {
        currentLevel = 0;
        player = c;
        player.level = currentLevel;
        xpThresholds = new int[MAX_LEVELS];

        for (int i = 0; i < xpThresholds.Length; i++) {
            xpThresholds[i] = Mathf.RoundToInt(448 * Mathf.Exp(0.115f * (i + 1)));
        }
    }

    /// <summary>
    /// Increases player level and adjusts stats accordingly. 
    /// </summary>
    /// <returns></returns>
    public int NextLevel() {

        //If there is more xp reward than what was need to level up, 
        //add the remaining xp after leveling up. 
        int leftoverXP = player.xp - xpThresholds[currentLevel];
        currentLevel++;
        AssignValuesToLevel(leftoverXP);

        return xpThresholds[currentLevel];
    }

    /// <summary>
    /// modifies the player's stats based on the current level. 
    /// </summary>
    /// <param name="remainder">the amount of xp that needs to be added back after leveling up.</param>
    private void AssignValuesToLevel(int remainder) {
        player.level = currentLevel;
        player.xp = remainder;
    }

}
