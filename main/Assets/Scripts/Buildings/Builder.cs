using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Builder : MonoBehaviour {

    private const int SNAP = 64;

    public List<int> buildingsDeployed;
    public BuildingCard buidlingToDeploy;
    public Building buidlingToEdit;

    public bool mode; //false for deploy mode, true for edit mode. 
    
    //Check if the mouse is on a menubutton. If so, 
    //then blocks will not be placed if the player
    //left-clicks. 
    private bool onMenuButton;

    public void Start() {
        
    }

    public void Update() {
        //Toggle the builder's modes.
        if (Input.GetKeyDown(KeyCode.F)) {
            mode = !mode;
        }

        if (mode) {
            DeployMode();
        }
        else {
            EditMode();
        }
    }

    public void DeployMode() {

    }

    public void EditMode() {

    }

    //Check if the builder object is near a deployed building
    public void OnTriggerEnter2D(Collider2D collision) {
        
    }
}