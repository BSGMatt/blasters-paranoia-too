using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour {

    private const float tickRate = 0.125f;
    private LevelManager lm;

    public Character player;
    public Slider xpBar;
    public Slider staminaBar;
    public Slider healthBar;
    public Slider sheildsBar;

    public Text levelCounterText;

    public int currentXPThreshold;
    public int playerLevel;

    //Over time coroutines
    private Coroutine heal;
    private Coroutine stam;
    private Coroutine dmgboost;
    private Coroutine spdboost;
    private Coroutine passiveHeal;
    private Coroutine waitToHeal;
    private Coroutine passiveStam;

    /// <summary>
    /// set the min and max values of every status bar. 
    /// </summary>
    public void Start() {
        lm = new LevelManager(player);

        currentXPThreshold = lm.xpThresholds[lm.currentLevel];

        healthBar.minValue = 0;
        staminaBar.minValue = 0;

        healthBar.maxValue = player.maxHP;
        staminaBar.maxValue = player.maxStamina;

        sheildsBar.minValue = 0;
        sheildsBar.maxValue = player.maxHP;

        xpBar.maxValue = 0;
        xpBar.maxValue = currentXPThreshold;

        player.takenDamage.AddListener(StopHealingPassive);
    }

    public void Update() {
        UpdateStatusBars();

        AddXP();
    }

    public void UpdateStatusBars() {
        healthBar.value = player.hp;
        staminaBar.value = player.stamina;
        sheildsBar.value = player.sheilds;
        xpBar.value = player.xp;
        levelCounterText.text = "Level: " + (player.level + 1);
    }

    private void AddXP() {
        if (Input.GetKeyDown(KeyCode.P)) {
            player.xp += 100;
        }

        if (player.xp >= currentXPThreshold) {
            LevelUp();
        }
    }

    public void LevelUp() {

        Debug.Log("Level Up! You're Winner!");

        currentXPThreshold = lm.NextLevel();

        xpBar.maxValue = 0;
        xpBar.maxValue = currentXPThreshold;
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
                player.hp += (int)card.amount;
                break;
            case PWEffect.InstantStamina:
                player.stamina += (int)card.amount;
                break;
            case PWEffect.DamageBoost:
                if (dmgboost != null) return false;
                dmgboost = StartCoroutine(DamageBoostEffect(card.amount, card.duration));
                break;
            case PWEffect.SpeedBoost:
                if (spdboost != null) return false;
                spdboost = StartCoroutine(SpeedBoostEffect(card.amount, card.duration));
                break;
            case PWEffect.HealOverTime:
                if (heal != null) return false;
                heal = StartCoroutine(HealOverTime(card.amount, card.duration));
                break;
            case PWEffect.StaminaOverTime:
                if (stam != null) return false;
                stam = StartCoroutine(StaminaOverTime(card.amount, card.duration));
                break;
            default:
                player.sheilds += (int)card.amount;
                break;
        }

        return true;
    }

    private IEnumerator HealOverTime(float amount, float duration) {

        float t = 0;
        int ticks = 0;
        int countUp = (int) amount % (int) (duration / tickRate);

        while (t < duration) {

            player.hp += (int) amount / (int) (duration / tickRate);
            if (ticks % countUp == 0) player.hp += 1;

            t += tickRate;
            ticks++;
            yield return new WaitForSeconds(tickRate);
        }

        heal = null;

        yield return 0;
    }

    private IEnumerator StaminaOverTime(float amount, float duration) {

        float t = 0;
        int ticks = 0;
        int countUp = (int)amount % (int)(duration / tickRate);

        while (t < duration) {

            player.stamina += (int)amount / (int)(duration / tickRate);
            if (ticks % countUp == 0) player.stamina += 1;

            t += tickRate;
            ticks++;
            yield return new WaitForSeconds(tickRate);
        }

        stam = null;

        yield return 0;
    }

    private IEnumerator DamageBoostEffect(float amount, float duration) {
        player.currentDmgDealtModValue = amount;
        bool ok = false;
        while (!ok) {
            yield return new WaitForSeconds(duration);

            ok = true;
        }

        player.currentDmgDealtModValue = 1f;

        dmgboost = null;

        yield return 0;
    }

    private IEnumerator SpeedBoostEffect(float amount, float duration) {
        player.currentSpeedModValue = amount;
        bool ok = false;
        while (!ok) {
            yield return new WaitForSeconds(duration);

            ok = true;
        }

        player.currentSpeedModValue = 1f;

        spdboost = null;

        yield return 0;
    }

    private IEnumerator WaitToPassivelyHeal() {
        yield return 0;
    }

    private IEnumerator PassiveHeal() {
        yield return 0;
    }

    /// <summary>
    /// Stops the current healing passive and starts the timer that waits to start healing again. 
    /// </summary>
    public void StopHealingPassive() {
        if (passiveHeal != null) {
            StopCoroutine(passiveHeal);
        }

        if (waitToHeal != null) {
            StopCoroutine(waitToHeal);
        }

        waitToHeal = StartCoroutine(WaitToPassivelyHeal());
    }

    private IEnumerator PassiveStaminaRegen() {
        yield return 0;
    }
}
