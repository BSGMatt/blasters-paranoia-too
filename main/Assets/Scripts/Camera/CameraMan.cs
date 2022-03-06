using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour {

    //FocalPoint - An object that helps with the camera when there are 2 objects that need
    //to be in focus. 
    public GameObject focalPointPrefab; //The prefab refrence to the FocalPoint object. 
    private GameObject focalPoint; //The instance that is created from the prefab. 


    //Used for the player and camera positions
    public Transform cameraTransform;
    public Transform playerTransform;


    //public Rigidbody2D playerRB;
    public Camera cam;

    //Camera's move speed
    public float camSpeed = 1.0f;
    private float xSpeed;
    private float ySpeed;

    //How close to the bounds of screen the player must be before the camera start moving. 
    //Think of these values like a percentage; 0.2 means the player must be in the left/right 20%
    //of the screen before the camera will move. 
    public float limit = 0.2f;
    private float xLimit;
    private float yLimit;

    private bool enableBounds = true;
    private bool following = true;

    [SerializeField]
    private float rectWidth;

    [SerializeField]
    private float rectHeight;

    void Start() {
        cam.pixelRect = new Rect(0, 0, rectWidth, rectHeight);

        xSpeed = camSpeed;
        ySpeed = camSpeed * (rectWidth / rectHeight); //Derive the y speed from the aspect ratio. 

        xLimit = limit;
        yLimit = limit / (rectWidth / rectHeight);//Derive the y limit from the aspect ratio. 
    }

    public void Update() {
        //Debug.Log("Following: " + following);
        if (following) FollowTarget();

        //Code to test CreateFocalPoint() method
        if (Input.GetKeyDown(KeyCode.Space)) {
            TestFocalPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Delete)) {
            focalPoint.GetComponent<FocalPoint>().Remove();
        }
    }

    // Update is called once per frame
    void FollowTarget() {

        //Gets the player's position relative to the camera. 
        Vector3 playerPos = cam.WorldToViewportPoint(playerTransform.position);
        /*Debug.Log("Player Transform: " + playerTransform);
        Debug.Log("Cam Position: " + cameraTransform.position.x + "," + cameraTransform.position.y);
        Debug.Log("Delta X: " + Mathf.Abs(playerPos.x - cameraTransform.position.x));
        Debug.Log("Player Position: " + playerPos.x + "," + playerPos.y);*/

        //Interpolation values for each axis
        int ignoreBounds = BSGUtility.BoolAsInt(enableBounds);
        float interpolationX = xSpeed * Time.deltaTime * CheckHorizontalBounds(playerPos.x) * ignoreBounds;
        float interpolationY = ySpeed * Time.deltaTime * CheckVerticalBounds(playerPos.y) * ignoreBounds;

        //"Smooth" the camera using lerp method. 
        Vector3 position = cameraTransform.position;
        position.y = Mathf.Lerp(cameraTransform.position.y, playerTransform.position.y, interpolationY);
        position.x = Mathf.Lerp(cameraTransform.position.x, playerTransform.position.x, interpolationX);

        

        cameraTransform.position = position;
    }

    

    public void EnableBounds(bool val) {
        enableBounds = val;
    }

    public void SetFollowing(bool val) {
        following = val;
    }

    //Checks whether the player is near the vertical bounds of the screen. 
    private float CheckVerticalBounds(float y) {
        if (y < 1 - yLimit && y > yLimit) {
            return 0.0f;
        }
        else {
            return 1.0f;
        }
    }

    //Checks whether the player is near the horizontal bounds of the screen. 
    private float CheckHorizontalBounds(float x) {
        if (x < 1 - xLimit && x > xLimit) {
            return 0.0f;
        }
        else {
            return 1.0f;
        }
    }

    public void SetCameraMode(CameraMode mode) {
        switch (mode) {
            default: //Balanced
                camSpeed = 2.5f;
                limit = 0.42f;
                return;

            case CameraMode.CLASSIC:
                camSpeed = 1;
                limit = 0.33f;
                return;

            case CameraMode.SMOOTH:
                camSpeed = 4;
                limit = 1;
                return;
        }
    }

    public void CreateFocalPoint(Transform primary, Transform secondary, float priority) {
        if (focalPoint != null) focalPoint.GetComponent<FocalPoint>().Remove();
        focalPoint = Instantiate(focalPointPrefab); //Create an object of FocalPoint GameObject. 

        //Set its properties. 
        focalPoint.GetComponent<FocalPoint>().SetPrimary(primary);
        focalPoint.GetComponent<FocalPoint>().SetSecondary(secondary);
        focalPoint.GetComponent<FocalPoint>().SetPriority(priority);
    }

    public void TestFocalPoint() {
        GameObject s = GameObject.Find("secondary");
        if (s != null) Destroy(s);
        GameObject secondary = new GameObject("secondary");
        secondary.transform.position = playerTransform.position;
        if (focalPoint != null) focalPoint.GetComponent<FocalPoint>().Remove();
        CreateFocalPoint(playerTransform, secondary.transform, 0.5f);
    }

    public void SetTargetTransform(Transform t) {
        playerTransform = t;
    }

    public Transform GetTargetTransform() {
        return playerTransform;
    }

    public Camera GetCamera() {
        return cam;
    }

    public float GetLimit() {
        return limit;
    }

    public float GetXLimit() {
        return xLimit;
    }

    public float GetYLimit() {
        return yLimit;
    }

    public void SetLimit(float limit) {
        this.limit = limit;
        xLimit = limit;
        yLimit = limit / (rectWidth / rectHeight);
    }

    public float GetCameraSpeed() {
        return camSpeed;
    }

}
