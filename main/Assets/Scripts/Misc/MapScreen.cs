using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreen : MonoBehaviour
{
    public Transform topLeft;
    public Transform bottomRight;
    public Transform anchor;

    // Start is called before the first frame update

    private void FixedUpdate() {
        transform.position = anchor.position;
    }


}
