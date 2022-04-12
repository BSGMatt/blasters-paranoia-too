using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Builder : MonoBehaviour {

    private const int SNAP = 64;

    public List<Building> buildingsDeployed;
    public BuildingCard buidlingToDeploy;
    public Building buidlingToEdit;

    public Camera cam; //Camera used to update its position, usually the camera that follows the player. 

    public bool mode; //false for deploy mode, true for edit mode. 

    //Whether the aging Coroutine should be active. 
    public bool shouldAge;
    
    //Check if the mouse is on a menubutton. If so, 
    //then blocks will not be placed if the player
    //left-clicks. 
    private bool onMenuButton;

    public void Start() {
        
    }

    public void Update() {
        UpdatePosition();

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

    public void FixedUpdate() {

    }

    public void DeployMode() {

    }

    public void EditMode() {

    }

    private void UpdatePosition() {
        //Move the builder object based on the mouth position relative to the camera.  
        Vector3 mPosRelToCamera = cam.ScreenToWorldPoint(Input.mousePosition);

        mPosRelToCamera.z = 0;

        transform.position = new Vector3(Mathf.Round(mPosRelToCamera.x), Mathf.Round(mPosRelToCamera.y), mPosRelToCamera.z);
    }

    //Check if the builder object is near a deployed building
    public void OnTriggerEnter2D(Collider2D collision) {
        
    }

    /// <summary>
    /// Iterate through every building delpoyed 
    /// and increase their age every clock tick. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator AgeBuildings() {
        while (shouldAge) {
            yield return new WaitForSeconds(Character.tickRate);

            for (int i = 0; i < buildingsDeployed.Count; i++) {
                buildingsDeployed[i].age++;
            }
        }

        yield return 0;
    }
}