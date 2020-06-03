using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Object[] buildingtypes;
    public Transform content;

    void Start()
    {
        buildingtypes = Resources.LoadAll("Data/Buildings", typeof(Building));

        RectTransform rectTransform = content.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 550 - (Mathf.Ceil(buildingtypes.Length / 2f) * 500f));

        foreach (Building buildingtype in buildingtypes)
        {
            GameObject building = (GameObject)Instantiate(Resources.Load("BuildingUIButtonItem"), content);
            GameObject buildingButton = building.transform.GetChild(0).gameObject;
            GameObject buildingName = building.transform.GetChild(1).gameObject;
            GameObject buildingInfo = building.transform.GetChild(2).gameObject;

            if (buildingtype.buildingSprite != null)
            {
                buildingButton.GetComponent<Image>().sprite = buildingtype.buildingSprite;
            }
            if (buildingtype.name != null)
            {
                buildingName.GetComponent<TextMeshProUGUI>().text = buildingtype.name;
            }
            if (buildingtype.buildingInfo != null)
            {
                buildingInfo.GetComponent<TextMeshProUGUI>().text = buildingtype.buildingInfo;
            }

            buildingButton.GetComponent<Button>().onClick.AddListener(() => BuildBuilding(buildingtype));
        }
    }

    public void BuildBuilding(Building building)
    {
        if (InputManager.selectedTile.GetComponent<BuildingController>().buildable)
        {
            InputManager.selectedTile.GetComponent<SpriteRenderer>().sprite = building.buildingSprite;
        }
    }
}