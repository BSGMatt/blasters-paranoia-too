using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Character
{

    // Start is called before the first frame update
    void Start()
    {
        isEnemy = false;
        isLevelable = false;
        onlyHasHealth = true;
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Restore crystal back to full health and reset all of its visual changes
    /// </summary>
    public void ReviveCrystal() {
        hp = maxHP;
        dead = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void Die() {
        dead = true;
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public override void DisableMovement() {
        
    }

    public override void EnableMovement() {
        
    }

}
