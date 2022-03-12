using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLoader : MonoBehaviour
{
    public int currentIndex;
    public bool isInit = false; //Value to check 
    private Dictionary<string, int> lastAmmoValues;
    public Character host;
    public GameObject loadedWeapon;
    public Text ammoCounterText;
    private InventoryManager im;

    public void Init() {

        im = FindObjectOfType<InventoryManager>();

        currentIndex = 0;
        lastAmmoValues = new Dictionary<string, int>();

        //Create the weapon
        loadedWeapon = Instantiate<GameObject>(im.weaponCards[0].prefab);
        loadedWeapon.GetComponent<Weapon>().host = host;
        loadedWeapon.GetComponent<Weapon>().ammo = loadedWeapon.GetComponent<Weapon>().card.maxAmmo;

        //Save the the last ammo value of the newly created weapon in the dictionary. 
        lastAmmoValues.Add(im.weaponCards[0].name, im.weaponCards[0].maxAmmo);

        isInit = true;
    }

    public void Update() {
        if (loadedWeapon != null) {
            ammoCounterText.text = loadedWeapon.GetComponent<Weapon>().ammo.ToString() + "/" + loadedWeapon.GetComponent<Weapon>().card.maxAmmo;
        }
        else {
            ammoCounterText.text = "";
        }
    }

    public void NextWeapon() {
        //Ignore method if there is only one weapon in inventory. 
        if (im.weaponCards.Count == 1) return;

        currentIndex++;

        if (currentIndex >= im.weaponCards.Count) {
            currentIndex = 0;
        }

        LoadNewWeapon();
    }

    public void PreviousWeapon() {
        //Ignore method if there is only one weapon in inventory. 
        if (im.weaponCards.Count == 1) return;

        currentIndex--;

        if (currentIndex < 0) {
            currentIndex = im.weaponCards.Count -  1;
        }

        LoadNewWeapon();
    }

    private void LoadNewWeapon() {
        //Save the ammo of the current weapon. 
        lastAmmoValues[loadedWeapon.GetComponent<Weapon>().card.name] = loadedWeapon.GetComponent<Weapon>().ammo;

        //Destroy the loaded weapon and create a new one in its place. 
        Destroy(loadedWeapon);
        GameObject newWeapon = Instantiate<GameObject>(im.weaponCards[currentIndex].prefab);
        newWeapon.GetComponent<Weapon>().host = host;
        newWeapon.GetComponent<Weapon>().rb.position = host.GetRigidBody().position;

        //Check if the weapon's name was added to the dictionary, if not, add it, if so, set weapon's ammo to ammo stored in dictionary. 
        if (lastAmmoValues.ContainsKey(newWeapon.GetComponent<Weapon>().card.name)) {
            newWeapon.GetComponent<Weapon>().ammo = lastAmmoValues[newWeapon.GetComponent<Weapon>().card.name];
        }
        else {
            lastAmmoValues.Add(newWeapon.GetComponent<Weapon>().card.name, newWeapon.GetComponent<Weapon>().card.maxAmmo);
            newWeapon.GetComponent<Weapon>().ammo = newWeapon.GetComponent<Weapon>().card.maxAmmo;
        }

        loadedWeapon = newWeapon;
    }

    public void DisableWeapon() {
        loadedWeapon.SetActive(false);
    }

    public void EnableWeapon() {
        loadedWeapon.SetActive(true);
    }
}
