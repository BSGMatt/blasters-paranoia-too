using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * A Card is an object designed to contain information
 * about a gameobject, such as its price to unlock and price to deploy. It also contains 
 * a reference to the prefab that the gameobject will created from. 
 * 
 */
public abstract class Card : ScriptableObject
{
    public int unlockPrice; //Price to unlcock from the shop
    public new string name;
    public GameObject prefab; //The gameobject that this card will create. 
    public Sprite sprite;

    public abstract string infoText();
}
