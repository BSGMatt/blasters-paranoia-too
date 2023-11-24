using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour {


    private LevelManager lm;

    public Character player;
    public Slider xpBar;
    public Slider staminaBar;
    public Slider healthBar;
    public Slider sheildsBar;

    public Text levelCounterText;

    public int currentXPThreshold;
    public int playerLevel;

    public int healSpeed = 2;
    public int staminaSpeed = 1;

    public bool passiveHealEnabled = true;

    //Lists to store all active over time effects. 
    private List<StatusEffectInfo> heals;
    private List<StatusEffectInfo> stams;
    private List<StatusEffectInfo> dmgboosts;
    private List<StatusEffectInfo> spdboosts;


    private Coroutine passiveHeal;
    private Coroutine waitToHeal;

    private Coroutine passiveStam;
    private Coroutine waitToStam;

    /// <summary>
    /// set the min and max values of every status bar. 
    /// </summary>
    public void Start() {
        lm = new LevelManager(player);

        currentXPThreshold = lm.xpThresholds[lm.currentLevel];

        healthBar.minValue = 0;
        healthBar.maxValue = player.maxHP;

        heals = new List<StatusEffectInfo>();
        stams = new List<StatusEffectInfo>();
        dmgboosts = new List<StatusEffectInfo>();
        spdboosts = new List<StatusEffectInfo>();

        //Only do the following if the character referenced uses the non-health stats.
        if (!player.onlyHasHealth) {
            staminaBar.minValue = 0;
            staminaBar.maxValue = player.maxStamina;

            sheildsBar.minValue = 0;
            sheildsBar.maxValue = player.maxHP;

            xpBar.maxValue = 0;
            xpBar.maxValue = currentXPThreshold;

            player.startedMoving.AddListener(StopStaminaPassive);
            waitToStam = StartCoroutine(WaitToRegenStamina());
        }

        player.takenDamage.AddListener(StopHealingPassive);
        player.characterDied.AddListener(StopAllHeals);
        if (passiveHealEnabled) waitToHeal = StartCoroutine(WaitToPassivelyHeal());

    }

    public void Update() {
        UpdateStatusBars();

        if (player.xp >= currentXPThreshold && player.isLevelable) {
            LevelUp();
        }
    }

    public void UpdateStatusBars() {
        //Debug.Log(player);
        healthBar.value = player.hp;
        healthBar.maxValue = player.maxHP;

        if (player.onlyHasHealth) return;

        staminaBar.value = player.stamina;
        sheildsBar.value = player.sheilds;
        xpBar.value = player.xp;
        levelCounterText.text = "Level: " + (player.level + 1);
    }

    public void LevelUp() {

        Debug.Log("Level Up! You're Winner!");
        FindObjectOfType<GameManager>().audioManager.Play("LevelUp");

        currentXPThreshold = lm.NextLevel();

        xpBar.minValue = player.xp;
        xpBar.maxValue = currentXPThreshold;
    }

    /// <summary>
    /// Adds xp to the assigned character
    /// </summary>
    /// <param name="amount"></param>
    public void AddXP(int amount) {
        player.xp += amount;
    }

    /// <summary>
    /// Attemps to apply a power up's effects on the player.
    /// </summary>
    /// <param name="card"></param>
    /// <returns>If the power-up has an over-time effect, then a StatusEffectInfo object is returned.
    /// Otherwise, the function returns null. </returns>
    public StatusEffectInfo ApplyPWEffect(PowerUpCard card) {
        if (card == null) {
            Debug.LogWarning("@ApplyPWEffect: card was null.");
            return null;
        }
        Debug.Log(card.infoText());
        
        switch (card.effect) {
            case PWEffect.InstantHealth:
                player.hp += (int)card.amount;
                return null;
            case PWEffect.InstantStamina:
                player.stamina += (int)card.amount;
                return null;
            case PWEffect.Sheild:
                player.sheilds += (int)card.amount;
                return null;
            case PWEffect.DamageBoost:
                StatusEffectInfo dmgboost = new StatusEffectInfo(card, dmgboosts);
                dmgboosts.Add(dmgboost);
                dmgboost.coroutine = StartCoroutine(DamageBoostEffect(dmgboost));
                dmgboost.character = player;
                return dmgboost;
            case PWEffect.SpeedBoost:
                StatusEffectInfo spdboost = new StatusEffectInfo(card, spdboosts);
                spdboosts.Add(spdboost);
                spdboost.coroutine = StartCoroutine(SpeedBoostEffect(spdboost));
                spdboost.character = player;
                return spdboost;
            case PWEffect.HealOverTime:
                StatusEffectInfo heal = new StatusEffectInfo(card, heals);
                heals.Add(heal);
                heal.coroutine = StartCoroutine(HealOverTime(heal));
                heal.character = player;
                return heal;
            case PWEffect.StaminaOverTime:
                StatusEffectInfo stam = new StatusEffectInfo(card, stams);
                stams.Add(stam);
                stam.coroutine = StartCoroutine(StaminaOverTime(stam));
                stam.character = player;
                return stam;
        }

        return null;
    }

    /*
        Updates the player's current modifcation value. 
    */
    public void RevaluateModiferValue(PWEffect effect) {

        float spd = 1f;
        float dmg = 1f;

        foreach (StatusEffectInfo s in dmgboosts) {
            dmg *= s.card.amount;
        };
        foreach (StatusEffectInfo s in spdboosts) {
            spd *= s.card.amount;
        }

        if (effect == PWEffect.DamageBoost) {
            player.currentDmgDealtModValue = dmg;
        }
        else if(effect == PWEffect.SpeedBoost) {
            player.currentSpeedModValue = spd;
        }
        
    }

    private IEnumerator HealOverTime(StatusEffectInfo effectInfo) {

        float duration = effectInfo.card.duration;
        float tickRate = Character.tickRate;

        //Cause an infinite loop whenever the duration is negative.
        if (effectInfo.card.duration < 0){
            duration = -effectInfo.card.duration;
            tickRate = -Character.tickRate;
        }
        float t = 0;
        int ticks = 0;
        int countUp = (int) effectInfo.card.amount % (int) (duration / Character.tickRate);

        while (t < duration) {

            //interpolate intermediate healing values to avoid healing decimal amounts. 
            player.hp += (int) effectInfo.card.amount / (int) (duration / Character.tickRate);
            if (ticks % countUp == 0) player.hp += 1;

            t += tickRate;
            ticks++;

            if (player.hp > player.maxHP) player.hp = player.maxHP;

            yield return new WaitForSeconds(Character.tickRate);
        }

        effectInfo.coroutine = null;
        effectInfo.list.Remove(effectInfo);
        

        yield return 0;
    }

    private IEnumerator StaminaOverTime(StatusEffectInfo effectInfo) {

        float duration = effectInfo.card.duration;
        float tickRate = Character.tickRate;
        if (effectInfo.card.duration < 0){
            duration = -effectInfo.card.duration;
            tickRate = -Character.tickRate;
        }
        float t = 0;
        int ticks = 0;
        int countUp = (int) effectInfo.card.amount % (int) (duration / Character.tickRate);

        while (t < duration) {

            //interpolate intermediate healing values to avoid healing decimal amounts. 
            player.stamina += (int) effectInfo.card.amount / (int) (duration / Character.tickRate);
            if (ticks % countUp == 0) player.stamina += 1;

            t += tickRate;
            ticks++;

            if (player.stamina > player.maxStamina) player.stamina = player.maxStamina;

            yield return new WaitForSeconds(tickRate);
        }

        effectInfo.coroutine = null;
        effectInfo.list.Remove(effectInfo);
        

        yield return 0;
    }

    private IEnumerator DamageBoostEffect(StatusEffectInfo effectInfo) {
        player.currentDmgDealtModValue = effectInfo.card.amount;
        bool ok = false;
        while (!ok) {
            yield return new WaitForSeconds(effectInfo.card.duration);

            ok = true;
        }

        effectInfo.coroutine = null;
        effectInfo.list.Remove(effectInfo);
        RevaluateModiferValue(PWEffect.DamageBoost);

        yield return 0;
    }

    private IEnumerator SpeedBoostEffect(StatusEffectInfo effectInfo) {
        player.currentSpeedModValue = effectInfo.card.amount;
        bool ok = false;
        while (!ok) {
            yield return new WaitForSeconds(effectInfo.card.duration);

            ok = true;
        }

        effectInfo.coroutine = null;
        effectInfo.list.Remove(effectInfo);
        RevaluateModiferValue(PWEffect.SpeedBoost);

        yield return 0;
    }


    private IEnumerator WaitToPassivelyHeal() {

        //If character is supposed to be dead, then don't activate passive heal. 
        if (player.dead) {
            Debug.Log(player.characterName + " is dead");
            yield return 0;
        }

        bool ok = false;
        while (!ok) {
            
            yield return new WaitForSeconds(Character.timeToStartPassiveHeal);
            ok = true;
        }

        waitToHeal = null;

        passiveHeal = StartCoroutine(PassiveHeal());

        yield return 0;
    }

    private IEnumerator PassiveHeal() {

        //If character is supposed to be dead, then don't activate passive heal. 
        if (player.dead) {
            Debug.Log(player.characterName + " is dead");
            yield return 0;
        }

        while (player.hp < player.maxHP) {

            player.hp += healSpeed;

            yield return new WaitForSeconds(Character.tickRate);
        }
        
        player.hp = player.maxHP;

        passiveHeal = null;

        yield return 0;
    }

    private void StopAllHeals() {

        if (passiveHeal != null) {
            StopCoroutine(passiveHeal);
        }
        if (waitToHeal != null) {
            StopCoroutine(waitToHeal);
        }

        passiveHeal = null;
        waitToHeal = null;
    }


    private IEnumerator WaitToRegenStamina() {

        bool ok = false;
        while (!ok) {

            yield return new WaitForSeconds(Character.timeToStartPassiveHeal / 2);
            ok = true;
        }

        waitToStam = null;

        passiveStam = StartCoroutine(PassiveStaminaRegen());

        yield return 0;
    }


    private IEnumerator PassiveStaminaRegen() {

        while (player.stamina < player.maxStamina && player.GetRigidBody().velocity == Vector2.zero) {

            player.stamina += staminaSpeed;

            yield return new WaitForSeconds(Character.tickRate);
        }

        passiveStam = null;

        yield return 0;
    }

    /// <summary>
    /// Stops the current healing passive and starts the timer that waits to start healing again. 
    /// </summary>
    public void StopHealingPassive() {

        if (passiveHeal != null) {
            StopCoroutine(passiveHeal);
            passiveHeal = null;
        }

        if (waitToHeal != null) {
            StopCoroutine(waitToHeal);
            waitToHeal = null;
        }

        if (player.dead) return;

        waitToHeal = StartCoroutine(WaitToPassivelyHeal());
    }

    /// <summary>
    /// Stops the current stamina passive and starts the timer that waits to start regenerating stamina again. 
    /// </summary>
    public void StopStaminaPassive() {
        if (passiveStam != null) {
            StopCoroutine(passiveStam);
            passiveStam = null;
        }

        if (waitToStam != null) {
            StopCoroutine(waitToStam);
            waitToStam = null;
        }

        waitToStam = StartCoroutine(WaitToRegenStamina());
    }


}
