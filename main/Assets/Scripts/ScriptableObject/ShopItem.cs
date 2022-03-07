using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : ScriptableObject
{
    public Card card;
    public Image displayImage;

    [TextArea(1, 10)]
    public string description;
    public bool sold;
}
