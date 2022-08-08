using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isEnemy;
    public int damage;

    private Vector2 velocity;
    private Vector2 start;


    /// <summary>
    /// Set the starting position and velocity to the given values. 
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="s"></param>
    public void Init(Vector2 vel, Vector2 s) {
        velocity = vel;
        start = s;
    }

    public void Start() {
        transform.position = start;
        rb.velocity = velocity;
    }

    public void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Character")) {

            Character c = collision.gameObject.GetComponent<Character>();

            if (c.invincible) return;

            //Have the character take damage if the pellet has the opposite affiliation
            if (isEnemy != c.IsEnemy()) {
                c.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Pellet")) {
            return; //Don't destroy if colliding with another pellet. 
        }

        Destroy(gameObject);
    }
}
