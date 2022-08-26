using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public HazardCard card;

    /// <summary>
    /// This value controls how smooth the increase in size should be. 
    /// </summary>
    public float growthSmoothing = 1;


    public void Start() {

        StartCoroutine(Exist());
    }

    public IEnumerator Exist() {
        float radiInc = (card.maxSize - card.minSize) / card.duration / growthSmoothing;
        float t = 0;
        while (t < card.duration && transform.localScale.x < card.maxSize) {
            transform.localScale = new Vector3(transform.localScale.x + radiInc, transform.localScale.y + radiInc, 1);
            t += 1f/ growthSmoothing;
            yield return new WaitForSeconds(1f / growthSmoothing);
        }

        Destroy(gameObject);

    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Character")) {

            Character c = collision.gameObject.GetComponent<Character>();

            if (c.invincible) return;

            //Have the character take damage if the hazard has the opposite affiliation
            if (card.isEnemy != c.IsEnemy()) {
                c.TakeDamage(card.damage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Pellet")) {
            return; //Don't destroy if colliding with another pellet. 
        }
        else {
            Destroy(gameObject);
        }

    }
}
