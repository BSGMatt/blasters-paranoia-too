using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Button button;
    public ShopItem shopItem;

    public void SelectShopItem() {
        ShopManager s = FindObjectOfType<ShopManager>();
        s.selectedItem = shopItem;
        s.itemDescriptionText.text = shopItem.description + "\n" + shopItem.card.infoText();
    }

    public void DisplayItem() {
        button.image.sprite = shopItem.displaySprite;
        button.GetComponentInChildren<Text>().text = shopItem.card.name;
    }
}
