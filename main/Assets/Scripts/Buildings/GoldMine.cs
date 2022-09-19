using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Building designed to generate resources for the player. 
/// </summary>
public class GoldMine : Building
{
    // Start is called before the first frame update
    GameManager gm;
    Character player;
    Coroutine mining;

    void Start()
    {
        BaseInit();
        player = FindObjectOfType<BB>();
        gm = FindObjectOfType<GameManager>();
        mining = StartCoroutine(Mine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Passive() {
        gm.cash += card.resourceAmount;
    }

    public override void Die() {
        Destroy(gameObject);
    }

    public IEnumerator Mine() {
        while (hp > 0) {
            Passive();
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }

    public override void DisableMovement() {

    }

    public override void EnableMovement() {

    }
}
