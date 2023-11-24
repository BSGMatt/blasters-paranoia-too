using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public HazardCard card;

    private float damage;


    /// <summary>
    /// This value controls how smooth the increase in size should be. 
    /// </summary>
    public float growthSmoothing = 4;


    public void Start() {
        damage = card.minDamage;
        StartCoroutine(Exist());
    }

    public IEnumerator Exist() {
        float radiInc = (card.maxSize - card.minSize) / card.duration / growthSmoothing;
        float damageInc = (card.maxDamage - card.minDamage) / card.duration / growthSmoothing;
        float t = 0;
        while (t < card.duration && transform.localScale.x < card.maxSize) {
            transform.localScale = new Vector3(transform.localScale.x + radiInc, transform.localScale.y + radiInc, 1);
            t += 1f/ growthSmoothing;
            damage += damageInc;
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
                c.TakeDamage((int) damage);
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
