using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Character player;
    public Slider xpBar;
    public Slider staminaBar;
    public Slider healthBar;
    public Slider sheildsBar;

    public int currentXPThreshold;

    /// <summary>
    /// set the min and max values of every status bar. 
    /// </summary>
    public void Start() {
        healthBar.minValue = 0;
        staminaBar.minValue = 0;

        healthBar.maxValue = player.maxHP;
        staminaBar.maxValue = player.maxStamina;

        sheildsBar.minValue = 0;
        sheildsBar.maxValue = player.maxHP;

        xpBar.maxValue = 0;
        xpBar.maxValue = player.maxHP;
    }

    public void Update() {
        UpdateStatusBars();    
    }

    public void UpdateStatusBars() {
        healthBar.value = player.hp;
        staminaBar.value = player.stamina;
        sheildsBar.value = player.sheilds;
        xpBar.value = player.xp;
    }

    public void LevelUp() {

    }

    public void ApplyPWEffect(PowerUpCard card) {

    }

    public void StartHealOverTime() {

    }

    public void StartStaminaOverTime() {

    }

    private IEnumerator HealOverTime(float ammount, float duration) {
        yield return 0;
    }

    private IEnumerator StaminaOverTime(float ammount, float duration) {
        yield return 0;
    }

    private IEnumerator WaitToPassivelyHeal() {
        yield return 0;
    }

    private IEnumerator PassiveHeal() {
        yield return 0;
    }

    private IEnumerator PassiveStaminaRegen() {
        yield return 0;
    }
}
