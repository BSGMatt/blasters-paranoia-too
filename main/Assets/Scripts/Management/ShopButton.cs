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
        s.itemDescriptionText.text = shopItem.description + "\n\n" + shopItem.card.infoText();
    }

    public void DisplayItem() {
        button.image.sprite = shopItem.displaySprite;
        if (shopItem.sold) {
            button.image.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else {
            button.image.color = new Color(1, 1, 1);
        }

        Text buttonText = button.GetComponentInChildren<Text>();

        buttonText.text = shopItem.card.name;
        buttonText.color = shopItem.displayTextColor;
    }
}
