using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stores references to items that can be purchased by the player. Also controls the UI and selection of Items
/// </summary>
public class ShopManager : MonoBehaviour
{
    public enum itemType {
        BUILDINGS = 0,
        WEAPONS = 1,
        POWERUPS = 2
    }

    public const string defaultDescString = "Click on an item to learn more about it. ";

    public List<ShopItem> buildingsForSale;
    public List<ShopItem> weaponsForSale;
    public List<ShopItem> powerupsForSale;

    public ShopButton[] itemsToDisplay;
    public Text itemDescriptionText;
    public ShopItem selectedItem;
    public itemType itemTypeDisplay;

    private int nextItem = 0;

    public void Start() {
        itemTypeDisplay = itemType.BUILDINGS;
        SwitchItemTypeToDisplay(itemTypeDisplay);
        selectedItem = itemsToDisplay[0].shopItem;
    }

    public void ShowBuildingsForSale() {
        SwitchItemTypeToDisplay(itemType.BUILDINGS);
    }
    public void ShowWeaponsForSale() {
        SwitchItemTypeToDisplay(itemType.WEAPONS);
    }
    public void ShowPowerUpsForSale() {
        SwitchItemTypeToDisplay(itemType.POWERUPS);
    }

    private void SwitchItemTypeToDisplay(itemType type) {

        List<ShopItem> listToDisplay;

        switch(type) {
            case itemType.BUILDINGS:
                listToDisplay = buildingsForSale;
                break;
            case itemType.WEAPONS:
                listToDisplay = weaponsForSale;
                break;
            default:
                listToDisplay = powerupsForSale;
                break;
        }

        for (int i = nextItem; i < itemsToDisplay.Length + nextItem && i < listToDisplay.Count; i++) {
            Debug.Log("i: " + i);
            Debug.Log("(i - nextItem) % listToDisplay.Count: " + (i - nextItem) % listToDisplay.Count);
            itemsToDisplay[(i - nextItem)].gameObject.SetActive(true);
            itemsToDisplay[(i - nextItem)].shopItem = listToDisplay[i];
            itemsToDisplay[(i - nextItem)].DisplayItem();
        }

        nextItem += itemsToDisplay.Length;
        if (nextItem >= listToDisplay.Count) nextItem = 0;
        Debug.Log("Next Item: " + nextItem);

        itemDescriptionText.text = defaultDescString;
    }

    public void NextPage() {

        for (int i = 0; i < itemsToDisplay.Length; i++) {
            itemsToDisplay[i].gameObject.SetActive(false);
        }

        SwitchItemTypeToDisplay(itemTypeDisplay);
        itemDescriptionText.text = defaultDescString;
    }

    public void PreviousPage() {

        for (int i = 0; i < itemsToDisplay.Length; i++) {
            itemsToDisplay[i].gameObject.SetActive(false);
        }

        List<ShopItem> listToDisplay;

        switch (itemTypeDisplay) {
            case itemType.BUILDINGS:
                listToDisplay = buildingsForSale;
                break;
            case itemType.WEAPONS:
                listToDisplay = weaponsForSale;
                break;
            default:
                listToDisplay = powerupsForSale;
                break;
        }

        for (int i = nextItem; i < itemsToDisplay.Length + nextItem && i < listToDisplay.Count; i++) {
            itemsToDisplay[(i - nextItem)].gameObject.SetActive(true);
            itemsToDisplay[(i - nextItem)].shopItem = listToDisplay[i];
            itemsToDisplay[(i - nextItem)].DisplayItem();
        }

        nextItem -= itemsToDisplay.Length;
        if (nextItem < 0) nextItem = listToDisplay.Count - 1;
        Debug.Log("Next Item: " + nextItem);

        itemDescriptionText.text = defaultDescString;
    }
}
