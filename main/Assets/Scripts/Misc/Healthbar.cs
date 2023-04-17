using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void UpdateValue(int val) {
        slider.value = val;
    }
}
