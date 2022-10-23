using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A Script designed to managed adjust the camera size based on the distance between 
 * a primary subject of focus and a secondary subject. 
 * 
 * */
public class FocalPoint : MonoBehaviour {
    private new Camera camera;
    private CameraMan cm;

    private Transform[] focalPoints; //<-An array containing every possible focal point. 

    /// <summary>
    /// The subject that the camera will piortize. The camera will follow 
    /// the primary subject more than the secondary. 
    /// </summary>
    [SerializeField] private Transform primary;

    /// <summary>
    /// The subject that camera will NOT prioritzie. The camera will 
    /// follow the primary subject over the secondary. 
    /// </summary>
    [SerializeField] private Transform secondary;

    /// <summary>
    /// How much will the camera priortize the primary object over
    /// the secondary. If the priority is 1, then the primary will only focus on the primary. 
    /// If the priority is 0, then the primary and secondary subjects will be focused on equally. 
    /// </summary>
    [Range(0.0f, 1f)]
    [SerializeField] private float priority;

    private Transform formerTarget;

    [SerializeField] private float def_cameraSize = 8;
    [SerializeField] private float grad;


    /// <summary>
    /// The distance between the subjects' positions in normalized to the viewport. 
    /// </summary>
    private float viewportDistance;

    private Coroutine sizeAdjuster = null;

    // Start is called before the first frame update
    void Start() {
        cm = FindObjectOfType<CameraMan>();
        camera = cm.GetCamera();

        //Set former values. 
        formerTarget = cm.GetTargetTransform();
        Debug.Log("former Target: " + formerTarget);

        cm.SetTargetTransform(transform); //Make the Camera Man follow the focal point
    }

    //Set's the target position based on the priority. 
    private Vector3 TargetPosition() {

        //Find the distance between the subjects in the form of a vector. 
        Vector3 distanceVector = BSGUtility.VectorSubtract(secondary.position, primary.position);
        return BSGUtility.VectorSubtract(BSGUtility.Midpoint(primary.position, secondary.position),
            BSGUtility.ScalarMultiply(distanceVector, priority / 2));
    }

    public void FixedUpdate() {

        //Set the position of the focal point.  
        transform.position = TargetPosition();

        Vector3 objAViewport = primary.position;
        Vector3 objBViewport = secondary.position;

        //Find the distance between the two objects relative to the viewport. 
        viewportDistance = Vector3.Distance(objBViewport, objAViewport);

        if (!BSGUtility.Within(camera.orthographicSize, viewportDistance / 2, 0.1f)) {
            if (sizeAdjuster != null) {
                StopCoroutine(sizeAdjuster);
                sizeAdjuster = null;
            }

            sizeAdjuster = StartCoroutine(AdjustScreenSize(viewportDistance / 2));
        }
    }

    private IEnumerator AdjustScreenSize(float targetSize) {

        Debug.Log("Starting Coroutine");
        Debug.Log("Target Size: " + targetSize);

        if (targetSize < def_cameraSize) targetSize = def_cameraSize;

        while (!BSGUtility.Within(camera.orthographicSize, targetSize, 0.1f)) {

            if (camera.orthographicSize < targetSize) {
                camera.orthographicSize += Time.deltaTime * cm.camSpeed;
            }
            else if (camera.orthographicSize > targetSize) {
                camera.orthographicSize -= Time.deltaTime * cm.camSpeed;
            }

            yield return new WaitForEndOfFrame();
        }

        sizeAdjuster = null;
        yield return null;
    }

    /// <summary>
    /// The subject that the camera will pioritize. The camera will follow 
    /// the primary subject more than the secondary. 
    /// </summary>
    public Transform GetPrimary() {
        return primary;
    }

    /// <summary>
    /// The subject that the camera will not prioritize. The camera will 
    /// follow the primary subject over the secondary. 
    /// </summary>
    public Transform GetSecondary() {
        return secondary;
    }

    public void SetPrimary(Transform primary) {
        this.primary = primary;
    }

    public void SetSecondary(Transform secondary) {
        this.secondary = secondary;
    }

    /// <summary>
    /// <para>
    /// Swaps the primary and subjects. I.E, the secondary subject will become the primary and
    /// the primary subject will become the secondary. 
    /// </para>
    /// </summary>
    public void SwapSubjects() {
        Transform temp = primary;
        primary = secondary;
        secondary = temp;
    }

    /// <summary>
    /// How much will the camera priortize the primary object over
    /// the secondary. If the priority is 1, then the primary will only focus on the primary. 
    /// If the priority is 0, then the primary and secondary subjects will be focused on equally. 
    /// </summary>
    public void SetPriority(float priority) {
        this.priority = priority;
    }

    /// <summary>
    /// How much will the camera priortize the primary object over
    /// the secondary. If the priority is 1, then the primary will only focus on the primary. 
    /// If the priority is 0, then the primary and secondary subjects will be focused on equally. 
    /// </summary>
    public float GetPiority() {
        return priority;
    }

    /// <summary>
    /// Deletes the Focal Point. Please use this instead of Destroy(), as the camera's target needs to be reset
    /// to the original target
    /// </summary>
    public void Remove() {
        cm.GetCamera().orthographicSize = def_cameraSize;
        cm.SetTargetTransform(formerTarget);
        Destroy(gameObject);
    }
}