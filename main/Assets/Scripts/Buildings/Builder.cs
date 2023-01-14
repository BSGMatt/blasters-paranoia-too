using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Events;

public class Builder : MonoBehaviour {

    private const int SNAP = 64;

    public List<Building> buildingsDeployed;
    public BuildingCard buildingToDeploy;
    public Building buildingToEdit;
    public InventoryManager im;
    public GameObject buildingPreview;
    public Text modeDisplayText;
    public Tilemap mapGeometery;
    public BuilderWindow builderWindow;
    public Minimap minimap;

    private int buildingListIndex;

    private RaycastHit2D[] hits;

    public Camera cam; //Camera used to update its position, usually the camera that follows the player. 

    public bool mode; //false for deploy mode, true for edit mode. 
    
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
            if (builderWindow.gameObject.activeSelf == true) {
                builderWindow.gameObject.SetActive(false);
            }
            //Debug.Log("You're now in Deploy mode.");
            modeDisplayText.text = "DEPLOY MODE";
            DeployMode();
        }
        else {
            //Debug.Log("You're now in Edit mode.");
            modeDisplayText.text = "EDIT MODE";
            EditMode();
        }
    }

    public void DeployMode() {

        //Don't do anything if there is buildings available to be deployed. 
        if (im.buildingCards.Count == 0) return;
        
        //Select a building from IM's list
        if (Input.GetKeyDown(KeyCode.E)) { //Forward
            buildingListIndex = (buildingListIndex + 1) % im.buildingCards.Count;
            
        }
        else if (Input.GetKeyDown(KeyCode.Q)) { //Backward
            buildingListIndex--;
            if (buildingListIndex < 0) buildingListIndex = im.buildingCards.Count - 1;
        }

        buildingToDeploy = im.buildingCards[buildingListIndex];

        //Set the preview's sprite to the buildings sprite, and the make the sprite translucent. 
        buildingPreview.GetComponent<SpriteRenderer>().sprite = buildingToDeploy.prefab.GetComponent<SpriteRenderer>().sprite;
        buildingPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        Vector3Int offset = new Vector3Int(-1, -1, 0);

        //Check if there is a map geometery tile or a building under the builder. 
        if (mapGeometery.GetTile(BSGUtility.ToVectorInt(transform.position, false) + offset) != null) {
            return;
        }

        //Clear the hits array and fill it with new collider data
        hits = new RaycastHit2D[10];
        Physics2D.Raycast(transform.position, Vector2.zero, new ContactFilter2D().NoFilter(), hits);

        //Check if the builder is on top of any existing buildings
        foreach (RaycastHit2D hit in hits) {
            Debug.Log("Hit: " + hit.collider);
            if (hit && hit.collider.gameObject.GetComponent<Building>() != null) {
                return;
            }
        }

        //Attempt deploy on left-click. 
        if (Input.GetMouseButtonDown(0)) {
            if (FindObjectOfType<GameManager>().cash >= buildingToDeploy.buildPrice) {
                FindObjectOfType<GameManager>().cash -= buildingToDeploy.buildPrice;

                //Place the building down. 
                GameObject b = GameObject.Instantiate(buildingToDeploy.prefab, transform.position, Quaternion.identity);
                buildingsDeployed.Add(b.GetComponent<Building>());
                b.GetComponent<Building>().card = buildingToDeploy;

                minimap.CreateMinimapIcon(b.GetComponent<Building>(), true);
            }
            else {
                Debug.Log("Player could not afford to deploy " + buildingToDeploy.name);
            }
        }
    }

    public void EditMode() {

        // Clear the hits array and fill it with new collider data
        hits = new RaycastHit2D[10];
        Physics2D.Raycast(transform.position, Vector2.zero, new ContactFilter2D().NoFilter(), hits);

        //Check if the builder is on top of any existing buildings
        foreach (RaycastHit2D hit in hits) {
            Debug.Log("Hit: " + hit.collider);
            if (hit && hit.collider.gameObject.GetComponent<Building>() != null) {
                buildingToEdit = hit.collider.gameObject.GetComponent<Building>();
            }
        }

        Debug.Log("buildingToEdit: " + buildingToEdit);

        if (Input.GetMouseButtonDown(0)) {
            if (builderWindow.gameObject.activeSelf == false) {
                builderWindow.gameObject.SetActive(true);          
            }

            builderWindow.UpdateValues(buildingToEdit);
        }

        if (Input.GetMouseButtonDown(1)) {
            if (builderWindow.gameObject.activeSelf == true) {
                builderWindow.gameObject.SetActive(false);
            }
        }

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

    public void OnTriggerExit2D(Collider2D collision) {

    }


}