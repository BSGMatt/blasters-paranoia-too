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

    //Over time coroutines
    private Coroutine heal;
    private Coroutine stam;
    private Coroutine dmgboost;
    private Coroutine spdboost;

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

    /// <summary>
    /// Attemps to apply a power up's effects on the player.
    /// </summary>
    /// <param name="card"></param>
    /// <returns>Returns false iff the powerup is an over time powerup (e.g. heal/stamina and stat boosts), and there is an over time affect of the same still being applied. Otherwise, the function
    /// returns true. </returns>
    public bool ApplyPWEffect(PowerUpCard card) {
        if (card == null) {
            Debug.LogWarning("@ApplyPWEffect: card was null.");
            return false;
        }
        Debug.Log(card.infoText());

        switch (card.effect) {
            case PWEffect.InstantHealth:
                player.hp += (int) card.amount;
                break;
            case PWEffect.InstantStamina:
                player.stamina += (int)card.amount;
                break;
            case PWEffect.DamageBoost:
                if (dmgboost != null) return false;
                DamageBoostEffect(card.amount, card.duration);
                break;
            case PWEffect.SpeedBoost:
                if (spdboost != null) return false;
                SpeedBoostEffect(card.amount, card.duration);
                break;
            case PWEffect.HealOverTime:
                if (heal != null) return false;
                HealOverTime(card.amount, card.duration);
                break;
            case PWEffect.StaminaOverTime:
                if (stam != null) return false;
                StaminaOverTime(card.amount, card.duration);
                break;
            default:
                player.sheilds += (int)card.amount;
                break;
        }

        return true;
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

    private IEnumerator DamageBoostEffect(float amount, float duration) {
        yield return 0;
    }

    private IEnumerator SpeedBoostEffect(float amount, float duration) {
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
