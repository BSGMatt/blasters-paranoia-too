using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Pellet
{
    public HazardCard hazardCard;
    public int numShocks;
    public float timeBetweenShocks;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(CreateShocks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CreateShocks() {
        float t = 0;
        int shocks = 0;
        while (t < duration && shocks < numShocks) {

            Instantiate<GameObject>(hazardCard.prefab, transform.position, Quaternion.identity);

            t += timeBetweenShocks;
            shocks++;
            Debug.Log("t: " + t);
            yield return new WaitForSeconds(timeBetweenShocks);
        }
    }
}
