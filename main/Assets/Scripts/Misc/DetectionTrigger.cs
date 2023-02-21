using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An object that keeps track of the characters that enter and exit its boundaries. 
/// </summary>
public class DetectionTrigger : MonoBehaviour
{
    public CircleCollider2D trigger;
    public List<Character> characterList;

    public UnityEvent listUpdateEvent; //An event that invokes whenever the character list changes. 

    public void Awake() {
        characterList = new List<Character>();
        listUpdateEvent = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Character")) {
            characterList.Add(collision.GetComponent<Character>());
            listUpdateEvent.Invoke();
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.CompareTag("Character")) {
            characterList.Remove(collision.GetComponent<Character>());
            listUpdateEvent.Invoke();
        }

    }
}
