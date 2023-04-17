using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLoader : MonoBehaviour
{
    public int currentIndex;
    public bool isInit = false; //Value to check 
    public Dictionary<string, int> lastAmmoValues;
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
        loadedWeapon.GetComponent<Weapon>().card = im.weaponCards[0]; //Assign card's value to prefab
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
        Debug.Log(loadedWeapon.GetComponent<Weapon>().card.name + ", " + lastAmmoValues[loadedWeapon.GetComponent<Weapon>().card.name]);
        lastAmmoValues[loadedWeapon.GetComponent<Weapon>().card.name] = loadedWeapon.GetComponent<Weapon>().ammo;

        //Destroy the loaded weapon and create a new one in its place. 
        Destroy(loadedWeapon);
        GameObject newWeapObj = Instantiate<GameObject>(im.weaponCards[currentIndex].prefab);
        Weapon newWeapon = newWeapObj.GetComponent<Weapon>();

        newWeapon.card = im.weaponCards[currentIndex];
        newWeapon.host = host;
        newWeapon.rb.position = host.GetRigidBody().position;

        //Check if the weapon's name was added to the dictionary, if not, add it, if so, set weapon's ammo to ammo stored in dictionary. 
        if (lastAmmoValues.ContainsKey(newWeapon.card.name)) {
            Debug.Log("Key: " + newWeapon.card.name + " is a dictionary key");
            Debug.Log("Setting ammo to: " + lastAmmoValues[newWeapon.card.name]);
            newWeapon.ammo = lastAmmoValues[newWeapon.card.name];
        }
        else {
            Debug.Log("Key: " + newWeapon.card.name + " isn't a dictionary key");
            lastAmmoValues.Add(newWeapon.card.name, newWeapon.card.maxAmmo);
            newWeapon.ammo = newWeapon.card.maxAmmo;
        }

        loadedWeapon = newWeapObj;
    }

    public void DisableWeapon() {
        loadedWeapon.SetActive(false);
    }

    public void EnableWeapon() {
        loadedWeapon.SetActive(true);
    }
}
