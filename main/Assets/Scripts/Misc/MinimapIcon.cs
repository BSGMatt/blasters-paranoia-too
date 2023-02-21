using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public RectTransform icon;
    public Transform worldObject; //The object in world space the icon represents. 
    public Minimap minimap; //The minimap this icon is a part of. 
    public Image image;

    public Color imageColor;

    // Start is called before the first frame update
    void Start()
    {
        image.color = imageColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePosition() {

        if (worldObject == null) return;

        icon.anchoredPosition = minimap.minimapPoint_1.anchoredPosition + 
            new Vector2((worldObject.position.x - minimap.worldPoint_1.position.x) * minimap.minimapRatioX,
            (worldObject.position.y - minimap.worldPoint_1.position.y) * minimap.minimapRatioY);

        ///
        /// Make the icon transparent if it is outside of the minimap's bounds. 
        ///
        if (BSGUtility.WithinBoundsOf(icon.anchoredPosition,
            minimap.minimapPoint_1.anchoredPosition.x, minimap.minimapPoint_2.anchoredPosition.x,
            minimap.minimapPoint_2.anchoredPosition.y, minimap.minimapPoint_1.anchoredPosition.y)) {

            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1);
        }
        else {
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        }
    }

}
