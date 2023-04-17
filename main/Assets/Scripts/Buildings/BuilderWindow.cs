using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderWindow : MonoBehaviour
{
    public Builder builder;
    public Text nameText;
    public Text descriptionText;
    public Text ageValueText;
    public Healthbar healthbar;

    public void UpdateValues(Building building) {
        nameText.text = building.card.name;
        descriptionText.text = building.card.infoText();
        ageValueText.text = building.age.ToString();
        healthbar.UpdateValue(building.hp);
    }
}
