using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores lists of all the weapon and building cards purchased from the shop, and keeps track of the items the player can use. 
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public List<WeaponCard> weaponCards;
    public List<BuildingCard> buildingCards;
    public Inventory<PowerUpCard> powerups;
    public HotbarSlot[] powerupHotBar;

    public void Start() {
        powerups = new Inventory<PowerUpCard>();
    }

    public void Update() {
        UpdateInventoryDisplay();
    }

    //Display all of the inventory's information onto the hotbar. 
    private void UpdateInventoryDisplay() {
        for (int i = 0; i < powerupHotBar.Length; i++) {
            powerupHotBar[i].slot = i;
            if (powerups.CountOfSlot(i) > 0) {
                //Add the powerup's icon and count in the inventory. 
                powerupHotBar[i].hotbarIcon.sprite = powerups.Peek(i).hotbarSprite;
                powerupHotBar[i].hotbarIcon.color = new Color(1, 1, 1, 1);
                powerupHotBar[i].counterText.text = powerups.CountOfSlot(i).ToString();
            }
            else {
                //Make the hotbar blank if slot is empty. 
                powerupHotBar[i].hotbarIcon.sprite = null;
                powerupHotBar[i].hotbarIcon.color = new Color(1, 1, 1, 0);
                powerupHotBar[i].counterText.text = "";
            }
            
        }
    }

    public void AddToWeapons(WeaponCard wc) {
        weaponCards.Add(wc);
        if (weaponCards.Count == 1) {
            FindObjectOfType<WeaponLoader>().Init();
        }
    }

    public void AddToBuildlings(BuildingCard bc) {
        buildingCards.Add(bc);
    }

    public int AddPowerUp(PowerUpCard pc) {
        return powerups.Add(pc);
    }

    /// <summary>
    /// Iterates through each hotbar to check if the player pressed a key associated with one of them. 
    /// </summary>
    /// <returns>The slot that the hotbar's key was associated with. For example, if the key to use an item in slot 1 is Numpad 1, then 
    /// the method will return 1 if if Numpad 1 is pressed. </returns>
    public int HotBarButtonPressed() {

        for (int i = 0; i < powerupHotBar.Length; i++) {
            if (Input.GetKeyDown(powerupHotBar[i].key)) {
                return powerupHotBar[i].slot;
            }
        }

        return -1;
    }
}
