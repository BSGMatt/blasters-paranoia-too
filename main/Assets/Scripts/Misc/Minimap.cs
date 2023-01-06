using UnityEngine;


public class Minimap : MonoBehaviour
{
    [Header("References")]
    public RectTransform minimapPoint_1;
    public RectTransform minimapPoint_2;
    public Transform worldPoint_1;
    public Transform worldPoint_2;

    [Header("Player")]
    public RectTransform playerMinimap;
    public Transform playerWorld;
    public Vector2 playerMiniMapOffset;


    private float minimapRatioX, minimapRatioY;


    /**/


    private void Awake()
    {
        CalculateMapRatio();
    }


    private void Update()
    {
        playerMinimap.anchoredPosition = minimapPoint_1.anchoredPosition + new Vector2((playerWorld.position.x - worldPoint_1.position.x) * minimapRatioX,
                                         (playerWorld.position.y - worldPoint_1.position.y) * minimapRatioY)
                                        + playerMiniMapOffset;

        Debug.Log(playerMinimap.anchoredPosition);
    }


    public void CalculateMapRatio()
    {
        //distance world ignoring Z axis
        Vector2 distanceWorldVector = worldPoint_1.position - worldPoint_2.position;

        Vector2 minimapVector = minimapPoint_1.anchoredPosition - minimapPoint_2.anchoredPosition;

        minimapRatioX = minimapVector.x / distanceWorldVector.x;
        minimapRatioY = minimapVector.y / distanceWorldVector.y;
        Debug.Log("Mini-Map Ratio: " + minimapRatioX + ", " + minimapRatioY);
    }
}