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

    public int NextLevel() {
        currentLevel++;

        AssignValuesToLevel();

        return xpThresholds[currentLevel];
    }

    private void AssignValuesToLevel() {
        player.level = currentLevel;
        player.xp = 0;
    }

}
