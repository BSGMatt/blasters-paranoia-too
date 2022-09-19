using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A building object designed to test building deploying functionality. 
/// </summary>
public class TestBuilding : Building
{

    public override void Passive() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DisableMovement() {
        Debug.Log("I'm a buidling, I can't move!");
    }

    public override void EnableMovement() {
        Debug.Log("I'm a buidling, I can't move!");
    }

    public override void Die() {

        Debug.Log("I'm-a dying!");

        Destroy(this.gameObject);
    }

}
