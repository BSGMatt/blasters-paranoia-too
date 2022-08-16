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
    public Text shelfHeaderText;
    public Text cashAmountText;
    public ShopItem selectedItem;
    public itemType itemTypeDisplay;

    private int nextItem = 0;

    public void Start() {
        ResetPurchases();

        itemTypeDisplay = itemType.BUILDINGS;
        SwitchItemTypeToDisplay(itemTypeDisplay);
        selectedItem = itemsToDisplay[0].shopItem;
    }

    public void Update() {
        cashAmountText.text = "$" + FindObjectOfType<GameManager>().cash.ToString();
    }

    /// <summary>
    /// Iterates through every ShopItem and marks them as unsold. 
    /// </summary>
    public void ResetPurchases() {
        for (int i = 0; i < buildingsForSale.Count; i++) {
            buildingsForSale[i].sold = false;
        }

        for (int i = 0; i < weaponsForSale.Count; i++) {
            weaponsForSale[i].sold = false;
        }

        for (int i = 0; i < powerupsForSale.Count; i++) {
            powerupsForSale[i].sold = false;
        }
    }

    public void ShowBuildingsForSale() {
        for (int i = 0; i < itemsToDisplay.Length; i++) {
            itemsToDisplay[i].gameObject.SetActive(false);
        }

        //Reset nextItem unless we're already on this section. 
        if (itemTypeDisplay != itemType.BUILDINGS) nextItem = 0;
        SwitchItemTypeToDisplay(itemType.BUILDINGS);
    }
    public void ShowWeaponsForSale() {
        for (int i = 0; i < itemsToDisplay.Length; i++) {
            itemsToDisplay[i].gameObject.SetActive(false);
        }

        if (itemTypeDisplay != itemType.WEAPONS) nextItem = 0;
        SwitchItemTypeToDisplay(itemType.WEAPONS);
    }
    public void ShowPowerUpsForSale() {

        for (int i = 0; i < itemsToDisplay.Length; i++) {
            itemsToDisplay[i].gameObject.SetActive(false);
        }

        if (itemTypeDisplay != itemType.POWERUPS) nextItem = 0;
        SwitchItemTypeToDisplay(itemType.POWERUPS);
    }

    //Makes an attempt to purchase an item. 
    public void PurchaseItem() {

        GameManager gm = FindObjectOfType<GameManager>();
        InventoryManager im = FindObjectOfType<InventoryManager>();

        if (selectedItem.sold) {
            Debug.Log("You've already bought this item!");
            return;
        }
        else if (gm.cash < selectedItem.card.unlockPrice) {
            Debug.Log("You cannot afford this item!");
            return;
        }


        if (selectedItem.card.GetType() == typeof (BuildingCard)) {
            Debug.Log("Purchased: " + selectedItem.card.ToString());
            im.AddToBuildlings((BuildingCard)selectedItem.card);
            gm.cash -= selectedItem.card.unlockPrice;
            selectedItem.sold = true;
            FindObjectOfType<StatsManager>().AddXP(XPBonusManager.xpBonusForBuying);
        }
        else if (selectedItem.card.GetType() == typeof(WeaponCard)) {
            Debug.Log("Purchased: " + selectedItem.card.ToString());
            im.AddToWeapons((WeaponCard)selectedItem.card);
            gm.cash -= selectedItem.card.unlockPrice;
            selectedItem.sold = true;
            FindObjectOfType<StatsManager>().AddXP(XPBonusManager.xpBonusForBuying);
        }
        else {
            //Check if there is room for the powerup to be added to the inventory. 
            if (im.AddPowerUp((PowerUpCard) selectedItem.card) != -1) {
                Debug.Log("Purchased: " + selectedItem.card.ToString());
                gm.cash -= selectedItem.card.unlockPrice;
                FindObjectOfType<StatsManager>().AddXP(XPBonusManager.xpBonusForBuying);
            }
            else {
                Debug.Log("You're inventory is full!");
            }
        }

    }

    private void SwitchItemTypeToDisplay(itemType type) {

        List<ShopItem> listToDisplay;

        switch(type) {
            case itemType.BUILDINGS:
                listToDisplay = buildingsForSale;
                shelfHeaderText.text = "BUILDINGS";
                itemTypeDisplay = itemType.BUILDINGS;
                break;
            case itemType.WEAPONS:
                listToDisplay = weaponsForSale;
                shelfHeaderText.text = "WEAPONS";
                itemTypeDisplay = itemType.WEAPONS;
                break;
            default:
                listToDisplay = powerupsForSale;
                shelfHeaderText.text = "POWER-UPS";
                itemTypeDisplay = itemType.POWERUPS;
                break;
        }

        for (int i = nextItem; i < itemsToDisplay.Length + nextItem && i < listToDisplay.Count; i++) {
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
                shelfHeaderText.text = "BUILDINGS";
                break;
            case itemType.WEAPONS:
                listToDisplay = weaponsForSale;
                shelfHeaderText.text = "WEAPONS";
                break;
            default:
                listToDisplay = powerupsForSale;
                shelfHeaderText.text = "POWER-UPS";
                break;
        }

        for (int i = nextItem; i < itemsToDisplay.Length + nextItem && i < listToDisplay.Count; i++) {
            itemsToDisplay[(i - nextItem)].gameObject.SetActive(true);
            itemsToDisplay[(i - nextItem)].shopItem = listToDisplay[i];
            itemsToDisplay[(i - nextItem)].DisplayItem();
        }

        nextItem -= itemsToDisplay.Length;
        if (nextItem < 0) {
            if (listToDisplay.Count < itemsToDisplay.Length) {
                nextItem = 0;
            }
            else {
                nextItem = -nextItem % listToDisplay.Count;
            }
        }
        Debug.Log("Next Item: " + nextItem);

        itemDescriptionText.text = defaultDescString;
    }
}
