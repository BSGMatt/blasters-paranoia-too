using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PowerUpCard : Card
{
    public PWEffect effect;
    public float amount;
    public float duration;

    public override string infoText() {
        StringBuilder sb = new StringBuilder();

        sb.Append("Cost to Purchase: " + unlockPrice + "\n");

        switch(effect) {
            case PWEffect.InstantHealth:
                sb.Append("Instantly heals " + amount + " health.");
                break;
            case PWEffect.InstantStamina:
                sb.Append("Instantly recovers " + amount + " stamina.");
                break;
            case PWEffect.HealOverTime:
                sb.Append("Heals " + (amount/duration) + " health per second for " + duration + " seconds.");
                break;
            case PWEffect.StaminaOverTime:
                sb.Append("Recovers " + (amount / duration) + " stamina per second for " + duration + " seconds.");
                break;
            case PWEffect.DamageBoost:
                sb.Append("Increases your damage by " + ((amount - 1) * 100) + "% for " + duration + " seconds.");
                break;
            case PWEffect.SpeedBoost:
                sb.Append("Increases your speed by " + ((amount - 1) * 100) + "% for " + duration + " seconds.");
                break;
            case PWEffect.Sheild:
                sb.Append("Grants you " + amount + "sheild health.");
                break;
        }

        return sb.ToString();
    }
}
