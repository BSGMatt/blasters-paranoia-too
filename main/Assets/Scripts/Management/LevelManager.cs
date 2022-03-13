using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public const int MAX_LEVELS = 100;

    public int currentLevel;
    public int[] xpThresholds;

    public void GenerateXPThreshold() {
        xpThresholds = new int[MAX_LEVELS];

        for (int i = 0; i < xpThresholds.Length; i++) {
            xpThresholds[i] = Mathf.RoundToInt(448 * Mathf.Exp(0.115f * (i + 1)));
        }
    }

    public void AssignValuesToLevel() {

    }

}
