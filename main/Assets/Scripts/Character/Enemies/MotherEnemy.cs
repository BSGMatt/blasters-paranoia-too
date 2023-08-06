using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/*

    Just like a normal enemy, expect that it will spawn another enemy upon death. 

*/

///
public class MotherEnemy : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void Die() {
        FindObjectOfType<Minimap>().DeleteMinimapIcon(minimapIcon);
        SpawnChildrenUponDeath();
        CommonDieMethod();
    }

    //Spawns the enemy's children. 
    private void SpawnChildrenUponDeath() {

        //Use angles so that children spawn in a circle around the mother. 
        float currentAngle = 0f;
        SpawnManager sm = FindObjectOfType<SpawnManager>();

        for (int i = 0; i < enemyCard.childCount; i++) {

            Vector3 childPosition = new Vector3(enemyCard.childDistance * Mathf.Cos(currentAngle), 
                enemyCard.childDistance * Mathf.Sin(currentAngle));

            sm.SpawnEnemy(enemyCard.child, (Vector3) rb.position + childPosition);

            currentAngle += (2f  * Mathf.PI / enemyCard.childCount);
        }

    }
}
