using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Object[] buildingTypes;
    private Object[] resourceTypes;

    public InputManager inputManager;

    public GameObject buildPanel;
    public GameObject upgradePanel;
    public GameObject eventPanel;
    public GameObject topLevelHUD;

    public Sprite emptyTileSprite;

    private Button upgradeButton;
    private Button destroyButton;

    private Transform resourceBar;
    private Image clock;
    public static bool clockAnimationFinished;

    void Start()
    {
        resourceBar = topLevelHUD.transform.GetChild(0);
        clock = topLevelHUD.transform.GetChild(1).gameObject.GetComponent<Image>();

        Transform buildingListContent = buildPanel.transform.GetChild(1).GetChild(0).GetChild(0);

        upgradeButton = upgradePanel.transform.GetChild(1).gameObject.GetComponent<Button>();
        destroyButton = upgradePanel.transform.GetChild(2).gameObject.GetComponent<Button>();

        destroyButton.onClick.AddListener(() => DestroyBuilding());
        upgradeButton.onClick.AddListener(() => UpgradeBuilding());

        buildingTypes = Resources.LoadAll("Data/Buildings", typeof(Building));

        RectTransform rectTransform = buildingListContent.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 550 - (Mathf.Ceil(buildingTypes.Length / 2f) * 500f));

        foreach (Building buildingtype in buildingTypes)
        {
            GameObject building = (GameObject)Instantiate(Resources.Load("BuildingUIButtonItem"), buildingListContent);
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
        BuildingController buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
        if (buildingController.buildable)
        {
            buildingController.buildable = false;
            buildingController.building = building;
            buildingController.productionPerTick = building.resourceProducedPerTick;
            buildingController.returnedMaterialsOnDestroy += (int)Mathf.Floor(building.buildingCost / 2f);
            InputManager.selectedTile.GetComponent<SpriteRenderer>().sprite = building.buildingSprite;

            if (building.producedResource != AResource.Resource.Seconds && building.producedResource != AResource.Resource.None)
            {
                GameManager.resourceGrowth[building.producedResource.ToString()] += buildingController.productionPerTick;
            }

            UpdateHUD();
            buildPanel.SetActive(false);
            UpdateUpgradeMenu(buildingController);
        }
    }

    public void DestroyBuilding()
    {
        BuildingController buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
        Building building = buildingController.building;
        //materials += building.buildCost;
        InputManager.selectedTile.GetComponent<SpriteRenderer>().sprite = emptyTileSprite;
        buildingController.building = null;
        buildingController.buildable = true;
        buildingController.level = 0;

        if (building.producedResource != AResource.Resource.Seconds && building.producedResource != AResource.Resource.None)
        {
            GameManager.resourceGrowth[building.producedResource.ToString()] -= buildingController.productionPerTick;
        }

        UpdateHUD();
        upgradePanel.SetActive(false);
        inputManager.Deselect();
    }

    public void UpgradeBuilding()
    {
        BuildingController buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
        Building building = buildingController.building;
        buildingController.productionPerTick += building.upgrades[buildingController.level].productionBoost;
        buildingController.returnedMaterialsOnDestroy += (int)Mathf.Floor(building.upgrades[buildingController.level].materialCost / 2f);

        if (building.producedResource != AResource.Resource.Seconds && building.producedResource != AResource.Resource.None)
        {
            GameManager.resourceGrowth[building.producedResource.ToString()] += building.upgrades[buildingController.level].productionBoost;
        }

        buildingController.level++;
        UpdateUpgradeMenu(buildingController);
        UpdateHUD();
    }

    public void ActivateBuildMenu(RaycastHit2D hit)
    {
        upgradePanel.SetActive(false);
        buildPanel.SetActive(false);
        GameObject tile = hit.collider.gameObject;
        BuildingController buildingController = tile.GetComponent<BuildingController>();
        if (buildingController.unlockedForBuilding && InputManager.selectedTile != tile)
        {
            // Activate build menu if nothing is built yet
            if (buildingController.building == null)
            {
                buildPanel.SetActive(true);
            }
            else
            {
                UpdateUpgradeMenu(buildingController);
            }

            inputManager.Select(hit);
        }
        else
        {
            inputManager.Deselect();
        }
    }

    void UpdateUpgradeMenu(BuildingController buildingController)
    {
        GameObject upgradeTextPanel = upgradePanel.transform.GetChild(0).gameObject;
        TextMeshProUGUI buildingName = upgradeTextPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI buildingLevel = upgradeTextPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI productionInfo = upgradeTextPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI upgradeInfo = upgradeTextPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();

        buildingName.text = buildingController.building.name;
        if (buildingController.building.producedResource != AResource.Resource.None)
        {
            productionInfo.text = buildingController.productionPerTick.ToString() + " " + buildingController.building.producedResource + "/tick";
        }
        else
        {
            productionInfo.text = "";
        }

        if (buildingController.level >= buildingController.building.upgrades.Length)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }

        if (buildingController.building.upgrades.Length > 0 && buildingController.level < buildingController.building.upgrades.Length)
        {
            buildingLevel.text = "Level " + (buildingController.level + 1).ToString();
            upgradeInfo.text = "Next upgrade: " + buildingController.building.upgrades[buildingController.level].description + "\nCost: " + buildingController.building.upgrades[buildingController.level].materialCost + " materials";
        }
        else
        {
            buildingLevel.text = "Max Level";
            upgradeInfo.text = buildingController.building.buildingInfo;
        }

        upgradePanel.SetActive(true);
    }

    public void DisplayEvent(Event currentevent)
    {
        TextMeshProUGUI eventTextPanel = eventPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        eventTextPanel.text = currentevent.description;


        upgradePanel.SetActive(false);
        buildPanel.SetActive(false);
        eventPanel.SetActive(true);
    }

    public void UpdateHUD()
    {
        string[] resources = new string[] { "Food", "Materials", "Luxuries", "Population" };

        for (int i = 0; i < 4; i++)
        {
            TextMeshProUGUI resourceText = topLevelHUD.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI growthText = topLevelHUD.transform.GetChild(0).GetChild(i).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

            resourceText.text = GameManager.resources[resources[i]].ToString();
            growthText.text = "+" + GameManager.resourceGrowth[resources[i]].ToString();
        }
    }

    public void AddResourceIndicators()
    {
        resourceTypes = Resources.LoadAll("Data/Resources", typeof(AResource));
        foreach (AResource resourceType in resourceTypes)
        {
            GameObject resourceIndicator = (GameObject)Instantiate(Resources.Load("ResourceIndicator"), resourceBar);
            TextMeshProUGUI resourceAmount = resourceIndicator.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            Image resourceIcon = resourceIndicator.transform.GetChild(1).gameObject.GetComponent<Image>();
            //TextMeshProUGUI resourceChange = resourceIndicator.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

            GameManager.resources[resourceType.name] = resourceType.startAmount;

            resourceAmount.text = GameManager.resources[resourceType.name].ToString();
            resourceIcon.sprite = resourceType.resourceIcon;
        }
    }

    public IEnumerator SetClock(float target, float increment)
    {
        float _target = Mathf.Clamp(target + increment, 0, 1);
        float _increment = increment;
        while (clock.fillAmount < _target)
        {
            clock.fillAmount += _increment;
            _increment += increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        while (clock.fillAmount > target)
        {
            clock.fillAmount -= _increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        clockAnimationFinished = true;
    }

    public IEnumerator ResetClock()
    {
        while (clock.fillAmount > 0)
        {
            clock.fillAmount -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

    }
}