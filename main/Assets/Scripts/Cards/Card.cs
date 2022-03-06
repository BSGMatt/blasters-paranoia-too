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
public class Card : ScriptableObject
{
    public int unlockPrice; //Price to unlcock from the shop
    public GameObject prefab; //The gameobject that this card will create. 

}
