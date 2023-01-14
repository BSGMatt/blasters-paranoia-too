using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A script for handling the minimap. 
/// </summary>
public class Minimap : MonoBehaviour
{

    [Header("References")]
    public RectTransform minimapPoint_1;
    public RectTransform minimapPoint_2;
    public Transform worldPoint_1;
    public Transform worldPoint_2;

    [Header("Player")]
    public MinimapIcon playerMinimapIcon;

    [Header("Buildings")]
    public Builder builder;
    public Color buildingIconColor;

    [Header("Enemies")]
    public SpawnManager spawnManager;
    public Color enemyIconColor;

    public float minimapRatioX { get; private set; }
    public float minimapRatioY { get; private set; }


    private List<MinimapIcon> buildingIcons;
    private List<MinimapIcon> enemyIcons;
    private int nextIconID = 0;

    private void Awake()
    {
        CalculateMapRatio();
    }

    public void Start() {
        buildingIcons = new List<MinimapIcon>();
        enemyIcons = new List<MinimapIcon>();
    }


    public void Update()
    {
        playerMinimapIcon.UpdatePosition();

        foreach (MinimapIcon b in buildingIcons) {
            if (b != null) b.UpdatePosition();
        }

        foreach (MinimapIcon e in enemyIcons) {
            if (e != null) e.UpdatePosition();
        }
    }

    private void UpdateBuildingIcons() {

    }

    public void CreateMinimapIcon(Character c, bool isBuilding) {
        GameObject newIcon = new GameObject("MinimapIcon_" + nextIconID);
        newIcon.transform.SetParent(this.transform);
        MinimapIcon nIComp = newIcon.AddComponent<MinimapIcon>();
        nIComp.worldObject = c.transform;
        nIComp.icon = newIcon.AddComponent<RectTransform>();
        nIComp.minimap = this;
        nIComp.image = newIcon.AddComponent<Image>();
        nIComp.icon.localScale = new Vector3(1, 1, 1);
        nIComp.icon.sizeDelta = new Vector2(10, 10);
        nIComp.icon.anchorMin = new Vector2(0, 1);
        nIComp.icon.anchorMax = new Vector2(0, 1);

        if (isBuilding) {
            nIComp.image.color = buildingIconColor;
            buildingIcons.Add(nIComp);
        }    
        else {
            nIComp.image.color = enemyIconColor;
            enemyIcons.Add(nIComp);
        }

        c.minimapIcon = nIComp;
    }

    public void DeleteMinimapIcon(MinimapIcon mi) {
        for (int i = 0; i < enemyIcons.Count; i++) {
            if (enemyIcons[i] == mi) {
                enemyIcons.RemoveAt(i);
                Destroy(mi.gameObject);
                return;
            }
        }

        for (int i = 0; i < buildingIcons.Count; i++) {
            if (buildingIcons[i] == mi) {
                buildingIcons.RemoveAt(i);
                Destroy(mi.gameObject);
                return;
            }
        }
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