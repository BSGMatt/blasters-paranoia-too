using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop Item")]
public class ShopItem : ScriptableObject
{
    public Card card;
    public Sprite displaySprite;
    public Color displayTextColor;

    [TextArea(1, 10)]
    public string description;
    public bool sold;
}
