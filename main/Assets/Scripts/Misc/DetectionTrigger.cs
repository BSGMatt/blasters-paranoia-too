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

    public UnityEvent listUpdateEvent; //Occurs when a character enters or leaves the trigger. 
    public UnityEvent listAddEvent; //Occurs when a character enters the trigger.
    public UnityEvent listRemoveEvent; //Occurs when a character leaves the trigger.

    public void Awake() {
        characterList = new List<Character>();
        listUpdateEvent = new UnityEvent();
        listAddEvent = new UnityEvent();
        listRemoveEvent = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Character")) {
            characterList.Add(collision.GetComponent<Character>());
            listUpdateEvent.Invoke();
            listAddEvent.Invoke();
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.CompareTag("Character")) {
            characterList.Remove(collision.GetComponent<Character>());
            listUpdateEvent.Invoke();
            listRemoveEvent.Invoke();
        }

    }
}
